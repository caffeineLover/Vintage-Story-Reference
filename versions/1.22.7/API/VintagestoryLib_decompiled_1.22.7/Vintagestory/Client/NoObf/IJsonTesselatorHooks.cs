using Vintagestory.API.Client;

namespace Vintagestory.Client.NoObf;

public interface IJsonTesselatorHooks
{
	bool TryGetCustomTexture(TCTCache vars, int localFace, int worldFace, float[] tfMatrix, MeshData sourceMesh, int faceIndex, int shapeRotationSteps, out CustomTextureResult result);
}
