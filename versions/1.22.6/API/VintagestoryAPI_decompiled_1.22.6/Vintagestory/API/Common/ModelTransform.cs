using System.Runtime.Serialization;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common;

/// <summary>
/// Used for transformations applied to a block or item model. Uses values from <see cref="T:Vintagestory.API.Common.ModelTransformNoDefaults" /> but will assign defaults if not included.
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
public class ModelTransform : ModelTransformNoDefaults
{
	/// <summary>
	/// Gets a new model with all values set to default.
	/// </summary>
	public static ModelTransform NoTransform => new ModelTransform().EnsureDefaultValues();

	public ModelTransform(ModelTransformNoDefaults baseTf, ModelTransform defaults)
	{
		Rotate = baseTf.Rotate;
		if (baseTf.Translation.Equals(ModelTransformNoDefaults.defaultTf))
		{
			Translation = defaults.Translation;
		}
		else
		{
			Translation = baseTf.Translation;
		}
		if (baseTf.Rotation.Equals(ModelTransformNoDefaults.defaultTf))
		{
			Rotation = defaults.Rotation;
		}
		else
		{
			Rotation = baseTf.Rotation;
		}
		Origin = baseTf.Origin;
		ScaleXYZ = baseTf.ScaleXYZ;
	}

	public ModelTransform()
	{
	}

	/// <summary>
	/// Scale = 1, No Translation, Rotation by -45 deg in Y-Axis
	/// </summary>
	/// <returns></returns>
	public static ModelTransform BlockDefaultGui()
	{
		return new ModelTransform
		{
			Translation = default(FastVec3f),
			Rotation = new FastVec3f(-22.6f, -45.3f, 0f),
			Scale = 1f
		};
	}

	/// <summary>
	/// Scale = 1, No Translation, Rotation by -45 deg in Y-Axis
	/// </summary>
	/// <returns></returns>
	public static ModelTransform BlockDefaultFp()
	{
		return new ModelTransform
		{
			Translation = new FastVec3f(0f, -0.15f, 0.5f),
			Rotation = new FastVec3f(0f, -20f, 0f),
			Scale = 1.3f
		};
	}

	/// <summary>
	/// Scale = 1, No Translation, Rotation by -45 deg in Y-Axis
	/// </summary>
	/// <returns></returns>
	public static ModelTransform BlockDefaultTp()
	{
		return new ModelTransform
		{
			Translation = new FastVec3f(-2.1f, -1.8f, -1.5f),
			Rotation = new FastVec3f(0f, -45f, -25f),
			Scale = 0.3f
		};
	}

	/// <summary>
	/// Scale = 1, No Translation, Rotation by -45 deg in Y-Axis, 1.5x scale
	/// </summary>
	/// <returns></returns>
	public static ModelTransform BlockDefaultGround()
	{
		return new ModelTransform
		{
			Translation = default(FastVec3f),
			Rotation = new FastVec3f(0f, -45f, 0f),
			Origin = new FastVec3f(0.5f, 0f, 0.5f),
			Scale = 1.5f
		};
	}

	/// <summary>
	/// Scale = 1, No Translation, No Rotation
	/// </summary>
	/// <returns></returns>
	public static ModelTransform ItemDefaultGui()
	{
		return new ModelTransform
		{
			Translation = new FastVec3f(0f, 0f, 0f),
			Rotation = new FastVec3f(0f, 0f, 0f),
			Origin = new FastVec3f(0.5f, 0.5f, 0.5f),
			Scale = 1f,
			Rotate = false
		};
	}

	/// <summary>
	/// Scale = 1, No Translation, Rotation by 180 deg in X-Axis
	/// </summary>
	/// <returns></returns>
	public static ModelTransform ItemDefaultFp()
	{
		return new ModelTransform
		{
			Translation = new FastVec3f(0.05f, 0f, 0f),
			Rotation = new FastVec3f(180f, 90f, -30f),
			Scale = 1f
		};
	}

