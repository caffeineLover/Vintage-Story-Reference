using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using BetterErProspecting.Extensions;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.CommandAbbr;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace BetterErProspecting.Tracking
{
	// Token: 0x02000006 RID: 6
	[NullableContext(1)]
	[Nullable(0)]
	public class PptTracker : ModSystem
	{
		// Token: 0x06000019 RID: 25 RVA: 0x00002480 File Offset: 0x00000680
		private void ClearOldWaypoints(float dt)
		{
			int currentMax = PptTracker.hashCounter;
			if (currentMax >= PptTracker.lastClearedIndex)
			{
				for (int i = PptTracker.lastClearedIndex; i < currentMax; i++)
				{
					PptTracker.BoreholeData boreholeData;
					PptTracker.hashToWaypointString.TryRemove(i, ref boreholeData);
				}
			}
			else
			{
				PptTracker.BoreholeData boreholeData;
				for (int j = PptTracker.lastClearedIndex; j < 100000; j++)
				{
					PptTracker.hashToWaypointString.TryRemove(j, ref boreholeData);
				}
				for (int k = 0; k < currentMax; k++)
				{
					PptTracker.hashToWaypointString.TryRemove(k, ref boreholeData);
				}
			}
			PptTracker.lastClearedIndex = currentMax;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002504 File Offset: 0x00000704
		public override void StartServerSide(ICoreServerAPI api)
		{
			base.StartServerSide(api);
			this.sapi = api;
			IChatCommandExt.EndSub(IChatCommandExt.WithDesc(IChatCommandExt.BeginSub(this.sapi.ChatCommands.GetOrCreate("btrpr").RequiresPrivilege(Privilege.controlserver), "oreData").RequiresPlayer(), "Dumps all ores data from memory and file storage and rewrites from all existing ore readings. Best to reprospect first. May cause lags.").WithExamples(new string[]
			{
				"/btrpr oreData"
			}).HandleWith(new OnCommandDelegate(this.DumpAndReload)));
			this.serverChannel = api.Network.RegisterChannel("bettererprospecting").RegisterMessageType<CreateBoreholeWpPacket>().RegisterMessageType<PptDataPacket>().SetMessageHandler<CreateBoreholeWpPacket>(new NetworkClientMessageHandler<CreateBoreholeWpPacket>(this.OnServerRecieveBoreholeWpPacket));
			api.Event.RegisterGameTickListener(new Action<float>(this.ClearOldWaypoints), (int)TimeSpan.FromHours(1).TotalMilliseconds, 0);
			api.Event.PlayerJoin += new PlayerDelegate(this.OnPlayerJoin);
			api.Event.SaveGameLoaded += new Action(this.OnSaveGameLoaded);
			api.Event.GameWorldSave += new Action(this.OnSaveGameGettingSaved);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002620 File Offset: 0x00000820
		public override void StartClientSide(ICoreClientAPI api)
		{
			base.StartClientSide(api);
			this.capi = api;
			this.capi.RegisterLinkProtocol("btrprwayp", new Action<LinkTextComponent>(this.onCreateWaypointClicked));
			this.clientChannel = api.Network.RegisterChannel("bettererprospecting").RegisterMessageType<CreateBoreholeWpPacket>().RegisterMessageType<PptDataPacket>().SetMessageHandler<PptDataPacket>(new NetworkServerMessageHandler<PptDataPacket>(this.OnClientReceivePacket));
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002688 File Offset: 0x00000888
		public static string trackPlayerBoreholeWp(IServerPlayer serverPlayer, string linkedNames)
		{
			if (PptTracker.hashCounter >= 100000)
			{
				PptTracker.hashCounter = 0;
			}
			PptTracker.hashToWaypointString[PptTracker.hashCounter] = new PptTracker.BoreholeData(linkedNames, serverPlayer.Entity.Pos.XYZ);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(27, 2);
			defaultInterpolatedStringHandler.AppendLiteral("<a href=\"btrprwayp://");
			defaultInterpolatedStringHandler.AppendFormatted<int>(PptTracker.hashCounter);
			defaultInterpolatedStringHandler.AppendLiteral("\">");
			defaultInterpolatedStringHandler.AppendFormatted(serverPlayer.L("borehole-waypoint-create", Array.Empty<object>()));
			defaultInterpolatedStringHandler.AppendLiteral("</a>");
			string result = defaultInterpolatedStringHandler.ToStringAndClear();
			PptTracker.hashCounter++;
			return result;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002730 File Offset: 0x00000930
		private void OnServerRecieveBoreholeWpPacket(IServerPlayer sP, CreateBoreholeWpPacket packet)
		{
			PptTracker.BoreholeData bd;
			if (PptTracker.hashToWaypointString.TryGetValue(packet.hash, ref bd))
			{
				Random rand = this.sapi.World.Rand;
				int color = ColorUtil.HsvToRgba(rand.Next(256), rand.Next(180, 256), rand.Next(200, 256), 255);
				Waypoint waypoint = new Waypoint
				{
					Color = color,
					OwningPlayerUid = sP.PlayerUID,
					Position = bd.position,
					Title = "Borehole results:\n" + bd.text,
					Icon = "propick"
				};
				WaypointMapLayer waypointMapLayer = Enumerable.FirstOrDefault<MapLayer>(this.sapi.ModLoader.GetModSystem<WorldMapManager>(true).MapLayers, (MapLayer ml) => ml is WaypointMapLayer) as WaypointMapLayer;
				if (waypointMapLayer != null)
				{
					waypointMapLayer.AddWaypoint(waypoint, sP);
				}
				sP.Info("borehole-waypoint-created", Array.Empty<object>());
				return;
			}
			sP.Info("borehole-waypoint-no-data", Array.Empty<object>());
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002850 File Offset: 0x00000A50
		private void onCreateWaypointClicked(LinkTextComponent comp)
		{
			string target = comp.Href.Substring("btrprwayp://".Length);
			this.clientChannel.SendPacket<CreateBoreholeWpPacket>(new CreateBoreholeWpPacket
			{
				hash = int.Parse(target)
			});
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002890 File Offset: 0x00000A90
		private void OnSaveGameLoaded()
		{
			byte[] pptData = this.sapi.WorldManager.SaveGame.GetData("betterErProspectingPptData");
			byte[] notifyReprospectData = this.sapi.WorldManager.SaveGame.GetData("betterErProspectingShouldReprospectNotify");
			if (notifyReprospectData != null)
			{
				PptTracker.ShouldReprospectNotify = SerializerUtil.Deserialize<int>(notifyReprospectData);
			}
			else if (pptData != null)
			{
				base.Mod.Logger.Notification("Found no ppt data for save file. Notifying the player to run reprospect");
				this.sapi.WorldManager.SaveGame.StoreData("betterErProspectingShouldReprospectNotify", SerializerUtil.SerializedOne);
				PptTracker.ShouldReprospectNotify = 1;
			}
			if (pptData != null)
			{
				Dictionary<string, PptData> loaded = SerializerUtil.Deserialize<Dictionary<string, PptData>>(pptData);
				if (loaded == null)
				{
					return;
				}
				foreach (KeyValuePair<string, PptData> kvp in loaded)
				{
					PptTracker.oreData[kvp.Key] = kvp.Value;
				}
				ILogger logger = base.Mod.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Loaded ppt data for ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(loaded.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" ore codes from save");
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear());
				this.sapi.Event.ServerRunPhase(8, new Action(this.FillOreDataFromReadings));
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000029E8 File Offset: 0x00000BE8
		private void OnSaveGameGettingSaved()
		{
			this.sapi.WorldManager.SaveGame.StoreData("betterErProspectingShouldReprospectNotify", SerializerUtil.Serialize<int>(PptTracker.ShouldReprospectNotify));
			if (PptTracker.oreData.IsEmpty)
			{
				return;
			}
			using (FastMemoryStream ms = new FastMemoryStream())
			{
				Dictionary<string, PptData> dataToSave = new Dictionary<string, PptData>(PptTracker.oreData);
				this.sapi.WorldManager.SaveGame.StoreData("betterErProspectingPptData", SerializerUtil.Serialize<Dictionary<string, PptData>>(dataToSave, ms));
				ILogger logger = base.Mod.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Saved ppt data for ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(dataToSave.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" ore codes");
				logger.Debug(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002ABC File Offset: 0x00000CBC
		private void OnPlayerJoin(IServerPlayer serverPlayer)
		{
			if (PptTracker.oreData.IsEmpty)
			{
				return;
			}
			PptDataPacket packet = new PptDataPacket(new Dictionary<string, PptData>(PptTracker.oreData));
			IServerNetworkChannel serverNetworkChannel = this.serverChannel;
			if (serverNetworkChannel == null)
			{
				return;
			}
			serverNetworkChannel.SendPacket<PptDataPacket>(packet, new IServerPlayer[]
			{
				serverPlayer
			});
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002B04 File Offset: 0x00000D04
		private void OnClientReceivePacket(PptDataPacket packet)
		{
			if (((packet != null) ? packet.codeToData : null) == null)
			{
				return;
			}
			foreach (KeyValuePair<string, PptData> kvp in packet.codeToData)
			{
				PptTracker.oreData[kvp.Key] = kvp.Value;
			}
			base.Mod.Logger.Debug("Client received ppt data for (" + string.Join(", ", packet.codeToData.Keys) + ") ore codes");
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002BAC File Offset: 0x00000DAC
		public void UpdatePpt([TupleElementNames(new string[]
		{
			"oreCode",
			"ppt"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] List<ValueTuple<string, double>> updatePairs)
		{
			PptDataPacket updatePacket = new PptDataPacket(new Dictionary<string, PptData>());
			foreach (ValueTuple<string, double> pair in updatePairs)
			{
				if (string.IsNullOrEmpty(pair.Item1))
				{
					return;
				}
				PptData data = PptTracker.oreData.GetOrAdd(pair.Item1, (string _) => new PptData());
				data.Update(pair.Item2);
				updatePacket.codeToData[pair.Item1] = data;
			}
			IServerNetworkChannel serverNetworkChannel = this.serverChannel;
			if (serverNetworkChannel == null)
			{
				return;
			}
			serverNetworkChannel.BroadcastPacket<PptDataPacket>(updatePacket, Array.Empty<IServerPlayer>());
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002C74 File Offset: 0x00000E74
		public void AdjustFactor(PropickReading readings)
		{
			foreach (KeyValuePair<string, OreReading> reading in readings.OreReadings)
			{
				reading.Value.DepositCode = reading.Key;
				this.AdjustFactor(reading.Value);
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002CE0 File Offset: 0x00000EE0
		public void AdjustFactor(Dictionary<string, OreReading> OreReadings)
		{
			foreach (KeyValuePair<string, OreReading> reading in OreReadings)
			{
				reading.Value.DepositCode = reading.Key;
				this.AdjustFactor(reading.Value);
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002D48 File Offset: 0x00000F48
		public void AdjustFactor(List<PropickReading> readings)
		{
			foreach (PropickReading reading in readings)
			{
				this.AdjustFactor(reading);
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002D98 File Offset: 0x00000F98
		public void AdjustFactor(OreReading reading)
		{
			reading.TotalFactor = this.GetAdjustedFactor(reading);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002DA8 File Offset: 0x00000FA8
		public double GetAdjustedFactor(OreReading reading)
		{
			if (reading == null || reading.DepositCode == null || reading.DepositCode.StartsWith("rock-"))
			{
				if (reading == null)
				{
					return 0.0;
				}
				return reading.TotalFactor;
			}
			else
			{
				PptData data;
				if (!PptTracker.oreData.TryGetValue(reading.DepositCode, ref data))
				{
					return reading.TotalFactor;
				}
				if (Math.Abs(data.MaxPpt - data.MinPpt) < 0.0001)
				{
					return 1.0;
				}
				double normalizedValue = (reading.PartsPerThousand - data.MinPpt) / (data.MaxPpt - data.MinPpt);
				return Math.Clamp(0.04 + normalizedValue * 0.96, 0.04, 1.0);
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002E70 File Offset: 0x00001070
		private TextCommandResult DumpAndReload(TextCommandCallingArgs args)
		{
			base.Mod.Logger.Notification("[BetterErProspecting] Starting dump and reload of all ore readings data. Initiated by " + args.Caller.Player.PlayerName + "...");
			using (FastMemoryStream ms = new FastMemoryStream())
			{
				Dictionary<string, PptData> emptyDict = new Dictionary<string, PptData>();
				this.sapi.WorldManager.SaveGame.StoreData("betterErProspectingPptData", SerializerUtil.Serialize<Dictionary<string, PptData>>(emptyDict, ms));
			}
			this.FillOreDataFromReadings();
			ILogger logger = base.Mod.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(45, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Dump and reload complete. Tracked ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(PptTracker.oreData.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" ore codes.");
			logger.Notification(defaultInterpolatedStringHandler.ToStringAndClear());
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
			defaultInterpolatedStringHandler2..ctor(65, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("Successfully reloaded ore readings data. Now tracking ");
			defaultInterpolatedStringHandler2.AppendFormatted<int>(PptTracker.oreData.Count);
			defaultInterpolatedStringHandler2.AppendLiteral(" ore codes.");
			return TextCommandResult.Success(defaultInterpolatedStringHandler2.ToStringAndClear(), null);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002F88 File Offset: 0x00001188
		private void FillOreDataFromReadings()
		{
			ProPickWorkSpace ppws = ObjectCacheUtil.TryGet<ProPickWorkSpace>(this.sapi, "propickworkspace");
			if (((ppws != null) ? ppws.pageCodes : null) == null)
			{
				base.Mod.Logger.Error("ProPickWorkSpace not available, couldn't perform backfill");
				return;
			}
			ILogger logger = base.Mod.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(24, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Backfilling ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(ppws.pageCodes.Keys.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" ore codes: ");
			defaultInterpolatedStringHandler.AppendFormatted(string.Join(", ", ppws.pageCodes.Keys));
			logger.Notification(defaultInterpolatedStringHandler.ToStringAndClear());
			List<PropickReading> allReadings = PptTracker.getAllPlayerReadings(this.sapi);
			foreach (PropickReading propickReading in allReadings)
			{
				foreach (KeyValuePair<string, OreReading> oreReading in propickReading.OreReadings)
				{
					PptTracker.oreData.GetOrAdd(oreReading.Key, (string _) => new PptData()).Update(oreReading.Value.PartsPerThousand);
				}
			}
			PptDataPacket updatePacket = new PptDataPacket(new Dictionary<string, PptData>());
			foreach (KeyValuePair<string, PptData> data in PptTracker.oreData)
			{
				updatePacket.codeToData[data.Key] = data.Value;
			}
			IServerNetworkChannel serverNetworkChannel = this.serverChannel;
			if (serverNetworkChannel != null)
			{
				serverNetworkChannel.BroadcastPacket<PptDataPacket>(updatePacket, Array.Empty<IServerPlayer>());
			}
			ILogger logger2 = base.Mod.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
			defaultInterpolatedStringHandler2..ctor(25, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("Backfilled from ");
			defaultInterpolatedStringHandler2.AppendFormatted<int>(allReadings.Count);
			defaultInterpolatedStringHandler2.AppendLiteral(" readings");
			logger2.Debug(defaultInterpolatedStringHandler2.ToStringAndClear());
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000031B8 File Offset: 0x000013B8
		public static List<PropickReading> getAllPlayerReadings(ICoreServerAPI sapi)
		{
			List<PropickReading> result = new List<PropickReading>();
			IModLoader modLoader = sapi.ModLoader;
			object obj;
			if (modLoader == null)
			{
				obj = null;
			}
			else
			{
				WorldMapManager modSystem = modLoader.GetModSystem<WorldMapManager>(true);
				if (modSystem == null)
				{
					obj = null;
				}
				else
				{
					List<MapLayer> mapLayers = modSystem.MapLayers;
					if (mapLayers == null)
					{
						obj = null;
					}
					else
					{
						obj = Enumerable.FirstOrDefault<MapLayer>(mapLayers, (MapLayer ml) => ml is OreMapLayer);
					}
				}
			}
			OreMapLayer oml = obj as OreMapLayer;
			if (oml == null)
			{
				BetterErProspect.Logger.Warning("OreMapLayer not available for backfill");
				return result;
			}
			foreach (string playerUid in sapi.PlayerData.PlayerDataByUid.Keys)
			{
				result.AddRange(PptTracker.getOrLoadReadings(playerUid, oml, sapi));
			}
			return result;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003288 File Offset: 0x00001488
		public static List<PropickReading> getOrLoadReadings(string playeruid, OreMapLayer oml, ICoreServerAPI sapi)
		{
			List<PropickReading> orLoadReadings;
			if (oml.PropickReadingsByPlayer.TryGetValue(playeruid, ref orLoadReadings))
			{
				return orLoadReadings;
			}
			byte[] data = sapi.WorldManager.SaveGame.GetData("oreMapMarkers-" + playeruid);
			if (data == null)
			{
				return oml.PropickReadingsByPlayer[playeruid] = new List<PropickReading>();
			}
			return oml.PropickReadingsByPlayer[playeruid] = SerializerUtil.Deserialize<List<PropickReading>>(data);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000032F0 File Offset: 0x000014F0
		public override void Dispose()
		{
			if (this.sapi != null)
			{
				this.OnSaveGameGettingSaved();
			}
			ICoreClientAPI coreClientAPI = this.capi;
			if ((coreClientAPI != null && !coreClientAPI.IsSinglePlayer) || this.sapi != null)
			{
				PptTracker.oreData.Clear();
			}
			base.Dispose();
		}

		// Token: 0x0400000A RID: 10
		public static readonly ConcurrentDictionary<string, PptData> oreData = new ConcurrentDictionary<string, PptData>();

		// Token: 0x0400000B RID: 11
		private const string SaveKey = "betterErProspectingPptData";

		// Token: 0x0400000C RID: 12
		private const string ChannelName = "bettererprospecting";

		// Token: 0x0400000D RID: 13
		public const string ShouldReprospectNotifyKey = "betterErProspectingShouldReprospectNotify";

		// Token: 0x0400000E RID: 14
		public static int ShouldReprospectNotify;

		// Token: 0x0400000F RID: 15
		private IServerNetworkChannel serverChannel;

		// Token: 0x04000010 RID: 16
		private IClientNetworkChannel clientChannel;

		// Token: 0x04000011 RID: 17
		private ICoreServerAPI sapi;

		// Token: 0x04000012 RID: 18
		private ICoreClientAPI capi;

		// Token: 0x04000013 RID: 19
		public static readonly ConcurrentDictionary<int, PptTracker.BoreholeData> hashToWaypointString = new ConcurrentDictionary<int, PptTracker.BoreholeData>();

		// Token: 0x04000014 RID: 20
		private static int lastClearedIndex;

		// Token: 0x04000015 RID: 21
		private static int hashCounter;

		// Token: 0x04000016 RID: 22
		private const int MaxSlots = 100000;

		// Token: 0x0200001A RID: 26
		[Nullable(0)]
		public class BoreholeData : IEquatable<PptTracker.BoreholeData>
		{
			// Token: 0x060000A4 RID: 164 RVA: 0x0000623D File Offset: 0x0000443D
			public BoreholeData(string text, Vec3d position)
			{
				this.text = text;
				this.position = position;
				base..ctor();
			}

			// Token: 0x17000016 RID: 22
			// (get) Token: 0x060000A5 RID: 165 RVA: 0x00006253 File Offset: 0x00004453
			[CompilerGenerated]
			protected virtual Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(PptTracker.BoreholeData);
				}
			}

			// Token: 0x17000017 RID: 23
			// (get) Token: 0x060000A6 RID: 166 RVA: 0x0000625F File Offset: 0x0000445F
			// (set) Token: 0x060000A7 RID: 167 RVA: 0x00006267 File Offset: 0x00004467
			public string text { get; set; }

			// Token: 0x17000018 RID: 24
			// (get) Token: 0x060000A8 RID: 168 RVA: 0x00006270 File Offset: 0x00004470
			// (set) Token: 0x060000A9 RID: 169 RVA: 0x00006278 File Offset: 0x00004478
			public Vec3d position { get; set; }

			// Token: 0x060000AA RID: 170 RVA: 0x00006284 File Offset: 0x00004484
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("BoreholeData");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060000AB RID: 171 RVA: 0x000062D0 File Offset: 0x000044D0
			[CompilerGenerated]
			protected virtual bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("text = ");
				builder.Append(this.text);
				builder.Append(", position = ");
				builder.Append(this.position);
				return true;
			}

			// Token: 0x060000AC RID: 172 RVA: 0x0000630A File Offset: 0x0000450A
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(PptTracker.BoreholeData left, PptTracker.BoreholeData right)
			{
				return !(left == right);
			}

			// Token: 0x060000AD RID: 173 RVA: 0x00006316 File Offset: 0x00004516
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(PptTracker.BoreholeData left, PptTracker.BoreholeData right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x060000AE RID: 174 RVA: 0x0000632A File Offset: 0x0000452A
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<text>k__BackingField)) * -1521134295 + EqualityComparer<Vec3d>.Default.GetHashCode(this.<position>k__BackingField);
			}

			// Token: 0x060000AF RID: 175 RVA: 0x0000636A File Offset: 0x0000456A
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as PptTracker.BoreholeData);
			}

			// Token: 0x060000B0 RID: 176 RVA: 0x00006378 File Offset: 0x00004578
			[NullableContext(2)]
			[CompilerGenerated]
			public virtual bool Equals(PptTracker.BoreholeData other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<text>k__BackingField, other.<text>k__BackingField) && EqualityComparer<Vec3d>.Default.Equals(this.<position>k__BackingField, other.<position>k__BackingField));
			}

			// Token: 0x060000B2 RID: 178 RVA: 0x000063D9 File Offset: 0x000045D9
			[CompilerGenerated]
			protected BoreholeData(PptTracker.BoreholeData original)
			{
				this.text = original.<text>k__BackingField;
				this.position = original.<position>k__BackingField;
			}

			// Token: 0x060000B3 RID: 179 RVA: 0x000063F9 File Offset: 0x000045F9
			[CompilerGenerated]
			public void Deconstruct(out string text, out Vec3d position)
			{
				text = this.text;
				position = this.position;
			}
		}
	}
}
