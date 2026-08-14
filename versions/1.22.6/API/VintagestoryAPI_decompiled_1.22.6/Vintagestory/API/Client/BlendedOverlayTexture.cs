using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Client;

/// <summary>
/// Defines a texture to be overlayed on another texture.
/// </summary>
[DocumentAsJson]
public class BlendedOverlayTexture
{
	/// <summary>
	/// The path to the texture to use as an overlay.
	/// </summary>
	[DocumentAsJson("Required", "", false)]
	public AssetLocation Base;

	/// <summary>
	/// The type of blend for each pixel.
	/// </summary>
	[DocumentAsJson("Optional", "Normal", false)]
	public EnumColorBlendMode BlendMode;

	public BlendedOverlayTexture Clone()
	{
		return new BlendedOverlayTexture
		{
			Base = Base.Clone(),
			BlendMode = BlendMode
		};
	}

	public override string ToString()
	{
		return Base.ToString() + "-b" + BlendMode;
	}

	public void ToString(StringBuilder sb)
	{
		sb.Append(Base.ToString());
		sb.Append("-b");
		sb.Append(BlendMode);
	}
}
