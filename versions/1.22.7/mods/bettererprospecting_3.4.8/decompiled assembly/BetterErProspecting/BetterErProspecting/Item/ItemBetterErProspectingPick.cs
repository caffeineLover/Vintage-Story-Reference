using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using BetterErProspecting.Config;
using BetterErProspecting.Extensions;
using BetterErProspecting.Item.Data;
using BetterErProspecting.Prospecting;
using BetterErProspecting.Tracking;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using Vintagestory.ServerMods;

namespace BetterErProspecting.Item
{
	// Token: 0x02000010 RID: 16
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ItemBetterErProspectingPick : ItemProspectingPick
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00004C64 File Offset: 0x00002E64
		private ILogger log
		{
			get
			{
				return BetterErProspect.Logger;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00004C6B File Offset: 0x00002E6B
		public static ModConfig config
		{
			get
			{
				return ModConfig.Instance;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004C74 File Offset: 0x00002E74
		public void RegisterMode(ModeData modeData, bool regenerateModes = true)
		{
			if (!this.modeDataStorage.TryAdd(modeData.Skill.Code.Path, modeData))
			{
				this.log.Error("Trying to add an already existing mode " + modeData.Skill.Code.Path);
			}
			if (regenerateModes)
			{
				this.RegenerateToolModes();
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004CD0 File Offset: 0x00002ED0
		public override void OnLoaded(ICoreAPI Api)
		{
			this.sapi = (Api as ICoreServerAPI);
			base.OnLoaded(Api);
			List<ModeData> list = new List<ModeData>();
			list.Add(new ModeData("density", "textures/icons/heatmap.svg", new Func<IWorldAccessor, IServerPlayer, ItemSlot, BlockSelection, int>(this.ProbeDensity), null, "game"));
			list.Add(new ModeData("node", "textures/icons/rocks.svg", new Func<IWorldAccessor, IServerPlayer, ItemSlot, BlockSelection, int>(this.ProbeNode), "bettererprospecting:node", "game"));
			list.Add(new ModeData("proximity", "textures/icons/worldmap/spiral.svg", new Func<IWorldAccessor, IServerPlayer, ItemSlot, BlockSelection, int>(this.ProbeProximity), "bettererprospecting:proximity", "game"));
			list.Add(new ModeData("stone", "textures/icons/probe_stone.svg", new Func<IWorldAccessor, IServerPlayer, ItemSlot, BlockSelection, int>(this.ProbeStone), "bettererprospecting:stone", "bettererprospecting"));
			list.Add(new ModeData("borehole", "textures/icons/probe_borehole.svg", new Func<IWorldAccessor, IServerPlayer, ItemSlot, BlockSelection, int>(this.ProbeBorehole), "bettererprospecting:borehole", "bettererprospecting"));
			list.ForEach(delegate(ModeData m)
			{
				this.RegisterMode(m, false);
			});
			this.RegenerateToolModes();
			BetterErProspect.ReloadTools += new Action(this.RegenerateToolModes);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004DF0 File Offset: 0x00002FF0
		public void RegenerateToolModes()
		{
			ObjectCacheUtil.Delete(this.api, "proPickToolModes");
			this.toolModes = ObjectCacheUtil.GetOrCreate<SkillItem[]>(this.api, "proPickToolModes", delegate()
			{
				ModeData modeData = this.modeDataStorage["density"];
				modeData.Enabled = ItemBetterErProspectingPick.config.EnableDensityMode;
				modeData.Skill.Name = (ItemBetterErProspectingPick.config.NewDensityMode ? Lang.Get("bettererprospecting:density-block-based", Array.Empty<object>()) : Lang.Get("Density Search Mode (Long range, chance based search)", Array.Empty<object>()));
				this.modeDataStorage["node"].Enabled = (this.api.World.Config.GetAsInt("propickNodeSearchRadius", 0) > 0);
				this.modeDataStorage["proximity"].Enabled = ItemBetterErProspectingPick.config.AddProximityMode;
				this.modeDataStorage["borehole"].Enabled = ItemBetterErProspectingPick.config.AddBoreHoleMode;
				this.modeDataStorage["stone"].Enabled = ItemBetterErProspectingPick.config.AddStoneMode;
				return Enumerable.ToArray<SkillItem>(Enumerable.Select<ModeData, SkillItem>(Enumerable.Where<ModeData>(this.modeDataStorage.Values, (ModeData m) => m.Enabled), (ModeData m) => m.Skill));
			});
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004E28 File Offset: 0x00003028
		public override bool OnBlockBrokenWith(IWorldAccessor world, Entity byEntity, ItemSlot itemslot, BlockSelection blockSel, float dropQuantityMultiplier = 1f)
		{
			EntityPlayer entityPlayer = byEntity as EntityPlayer;
			IPlayer byPlayer = (entityPlayer != null) ? entityPlayer.Player : null;
			int tm = this.GetToolMode(itemslot, byPlayer, blockSel);
			int damage = 1;
			IServerPlayer serverPlayer = byPlayer as IServerPlayer;
			if (serverPlayer != null && tm >= 0 && ItemBetterErProspectingPick.breakIsPropickable(world, blockSel, ref dropQuantityMultiplier))
			{
				string skillItemCode = this.toolModes[tm].Code.Path;
				ModeData modeData;
				if (!this.modeDataStorage.TryGetValue(skillItemCode, ref modeData) || modeData.Execute == null)
				{
					throw new ArgumentException("Declared skill item code not handled for propick: " + skillItemCode);
				}
				damage = modeData.Execute.Invoke(world, serverPlayer, itemslot, blockSel);
			}
			world.BlockAccessor.GetBlock(blockSel.Position).OnBlockBroken(world, blockSel.Position, byPlayer, dropQuantityMultiplier);
			if (this.DamagedBy != null && ArrayExtensions.Contains<EnumItemDamageSource>(this.DamagedBy, 0))
			{
				this.DamageItem(world, byEntity, itemslot, damage, true);
			}
			return true;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004F07 File Offset: 0x00003107
		private int ProbeNode(IWorldAccessor world, IServerPlayer serverPlayer, ItemSlot itemslot, BlockSelection blockSel)
		{
			this.ProbeBlockNodeMode(world, serverPlayer.Entity, itemslot, blockSel, this.api.World.Config.GetAsInt("propickNodeSearchRadius", 0));
			return 1;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004F38 File Offset: 0x00003138
		private int ProbeDensity(IWorldAccessor world, IServerPlayer serverPlayer, ItemSlot itemslot, BlockSelection blockSel)
		{
			if (ItemBetterErProspectingPick.config.NewDensityMode)
			{
				return this.ProbeBlockDensityMode(serverPlayer, blockSel);
			}
			if (ItemBetterErProspectingPick.config.OneShotDensity)
			{
				this.PrintProbeResults(world, serverPlayer, itemslot, blockSel.Position);
				return 3;
			}
			base.ProbeBlockDensityMode(world, serverPlayer.Entity, itemslot, blockSel);
			return 1;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004F8C File Offset: 0x0000318C
		private int ProbeBlockDensityMode(IServerPlayer serverPlayer, BlockSelection blockSel)
		{
			List<DelayedMessage> delayedMessages = new List<DelayedMessage>();
			List<DelayedMessage> debugMessages = new List<DelayedMessage>();
			Dictionary<string, int> codeToFoundCount = ProspectingSystem.GenerateBlockData(this.sapi, blockSel.Position, debugMessages);
			PropickReading readings;
			List<ValueTuple<string, double>> updatePairs;
			if (!ProspectingSystem.generateReadigs(this.sapi, blockSel.Position, codeToFoundCount, out readings, out updatePairs, delayedMessages))
			{
				return 1;
			}
			PptTracker modSystem = this.sapi.ModLoader.GetModSystem<PptTracker>(true);
			if (modSystem != null)
			{
				modSystem.UpdatePpt(updatePairs);
			}
			ProPickWorkSpace ppws = ObjectCacheUtil.TryGet<ProPickWorkSpace>(this.api, "propickworkspace");
			string textResults = readings.ToHumanReadable(serverPlayer.LanguageCode, ppws.pageCodes);
			serverPlayer.Info(textResults, Array.Empty<object>());
			if (ItemBetterErProspectingPick.config.DebugMode)
			{
				debugMessages.ForEach(delegate(DelayedMessage msg)
				{
					msg.Send(serverPlayer);
				});
			}
			delayedMessages.ForEach(delegate(DelayedMessage msg)
			{
				msg.Send(serverPlayer);
			});
			ModSystemOreMap modSystem2 = this.sapi.ModLoader.GetModSystem<ModSystemOreMap>(true);
			if (modSystem2 != null)
			{
				modSystem2.DidProbe(readings, serverPlayer);
			}
			return ItemBetterErProspectingPick.config.NewDensityDmg;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000509C File Offset: 0x0000329C
		private int ProbeProximity(IWorldAccessor world, IServerPlayer serverPlayer, ItemSlot _, BlockSelection blockSel)
		{
			int radius = ItemBetterErProspectingPick.config.ProximitySearchRadius;
			BlockPos pos = blockSel.Position.Copy();
			int closestOre = -1;
			Dictionary<string, string> cache = new Dictionary<string, string>();
			HashSet<string> blacklistedCodes = Enumerable.ToHashSet<string>(Enumerable.Distinct<string>(BetterErProspect.Config.DensityBlackListedOres.Split(',', 3)));
			this.WalkBlocksSphere(pos, radius, delegate(Block walkBlock, int x, int y, int z)
			{
				string text;
				string typeKey;
				if (!ItemBetterErProspectingPick.IsOre(walkBlock, cache, out text, out typeKey))
				{
					return;
				}
				if (blacklistedCodes.Contains(typeKey))
				{
					return;
				}
				int distanceTo = (int)Math.Round((double)pos.DistanceTo((double)x, (double)y, (double)z));
				if (closestOre == -1 || closestOre > distanceTo)
				{
					closestOre = distanceTo;
				}
			});
			if (!ItemBetterErProspectingPick.config.ProximityVagueDescriptors)
			{
				string messageKey = (closestOre != -1) ? "bettererprospecting:closest-ore-is" : "bettererprospecting:closest-ore-not-found";
				object[] array;
				if (closestOre == -1)
				{
					(array = new object[1])[0] = radius;
				}
				else
				{
					(array = new object[1])[0] = closestOre;
				}
				object[] messageArgs = array;
				serverPlayer.Info(messageKey, messageArgs);
			}
			else
			{
				string messageKey = (closestOre != -1) ? "bettererprospecting:promimity-vague-ore-nearby" : "bettererprospecting:proximity-vague-ore-not-found";
				serverPlayer.Info(messageKey, Array.Empty<object>());
			}
			return ItemBetterErProspectingPick.config.ProximityDmg;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000051A0 File Offset: 0x000033A0
		private int ProbeStone(IWorldAccessor world, IServerPlayer serverPlayer, ItemSlot __, BlockSelection blockSel)
		{
			int walkRadius = ItemBetterErProspectingPick.config.StoneSearchRadius;
			bool distanceSearch = !ItemBetterErProspectingPick.config.StonePercentSearch;
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(serverPlayer.L("bettererprospecting:area-sample", new object[]
			{
				walkRadius
			}));
			Dictionary<string, ValueTuple<int, int>> rockInfo = new Dictionary<string, ValueTuple<int, int>>();
			HashSet<string> blacklistedCodes = Enumerable.ToHashSet<string>(Enumerable.Distinct<string>(BetterErProspect.Config.StoneSearchBlackList.Split(',', 3)));
			BlockPos blockPos = blockSel.Position.Copy();
			BlockPos blockEnd = blockPos.AddCopy(-walkRadius, 0, -walkRadius);
			blockEnd.Y = 1;
			Dictionary<string, string> cache = new Dictionary<string, string>();
			this.api.World.BlockAccessor.WalkBlocks(blockPos.AddCopy(walkRadius, walkRadius, walkRadius), blockEnd, delegate(Block walkBlock, int x, int y, int z)
			{
				string fullKey;
				if (!ItemBetterErProspectingPick.IsRock(walkBlock, cache, out fullKey))
				{
					return;
				}
				if (blacklistedCodes.Contains(fullKey))
				{
					return;
				}
				int distance2 = -1;
				if (distanceSearch)
				{
					distance2 = (int)blockSel.Position.DistanceTo(new BlockPos(x, y, z));
				}
				ValueTuple<int, int> existing;
				if (rockInfo.TryGetValue(fullKey, ref existing))
				{
					rockInfo[fullKey] = new ValueTuple<int, int>(Math.Min(existing.Item1, distance2), existing.Item2 + 1);
					return;
				}
				rockInfo[fullKey] = new ValueTuple<int, int>(distance2, 1);
			}, false);
			if (rockInfo.Count == 0)
			{
				serverPlayer.Info("bettererprospecting:no-rocks-near", Array.Empty<object>());
				return ItemBetterErProspectingPick.config.StoneDmg;
			}
			sb.AppendLine(serverPlayer.L("bettererprospecting:found-rocks", Array.Empty<object>()));
			int totalRocks = Enumerable.Sum<ValueTuple<int, int>>(rockInfo.Values, ([TupleElementNames(new string[]
			{
				"Distance",
				"Count"
			})] ValueTuple<int, int> v) => v.Item2);
			List<KeyValuePair<string, ValueTuple<int, int>>> list;
			if (!ItemBetterErProspectingPick.config.StonePercentSearch)
			{
				list = Enumerable.ToList<KeyValuePair<string, ValueTuple<int, int>>>(Enumerable.OrderBy<KeyValuePair<string, ValueTuple<int, int>>, int>(rockInfo, ([TupleElementNames(new string[]
				{
					"Distance",
					"Count"
				})] KeyValuePair<string, ValueTuple<int, int>> kvp) => kvp.Value.Item1));
			}
			else
			{
				list = Enumerable.ToList<KeyValuePair<string, ValueTuple<int, int>>>(Enumerable.OrderByDescending<KeyValuePair<string, ValueTuple<int, int>>, int>(rockInfo, ([TupleElementNames(new string[]
				{
					"Distance",
					"Count"
				})] KeyValuePair<string, ValueTuple<int, int>> kvp) => kvp.Value.Item2));
			}
			PropickReading propickReading = new PropickReading
			{
				Position = blockPos.ToVec3d()
			};
			foreach (KeyValuePair<string, ValueTuple<int, int>> keyValuePair in list)
			{
				string text;
				ValueTuple<int, int> valueTuple;
				keyValuePair.Deconstruct(ref text, ref valueTuple);
				ValueTuple<int, int> valueTuple2 = valueTuple;
				string key = text;
				int distance = valueTuple2.Item1;
				double item = (double)valueTuple2.Item2;
				OreReading rockReading = new OreReading
				{
					DepositCode = key
				};
				double percent = item / (double)totalRocks;
				double percentScaled = Math.Max(percent * 100.0, 0.01);
				rockReading.TotalFactor = Math.Max(percent, 0.026);
				rockReading.PartsPerThousand = percentScaled;
				propickReading.OreReadings[key] = rockReading;
				string itemLink = ItemBetterErProspectingPick.getHandbookLinkOrName(world, serverPlayer, key, null, null);
				if (distanceSearch)
				{
					sb.AppendLine(serverPlayer.L("stone-mode-blocks-away", new object[]
					{
						itemLink,
						distance
					}));
				}
				else
				{
					StringBuilder stringBuilder = sb;
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(4, 2, stringBuilder);
					appendInterpolatedStringHandler.AppendFormatted(itemLink);
					appendInterpolatedStringHandler.AppendLiteral(": ");
					appendInterpolatedStringHandler.AppendFormatted<double>(percentScaled, "0.##");
					appendInterpolatedStringHandler.AppendLiteral(" %");
					stringBuilder2.AppendLine(ref appendInterpolatedStringHandler);
				}
			}
			if (ItemBetterErProspectingPick.config.StoneSearchCreatesReadings)
			{
				ModSystemOreMap modSystem = world.Api.ModLoader.GetModSystem<ModSystemOreMap>(true);
				if (modSystem != null)
				{
					modSystem.DidProbe(propickReading, serverPlayer);
				}
			}
			serverPlayer.Info(sb.ToString(), Array.Empty<object>());
			return ItemBetterErProspectingPick.config.StoneDmg;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00005514 File Offset: 0x00003714
		private int ProbeBorehole(IWorldAccessor world, IServerPlayer serverPlayer, ItemSlot __, BlockSelection blockSel)
		{
			int radius = ItemBetterErProspectingPick.config.BoreholeRadius;
			BlockFacing face = blockSel.Face;
			if (!ItemBetterErProspectingPick.config.BoreholeScansOre && !ItemBetterErProspectingPick.config.BoreholeScansStone)
			{
				serverPlayer.Info("borehole-no-filter", Array.Empty<object>());
				return 1;
			}
			if (face != BlockFacing.UP)
			{
				serverPlayer.Info("borehole-sample-upside", Array.Empty<object>());
				return 1;
			}
			StringBuilder sb = new StringBuilder();
			ProPickWorkSpace ppws = ObjectCacheUtil.TryGet<ProPickWorkSpace>(this.api, "propickworkspace");
			sb.Append(serverPlayer.L("bettererprospecting:borehole-sample-taken", Array.Empty<object>()));
			OrderedDictionary<string, string> blockKeys = new OrderedDictionary<string, string>();
			Dictionary<string, string> cache = new Dictionary<string, string>();
			BlockPos blockPos = blockSel.Position.Copy();
			this.WalkBlocksCylinder(blockPos, radius, delegate(Block walkBlock, int _, int _, int _)
			{
				string fullKey;
				string oreKey;
				if (ItemBetterErProspectingPick.config.BoreholeScansOre && ItemBetterErProspectingPick.IsOre(walkBlock, cache, out fullKey, out oreKey))
				{
					DepositVariant valueOrDefault = CollectionExtensions.GetValueOrDefault<string, DepositVariant>(ppws.depositsByCode, oreKey, null);
					string oreHandbook = (valueOrDefault != null) ? valueOrDefault.HandbookPageCode : null;
					CollectionExtensions.TryAdd<string, string>(blockKeys, fullKey, oreHandbook);
					return;
				}
				string text;
				if (ItemBetterErProspectingPick.config.BoreholeScansStone && ItemBetterErProspectingPick.IsRock(walkBlock, cache, out fullKey, out text))
				{
					CollectionExtensions.TryAdd<string, string>(blockKeys, fullKey, null);
				}
			});
			if (blockKeys.Count == 0)
			{
				sb.AppendLine();
				sb.AppendLine(serverPlayer.L("bettererprospecting:borehole-not-found", Array.Empty<object>()));
			}
			else
			{
				sb.AppendLine(serverPlayer.L("bettererprospecting:borehole-found", Array.Empty<object>()));
				string linkedNames = string.Join(", ", Enumerable.ToList<string>(Enumerable.Select<KeyValuePair<string, string>, string>(blockKeys, (KeyValuePair<string, string> kv) => ItemBetterErProspectingPick.getHandbookLinkOrName(world, serverPlayer, kv.Key, null, blockKeys[kv.Key]))));
				sb.AppendLine(linkedNames);
				sb.AppendLine(PptTracker.trackPlayerBoreholeWp(serverPlayer, linkedNames));
			}
			serverPlayer.Info(sb.ToString(), Array.Empty<object>());
			return ItemBetterErProspectingPick.config.BoreholeDmg;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000056BC File Offset: 0x000038BC
		public override SkillItem[] GetToolModes(ItemSlot slot, IClientPlayer forPlayer, BlockSelection blockSel)
		{
			ICoreAPI api = this.api;
			ICoreClientAPI capi = api as ICoreClientAPI;
			if (capi == null)
			{
				return null;
			}
			EnumerableExtensions.Foreach<SkillItem>(this.toolModes, delegate(SkillItem skill)
			{
				if (skill.Texture != null)
				{
					return;
				}
				AssetLocation asset = this.modeDataStorage[skill.Code.Path].TextureAssetLocation;
				skill.Texture = capi.Gui.LoadSvgWithPadding(asset, 48, 48, 5, new int?(-1));
				skill.TexturePremultipliedAlpha = false;
			});
			return this.toolModes;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00005710 File Offset: 0x00003910
		public override int GetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSel)
		{
			return Math.Min(this.toolModes.Length - 1, slot.Itemstack.Attributes.GetInt("toolMode", 0));
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00005737 File Offset: 0x00003937
		public override void OnUnloaded(ICoreAPI coreApi)
		{
			EnumerableExtensions.Foreach<ModeData>(this.modeDataStorage.Values, delegate(ModeData item)
			{
				item.Skill.Dispose();
			});
			base.OnUnloaded(coreApi);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00005770 File Offset: 0x00003970
		private static string ConvertChildRocks(string code)
		{
			if (code == null)
			{
				return null;
			}
			ReadOnlySpan<char> span = MemoryExtensions.AsSpan(code);
			int idx = code.LastIndexOf('_');
			ReadOnlySpan<char> readOnlySpan;
			if (idx < 0)
			{
				readOnlySpan = span;
			}
			else
			{
				ref ReadOnlySpan<char> ptr = ref span;
				int num = idx + 1;
				readOnlySpan = ptr.Slice(num, ptr.Length - num);
			}
			ReadOnlySpan<char> suffixSpan = readOnlySpan;
			string suffix = suffixSpan.ToString();
			return CollectionExtensions.GetValueOrDefault<string, string>(ItemBetterErProspectingPick.specialOreCodeConversion, suffix, suffix);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000057D0 File Offset: 0x000039D0
		public void WalkBlocksCylinder(BlockPos startPos, int radius, Action<Block, int, int, int> onBlock)
		{
			IBlockAccessor ba = this.sapi.World.BlockAccessor;
			int startX = startPos.X;
			int internalY = startPos.InternalY;
			int startZ = startPos.Z;
			for (int y = internalY; y > 0; y--)
			{
				for (int x = startX - radius; x <= startX + radius; x++)
				{
					for (int z = startZ - radius; z <= startZ + radius; z++)
					{
						int num = x - startX;
						int dz = z - startZ;
						if (num * num + dz * dz <= radius * radius)
						{
							Block block = ba.GetBlock(new BlockPos(x, y, z));
							onBlock.Invoke(block, x, y, z);
						}
					}
				}
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000586C File Offset: 0x00003A6C
		public void WalkBlocksSphere(BlockPos startPos, int radius, Action<Block, int, int, int> onBlock)
		{
			IBlockAccessor ba = this.sapi.World.BlockAccessor;
			int startX = startPos.X;
			int startY = startPos.InternalY;
			int startZ = startPos.Z;
			for (int y = startY - radius; y <= startY + radius; y++)
			{
				for (int x = startX - radius; x <= startX + radius; x++)
				{
					for (int z = startZ - radius; z <= startZ + radius; z++)
					{
						int num = x - startX;
						int dy = y - startY;
						int dz = z - startZ;
						if (num * num + dy * dy + dz * dz <= radius * radius)
						{
							Block block = ba.GetBlock(new BlockPos(x, y, z));
							onBlock.Invoke(block, x, y, z);
						}
					}
				}
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000591F File Offset: 0x00003B1F
		public static string getHandbookLinkOrName(IWorldAccessor world, IServerPlayer serverPlayer, string key, string itemName = null, string handbookUrl = null)
		{
			return ItemBetterErProspectingPick.getHandbookLinkOrName(world, serverPlayer.LanguageCode, key, itemName, handbookUrl);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00005934 File Offset: 0x00003B34
		public static string getHandbookLinkOrName(IWorldAccessor world, string languageCode, string key, [Nullable(2)] string itemName = null, string handbookUrl = null)
		{
			if (itemName == null)
			{
				itemName = Lang.GetL(languageCode, key, Array.Empty<object>());
			}
			if (handbookUrl != null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(26, 2);
				defaultInterpolatedStringHandler.AppendLiteral("<a href=\"handbook://");
				defaultInterpolatedStringHandler.AppendFormatted(handbookUrl);
				defaultInterpolatedStringHandler.AppendLiteral("\">");
				defaultInterpolatedStringHandler.AppendFormatted(itemName);
				defaultInterpolatedStringHandler.AppendLiteral("</a>");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			Block block = world.GetBlock(key);
			if (block != null)
			{
				handbookUrl = GuiHandbookItemStackPage.PageCodeForStack(new ItemStack(block, 1));
			}
			else
			{
				Item item = world.GetItem(key);
				if (item != null)
				{
					handbookUrl = GuiHandbookItemStackPage.PageCodeForStack(new ItemStack(item, 1));
				}
			}
			if (handbookUrl == null)
			{
				return itemName;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
			defaultInterpolatedStringHandler2..ctor(26, 2);
			defaultInterpolatedStringHandler2.AppendLiteral("<a href=\"handbook://");
			defaultInterpolatedStringHandler2.AppendFormatted(handbookUrl);
			defaultInterpolatedStringHandler2.AppendLiteral("\">");
			defaultInterpolatedStringHandler2.AppendFormatted(itemName);
			defaultInterpolatedStringHandler2.AppendLiteral("</a>");
			return defaultInterpolatedStringHandler2.ToStringAndClear();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00005A28 File Offset: 0x00003C28
		public static bool IsOre(Block block, Dictionary<string, string> cache, out string fullKey, out string typeKey)
		{
			fullKey = null;
			typeKey = null;
			if (block.BlockMaterial != 7 || block.Variant == null)
			{
				return false;
			}
			string oreKey;
			if (!block.Variant.TryGetValue("type", ref oreKey))
			{
				return false;
			}
			if (!cache.TryGetValue(oreKey, ref typeKey))
			{
				typeKey = ItemBetterErProspectingPick.ConvertChildRocks(oreKey);
				cache[oreKey] = typeKey;
			}
			fullKey = "ore-" + typeKey;
			return true;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00005A90 File Offset: 0x00003C90
		public static bool IsOre(Block block, Dictionary<string, string> cache, out string fullKey)
		{
			string text;
			return ItemBetterErProspectingPick.IsOre(block, cache, out fullKey, out text);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00005AA8 File Offset: 0x00003CA8
		public static bool IsRock(Block block, Dictionary<string, string> cache, out string fullkey, out string rockKey)
		{
			fullkey = null;
			rockKey = null;
			if (block.Variant == null || !block.Variant.TryGetValue("rock", ref rockKey))
			{
				return false;
			}
			string cached;
			if (!cache.TryGetValue(rockKey, ref cached))
			{
				cached = "rock-" + rockKey;
				cache[rockKey] = cached;
			}
			fullkey = cached;
			return true;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00005B00 File Offset: 0x00003D00
		public static bool IsRock(Block block, Dictionary<string, string> cache, out string fullkey)
		{
			string text;
			return ItemBetterErProspectingPick.IsRock(block, cache, out fullkey, out text);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00005B18 File Offset: 0x00003D18
		private static bool breakIsPropickable(IWorldAccessor world, BlockSelection blockSel, ref float dropMultiplier)
		{
			Block block = world.BlockAccessor.GetBlock(blockSel.Position);
			bool flag;
			if (block == null)
			{
				flag = false;
			}
			else
			{
				JsonObject attributes = block.Attributes;
				flag = ((attributes != null) ? new bool?(!attributes["propickable"].AsBool(false)) : default(bool?)).GetValueOrDefault();
			}
			if (flag)
			{
				return false;
			}
			dropMultiplier = 0f;
			return true;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00005B90 File Offset: 0x00003D90
		// Note: this type is marked as 'beforefieldinit'.
		static ItemBetterErProspectingPick()
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("nativegold", "gold");
			dictionary.Add("nativesilver", "silver");
			dictionary.Add("lapislazuli", "lapis");
			ItemBetterErProspectingPick.specialOreCodeConversion = dictionary;
		}

		// Token: 0x0400001E RID: 30
		private ICoreServerAPI sapi;

		// Token: 0x0400001F RID: 31
		private SkillItem[] toolModes;

		// Token: 0x04000020 RID: 32
		public const int densityRadius = 32;

		// Token: 0x04000021 RID: 33
		private readonly OrderedDictionary<string, ModeData> modeDataStorage = new OrderedDictionary<string, ModeData>();

		// Token: 0x04000022 RID: 34
		public static Dictionary<string, string> specialOreCodeConversion;
	}
}
