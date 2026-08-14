using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Client;

/// <summary>
/// An expanded, atlas-friendly version of a CompositeTexture
/// </summary>
public class BakedCompositeTexture
{
	/// <summary>
	/// Unique identifier for this texture
	/// </summary>
	public int TextureSubId;

	/// <summary>
	/// The Base name and Overlay concatenated (if there was any defined)
	/// </summary>
	public AssetLocation BakedName;

	/// <summary>
	/// The base name and overlays as array
	/// </summary>
	public AssetLocation[] TextureFilenames;

	/// <summary>
	/// If non-null also contains BakedName
	/// </summary>
	public BakedCompositeTexture[] BakedVariants;

	/// <summary>
	/// If non-null also contains BakedName
	/// </summary>
	public BakedCompositeTexture[] BakedTiles;

	public int TilesWidth;

	/// <summary>
	/// For tiled textures, returns the selector index for which one of the tiles to use, for the given x,y,z position and tileSide
	/// </summary>
	/// <param name="tiles"></param>
	/// <param name="tileSide"></param>
	/// <param name="posX"></param>
	/// <param name="posY"></param>
	/// <param name="posZ"></param>
	/// <returns></returns>
	public static int GetTiledTexturesSelector(BakedCompositeTexture[] tiles, int tileSide, int posX, int posY, int posZ)
	{
		BakedCompositeTexture obj = tiles[0];
		int width = obj.TilesWidth;
		int height = tiles.Length / width;
		string name = obj.BakedName.Path;
		int index = name.IndexOf('@');
		int rotation = 0;
		if (index > 0)
		{
			int num = index + 1;
			int.TryParse(name.Substring(num, name.Length - num), out rotation);
			rotation /= 90;
		}
		int x = 0;
		int y = 0;
		int z = 0;
		switch (tileSide)
		{
		case 0:
			x = posX;
			y = posZ;
			z = posY;
			break;
		case 1:
			x = posZ;
			y = -posX;
			z = posY;
			break;
		case 2:
			x = -posX;
			y = posZ;
			z = posY;
			break;
		case 3:
			x = -posZ;
			y = -posX;
			z = posY;
			break;
		case 4:
			x = posX;
			y = posY;
			z = posZ;
			break;
		case 5:
			x = posX;
			y = posY;
			z = -posZ;
			break;
		}
		return rotation switch
		{
			0 => GameMath.Mod(-x + y, width) + width * GameMath.Mod(-z, height), 
			1 => GameMath.Mod(z + y, width) + width * GameMath.Mod(x, height), 
			2 => GameMath.Mod(x + y, width) + width * GameMath.Mod(z, height), 
			3 => GameMath.Mod(-z + y, width) + width * GameMath.Mod(-x, height), 
			_ => 0, 
		};
	}
}
