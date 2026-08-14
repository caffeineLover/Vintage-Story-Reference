using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.API.Common;

/// <summary>
/// A shape element built from JSON data within the model.
/// </summary>
[JsonObject(MemberSerialization.OptIn)]
public class ShapeElement
{
	/// <summary>
	/// A static reference to the logger (null on a server) - we don't want to hold a reference to the platform or api in every ShapeElement
	/// </summary>
	public static ILogger Logger;

	public static object locationForLogging;

	/// <summary>
	/// The name of the ShapeElement
	/// </summary>
	[JsonProperty]
	public string? Name;

	[JsonProperty]
	public double[]? From;

	[JsonProperty]
	public double[]? To;

	/// <summary>
	/// Whether or not the shape element is shaded.
	/// </summary>
	[JsonProperty]
	public bool Shade = true;

	[JsonProperty]
	public bool GradientShade;

	/// <summary>
	/// The faces of the shape element by name (will normally be null except during object deserialization: use FacesResolved instead!)
	/// </summary>
	[JsonProperty]
	[Obsolete("Use FacesResolved instead")]
	public Dictionary<string, ShapeElementFace>? Faces;

	/// <summary>
	/// An array holding the faces of this shape element in BlockFacing order: North, East, South, West, Up, Down.  May be null if not present or not enabled.
	/// <br />Note: from game version 1.20.4, this is <b>null on server-side</b> (except during asset loading start-up stage)
	/// </summary>
	public ShapeElementFace[]? FacesResolved = new ShapeElementFace[6];

	/// <summary>
	/// The origin point for rotation.
	/// </summary>
	[JsonProperty]
	public double[]? RotationOrigin;

	/// <summary>
	/// The forward vertical rotation of the shape element.
	/// </summary>
	[JsonProperty]
	public double RotationX;

	/// <summary>
	/// The forward vertical rotation of the shape element.
	/// </summary>
	[JsonProperty]
	public double RotationY;

	/// <summary>
	/// The left/right tilt of the shape element
	/// </summary>
	[JsonProperty]
	public double RotationZ;

	/// <summary>
	/// How far away are the left/right sides of the shape from the center
	/// </summary>
	[JsonProperty]
	public double ScaleX = 1.0;

	/// <summary>
	/// How far away are the top/bottom sides of the shape from the center
	/// </summary>
	[JsonProperty]
	public double ScaleY = 1.0;

	/// <summary>
	/// How far away are the front/back sides of the shape from the center.
	/// </summary>
	[JsonProperty]
	public double ScaleZ = 1.0;

	[JsonProperty]
	public string? ClimateColorMap;

	[JsonProperty]
	public string? SeasonColorMap;

	[JsonProperty]
	public short RenderPass = -1;

	[JsonProperty]
	public short ZOffset;

	/// <summary>
	/// Set this to true to disable randomDrawOffset and randomRotations on this specific element (e.g. used for the ice element of Coopers Reeds in Ice)
	/// </summary>
	[JsonProperty]
	public bool DisableRandomDrawOffset;

	/// <summary>
	/// The child shapes of this shape element
	/// </summary>
	[JsonProperty]
	public ShapeElement[]? Children;

	/// <summary>
	/// The attachment points for this shape.
	/// </summary>
	[JsonProperty]
	public AttachmentPoint[]? AttachmentPoints;

	/// <summary>
	/// The "remote" parent for this element
	/// </summary>
	[JsonProperty]
	public string? StepParentName;

	/// <summary>
	/// The parent element reference for this shape.
	/// </summary>
	public ShapeElement? ParentElement;

	/// <summary>
	/// The id of the joint attached to the parent element.
	/// </summary>
	public int JointId;

	/// <summary>
	/// For entity animations
	/// </summary>
	public int Color = -1;

	public float DamageEffect;

	public float[]? inverseModelTransform;

	protected static ElementPose? noTransform = new ElementPose();

	/// <summary>
	/// Walks the element tree and collects all parents, starting with the root element
	/// </summary>
	/// <returns></returns>
	public List<ShapeElement> GetParentPath()
	{
		List<ShapeElement> path = new List<ShapeElement>();
		for (ShapeElement parentElem = ParentElement; parentElem != null; parentElem = parentElem.ParentElement)
		{
			path.Add(parentElem);
		}
		path.Reverse();
		return path;
	}

	public int CountParents()
	{
		int count = 0;
		for (ShapeElement parentElem = ParentElement; parentElem != null; parentElem = parentElem.ParentElement)
		{
			count++;
		}
		return count;
	}

