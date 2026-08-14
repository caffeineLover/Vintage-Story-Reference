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
	// Token: 0x020000BA RID: 186
	public class BlockEntityLucerne : BlockEntityOpenableContainer, IBlockEntityWithCustomLightPosition, IHeatSource
	{
		// Token: 0x06000688 RID: 1672 RVA: 0x00024698 File Offset: 0x00022898
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

		// Token: 0x06000689 RID: 1673 RVA: 0x0002498C File Offset: 0x00022B8C
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

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x00024A2C File Offset: 0x00022C2C
		public override InventoryBase Inventory
		{
			get
			{
				return this._inventory;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x00024A34 File Offset: 0x00022C34
		public override string InventoryClassName
		{
			get
			{
				return "lucerne";
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x00024A3B File Offset: 0x00022C3B
		public bool ShowStructureGuide
		{
			get
			{
				return this._showStructureGuide;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x00024A43 File Offset: 0x00022C43
		public bool StructureComplete
		{
			get
			{
				return this._structureComplete;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x00024A4B File Offset: 0x00022C4B
		public bool IsFired
		{
			get
			{
				Block block = base.Block;
				return ((block != null) ? block.Variant["materialtype"] : null) == "fired";
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x00024A74 File Offset: 0x00022C74
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

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x00024AC0 File Offset: 0x00022CC0
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

		// Token: 0x06000692 RID: 1682 RVA: 0x00024B08 File Offset: 0x00022D08
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

		// Token: 0x06000693 RID: 1683 RVA: 0x00024BDC File Offset: 0x00022DDC
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

		// Token: 0x06000694 RID: 1684 RVA: 0x00024C4A File Offset: 0x00022E4A
		public override void OnBlockPlaced(ItemStack byItemStack = null)
		{
			base.OnBlockPlaced(byItemStack);
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 1 && this.IsFired)
			{
				this.RevalidateStructure();
			}
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00024C78 File Offset: 0x00022E78
		public void ToggleStructureGuide(IPlayer byPlayer)
		{
			this._showStructureGuide = !this._showStructureGuide;
			this.MarkDirty(true, null);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00024C94 File Offset: 0x00022E94
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

		// Token: 0x06000697 RID: 1687 RVA: 0x00024CFC File Offset: 0x00022EFC
		public BlockPos GetStructureCenterWorldPos()
		{
			BlockPos c = WarningBeaconStructure.CenterLocal;
			return WarningBeaconStructure.LocalToWorld(this.Pos, c.X, c.Y, c.Z, this.GetFacing());
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00024D32 File Offset: 0x00022F32
		public Vec3d GetStructureCenter3x3WorldPos()
		{
			return this.GetStructureCenterWorldPos().ToWorldCenter();
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00024D3F File Offset: 0x00022F3F
		Vec3d IBlockEntityWithCustomLightPosition.GetLightOrigin()
		{
			return this.GetStructureCenter3x3WorldPos();
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00024D48 File Offset: 0x00022F48
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

		// Token: 0x0600069B RID: 1691 RVA: 0x00024E60 File Offset: 0x00023060
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

		// Token: 0x0600069C RID: 1692 RVA: 0x00024F44 File Offset: 0x00023144
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

		// Token: 0x0600069D RID: 1693 RVA: 0x0002504C File Offset: 0x0002324C
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

		// Token: 0x0600069E RID: 1694 RVA: 0x0002516C File Offset: 0x0002336C
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

		// Token: 0x0600069F RID: 1695 RVA: 0x00025244 File Offset: 0x00023444
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

		// Token: 0x060006A0 RID: 1696 RVA: 0x000252E0 File Offset: 0x000234E0
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

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x00025419 File Offset: 0x00023619
		public bool ShowAshes
		{
			get
			{
				return this._showAshes;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x00025424 File Offset: 0x00023624
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

		// Token: 0x060006A3 RID: 1699 RVA: 0x00025474 File Offset: 0x00023674
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

		// Token: 0x060006A4 RID: 1700 RVA: 0x00025588 File Offset: 0x00023788
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

		// Token: 0x060006A5 RID: 1701 RVA: 0x00025637 File Offset: 0x00023837
		public void RefreshBonfireRenderer()
		{
			ICoreAPI api = this.Api;
			if (api != null && api.Side == 2)
			{
				this.UpdateBonfireRenderer();
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00025656 File Offset: 0x00023856
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

		// Token: 0x060006A7 RID: 1703 RVA: 0x0002568F File Offset: 0x0002388F
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

		// Token: 0x060006A8 RID: 1704 RVA: 0x000256C8 File Offset: 0x000238C8
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

		// Token: 0x060006A9 RID: 1705 RVA: 0x0002577C File Offset: 0x0002397C
		private void GetFlameSpawnBounds(out Vec3d minPos, out Vec3d addPos)
		{
			BlockPos center = this.GetStructureCenterWorldPos();
			minPos = new Vec3d((double)center.X - 0.5 - 0.5, (double)center.Y - 0.5, (double)center.Z - 0.5 - 0.5);
			addPos = new Vec3d(3.0, 1.5, 3.0);
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00025800 File Offset: 0x00023A00
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

		// Token: 0x060006AB RID: 1707 RVA: 0x00025974 File Offset: 0x00023B74
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

		// Token: 0x060006AC RID: 1708 RVA: 0x00025AC3 File Offset: 0x00023CC3
		public float GetHeatStrength(IWorldAccessor world, BlockPos heatSourcePos, BlockPos heatReceiverPos)
		{
			if (!this.IsFired || !this.IsBurning)
			{
				return 0f;
			}
			return 3f;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00025AE0 File Offset: 0x00023CE0
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

		// Token: 0x040002DC RID: 732
		private InventoryLucerne _inventory;

		// Token: 0x040002DD RID: 733
		private bool _showStructureGuide;

		// Token: 0x040002DE RID: 734
		private bool _structureComplete;

		// Token: 0x040002DF RID: 735
		private double _burnEndTotalHours;

		// Token: 0x040002E0 RID: 736
		private bool _showAshes;

		// Token: 0x040002E1 RID: 737
		private const int StructureGuideHighlightSlotId = 1380996675;

		// Token: 0x040002E2 RID: 738
		private static readonly List<BlockPos> EmptyBlockPosList = new List<BlockPos>();

		// Token: 0x040002E3 RID: 739
		private WarningBeaconBonfireRenderer _bonfireRenderer;

		// Token: 0x040002E4 RID: 740
		private long _flameParticlesTickListenerId;

		// Token: 0x040002E5 RID: 741
		private static SimpleParticleProperties _fireParticles;

		// Token: 0x040002E6 RID: 742
		private static SimpleParticleProperties _smokeParticles;

		// Token: 0x040002E7 RID: 743
		private static SimpleParticleProperties _emberParticles;

		// Token: 0x040002E8 RID: 744
		public const double BurnDurationGameHours = 24.0;

		// Token: 0x040002E9 RID: 745
		public const int FatRequired = 2;

		// Token: 0x040002EA RID: 746
		public const int FirewoodRequired = 32;

		// Token: 0x040002EB RID: 747
		public static readonly int PacketIdLightBeacon = BlockEntityLucerne.GetCustomPacketIdBase();

		// Token: 0x040002EC RID: 748
		public static readonly int PacketIdSyncState = BlockEntityLucerne.GetCustomPacketIdBase() + 1;

		// Token: 0x040002ED RID: 749
		public static readonly int PacketIdInventory = BlockEntityLucerne.GetCustomPacketIdBase() + 2;
	}
}
