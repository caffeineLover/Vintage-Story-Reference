using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using BetterErProspecting.Item;
using BetterErProspecting.Item.Data;
using BetterErProspecting.Tracking;
using HydrateOrDiedrate;
using HydrateOrDiedrate.Config;
using HydrateOrDiedrate.Wells.Aquifer;
using HydrateOrDiedrate.Wells.Aquifer.ModData;
using Vintagestory.API.Common;
using Vintagestory.API.Common.CommandAbbr;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using Vintagestory.ServerMods;

namespace BetterErProspecting.Prospecting
{
	// Token: 0x02000007 RID: 7
	[NullableContext(1)]
	[Nullable(0)]
	public class ProspectingSystem : ModSystem
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000030 RID: 48 RVA: 0x0000334D File Offset: 0x0000154D
		private static ILogger logger
		{
			get
			{
				return BetterErProspect.Logger;
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003354 File Offset: 0x00001554
		public override void StartPre(ICoreAPI api)
		{
			base.StartPre(api);
			this.sapi = (api as ICoreServerAPI);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x0000336C File Offset: 0x0000156C
		public override void StartServerSide(ICoreServerAPI api)
		{
			base.StartServerSide(api);
			this.sapi = api;
			this.pptTracker = api.ModLoader.GetModSystem<PptTracker>(true);
			IChatCommandExt.EndSub(IChatCommandExt.WithDesc(IChatCommandExt.BeginSub(IChatCommandExt.EndSub(IChatCommandExt.BeginSub(api.ChatCommands.GetOrCreate("btrpr").RequiresPrivilege(Privilege.controlserver), "reprospectcancell").WithExamples(new string[]
			{
				"/btrpr reprospectcancell"
			}).HandleWith(new OnCommandDelegate(this.CancelReprospect))), "reprospect"), "Regenerates prospecting data for all players ( including offline ). Optionally only for one player. Expensive operation.").WithExamples(new string[]
			{
				"/btrpr reprospect",
				"/btrpr reprospect KnewOne"
			}).WithArgs(new ICommandArgumentParser[]
			{
				new OnlinePlayerArgParser("player", api, false)
			}).HandleWith(new OnCommandDelegate(this.Reprospect)));
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003446 File Offset: 0x00001646
		private TextCommandResult CancelReprospect(TextCommandCallingArgs args)
		{
			CancellationTokenSource cancellationTokenSource = Interlocked.Exchange<CancellationTokenSource>(ref this._cts, new CancellationTokenSource());
			cancellationTokenSource.Cancel();
			cancellationTokenSource.Dispose();
			this.isReprospecting = false;
			return TextCommandResult.Success("Done", null);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003478 File Offset: 0x00001678
		private TextCommandResult Reprospect(TextCommandCallingArgs args)
		{
			if (this.isReprospecting)
			{
				return TextCommandResult.Error("Please wait before the previous command ends", "");
			}
			IServerPlayer caller = args.Caller.Player as IServerPlayer;
			IServerPlayer targetPlayer = args.Parsers[0].GetValue() as IServerPlayer;
			Task.Run(delegate()
			{
				this.ReprospectTask(caller, targetPlayer);
			});
			return TextCommandResult.Success("[BetterEr Prospecting] Began reprospecting", null);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000034FC File Offset: 0x000016FC
		private Task EnsureChunkLoaded(int cx, int cz)
		{
			return this.chunkLoadTasks.GetOrAdd(new ValueTuple<int, int>(cx, cz), delegate(ValueTuple<int, int> _)
			{
				TaskCompletionSource tcs = new TaskCompletionSource();
				if (this.sapi.WorldManager.GetMapChunk(cx, cz) != null)
				{
					tcs.SetResult();
					return tcs.Task;
				}
				ChunkLoadOptions opts = new ChunkLoadOptions();
				ChunkLoadOptions chunkLoadOptions = opts;
				chunkLoadOptions.OnLoaded = (Action)Delegate.Combine(chunkLoadOptions.OnLoaded, delegate()
				{
					tcs.TrySetResult();
				});
				this.sapi.WorldManager.LoadChunkColumnPriority(cx, cz, opts);
				return tcs.Task;
			});
		}

		// Token: 0x06000036 RID: 54 RVA: 0x0000354C File Offset: 0x0000174C
		public Task ReprospectTask(IServerPlayer caller, IServerPlayer targetPlayer)
		{
			ProspectingSystem.<ReprospectTask>d__13 <ReprospectTask>d__;
			<ReprospectTask>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ReprospectTask>d__.<>4__this = this;
			<ReprospectTask>d__.caller = caller;
			<ReprospectTask>d__.targetPlayer = targetPlayer;
			<ReprospectTask>d__.<>1__state = -1;
			<ReprospectTask>d__.<>t__builder.Start<ProspectingSystem.<ReprospectTask>d__13>(ref <ReprospectTask>d__);
			return <ReprospectTask>d__.<>t__builder.Task;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000035A0 File Offset: 0x000017A0
		public static Dictionary<string, int> GenerateBlockData(ICoreServerAPI api, BlockPos blockPos, List<DelayedMessage> debugMessages = null)
		{
			if (debugMessages == null)
			{
				debugMessages = new List<DelayedMessage>();
			}
			int mapHeight = api.World.BlockAccessor.GetTerrainMapheightAt(blockPos);
			HashSet<string> blacklistedCodes = Enumerable.ToHashSet<string>(Enumerable.Distinct<string>(BetterErProspect.Config.DensityBlackListedOres.Split(',', 3)));
			ProPickWorkSpace ppws = ObjectCacheUtil.TryGet<ProPickWorkSpace>(api, "propickworkspace");
			Dictionary<string, int> codeToFoundCount = new Dictionary<string, int>();
			HashSet<string> nopageVariant = new HashSet<string>();
			HashSet<string> depositKeys = new HashSet<string>(ppws.depositsByCode.Keys);
			Dictionary<string, string> blockCache = new Dictionary<string, string>();
			api.World.BlockAccessor.WalkBlocks(new BlockPos(blockPos.X - 32, mapHeight, blockPos.Z - 32), new BlockPos(blockPos.X + 32, 0, blockPos.Z + 32), delegate(Block walkBlock, int x, int y, int z)
			{
				if (walkBlock.Variant == null)
				{
					return;
				}
				string text;
				string typeKey;
				bool isOre = ItemBetterErProspectingPick.IsOre(walkBlock, blockCache, out text, out typeKey);
				bool isRock = !isOre && ItemBetterErProspectingPick.IsRock(walkBlock, blockCache, out text, out typeKey);
				if (!isOre && !isRock)
				{
					return;
				}
				if (blacklistedCodes.Contains(typeKey))
				{
					return;
				}
				if (depositKeys.Contains(typeKey))
				{
					codeToFoundCount[typeKey] = CollectionExtensions.GetValueOrDefault<string, int>(codeToFoundCount, typeKey, 0) + 1;
					return;
				}
				if (isOre)
				{
					nopageVariant.Add(typeKey);
				}
			}, false);
			if (nopageVariant.Count > 0)
			{
				debugMessages.Add(new DelayedMessage("debug-bad-ppws-key", new object[]
				{
					string.Join(", ", nopageVariant)
				}));
				debugMessages.Add(new DelayedMessage("debug-bad-ppws-key-expected", new object[]
				{
					string.Join(", ", ppws.depositsByCode.Keys)
				}));
			}
			return codeToFoundCount;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000036EC File Offset: 0x000018EC
		public static bool generateReadigs(ICoreServerAPI sapi, BlockPos blockPos, Dictionary<string, int> codeToFoundOre, out PropickReading readings, [TupleElementNames(new string[]
		{
			"oreCode",
			"ppt"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] out List<ValueTuple<string, double>> updatePairs, List<DelayedMessage> delayedMessages = null)
		{
			if (delayedMessages == null)
			{
				delayedMessages = new List<DelayedMessage>();
			}
			IServerWorldAccessor world = sapi.World;
			GenDeposits modSystem = sapi.ModLoader.GetModSystem<GenDeposits>(true);
			bool flag = ((modSystem != null) ? modSystem.Deposits : null) != null;
			ProPickWorkSpace ppws = ObjectCacheUtil.TryGet<ProPickWorkSpace>(world.Api, "propickworkspace");
			if (!flag)
			{
				readings = null;
				updatePairs = new List<ValueTuple<string, double>>();
				return false;
			}
			int mapHeight = world.BlockAccessor.GetTerrainMapheightAt(blockPos);
			int zoneBlocks = 4096 * mapHeight;
			readings = new PropickReading
			{
				Position = blockPos.ToVec3d()
			};
			updatePairs = new List<ValueTuple<string, double>>();
			foreach (KeyValuePair<string, int> keyValuePair in codeToFoundOre)
			{
				string text;
				int num;
				keyValuePair.Deconstruct(ref text, ref num);
				string oreCode = text;
				int empiricalAmount = num;
				OreReading reading = new OreReading
				{
					PartsPerThousand = (double)empiricalAmount / (double)zoneBlocks * 1000.0
				};
				int regsize = world.BlockAccessor.RegionSize;
				IntDataMap2D map;
				if (world.BlockAccessor.GetMapRegion(blockPos.X / regsize, blockPos.Z / regsize).OreMaps.TryGetValue(oreCode, ref map))
				{
					float num2 = (float)(blockPos.X % regsize);
					int lz = blockPos.Z % regsize;
					int noiseSize = map.InnerSize;
					float posXInRegionOre = num2 / (float)regsize * (float)noiseSize;
					float posZInRegionOre = (float)lz / (float)regsize * (float)noiseSize;
					int oreDist = map.GetUnpaddedColorLerped(posXInRegionOre, posZInRegionOre);
					int[] blockColumn = ppws.GetRockColumn(blockPos.X, blockPos.Z);
					double num3;
					double baseGameFactor;
					ppws.depositsByCode[oreCode].GeneratorInst.GetPropickReading(blockPos, oreDist, blockColumn, ref num3, ref baseGameFactor);
					reading.TotalFactor = baseGameFactor;
				}
				else
				{
					ProspectingSystem.logger.Warning("Oremap for orecode withOreMap: true, " + oreCode + " not found. Value changed in an existing world ? Generating with base 0.13 factor");
					reading.TotalFactor = 0.13;
				}
				readings.OreReadings[oreCode] = reading;
				updatePairs.Add(new ValueTuple<string, double>(oreCode, reading.PartsPerThousand));
			}
			ProspectingSystem.addMiscReadings(sapi, readings, blockPos, delayedMessages);
			if (PptTracker.ShouldReprospectNotify == 1)
			{
				delayedMessages.Add(new DelayedMessage("[BetterEr Prospecting] World requires a reprospect. Please ask a person with server control permissions to run '/btrpr reprospect' with a '/btrpr oreData` afterwards.", GlobalConstants.GeneralChatGroup, Array.Empty<object>()));
			}
			return true;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003930 File Offset: 0x00001B30
		private static void addMiscReadings(ICoreServerAPI sapi, PropickReading readings, BlockPos blockPos, List<DelayedMessage> delayedMessages)
		{
			if (sapi.ModLoader.IsModEnabled("hydrateordiedrate"))
			{
				ProspectingSystem.addHoDReadingsIfCan(sapi, readings, blockPos, delayedMessages);
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003950 File Offset: 0x00001B50
		private static void addHoDReadingsIfCan(ICoreServerAPI sapi, PropickReading readings, BlockPos pos, List<DelayedMessage> delayedMessages)
		{
			ModSystem modSystem = sapi.ModLoader.GetModSystem<HydrateOrDiedrateModSystem>(true);
			Version minVer = new Version("2.2.13");
			if (new Version(modSystem.Mod.Info.Version) <= minVer)
			{
				delayedMessages.Add(new DelayedMessage("[BetterEr Prospecting] Please update HydrateOrDietrade to at least " + minVer.ToString() + " for aquifer support"));
				return;
			}
			IServerWorldAccessor world = sapi.World;
			AquiferChunkData aquiferChunkData = AquiferManager.GetAquiferChunkData(world, pos, null);
			AquiferData chnData = (aquiferChunkData != null) ? aquiferChunkData.Data : null;
			if (chnData == null)
			{
				return;
			}
			if (ModConfig.Instance.GroundWater.ShowAquiferProspectingDataOnMap)
			{
				readings.OreReadings.Add("$aquifer$", chnData);
			}
			delayedMessages.Add(new DelayedMessage(AquiferManager.GetAquiferDirectionHint(world, pos)));
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003A0A File Offset: 0x00001C0A
		public override void Dispose()
		{
			this.sapi = null;
			base.Dispose();
		}

		// Token: 0x04000017 RID: 23
		private ICoreServerAPI sapi;

		// Token: 0x04000018 RID: 24
		private PptTracker pptTracker;

		// Token: 0x04000019 RID: 25
		private CancellationTokenSource _cts = new CancellationTokenSource();

		// Token: 0x0400001A RID: 26
		[Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private readonly ConcurrentDictionary<ValueTuple<int, int>, Task> chunkLoadTasks = new ConcurrentDictionary<ValueTuple<int, int>, Task>();

		// Token: 0x0400001B RID: 27
		private bool isReprospecting;

		// Token: 0x0400001C RID: 28
		private readonly Lock notifyLock = new Lock();
	}
}
