using System;
using Newtonsoft.Json;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common;

/// <summary>
/// Controls the transformations of 3D shapes. Note that defaults change depending on where this class is used.
/// </summary>
/// <example>
/// Use '.tfedit' in game to help customize these values, just make sure to copy them into your json file when you finish.
/// <code language="json">
///             "tpHandTransform": {
///             	"translation": {
///             		"x": -0.87,
///             		"y": -0.01,
///             		"z": -0.56
///             	},
///             	"rotation": {
///             		"x": -90,
///             		"y": 0,
///             		"z": 0
///             	},
///             	"origin": {
///             		"x": 0.5,
///             		"y": 0,
///             		"z": 0.5
///             	},
///             	"scale": 0.8
///             },
/// </code>
/// </example>
[DocumentAsJson]
[JsonObject(MemberSerialization.OptIn)]
public class ModelTransformNoDefaults
{
	public static readonly FastVec3f defaultTf = new FastVec3f(-9.9E-05f, -9.9E-05f, -9.9E-05f);

	/// <summary>
	/// Offsetting
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "", false)]
	public FastVec3f Translation = defaultTf;

	/// <summary>
	/// Rotation in degrees
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "", false)]
	public FastVec3f Rotation = defaultTf;

	/// <summary>
	/// Rotation/Scaling Origin
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "0.5, 0.5, 0.5", false)]
	public FastVec3f Origin = new FastVec3f(0.5f, 0.5f, 0.5f);

	/// <summary>
	/// For Gui Transform: Whether to slowly spin in gui item preview 
	/// For Ground Transform: Whether to apply a random rotation to the dropped item
	/// No effect on other transforms
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "True", false)]
	public bool Rotate = true;

	/// <summary>
	/// Scaling per axis
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "1, 1, 1", false)]
	public FastVec3f ScaleXYZ = new FastVec3f(1f, 1f, 1f);

	/// <summary>
	/// Sets the same scale of an object for all axes.
	/// </summary>
	[JsonProperty]
	[DocumentAsJson("Optional", "", false)]
	public float Scale
	{
		set
		{
			ScaleXYZ.Set(value, value, value);
		}
	}

	/// <summary>
	/// Converts the transform into a matrix.
	/// </summary>
	public float[] AsMatrix
	{
		get
		{
			float[] mat = Mat4f.Create();
			Mat4f.Translate(mat, mat, Translation.X, Translation.Y, Translation.Z);
			Mat4f.Translate(mat, mat, Origin.X, Origin.Y, Origin.Z);
			if (Rotation.X != 0f || Rotation.Y != 0f || Rotation.Z != 0f)
			{
				Mat4f.RotateX(mat, mat, Rotation.X * ((float)Math.PI / 180f));
				Mat4f.RotateY(mat, mat, Rotation.Y * ((float)Math.PI / 180f));
				Mat4f.RotateZ(mat, mat, Rotation.Z * ((float)Math.PI / 180f));
			}
			Mat4f.Scale(mat, mat, ScaleXYZ.X, ScaleXYZ.Y, ScaleXYZ.Z);
			Mat4f.Translate(mat, mat, 0f - Origin.X, 0f - Origin.Y, 0f - Origin.Z);
			return mat;
		}
	}

	/// <summary>
	/// Clears the transformation values.
	/// </summary>
	public void Clear()
	{
		Rotation.Set(0f, 0f, 0f);
		Translation.Set(0f, 0f, 0f);
		Origin.Set(0f, 0f, 0f);
		Scale = 1f;
	}
}
