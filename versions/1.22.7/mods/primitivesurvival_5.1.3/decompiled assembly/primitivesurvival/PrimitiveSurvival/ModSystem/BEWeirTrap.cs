using System;
using System.Linq;
using System.Text;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000015 RID: 21
	public class BEWeirTrap : BlockEntityDisplayCase, ITexPositionSource
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00010199 File Offset: 0x0000E399
		public override string InventoryClassName
		{
			get
			{
				return "weirtrap";
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000143 RID: 323 RVA: 0x000101A0 File Offset: 0x0000E3A0
		public override InventoryBase Inventory
		{
			get
			{
				return this.inventory;
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000101A8 File Offset: 0x0000E3A8
		public BEWeirTrap()
		{
			this.inventory = new InventoryGeneric(this.maxSlots, null, null, null);
			new MeshData[this.maxSlots];
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00010446 File Offset: 0x0000E646
		public ItemSlot Catch1Slot
		{
			get
			{
				return this.inventory[0];
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00010454 File Offset: 0x0000E654
		public ItemSlot Catch2Slot
		{
			get
			{
				return this.inventory[1];
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00010462 File Offset: 0x0000E662
		// (set) Token: 0x06000148 RID: 328 RVA: 0x00010475 File Offset: 0x0000E675
		public ItemStack Catch1Stack
		{
			get
			{
				return this.inventory[0].Itemstack;
			}
			set
			{
				this.inventory[0].Itemstack = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00010489 File Offset: 0x0000E689
		// (set) Token: 0x0600014A RID: 330 RVA: 0x0001049C File Offset: 0x0000E69C
		public ItemStack Catch2Stack
		{
			get
			{
				return this.inventory[1].Itemstack;
			}
			set
			{
				this.inventory[1].Itemstack = value;
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000104B0 File Offset: 0x0000E6B0
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			if (EnumAppSideExtensions.IsServer(api.Side))
			{
				this.particleTick = this.RegisterGameTickListener(new Action<float>(this.ParticleUpdate), this.tickSeconds * 1000, 0);
				this.RegisterGameTickListener(new Action<float>(this.WeirTrapUpdate), (int)(this.updateMinutes * 60000.0), 0);
			}
			this.wetPickupSound = new AssetLocation("game", "sounds/environment/smallsplash");
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00010530 File Offset: 0x0000E730
		public override void OnBlockUnloaded()
		{
			base.OnBlockUnloaded();
			this.UnregisterGameTickListener(this.particleTick);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00010544 File Offset: 0x0000E744
		private void GenerateWaterParticles(int slot, string type, BlockPos pos, IWorldAccessor world)
		{
			string dir = (this.Api.World.BlockAccessor.GetBlock(this.Pos, 0) as BlockWeirTrap).LastCodePart(0);
			float num = 1f;
			float maxQuantity = 8f;
			if (type == "seashell")
			{
				maxQuantity = 2f;
			}
			int color = ColorUtil.ToRgba(40, 125, 185, 255);
			Vec3d minPos = new Vec3d();
			Vec3d addPos = new Vec3d();
			Vec3f minVelocity = new Vec3f(0.2f, 0.2f, 0.2f);
			Vec3f maxVelocity = new Vec3f(0.6f, 0.6f, 0.6f);
			float lifeLength = 5f;
			float gravityEffect = -0.1f;
			float minSize = 0.1f;
			float maxSize = 0.7f;
			SimpleParticleProperties waterParticles = new SimpleParticleProperties(num, maxQuantity, color, minPos, addPos, minVelocity, maxVelocity, lifeLength, gravityEffect, minSize, maxSize, 0);
			if (slot == 0)
			{
				if (type == "fish")
				{
					Vec3f min;
					if (dir == "north")
					{
						min = new Vec3f(1.3f, 0.2f, 1.3f);
					}
					else if (dir == "south")
					{
						min = new Vec3f(-0.3f, 0.2f, -0.3f);
					}
					else if (dir == "east")
					{
						min = new Vec3f(-0.3f, 0.2f, 1.3f);
					}
					else
					{
						min = new Vec3f(1.3f, 0.2f, -0.3f);
					}
					waterParticles.MinPos.Set(pos.ToVec3d().AddCopy(min));
					waterParticles.AddPos.Set(new Vec3d(0.1, 0.0, 0.0));
				}
				else
				{
					Vec3f min2;
					if (dir == "north")
					{
						min2 = new Vec3f(1f, 0.2f, 0.7f);
					}
					else if (dir == "south")
					{
						min2 = new Vec3f(0f, 0.2f, 0.3f);
					}
					else if (dir == "east")
					{
						min2 = new Vec3f(0.3f, 0.2f, 1f);
					}
					else
					{
						min2 = new Vec3f(0.7f, 0.2f, 0f);
					}
					waterParticles.MinPos.Set(pos.ToVec3d().AddCopy(min2));
				}
			}
			else if (type == "fish")
			{
				Vec3f min3;
				if (dir == "north")
				{
					min3 = new Vec3f(-0.3f, 0.2f, 1.3f);
				}
				else if (dir == "south")
				{
					min3 = new Vec3f(1.3f, 0.2f, -0.3f);
				}
				else if (dir == "east")
				{
					min3 = new Vec3f(-0.3f, 0.2f, -0.3f);
				}
				else
				{
					min3 = new Vec3f(1.3f, 0.2f, 1.3f);
				}
				waterParticles.MinPos.Set(pos.ToVec3d().AddCopy(min3));
				waterParticles.AddPos.Set(new Vec3d(0.1, 0.0, 0.0));
			}
			else
			{
				Vec3f min4;
				if (dir == "north")
				{
					min4 = new Vec3f(0f, 0.2f, 0.4f);
				}
				else if (dir == "south")
				{
					min4 = new Vec3f(1f, 0.2f, 0.6f);
				}
				else if (dir == "east")
				{
					min4 = new Vec3f(0.6f, 0.2f, 0f);
				}
				else
				{
					min4 = new Vec3f(0.4f, 0.2f, 1f);
				}
				waterParticles.MinPos.Set(pos.ToVec3d().AddCopy(min4));
			}
			waterParticles.SizeEvolve = new EvolvingNatFloat(4, 0.7f);
			waterParticles.ShouldDieInAir = true;
			waterParticles.SelfPropelled = true;
			world.SpawnParticles(waterParticles, null);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00010950 File Offset: 0x0000EB50
		public void ParticleUpdate(float par)
		{
			int slot = BEWeirTrap.Rnd.Next(3);
			if (slot <= 1 && !this.inventory[slot].Empty)
			{
				if (this.inventory[slot].Itemstack.Item != null)
				{
					this.GenerateWaterParticles(slot, "fish", this.Pos, this.Api.World);
					return;
				}
				if (this.inventory[slot].Itemstack.Block != null)
				{
					this.GenerateWaterParticles(slot, "seashell", this.Pos, this.Api.World);
				}
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000109EC File Offset: 0x0000EBEC
		private void WeirTrapUpdate(float par)
		{
			bool openWater = false;
			foreach (BlockPos neib in new BlockPos[]
			{
				this.Pos.EastCopy(1),
				this.Pos.NorthCopy(1),
				this.Pos.SouthCopy(1),
				this.Pos.WestCopy(1)
			})
			{
				if (this.Api.World.BlockAccessor.GetBlock(neib, 0).Code.Path.Contains("open") && this.Api.World.BlockAccessor.GetBlock(neib, 2).Code.Path.Contains("water"))
				{
					openWater = true;
				}
			}
			Block block = this.Api.World.BlockAccessor.GetBlock(this.Pos, 2);
			if (!block.FirstCodePart(0).Contains("water"))
			{
				openWater = false;
			}
			if (block.FirstCodePart(0).Contains("boiling"))
			{
				openWater = false;
			}
			if (openWater)
			{
				int escaped = BEWeirTrap.Rnd.Next(100);
				if (BEWeirTrap.Rnd.Next(100) < this.catchPercent)
				{
					this.WorldPut(0);
				}
				else if (!this.Catch1Slot.Empty || !this.Catch2Slot.Empty)
				{
					if (escaped < this.escapePercent)
					{
						this.WorldTake(1, this.Pos);
					}
				}
				else if (BEWeirTrap.Rnd.Next(100) < this.catchPercent)
				{
					this.WorldPut(1);
				}
				else if (escaped < this.escapePercent)
				{
					this.WorldTake(0, this.Pos);
				}
				if (!this.Catch1Slot.Empty && this.Catch1Stack.Item != null && this.Catch1Stack.Item.Code.Path == "rot")
				{
					escaped = BEWeirTrap.Rnd.Next(100);
					if (escaped < this.rotRemovedPercent)
					{
						this.WorldTake(0, this.Pos);
					}
				}
				if (!this.Catch2Slot.Empty && this.Catch2Stack.Item != null && this.Catch2Stack.Item.Code.Path == "rot")
				{
					escaped = BEWeirTrap.Rnd.Next(100);
					if (escaped < this.rotRemovedPercent)
					{
						this.WorldTake(1, this.Pos);
					}
				}
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00010C50 File Offset: 0x0000EE50
		internal void OnBreak()
		{
			for (int index = 1; index >= 0; index--)
			{
				if (!this.inventory[index].Empty)
				{
					if (this.inventory[index].Itemstack.Collectible.FirstCodePart(0).Contains("waterfish"))
					{
						string fishType = this.inventory[index].Itemstack.Collectible.FirstCodePart(1) + "-" + this.inventory[index].Itemstack.Collectible.FirstCodePart(2);
						if (fishType == "mahi-mahi")
						{
							fishType = fishType + "-" + this.inventory[index].Itemstack.Collectible.FirstCodePart(3);
						}
						AssetLocation fishAsset = new AssetLocation("game:creature-fish-freshwater-" + fishType + "-adult");
						Item newItem = this.Api.World.GetItem(new AssetLocation(fishAsset));
						if (newItem == null)
						{
							fishAsset = new AssetLocation("game:creature-fish-saltwater-" + fishType + "-adult");
							newItem = this.Api.World.GetItem(new AssetLocation(fishAsset));
						}
						if (newItem != null)
						{
							ItemStack newstack = new ItemStack(newItem, 1);
							if (newstack != null)
							{
								this.Api.World.SpawnItemEntity(newstack, this.Pos.ToVec3d().Add(0.5, 0.5, 0.5), null);
							}
						}
						this.inventory[index].TakeOut(1);
					}
					else
					{
						ItemStack stack = this.inventory[index].TakeOut(1);
						if (stack.StackSize > 0)
						{
							this.Api.World.SpawnItemEntity(stack, this.Pos.ToVec3d().Add(0.5, 0.5, 0.5), null);
						}
					}
					this.MarkDirty(true, null);
				}
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00010E58 File Offset: 0x0000F058
		public bool WorldPut(int slot)
		{
			ItemStack newStack = null;
			if (slot == 0 || slot == 1)
			{
				if (BEWeirTrap.Rnd.Next(10) < 1)
				{
					Block shellBlock = this.Api.World.GetBlock(new AssetLocation("game:seashell-" + this.shellStates[BEWeirTrap.Rnd.Next(Enumerable.Count<string>(this.shellStates))] + "-" + this.shellColors[BEWeirTrap.Rnd.Next(Enumerable.Count<string>(this.shellColors))]));
					if (shellBlock != null)
					{
						newStack = new ItemStack(shellBlock, 1);
					}
				}
				else if (this.Api.World.BlockAccessor.GetBlock(this.Pos, 2).Code.Path.Contains("saltwater"))
				{
					Item fishItem = this.Api.World.GetItem(new AssetLocation("primitivesurvival:pssaltwaterfish-" + this.saltWaterFishTypes[BEWeirTrap.Rnd.Next(Enumerable.Count<string>(this.saltWaterFishTypes))] + "-raw"));
					if (fishItem != null)
					{
						newStack = new ItemStack(fishItem, 1);
					}
				}
				else
				{
					Item fishItem2 = this.Api.World.GetItem(new AssetLocation("primitivesurvival:psfreshwaterfish-" + this.freshWaterFishTypes[BEWeirTrap.Rnd.Next(Enumerable.Count<string>(this.freshWaterFishTypes))] + "-raw"));
					if (fishItem2 != null)
					{
						newStack = new ItemStack(fishItem2, 1);
					}
				}
			}
			if (newStack == null)
			{
				return false;
			}
			if (this.inventory[slot].Empty)
			{
				if (newStack.Collectible.Code.Path.Contains("psfreshwaterfish") || newStack.Collectible.Code.Path.Contains("pssaltwaterfish"))
				{
					int rate = PrimitiveSurvivalSystem.FishDepletedPercent(this.Api as ICoreServerAPI, this.Pos);
					if (BEWeirTrap.Rnd.Next(100) < rate)
					{
						return false;
					}
					PrimitiveSurvivalSystem.UpdateChunkInDictionary(this.Api as ICoreServerAPI, this.Pos, ModConfig.Loaded.FishChunkDepletionRate);
				}
				this.inventory[slot].Itemstack = newStack;
				this.MarkDirty(true, null);
				return true;
			}
			return false;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00011078 File Offset: 0x0000F278
		public bool WorldTake(int slot, BlockPos pos)
		{
			if (!this.inventory[slot].Empty)
			{
				if (this.inventory[slot].Itemstack.Collectible.Code.Path.Contains("psfreshwaterfish") || this.inventory[slot].Itemstack.Collectible.Code.Path.Contains("pssaltwaterfish"))
				{
					PrimitiveSurvivalSystem.UpdateChunkInDictionary(this.Api as ICoreServerAPI, this.Pos, -ModConfig.Loaded.FishChunkDepletionRate);
				}
				this.inventory[slot].TakeOut(1);
				this.Api.World.BlockAccessor.MarkBlockDirty(pos, null);
				this.MarkDirty(false, null);
				return true;
			}
			return false;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0001114C File Offset: 0x0000F34C
		internal bool OnInteract(IPlayer byPlayer, BlockSelection blockSel)
		{
			ItemSlot playerSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
			if (playerSlot.Empty)
			{
				if (this.TryTake(byPlayer))
				{
					this.Api.World.PlaySoundAt(this.wetPickupSound, (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, null, true, 32f, 1f);
					return true;
				}
				return false;
			}
			else
			{
				if (playerSlot.Itemstack.Collectible == null)
				{
					return false;
				}
				if (this.TryPut(playerSlot))
				{
					this.Api.World.PlaySoundAt(this.wetPickupSound, (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, null, true, 32f, 1f);
					return true;
				}
				return false;
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00011224 File Offset: 0x0000F424
		private bool TryPut(ItemSlot playerSlot)
		{
			int index = -1;
			ItemStack playerStack = playerSlot.Itemstack;
			if (this.inventory != null)
			{
				if (Enumerable.Count<ItemStack>(Enumerable.ToArray<ItemStack>(Enumerable.Select<ItemSlot, ItemStack>(Enumerable.Where<ItemSlot>(this.inventory, (ItemSlot slot) => !slot.Empty), (ItemSlot slot) => slot.Itemstack))) >= this.maxSlots)
				{
					return false;
				}
			}
			if (playerStack.Item != null)
			{
				if (playerStack.Item.Code.Path.Contains("psfreshwaterfish") || playerStack.Item.Code.Path.Contains("pssaltwaterfish"))
				{
					if (this.Catch1Slot.Empty)
					{
						index = 0;
					}
					else if (this.Catch2Slot.Empty)
					{
						index = 1;
					}
				}
			}
			else if (playerStack.Block != null && playerStack.Block.Code.Path.Contains("seashell"))
			{
				if (this.Catch1Slot.Empty)
				{
					index = 0;
				}
				else if (this.Catch2Slot.Empty)
				{
					index = 1;
				}
			}
			if (index > -1)
			{
				int moved = playerSlot.TryPutInto(this.Api.World, this.inventory[index], 1);
				if (moved > 0)
				{
					this.MarkDirty(true, null);
					return moved > 0;
				}
			}
			return false;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00011384 File Offset: 0x0000F584
		private bool TryTake(IPlayer byPlayer)
		{
			if (!this.Catch2Slot.Empty)
			{
				string fishType = this.Catch2Stack.Collectible.FirstCodePart(1) + "-" + this.Catch2Stack.Collectible.FirstCodePart(2);
				if (fishType == "mahi-mahi")
				{
					fishType = fishType + "-" + this.Catch2Stack.Collectible.FirstCodePart(3);
				}
				AssetLocation fishAsset = new AssetLocation("game:creature-fish-freshwater-" + fishType + "-adult");
				Item newItem = byPlayer.Entity.World.GetItem(new AssetLocation(fishAsset));
				if (newItem == null)
				{
					fishAsset = new AssetLocation("game:creature-fish-saltwater-" + fishType + "-adult");
					newItem = byPlayer.Entity.World.GetItem(new AssetLocation(fishAsset));
				}
				if (newItem != null)
				{
					ItemStack stack = new ItemStack(newItem, 1);
					byPlayer.InventoryManager.TryGiveItemstack(stack, false);
					this.Catch2Slot.TakeOut(1);
					this.MarkDirty(true, null);
					return true;
				}
				byPlayer.InventoryManager.TryGiveItemstack(this.Catch2Stack, false);
				this.Catch2Slot.TakeOut(1);
				this.MarkDirty(true, null);
				return true;
			}
			else
			{
				if (this.Catch1Slot.Empty)
				{
					return false;
				}
				string fishType2 = this.Catch1Stack.Collectible.FirstCodePart(1) + "-" + this.Catch1Stack.Collectible.FirstCodePart(2);
				if (fishType2 == "mahi-mahi")
				{
					fishType2 = fishType2 + "-" + this.Catch1Stack.Collectible.FirstCodePart(3);
				}
				AssetLocation fishAsset2 = new AssetLocation("game:creature-fish-freshwater-" + fishType2 + "-adult");
				Item newItem2 = byPlayer.Entity.World.GetItem(new AssetLocation(fishAsset2));
				if (newItem2 == null)
				{
					fishAsset2 = new AssetLocation("game:creature-fish-saltwater-" + fishType2 + "-adult");
					newItem2 = byPlayer.Entity.World.GetItem(new AssetLocation(fishAsset2));
				}
				if (newItem2 != null)
				{
					ItemStack stack2 = new ItemStack(newItem2, 1);
					byPlayer.InventoryManager.TryGiveItemstack(stack2, false);
					this.Catch1Slot.TakeOut(1);
					this.MarkDirty(true, null);
					return true;
				}
				byPlayer.InventoryManager.TryGiveItemstack(this.Catch1Stack, false);
				this.Catch1Slot.TakeOut(1);
				this.MarkDirty(true, null);
				return true;
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000115F4 File Offset: 0x0000F7F4
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb)
		{
			bool rot = false;
			if (!this.Catch1Slot.Empty || !this.Catch2Slot.Empty)
			{
				sb.Append(Lang.Get("primitivesurvival:blockdesc-weirtrap-catch", Array.Empty<object>()));
				if (!this.Catch1Slot.Empty)
				{
					if (this.Catch1Stack.Block == null && !this.Catch1Stack.Item.Code.Path.Contains("psfreshwaterfish") && !this.Catch1Stack.Item.Code.Path.Contains("pssaltwaterfish"))
					{
						sb.Append(" " + Lang.Get("primitivesurvival:blockdesc-fishbasket-catch-rotten", Array.Empty<object>()));
					}
				}
				else if (!this.Catch2Slot.Empty && !rot && this.Catch2Stack.Block == null && !this.Catch2Stack.Item.Code.Path.Contains("psfreshwaterfish") && !this.Catch2Stack.Item.Code.Path.Contains("pssaltwaterfish"))
				{
					sb.Append(" " + Lang.Get("primitivesurvival:blockdesc-fishbasket-catch-rotten", Array.Empty<object>()));
				}
				sb.AppendLine().AppendLine();
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00011748 File Offset: 0x0000F948
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolve)
		{
			base.FromTreeAttributes(tree, worldForResolve);
			if (this.Api != null && this.Api.Side == 2)
			{
				this.Api.World.BlockAccessor.MarkBlockDirty(this.Pos, null);
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00011784 File Offset: 0x0000F984
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			BlockWeirTrap block = this.Api.World.BlockAccessor.GetBlock(this.Pos, 0) as BlockWeirTrap;
			ITexPositionSource tmpTextureSource = tesselator.GetTextureSource(block, 0, false);
			if (this.inventory != null)
			{
				for (int i = 0; i <= 1; i++)
				{
					bool alive = false;
					if (!this.inventory[i].Empty)
					{
						string shapePath;
						if (this.inventory[i].Itemstack.Block != null)
						{
							shapePath = "game:shapes/block/aquatic/seashell/" + this.inventory[i].Itemstack.Block.FirstCodePart(1);
							tmpTextureSource = tesselator.GetTextureSource(this.inventory[i].Itemstack.Block, 0, false);
						}
						else if (this.inventory[i].Itemstack.Item.Code.Path.Contains("psfreshwaterfish"))
						{
							string fishcode = this.inventory[i].Itemstack.Item.LastCodePart(2).ToString();
							shapePath = "game:shapes/entity/animal/fish/freshwater/" + fishcode + "-adult";
							string tmpBlockCode = "freshwaterfishoverride-" + fishcode + "-" + this.inventory[i].Itemstack.Item.LastCodePart(1).ToString();
							Block tmpBlock = this.Api.World.GetBlock(block.CodeWithPath(tmpBlockCode));
							tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tmpBlock, 0, false);
							alive = true;
						}
						else if (this.inventory[i].Itemstack.Item.Code.Path.Contains("pssaltwaterfish"))
						{
							string fishcode2 = this.inventory[i].Itemstack.Item.LastCodePart(2).ToString();
							if (fishcode2 == "mahi")
							{
								fishcode2 = "mahi-mahi";
							}
							shapePath = "game:shapes/entity/animal/fish/saltwater/" + fishcode2 + "-adult";
							string tmpBlockCode2 = "saltwaterfishoverride-" + fishcode2 + "-" + this.inventory[i].Itemstack.Item.LastCodePart(1).ToString();
							Block tmpBlock = this.Api.World.GetBlock(block.CodeWithPath(tmpBlockCode2));
							tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tmpBlock, 0, false);
							alive = true;
						}
						else
						{
							Block tmpBlock = this.Api.World.GetBlock(block.CodeWithPath("texturerot"));
							tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tmpBlock, 0, false);
							shapePath = "primitivesurvival:shapes/item/fishing/fish-pike";
						}
						if (shapePath != "")
						{
							MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapePath, tmpTextureSource, i, alive);
							mesher.AddMeshData(mesh, 1);
						}
					}
				}
			}
			return true;
		}

		// Token: 0x040000B0 RID: 176
		private readonly int catchPercent = ModConfig.Loaded.WeirTrapCatchPercent;

		// Token: 0x040000B1 RID: 177
		private readonly int escapePercent = ModConfig.Loaded.WeirTrapEscapePercent;

		// Token: 0x040000B2 RID: 178
		private readonly double updateMinutes = ModConfig.Loaded.WeirTrapUpdateMinutes;

		// Token: 0x040000B3 RID: 179
		private readonly int rotRemovedPercent = ModConfig.Loaded.WeirTrapRotRemovedPercent;

		// Token: 0x040000B4 RID: 180
		private readonly int tickSeconds = 3;

		// Token: 0x040000B5 RID: 181
		private readonly int maxSlots = 2;

		// Token: 0x040000B6 RID: 182
		private readonly string[] saltWaterFishTypes = new string[]
		{
			"bream-sea",
			"gurnard-cape",
			"haddock-common",
			"hake-silver",
			"herring-atlantic",
			"mackerel-atlantic",
			"pollock-alaska",
			"perch-pacific",
			"barracuda-great",
			"grouper-black",
			"salmon-pink",
			"snapper-red",
			"tuna-skipjack",
			"wolf-bering",
			"amberjack-yellowtail",
			"mahi-mahi-common",
			"wreckfish-atlantic",
			"coelacanth-common",
			"sturgeon-atlantic"
		};

		// Token: 0x040000B7 RID: 183
		private readonly string[] freshWaterFishTypes = new string[]
		{
			"alewife-shad",
			"chub-river",
			"crappie-black",
			"crappie-white",
			"perch-european",
			"perch-yellow",
			"piranha-black",
			"piranha-red",
			"trout-brown",
			"trout-rainbow",
			"bass-largemouth",
			"bass-smallmouth",
			"carp-common",
			"carp-grass",
			"catfish-blue",
			"catfish-channel",
			"pickerel-chain",
			"salmon-coho",
			"tilapia-nile",
			"tilapia-red",
			"walleye-common",
			"pike-northern",
			"arapaima-arapaima",
			"arapaima-gigas",
			"sheatfish-black",
			"sheatfish-white"
		};

		// Token: 0x040000B8 RID: 184
		private readonly string[] shellStates = new string[]
		{
			"scallop",
			"sundial",
			"turritella",
			"clam",
			"conch",
			"seastar",
			"volute"
		};

		// Token: 0x040000B9 RID: 185
		private readonly string[] shellColors = new string[]
		{
			"latte",
			"plain",
			"seafoam",
			"darkpurple",
			"cinnamon",
			"turquoise"
		};

		// Token: 0x040000BA RID: 186
		private static readonly Random Rnd = new Random();

		// Token: 0x040000BB RID: 187
		private long particleTick;

		// Token: 0x040000BC RID: 188
		protected InventoryGeneric inventory;

		// Token: 0x040000BD RID: 189
		private AssetLocation wetPickupSound;
	}
}
