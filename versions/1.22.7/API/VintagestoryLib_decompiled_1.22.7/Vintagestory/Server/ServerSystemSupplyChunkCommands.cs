using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ProtoBuf;
using SkiaSharp;
using Vintagestory.API.Common;
using Vintagestory.API.Common.CommandAbbr;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Common.Database;

namespace Vintagestory.Server;

internal class ServerSystemSupplyChunkCommands : ServerSystem
{
	private string backupFileName;

	private ChunkServerThread chunkthread;

	public ServerSystemSupplyChunkCommands(ServerMain server, ChunkServerThread chunkthread)
		: base(server)
	{
		this.chunkthread = chunkthread;
		server.api.ChatCommands.GetOrCreate("chunk").BeginSub("cit").WithDescription("Chunk information from the supply chunks thread")
			.WithArgs(server.api.ChatCommands.Parsers.OptionalWord("perf"))
			.HandleWith(OnChunkInfoCmd)
			.EndSub()
			.BeginSub("printmap")
			.WithDescription("Export a png file of a map of loaded chunks. Marks call location with a yellow pixel")
			.HandleWith(OnChunkMap)
			.EndSub();
	}

	private TextCommandResult OnChunkMap(TextCommandCallingArgs args)
	{
		string filename = PrintServerChunkMap(new Vec2i(args.Caller.Pos.XInt / 32, args.Caller.Pos.ZInt / 32));
		return TextCommandResult.Success("map " + filename + " generated");
	}

	public override void OnBeginModsAndConfigReady()
	{
		base.OnBeginModsAndConfigReady();
		CommandArgumentParsers parsers = server.api.ChatCommands.Parsers;
		IChatCommand dbcmd = server.api.ChatCommands.GetOrCreate("db").RequiresPrivilege(Privilege.controlserver).WithDesc("Save-game related commands");
		if (!server.Config.HostedMode)
		{
			dbcmd.BeginSub("backup").WithDesc("Creates a copy of the current save game in the Backups folder").WithArgs(parsers.OptionalWord("filename"))
				.HandleWith(onCmdGenBackup)
				.WithRootAlias("genbackup")
				.EndSub()
				.BeginSub("vacuum")
				.WithDesc("Repack save game to minimize its file size")
				.HandleWith(onCmdVacuum)
				.EndSub()
				.BeginSub("backuppartial")
				.WithDesc("Create a copy of the main data and extract some chunks. Useful for extracting a working savegame with a smaller savegame size for debugging. This will take the data from disk so make sure to save first using /autosave.")
				.WithArgs(parsers.Int("chunk_x_start"), parsers.Int("chunk_z_start"), parsers.Int("chunk_x_end"), parsers.Int("chunk_z_end"), parsers.OptionalWord("filename"))
				.HandleWith(onBackupPartial)
				.EndSub();
		}
		else
		{
			dbcmd.WithAdditionalInformation("(/db backup and /db vacuum sub-commands are not available for hosted servers, sorry)");
		}
		dbcmd.BeginSub("prune").WithDesc("Delete all unchanged or hardly changed chunks, with changes below a specified threshold. Chunks with land claims can be protected.").WithAdditionalInformation("'Changes' refers to edits by players, counted separately in each 32x32 chunk in the world. The number of edits is the count of blocks of any kind placed or broken by any player in either Survival or Creative modes. Breaking grass or leaves is counted, harvesting berries or collecting sticks is not counted. Only player actions since game version 1.18.0 (April 2023) are counted. Chunks with land claims of any size, even a single block, can be protected using the 'keep' option. The 'keep' option will preserve all trader caravans and the Resonance Archives.\n\nPruned chunks are fully deleted and destroyed and, when next visited, will be regenerated with up-to-date worldgen from the current game version, including new vegetation and ruins. Bodies of water, general terrain shape and climate conditions will be unchanged or almost unchanged. Ore presence in each chunk will be similar as before, may be in slightly different positions.\n\nWithout the 'confirm' arg, does a dry-run only! If mods or worldconfig have changed since the world was first created, or if the map was first created in game version 1.17 or earlier, results of a prune may be unpredictable or chunk borders may become visible, a backup first is advisable. This command is irreversible, use with care!")
			.WithArgs(parsers.Int("threshold").WithExplanation("is the number of changes, at this level of changes or more the chunks are kept"), parsers.WordRange("keep or drop", "keep", "drop").WithExplanation("is the choice whether to protect (keep) all chunks which have land claims"), parsers.OptionalWordRange("confirm flag", "confirm", "dryrun"), parsers.OptionalWord("version").WithExplanation("is (optionally) the lowest version to remove. All worldgen created earlier than this game version will be preserved, even if chunks had no player edits and no land claims."))
			.HandleWith(onCmdPrune)
			.EndSub()
			.Validate();
	}

