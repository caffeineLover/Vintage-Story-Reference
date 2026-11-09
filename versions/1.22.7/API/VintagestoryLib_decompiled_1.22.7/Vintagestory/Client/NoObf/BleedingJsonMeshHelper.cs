using Vintagestory.API.Client;

namespace Vintagestory.Client.NoObf;

internal class BleedingJsonMeshHelper : ITerrainMeshPool, IMeshPoolSupplier
{
	internal TCTCache vars;

	internal BleedingJsonTesselator tess;

	internal JsonTesselator jsonTess;

	public void AddMeshData(MeshData sourceMesh, float[] transformationMatrix, int lodLevel = 1)
	{
		if (sourceMesh != null)
		{
			jsonTess.AddJsonModelDataToMesh(sourceMesh, lodLevel, vars, this, transformationMatrix, tess, 0);
		}
	}

	public void AddMeshData(MeshData sourceMesh, int lodLevel = 1)
	{
		if (sourceMesh != null)
		{
			jsonTess.AddJsonModelDataToMesh(sourceMesh, lodLevel, vars, this, null, tess, 0);
		}
	}

	public void AddMeshData(MeshData sourceMesh, ColorMapData colorMapData, int lodlevel = 1)
	{
		vars.ColorMapData = colorMapData;
		AddMeshData(sourceMesh, lodlevel);
	}

	public MeshData GetMeshPoolForPass(int textureId, EnumChunkRenderPass forRenderPass, int lodLevel)
	{
		return vars.tct.GetMeshPoolForPass(textureId, forRenderPass, lodLevel);
	}
}
