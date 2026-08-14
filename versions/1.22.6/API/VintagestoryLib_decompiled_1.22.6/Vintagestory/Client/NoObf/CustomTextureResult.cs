using Vintagestory.API.Client;

namespace Vintagestory.Client.NoObf;

public struct CustomTextureResult
{
	public TextureAtlasPosition TexturePosition;

	public int AtlasNumber;

	public float OrigMinU;

	public float OrigMaxU;

	public float OrigMinV;

	public float OrigMaxV;

	public void RemapUV(float srcU, float srcV, out float dstU, out float dstV)
	{
		float rangeU = OrigMaxU - OrigMinU;
		float rangeV = OrigMaxV - OrigMinV;
		float normU = ((rangeU > 0.0001f) ? ((srcU - OrigMinU) / rangeU) : 0f);
		float normV = ((rangeV > 0.0001f) ? ((srcV - OrigMinV) / rangeV) : 0f);
		if (normU < 0f)
		{
			normU = 0f;
		}
		else if (normU > 1f)
		{
			normU = 1f;
		}
		if (normV < 0f)
		{
			normV = 0f;
		}
		else if (normV > 1f)
		{
			normV = 1f;
		}
		dstU = TexturePosition.x1 + normU * (TexturePosition.x2 - TexturePosition.x1);
		dstV = TexturePosition.y1 + normV * (TexturePosition.y2 - TexturePosition.y1);
	}
}