	/// <summary>
	/// Scale = 1, No Translation, Rotation by 180 deg in X-Axis
	/// </summary>
	/// <returns></returns>
	public static ModelTransform ItemDefaultTp()
	{
		return new ModelTransform
		{
			Translation = new FastVec3f(-1.5f, -1.6f, -1.4f),
			Rotation = new FastVec3f(0f, -62f, 18f),
			Scale = 0.33f
		};
	}

	/// <summary>
	/// Creates a default transform for a model that is now on the ground
	/// </summary>
	/// <returns></returns>
	public static ModelTransform ItemDefaultGround()
	{
		return new ModelTransform
		{
			Translation = default(FastVec3f),
			Rotation = new FastVec3f(90f, 0f, 0f),
			Origin = new FastVec3f(0.5f, 0.5f, 0.53f),
			Scale = 1.5f
		};
	}

	/// <summary>
	/// Makes sure that Translation and Rotation is not null
	/// </summary>
	public ModelTransform EnsureDefaultValues()
	{
		if (Translation.Equals(ModelTransformNoDefaults.defaultTf))
		{
			Translation = default(FastVec3f);
		}
		if (Rotation.Equals(ModelTransformNoDefaults.defaultTf))
		{
			Rotation = default(FastVec3f);
		}
		return this;
	}

	public ModelTransform WithRotation(Vec3f rot)
	{
		Rotation = new FastVec3f(rot);
		return this;
	}

	/// <summary>
	/// Clones this specific transform.
	/// </summary>
	/// <returns></returns>
	public ModelTransform Clone()
	{
		return new ModelTransform
		{
			Rotate = Rotate,
			Rotation = Rotation,
			Translation = Translation,
			ScaleXYZ = ScaleXYZ,
			Origin = Origin
		};
	}

	public void ToTreeAttribute(TreeAttribute tree)
	{
		tree.SetFloat("rotateX", Rotation.X);
		tree.SetFloat("rotateY", Rotation.Y);
		tree.SetFloat("rotateZ", Rotation.Z);
		tree.SetFloat("translateX", Translation.X);
		tree.SetFloat("translateY", Translation.Y);
		tree.SetFloat("translateZ", Translation.Z);
		tree.SetFloat("scaleX", ScaleXYZ.X);
		tree.SetFloat("scaleY", ScaleXYZ.Y);
		tree.SetFloat("scaleZ", ScaleXYZ.Z);
		tree.SetFloat("originX", Origin.X);
		tree.SetFloat("originY", Origin.Y);
		tree.SetFloat("originZ", Origin.Z);
		tree.SetBool("rotate", Rotate);
	}

	public ModelTransform FromTreeAttribute(TreeAttribute tree)
	{
		Rotation.X = tree.GetFloat("rotateX");
		Rotation.Y = tree.GetFloat("rotateY");
		Rotation.Z = tree.GetFloat("rotateZ");
		Translation.X = tree.GetFloat("translateX");
		Translation.Y = tree.GetFloat("translateY");
		Translation.Z = tree.GetFloat("translateZ");
		ScaleXYZ.X = tree.GetFloat("scaleX");
		ScaleXYZ.Y = tree.GetFloat("scaleY");
		ScaleXYZ.Z = tree.GetFloat("scaleZ");
		Origin.X = tree.GetFloat("originX");
		Origin.Y = tree.GetFloat("originY");
		Origin.Z = tree.GetFloat("originZ");
		Rotate = tree.GetBool("rotate", Rotate);
		return this;
	}

	public static ModelTransform CreateFromTreeAttribute(TreeAttribute tree)
	{
		return new ModelTransform().FromTreeAttribute(tree);
	}

	[OnDeserialized]
	internal void OnDeserializedMethod(StreamingContext context)
	{
		if (Translation.Equals(ModelTransformNoDefaults.defaultTf))
		{
			Translation = default(FastVec3f);
		}
		if (Rotation.Equals(ModelTransformNoDefaults.defaultTf))
		{
			Rotation = default(FastVec3f);
		}
	}
}
