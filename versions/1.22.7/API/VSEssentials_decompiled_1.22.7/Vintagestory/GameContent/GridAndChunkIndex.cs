namespace Vintagestory.GameContent;

public struct GridAndChunkIndex(int gridIndex, long chunkIndex)
{
	public int GridIndex = gridIndex;

	public long ChunkIndex = chunkIndex;
}