	public void CacheInverseTransformMatrix()
	{
		if (inverseModelTransform == null)
		{
			inverseModelTransform = GetInverseModelMatrix();
		}
	}

	/// <summary>
	/// Returns the full inverse model matrix (includes all parent transforms)
	/// </summary>
	/// <returns></returns>
	public float[] GetInverseModelMatrix()
	{
		List<ShapeElement> elems = GetParentPath();
		Span<float> modelTransform = stackalloc float[16];
		Mat4f.NewIdentity(modelTransform);
		float[] tmp = new float[16];
		for (int i = 0; i < elems.Count; i++)
		{
			ShapeElement elem = elems[i];
			Mat4f.Identity(tmp);
			Mat4f.Mul(modelTransform, elem.GetLocalTransformMatrix(0, tmp));
		}
		Mat4f.Identity(tmp);
		Mat4f.Mul(modelTransform, GetLocalTransformMatrix(0, tmp));
		Mat4f.Invert(modelTransform);
		return Mat4f.Copy(tmp, modelTransform);
	}

	public void SetJointId(int jointId)
	{
		JointId = jointId;
		ShapeElement[] Children = this.Children;
		if (Children != null)
		{
			for (int i = 0; i < Children.Length; i++)
			{
				Children[i].SetJointId(jointId);
			}
		}
	}

	public void ResolveReferences()
	{
		ShapeElement[] Children = this.Children;
		if (Children != null)
		{
			foreach (ShapeElement obj in Children)
			{
				obj.ParentElement = this;
				obj.ResolveReferences();
			}
		}
		AttachmentPoint[] AttachmentPoints = this.AttachmentPoints;
		if (AttachmentPoints != null)
		{
			for (int j = 0; j < AttachmentPoints.Length; j++)
			{
				AttachmentPoints[j].ParentElement = this;
			}
		}
	}

	public void TrimTextureNamesAndResolveFaces()
	{
		if (Faces != null)
		{
			foreach (KeyValuePair<string, ShapeElementFace> val in Faces)
			{
				ShapeElementFace f = val.Value;
				if (f.Enabled)
				{
					BlockFacing facing = BlockFacing.FromFirstLetter(val.Key);
					if (facing == null)
					{
						Logger?.Warning("Shape element in " + locationForLogging?.ToString() + ": Unknown facing '" + facing.Code + "'. Ignoring face.");
					}
					else
					{
						FacesResolved[facing.Index] = f;
						f.Texture = f.Texture.Substring(1).DeDuplicate();
					}
				}
			}
		}
		Faces = null;
		if (Children != null)
		{
			ShapeElement[] children = Children;
			for (int i = 0; i < children.Length; i++)
			{
				children[i].TrimTextureNamesAndResolveFaces();
			}
		}
		Name = Name.DeDuplicate();
		StepParentName = StepParentName.DeDuplicate();
		AttachmentPoint[] AttachmentPoints = this.AttachmentPoints;
		if (AttachmentPoints != null)
		{
			for (int j = 0; j < AttachmentPoints.Length; j++)
			{
				AttachmentPoints[j].DeDuplicate();
			}
		}
	}

	public float[] GetLocalTransformMatrix(int animVersion, float[] output = null, ElementPose tf = null)
	{
		if (tf == null)
		{
			tf = noTransform;
		}
		if (output == null)
		{
			output = Mat4f.Create();
		}
		Span<float> matrix = output;
		float origin0 = 0f;
		float origin1 = 0f;
		float origin2 = 0f;
		if (RotationOrigin != null)
		{
			origin0 = (float)RotationOrigin[0] / 16f;
			origin1 = (float)RotationOrigin[1] / 16f;
			origin2 = (float)RotationOrigin[2] / 16f;
		}
		if (animVersion == 1)
		{
			Mat4f.Translate(matrix, origin0, origin1, origin2);
			Mat4f.Scale(matrix, (float)ScaleX, (float)ScaleY, (float)ScaleZ);
			Mat4f.RotateByXYZ(matrix, (float)(RotationX * 0.01745329238474369), (float)(RotationY * 0.01745329238474369), (float)(RotationZ * 0.01745329238474369));
			Mat4f.Translate(matrix, 0f - origin0 + (float)From[0] / 16f + tf.translateX, 0f - origin1 + (float)From[1] / 16f + tf.translateY, 0f - origin2 + (float)From[2] / 16f + tf.translateZ);
			Mat4f.Scale(matrix, tf.scaleX, tf.scaleY, tf.scaleZ);
			Mat4f.RotateByXYZ(matrix, (tf.degX + tf.degOffX) * ((float)Math.PI / 180f), (tf.degY + tf.degOffY) * ((float)Math.PI / 180f), (tf.degZ + tf.degOffZ) * ((float)Math.PI / 180f));
		}
		else
		{
			Mat4f.Translate(matrix, origin0, origin1, origin2);
			Mat4f.RotateByXYZ(matrix, (float)(RotationX + (double)tf.degX + (double)tf.degOffX) * ((float)Math.PI / 180f), (float)(RotationY + (double)tf.degY + (double)tf.degOffY) * ((float)Math.PI / 180f), (float)(RotationZ + (double)tf.degZ + (double)tf.degOffZ) * ((float)Math.PI / 180f));
			Mat4f.Scale(matrix, (float)ScaleX * tf.scaleX, (float)ScaleY * tf.scaleY, (float)ScaleZ * tf.scaleZ);
			Mat4f.Translate(matrix, (float)From[0] / 16f + tf.translateX - origin0, (float)From[1] / 16f + tf.translateY - origin1, (float)From[2] / 16f + tf.translateZ - origin2);
		}
		return output;
	}

