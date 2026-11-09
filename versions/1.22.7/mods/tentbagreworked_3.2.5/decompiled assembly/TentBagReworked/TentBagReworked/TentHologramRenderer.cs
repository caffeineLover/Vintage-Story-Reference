using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TentBagReworked.Config;
using TentBagReworked.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace TentBagReworked
{
	// Token: 0x02000003 RID: 3
	[NullableContext(1)]
	[Nullable(0)]
	public class TentHologramRenderer : ModSystem
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00003352 File Offset: 0x00001552
		// (set) Token: 0x0600002F RID: 47 RVA: 0x0000335A File Offset: 0x0000155A
		public bool PreviewEnabled { get; private set; } = true;

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00003363 File Offset: 0x00001563
		private static TentBagReworkedModSystem ModSys
		{
			get
			{
				return TentBagReworkedModSystem.Instance;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000336A File Offset: 0x0000156A
		private static TentBagReworkedConfig Cfg
		{
			get
			{
				return TentHologramRenderer.ModSys.EffectiveConfig;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003376 File Offset: 0x00001576
		public override bool ShouldLoad(EnumAppSide forSide)
		{
			return forSide == 2;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000337C File Offset: 0x0000157C
		public override void StartClientSide(ICoreClientAPI api)
		{
			this.capi = api;
			TentHologramRenderer.PreviewInstance = this;
			bool flag = api.Settings.Bool.Exists("tentbagreworked-preview-enabled");
			if (flag)
			{
				this.PreviewEnabled = api.Settings.Bool["tentbagreworked-preview-enabled"];
			}
			api.Event.RegisterGameTickListener(new Action<float>(this.OnPreviewTick), 100, 0);
			api.Input.RegisterHotKey("tentbagpreview", Lang.HotkeyTogglePreview(Array.Empty<object>()), 84, 2, false, false, false);
			api.Input.SetHotKeyHandler("tentbagpreview", new ActionConsumable<KeyCombination>(this.OnTogglePreview));
			api.Input.RegisterHotKey("tentbagrotate", Lang.HotkeyRotate(Array.Empty<object>()), 100, 2, false, false, false);
			api.Input.SetHotKeyHandler("tentbagrotate", new ActionConsumable<KeyCombination>(this.OnRotate));
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003468 File Offset: 0x00001668
		private bool OnTogglePreview(KeyCombination comb)
		{
			this.PreviewEnabled = !this.PreviewEnabled;
			this.capi.Settings.Bool["tentbagreworked-preview-enabled"] = this.PreviewEnabled;
			bool flag = !this.PreviewEnabled;
			if (flag)
			{
				bool flag2 = this.sentEnable;
				if (flag2)
				{
					TentHologramRenderer.ModSys.SendPreviewRequest(false, 0, 0, 0, 0);
					this.sentEnable = false;
					this.sentContents = null;
					this.lastSentTarget = null;
				}
				this.HideClientPreview();
				this.ClearHighlights();
			}
			string status = this.PreviewEnabled ? Lang.StatusOn(Array.Empty<object>()) : Lang.StatusOff(Array.Empty<object>());
			this.capi.ShowChatMessage(Lang.PreviewToggled(new object[]
			{
				status
			}));
			return true;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003534 File Offset: 0x00001734
		private bool OnRotate(KeyCombination comb)
		{
			IClientWorldAccessor world = this.capi.World;
			ItemStack itemStack;
			if (world == null)
			{
				itemStack = null;
			}
			else
			{
				IClientPlayer player = world.Player;
				if (player == null)
				{
					itemStack = null;
				}
				else
				{
					IPlayerInventoryManager inventoryManager = player.InventoryManager;
					if (inventoryManager == null)
					{
						itemStack = null;
					}
					else
					{
						ItemSlot activeHotbarSlot = inventoryManager.ActiveHotbarSlot;
						itemStack = ((activeHotbarSlot != null) ? activeHotbarSlot.Itemstack : null);
					}
				}
			}
			ItemStack stack = itemStack;
			bool flag = !TentHologramRenderer.IsDeployable(stack);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				TentHologramRenderer.ModSys.SendRotateRequest(90);
				result = true;
			}
			return result;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000035A4 File Offset: 0x000017A4
		private void OnPreviewTick(float dt)
		{
			string contents;
			BlockPos basePos;
			int rotation;
			bool flag = !this.TryGetPreviewState(out contents, out basePos, out rotation);
			if (flag)
			{
				bool flag2 = this.sentEnable;
				if (flag2)
				{
					TentHologramRenderer.ModSys.SendPreviewRequest(false, 0, 0, 0, 0);
					this.sentEnable = false;
					this.sentContents = null;
					this.lastSentTarget = null;
				}
				this.HideClientPreview();
				this.ClearHighlights();
			}
			else
			{
				bool targetChanged = !TentHologramRenderer.SamePos(basePos, this.lastSentTarget);
				bool flag3 = !this.sentEnable || this.sentContents != contents || this.sentRotation != rotation || targetChanged;
				if (flag3)
				{
					TentHologramRenderer.ModSys.SendPreviewRequest(true, basePos.X, basePos.Y, basePos.Z, basePos.dimension);
					this.sentEnable = true;
					this.sentContents = contents;
					this.sentRotation = rotation;
					this.lastSentTarget = basePos.Copy();
				}
				long now = Environment.TickCount64;
				bool flag4 = !TentHologramRenderer.SamePos(basePos, this.lastValidatedPos) || now - this.lastValidatedMs > 500L;
				if (flag4)
				{
					List<BlockPos> badBlocks;
					this.lastPlacementValid = this.ValidatePlacement(basePos, out badBlocks);
					this.SetHighlights(this.lastPlacementValid ? TentHologramRenderer.EmptyBlocks : badBlocks);
					this.lastValidatedPos = basePos.Copy();
					this.lastValidatedMs = now;
				}
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003708 File Offset: 0x00001908
		public void OnServerPreviewDim(int dimId, int posX, int posY, int posZ)
		{
			bool flag = dimId < 0;
			if (flag)
			{
				this.HideClientPreview();
			}
			else
			{
				bool first = this.serverDimId < 0;
				this.serverDimId = dimId;
				this.capi.World.SetBlocksPreviewDimension(dimId);
				this.clientDim = this.capi.World.GetOrCreateDimension(dimId, new Vec3d((double)posX, (double)posY, (double)posZ));
				this.clientDim.selectionTrackingOriginalPos = new BlockPos(posX, posY, posZ);
				this.clientDim.TrackSelection = false;
				bool flag2 = first;
				if (flag2)
				{
					this.capi.Logger.VerboseDebug(string.Concat(new string[]
					{
						"[TentBagReworked] preview shown: dim=",
						dimId.ToString(),
						" at ",
						posX.ToString(),
						",",
						posY.ToString(),
						",",
						posZ.ToString()
					}));
				}
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003800 File Offset: 0x00001A00
		private void HideClientPreview()
		{
			bool flag;
			if (this.serverDimId >= 0)
			{
				ICoreClientAPI coreClientAPI = this.capi;
				flag = (((coreClientAPI != null) ? coreClientAPI.World : null) != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				this.capi.World.SetBlocksPreviewDimension(-1);
			}
			this.serverDimId = -1;
			this.clientDim = null;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003858 File Offset: 0x00001A58
		private bool ValidatePlacement(BlockPos basePos, out List<BlockPos> badBlocks)
		{
			List<BlockPos> bad = new List<BlockPos>();
			TentBagReworkedConfig cfg = TentHologramRenderer.Cfg;
			IBlockAccessor ba = this.capi.World.BlockAccessor;
			IClientPlayer player = this.capi.World.Player;
			int plantRock = TentHologramRenderer.IsPlantOrRock(ba.GetBlock(basePos)) ? 1 : 0;
			BlockPos start = basePos.AddCopy(-cfg.MaxRadius, -plantRock, -cfg.MaxRadius);
			BlockPos end = basePos.AddCopy(cfg.MaxRadius, Math.Max(cfg.MaxHeight, 3), cfg.MaxRadius);
			int startY = start.Y;
			int dim = basePos.dimension;
			ba.WalkBlocks(start, end, delegate(Block block, int px, int py, int pz)
			{
				BlockPos pos = new BlockPos(px, py, pz, dim);
				bool flag = player != null && !this.capi.World.Claims.TryAccess(player, pos, 1);
				if (flag)
				{
					bad.Add(pos);
				}
				else
				{
					bool flag2 = py == startY;
					if (flag2)
					{
						bool flag3 = cfg.RequireSolidGround && !block.SideSolid[4];
						if (flag3)
						{
							bad.Add(pos);
						}
					}
					else
					{
						bool flag4 = !TentHologramRenderer.IsReplaceable(block);
						if (flag4)
						{
							bad.Add(pos);
						}
					}
				}
			}, false);
			badBlocks = bad;
			return bad.Count == 0;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003960 File Offset: 0x00001B60
		private void SetHighlights(List<BlockPos> badBlocks)
		{
			IClientWorldAccessor world = this.capi.World;
			IClientPlayer player = (world != null) ? world.Player : null;
			bool flag = world == null || player == null;
			if (!flag)
			{
				bool flag2 = badBlocks.Count == 0;
				if (flag2)
				{
					bool flag3 = this.highlightsShown;
					if (flag3)
					{
						world.HighlightBlocks(player, 1338, TentHologramRenderer.EmptyBlocks, 0, 0);
						this.highlightsShown = false;
					}
				}
				else
				{
					int color = TentHologramRenderer.Cfg.HighlightErrorColor.ToColor().Reverse();
					List<int> colors = Enumerable.ToList<int>(Enumerable.Repeat<int>(color, badBlocks.Count));
					world.HighlightBlocks(player, 1338, badBlocks, colors, 0, 0, 1f);
					this.highlightsShown = true;
				}
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003A1C File Offset: 0x00001C1C
		private void ClearHighlights()
		{
			bool flag = !this.highlightsShown;
			if (!flag)
			{
				IClientWorldAccessor world = this.capi.World;
				IClientPlayer player = (world != null) ? world.Player : null;
				bool flag2 = player != null;
				if (flag2)
				{
					world.HighlightBlocks(player, 1338, TentHologramRenderer.EmptyBlocks, 0, 0);
				}
				this.highlightsShown = false;
			}
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003A78 File Offset: 0x00001C78
		private bool TryGetPreviewState(out string contents, out BlockPos basePos, out int rotation)
		{
			contents = "";
			basePos = null;
			rotation = 0;
			bool flag = !this.PreviewEnabled;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				IClientWorldAccessor world = this.capi.World;
				IClientPlayer player = (world != null) ? world.Player : null;
				ItemStack itemStack;
				if (player == null)
				{
					itemStack = null;
				}
				else
				{
					IPlayerInventoryManager inventoryManager = player.InventoryManager;
					if (inventoryManager == null)
					{
						itemStack = null;
					}
					else
					{
						ItemSlot activeHotbarSlot = inventoryManager.ActiveHotbarSlot;
						itemStack = ((activeHotbarSlot != null) ? activeHotbarSlot.Itemstack : null);
					}
				}
				ItemStack stack = itemStack;
				bool flag2 = !TentHologramRenderer.IsDeployable(stack);
				if (flag2)
				{
					result = false;
				}
				else
				{
					BlockSelection sel = player.CurrentBlockSelection;
					bool flag3 = ((sel != null) ? sel.Position : null) == null;
					if (flag3)
					{
						result = false;
					}
					else
					{
						ITreeAttribute attributes = stack.Attributes;
						string text;
						if ((text = ((attributes != null) ? attributes.GetString("packed-contents", null) : null)) == null)
						{
							ITreeAttribute attributes2 = stack.Attributes;
							text = (((attributes2 != null) ? attributes2.GetString("tent-contents", null) : null) ?? "");
						}
						contents = text;
						bool flag4 = string.IsNullOrEmpty(contents);
						if (flag4)
						{
							result = false;
						}
						else
						{
							ITreeAttribute attributes3 = stack.Attributes;
							rotation = ((attributes3 != null) ? attributes3.GetInt("packed-rotation", 0) : 0);
							basePos = sel.Position;
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003BA0 File Offset: 0x00001DA0
		[NullableContext(2)]
		private static bool IsDeployable(ItemStack stack)
		{
			bool flag = ((stack != null) ? stack.Attributes : null) == null;
			return !flag && (!string.IsNullOrEmpty(stack.Attributes.GetString("packed-contents", null)) || !string.IsNullOrEmpty(stack.Attributes.GetString("tent-contents", null)));
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003BFE File Offset: 0x00001DFE
		[NullableContext(2)]
		private static bool IsAirOrNull(Block block)
		{
			return block == null || block.Replaceable >= 9505;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003C18 File Offset: 0x00001E18
		[NullableContext(2)]
		private static bool IsPlantOrRock(Block block)
		{
			bool result;
			if (TentHologramRenderer.Cfg.ReplacePlantsAndRocks)
			{
				int? num = (block != null) ? new int?(block.Replaceable) : default(int?);
				if (num != null)
				{
					int valueOrDefault = num.GetValueOrDefault();
					if (valueOrDefault >= 5500)
					{
						result = (valueOrDefault <= 6500);
						goto IL_4D;
					}
				}
				result = false;
				IL_4D:;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003C75 File Offset: 0x00001E75
		[NullableContext(2)]
		private static bool IsReplaceable(Block block)
		{
			return TentHologramRenderer.IsAirOrNull(block) || TentHologramRenderer.IsPlantOrRock(block);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003C88 File Offset: 0x00001E88
		[NullableContext(2)]
		private static bool SamePos(BlockPos a, BlockPos b)
		{
			bool flag = a == null || b == null;
			bool result;
			if (flag)
			{
				result = (a == b);
			}
			else
			{
				result = (a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.dimension == b.dimension);
			}
			return result;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003CF4 File Offset: 0x00001EF4
		public override void Dispose()
		{
			base.Dispose();
			bool flag = this.capi != null;
			if (flag)
			{
				bool flag2 = this.sentEnable;
				if (flag2)
				{
					TentHologramRenderer.ModSys.SendPreviewRequest(false, 0, 0, 0, 0);
				}
				this.ClearHighlights();
				this.HideClientPreview();
			}
			bool flag3 = TentHologramRenderer.PreviewInstance == this;
			if (flag3)
			{
				TentHologramRenderer.PreviewInstance = null;
			}
		}

		// Token: 0x04000016 RID: 22
		[Nullable(2)]
		internal static TentHologramRenderer PreviewInstance;

		// Token: 0x04000017 RID: 23
		private const int PreviewHighlightSlot = 1338;

		// Token: 0x04000018 RID: 24
		private const string HotkeyCode = "tentbagpreview";

		// Token: 0x04000019 RID: 25
		private const string RotateHotkeyCode = "tentbagrotate";

		// Token: 0x0400001A RID: 26
		private const string PreviewEnabledSetting = "tentbagreworked-preview-enabled";

		// Token: 0x0400001B RID: 27
		private static readonly List<BlockPos> EmptyBlocks = new List<BlockPos>();

		// Token: 0x0400001C RID: 28
		private ICoreClientAPI capi = null;

		// Token: 0x0400001E RID: 30
		[Nullable(2)]
		private IMiniDimension clientDim;

		// Token: 0x0400001F RID: 31
		private int serverDimId = -1;

		// Token: 0x04000020 RID: 32
		private bool sentEnable;

		// Token: 0x04000021 RID: 33
		[Nullable(2)]
		private string sentContents;

		// Token: 0x04000022 RID: 34
		private int sentRotation = -1;

		// Token: 0x04000023 RID: 35
		[Nullable(2)]
		private BlockPos lastSentTarget;

		// Token: 0x04000024 RID: 36
		private bool lastPlacementValid = true;

		// Token: 0x04000025 RID: 37
		[Nullable(2)]
		private BlockPos lastValidatedPos;

		// Token: 0x04000026 RID: 38
		private long lastValidatedMs;

		// Token: 0x04000027 RID: 39
		private bool highlightsShown;
	}
}
