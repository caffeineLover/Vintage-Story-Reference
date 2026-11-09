using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Common.Entities;

public class EntityClientProperties(JsonObject[] behaviors, Dictionary<string, JsonObject> commonConfigs) : EntitySidedProperties(behaviors, commonConfigs)
{
	/// <summary>
	/// Set by the game client
	/// </summary>
	public EntityRenderer Renderer;

	/// <summary>
	/// Name of there renderer system that draws this entity
	/// </summary>
	public string RendererName;

	/// <summary>
	/// Directory of all available textures. First one will be default one
	/// <br />Note: from game version 1.20.4, this is <b>null on server-side</b> (except during asset loading start-up stage)
	/// </summary>
	public IDictionary<string, CompositeTexture> Textures = new FastSmallDictionary<string, CompositeTexture>(0);

	/// <summary>
	/// Set by a server at the end of asset loading, immediately prior to setting Textures to null; relevant to spawning entities with variant textures
	/// </summary>
	public int TexturesAlternatesCount;

	/// <summary>
	/// The glow level for the entity.
	/// </summary>
	public int GlowLevel;

	/// <summary>
	/// Makes entities pitch forward and backwards when stepping
	/// </summary>
	public bool PitchStep = true;

	/// <summary>
	/// The shape of the entity
	/// </summary>
	public CompositeShape Shape;

	/// <summary>
	/// Only loaded for World.EntityTypes instances of EntityProperties, because it makes no sense to have 1000 loaded entities needing to load 1000 shapes. During entity load/spawn this value is assigned however.
	/// On the client it gets set by the EntityTextureAtlasManager.
	/// On the server by the EntitySimulation system
	/// </summary>
	public Shape LoadedShape;

	public Shape[] LoadedAlternateShapes;

	/// <summary>
	/// The shape for this particular entity who owns this properties object
	/// </summary>
	public Shape LoadedShapeForEntity;

	public CompositeShape ShapeForEntity;

	/// <summary>
	/// The size of the entity (default: 1f)
	/// </summary>
	public float Size = 1f;

	/// <summary>
	/// The rate at which the entity's size grows with age - used for chicks and other small baby animals
	/// </summary>
	public float SizeGrowthFactor;

	/// <summary>
	/// The animations of the entity.
	/// </summary>
	public AnimationMetaData[] Animations;

	public Dictionary<string, AnimationMetaData> AnimationsByMetaCode = new Dictionary<string, AnimationMetaData>(StringComparer.OrdinalIgnoreCase);

	public Dictionary<uint, AnimationMetaData> AnimationsByCrc32 = new Dictionary<uint, AnimationMetaData>();

	/// <summary>
	/// Used by various renderers to retrieve the entities texture it should be drawn with
	/// </summary>
	public virtual CompositeTexture Texture
	{
		get
		{
			if (Textures != null && Textures.Count != 0)
			{
				return Textures.First().Value;
			}
			return null;
		}
	}

	/// <summary>
	/// Returns the first texture in Textures dict
	/// </summary>
	public CompositeTexture FirstTexture
	{
		get
		{
			if (Textures != null && Textures.Count != 0)
			{
				return Textures.First().Value;
			}
			return null;
		}
	}

	public void DetermineLoadedShape(long forEntityId)
	{
		if (LoadedAlternateShapes != null && LoadedAlternateShapes.Length != 0)
		{
			int index = GameMath.MurmurHash3Mod(0, 0, (int)forEntityId, 1 + LoadedAlternateShapes.Length);
			if (index == 0)
			{
				LoadedShapeForEntity = LoadedShape;
				ShapeForEntity = Shape;
			}
			else
			{
				LoadedShapeForEntity = LoadedAlternateShapes[index - 1];
				ShapeForEntity = Shape.Alternates[index - 1];
			}
		}
		else
		{
			LoadedShapeForEntity = LoadedShape;
			ShapeForEntity = Shape;
		}
	}

