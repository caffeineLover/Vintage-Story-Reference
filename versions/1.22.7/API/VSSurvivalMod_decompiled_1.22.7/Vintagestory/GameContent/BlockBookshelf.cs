using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockBookshelf : BlockShapeMaterialFromAttributes
{
	public Dictionary<string, int[]> UsableSlots;

	public override string MeshKey { get; } = "BookshelfMeshes";

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		LoadTypes();
	}

	public override void LoadTypes()
	{
		base.LoadTypes();
		UsableSlots = Attributes["usableSlots"].AsObject<Dictionary<string, int[]>>();
		ArgumentNullException.ThrowIfNull(UsableSlots, "UsableSlots");
	}

	public override Cuboidf[] GetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		BlockEntityBookshelf beshelf = blockAccessor.GetBlockEntity(pos) as BlockEntityBookshelf;
		if (beshelf?.UsableSlots != null)
		{
			List<Cuboidf> cubs = new List<Cuboidf>
			{
				new Cuboidf(0f, 0f, 0f, 1f, 1f, 0.1f),
				new Cuboidf(0f, 0f, 0f, 1f, 0.0625f, 0.5f),
				new Cuboidf(0f, 0.9375f, 0f, 1f, 1f, 0.5f),
				new Cuboidf(0f, 0f, 0f, 0.0625f, 1f, 0.5f),
				new Cuboidf(0.9375f, 0f, 0f, 1f, 1f, 0.5f)
			};
			for (int i = 0; i < 14; i++)
			{
				if (!beshelf.UsableSlots.Contains(i))
				{
					cubs.Add(new Cuboidf());
					continue;
				}
				float x = (float)(i % 7) * 2f / 16f + 11f / 160f;
				float y = (float)(i / 7) * 7.5f / 16f;
				float z = 13f / 32f;
				Cuboidf cub = new Cuboidf(x, y + 0.0625f, 0.0625f, x + 19f / 160f, y + 0.4375f, z);
				cubs.Add(cub);
			}
			for (int j = 0; j < cubs.Count; j++)
			{
				cubs[j] = cubs[j].RotatedCopy(0f, (beshelf?.MeshAngleRad ?? 0f) * (180f / (float)Math.PI), 0f, new Vec3d(0.5, 0.5, 0.5));
			}
			return cubs.ToArray();
		}
		return new Cuboidf[1] { new Cuboidf(0f, 0f, 0f, 1f, 1f, 0.5f).RotatedCopy(0f, (beshelf?.MeshAngleRad ?? 0f) * (180f / (float)Math.PI), 0f, new Vec3d(0.5, 0.5, 0.5)) };
	}

	public override Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		BlockEntityBookshelf beshelf = blockAccessor.GetBlockEntity(pos) as BlockEntityBookshelf;
		return new Cuboidf[1] { new Cuboidf(0f, 0f, 0f, 1f, 1f, 0.5f).RotatedCopy(0f, (beshelf?.MeshAngleRad ?? 0f) * (180f / (float)Math.PI), 0f, new Vec3d(0.5, 0.5, 0.5)) };
	}

	public override void GetDecal(IWorldAccessor world, BlockPos pos, ITexPositionSource decalTexSource, ref MeshData decalModelData, ref MeshData blockModelData)
	{
		BlockEntityBookshelf beb = GetBlockEntity<BlockEntityBookshelf>(pos);
		if (beb != null && beb.Type != null && beb.Material != null)
		{
			float[] mat = Matrixf.Create().Translate(0.5f, 0.5f, 0.5f).RotateY(beb.MeshAngleRad)
				.Translate(-0.5f, -0.5f, -0.5f)
				.Values;
			blockModelData = GetOrCreateMesh(beb.Type, beb.Material).Clone().MatrixTransform(mat);
			decalModelData = GetOrCreateMesh(beb.Type, beb.Material, null, decalTexSource).Clone().MatrixTransform(mat);
		}
		else
		{
			base.GetDecal(world, pos, decalTexSource, ref decalModelData, ref blockModelData);
		}
	}

	public override bool DoPartialSelection(IWorldAccessor world, BlockPos pos)
	{
		return true;
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityBookshelf beshelf)
		{
			return beshelf.OnInteract(byPlayer, blockSel);
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel);
	}
}