	private TextCommandResult onCmdVacuum(TextCommandCallingArgs args)
	{
		IServerPlayer logToPlayer = args.Caller.Player as IServerPlayer;
		processInBackground(chunkthread.gameDatabase.Vacuum, delegate
		{
			notifyIndirect(logToPlayer, Lang.Get("Vacuum complete!"));
		});
		return TextCommandResult.Success(Lang.Get("Vacuum started, this may take some time"));
	}

	private TextCommandResult onCmdPrune(TextCommandCallingArgs args)
	{
		IServerPlayer logToPlayer = args.Caller.Player as IServerPlayer;
		int threshold = (int)args[0];
		bool keepClaims = (string)args[1] == "keep";
		bool dryRun = (string)args[2] != "confirm";
		string aboveVersion = (args.Parsers[3].IsMissing ? null : ((string)args[3]));
		return prune(logToPlayer, threshold, dryRun, keepClaims, aboveVersion);
	}

	private TextCommandResult prune(IServerPlayer logToPlayer, int threshold, bool dryRun, bool keepClaims, string aboveVersion)
	{
		int qBelowThreshold = 0;
		HashSet<FastVec2i> toDelete = new HashSet<FastVec2i>();
		HashSet<FastVec2i> toKeep = new HashSet<FastVec2i>();
		List<LandClaim> claims = server.WorldMap.All;
		HorRectanglei rect = new HorRectanglei();
		int chunksize = server.api.worldapi.ChunkSize;
		processInBackground(delegate
		{
			chunkthread.gameDatabase.ForAllChunks(delegate(DbChunk schunk)
			{
				try
				{
					if (keepClaims)
					{
						bool flag = false;
						rect.X1 = schunk.Position.X * chunksize;
						rect.Z1 = schunk.Position.Z * chunksize;
						rect.X2 = schunk.Position.X * chunksize + chunksize;
						rect.Z2 = schunk.Position.Z * chunksize + chunksize;
						foreach (LandClaim current2 in claims)
						{
							if (current2 != null && current2.Intersects2d(rect))
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							toKeep.Add(new FastVec2i(schunk.Position.X, schunk.Position.Z));
							return;
						}
					}
					bool flag2 = false;
					if (threshold > 0 || aboveVersion != null)
					{
						ServerChunk serverChunk;
						using (MemoryStream source = new MemoryStream(schunk.Data))
						{
							serverChunk = Serializer.Deserialize<ServerChunk>(source);
						}
						flag2 = serverChunk.BlocksRemoved + serverChunk.BlocksPlaced >= threshold && threshold > 0;
						if (aboveVersion != null && GameVersion.IsLowerVersionThan(serverChunk.GameVersionCreated, aboveVersion))
						{
							flag2 = true;
						}
					}
					if (flag2)
					{
						toKeep.Add(new FastVec2i(schunk.Position.X, schunk.Position.Z));
					}
					else
					{
						qBelowThreshold++;
						toDelete.Add(new FastVec2i(schunk.Position.X, schunk.Position.Z));
					}
				}
				catch (Exception)
				{
					throw;
				}
			});
			foreach (FastVec2i current in toKeep)
			{
				toDelete.Remove(current);
			}
			server.EnqueueMainThreadTask(delegate
			{
				if (dryRun)
				{
					notifyIndirect(logToPlayer, Lang.Get("Dry run prune complete. With a {0} block edits threshold, {1} chunk columns can be removed, {2} chunk columns would be kept.", threshold, toDelete.Count, toKeep.Count));
				}
				else
				{
					server.api.Server.PauseThread("chunkdbthread");
					int num = server.api.worldapi.RegionSize / chunksize;
					Cuboidi cuboidi = new Cuboidi();
					Dictionary<long, ServerMapRegion> dictionary = new Dictionary<long, ServerMapRegion>(10);
					Queue<long> queue = new Queue<long>(10);
					FastMemoryStream ms = new FastMemoryStream();
					foreach (FastVec2i current2 in toDelete)
					{
						int num2 = current2.X / num;
						int num3 = current2.Y / num;
						ServerMapRegion value = server.WorldMap.GetMapRegion(num2, num3) as ServerMapRegion;
						long num4 = server.WorldMap.MapRegionIndex2D(num2, num3);
						if (value == null && !dictionary.TryGetValue(num4, out value))
						{
							byte[] mapRegion = chunkthread.gameDatabase.GetMapRegion(num2, num3);
							if (mapRegion != null)
							{
								if (queue.Count >= 9)
								{
									long num5 = queue.Dequeue();
									ServerMapRegion serverMapRegion = dictionary[num5];
									DbChunk item = new DbChunk
									{
										Position = server.WorldMap.MapRegionPosFromIndex2D(num5),
										Data = serverMapRegion.ToBytes(ms)
									};
									chunkthread.gameDatabase.SetMapRegions(new List<DbChunk> { item });
									dictionary.Remove(num5);
								}
								value = (dictionary[num4] = ServerMapRegion.FromBytes(mapRegion));
								queue.Enqueue(num4);
							}
						}
						List<GeneratedStructure> list = new List<GeneratedStructure>();
						cuboidi.X1 = current2.X * chunksize;
						cuboidi.Z1 = current2.Y * chunksize;
						cuboidi.X2 = current2.X * chunksize + chunksize;
						cuboidi.Z2 = current2.Y * chunksize + chunksize;
						if (value?.GeneratedStructures != null)
						{
							foreach (GeneratedStructure current3 in value.GeneratedStructures)
							{
								if (cuboidi.Contains(current3.Location.Start.X, current3.Location.Start.Z))
								{
									list.Add(current3);
								}
							}
							foreach (GeneratedStructure current4 in list)
							{
								value.GeneratedStructures.Remove(current4);
							}
						}
						server.api.WorldManager.DeleteChunkColumn(current2.X, current2.Y);
					}
					chunkthread.gameDatabase.SetMapRegions(dictionary.Select((KeyValuePair<long, ServerMapRegion> r) => new DbChunk
					{
						Position = server.WorldMap.MapRegionPosFromIndex2D(r.Key),
						Data = r.Value.ToBytes(ms)
					}));
					server.api.Server.ResumeThread("chunkdbthread");
					notifyIndirect(logToPlayer, Lang.Get("Prune complete, {1} chunk columns were removed, {2} chunk columns were kept.", threshold, toDelete.Count, toKeep.Count));
				}
			});
		}, null);
		return TextCommandResult.Success(dryRun ? Lang.Get("Dry run prune started, this may take some time.") : Lang.Get("Prune started, this may take some time."));
	}

