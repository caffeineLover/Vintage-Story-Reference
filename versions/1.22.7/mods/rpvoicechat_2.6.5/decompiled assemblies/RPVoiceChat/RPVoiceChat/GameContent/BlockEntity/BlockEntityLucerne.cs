using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RPVoiceChat.GameContent.Inventory;
using RPVoiceChat.GameContent.Renderers;
using RPVoiceChat.Gui;
using RPVoiceChat.Systems;
using RPVoiceChat.Util;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace RPVoiceChat.GameContent.BlockEntity
{
	// Token: 0x020000F3 RID: 243
	public class BlockEntityLucerne : BlockEntityOpenableContainer, IBlockEntityWithCustomLightPosition, IHeatSource
	{
		// Token: 0x060008A0 RID: 2208 RVA: 0x0002FAB0 File Offset: 0x0002DCB0
		static BlockEntityLucerne()
		{
			BlockEntityLucerne._fireParticles = new SimpleParticleProperties(2f, 3f, ColorUtil.ColorFromRgba(30, 120, 255, 255), new Vec3d(), new Vec3d(3.0, 1.5, 3.0), new Vec3f(0f, 0.15f, 0f), new Vec3f(0.12f, 0.06f, 0.12f), 1f, 1f, 1f, 1f, 1)
			{
				LifeLength = 0.35f,
				GravityEffect = 0f,
				MinSize = 0.4f,
				MaxSize = 0.55f,
				ParticleModel = 0,
				SizeEvolve = new EvolvingNatFloat(1, 0.35f),
				OpacityEvolve = new EvolvingNatFloat(5, -14f),
				VertexFlags = 128,
				WindAffected = true
			};
			BlockEntityLucerne._smokeParticles = new SimpleParticleProperties(0f, 1f, ColorUtil.ColorFromRgba(55, 70, 85, 200), new Vec3d(), new Vec3d(3.0, 1.5, 3.0), new Vec3f(0f, 0.22f, 0f), new Vec3f(0.03f, 0.06f, 0.03f), 1f, 1f, 1f, 1f, 1)
			{
				LifeLength = 20f,
				GravityEffect = 0f,
				MinSize = 0.35f,
				MaxSize = 0.5f,
				ParticleModel = 0,
				SizeEvolve = new EvolvingNatFloat(1, 1.8f),
				OpacityEvolve = new EvolvingNatFloat(1, -255f),
				SelfPropelled = true,
				WindAffected = true
			};
			BlockEntityLucerne._emberParticles = new SimpleParticleProperties(0f, 1f, ColorUtil.ColorFromRgba(25, 100, 255, 255), new Vec3d(), new Vec3d(3.0, 1.5, 3.0), new Vec3f(0f, 0.55f, 0f), new Vec3f(0.18f, 0.12f, 0.18f), 1f, 1f, 1f, 1f, 1)
			{
				LifeLength = 1.8f,
				GravityEffect = -0.08f,
				MinSize = 0.35f,
				MaxSize = 0.5f,
				ParticleModel = 1,
				SizeEvolve = new EvolvingNatFloat(5, -0.65f),
				VertexFlags = 128,
				WindAffected = true
			};
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0002FDA4 File Offset: 0x0002DFA4
		private static int GetCustomPacketIdBase()
		{
			int result;
			try
			{
				Type enumType = typeof(EnumBlockContainerPacketId);
				if (enumType == null || !enumType.IsEnum)
				{
					result = 10000;
				}
				else
				{
					Array values = Enum.GetValues(enumType);
					int max = 0;
					foreach (object obj in values)
					{
						int i = (int)obj;
						if (i > max)
						{
							max = i;
						}
					}
					result = max + 1;
				}
			}
			catch
			{
				result = 10000;
			}
			return result;
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x0002FE44 File Offset: 0x0002E044
		public override InventoryBase Inventory
		{
			get
			{
				return this._inventory;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060008A3 RID: 2211 RVA: 0x0002FE4C File Offset: 0x0002E04C
		public override string InventoryClassName
		{
			get
			{
				return "lucerne";
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x0002FE53 File Offset: 0x0002E053
		public bool ShowStructureGuide
		{
			get
			{
				return this._showStructureGuide;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060008A5 RID: 2213 RVA: 0x0002FE5B File Offset: 0x0002E05B
		public bool StructureComplete
		{
			get
			{
				return this._structureComplete;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x0002FE63 File Offset: 0x0002E063
		public bool IsFired
		{
			get
			{
				Block block = base.Block;
				return ((block != null) ? block.Variant["materialtype"] : null) == "fired";
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060008A7 RID: 2215 RVA: 0x0002FE8C File Offset: 0x0002E08C
		public bool IsBurning
		{
			get
			{
				ICoreAPI api = this.Api;
				bool flag;
				if (api == null)
				{
					flag = (null != null);
				}
				else
				{
					IWorldAccessor world = api.World;
					flag = (((world != null) ? world.Calendar : null) != null);
				}
				return flag && this.Api.World.Calendar.TotalHours < this._burnEndTotalHours;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x0002FED8 File Offset: 0x0002E0D8
		public double BurnTimeRemainingGameHours
		{
			get
			{
				if (!this.IsBurning)
				{
					return 0.0;
				}
				return Math.Max(0.0, this._burnEndTotalHours - this.Api.World.Calendar.TotalHours);
			}
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0002FF20 File Offset: 0x0002E120
		public override void Initialize(ICoreAPI api)
		{
			if (this._inventory == null)
			{
				this._inventory = new InventoryLucerne(api, this.Pos);
			}
			base.Initialize(api);
			this.LateInitInventory();
			if (api.Side == 1)
			{
				ICoreServerAPI coreServerAPI = api as ICoreServerAPI;
				if (coreServerAPI != null)
				{
					coreServerAPI.Event.RegisterGameTickListener(new Action<float>(this.OnServerGameTick), 100, 0);
				}
				if (this.IsFired)
				{
					WarningBeaconStructure.RegisterLucerne(this.Pos, this.GetFacing());
				}
			}
			if (api.Side == 2)
			{
				ICoreClientAPI capi = api as ICoreClientAPI;
				if (capi != null)
				{
					capi.Event.EnqueueMainThreadTask(delegate()
					{
						this.UpdateStructureGuideHighlight();
						this.UpdateBonfireRenderer();
						this._flameParticlesTickListenerId = capi.Event.RegisterGameTickListener(new Action<float>(this.OnClientGameTickFlames), 4, 0);
					}, "rpvoicechat:InitBeaconRenderers");
				}
				this.RevalidateStructure();
				return;
			}
			this.RevalidateStructure();
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0002FFF4 File Offset: 0x0002E1F4
		protected void LateInitInventory()
		{
			InventoryBase inventory = this._inventory;
			string inventoryClassName = this.InventoryClassName;
			string text = "-";
			BlockPos pos = this.Pos;
			inventory.LateInitialize(inventoryClassName + text + ((pos != null) ? pos.ToString() : null), this.Api);
			this._inventory.ResolveBlocksOrItems();
			InventoryLucerne inventory2 = this._inventory;
			if (inventory2.Pos == null)
			{
				inventory2.Pos = this.Pos;
			}
			this.MarkDirty(false, null);
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00030062 File Offset: 0x0002E262
		public override void OnBlockPlaced(ItemStack byItemStack = null)
		{
			base.OnBlockPlaced(byItemStack);
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1 && this.IsFired)
			{
				this.RevalidateStructure();
			}
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00030090 File Offset: 0x0002E290
		public void ToggleStructureGuide(IPlayer byPlayer)
		{
			this._showStructureGuide = !this._showStructureGuide;
			this.MarkDirty(true, null);
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x000300AC File Offset: 0x0002E2AC
		private BlockFacing GetFacing()
		{
			ICoreAPI api = this.Api;
			Block block2;
			if (api == null)
			{
				block2 = null;
			}
			else
			{
				IWorldAccessor world = api.World;
				if (world == null)
				{
					block2 = null;
				}
				else
				{
					IBlockAccessor blockAccessor = world.BlockAccessor;
					block2 = ((blockAccessor != null) ? blockAccessor.GetBlock(this.Pos) : null);
				}
			}
			Block block = block2;
			if (block == null)
			{
				return BlockFacing.NORTH;
			}
			return BlockFacing.FromCode(block.Variant["side"] ?? "north");
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x00030114 File Offset: 0x0002E314
		public BlockPos GetStructureCenterWorldPos()
		{
			BlockPos c = WarningBeaconStructure.CenterLocal;
			return WarningBeaconStructure.LocalToWorld(this.Pos, c.X, c.Y, c.Z, this.GetFacing());
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0003014A File Offset: 0x0002E34A
		public Vec3d GetStructureCenter3x3WorldPos()
		{
			return this.GetStructureCenterWorldPos().ToWorldCenter();
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00030157 File Offset: 0x0002E357
		Vec3d IBlockEntityWithCustomLightPosition.GetLightOrigin()
		{
			return this.GetStructureCenter3x3WorldPos();
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x00030160 File Offset: 0x0002E360
		private void RevalidateStructure()
		{
			if (!this.IsFired)
			{
				return;
			}
			ICoreAPI api = this.Api;
			IBlockAccessor blockAccessor;
			if (api == null)
			{
				blockAccessor = null;
			}
			else
			{
				IWorldAccessor world = api.World;
				blockAccessor = ((world != null) ? world.BlockAccessor : null);
			}
			IBlockAccessor accessor = blockAccessor;
			Block block = (accessor != null) ? accessor.GetBlock(this.Pos) : null;
			if (accessor == null || block == null)
			{
				return;
			}
			bool wasComplete = this._structureComplete;
			int filled;
			int total;
			WarningBeaconStructure.ValidateStructure(accessor, this.Pos, this.GetFacing(), block, out filled, out total);
			this._structureComplete = (filled == total);
			if (!wasComplete && this._structureComplete)
			{
				this._showStructureGuide = false;
				this.MarkDirty(true, null);
				this.Api.World.PlaySoundAt(new AssetLocation("game", "sounds/block/rock"), (double)this.Pos.X + 0.5, (double)this.Pos.Y + 0.5, (double)this.Pos.Z + 0.5, null, false, 8f, 0.5f);
				return;
			}
			if (wasComplete && !this._structureComplete)
			{
				this.MarkDirty(true, null);
			}
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x00030278 File Offset: 0x0002E478
		public override bool OnPlayerRightClick(IPlayer byPlayer, BlockSelection blockSel)
		{
			if (!this.IsFired || !this._structureComplete)
			{
				return false;
			}
			if (this.Api.World is IServerWorldAccessor)
			{
				byte[] data;
				using (MemoryStream ms = new MemoryStream())
				{
					BinaryWriter writer = new BinaryWriter(ms);
					writer.Write("BlockEntityLucerne");
					writer.Write(Lang.Get(RPVoiceChatMod.modID + ":WarningBeacon.GuiTitle", Array.Empty<object>()));
					TreeAttribute tree = new TreeAttribute();
					this._inventory.ToTreeAttributes(tree);
					tree.ToBytes(writer);
					data = ms.ToArray();
				}
				((ICoreServerAPI)this.Api).Network.SendBlockEntityPacket((IServerPlayer)byPlayer, this.Pos, 5000, data);
				byPlayer.InventoryManager.OpenInventory(this._inventory);
			}
			return true;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x0003035C File Offset: 0x0002E55C
		public override void OnReceivedClientPacket(IPlayer fromPlayer, int packetid, byte[] data)
		{
			if (packetid == BlockEntityLucerne.PacketIdLightBeacon)
			{
				if (this.TryLightBeacon())
				{
					TreeAttribute tree = new TreeAttribute();
					this.ToTreeAttributes(tree);
					using (MemoryStream ms = new MemoryStream())
					{
						tree.ToBytes(new BinaryWriter(ms));
						byte[] syncData = ms.ToArray();
						ICoreServerAPI coreServerAPI = this.Api as ICoreServerAPI;
						if (coreServerAPI != null)
						{
							coreServerAPI.Network.BroadcastBlockEntityPacket(this.Pos, BlockEntityLucerne.PacketIdSyncState, syncData);
						}
					}
				}
				return;
			}
			if (packetid == BlockEntityLucerne.PacketIdInventory && data != null && data.Length != 0)
			{
				try
				{
					TreeAttribute tree2 = new TreeAttribute();
					using (MemoryStream ms2 = new MemoryStream(data))
					{
						tree2.FromBytes(new BinaryReader(ms2));
					}
					this._inventory.FromTreeAttributes(tree2);
					this._inventory.ResolveBlocksOrItems();
					this.MarkDirty(false, null);
				}
				catch
				{
				}
				return;
			}
			base.OnReceivedClientPacket(fromPlayer, packetid, data);
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00030464 File Offset: 0x0002E664
		public override void OnReceivedServerPacket(int packetid, byte[] data)
		{
			ICoreClientAPI capi = this.Api as ICoreClientAPI;
			if (capi == null)
			{
				return;
			}
			if (packetid == BlockEntityLucerne.PacketIdSyncState)
			{
				TreeAttribute syncTree = new TreeAttribute();
				using (MemoryStream ms = new MemoryStream(data))
				{
					syncTree.FromBytes(new BinaryReader(ms));
				}
				this.FromTreeAttributes(syncTree, capi.World);
				return;
			}
			if (packetid != 5000)
			{
				return;
			}
			TreeAttribute tree = new TreeAttribute();
			string dialogTitle;
			using (MemoryStream ms2 = new MemoryStream(data))
			{
				BinaryReader reader = new BinaryReader(ms2);
				reader.ReadString();
				dialogTitle = reader.ReadString();
				tree.FromBytes(reader);
			}
			if (this._inventory == null)
			{
				this._inventory = new InventoryLucerne(this.Api, this.Pos);
				this.LateInitInventory();
			}
			this._inventory.FromTreeAttributes(tree);
			this._inventory.ResolveBlocksOrItems();
			new WarningBeaconDialog(dialogTitle, this._inventory, this.Pos, capi, this, delegate
			{
				this.MarkDirty(true, null);
			}).TryOpen();
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00030584 File Offset: 0x0002E784
		public bool TryLightBeacon()
		{
			if (this.IsFired)
			{
				ICoreAPI api = this.Api;
				if (api != null && api.Side == 1 && this._structureComplete)
				{
					if (this._inventory.FatSlot.StackSize < 2 || this._inventory.FirewoodSlot.StackSize < 32)
					{
						return false;
					}
					this._inventory.FatSlot.TakeOut(2);
					this._inventory.FirewoodSlot.TakeOut(32);
					this.MarkDirty(false, null);
					this._burnEndTotalHours = this.Api.World.Calendar.TotalHours + 24.0;
					this.MarkDirty(true, null);
					BEBehaviorLightable lightBhv = base.GetBehavior<BEBehaviorLightable>();
					if (lightBhv != null)
					{
						lightBhv.SetLightActive(true);
					}
					this._showAshes = false;
					return true;
				}
			}
			return false;
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0003065C File Offset: 0x0002E85C
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			if (this._inventory != null)
			{
				TreeAttribute invTree = new TreeAttribute();
				this._inventory.ToTreeAttributes(invTree);
				tree["inventory"] = invTree;
			}
			tree.SetBool("showGuide", this._showStructureGuide);
			tree.SetBool("structureComplete", this._structureComplete);
			tree.SetDouble("burnEndTotalHours", this._burnEndTotalHours);
			tree.SetBool("showAshes", this._showAshes);
			BEBehaviorLightable lightBhv = base.GetBehavior<BEBehaviorLightable>();
			tree.SetBool("beaconLightActive", lightBhv != null && lightBhv.IsLightActive);
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x000306F8 File Offset: 0x0002E8F8
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			if (this.Api == null)
			{
				this.Api = worldForResolving.Api;
			}
			if (this._inventory == null)
			{
				this._inventory = new InventoryLucerne(worldForResolving.Api, this.Pos);
			}
			base.FromTreeAttributes(tree, worldForResolving);
			if (tree["inventory"] != null)
			{
				this._inventory.FromTreeAttributes(tree["inventory"] as TreeAttribute);
				this._inventory.ResolveBlocksOrItems();
			}
			if (this._inventory != null)
			{
				this.LateInitInventory();
			}
			this._showStructureGuide = tree.GetBool("showGuide", false);
			this._structureComplete = tree.GetBool("structureComplete", false);
			this._burnEndTotalHours = tree.GetDouble("burnEndTotalHours", 0.0);
			this._showAshes = tree.GetBool("showAshes", false);
			bool beaconLightActive = tree.GetBool("beaconLightActive", false);
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 2)
			{
				BEBehaviorLightable lightBhv = base.GetBehavior<BEBehaviorLightable>();
				if (lightBhv != null)
				{
					lightBhv.SetLightActive(beaconLightActive);
				}
				ICoreClientAPI capi = this.Api as ICoreClientAPI;
				if (capi != null)
				{
					capi.Event.EnqueueMainThreadTask(delegate()
					{
						this.UpdateStructureGuideHighlight();
						this.UpdateBonfireRenderer();
					}, "rpvoicechat:UpdateBeaconRenderers");
				}
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x00030831 File Offset: 0x0002EA31
		public bool ShowAshes
		{
			get
			{
				return this._showAshes;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x0003083C File Offset: 0x0002EA3C
		public bool HasEnoughFirewoodForBonfire
		{
			get
			{
				InventoryLucerne inventory = this._inventory;
				int? num;
				if (inventory == null)
				{
					num = default(int?);
				}
				else
				{
					SlotRestrictedItemPath firewoodSlot = inventory.FirewoodSlot;
					num = ((firewoodSlot != null) ? new int?(firewoodSlot.StackSize) : default(int?));
				}
				int? num2 = num;
				return num2.GetValueOrDefault() >= 32;
			}
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x0003088C File Offset: 0x0002EA8C
		private void UpdateStructureGuideHighlight()
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			ICoreClientAPI capi = this.Api as ICoreClientAPI;
			if (((capi != null) ? capi.World : null) == null)
			{
				return;
			}
			IClientPlayer player = capi.World.Player;
			if (player == null)
			{
				return;
			}
			if (!this._showStructureGuide || !this.IsFired || this._structureComplete)
			{
				capi.World.HighlightBlocks(player, 1380996675, BlockEntityLucerne.EmptyBlockPosList, 0, 0);
				return;
			}
			BlockFacing facing = this.GetFacing();
			List<BlockPos> blocks = new List<BlockPos>(WarningBeaconStructure.StructurePositions.Count);
			foreach (BlockPos local in WarningBeaconStructure.StructurePositions)
			{
				blocks.Add(WarningBeaconStructure.LocalToWorld(this.Pos, local.X, local.Y, local.Z, facing));
			}
			capi.World.HighlightBlocks(player, 1380996675, blocks, 0, 0);
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x000309A0 File Offset: 0x0002EBA0
		private void UpdateBonfireRenderer()
		{
			ICoreAPI api = this.Api;
			if (api == null || api.Side != 2)
			{
				return;
			}
			ICoreClientAPI capi = this.Api as ICoreClientAPI;
			if (capi == null)
			{
				return;
			}
			bool shouldShow = this.IsFired && this._structureComplete && (this._showAshes || this.HasEnoughFirewoodForBonfire || this.IsBurning);
			if (shouldShow && this._bonfireRenderer == null)
			{
				this._bonfireRenderer = new WarningBeaconBonfireRenderer(this, capi);
				return;
			}
			if (!shouldShow && this._bonfireRenderer != null)
			{
				this._bonfireRenderer.Dispose();
				this._bonfireRenderer = null;
				return;
			}
			if (this._bonfireRenderer != null)
			{
				this._bonfireRenderer.UpdateState();
			}
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00030A4F File Offset: 0x0002EC4F
		public void RefreshBonfireRenderer()
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 2)
			{
				this.UpdateBonfireRenderer();
			}
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00030A6E File Offset: 0x0002EC6E
		public override void OnBlockBroken(IPlayer byPlayer)
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1 && this.IsFired)
			{
				WarningBeaconStructure.UnregisterLucerne(this.Pos);
			}
			this.DisposeClientRenderers();
			base.OnBlockBroken(byPlayer);
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00030AA7 File Offset: 0x0002ECA7
		public override void OnBlockUnloaded()
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1 && this.IsFired)
			{
				WarningBeaconStructure.UnregisterLucerne(this.Pos);
			}
			this.DisposeClientRenderers();
			base.OnBlockUnloaded();
		}

		// Token: 0x060008C0 RID: 2240 RVA: 0x00030AE0 File Offset: 0x0002ECE0
		private void DisposeClientRenderers()
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 2 && this._flameParticlesTickListenerId != 0L)
			{
				ICoreClientAPI coreClientAPI = this.Api as ICoreClientAPI;
				if (coreClientAPI != null)
				{
					coreClientAPI.Event.UnregisterGameTickListener(this._flameParticlesTickListenerId);
				}
				this._flameParticlesTickListenerId = 0L;
			}
			ICoreClientAPI capi = this.Api as ICoreClientAPI;
			if (capi != null)
			{
				IClientWorldAccessor world = capi.World;
				if (((world != null) ? world.Player : null) != null)
				{
					capi.World.HighlightBlocks(capi.World.Player, 1380996675, BlockEntityLucerne.EmptyBlockPosList, 0, 0);
				}
			}
			WarningBeaconBonfireRenderer bonfireRenderer = this._bonfireRenderer;
			if (bonfireRenderer != null)
			{
				bonfireRenderer.Dispose();
			}
			this._bonfireRenderer = null;
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00030B94 File Offset: 0x0002ED94
		private void GetFlameSpawnBounds(out Vec3d minPos, out Vec3d addPos)
		{
			BlockPos center = this.GetStructureCenterWorldPos();
			minPos = new Vec3d((double)center.X - 0.5 - 0.5, (double)center.Y - 0.5, (double)center.Z - 0.5 - 0.5);
			addPos = new Vec3d(3.0, 1.5, 3.0);
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x00030C18 File Offset: 0x0002EE18
		private void OnClientGameTickFlames(float dt)
		{
			if (this.IsFired && this.IsBurning && this._structureComplete)
			{
				ICoreAPI api = this.Api;
				if (((api != null) ? api.World : null) != null)
				{
					Vec3d minPos;
					Vec3d addPos;
					this.GetFlameSpawnBounds(out minPos, out addPos);
					Random rand = this.Api.World.Rand;
					BlockEntityLucerne._fireParticles.MinPos = minPos;
					BlockEntityLucerne._fireParticles.AddPos = addPos;
					this.Api.World.SpawnParticles(BlockEntityLucerne._fireParticles, null);
					if (rand.NextDouble() < 0.14)
					{
						BlockEntityLucerne._smokeParticles.MinPos = minPos;
						BlockEntityLucerne._smokeParticles.AddPos = addPos;
						BlockEntityLucerne._smokeParticles.MinQuantity = 1f;
						BlockEntityLucerne._smokeParticles.AddQuantity = 1f;
						this.Api.World.SpawnParticles(BlockEntityLucerne._smokeParticles, null);
						BlockEntityLucerne._smokeParticles.MinQuantity = 0f;
						BlockEntityLucerne._smokeParticles.AddQuantity = 1f;
					}
					if (rand.NextDouble() < 0.04)
					{
						BlockEntityLucerne._emberParticles.MinPos = minPos;
						BlockEntityLucerne._emberParticles.AddPos = addPos;
						BlockEntityLucerne._emberParticles.MinQuantity = 1f;
						BlockEntityLucerne._emberParticles.AddQuantity = 1f;
						this.Api.World.SpawnParticles(BlockEntityLucerne._emberParticles, null);
						BlockEntityLucerne._emberParticles.MinQuantity = 0f;
						BlockEntityLucerne._emberParticles.AddQuantity = 1f;
					}
					return;
				}
			}
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00030D8C File Offset: 0x0002EF8C
		private void OnServerGameTick(float dt)
		{
			this.RevalidateStructure();
			if (!this.IsBurning)
			{
				BEBehaviorLightable lightBhv = base.GetBehavior<BEBehaviorLightable>();
				if (lightBhv != null && lightBhv.IsLightActive)
				{
					lightBhv.SetLightActive(false);
					this._showAshes = true;
					this.MarkDirty(true, null);
				}
				if (this._showAshes)
				{
					InventoryLucerne inventory = this._inventory;
					int? num;
					if (inventory == null)
					{
						num = default(int?);
					}
					else
					{
						SlotRestrictedItemPath firewoodSlot = inventory.FirewoodSlot;
						num = ((firewoodSlot != null) ? new int?(firewoodSlot.StackSize) : default(int?));
					}
					int? num2 = num;
					if (num2.GetValueOrDefault() >= 32)
					{
						this._showAshes = false;
						this.MarkDirty(true, null);
					}
				}
				return;
			}
			if (!this.IsFired || !this._structureComplete || base.Block == null)
			{
				return;
			}
			Vec3d center = this.GetStructureCenter3x3WorldPos();
			foreach (Entity entity in this.Api.World.GetEntitiesAround(center, 1.5f, 1.5f, null))
			{
				if (entity != null && entity.Alive && this.Api.World.Rand.NextDouble() < 0.05)
				{
					entity.ReceiveDamage(new DamageSource
					{
						Source = 0,
						SourceBlock = base.Block,
						Type = 1,
						SourcePos = center
					}, 0.5f);
				}
			}
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00030EDB File Offset: 0x0002F0DB
		public float GetHeatStrength(IWorldAccessor world, BlockPos heatSourcePos, BlockPos heatReceiverPos)
		{
			if (!this.IsFired || !this.IsBurning)
			{
				return 0f;
			}
			return 3f;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00030EF8 File Offset: 0x0002F0F8
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
		{
			if (!this.IsFired)
			{
				return;
			}
			if (this._structureComplete)
			{
				if (!this.IsBurning)
				{
					dsc.AppendLine(Lang.Get(RPVoiceChatMod.modID + ":WarningBeacon.Ready", Array.Empty<object>()));
					return;
				}
			}
			else
			{
				dsc.AppendLine(Lang.Get(RPVoiceChatMod.modID + ":WarningBeacon.StructureIncomplete", Array.Empty<object>()));
			}
		}

		// Token: 0x040003BB RID: 955
		private InventoryLucerne _inventory;

		// Token: 0x040003BC RID: 956
		private bool _showStructureGuide;

		// Token: 0x040003BD RID: 957
		private bool _structureComplete;

		// Token: 0x040003BE RID: 958
		private double _burnEndTotalHours;

		// Token: 0x040003BF RID: 959
		private bool _showAshes;

		// Token: 0x040003C0 RID: 960
		private const int StructureGuideHighlightSlotId = 1380996675;

		// Token: 0x040003C1 RID: 961
		private static readonly List<BlockPos> EmptyBlockPosList = new List<BlockPos>();

		// Token: 0x040003C2 RID: 962
		private WarningBeaconBonfireRenderer _bonfireRenderer;

		// Token: 0x040003C3 RID: 963
		private long _flameParticlesTickListenerId;

		// Token: 0x040003C4 RID: 964
		private static SimpleParticleProperties _fireParticles;

		// Token: 0x040003C5 RID: 965
		private static SimpleParticleProperties _smokeParticles;

		// Token: 0x040003C6 RID: 966
		private static SimpleParticleProperties _emberParticles;

		// Token: 0x040003C7 RID: 967
		public const double BurnDurationGameHours = 24.0;

		// Token: 0x040003C8 RID: 968
		public const int FatRequired = 2;

		// Token: 0x040003C9 RID: 969
		public const int FirewoodRequired = 32;

		// Token: 0x040003CA RID: 970
		public static readonly int PacketIdLightBeacon = BlockEntityLucerne.GetCustomPacketIdBase();

		// Token: 0x040003CB RID: 971
		public static readonly int PacketIdSyncState = BlockEntityLucerne.GetCustomPacketIdBase() + 1;

		// Token: 0x040003CC RID: 972
		public static readonly int PacketIdInventory = BlockEntityLucerne.GetCustomPacketIdBase() + 2;
	}
}
