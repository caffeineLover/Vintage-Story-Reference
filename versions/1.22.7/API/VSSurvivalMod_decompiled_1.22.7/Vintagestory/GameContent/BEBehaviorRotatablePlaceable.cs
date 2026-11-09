using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BEBehaviorRotatablePlaceable : BlockEntityBehavior, IMultiBlockColSelBoxes, IRotatable
{
	private Cuboidf[][] selectionBoxes;

	private Cuboidf[][] collisionboxes;

	private float height;

	public float MeshAngleRad { get; set; }

	public BEBehaviorRotatablePlaceable(BlockEntity blockentity)
		: base(blockentity)
	{
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		height = properties["height"].AsFloat(1f);
		selectionBoxes = null;
		collisionboxes = null;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		base.FromTreeAttributes(tree, worldAccessForResolve);
		MeshAngleRad = tree.GetFloat("meshAngleRad");
		Blockentity.MarkDirty(redrawOnClient: true);
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetFloat("meshAngleRad", MeshAngleRad);
	}

	public Cuboidf[] GetCollisionBoxes()
	{
		if (collisionboxes == null)
		{
			collisionboxes = rotatedCopy(base.Block.CollisionBoxes);
		}
		return collisionboxes[0];
	}

	public Cuboidf[] GetSelectionBoxes()
	{
		if (selectionBoxes == null)
		{
			selectionBoxes = rotatedCopy(base.Block.SelectionBoxes);
		}
		return selectionBoxes[0];
	}

	private Cuboidf[][] rotatedCopy(Cuboidf[] origboxes)
	{
		Cuboidf[][] rotatedBoxes = new Cuboidf[(int)height + 1][];
		for (int y = 0; (float)y < height; y++)
		{
			Cuboidf[] boxes = origboxes.Select((Cuboidf box) => box.RotatedCopy(0f, MeshAngleRad * (180f / (float)Math.PI), 0f, new Vec3d(0.5, 0.0, 0.5))).ToArray();
			Cuboidf[] array = boxes;
			for (int num = 0; num < array.Length; num++)
			{
				array[num].Offset(0f, -y, 0f);
			}
			rotatedBoxes[y] = boxes;
		}
		return rotatedBoxes;
	}

	public Cuboidf[] MBGetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos, Vec3i offset)
	{
		if (collisionboxes == null)
		{
			collisionboxes = rotatedCopy(base.Block.CollisionBoxes);
		}
		return collisionboxes[GameMath.Clamp(-offset.Y, 0, collisionboxes.Length - 1)];
	}

	public Cuboidf[] MBGetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos, Vec3i offset)
	{
		if (selectionBoxes == null)
		{
			selectionBoxes = rotatedCopy(base.Block.SelectionBoxes);
		}
		return selectionBoxes[GameMath.Clamp(-offset.Y, 0, selectionBoxes.Length - 1)];
	}

	public bool DoPartialSelection()
	{
		return true;
	}

	public void OnTransformed(IWorldAccessor worldAccessor, ITreeAttribute tree, int degreeRotation, Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, EnumAxis? flipAxis)
	{
		MeshAngleRad = tree.GetFloat("meshAngleRad");
		MeshAngleRad -= (float)degreeRotation * ((float)Math.PI / 180f);
		tree.SetFloat("meshAngleRad", MeshAngleRad);
	}
}