	private TextCommandResult onCmdGenBackup(TextCommandCallingArgs args)
	{
		if (server.Config.HostedMode)
		{
			return TextCommandResult.Error(Lang.Get("Can't access this feature, server is in hosted mode"));
		}
		backupFileName = (args.Parsers[0].IsMissing ? null : Path.GetFileName(args[0] as string));
		GenBackup(args.Caller.Player as IServerPlayer);
		return TextCommandResult.Success(Lang.Get("Ok, generating backup, this might take a while"));
	}

	private void GenBackup(IServerPlayer logToPlayer = null)
	{
		if (chunkthread.BackupInProgress)
		{
			notifyIndirect(logToPlayer, Lang.Get("Can't run backup. A backup is already in progress"));
			return;
		}
		chunkthread.BackupInProgress = true;
		if (backupFileName == null || backupFileName.Length == 0 || backupFileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
		{
			string filename = Path.GetFileName(server.Config.WorldConfig.SaveFileLocation).Replace(".vcdbs", "");
			if (filename.Length == 0)
			{
				filename = "world";
			}
			backupFileName = filename + "-" + $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}" + ".vcdbs";
		}
		processInBackground(delegate
		{
			chunkthread.gameDatabase.CreateBackup(backupFileName);
		}, delegate
		{
			chunkthread.BackupInProgress = false;
			string msg = Lang.Get("Backup complete!");
			notifyIndirect(logToPlayer, msg);
		});
	}

