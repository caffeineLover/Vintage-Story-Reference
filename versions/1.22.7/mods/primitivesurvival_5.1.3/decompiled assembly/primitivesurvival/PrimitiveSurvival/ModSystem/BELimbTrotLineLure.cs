using System;
using System.Linq;
using System.Runtime.CompilerServices;
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
	// Token: 0x02000010 RID: 16
	public class BELimbTrotLineLure : BlockEntityDisplayCase, ITexPositionSource
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000CC RID: 204 RVA: 0x0000AEA7 File Offset: 0x000090A7
		public override string InventoryClassName
		{
			get
			{
				return "limbtrotlinelure";
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000CD RID: 205 RVA: 0x0000AEAE File Offset: 0x000090AE
		public override InventoryBase Inventory
		{
			get
			{
				return this.inventory;
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x0000AEB8 File Offset: 0x000090B8
		public BELimbTrotLineLure()
		{
			this.inventory = new InventoryGeneric(this.maxSlots, null, null, null);
			new MeshData[this.maxSlots];
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000CF RID: 207 RVA: 0x0000B1B5 File Offset: 0x000093B5
		public ItemSlot HookSlot
		{
			get
			{
				return this.inventory[0];
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x0000B1C3 File Offset: 0x000093C3
		public ItemSlot BaitSlot
		{
			get
			{
				return this.inventory[1];
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000B1D1 File Offset: 0x000093D1
		public ItemSlot LureSlot
		{
			get
			{
				return this.inventory[2];
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x0000B1DF File Offset: 0x000093DF
		public ItemSlot CatchSlot
		{
			get
			{
				return this.inventory[3];
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x0000B1ED File Offset: 0x000093ED
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x0000B200 File Offset: 0x00009400
		public ItemStack HookStack
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

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x0000B214 File Offset: 0x00009414
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x0000B227 File Offset: 0x00009427
		public ItemStack BaitStack
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

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x0000B23B File Offset: 0x0000943B
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x0000B24E File Offset: 0x0000944E
		public ItemStack LureStack
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

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x0000B262 File Offset: 0x00009462
		// (set) Token: 0x060000DA RID: 218 RVA: 0x0000B275 File Offset: 0x00009475
		public ItemStack CatchStack
		{
			get
			{
				return this.inventory[3].Itemstack;
			}
			set
			{
				this.inventory[3].Itemstack = value;
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000B28C File Offset: 0x0000948C
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			if (EnumAppSideExtensions.IsServer(api.Side))
			{
				this.particleTick = this.RegisterGameTickListener(new Action<float>(this.ParticleUpdate), this.tickSeconds * 1000, 0);
				this.RegisterGameTickListener(new Action<float>(this.LimbTrotLineUpdate), (int)(this.updateMinutes * 60000.0), 0);
			}
			this.wetPickupSound = new AssetLocation("game", "sounds/environment/smallsplash");
			this.dryPickupSound = new AssetLocation("game", "sounds/block/cloth");
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000B321 File Offset: 0x00009521
		public override void OnBlockUnloaded()
		{
			base.OnBlockUnloaded();
			this.UnregisterGameTickListener(this.particleTick);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000B338 File Offset: 0x00009538
		private void GenerateWaterParticles(BlockPos pos, IWorldAccessor world)
		{
			float num = 1f;
			float maxQuantity = 8f;
			int color = ColorUtil.ToRgba(40, 125, 185, 255);
			Vec3d minPos = new Vec3d();
			Vec3d addPos = new Vec3d();
			Vec3f minVelocity = new Vec3f(0.1f, 0f, 0.1f);
			Vec3f maxVelocity = new Vec3f(0.6f, 0.1f, 0.6f);
			float lifeLength = 1f;
			float gravityEffect = 0f;
			float minSize = 0.1f;
			float maxSize = 0.3f;
			SimpleParticleProperties waterParticles = new SimpleParticleProperties(num, maxQuantity, color, minPos, addPos, minVelocity, maxVelocity, lifeLength, gravityEffect, minSize, maxSize, 0);
			waterParticles.MinPos.Set(pos.ToVec3d().AddCopy(0.5, 0.2, 0.5));
			waterParticles.AddPos.Set(new Vec3d(0.1, -0.6, 0.0));
			waterParticles.SizeEvolve = new EvolvingNatFloat(4, 0.3f);
			waterParticles.ShouldDieInAir = true;
			waterParticles.SelfPropelled = true;
			world.SpawnParticles(waterParticles, null);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000B45C File Offset: 0x0000965C
		public void ParticleUpdate(float par)
		{
			if (!this.CatchSlot.Empty)
			{
				BlockPos belowblock = new BlockPos(this.Pos.X, this.Pos.Y - 1, this.Pos.Z, 0);
				Block belowBlock = this.Api.World.BlockAccessor.GetBlock(belowblock, 0);
				if (belowBlock.Code.Path.Contains("water") && !belowBlock.Code.Path.Contains("inwater") && BELimbTrotLineLure.Rnd.Next(3) == 0)
				{
					this.GenerateWaterParticles(this.Pos, this.Api.World);
				}
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000B510 File Offset: 0x00009710
		private void LimbTrotLineUpdate(float par)
		{
			if (!this.HookSlot.Empty && this.CatchSlot.Empty)
			{
				BlockPos belowblock = new BlockPos(this.Pos.X, this.Pos.Y - 1, this.Pos.Z, 0);
				Block belowBlock = this.Api.World.BlockAccessor.GetBlock(belowblock, 0);
				Block belowBlockLiquid = this.Api.World.BlockAccessor.GetBlock(belowblock, 2);
				if (belowBlock.Code.Path.Contains("water") && !belowBlock.Code.Path.Contains("inwater") && belowBlockLiquid.Code.Path.Contains("water") && !belowBlockLiquid.Code.Path.Contains("boiling"))
				{
					int caught = BELimbTrotLineLure.Rnd.Next(100);
					if (!this.BaitSlot.Empty)
					{
						if (BELimbTrotLineLure.Rnd.Next(100) < this.baitStolenPercent)
						{
							if (!this.BaitSlot.Empty)
							{
								this.BaitSlot.TakeOut(1);
								this.GenerateWaterParticles(this.Pos, this.Api.World);
								this.MarkDirty(false, null);
							}
						}
						else
						{
							int toCatch = this.baitedCatchPercent;
							if (!this.LureSlot.Empty)
							{
								toCatch = this.baitedLuredCatchPercent;
							}
							if (caught < toCatch && this.CatchSlot.Empty)
							{
								int rate = PrimitiveSurvivalSystem.FishDepletedPercent(this.Api as ICoreServerAPI, this.Pos);
								if (BELimbTrotLineLure.Rnd.Next(100) < rate)
								{
									return;
								}
								PrimitiveSurvivalSystem.UpdateChunkInDictionary(this.Api as ICoreServerAPI, this.Pos, ModConfig.Loaded.FishChunkDepletionRate);
								Item caughtItem;
								if (belowBlock.Code.Path.Contains("saltwater"))
								{
									caughtItem = this.Api.World.GetItem(new AssetLocation("primitivesurvival:pssaltwaterfish-" + this.saltWaterFishTypes[BELimbTrotLineLure.Rnd.Next(Enumerable.Count<string>(this.saltWaterFishTypes))] + "-raw"));
								}
								else
								{
									caughtItem = this.Api.World.GetItem(new AssetLocation("primitivesurvival:psfreshwaterfish-" + this.freshWaterFishTypes[BELimbTrotLineLure.Rnd.Next(Enumerable.Count<string>(this.freshWaterFishTypes))] + "-raw"));
								}
								if (caughtItem != null)
								{
									this.CatchStack = new ItemStack(caughtItem, 1);
									if (BELimbTrotLineLure.Rnd.Next(2) == 0)
									{
										this.BaitSlot.TakeOut(1);
									}
									this.MarkDirty(false, null);
								}
							}
						}
					}
					else
					{
						int toCatch2 = this.catchPercent;
						if (!this.LureSlot.Empty)
						{
							toCatch2 = this.luredCatchPercent;
						}
						if (caught < toCatch2 && this.CatchSlot.Empty)
						{
							int rate2 = PrimitiveSurvivalSystem.FishDepletedPercent(this.Api as ICoreServerAPI, this.Pos);
							if (BELimbTrotLineLure.Rnd.Next(100) < rate2)
							{
								return;
							}
							PrimitiveSurvivalSystem.UpdateChunkInDictionary(this.Api as ICoreServerAPI, this.Pos, ModConfig.Loaded.FishChunkDepletionRate);
							Item caughtItem2;
							if (belowBlock.Code.Path.Contains("saltwater"))
							{
								caughtItem2 = this.Api.World.GetItem(new AssetLocation("primitivesurvival:pssaltwaterfish-" + this.saltWaterFishTypes[BELimbTrotLineLure.Rnd.Next(Enumerable.Count<string>(this.saltWaterFishTypes))] + "-raw"));
							}
							else
							{
								caughtItem2 = this.Api.World.GetItem(new AssetLocation("primitivesurvival:psfreshwaterfish-" + this.freshWaterFishTypes[BELimbTrotLineLure.Rnd.Next(Enumerable.Count<string>(this.freshWaterFishTypes))] + "-raw"));
							}
							if (caughtItem2 != null)
							{
								this.CatchStack = new ItemStack(caughtItem2, 1);
								this.MarkDirty(false, null);
							}
						}
					}
				}
			}
			if (!this.CatchSlot.Empty && this.CatchStack.Item.Code.Path == "rot")
			{
				if (BELimbTrotLineLure.Rnd.Next(100) < this.rotRemovedPercent)
				{
					this.CatchSlot.TakeOut(1);
				}
				this.MarkDirty(false, null);
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000B954 File Offset: 0x00009B54
		internal bool OnInteract(IPlayer byPlayer, BlockSelection blockSel)
		{
			ItemSlot playerSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
			BlockPos belowblock = new BlockPos(this.Pos.X, this.Pos.Y - 1, this.Pos.Z, 0);
			Block belowBlock = this.Api.World.BlockAccessor.GetBlock(belowblock, 0);
			if (playerSlot.Empty)
			{
				if (this.TryTake(byPlayer))
				{
					if (belowBlock.Code.Path.Contains("water") && !belowBlock.Code.Path.Contains("inwater"))
					{
						this.Api.World.PlaySoundAt(this.wetPickupSound, (double)blockSel.Position.X, (double)(blockSel.Position.Y - 1), (double)blockSel.Position.Z, byPlayer, true, 32f, 1f);
					}
					else
					{
						this.Api.World.PlaySoundAt(this.dryPickupSound, (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, byPlayer, true, 32f, 1f);
					}
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
					if (belowBlock.Code.Path.Contains("water") && !belowBlock.Code.Path.Contains("inwater"))
					{
						this.Api.World.PlaySoundAt(this.wetPickupSound, (double)blockSel.Position.X, (double)(blockSel.Position.Y - 1), (double)blockSel.Position.Z, byPlayer, true, 32f, 1f);
					}
					else
					{
						this.Api.World.PlaySoundAt(this.dryPickupSound, (double)blockSel.Position.X, (double)blockSel.Position.Y, (double)blockSel.Position.Z, byPlayer, true, 32f, 1f);
					}
					return true;
				}
				return false;
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000BB68 File Offset: 0x00009D68
		internal void OnBreak()
		{
			for (int index = 3; index >= 0; index--)
			{
				if (!this.inventory[index].Empty)
				{
					if (index == 3)
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

		// Token: 0x060000E2 RID: 226 RVA: 0x0000BD4C File Offset: 0x00009F4C
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
				if (playerStack.Item.Code.Path.Contains("fishinghook") && this.HookSlot.Empty)
				{
					if (!this.BaitSlot.Empty || !this.CatchSlot.Empty)
					{
						return false;
					}
					index = 0;
				}
				else if (playerStack.Item.Code.Path.Contains("fishinglure") && this.LureSlot.Empty)
				{
					if (this.HookSlot.Empty)
					{
						return false;
					}
					index = 2;
				}
				else if ((Array.IndexOf<string>(this.baitTypes, playerStack.Item.FirstCodePart(0)) >= 0 || playerStack.Item.Code.Path.Contains("creature-earthworm")) && this.BaitSlot.Empty)
				{
					if (this.HookSlot.Empty || !this.CatchSlot.Empty)
					{
						return false;
					}
					index = 1;
				}
				else if ((playerStack.Item.Code.Path.Contains("psfreshwaterfish") || playerStack.Item.Code.Path.Contains("pssaltwaterfish")) && this.CatchSlot.Empty)
				{
					if (this.HookSlot.Empty)
					{
						return false;
					}
					index = 3;
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
			}
			else if (playerStack.Block != null)
			{
				if (Array.IndexOf<string>(this.baitTypes, playerStack.Block.FirstCodePart(0)) >= 0 && this.BaitSlot.Empty)
				{
					if (this.HookSlot.Empty || !this.CatchSlot.Empty)
					{
						return false;
					}
					index = 1;
				}
				if (index > -1)
				{
					int moved2 = playerSlot.TryPutInto(this.Api.World, this.inventory[index], 1);
					if (moved2 > 0)
					{
						this.MarkDirty(true, null);
						return moved2 > 0;
					}
				}
			}
			return false;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000BFD8 File Offset: 0x0000A1D8
		private bool TryTake(IPlayer byPlayer)
		{
			if (!this.CatchSlot.Empty)
			{
				string fishType = this.CatchStack.Collectible.FirstCodePart(1) + "-" + this.CatchStack.Collectible.FirstCodePart(2);
				if (fishType == "mahi-mahi")
				{
					fishType = fishType + "-" + this.CatchStack.Collectible.FirstCodePart(3);
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
					this.CatchSlot.TakeOut(1);
					this.MarkDirty(true, null);
					return true;
				}
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
				if (!this.LureSlot.Empty)
				{
					byPlayer.InventoryManager.TryGiveItemstack(this.LureStack, false);
					this.LureSlot.TakeOut(1);
					this.MarkDirty(true, null);
					return true;
				}
				if (!this.HookSlot.Empty)
				{
					byPlayer.InventoryManager.TryGiveItemstack(this.HookStack, false);
					this.HookSlot.TakeOut(1);
					this.MarkDirty(true, null);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x0000C18C File Offset: 0x0000A38C
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolve)
		{
			base.FromTreeAttributes(tree, worldForResolve);
			if (this.Api != null && this.Api.Side == 2)
			{
				this.Api.World.BlockAccessor.MarkBlockDirty(this.Pos, null);
			}
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x0000C1C8 File Offset: 0x0000A3C8
		public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb)
		{
			if (!this.CatchSlot.Empty)
			{
				sb.Append(Lang.Get("primitivesurvival:blockdesc-limbtrotlinelure-catch", Array.Empty<object>()));
				if (!this.CatchStack.Item.Code.Path.Contains("psfreshwaterfish") && !this.CatchStack.Item.Code.Path.Contains("pssaltwaterfish"))
				{
					sb.Append(" " + Lang.Get("primitivesurvival:blockdesc-limbtrotlinelure-catch-rotten", Array.Empty<object>()));
				}
			}
			else
			{
				if (!this.HookSlot.Empty)
				{
					string hookmsg = "-";
					string baitmsg = "-";
					string luremsg = "-";
					if (!this.HookSlot.Empty)
					{
						hookmsg = this.HookStack.GetName().Split(RuntimeHelpers.CreateSpan<char>(fieldof(<PrivateImplementationDetails>.14A4CC14B45CCADC5B677EEAB759DCB62BDDE391CEBA9E8E97CFAFDF6C265F082).FieldHandle))[1];
					}
					if (!this.BaitSlot.Empty)
					{
						baitmsg = this.BaitStack.GetName();
					}
					if (!this.LureSlot.Empty)
					{
						luremsg = this.LureStack.GetName().Split(RuntimeHelpers.CreateSpan<char>(fieldof(<PrivateImplementationDetails>.14A4CC14B45CCADC5B677EEAB759DCB62BDDE391CEBA9E8E97CFAFDF6C265F082).FieldHandle))[1];
					}
					sb.AppendLine(Lang.Get("primitivesurvival:blockdesc-limbtrotlinelure-hook-type", Array.Empty<object>()) + ": " + hookmsg);
					sb.AppendLine(Lang.Get("primitivesurvival:blockdesc-limbtrotlinelure-bait-type", Array.Empty<object>()) + ": " + baitmsg);
					sb.AppendLine(Lang.Get("primitivesurvival:blockdesc-limbtrotlinelure-lure-type", Array.Empty<object>()) + ": " + luremsg);
					sb.AppendLine();
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
				else if (this.BaitSlot.Empty && !this.HookSlot.Empty)
				{
					sb.Append(Lang.Get("primitivesurvival:blockdesc-deadfall-bait-needed", Array.Empty<object>()));
				}
				else if (this.HookSlot.Empty)
				{
					sb.Append(Lang.Get("primitivesurvival:blockdesc-limbtrotlinelure-hook-needed", Array.Empty<object>())).AppendLine().AppendLine();
				}
			}
			sb.AppendLine().AppendLine();
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0000C4C0 File Offset: 0x0000A6C0
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			string shapeBase = "primitivesurvival:shapes/";
			bool alive = false;
			BlockLimbTrotLineLure block = this.Api.World.BlockAccessor.GetBlock(this.Pos, 0) as BlockLimbTrotLineLure;
			ITexPositionSource texture = tesselator.GetTextureSource(block, 0, false);
			ITexPositionSource tmpTextureSource = texture;
			if (block.Code.Path.Contains("-middle"))
			{
				string shapePath = "block/limbtrotline/limbtrotline-middle";
				MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase + shapePath, texture, alive);
				mesher.AddMeshData(mesh, 1);
			}
			if (block.Code.Path.Contains("small"))
			{
				string shapePath = "block/limbtrotline/limbtrotline-end-small";
				MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase + shapePath, texture, alive);
				mesher.AddMeshData(mesh, 1);
			}
			else if (block.Code.Path.Contains("medium"))
			{
				string shapePath = "block/limbtrotline/limbtrotline-end-medium";
				MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase + shapePath, texture, alive);
				mesher.AddMeshData(mesh, 1);
			}
			else if (block.Code.Path.Contains("large"))
			{
				string shapePath = "block/limbtrotline/limbtrotline-end-large";
				MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase + shapePath, texture, alive);
				mesher.AddMeshData(mesh, 1);
			}
			if (block.Code.Path.Contains("-withmiddle"))
			{
				string shapePath = "block/limbtrotline/limbtrotline-end-extension";
				MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase + shapePath, texture, alive);
				mesher.AddMeshData(mesh, 1);
			}
			if (this.inventory != null)
			{
				if (!this.HookSlot.Empty)
				{
					string hookType = this.HookStack.Item.LastCodePart(0).ToString();
					string newPath = "texture" + hookType;
					Block tempblock = this.Api.World.GetBlock(block.CodeWithPath(newPath));
					tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tempblock, 0, false);
					string shapePath = "item/fishing/fishinghook";
					MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase + shapePath, tmpTextureSource, alive);
					mesher.AddMeshData(mesh, 1);
				}
				if (!this.LureSlot.Empty)
				{
					string lureType = this.LureStack.Item.LastCodePart(0).ToString();
					string newPath2 = "texture" + lureType;
					Block tempblock2 = this.Api.World.GetBlock(block.CodeWithPath(newPath2));
					tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tempblock2, 0, false);
					string shapePath = "item/fishing/fishinglure";
					MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase + shapePath, tmpTextureSource, alive);
					mesher.AddMeshData(mesh, 1);
				}
				if (!this.BaitSlot.Empty)
				{
					if (this.BaitStack.Item != null)
					{
						if (Array.IndexOf<string>(this.baitTypes, this.BaitStack.Item.FirstCodePart(0)) < 0 && !this.BaitStack.Item.Code.Path.Contains("creature-earthworm"))
						{
							Block tempblock3 = this.Api.World.GetBlock(block.CodeWithPath("texturerot"));
							tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tempblock3, 0, false);
						}
						else
						{
							tmpTextureSource = texture;
						}
					}
					else if (this.BaitStack.Block != null)
					{
						if (Array.IndexOf<string>(this.baitTypes, this.BaitStack.Block.FirstCodePart(0)) < 0)
						{
							Block tempblock4 = this.Api.World.GetBlock(block.CodeWithPath("texturerot"));
							tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tempblock4, 0, false);
						}
						else
						{
							tmpTextureSource = texture;
						}
					}
					string shapePath = "item/fishing/hookbait";
					MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapeBase + shapePath, tmpTextureSource, alive);
					mesher.AddMeshData(mesh, 1);
				}
				if (!this.CatchSlot.Empty)
				{
					string shapePath = "primitivesurvival:shapes/item/fishing/fish-pike";
					if (this.CatchStack.Item.Code.Path.Contains("psfreshwaterfish"))
					{
						string fishcode = this.CatchStack.Item.LastCodePart(2).ToString();
						shapePath = "game:shapes/entity/animal/fish/freshwater/" + fishcode + "-adult";
						string tmpBlockCode = "freshwaterfishoverride-" + fishcode + "-" + this.CatchStack.Item.LastCodePart(1).ToString();
						Block tmpBlock = this.Api.World.GetBlock(block.CodeWithPath(tmpBlockCode));
						tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tmpBlock, 0, false);
						alive = true;
					}
					else if (this.CatchStack.Item.Code.Path.Contains("pssaltwaterfish"))
					{
						string fishcode2 = this.CatchStack.Item.LastCodePart(2).ToString();
						if (fishcode2 == "mahi")
						{
							fishcode2 = "mahi-mahi";
						}
						shapePath = "game:shapes/entity/animal/fish/saltwater/" + fishcode2 + "-adult";
						string tmpBlockCode2 = "saltwaterfishoverride-" + fishcode2 + "-" + this.CatchStack.Item.LastCodePart(1).ToString();
						Block tmpBlock = this.Api.World.GetBlock(block.CodeWithPath(tmpBlockCode2));
						tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tmpBlock, 0, false);
						alive = true;
					}
					else
					{
						Block tmpBlock = this.Api.World.GetBlock(block.CodeWithPath("texturerot"));
						tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tmpBlock, 0, false);
					}
					MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapePath, tmpTextureSource, alive);
					mesher.AddMeshData(mesh, 1);
				}
			}
			return true;
		}

		// Token: 0x04000078 RID: 120
		private readonly int catchPercent = ModConfig.Loaded.LimbTrotlineCatchPercent;

		// Token: 0x04000079 RID: 121
		private readonly int baitedCatchPercent = ModConfig.Loaded.LimbTrotlineBaitedCatchPercent;

		// Token: 0x0400007A RID: 122
		private readonly int luredCatchPercent = ModConfig.Loaded.LimbTrotlineLuredCatchPercent;

		// Token: 0x0400007B RID: 123
		private readonly int baitedLuredCatchPercent = ModConfig.Loaded.LimbTrotlineBaitedLuredCatchPercent;

		// Token: 0x0400007C RID: 124
		private readonly int baitStolenPercent = ModConfig.Loaded.LimbTrotlineBaitStolenPercent;

		// Token: 0x0400007D RID: 125
		private readonly double updateMinutes = ModConfig.Loaded.LimbTrotlineUpdateMinutes;

		// Token: 0x0400007E RID: 126
		private readonly int rotRemovedPercent = ModConfig.Loaded.LimbTrotlineRotRemovedPercent;

		// Token: 0x0400007F RID: 127
		private readonly int tickSeconds = 5;

		// Token: 0x04000080 RID: 128
		private readonly int maxSlots = 4;

		// Token: 0x04000081 RID: 129
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

		// Token: 0x04000082 RID: 130
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

		// Token: 0x04000083 RID: 131
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

		// Token: 0x04000084 RID: 132
		private static readonly Random Rnd = new Random();

		// Token: 0x04000085 RID: 133
		private long particleTick;

		// Token: 0x04000086 RID: 134
		protected InventoryGeneric inventory;

		// Token: 0x04000087 RID: 135
		private AssetLocation wetPickupSound;

		// Token: 0x04000088 RID: 136
		private AssetLocation dryPickupSound;
	}
}
