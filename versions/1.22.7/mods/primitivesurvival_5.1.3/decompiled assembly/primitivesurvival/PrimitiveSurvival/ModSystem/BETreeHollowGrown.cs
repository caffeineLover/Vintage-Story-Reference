using System;
using System.Text;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Common;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000013 RID: 19
	public class BETreeHollowGrown : BlockEntityDisplayCase, ITexPositionSource
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600011B RID: 283 RVA: 0x0000ECE1 File Offset: 0x0000CEE1
		public override string InventoryClassName
		{
			get
			{
				return "treehollowgrown";
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600011C RID: 284 RVA: 0x0000ECE8 File Offset: 0x0000CEE8
		public override InventoryBase Inventory
		{
			get
			{
				return this.inventory;
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000ECF0 File Offset: 0x0000CEF0
		public BETreeHollowGrown()
		{
			this.inventory = new InventoryGeneric(this.maxSlots, null, null, null);
			new MeshData[this.maxSlots];
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000ED5C File Offset: 0x0000CF5C
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			if (EnumAppSideExtensions.IsServer(api.Side))
			{
				this.RegisterGameTickListener(new Action<float>(this.TreeHollowUpdate), 20000, 0);
			}
			this.irlDayLengthMinutes = (double)(((GameCalendar)this.Api.World.Calendar).DayLengthInRealLifeSeconds / 60f);
			if (this.lastEmptied == 0.0)
			{
				this.lastEmptied = this.Api.World.Calendar.TotalHours;
				this.MarkDirty(false, null);
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000EDF4 File Offset: 0x0000CFF4
		private BlockPos RelativeToSpawn(BlockPos pos)
		{
			BlockPos worldSpawn = this.Api.World.DefaultSpawnPosition.XYZ.AsBlockPos;
			BlockPos blockPos = pos.SubCopy(worldSpawn);
			return new BlockPos(blockPos.X, pos.Y, blockPos.Z, 0);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000EE3C File Offset: 0x0000D03C
		public void TreeHollowUpdate(float par)
		{
			if (this.inventory.Empty)
			{
				double gameCurrentHours = this.Api.World.Calendar.TotalHours;
				double updateEveryGameHours = ((this.updateMinutes > 0.0) ? this.updateMinutes : 360.0) / this.irlDayLengthMinutes * 24.0;
				if (this.lastEmptied <= 0.0)
				{
					this.lastEmptied = gameCurrentHours;
					this.MarkDirty(false, null);
					return;
				}
				double gameHoursPassed = gameCurrentHours - this.lastEmptied;
				if (updateEveryGameHours - gameHoursPassed < 0.0)
				{
					try
					{
						TreeHollows uf = new TreeHollows();
						uf.AddItemStacks(this, uf.MakeItemStacks(base.Block, this.Api as ICoreServerAPI));
						this.MarkDirty(false, null);
					}
					catch
					{
					}
					this.lastEmptied = this.Api.World.Calendar.TotalHours;
					this.MarkDirty(false, null);
				}
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000EF40 File Offset: 0x0000D140
		internal bool OnInteract(IPlayer byPlayer, BlockSelection blockSel)
		{
			if (EnumAppSideExtensions.IsServer(this.Api.Side))
			{
				this.UnregisterGameTickListener(this.updateTick);
				this.updateTick = this.RegisterGameTickListener(new Action<float>(this.TreeHollowUpdate), (int)(this.updateMinutes * 60000.0), 0);
			}
			ItemSlot playerSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
			if (playerSlot.Empty)
			{
				return this.TryTake(byPlayer);
			}
			if (!this.devToolsEnabled || playerSlot.Itemstack.Collectible.Attributes == null)
			{
				return false;
			}
			if (this.TryPut(playerSlot, blockSel))
			{
				SoundAttributes sound = base.Block.Sounds.Place;
				this.Api.World.PlaySoundAt(sound, (double)this.Pos.X, (double)this.Pos.Y, (double)this.Pos.Z, 0, null, 1f);
				return true;
			}
			return false;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000F030 File Offset: 0x0000D230
		internal void OnBreak()
		{
			for (int index = this.maxSlots - 1; index >= 0; index--)
			{
				if (!this.inventory[index].Empty)
				{
					ItemStack stack = this.inventory[index].TakeOut(1);
					if (stack.StackSize > 0)
					{
						this.Api.World.SpawnItemEntity(stack, this.Pos.ToVec3d().Add(0.5, 0.5, 0.5), null);
					}
					this.MarkDirty(true, null);
				}
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000F0C8 File Offset: 0x0000D2C8
		private int FirstFreeSlot()
		{
			int slot = 0;
			bool found = false;
			do
			{
				if (this.inventory[slot].Empty)
				{
					found = true;
				}
				else
				{
					slot++;
				}
			}
			while (slot < this.maxSlots && !found);
			if (!found)
			{
				slot = -1;
			}
			return slot;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000F108 File Offset: 0x0000D308
		private int LastFilledSlot()
		{
			int slot = this.maxSlots - 1;
			bool found = false;
			do
			{
				if (!this.inventory[slot].Empty)
				{
					found = true;
				}
				else
				{
					slot--;
				}
			}
			while (slot > -1 && !found);
			return slot;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000F144 File Offset: 0x0000D344
		internal CollectibleObject GetDisplayedCollectible()
		{
			int index = this.LastFilledSlot();
			if (index < 0)
			{
				return null;
			}
			return this.inventory[index].Itemstack.Collectible;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000F174 File Offset: 0x0000D374
		private bool TryPut(ItemSlot slot, BlockSelection blockSel)
		{
			int index = this.FirstFreeSlot();
			if (index == -1)
			{
				return false;
			}
			int num = slot.TryPutInto(this.Api.World, this.inventory[index], 1);
			if (num > 0)
			{
				this.MarkDirty(true, null);
			}
			return num > 0;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000F1BC File Offset: 0x0000D3BC
		private bool TryTake(IPlayer byPlayer)
		{
			int index = this.LastFilledSlot();
			if (index == -1)
			{
				return false;
			}
			if (!this.inventory[index].Empty)
			{
				ItemStack stack = this.inventory[index].TakeOut(1);
				if (byPlayer.InventoryManager.TryGiveItemstack(stack, false))
				{
					if (index == 0)
					{
						this.lastEmptied = this.Api.World.Calendar.TotalHours;
						this.MarkDirty(false, null);
					}
					SoundAttributes sound = base.Block.Sounds.Place;
					this.Api.World.PlaySoundAt(sound, (double)this.Pos.X, (double)this.Pos.Y, (double)this.Pos.Z, 0, null, 1f);
				}
				if (stack.StackSize > 0)
				{
					this.Api.World.SpawnItemEntity(stack, this.Pos.ToVec3d().Add(0.5, 0.5, 0.5), null);
				}
				this.MarkDirty(false, null);
				return true;
			}
			return false;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000F2D4 File Offset: 0x0000D4D4
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolve)
		{
			base.FromTreeAttributes(tree, worldForResolve);
			this.lastEmptied = tree.GetDouble("lastEmptied", 0.0);
			if (this.Api != null && this.Api.Side == 2)
			{
				this.Api.World.BlockAccessor.MarkBlockDirty(this.Pos, null);
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000F335 File Offset: 0x0000D535
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetDouble("lastEmptied", this.lastEmptied);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000F350 File Offset: 0x0000D550
		public override void updateMeshes()
		{
			try
			{
				int index = this.LastFilledSlot() + 1;
				if (index != 0)
				{
					for (int slot = 0; slot < index; slot++)
					{
						if (!this.inventory[slot].Empty)
						{
							ItemStack stack = this.inventory[slot].Itemstack;
							bool flag;
							if (stack == null)
							{
								flag = (null != null);
							}
							else
							{
								Item item = stack.Item;
								flag = (((item != null) ? item.Shape : null) != null);
							}
							if (flag && stack.Collectible.GetCollectibleInterface<IWearableStatsSupplier>() != null)
							{
								this.updateMesh(slot);
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000F3E0 File Offset: 0x0000D5E0
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			MeshData mesh = null;
			BlockTreeHollowGrown block = this.Api.World.BlockAccessor.GetBlock(this.Pos, 0) as BlockTreeHollowGrown;
			if (block == null)
			{
				return base.OnTesselation(mesher, tesselator);
			}
			int index = this.LastFilledSlot();
			if (index > -1)
			{
				ItemStack stack = this.inventory[index].Itemstack;
				if (stack != null)
				{
					if (stack.Class == null)
					{
						mesh = this.capi.TesselatorManager.GetDefaultBlockMesh(stack.Block).Clone();
					}
					else
					{
						this.nowTesselatingObj = stack.Item;
						bool flag;
						if (stack == null)
						{
							flag = (null != null);
						}
						else
						{
							Item item = stack.Item;
							flag = (((item != null) ? item.Shape : null) != null);
						}
						if (flag)
						{
							if (((stack != null) ? stack.Item : null) == null)
							{
								goto IL_139;
							}
							try
							{
								this.capi.Tesselator.TesselateItem(stack.Item, ref mesh, this);
								if (mesh != null)
								{
									ArrayExtensions.Fill<short>(mesh.RenderPassesAndExtraBits, 2);
								}
								goto IL_139;
							}
							catch
							{
								goto IL_139;
							}
						}
						string shapeBase = "primitivesurvival:shapes/";
						string shapePath = "block/trapbait";
						ITexPositionSource texture = tesselator.GetTextureSource(block, 0, false);
						mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase + shapePath, texture, tesselator);
						if (mesh != null)
						{
							mesh.Translate(new Vec3f(0f, 0.03f, 0f));
						}
					}
					IL_139:
					if (mesh != null)
					{
						if (block.FirstCodePart(1).Contains("base"))
						{
							mesh.Translate(new Vec3f(0f, 0.03f, 0f));
						}
						else
						{
							mesh.Translate(new Vec3f(0f, 0.23f, 0f));
						}
						BehaviorInTreeHollowTransform behavior = stack.Collectible.GetBehavior<BehaviorInTreeHollowTransform>();
						ModelTransform transform = (behavior != null) ? behavior.Transform : null;
						if (transform != null)
						{
							mesh.ModelTransform(transform);
						}
						if (block.LastCodePart(0) == "north" || block.LastCodePart(0) == "south")
						{
							mesh.Rotate(new Vec3f(0.5f, 0f, 0.5f), 0f, 90f, 0f);
						}
						mesher.AddMeshData(mesh, 1);
					}
				}
			}
			return false;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000F608 File Offset: 0x0000D808
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb)
		{
			int index = this.LastFilledSlot();
			if (index == -1)
			{
				sb.AppendLine(Lang.Get("Empty", Array.Empty<object>()));
			}
			else
			{
				string desc = this.inventory[index].Itemstack.GetName();
				sb.AppendLine(Lang.Get(desc, Array.Empty<object>()));
			}
			sb.AppendLine();
			if (forPlayer != null)
			{
				BlockSelection currentBlockSelection = forPlayer.CurrentBlockSelection;
			}
		}

		// Token: 0x0400009D RID: 157
		private readonly int maxSlots = 8;

		// Token: 0x0400009E RID: 158
		protected InventoryGeneric inventory;

		// Token: 0x0400009F RID: 159
		private double lastEmptied;

		// Token: 0x040000A0 RID: 160
		private double irlDayLengthMinutes = 2880.0;

		// Token: 0x040000A1 RID: 161
		private readonly double updateMinutes = ModConfig.Loaded.TreeHollowsUpdateMinutes;

		// Token: 0x040000A2 RID: 162
		private readonly bool devToolsEnabled = ModConfig.Loaded.TreeHollowsEnableDeveloperTools;

		// Token: 0x040000A3 RID: 163
		private long updateTick;
	}
}