	private TextCommandResult onBackupPartial(TextCommandCallingArgs args)
	{
		if (chunkthread.BackupInProgress)
		{
			return TextCommandResult.Success("Can't run backup partial. A backup is already in progress");
		}
		chunkthread.BackupInProgress = true;
		Vec2i start = new Vec2i();
		Vec2i end = new Vec2i();
		start.X = Math.Min((int)args[0], (int)args[2]);
		start.Y = Math.Min((int)args[1], (int)args[3]);
		end.X = Math.Max((int)args[0], (int)args[2]);
		end.Y = Math.Max((int)args[1], (int)args[3]);
		string fileNameBackup = (args.Parsers[4].IsMissing ? null : Path.GetFileName(args[4] as string));
		if (fileNameBackup == null)
		{
			string filename = Path.GetFileName(server.Config.WorldConfig.SaveFileLocation).Replace(".vcdbs", "");
			if (filename.Length == 0)
			{
				filename = "world";
			}
			fileNameBackup = $"{filename}-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}{".vcdbs"}";
		}
		IServerPlayer player = args.Caller.Player as IServerPlayer;
		processInBackground(delegate
		{
			chunkthread.gameDatabase.BackupPartial(fileNameBackup, start, end, server.MapSize.Y / 32);
		}, delegate
		{
			chunkthread.BackupInProgress = false;
			notifyIndirect(player, Lang.Get("BackupPartial: completed") + " file is at Backups/" + fileNameBackup);
		});
		return TextCommandResult.Success($"BackupPartial: started from chunk {start} to chunk {end}");
	}

	private void processInBackground(Action backgroundProc, Action onDoneOnMainthread)
	{
		TyronThreadPool.QueueTask(delegate
		{
			backgroundProc();
			server.EnqueueMainThreadTask(delegate
			{
				onDoneOnMainthread?.Invoke();
			});
		}, "supplychunkcommand");
	}

	private void notifyIndirect(IServerPlayer logToPlayer, string msg)
	{
		if (logToPlayer != null)
		{
			logToPlayer.SendMessage(server.IsDedicatedServer ? GlobalConstants.ServerInfoChatGroup : GlobalConstants.GeneralChatGroup, msg, EnumChatType.CommandSuccess, "backupdone");
		}
		else
		{
			ServerMain.Logger.Notification(msg);
		}
	}

