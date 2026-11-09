using Vintagestory.API;
using Vintagestory.API.Client;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class TextAreaConfig
{
	[DocumentAsJson("Optional", "160", false)]
	public int MaxWidth = 160;

	[DocumentAsJson("Optional", "165", false)]
	public int MaxHeight = 165;

	[DocumentAsJson("Optional", "20", false)]
	public float FontSize = 20f;

	[DocumentAsJson("Optional", "false", false)]
	public bool BoldFont;

	[DocumentAsJson("Optional", "Top", false)]
	public EnumVerticalAlign VerticalAlign;

	[DocumentAsJson("Optional", "sans-serif", false)]
	public string FontName = GuiStyle.StandardFontName;

	[DocumentAsJson("Optional", "0", false)]
	public float textVoxelOffsetX;

	[DocumentAsJson("Optional", "0", false)]
	public float textVoxelOffsetY;

	[DocumentAsJson("Optional", "0", false)]
	public float textVoxelOffsetZ;

	[DocumentAsJson("Optional", "14", false)]
	public float textVoxelWidth = 14f;

	[DocumentAsJson("Optional", "6.5", false)]
	public float textVoxelHeight = 6.5f;

	[DocumentAsJson("Optional", "false", false)]
	public bool WithScrollbar;

	public TextAreaConfig CopyWithFontSize(float fontSize)
	{
		return new TextAreaConfig
		{
			MaxWidth = MaxWidth,
			MaxHeight = MaxHeight,
			FontSize = fontSize,
			BoldFont = BoldFont,
			FontName = FontName,
			textVoxelWidth = textVoxelWidth,
			textVoxelHeight = textVoxelHeight,
			textVoxelOffsetX = textVoxelOffsetX,
			textVoxelOffsetY = textVoxelOffsetY,
			textVoxelOffsetZ = textVoxelOffsetZ,
			WithScrollbar = WithScrollbar,
			VerticalAlign = VerticalAlign
		};
	}
}
