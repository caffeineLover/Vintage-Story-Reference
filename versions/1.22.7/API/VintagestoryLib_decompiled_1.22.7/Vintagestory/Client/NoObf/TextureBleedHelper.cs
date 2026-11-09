using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.Client.NoObf;

public static class TextureBleedHelper
{
	public struct BleedDirections
	{
		public bool HasBleed;

		public int[] BleedTextureSubIds;

		public int[] BleedPriorities;

		public int[] OverlayTextureSubIds;
	}

	private static int[][] faceNeighborOffsets;

	private static int[] faceOcclusionOffsets;

	public static void Initialize(int extChunkSize)
	{
		int moveX = 1;
		int moveY = extChunkSize * extChunkSize;
		faceOcclusionOffsets = new int[6]
		{
			-extChunkSize,
			moveX,
			extChunkSize,
			-moveX,
			moveY,
			-moveY
		};
		faceNeighborOffsets = new int[6][]
		{
			new int[8]
			{
				moveY,
				-moveY,
				-moveX,
				moveX,
				moveY - moveX,
				-moveY - moveX,
				-moveY + moveX,
				moveY + moveX
			},
			new int[8]
			{
				moveY,
				-moveY,
				-extChunkSize,
				extChunkSize,
				moveY - extChunkSize,
				-moveY - extChunkSize,
				-moveY + extChunkSize,
				moveY + extChunkSize
			},
			new int[8]
			{
				moveY,
				-moveY,
				moveX,
				-moveX,
				moveY + moveX,
				-moveY + moveX,
				-moveY - moveX,
				moveY - moveX
			},
			new int[8]
			{
				moveY,
				-moveY,
				extChunkSize,
				-extChunkSize,
				moveY + extChunkSize,
				-moveY + extChunkSize,
				-moveY - extChunkSize,
				moveY - extChunkSize
			},
			new int[8]
			{
				extChunkSize,
				-extChunkSize,
				-moveX,
				moveX,
				extChunkSize - moveX,
				-extChunkSize - moveX,
				-extChunkSize + moveX,
				extChunkSize + moveX
			},
			new int[8]
			{
				extChunkSize,
				-extChunkSize,
				-moveX,
				moveX,
				extChunkSize - moveX,
				-extChunkSize - moveX,
				-extChunkSize + moveX,
				extChunkSize + moveX
			}
		};
	}

	public static BleedDirections GetBleedDirections(Block[] chunkBlocks, int extIndex3d, int tileSide, Block currentBlock, int[][] fastBlockTextureSubidsByBlockAndFace, int[] overlayTextureSubIdsByBlockId = null)
	{
		BleedDirections result = new BleedDirections
		{
			BleedTextureSubIds = new int[8] { -1, -1, -1, -1, -1, -1, -1, -1 },
			BleedPriorities = new int[8],
			OverlayTextureSubIds = new int[8] { -1, -1, -1, -1, -1, -1, -1, -1 }
		};
		if (faceNeighborOffsets == null || !currentBlock.CanReceiveBleed)
		{
			return result;
		}
		int currentPriority = currentBlock.EffectiveBleedPriority;
		int[] offsets = faceNeighborOffsets[tileSide];
		int occlusionOffset = faceOcclusionOffsets[tileSide];
		string currentGroup = currentBlock.BleedGroup;
		for (int i = 0; i < 8; i++)
		{
			int neighborIndex = extIndex3d + offsets[i];
			Block neighbor = chunkBlocks[neighborIndex];
			int neighborPriority = neighbor.EffectiveBleedPriority;
			if (neighborPriority <= currentPriority || (neighbor.BleedTopFaceOnly && tileSide != BlockFacing.UP.Index))
			{
				continue;
			}
			string neighborGroup = neighbor.BleedGroup;
			if ((!string.IsNullOrEmpty(neighborGroup) || !string.IsNullOrEmpty(currentGroup)) && neighborGroup != currentGroup)
			{
				continue;
			}
			Block occluder = chunkBlocks[neighborIndex + occlusionOffset];
			if (!occluder.SideSolid[tileSide] && occluder.BlockId != neighbor.BlockId && (occluder.BlockId == 0 || !occluder.BlocksTextureBleed))
			{
				result.HasBleed = true;
				result.BleedPriorities[i] = neighborPriority;
				if (((fastBlockTextureSubidsByBlockAndFace != null) ? fastBlockTextureSubidsByBlockAndFace[neighbor.BlockId] : null) != null)
				{
					result.BleedTextureSubIds[i] = fastBlockTextureSubidsByBlockAndFace[neighbor.BlockId][tileSide];
				}
				if (overlayTextureSubIdsByBlockId != null)
				{
					result.OverlayTextureSubIds[i] = overlayTextureSubIdsByBlockId[neighbor.BlockId];
				}
			}
		}
		return result;
	}
}
