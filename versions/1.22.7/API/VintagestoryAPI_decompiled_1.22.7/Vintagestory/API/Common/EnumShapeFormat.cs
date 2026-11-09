namespace Vintagestory.API.Common;

/// <summary>
/// Types of shape that can be loaded by the game.
/// </summary>
[DocumentAsJson]
public enum EnumShapeFormat
{
	/// <summary>
	/// (Recommended) Imports a shape using the default JSON system.
	/// </summary>
	VintageStory,
	/// <summary>
	/// Imports a shape using an Obj file.
	/// </summary>
	Obj,
	/// <summary>
	/// Imports a shape using a Gltf file.
	/// </summary>
	GltfEmbedded
}
