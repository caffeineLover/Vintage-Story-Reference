using System;
using System.Collections.Generic;
using System.Text;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000021 RID: 33
	public class BlockIrrigationVessel : BlockLiquidContainerTopOpened
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00014C27 File Offset: 0x00012E27
		public override float CapacityLitres
		{
			get
			{
				return 50f;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00014C2E File Offset: 0x00012E2E
		public override bool CanDrinkFrom
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00014C34 File Offset: 0x00012E34
		public override ItemStack CreateItemStackFromJson(ITreeAttribute stackAttr, IWorldAccessor world, string domain)
		{
			ItemStack stack = base.CreateItemStackFromJson(stackAttr, world, domain);
			if (stackAttr.HasAttribute("makefull"))
			{
				WaterTightContainableProps props = BlockLiquidContainerBase.GetContainableProps(stack);
				stack.StackSize = (int)(this.CapacityLitres * props.ItemsPerLitre);
			}
			return stack;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00014C74 File Offset: 0x00012E74
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			if (api.Side != 2)
			{
				return;
			}
			ICoreClientAPI capi = api as ICoreClientAPI;
			this.interactions = ObjectCacheUtil.GetOrCreate<WorldInteraction[]>(api, "irrigationvessel", delegate()
			{
				List<ItemStack> liquidContainerStacks = new List<ItemStack>();
				foreach (CollectibleObject obj in api.World.Collectibles)
				{
					if (obj is ILiquidSource || obj is ILiquidSink || obj is BlockWateringCan)
					{
						List<ItemStack> stacks = obj.GetHandBookStacks(capi);
						if (stacks != null)
						{
							foreach (ItemStack stack in stacks)
							{
								stack.StackSize = 1;
								liquidContainerStacks.Add(stack);
							}
						}
					}
				}
				ItemStack[] lcstacks = liquidContainerStacks.ToArray();
				return new WorldInteraction[]
				{
					new WorldInteraction
					{
						ActionLangCode = "blockhelp-bucket-rightclick",
						MouseButton = 2,
						Itemstacks = lcstacks
					}
				};
			});
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00014CDC File Offset: 0x00012EDC
		public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
		{
			BEIrrigationVessel be = world.BlockAccessor.GetBlockEntity(pos) as BEIrrigationVessel;
			if (be != null && be.Buried && be.SoilType != null)
			{
				if (be.SoilType == "")
				{
					be.SoilType = "low";
				}
				AssetLocation AssetLoc = new AssetLocation("game:soil-" + be.SoilType + "-none");
				Block soilBlock = world.GetBlock(AssetLoc);
				if (soilBlock != null)
				{
					ItemStack tempStack = new ItemStack(soilBlock, 1);
					world.SpawnItemEntity(tempStack, pos.ToVec3d().Add(0.5, 1.1, 0.5), null);
				}
			}
			base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00014D99 File Offset: 0x00012F99
		public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
		{
			return byPlayer.Entity.Controls.Sneak && base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00014DBC File Offset: 0x00012FBC
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			ItemSlot playerSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
			if (!playerSlot.Empty)
			{
				Block block = world.BlockAccessor.GetBlock(blockSel.Position, 0);
				string path = block.Code.Path;
				ItemStack playerStack = playerSlot.Itemstack;
				if (!playerStack.Collectible.Code.Path.Contains("soil-") || !ArrayExtensions.Contains<string>(this.vesselTypes, block.LastCodePart(0)))
				{
					return base.OnBlockInteractStart(world, byPlayer, blockSel);
				}
				BEIrrigationVessel be = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BEIrrigationVessel;
				if (be.Buried)
				{
					return false;
				}
				be.Buried = true;
				be.SoilType = playerStack.Collectible.FirstCodePart(1);
				be.MarkDirty(false, null);
				playerSlot.TakeOut(1);
				playerSlot.MarkDirty();
				return true;
			}
			else
			{
				Block block2 = world.BlockAccessor.GetBlock(blockSel.Position, 0);
				ItemStack[] stacks = new ItemStack[]
				{
					block2.OnPickBlock(world, blockSel.Position)
				};
				BEIrrigationVessel beirrigationVessel = byPlayer.Entity.World.BlockAccessor.GetBlockEntity(blockSel.Position) as BEIrrigationVessel;
				if (beirrigationVessel != null && beirrigationVessel.Buried)
				{
					return false;
				}
				if (!world.Claims.TryAccess(byPlayer, blockSel.Position, 1))
				{
					return false;
				}
				if (!byPlayer.Entity.Controls.Sneak && byPlayer.InventoryManager.ActiveHotbarSlot.Empty)
				{
					bool blockToBreak = true;
					foreach (ItemStack stack in stacks)
					{
						if (!byPlayer.InventoryManager.TryGiveItemstack(stack, true))
						{
							world.SpawnItemEntity(stack, blockSel.Position.ToVec3d().AddCopy(0.5, 0.1, 0.5), null);
						}
						if (blockToBreak)
						{
							blockToBreak = false;
							world.BlockAccessor.SetBlock(0, blockSel.Position);
							world.BlockAccessor.TriggerNeighbourBlockUpdate(blockSel.Position);
						}
						if (world.Side == 1)
						{
							world.PlaySoundAt(block2.GetSounds(world.BlockAccessor, blockSel, null).Place, byPlayer, null, 1f);
						}
					}
					return true;
				}
				return false;
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00014FFC File Offset: 0x000131FC
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
		{
			if (blockSel == null || byEntity.Controls.Sneak)
			{
				return;
			}
			string path = slot.Itemstack.Block.Code.Path;
			BlockPos pos = blockSel.Position;
			byEntity.World.BlockAccessor.GetBlock(pos, 0);
			base.GetContent(slot.Itemstack);
			if (true)
			{
				base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handHandling);
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0001506C File Offset: 0x0001326C
		public override Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
		{
			BEIrrigationVessel be = blockAccessor.GetBlockEntity(pos) as BEIrrigationVessel;
			if (be != null && be.Buried)
			{
				return new Cuboidf[]
				{
					new Cuboidf(0f, 0f, 0f, 1f, 1f, 1f).RotatedCopy(0f, 0f, 0f, new Vec3d(0.5, 0.5, 0.5))
				};
			}
			return base.GetCollisionBoxes(blockAccessor, pos);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000150FC File Offset: 0x000132FC
		public override Cuboidf[] GetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
		{
			BEIrrigationVessel be = blockAccessor.GetBlockEntity(pos) as BEIrrigationVessel;
			if (be != null && be.Buried)
			{
				return new Cuboidf[]
				{
					new Cuboidf(0f, 0f, 0f, 1f, 1f, 1f).RotatedCopy(0f, 0f, 0f, new Vec3d(0.5, 0.5, 0.5))
				};
			}
			return base.GetSelectionBoxes(blockAccessor, pos);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0001518A File Offset: 0x0001338A
		public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack)
		{
			return base.DoPlaceBlock(world, byPlayer, blockSel, byItemStack);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00015198 File Offset: 0x00013398
		public MeshData GenMesh(ICoreClientAPI capi, string shapePath, ITexPositionSource texture)
		{
			ITesselatorAPI tesselator = capi.Tesselator;
			Shape shape = capi.Assets.TryGet(shapePath + ".json", true).ToObject<Shape>(null);
			MeshData mesh;
			tesselator.TesselateShape(shapePath, shape, ref mesh, texture, new Vec3f(0f, 0f, 0f), 0, 0, 0, default(int?), null);
			return mesh;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000151FC File Offset: 0x000133FC
		public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
		{
			BEIrrigationVessel beiv = forPlayer.Entity.World.BlockAccessor.GetBlockEntity(selection.Position) as BEIrrigationVessel;
			if (beiv != null)
			{
				return ArrayExtensions.Append<WorldInteraction>(new WorldInteraction[]
				{
					new WorldInteraction
					{
						ActionLangCode = "blockhelp-behavior-rightclickpickup",
						MouseButton = 2,
						RequireFreeHand = true,
						ShouldApply = ((WorldInteraction wi, BlockSelection bs, EntitySelection es) => !beiv.Buried)
					}
				}, base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
			}
			return base.GetPlacedBlockInteractionHelp(world, selection, forPlayer);
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00015290 File Offset: 0x00013490
		public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
		{
			WorldInteraction[] array = new WorldInteraction[2];
			array[0] = new WorldInteraction
			{
				ActionLangCode = "heldhelp-fill",
				MouseButton = 2,
				ShouldApply = ((WorldInteraction wi, BlockSelection bs, EntitySelection es) => this.GetCurrentLitres(inSlot.Itemstack) < this.CapacityLitres)
			};
			int num = 1;
			WorldInteraction worldInteraction = new WorldInteraction();
			worldInteraction.ActionLangCode = "heldhelp-place";
			worldInteraction.HotKeyCode = "sneak";
			worldInteraction.MouseButton = 2;
			worldInteraction.ShouldApply = ((WorldInteraction wi, BlockSelection bs, EntitySelection es) => true);
			array[num] = worldInteraction;
			return array;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00015330 File Offset: 0x00013530
		public override string GetPlacedBlockInfo(IWorldAccessor world, BlockPos pos, IPlayer forPlayer)
		{
			StringBuilder dsc = new StringBuilder();
			dsc.AppendLine(base.GetPlacedBlockInfo(world, pos, forPlayer));
			BEIrrigationVessel be = world.BlockAccessor.GetBlockEntity(pos) as BEIrrigationVessel;
			if (be != null)
			{
				if (be.Buried)
				{
					if (be.Inventory.Empty)
					{
						dsc.AppendLine().AppendLine(Lang.GetMatching("primitivesurvival:fill-irrigationvessel", Array.Empty<object>()));
					}
					else if (be.Inventory[0].Itemstack.Collectible.Code.Path.Contains("water"))
					{
						dsc.AppendLine().AppendLine(Lang.GetMatching("primitivesurvival:working-irrigationvessel", Array.Empty<object>()));
					}
					else
					{
						dsc.AppendLine().AppendLine(Lang.GetMatching("primitivesurvival:wrongliquid-irrigationvessel", Array.Empty<object>()));
					}
				}
				else
				{
					dsc.AppendLine().AppendLine(Lang.GetMatching("primitivesurvival:bury-irrigationvessel", Array.Empty<object>()));
				}
				if (ModConfig.Loaded.ShowModNameInHud)
				{
					dsc.AppendLine();
					dsc.AppendLine("\n<font color=\"#D8EAA3\"><i>" + Lang.GetMatching("game:tabname-primitive", Array.Empty<object>()) + "</i></font>").AppendLine();
				}
			}
			return dsc.AppendLine().ToString();
		}

		// Token: 0x040000CF RID: 207
		private readonly string[] vesselTypes = new string[]
		{
			"normal",
			"fire",
			"black",
			"brown",
			"cream",
			"gray",
			"orange",
			"red",
			"tan",
			"earthyorange"
		};

		// Token: 0x040000D0 RID: 208
		protected WorldInteraction[] interactions;
	}
}
