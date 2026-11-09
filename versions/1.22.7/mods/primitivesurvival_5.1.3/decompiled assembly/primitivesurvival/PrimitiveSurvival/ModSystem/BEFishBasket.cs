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
	// Token: 0x0200000B RID: 11
	public class BEFishBasket : BlockEntityDisplayCase, ITexPositionSource
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00006358 File Offset: 0x00004558
		public override string InventoryClassName
		{
			get
			{
				return "fishbasket";
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000069 RID: 105 RVA: 0x0000635F File Offset: 0x0000455F
		public override InventoryBase Inventory
		{
			get
			{
				return this.inventory;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00006368 File Offset: 0x00004568
		public BEFishBasket()
		{
			this.inventory = new InventoryGeneric(this.maxSlots, null, null, null);
			new MeshData[this.maxSlots];
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600006B RID: 107 RVA: 0x000066D5 File Offset: 0x000048D5
		public ItemSlot BaitSlot
		{
			get
			{
				return this.inventory[0];
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600006C RID: 108 RVA: 0x000066E3 File Offset: 0x000048E3
		public ItemSlot Catch1Slot
		{
			get
			{
				return this.inventory[1];
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600006D RID: 109 RVA: 0x000066F1 File Offset: 0x000048F1
		public ItemSlot Catch2Slot
		{
			get
			{
				return this.inventory[2];
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600006E RID: 110 RVA: 0x000066FF File Offset: 0x000048FF
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00006712 File Offset: 0x00004912
		public ItemStack BaitStack
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

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00006726 File Offset: 0x00004926
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00006739 File Offset: 0x00004939
		public ItemStack Catch1Stack
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

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000072 RID: 114 RVA: 0x0000674D File Offset: 0x0000494D
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00006760 File Offset: 0x00004960
		public ItemStack Catch2Stack
		{
			get
			{
				return this.inventory[2].Itemstack;
			}
			set
			{
				this.inventory[2].Itemstack = value;
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00006774 File Offset: 0x00004974
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			if (EnumAppSideExtensions.IsServer(api.Side))
			{
				this.particleTick = this.RegisterGameTickListener(new Action<float>(this.ParticleUpdate), this.tickSeconds * 1000, 0);
				this.RegisterGameTickListener(new Action<float>(this.FishBasketUpdate), (int)(this.updateMinutes * 60000.0), 0);
			}
			this.wetPickupSound = new AssetLocation("game", "sounds/environment/smallsplash");
			this.dryPickupSound = new AssetLocation("game", "sounds/block/cloth");
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00006809 File Offset: 0x00004A09
		public override void OnBlockUnloaded()
		{
			base.OnBlockUnloaded();
			this.UnregisterGameTickListener(this.particleTick);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00006820 File Offset: 0x00004A20
		private void GenerateWaterParticles(string type, BlockPos pos, IWorldAccessor world)
		{
			float num = 1f;
			float maxQuantity = 8f;
			if (type == "seashell")
			{
				maxQuantity = 2f;
			}
			int color = ColorUtil.ToRgba(40, 125, 185, 255);
			Vec3d minPos = new Vec3d();
			Vec3d addPos = new Vec3d();
			Vec3f minVelocity = new Vec3f(0.2f, 0f, 0.2f);
			Vec3f maxVelocity = new Vec3f(0.6f, 0.4f, 0.6f);
			float lifeLength = 5f;
			float gravityEffect = -0.1f;
			float minSize = 0.1f;
			float maxSize = 0.5f;
			SimpleParticleProperties waterParticles = new SimpleParticleProperties(num, maxQuantity, color, minPos, addPos, minVelocity, maxVelocity, lifeLength, gravityEffect, minSize, maxSize, 0);
			if (type == "fish")
			{
				waterParticles.MinPos.Set(pos.ToVec3d().AddCopy(0.5, 0.2, 0.5));
				waterParticles.AddPos.Set(new Vec3d(0.1, 0.0, 0.0));
			}
			else
			{
				waterParticles.MinPos.Set(pos.ToVec3d().AddCopy(0.5, 0.2, 0.3));
			}
			waterParticles.SizeEvolve = new EvolvingNatFloat(4, 0.5f);
			waterParticles.ShouldDieInAir = true;
			waterParticles.SelfPropelled = true;
			world.SpawnParticles(waterParticles, null);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00006998 File Offset: 0x00004B98
		public void ParticleUpdate(float par)
		{
			if (base.Block.Code.Path.Contains("inwater") && BEFishBasket.Rnd.Next(2) == 0)
			{
				for (int slot = 1; slot <= 2; slot++)
				{
					if (!this.inventory[slot].Empty)
					{
						if (this.inventory[slot].Itemstack.Item != null)
						{
							this.GenerateWaterParticles("fish", this.Pos, this.Api.World);
						}
						else if (this.inventory[slot].Itemstack.Block != null)
						{
							this.GenerateWaterParticles("seashell", this.Pos, this.Api.World);
						}
					}
				}
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00006A64 File Offset: 0x00004C64
		internal void OnBreak()
		{
			for (int index = 2; index >= 0; index--)
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

		// Token: 0x06000079 RID: 121 RVA: 0x00006C6C File Offset: 0x00004E6C
		public void FishBasketUpdate(float par)
		{
			bool waterAllAround = true;
			foreach (BlockPos neib in new BlockPos[]
			{
				this.Pos.EastCopy(1),
				this.Pos.NorthCopy(1),
				this.Pos.SouthCopy(1),
				this.Pos.WestCopy(1)
			})
			{
				if (!this.Api.World.BlockAccessor.GetBlock(neib, 0).Code.Path.Contains("water"))
				{
					waterAllAround = false;
				}
			}
			if (!this.Api.World.BlockAccessor.GetBlock(this.Pos, 2).FirstCodePart(0).Contains("water"))
			{
				waterAllAround = false;
			}
			RegistryObject block = this.Api.World.BlockAccessor.GetBlock(this.Pos, 0);
			BlockPos testPos = this.Pos.EastCopy(1);
			string basketFacing = block.LastCodePart(0);
			if (basketFacing == "north")
			{
				testPos = this.Pos.SouthCopy(1);
			}
			else if (basketFacing == "south")
			{
				testPos = this.Pos.NorthCopy(1);
			}
			else if (basketFacing == "east")
			{
				testPos = this.Pos.WestCopy(1);
			}
			Block block2 = this.Api.World.BlockAccessor.GetBlock(testPos, 2);
			if (!block2.FirstCodePart(0).Contains("water"))
			{
				waterAllAround = false;
			}
			if (block2.FirstCodePart(0).Contains("boiling"))
			{
				waterAllAround = false;
			}
			if (waterAllAround)
			{
				int caught = BEFishBasket.Rnd.Next(100);
				if (!this.BaitSlot.Empty)
				{
					int rando = BEFishBasket.Rnd.Next(100);
					if (rando < this.baitStolenPercent && this.WorldTake(0, this.Pos))
					{
						this.GenerateWaterParticles("fish", this.Pos, this.Api.World);
					}
				}
				bool caughtOk = false;
				if ((!this.BaitSlot.Empty && caught < this.baitedCatchPercent) || (this.BaitSlot.Empty && caught < this.catchPercent))
				{
					int rando = BEFishBasket.Rnd.Next(2);
					caughtOk = this.WorldPut(rando + 1);
					if (!caughtOk)
					{
						rando = 1 - rando;
						caughtOk = this.WorldPut(rando + 1);
					}
				}
				if (!caughtOk && (!this.Catch1Slot.Empty || !this.Catch2Slot.Empty) && BEFishBasket.Rnd.Next(100) < this.escapePercent)
				{
					int rando = BEFishBasket.Rnd.Next(2);
					bool escapedOk = this.WorldTake(rando + 1, this.Pos);
					if (!escapedOk)
					{
						rando = 1 - rando;
						escapedOk = this.WorldTake(rando + 1, this.Pos);
					}
					if (escapedOk)
					{
						this.GenerateWaterParticles("fish", this.Pos, this.Api.World);
					}
				}
				if (!this.Catch1Slot.Empty && this.Catch1Stack.Item != null && this.Catch1Stack.Item.Code.Path == "rot" && BEFishBasket.Rnd.Next(100) < this.rotRemovedPercent)
				{
					this.WorldTake(1, this.Pos);
				}
				if (!this.Catch2Slot.Empty && this.Catch2Stack.Item != null && this.Catch2Stack.Item.Code.Path == "rot" && BEFishBasket.Rnd.Next(100) < this.rotRemovedPercent)
				{
					this.WorldTake(2, this.Pos);
				}
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00007000 File Offset: 0x00005200
		public bool WorldPut(int slot)
		{
			ItemStack newStack = null;
			if (BEFishBasket.Rnd.Next(5) < 1)
			{
				Block shellBlock = this.Api.World.GetBlock(new AssetLocation("game:seashell-" + this.shellStates[BEFishBasket.Rnd.Next(Enumerable.Count<string>(this.shellStates))] + "-" + this.shellColors[BEFishBasket.Rnd.Next(Enumerable.Count<string>(this.shellColors))]));
				if (shellBlock != null)
				{
					newStack = new ItemStack(shellBlock, 1);
				}
			}
			else if (this.Api.World.BlockAccessor.GetBlock(this.Pos, 2).Code.Path.Contains("saltwater"))
			{
				Item fishItem = this.Api.World.GetItem(new AssetLocation("primitivesurvival:pssaltwaterfish-" + this.saltWaterFishTypes[BEFishBasket.Rnd.Next(Enumerable.Count<string>(this.saltWaterFishTypes))] + "-raw"));
				if (fishItem != null)
				{
					newStack = new ItemStack(fishItem, 1);
				}
			}
			else
			{
				Item fishItem2 = this.Api.World.GetItem(new AssetLocation("primitivesurvival:psfreshwaterfish-" + this.freshWaterFishTypes[BEFishBasket.Rnd.Next(Enumerable.Count<string>(this.freshWaterFishTypes))] + "-raw"));
				if (fishItem2 != null)
				{
					newStack = new ItemStack(fishItem2, 1);
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
					if (BEFishBasket.Rnd.Next(100) < rate)
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

		// Token: 0x0600007B RID: 123 RVA: 0x00007214 File Offset: 0x00005414
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

		// Token: 0x0600007C RID: 124 RVA: 0x000072E8 File Offset: 0x000054E8
		public bool OnInteract(IPlayer byPlayer, BlockSelection blockSel)
		{
			ItemSlot slot = byPlayer.InventoryManager.ActiveHotbarSlot;
			if (slot.Empty)
			{
				if (this.TryTake(byPlayer))
				{
					if (base.Block.Code.Path.Contains("inwater"))
					{
						this.Api.World.PlaySoundAt(this.wetPickupSound, (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, null, true, 32f, 1f);
					}
					else
					{
						this.Api.World.PlaySoundAt(this.dryPickupSound, (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, null, true, 32f, 1f);
					}
					return true;
				}
				return false;
			}
			else
			{
				CollectibleObject colObj = slot.Itemstack.Collectible;
				if (colObj == null)
				{
					string text;
					if (colObj == null)
					{
						text = null;
					}
					else
					{
						AssetLocation code = colObj.Code;
						text = ((code != null) ? code.Path : null);
					}
					if (!(text == "insect-grub"))
					{
						return false;
					}
				}
				if (this.TryPut(slot))
				{
					if (base.Block.Code.Path.Contains("inwater"))
					{
						this.Api.World.PlaySoundAt(this.wetPickupSound, (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, null, true, 32f, 1f);
					}
					else
					{
						this.Api.World.PlaySoundAt(this.dryPickupSound, (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, null, true, 32f, 1f);
					}
					return true;
				}
				return false;
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000074B8 File Offset: 0x000056B8
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
				if ((Array.IndexOf<string>(this.baitTypes, playerStack.Item.FirstCodePart(0)) >= 0 || playerStack.Item.Code.Path.Contains("creature-earthworm")) && this.BaitSlot.Empty)
				{
					index = 0;
				}
				else if (playerStack.Item.Code.Path.Contains("psfreshwaterfish") || playerStack.Item.Code.Path.Contains("pssaltwaterfish"))
				{
					if (this.Catch1Slot.Empty)
					{
						index = 1;
					}
					else if (this.Catch2Slot.Empty)
					{
						index = 2;
					}
				}
			}
			else if (playerStack.Block != null)
			{
				if (Array.IndexOf<string>(this.baitTypes, playerStack.Block.FirstCodePart(0)) >= 0 && this.BaitSlot.Empty)
				{
					index = 0;
				}
				else if (playerStack.Block.Code.Path.Contains("seashell"))
				{
					if (this.Catch1Slot.Empty)
					{
						index = 1;
					}
					else if (this.Catch2Slot.Empty)
					{
						index = 2;
					}
				}
			}
			if (index >= 0)
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

		// Token: 0x0600007E RID: 126 RVA: 0x00007694 File Offset: 0x00005894
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
			else if (!this.Catch1Slot.Empty)
			{
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
			else
			{
				if (!this.BaitSlot.Empty)
				{
					byPlayer.InventoryManager.TryGiveItemstack(this.BaitStack, false);
					this.BaitSlot.TakeOut(1);
					this.MarkDirty(true, null);
					return true;
				}
				return false;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000793C File Offset: 0x00005B3C
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb)
		{
			if (this.Api.World.BlockAccessor.GetBlock(this.Pos, 0).Code.Path.Contains("inwater"))
			{
				bool rot = false;
				if (!this.Catch1Slot.Empty || !this.Catch2Slot.Empty)
				{
					sb.Append(Lang.Get("primitivesurvival:blockdesc-fishbasket-catch", Array.Empty<object>()));
					if (!this.Catch1Slot.Empty)
					{
						if (this.Catch1Stack.Block == null && !this.Catch1Stack.Item.Code.Path.Contains("psfreshwaterfish") && !this.Catch1Stack.Item.Code.Path.Contains("pssaltwaterfish"))
						{
							sb.Append(" " + Lang.Get("primitivesurvival:blockdesc-fishbasket-catch-rotten", Array.Empty<object>()));
						}
						rot = true;
					}
					if (!this.Catch2Slot.Empty && !rot && this.Catch2Stack.Block == null && !this.Catch2Stack.Item.Code.Path.Contains("psfreshwaterfish") && !this.Catch2Stack.Item.Code.Path.Contains("pssaltwaterfish"))
					{
						sb.Append(" " + Lang.Get("primitivesurvival:blockdesc-fishbasket-catch-rotten", Array.Empty<object>()));
					}
					sb.AppendLine().AppendLine();
					return;
				}
				if (!this.BaitSlot.Empty)
				{
					if (this.BaitStack.Item != null)
					{
						if (Array.IndexOf<string>(this.baitTypes, this.BaitStack.Item.FirstCodePart(0)) < 0 && !this.BaitStack.Item.Code.Path.Contains("creature-earthworm"))
						{
							sb.Append(Lang.Get("primitivesurvival:blockdesc-deadfall-bait-rotten", Array.Empty<object>()));
						}
						else
						{
							sb.Append(Lang.Get("primitivesurvival:blockdesc-deadfall-bait-ok", Array.Empty<object>()));
						}
					}
					else if (this.BaitStack.Block != null)
					{
						if (Array.IndexOf<string>(this.baitTypes, this.BaitStack.Block.FirstCodePart(0)) < 0)
						{
							sb.Append(Lang.Get("primitivesurvival:blockdesc-deadfall-bait-rotten", Array.Empty<object>()));
						}
						else
						{
							sb.Append(Lang.Get("primitivesurvival:blockdesc-deadfall-bait-ok", Array.Empty<object>()));
						}
					}
				}
				else if (this.BaitSlot.Empty)
				{
					sb.Append(Lang.Get("primitivesurvival:blockdesc-deadfall-bait-needed", Array.Empty<object>()));
				}
				sb.AppendLine().AppendLine();
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00007BD8 File Offset: 0x00005DD8
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolve)
		{
			base.FromTreeAttributes(tree, worldForResolve);
			if (this.Api != null && this.Api.Side == 2)
			{
				this.Api.World.BlockAccessor.MarkBlockDirty(this.Pos, null);
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00007C14 File Offset: 0x00005E14
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			bool alive = false;
			BlockFishBasket block = this.Api.World.BlockAccessor.GetBlock(this.Pos, 0) as BlockFishBasket;
			ITexPositionSource texture = tesselator.GetTextureSource(block, 0, false);
			ITexPositionSource tmpTextureSource = texture;
			string shapePath = "primitivesurvival:shapes/block/fishbasket/fishbasket";
			MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapePath, texture, -1, alive, tesselator);
			mesher.AddMeshData(mesh, 1);
			if (this.inventory != null)
			{
				if (!this.BaitSlot.Empty)
				{
					if (this.BaitStack.Item != null)
					{
						if (Array.IndexOf<string>(this.baitTypes, this.BaitStack.Item.FirstCodePart(0)) < 0 && !this.BaitStack.Item.Code.Path.Contains("creature-earthworm"))
						{
							if (Array.IndexOf<string>(this.baitTypes, this.BaitStack.Item.FirstCodePart(0)) < 0)
							{
								Block tempblock = this.Api.World.GetBlock(block.CodeWithPath("texturerot"));
								tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tempblock, 0, false);
							}
							else
							{
								tmpTextureSource = texture;
							}
						}
					}
					else if (this.BaitStack.Block != null)
					{
						if (Array.IndexOf<string>(this.baitTypes, this.BaitStack.Block.FirstCodePart(0)) < 0)
						{
							Block tempblock2 = this.Api.World.GetBlock(block.CodeWithPath("texturerot"));
							tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tempblock2, 0, false);
						}
						else
						{
							tmpTextureSource = texture;
						}
					}
					shapePath = "primitivesurvival:shapes/item/fishing/hookbait";
					mesh = block.GenMesh(this.Api as ICoreClientAPI, shapePath, tmpTextureSource, 0, alive, tesselator);
					mesher.AddMeshData(mesh, 1);
				}
				for (int i = 1; i <= 2; i++)
				{
					alive = false;
					if (!this.inventory[i].Empty)
					{
						if (this.inventory[i].Itemstack.Block != null)
						{
							shapePath = "game:shapes/block/aquatic/seashell/" + this.inventory[i].Itemstack.Block.FirstCodePart(1);
							tmpTextureSource = tesselator.GetTextureSource(this.inventory[i].Itemstack.Block, 0, false);
						}
						else if (this.inventory[i].Itemstack.Item.Code.Path.Contains("gear"))
						{
							string gearType = this.inventory[i].Itemstack.Item.FirstCodePart(1);
							Block tmpBlock = this.Api.World.GetBlock(block.CodeWithPath("texture" + gearType));
							if (gearType != "rusty")
							{
								gearType = "temporal";
							}
							shapePath = "game:shapes/item/gear-" + gearType;
							tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tmpBlock, 0, false);
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
							mesh = block.GenMesh(this.Api as ICoreClientAPI, shapePath, tmpTextureSource, i, alive, tesselator);
							mesher.AddMeshData(mesh, 1);
						}
					}
				}
			}
			return true;
		}

		// Token: 0x04000038 RID: 56
		private readonly int catchPercent = ModConfig.Loaded.FishBasketCatchPercent;

		// Token: 0x04000039 RID: 57
		private readonly int baitedCatchPercent = ModConfig.Loaded.FishBasketBaitedCatchPercent;

		// Token: 0x0400003A RID: 58
		private readonly int baitStolenPercent = ModConfig.Loaded.FishBasketBaitStolenPercent;

		// Token: 0x0400003B RID: 59
		private readonly int escapePercent = ModConfig.Loaded.FishBasketEscapePercent;

		// Token: 0x0400003C RID: 60
		private readonly double updateMinutes = ModConfig.Loaded.FishBasketUpdateMinutes;

		// Token: 0x0400003D RID: 61
		private readonly int rotRemovedPercent = ModConfig.Loaded.FishBasketRotRemovedPercent;

		// Token: 0x0400003E RID: 62
		private readonly int tickSeconds = 4;

		// Token: 0x0400003F RID: 63
		private readonly int maxSlots = 3;

		// Token: 0x04000040 RID: 64
		private readonly string[] baitTypes = new string[]
		{
			"fruit",
			"grain",
			"legume",
			"meat",
			"vegetable",
			"jerky",
			"mushroom",
			"bread",
			"poultry",
			"pickledvegetable",
			"redmeat",
			"bushmeat",
			"nightcrawler",
			"cheese",
			"fisheggs",
			"fisheggscooked",
			"insect",
			"fish",
			"fishingbait"
		};

		// Token: 0x04000041 RID: 65
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

		// Token: 0x04000042 RID: 66
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

		// Token: 0x04000043 RID: 67
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

		// Token: 0x04000044 RID: 68
		private readonly string[] shellColors = new string[]
		{
			"latte",
			"plain",
			"seafoam",
			"darkpurple",
			"cinnamon",
			"turquoise"
		};

		// Token: 0x04000045 RID: 69
		protected static readonly Random Rnd = new Random();

		// Token: 0x04000046 RID: 70
		private long particleTick;

		// Token: 0x04000047 RID: 71
		protected InventoryGeneric inventory;

		// Token: 0x04000048 RID: 72
		private AssetLocation wetPickupSound;

		// Token: 0x04000049 RID: 73
		private AssetLocation dryPickupSound;
	}
}
