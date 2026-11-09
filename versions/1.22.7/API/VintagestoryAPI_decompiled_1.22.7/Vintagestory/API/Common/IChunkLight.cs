namespace Vintagestory.API.Common;

public interface IChunkLight
{
	bool IsNull { get; }

	int GetSunlight(int index3d);

	void SetSunlight(int index3d, int sunlevel);

	void SetSunlight_Buffered(int index3d, int sunlevel);

	int GetBlocklight(int index3d);

	void SetBlocklight(int index3d, int lightlevel);

	void SetBlocklight_Buffered(int index3d, int lightlevel);

	void ClearWithSunlight(ushort sunLight);

	void FillWithSunlight(ushort sunLight);

	void ClearLight();

	void ClearAllSunlight();
}