	private TextCommandResult OnChunkInfoCmd(TextCommandCallingArgs args)
	{
		if ((string)args[0] == "perf")
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < 20; i++)
			{
				sb.AppendLine(ServerSystemSendChunks.performanceTest(server));
			}
			return TextCommandResult.Success(sb.ToString());
		}
		BlockPos asBlockPos = args.Caller.Pos.AsBlockPos;
		int chunkX = asBlockPos.X / 32;
		int chunkY = asBlockPos.Y / 32;
		int chunkZ = asBlockPos.Z / 32;
		long index2d = server.WorldMap.MapChunkIndex2D(chunkX, chunkZ);
		ServerChunk chunk = chunkthread.GetGeneratingChunk(chunkX, chunkY, chunkZ);
		ChunkColumnLoadRequest chunkReq = chunkthread.requestedChunkColumns.GetByIndex(index2d);
		if (chunkReq != null)
		{
			global::_003C_003Ey__InlineArray4<object> buffer = default(global::_003C_003Ey__InlineArray4<object>);
			buffer[0] = chunk != null;
			buffer[1] = chunkReq != null;
			buffer[2] = chunkReq.CurrentIncompletePass;
			buffer[3] = chunkReq.GenerateUntilPass;
			return TextCommandResult.Success(string.Format("Chunk in genQ: {0}, chunkReq in Q: {1}, currentPass: {2}, untilPass: {3}", (ReadOnlySpan<object?>)buffer));
		}
		return TextCommandResult.Success($"Chunk in genQ: {chunk != null}, chunkReq in Q: {chunkReq != null}");
	}

	public string PrintServerChunkMap(Vec2i markChunkPos = null)
	{
		ChunkPos minPos = new ChunkPos(int.MaxValue, 0, int.MaxValue, 0);
		ChunkPos maxPos = new ChunkPos(0, 0, 0, 0);
		server.loadedChunksLock.AcquireReadLock();
		try
		{
			foreach (long index3d in server.loadedChunks.Keys)
			{
				ChunkPos vec = server.WorldMap.ChunkPosFromChunkIndex3D(index3d);
				if (vec.Dimension <= 0)
				{
					minPos.X = Math.Min(minPos.X, vec.X);
					minPos.Z = Math.Min(minPos.Z, vec.Z);
					maxPos.X = Math.Max(maxPos.X, vec.X);
					maxPos.Z = Math.Max(maxPos.Z, vec.Z);
				}
			}
		}
		finally
		{
			server.loadedChunksLock.ReleaseReadLock();
		}
		if (minPos.X == int.MaxValue)
		{
			return "";
		}
		int num = maxPos.X - minPos.X;
		int sizeZ = maxPos.Z - minPos.Z;
		SKBitmap bmp = new SKBitmap(num + 1, sizeZ + 1);
		server.loadedChunksLock.AcquireReadLock();
		try
		{
			foreach (long index3d2 in server.loadedChunks.Keys)
			{
				ChunkPos vec2 = server.WorldMap.ChunkPosFromChunkIndex3D(index3d2);
				if (vec2.Dimension <= 0)
				{
					bmp.SetPixel(vec2.X - minPos.X, vec2.Z - minPos.Z, new SKColor(0, byte.MaxValue, 0, byte.MaxValue));
				}
			}
		}
		finally
		{
			server.loadedChunksLock.ReleaseReadLock();
		}
		foreach (ChunkColumnLoadRequest req in chunkthread.requestedChunkColumns.Snapshot())
		{
			if (req != null && !req.Disposed)
			{
				if (req.Chunks == null)
				{
					bmp.SetPixel(req.chunkX, req.chunkZ, new SKColor(20, 20, 20, byte.MaxValue));
					continue;
				}
				int currentpass = req.CurrentIncompletePass_AsInt;
				SKColor c = new SKColor((byte)(5 + bmp.GetPixel(req.chunkX, req.chunkZ).Red), (byte)(currentpass * 30), (byte)(currentpass * 30), byte.MaxValue);
				bmp.SetPixel(req.chunkX - minPos.X, req.chunkZ - minPos.Z, c);
			}
		}
		int i = 0;
		while (File.Exists("serverchunks" + i + ".png"))
		{
			i++;
		}
		if (markChunkPos != null)
		{
			bmp.SetPixel(markChunkPos.X - minPos.X, markChunkPos.Y - minPos.Z, new SKColor(byte.MaxValue, 20, byte.MaxValue, byte.MaxValue));
		}
		bmp.Save("serverchunks" + i + ".png");
		return "serverchunks" + i + ".png";
	}
}
