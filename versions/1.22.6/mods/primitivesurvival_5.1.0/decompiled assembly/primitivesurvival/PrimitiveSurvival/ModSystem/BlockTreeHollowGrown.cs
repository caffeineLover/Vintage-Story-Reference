using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200002B RID: 43
	public class BlockTreeHollowGrown : Block
	{
		// Token: 0x06000208 RID: 520 RVA: 0x000189B0 File Offset: 0x00016BB0
		public override void OnLoaded(ICoreAPI api)
		{
			if (api.Side != 2)
			{
				return;
			}
			this.interactions = ObjectCacheUtil.GetOrCreate<WorldInteraction[]>(api, "treehollowInteractions", () => new WorldInteraction[]
			{
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-behavior-rightclickpickup",
					MouseButton = 2,
					RequireFreeHand = true
				}
			});
		}

		// Token: 0x06000209 RID: 521 RVA: 0x000189EC File Offset: 0x00016BEC
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

		// Token: 0x0600020A RID: 522 RVA: 0x00018A8C File Offset: 0x00016C8C
		public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
		{
			BETreeHollowGrown bedc = world.BlockAccessor.GetBlockEntity(pos) as BETreeHollowGrown;
			if (bedc != null)
			{
				bedc.OnBreak();
				AssetLocation newAsset = new AssetLocation("primitivesurvival:treehollowplaced-" + this.FirstCodePart(2) + "-north");
				bool found = ArrayExtensions.Contains<string>(this.validWoodTypes, this.FirstCodePart(2));
				if (newAsset != null && found)
				{
					BlockTreeHollowPlaced newBlock = this.api.World.GetBlock(newAsset) as BlockTreeHollowPlaced;
					world.BlockAccessor.SetBlock(newBlock.BlockId, pos);
					BETreeHollowPlaced be = world.BlockAccessor.GetBlockEntity(pos) as BETreeHollowPlaced;
					if (be != null)
					{
						be.Initialize(this.api);
						be.type = this.FirstCodePart(1);
						be.MarkDirty(false, null);
						world.BlockAccessor.BreakBlock(pos, null, 1f);
					}
				}
			}
			base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00018B78 File Offset: 0x00016D78
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			BETreeHollowGrown bedc = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BETreeHollowGrown;
			if (bedc != null)
			{
				return bedc.OnInteract(byPlayer, blockSel);
			}
			return base.OnBlockInteractStart(world, byPlayer, blockSel);
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00018BB4 File Offset: 0x00016DB4
		public MeshData GenMesh(ICoreClientAPI capi, string shapePath, ITexPositionSource texture, ITesselatorAPI tesselator = null)
		{
			MeshData mesh = null;
			Shape shape = capi.Assets.TryGet(shapePath + ".json", true).ToObject<Shape>(null);
			if (shape != null && texture != null)
			{
				tesselator.TesselateShape(shapePath, shape, ref mesh, texture, new Vec3f(this.Shape.rotateX, this.Shape.rotateY, this.Shape.rotateZ), 0, 0, 0, default(int?), null);
			}
			return mesh;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00018C2C File Offset: 0x00016E2C
		public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
		{
			BETreeHollowGrown bedc = world.BlockAccessor.GetBlockEntity(selection.Position) as BETreeHollowGrown;
			if (bedc != null && !bedc.Inventory.Empty)
			{
				return ArrayExtensions.Append<WorldInteraction>(this.interactions, base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
			}
			return null;
		}

		// Token: 0x040000E1 RID: 225
		private WorldInteraction[] interactions;

		// Token: 0x040000E2 RID: 226
		private readonly string[] validWoodTypes = new string[]
		{
			"acacia",
			"birch",
			"kapok",
			"larch",
			"maple",
			"oak",
			"pine",
			"walnut"
		};
	}
}