	/// <summary>
	/// Initializes the client properties.
	/// </summary>
	/// <param name="entityTypeCode"></param>
	/// <param name="world"></param>
	public void Init(AssetLocation entityTypeCode, IWorldAccessor world)
	{
		if (Animations != null)
		{
			for (int i = 0; i < Animations.Length; i++)
			{
				AnimationMetaData animMeta = Animations[i];
				animMeta.Init();
				if (animMeta.Animation != null)
				{
					AnimationsByMetaCode[animMeta.Code] = animMeta;
				}
				if (animMeta.Animation != null)
				{
					AnimationsByCrc32[animMeta.CodeCrc32] = animMeta;
				}
			}
		}
		if (world != null)
		{
			EntityClientProperties cprop = world.EntityTypes.FirstOrDefault((EntityProperties et) => et.Code.Equals(entityTypeCode))?.Client;
			LoadedShape = cprop?.LoadedShape;
			LoadedAlternateShapes = cprop?.LoadedAlternateShapes;
		}
	}

	/// <summary>
	/// Does not clone textures, but does clone Shape
	/// <br />Note: This method does not clone the LoadedShape (nor the LoadedAlternateShapes, if present). The LoadedShape (and any alternate) is not normally modified after entityType loading. An Entity created at runtime (each Entity carries a clone of its EntityProperties) should not normally require its own unique copy of the LoadedShape or the LoadedAlternateShapes. Exceptionally, an entity needing to clone and modify LoadedShape or one of the LoadedAlternateShapes (e.g. EntityDressedHumanoid, or a boat furling and unfurling sails) should do so in its own custom OnTesselation() method, likely accessing the shape via EntityClientProperties.LoadedShapeForEntity.
	/// </summary>
	/// <returns></returns>
	public override EntitySidedProperties Clone()
	{
		Dictionary<string, AnimationMetaData> newAnimationsByMetaData = new Dictionary<string, AnimationMetaData>(StringComparer.OrdinalIgnoreCase);
		Dictionary<uint, AnimationMetaData> animsByCrc32 = new Dictionary<uint, AnimationMetaData>();
		AnimationMetaData[] newAnimations = null;
		if (Animations != null)
		{
			AnimationMetaData[] oldAnimations = Animations;
			newAnimations = new AnimationMetaData[oldAnimations.Length];
			for (int i = 0; i < newAnimations.Length; i++)
			{
				AnimationMetaData clonedAnimation = (newAnimations[i] = oldAnimations[i].Clone());
				if (AnimationsByMetaCode.ContainsKey(clonedAnimation.Code))
				{
					newAnimationsByMetaData[clonedAnimation.Code] = clonedAnimation;
					animsByCrc32[clonedAnimation.CodeCrc32] = clonedAnimation;
				}
			}
		}
		return new EntityClientProperties(BehaviorsAsJsonObj, null)
		{
			Textures = ((Textures == null) ? null : new FastSmallDictionary<string, CompositeTexture>(Textures)),
			TexturesAlternatesCount = TexturesAlternatesCount,
			RendererName = RendererName,
			GlowLevel = GlowLevel,
			PitchStep = PitchStep,
			Size = Size,
			SizeGrowthFactor = SizeGrowthFactor,
			Shape = Shape?.Clone(),
			LoadedAlternateShapes = LoadedAlternateShapes,
			Animations = newAnimations,
			AnimationsByMetaCode = newAnimationsByMetaData,
			AnimationsByCrc32 = animsByCrc32
		};
	}

	public virtual void FreeRAMServer()
	{
		CompositeTexture[] alternates = FirstTexture?.Alternates;
		TexturesAlternatesCount = ((alternates != null) ? alternates.Length : 0);
		Textures = null;
		if (Animations != null)
		{
			AnimationMetaData[] AnimationsMetaData = Animations;
			for (int i = 0; i < AnimationsMetaData.Length; i++)
			{
				AnimationsMetaData[i].DeDuplicate();
			}
		}
	}
}