	public ShapeElement Clone()
	{
		ShapeElement elem = new ShapeElement
		{
			AttachmentPoints = (AttachmentPoint[])AttachmentPoints?.Clone(),
			FacesResolved = (ShapeElementFace[])FacesResolved?.Clone(),
			From = (double[])From?.Clone(),
			To = (double[])To?.Clone(),
			inverseModelTransform = (float[])inverseModelTransform?.Clone(),
			JointId = JointId,
			RenderPass = RenderPass,
			RotationX = RotationX,
			RotationY = RotationY,
			RotationZ = RotationZ,
			RotationOrigin = (double[])RotationOrigin?.Clone(),
			SeasonColorMap = SeasonColorMap,
			ClimateColorMap = ClimateColorMap,
			StepParentName = StepParentName,
			Shade = Shade,
			DisableRandomDrawOffset = DisableRandomDrawOffset,
			ZOffset = ZOffset,
			GradientShade = GradientShade,
			ScaleX = ScaleX,
			ScaleY = ScaleY,
			ScaleZ = ScaleZ,
			Name = Name
		};
		ShapeElement[] Children = this.Children;
		if (Children != null)
		{
			elem.Children = new ShapeElement[Children.Length];
			for (int i = 0; i < Children.Length; i++)
			{
				ShapeElement child = Children[i].Clone();
				child.ParentElement = elem;
				elem.Children[i] = child;
			}
		}
		return elem;
	}

	public void SetJointIdRecursive(int jointId)
	{
		JointId = jointId;
		ShapeElement[] Children = this.Children;
		if (Children != null)
		{
			for (int i = 0; i < Children.Length; i++)
			{
				Children[i].SetJointIdRecursive(jointId);
			}
		}
	}

	public void CacheInverseTransformMatrixRecursive()
	{
		CacheInverseTransformMatrix();
		ShapeElement[] Children = this.Children;
		if (Children != null)
		{
			for (int i = 0; i < Children.Length; i++)
			{
				Children[i].CacheInverseTransformMatrixRecursive();
			}
		}
	}

	public void WalkRecursive(Action<ShapeElement> onElem)
	{
		onElem(this);
		ShapeElement[] Children = this.Children;
		if (Children != null)
		{
			for (int i = 0; i < Children.Length; i++)
			{
				Children[i].WalkRecursive(onElem);
			}
		}
	}

	public bool HasFaces()
	{
		for (int i = 0; i < 6; i++)
		{
			if (FacesResolved[i] != null)
			{
				return true;
			}
		}
		return false;
	}

	public virtual void FreeRAMServer()
	{
		Faces = null;
		FacesResolved = null;
		ShapeElement[] Children = this.Children;
		if (Children != null)
		{
			for (int i = 0; i < Children.Length; i++)
			{
				Children[i].FreeRAMServer();
			}
		}
	}

	public override string ToString()
	{
		if (StepParentName != null)
		{
			if (ParentElement != null)
			{
				return Name + "(" + ParentElement.Name + ")";
			}
			return Name ?? "";
		}
		if (ParentElement != null)
		{
			return $"{Name}({ParentElement.Name}) -> {StepParentName}";
		}
		return Name + " -> " + StepParentName;
	}
}
