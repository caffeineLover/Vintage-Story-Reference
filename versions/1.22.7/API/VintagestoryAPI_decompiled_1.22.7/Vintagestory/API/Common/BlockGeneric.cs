using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common;

public class BlockGeneric : Block
{
	public override void GetDecal(IWorldAccessor world, BlockPos pos, ITexPositionSource decalTexSource, ref MeshData decalModelData, ref MeshData blockModelData)
	{
		bool preventDefault = false;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			if (blockBehaviors[i] is StrongBlockBehavior sbh)
			{
				EnumHandling handled = EnumHandling.PassThrough;
				sbh.GetDecal(world, pos, decalTexSource, ref decalModelData, ref blockModelData, ref handled);
				if (handled != EnumHandling.PassThrough)
				{
					preventDefault = true;
				}
				if (handled == EnumHandling.PreventSubsequent)
				{
					return;
				}
			}
		}
		if (!preventDefault)
		{
			base.GetDecal(world, pos, decalTexSource, ref decalModelData, ref blockModelData);
		}
	}

	public override void OnDecalTesselation(IWorldAccessor world, MeshData decalMesh, BlockPos pos)
	{
		bool preventDefault = false;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			if (blockBehaviors[i] is StrongBlockBehavior sbh)
			{
				EnumHandling handled = EnumHandling.PassThrough;
				sbh.OnDecalTesselation(world, decalMesh, pos, ref handled);
				if (handled != EnumHandling.PassThrough)
				{
					preventDefault = true;
				}
				if (handled == EnumHandling.PreventSubsequent)
				{
					return;
				}
			}
		}
		if (!preventDefault)
		{
			base.OnDecalTesselation(world, decalMesh, pos);
		}
	}

	public override Cuboidf GetParticleBreakBox(IBlockAccessor blockAccess, BlockPos pos, BlockFacing facing)
	{
		bool preventDefault = false;
		Cuboidf box = null;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			if (blockBehaviors[i] is StrongBlockBehavior sbh)
			{
				EnumHandling handled = EnumHandling.PassThrough;
				box = sbh.GetParticleBreakBox(blockAccess, pos, facing, ref handled);
				switch (handled)
				{
				case EnumHandling.PreventSubsequent:
					return box;
				case EnumHandling.PreventDefault:
					preventDefault = true;
					break;
				}
			}
		}
		if (preventDefault)
		{
			return box;
		}
		return base.GetParticleBreakBox(blockAccess, pos, facing);
	}

	public override Cuboidf[] GetParticleCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		bool preventDefault = false;
		List<Cuboidf> allboxes = null;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			if (!(blockBehaviors[i] is StrongBlockBehavior sbh))
			{
				continue;
			}
			EnumHandling handled = EnumHandling.PassThrough;
			Cuboidf[] boxes = sbh.GetParticleCollisionBoxes(blockAccessor, pos, ref handled);
			switch (handled)
			{
			case EnumHandling.PreventSubsequent:
				return boxes;
			case EnumHandling.PreventDefault:
				preventDefault = true;
				break;
			}
			if (boxes != null)
			{
				if (allboxes == null)
				{
					allboxes = new List<Cuboidf>();
				}
				allboxes.AddRange(boxes);
			}
		}
		if (preventDefault)
		{
			return allboxes.ToArray();
		}
		if (allboxes == null)
		{
			return base.GetParticleCollisionBoxes(blockAccessor, pos);
		}
		allboxes.AddRange(base.GetParticleCollisionBoxes(blockAccessor, pos));
		return allboxes.ToArray();
	}

	public override Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		bool preventDefault = false;
		List<Cuboidf> allboxes = null;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			if (!(blockBehaviors[i] is StrongBlockBehavior sbh))
			{
				continue;
			}
			EnumHandling handled = EnumHandling.PassThrough;
			Cuboidf[] boxes = sbh.GetCollisionBoxes(blockAccessor, pos, ref handled);
			switch (handled)
			{
			case EnumHandling.PreventSubsequent:
				return boxes;
			case EnumHandling.PreventDefault:
				preventDefault = true;
				break;
			}
			if (boxes != null)
			{
				if (allboxes == null)
				{
					allboxes = new List<Cuboidf>();
				}
				allboxes.AddRange(boxes);
			}
		}
		if (preventDefault)
		{
			return allboxes.ToArray();
		}
		if (allboxes == null)
		{
			return base.GetCollisionBoxes(blockAccessor, pos);
		}
		allboxes.AddRange(base.GetCollisionBoxes(blockAccessor, pos));
		return allboxes.ToArray();
	}

	public override Cuboidf[] GetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		bool preventDefault = false;
		List<Cuboidf> allboxes = null;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			if (!(blockBehaviors[i] is StrongBlockBehavior sbh))
			{
				continue;
			}
			EnumHandling handled = EnumHandling.PassThrough;
			Cuboidf[] boxes = sbh.GetSelectionBoxes(blockAccessor, pos, ref handled);
			switch (handled)
			{
			case EnumHandling.PreventSubsequent:
				return boxes;
			case EnumHandling.PreventDefault:
				preventDefault = true;
				break;
			}
			if (boxes != null)
			{
				if (allboxes == null)
				{
					allboxes = new List<Cuboidf>();
				}
				allboxes.AddRange(boxes);
			}
		}
		if (preventDefault)
		{
			return allboxes.ToArray();
		}
		if (allboxes == null)
		{
			return base.GetSelectionBoxes(blockAccessor, pos);
		}
		allboxes.AddRange(base.GetSelectionBoxes(blockAccessor, pos));
		return allboxes.ToArray();
	}

	public override bool TryPlaceBlockForWorldGen(IBlockAccessor blockAccessor, BlockPos pos, BlockFacing onBlockFace, IRandom worldgenRandom, BlockPatchAttributes attributes = null)
	{
		bool result = true;
		bool preventDefault = false;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			if (blockBehaviors[i] is StrongBlockBehavior sbh)
			{
				EnumHandling handled = EnumHandling.PassThrough;
				bool behaviorResult = sbh.TryPlaceBlockForWorldGen(blockAccessor, pos, onBlockFace, worldgenRandom, ref handled);
				if (handled != EnumHandling.PassThrough)
				{
					result = result && behaviorResult;
					preventDefault = true;
				}
				if (handled == EnumHandling.PreventSubsequent)
				{
					return result;
				}
			}
		}
		if (preventDefault)
		{
			return result;
		}
		return base.TryPlaceBlockForWorldGen(blockAccessor, pos, onBlockFace, worldgenRandom, attributes);
	}

	public override bool DoPartialSelection(IWorldAccessor world, BlockPos pos)
	{
		bool result = true;
		bool preventDefault = false;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			if (blockBehaviors[i] is StrongBlockBehavior sbh)
			{
				EnumHandling handled = EnumHandling.PassThrough;
				bool behaviorResult = sbh.DoPartialSelection(world, pos, ref handled);
				if (handled != EnumHandling.PassThrough)
				{
					result = result && behaviorResult;
					preventDefault = true;
				}
				if (handled == EnumHandling.PreventSubsequent)
				{
					return result;
				}
			}
		}
		if (preventDefault)
		{
			return result;
		}
		return base.DoPartialSelection(world, pos);
	}

	public override int GetColorWithoutTint(ICoreClientAPI capi, BlockPos pos)
	{
		int result = 0;
		bool preventDefault = false;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			if (blockBehaviors[i] is StrongBlockBehavior sbh)
			{
				EnumHandling handled = EnumHandling.PassThrough;
				int behaviorResult = sbh.GetColorWithoutTint(capi, pos, ref handled);
				if (handled != EnumHandling.PassThrough)
				{
					result = behaviorResult;
					preventDefault = true;
				}
				if (handled == EnumHandling.PreventSubsequent)
				{
					return result;
				}
			}
		}
		if (preventDefault)
		{
			return result;
		}
		return base.GetColorWithoutTint(capi, pos);
	}

	public override int GetRandomColor(ICoreClientAPI capi, BlockPos pos, BlockFacing facing, int rndIndex = -1)
	{
		int result = 0;
		bool preventDefault = false;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			if (blockBehaviors[i] is StrongBlockBehavior sbh)
			{
				EnumHandling handled = EnumHandling.PassThrough;
				int behaviorResult = sbh.GetRandomColor(capi, pos, facing, rndIndex, ref handled);
				if (handled != EnumHandling.PassThrough)
				{
					result = behaviorResult;
					preventDefault = true;
				}
				if (handled == EnumHandling.PreventSubsequent)
				{
					return result;
				}
			}
		}
		if (preventDefault)
		{
			return result;
		}
		return base.GetRandomColor(capi, pos, facing, rndIndex);
	}

	public override bool SideIsSolid(BlockPos pos, int faceIndex)
	{
		bool result = true;
		bool preventDefault = false;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			if (blockBehaviors[i] is StrongBlockBehavior sbh)
			{
				EnumHandling handled = EnumHandling.PassThrough;
				bool behaviorResult = sbh.SideIsSolid(pos, faceIndex, ref handled);
				if (handled != EnumHandling.PassThrough)
				{
					result = result && behaviorResult;
					preventDefault = true;
				}
				if (handled == EnumHandling.PreventSubsequent)
				{
					return result;
				}
			}
		}
		if (preventDefault)
		{
			return result;
		}
		return base.SideIsSolid(pos, faceIndex);
	}

	public override bool SideIsSolid(IBlockAccessor blockAccess, BlockPos pos, int faceIndex)
	{
		bool result = true;
		bool preventDefault = false;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			if (blockBehaviors[i] is StrongBlockBehavior sbh)
			{
				EnumHandling handled = EnumHandling.PassThrough;
				bool behaviorResult = sbh.SideIsSolid(blockAccess, pos, faceIndex, ref handled);
				if (handled != EnumHandling.PassThrough)
				{
					result = result && behaviorResult;
					preventDefault = true;
				}
				if (handled == EnumHandling.PreventSubsequent)
				{
					return result;
				}
			}
		}
		if (preventDefault)
		{
			return result;
		}
		return base.SideIsSolid(blockAccess, pos, faceIndex);
	}

	public override int GetLightAbsorption(IWorldChunk chunk, BlockPos pos)
	{
		int result = 0;
		bool preventDefault = false;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			if (blockBehaviors[i] is StrongBlockBehavior sbh)
			{
				EnumHandling handled = EnumHandling.PassThrough;
				int behaviorResult = sbh.GetLightAbsorption(chunk, pos, ref handled);
				if (handled != EnumHandling.PassThrough)
				{
					result = behaviorResult;
					preventDefault = true;
				}
				if (handled == EnumHandling.PreventSubsequent)
				{
					return result;
				}
			}
		}
		if (preventDefault)
		{
			return result;
		}
		return base.GetLightAbsorption(chunk, pos);
	}

	public override int GetLightAbsorption(IBlockAccessor blockAccessor, BlockPos pos)
	{
		int result = 0;
		bool preventDefault = false;
		BlockBehavior[] blockBehaviors = BlockBehaviors;
		for (int i = 0; i < blockBehaviors.Length; i++)
		{
			if (blockBehaviors[i] is StrongBlockBehavior sbh)
			{
				EnumHandling handled = EnumHandling.PassThrough;
				int behaviorResult = sbh.GetLightAbsorption(blockAccessor, pos, ref handled);
				if (handled != EnumHandling.PassThrough)
				{
					result = behaviorResult;
					preventDefault = true;
				}
				if (handled == EnumHandling.PreventSubsequent)
				{
					return result;
				}
			}
		}
		if (preventDefault)
		{
			return result;
		}
		return base.GetLightAbsorption(blockAccessor, pos);
	}
}
