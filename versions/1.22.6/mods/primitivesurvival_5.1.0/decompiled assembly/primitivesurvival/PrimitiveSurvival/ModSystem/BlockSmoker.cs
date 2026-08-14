using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000027 RID: 39
	public class BlockSmoker : Block, IIgnitable
	{
		// Token: 0x060001E8 RID: 488 RVA: 0x0001718E File Offset: 0x0001538E
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			EnumAppSide side = api.Side;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000171A0 File Offset: 0x000153A0
		public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
		{
			BESmoker be = world.BlockAccessor.GetBlockEntity(selection.Position) as BESmoker;
			if (((be != null) ? be.State : null) == "lit")
			{
				return base.GetPlacedBlockInteractionHelp(world, selection, forPlayer);
			}
			List<ItemStack> torchStacklist = BlockBehaviorCanIgnite.CanIgniteStacks(this.api, false);
			if (((be != null) ? be.State : null) == "closed" && be != null && be.WoodSlot.StackSize == 4 && be != null && !be.Inventory[0].Empty)
			{
				WorldInteraction[] placedBlockInteractionHelp = base.GetPlacedBlockInteractionHelp(world, selection, forPlayer);
				WorldInteraction[] array = new WorldInteraction[2];
				int num = 0;
				WorldInteraction worldInteraction = new WorldInteraction();
				worldInteraction.ActionLangCode = "blockhelp-forge-ignite";
				worldInteraction.MouseButton = 2;
				worldInteraction.HotKeyCode = "shift";
				worldInteraction.Itemstacks = torchStacklist.ToArray();
				worldInteraction.GetMatchingStacks = ((WorldInteraction wi, BlockSelection bs, EntitySelection es) => wi.Itemstacks);
				array[num] = worldInteraction;
				array[1] = new WorldInteraction
				{
					ActionLangCode = "primitivesurvival:blockhelp-smoker-rightclickopen",
					MouseButton = 2,
					HotKeyCode = null
				};
				return ArrayExtensions.Append<WorldInteraction>(placedBlockInteractionHelp, array);
			}
			if (((be != null) ? be.State : null) == "closed")
			{
				return ArrayExtensions.Append<WorldInteraction>(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer), new WorldInteraction[]
				{
					new WorldInteraction
					{
						ActionLangCode = "primitivesurvival:blockhelp-smoker-rightclickopen",
						MouseButton = 2,
						HotKeyCode = null
					}
				});
			}
			if (((be != null) ? be.State : null) == "open" && be != null && !be.Inventory[0].Empty && be.Inventory[0].Itemstack.Collectible.FirstCodePart(2) == "smoked")
			{
				return ArrayExtensions.Append<WorldInteraction>(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer), new WorldInteraction[]
				{
					new WorldInteraction
					{
						ActionLangCode = "primitivesurvival:blockhelp-smoker-smokedmeat",
						MouseButton = 2,
						HotKeyCode = null
					}
				});
			}
			List<ItemStack> trussedMeatStacklist = new List<ItemStack>();
			List<ItemStack> firewoodStacklist = new List<ItemStack>();
			foreach (CollectibleObject obj in this.api.World.Collectibles)
			{
				if (obj.Code.FirstCodePart() == "trussedmeat")
				{
					List<ItemStack> stacks = obj.GetHandBookStacks(this.api as ICoreClientAPI);
					if (stacks != null)
					{
						trussedMeatStacklist.AddRange(stacks);
					}
				}
				else if (obj.Code.FirstCodePart().Contains("firewood"))
				{
					List<ItemStack> stacks2 = obj.GetHandBookStacks(this.api as ICoreClientAPI);
					if (stacks2 != null)
					{
						firewoodStacklist.AddRange(stacks2);
					}
				}
			}
			return ArrayExtensions.Append<WorldInteraction>(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer), new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = "primitivesurvival:blockhelp-smoker-rightclickclose",
					MouseButton = 2,
					HotKeyCode = null
				},
				new WorldInteraction
				{
					ActionLangCode = "primitivesurvival:blockhelp-smoker-trussedmeat",
					HotKeyCode = null,
					MouseButton = 2,
					Itemstacks = trussedMeatStacklist.ToArray(),
					GetMatchingStacks = new InteractionStacksDelegate(this.getMatchingStacks)
				},
				new WorldInteraction
				{
					ActionLangCode = "primitivesurvival:blockhelp-smoker-firewood",
					HotKeyCode = null,
					MouseButton = 2,
					Itemstacks = firewoodStacklist.ToArray(),
					GetMatchingStacks = new InteractionStacksDelegate(this.getMatchingStacks)
				}
			});
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00017528 File Offset: 0x00015728
		private ItemStack[] getMatchingStacks(WorldInteraction wi, BlockSelection selection, EntitySelection entitySelection)
		{
			BESmoker be = this.api.World.BlockAccessor.GetBlockEntity(selection.Position) as BESmoker;
			if (be == null || wi.Itemstacks.Length == 0)
			{
				return null;
			}
			List<ItemStack> matchStacks = new List<ItemStack>();
			foreach (ItemStack stack in wi.Itemstacks)
			{
				if (stack.Collectible.Code.FirstCodePart().Contains("firewood") && be.Inventory[4].StackSize < 4)
				{
					matchStacks.Add(stack);
				}
				if (stack.Collectible.Code.FirstCodePart() == "trussedmeat" && be.Inventory[3].Empty)
				{
					matchStacks.Add(stack);
				}
			}
			return matchStacks.ToArray();
		}

		// Token: 0x060001EB RID: 491 RVA: 0x000175FB File Offset: 0x000157FB
		EnumIgniteState IIgnitable.OnTryIgniteStack(EntityAgent byEntity, BlockPos pos, ItemSlot slot, float secondsIgniting)
		{
			if (secondsIgniting <= 2f)
			{
				return 2;
			}
			return 3;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00017609 File Offset: 0x00015809
		public EnumIgniteState OnTryIgniteBlock(EntityAgent byEntity, BlockPos pos, float secondsIgniting)
		{
			if (!(byEntity.World.BlockAccessor.GetBlockEntity(pos) as BESmoker).CanIgnite())
			{
				return 1;
			}
			if (secondsIgniting <= 4f)
			{
				return 2;
			}
			return 3;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x00017635 File Offset: 0x00015835
		public void OnTryIgniteBlockOver(EntityAgent byEntity, BlockPos pos, float secondsIgniting, ref EnumHandling handling)
		{
			handling = 2;
			BESmoker besmoker = byEntity.World.BlockAccessor.GetBlockEntity(pos) as BESmoker;
			if (besmoker == null)
			{
				return;
			}
			besmoker.TryIgnite();
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0001765C File Offset: 0x0001585C
		public MeshData GenMesh(ICoreClientAPI capi, string shapePath, ITexPositionSource texture, string state, int count)
		{
			ITesselatorAPI tesselator = capi.Tesselator;
			Shape shape = capi.Assets.TryGet(shapePath + ".json", true).ToObject<Shape>(null);
			int glow = 0;
			if (shapePath.Contains("lit"))
			{
				glow = 200;
			}
			MeshData mesh;
			tesselator.TesselateShape(shapePath, shape, ref mesh, texture, new Vec3f(0f, 0f, 0f), glow, 0, 0, default(int?), null);
			float rotate = this.Shape.rotateY;
			if (state == "open" && shapePath.Contains("door"))
			{
				rotate -= 100f;
				mesh.Translate(0.2f, 0f, 0.8f);
			}
			if (shapePath.Contains("log") || shapePath.Contains("lit"))
			{
				if (count == 1)
				{
					mesh.Translate(0.04f, -0.25f, -0.26f);
				}
				else if (count == 2)
				{
					mesh.Translate(0.06f, -0.25f, 0f);
				}
				else if (count == 3)
				{
					mesh.Translate(0.01f, 0f, -0.26f);
				}
				else
				{
					mesh.Translate(0.02f, 0f, 0f);
				}
			}
			mesh.Rotate(new Vec3f(0.5f, 0f, 0.5f), 0f, rotate * 0.017453292f, 0f);
			return mesh;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x000177CC File Offset: 0x000159CC
		public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
		{
			BESmoker be = world.BlockAccessor.GetBlockEntity(pos) as BESmoker;
			if (be != null)
			{
				be.OnBreak();
			}
			base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00017800 File Offset: 0x00015A00
		public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
		{
			string facing = Block.SuggestedHVOrientation(byPlayer, blockSel)[0].ToString();
			bool flag = base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
			if (flag)
			{
				Block block = this.api.World.BlockAccessor.GetBlock(blockSel.Position, 0);
				string newPath = block.Code.Path;
				newPath = newPath.Replace("north", facing);
				block = this.api.World.GetBlock(block.CodeWithPath(newPath));
				this.api.World.BlockAccessor.SetBlock(block.BlockId, blockSel.Position);
			}
			return flag;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000178A0 File Offset: 0x00015AA0
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			BESmoker be = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BESmoker;
			if (be != null)
			{
				return be.OnInteract(byPlayer, blockSel);
			}
			return base.OnBlockInteractStart(world, byPlayer, blockSel);
		}
	}
}
