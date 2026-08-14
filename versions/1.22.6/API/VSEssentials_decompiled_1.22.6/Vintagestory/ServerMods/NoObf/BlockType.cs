using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.ServerMods.NoObf;

[DocumentAsJson]
[JsonObject(MemberSerialization.OptIn)]
public class BlockType : CollectibleType
{
	public static Cuboidf DefaultCollisionBox = new Cuboidf(0f, 0f, 0f, 1f, 1f, 1f);

	public static RotatableCube DefaultCollisionBoxR = new RotatableCube(0f, 0f, 0f, 1f, 1f, 1f);

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public string EntityClass;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public BlockEntityBehaviorType[] EntityBehaviors = Array.Empty<BlockEntityBehaviorType>();

	[JsonProperty]
	[DocumentAsJson("Optional", "JSON", false)]
	public EnumDrawType DrawType = EnumDrawType.JSON;

	[JsonProperty]
	[DocumentAsJson("Optional", "XYZ", false)]
	public EnumRandomizeAxes RandomizeAxes;

	[JsonProperty]
	[DocumentAsJson("Optional", "False", false)]
	public bool RandomDrawOffset;

	[JsonProperty]
	[DocumentAsJson("Optional", "False", false)]
	public bool RandomizeRotations;

	[JsonProperty]
	[DocumentAsJson("Optional", "False", false)]
	public float RandomSizeAdjust;

	[JsonProperty]
	[DocumentAsJson("Optional", "Opaque", false)]
	public EnumChunkRenderPass RenderPass;

	[JsonProperty]
	[DocumentAsJson("Optional", "Default", false)]
	public EnumFaceCullMode FaceCullMode;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public CompositeShape ShapeInventory;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public CompositeShape Lod0Shape;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public CompositeShape Lod2Shape;

	[JsonProperty]
	[DocumentAsJson("Optional", "False", false)]
	public bool DoNotRenderAtLod2;

	[JsonProperty]
	[DocumentAsJson("Unused", "", false)]
	public bool Ambientocclusion = true;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public BlockSounds Sounds;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public Dictionary<string, CompositeTexture> TexturesInventory;

	[JsonProperty]
	[DocumentAsJson("Optional", "All true", false)]
	public Dictionary<string, bool> SideOpaque;

	[JsonProperty]
	[DocumentAsJson("Optional", "All true", false)]
	public Dictionary<string, bool> SideAo;

	[JsonProperty]
	[DocumentAsJson("Optional", "See description", false)]
	public Dictionary<string, bool> EmitSideAo;

	[JsonProperty]
	[DocumentAsJson("Optional", "All true", false)]
	public Dictionary<string, bool> SideSolid;

	[JsonProperty]
	[DocumentAsJson("Optional", "", false)]
	public Dictionary<string, bool> SideSolidOpaqueAo;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public string ClimateColorMap;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public string SeasonColorMap;

	[JsonProperty]
	[DocumentAsJson("Optional", "0", false)]
	public int Replaceable;

	[JsonProperty]
	[DocumentAsJson("Optional", "0", false)]
	public int Fertility;

	[JsonProperty]
	[DocumentAsJson("Optional", "-1", false)]
	public int BleedPriority = -1;

	[JsonProperty]
	[DocumentAsJson("Optional", "false", false)]
	public bool CanReceiveBleed;

	[JsonProperty]
	[DocumentAsJson("Optional", "null", false)]
	public string BleedOverlayTexture;

	[JsonProperty]
	[DocumentAsJson("Optional", "false", false)]
	public bool BleedTopFaceOnly;

	[JsonProperty]
	[DocumentAsJson("Optional", "null", false)]
	public string BleedGroup;

	[JsonProperty]
	[DocumentAsJson("Optional", "true", false)]
	public bool BlocksTextureBleed = true;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public VertexFlags VertexFlags;

	[JsonProperty]
	[DocumentAsJson("Optional", "false", false)]
	public bool Frostable;

	[JsonProperty]
	[DocumentAsJson("Optional", "99", false)]
	public ushort LightAbsorption = 99;

	[JsonProperty]
	[DocumentAsJson("Recommended", "6", false)]
	public float Resistance = 6f;

	[JsonProperty]
	[DocumentAsJson("Recommended", "Stone", false)]
	public EnumBlockMaterial BlockMaterial = EnumBlockMaterial.Stone;

	[JsonProperty]
	[DocumentAsJson("Recommended", "0", false)]
	public int RequiredMiningTier;

	[JsonProperty("CollisionBox")]
	[DocumentAsJson("Optional", "Default Collision Box", false)]
	private RotatableCube CollisionBoxR = DefaultCollisionBoxR.Clone();

	[JsonProperty("SelectionBox")]
	[DocumentAsJson("Optional", "Default Collision Box", false)]
	private RotatableCube SelectionBoxR = DefaultCollisionBoxR.Clone();

	[JsonProperty("CollisionSelectionBox")]
	[DocumentAsJson("Optional", "Default Collision Box", false)]
	private RotatableCube CollisionSelectionBoxR;

	[JsonProperty("ParticleCollisionBox")]
	[DocumentAsJson("Optional", "Default Collision Box", false)]
	private RotatableCube ParticleCollisionBoxR;

	[JsonProperty("CollisionBoxes")]
	[DocumentAsJson("Optional", "Default Collision Box", false)]
	private RotatableCube[] CollisionBoxesR;

	[JsonProperty("SelectionBoxes")]
	[DocumentAsJson("Optional", "Default Collision Box", false)]
	private RotatableCube[] SelectionBoxesR;

	[JsonProperty("CollisionSelectionBoxes")]
	[DocumentAsJson("Optional", "Default Collision Box", false)]
	private RotatableCube[] CollisionSelectionBoxesR;

	[JsonProperty("ParticleCollisionBoxes")]
	[DocumentAsJson("Optional", "Default Collision Box", false)]
	private RotatableCube[] ParticleCollisionBoxesR;

	public Cuboidf[] CollisionBoxes;

	public Cuboidf[] SelectionBoxes;

	public Cuboidf[] ParticleCollisionBoxes;

	[JsonProperty]
	[DocumentAsJson("Optional", "False", false)]
	public bool Climbable;

	[JsonProperty]
	[DocumentAsJson("Optional", "False", false)]
	public bool RainPermeable;

	[JsonProperty]
	[DocumentAsJson("Optional", "0", false)]
	public int LiquidLevel;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public string LiquidCode;

	[JsonProperty]
	[DocumentAsJson("Optional", "1", false)]
	public float WalkspeedMultiplier = 1f;

	[JsonProperty]
	[DocumentAsJson("Optional", "1", false)]
	public float DragMultiplier = 1f;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public BlockDropItemStack[] Drops;

	[JsonProperty]
	[DocumentAsJson("Optional", "None", false)]
	public BlockCropPropertiesType CropProps;

	[JsonProperty]
	[DocumentAsJson("Optional", "All ( [\"*\"] )", false)]
	public string[] AllowSpawnCreatureGroups = Block.DefaultAllowAllSpawns;

	[ThreadStatic]
	private static List<BlockBehavior> reusableBehaviorList;

	[ThreadStatic]
	private static List<CollectibleBehavior> reusableCollectibleBehaviorList;

	[ThreadStatic]
	private static List<string> reusableStringList;

	public BlockType()
	{
		Class = "Block";
		Shape = new CompositeShape
		{
			Base = new AssetLocation("game", "block/basic/cube")
		};
		GuiTransform = ModelTransform.BlockDefaultGui();
		FpHandTransform = ModelTransform.BlockDefaultFp();
		TpHandTransform = ModelTransform.BlockDefaultTp();
		GroundTransform = ModelTransform.BlockDefaultGround();
		MaxStackSize = 64;
	}

	internal override RegistryObjectType CreateAndPopulate(ICoreServerAPI api, AssetLocation fullcode, JObject jobject, JsonSerializer deserializer, Vintagestory.API.Datastructures.OrderedDictionary<string, string> variant)
	{
		return CreateResolvedType<BlockType>(api, fullcode, jobject, deserializer, variant);
	}

	public Block CreateBlock(ICoreServerAPI api)
	{
		Block block;
		if (api.ClassRegistry.GetBlockClass(Class) == null)
		{
			api.Server.Logger.Error("Block with code {0} has defined a block class {1}, no such class registered. Will ignore.", Code, Class);
			block = new Block();
		}
		else
		{
			block = api.ClassRegistry.CreateBlock(Class);
		}
		if (EntityClass != null)
		{
			if (api.ClassRegistry.GetBlockEntity(EntityClass) != null)
			{
				block.EntityClass = EntityClass;
			}
			else
			{
				api.Server.Logger.Error("Block with code {0} has defined a block entity class {1}, no such class registered. Will ignore.", Code, EntityClass);
			}
		}
		block.Code = Code;
		block.VariantStrict = Variant;
		block.Variant = new RelaxedReadOnlyDictionary<string, string>(Variant);
		block.Class = Class;
		block.LiquidSelectable = LiquidSelectable;
		block.LiquidCode = LiquidCode;
		block.BlockEntityBehaviors = ((BlockEntityBehaviorType[])EntityBehaviors?.Clone()) ?? Array.Empty<BlockEntityBehaviorType>();
		block.Tags = Tags;
		if (block.EntityClass == null && block.BlockEntityBehaviors != null && block.BlockEntityBehaviors.Length != 0)
		{
			block.EntityClass = "Generic";
		}
		block.WalkSpeedMultiplier = WalkspeedMultiplier;
		block.DragMultiplier = DragMultiplier;
		block.Durability = Durability;
		block.Dimensions = Size ?? CollectibleObject.DefaultSize;
		block.DamagedBy = (EnumItemDamageSource[])DamagedBy?.Clone();
		block.Tool = Tool;
		block.DrawType = DrawType;
		block.Replaceable = Replaceable;
		block.Fertility = Fertility;
		block.BleedPriority = BleedPriority;
		block.CanReceiveBleed = CanReceiveBleed;
		block.BleedOverlayTexture = BleedOverlayTexture;
		block.BleedTopFaceOnly = BleedTopFaceOnly;
		block.BleedGroup = BleedGroup;
		block.BlocksTextureBleed = BlocksTextureBleed;
		block.LightAbsorption = LightAbsorption;
		block.LightHsv = LightHsv;
		block.VertexFlags = VertexFlags?.Clone() ?? new VertexFlags(0);
		block.Frostable = Frostable;
		block.Resistance = Resistance;
		block.BlockMaterial = BlockMaterial;
		block.Shape = Shape;
		block.Lod0Shape = Lod0Shape;
		block.Lod2Shape = Lod2Shape;
		block.ShapeInventory = ShapeInventory;
		block.DoNotRenderAtLod2 = DoNotRenderAtLod2;
		block.TexturesInventory = ((TexturesInventory == null) ? null : new FastSmallDictionary<string, CompositeTexture>(TexturesInventory));
		block.Textures = ((Textures == null) ? null : new FastSmallDictionary<string, CompositeTexture>(Textures));
		block.ClimateColorMap = ClimateColorMap;
		block.SeasonColorMap = SeasonColorMap;
		block.Ambientocclusion = Ambientocclusion;
		block.CollisionBoxes = CollisionBoxes;
		block.SelectionBoxes = SelectionBoxes;
		block.ParticleCollisionBoxes = ParticleCollisionBoxes;
		block.MaterialDensity = MaterialDensity;
		block.GuiTransform = GuiTransform;
		block.FpHandTransform = FpHandTransform;
		block.TpHandTransform = TpHandTransform;
		block.TpOffHandTransform = TpOffHandTransform;
		block.GroundTransform = GroundTransform;
		block.RenderPass = RenderPass;
		block.ParticleProperties = ParticleProperties;
		block.ParticlesTextureCode = ParticlesTextureCode;
		block.Climbable = Climbable;
		block.RainPermeable = RainPermeable;
		block.FaceCullMode = FaceCullMode;
		block.Drops = Drops;
		block.MaxStackSize = MaxStackSize;
		block.MatterState = MatterState;
		if (Attributes != null)
		{
			block.Attributes = Attributes.Clone();
		}
		block.NutritionProps = NutritionProps;
		block.TransitionableProps = TransitionableProps;
		block.GrindingProps = GrindingProps;
		block.CrushingProps = CrushingProps;
		block.LiquidLevel = LiquidLevel;
		block.AttackPower = AttackPower;
		block.MiningSpeed = MiningSpeed;
		block.ToolTier = ToolTier;
		block.RequiredMiningTier = RequiredMiningTier;
		block.HeldSounds = HeldSounds?.Clone();
		block.AttackRange = AttackRange;
		if (Sounds != null)
		{
			block.Sounds = Sounds.Clone();
		}
		block.RandomDrawOffset = (RandomDrawOffset ? 1 : 0);
		block.RandomizeRotations = RandomizeRotations;
		block.RandomizeAxes = RandomizeAxes;
		block.RandomSizeAdjust = RandomSizeAdjust;
		block.CombustibleProps = CombustibleProps;
		block.StorageFlags = (EnumItemStorageFlags)StorageFlags;
		block.RenderAlphaTest = RenderAlphaTest;
		block.HeldTpHitAnimation = HeldTpHitAnimation;
		block.HeldRightTpIdleAnimation = HeldRightTpIdleAnimation;
		block.HeldLeftTpIdleAnimation = HeldLeftTpIdleAnimation;
		block.HeldLeftReadyAnimation = HeldLeftReadyAnimation;
		block.HeldRightReadyAnimation = HeldRightReadyAnimation;
		block.HeldTpUseAnimation = HeldTpUseAnimation;
		block.CreativeInventoryStacks = ((CreativeInventoryStacks == null) ? null : ((CreativeTabAndStackList[])CreativeInventoryStacks.Clone()));
		block.AllowSpawnCreatureGroups = AllowSpawnCreatureGroups;
		block.AllCreaturesAllowed = AllowSpawnCreatureGroups.Length == 1 && AllowSpawnCreatureGroups[0] == "*";
		EnsureClientServerAccuracy(block.CollisionBoxes);
		EnsureClientServerAccuracy(block.SelectionBoxes);
		EnsureClientServerAccuracy(block.ParticleCollisionBoxes);
		InitBlock(api.ClassRegistry, api.World.Logger, block, Variant);
		return block;
	}

	private void EnsureClientServerAccuracy(Cuboidf[] boxes)
	{
		if (boxes != null)
		{
			for (int i = 0; i < boxes.Length; i++)
			{
				boxes[i].RoundToFracsOfOne10thousand();
			}
		}
	}

	private Cuboidf[] ToCuboidf(params RotatableCube[] cubes)
	{
		Cuboidf[] outcubes = new Cuboidf[cubes.Length];
		for (int i = 0; i < cubes.Length; i++)
		{
			outcubes[i] = cubes[i].RotatedCopy();
		}
		return outcubes;
	}

	internal override void OnDeserialized()
	{
		base.OnDeserialized();
		if (CollisionBoxR != null)
		{
			CollisionBoxes = ToCuboidf(CollisionBoxR);
		}
		if (SelectionBoxR != null)
		{
			SelectionBoxes = ToCuboidf(SelectionBoxR);
		}
		if (ParticleCollisionBoxR != null)
		{
			ParticleCollisionBoxes = ToCuboidf(ParticleCollisionBoxR);
		}
		if (CollisionBoxesR != null)
		{
			CollisionBoxes = ToCuboidf(CollisionBoxesR);
		}
		if (SelectionBoxesR != null)
		{
			SelectionBoxes = ToCuboidf(SelectionBoxesR);
		}
		if (ParticleCollisionBoxesR != null)
		{
			ParticleCollisionBoxes = ToCuboidf(ParticleCollisionBoxesR);
		}
		if (CollisionSelectionBoxR != null)
		{
			CollisionBoxes = ToCuboidf(CollisionSelectionBoxR);
			SelectionBoxes = CloneArray(CollisionBoxes);
		}
		if (CollisionSelectionBoxesR != null)
		{
			CollisionBoxes = ToCuboidf(CollisionSelectionBoxesR);
			SelectionBoxes = CloneArray(CollisionBoxes);
		}
		ResolveStringBoolDictFaces(SideSolidOpaqueAo);
		ResolveStringBoolDictFaces(SideSolid);
		ResolveStringBoolDictFaces(SideOpaque);
		ResolveStringBoolDictFaces(SideAo);
		ResolveStringBoolDictFaces(EmitSideAo);
		if (SideSolidOpaqueAo != null && SideSolidOpaqueAo.Count > 0)
		{
			ResolveDict(SideSolidOpaqueAo, ref SideSolid);
			ResolveDict(SideSolidOpaqueAo, ref SideOpaque);
			ResolveDict(SideSolidOpaqueAo, ref EmitSideAo);
		}
		if (EmitSideAo == null)
		{
			EmitSideAo = new Dictionary<string, bool> { 
			{
				"all",
				LightAbsorption > 0
			} };
			ResolveStringBoolDictFaces(EmitSideAo);
		}
		if (LightHsv == null)
		{
			LightHsv = new byte[3];
		}
		LightHsv[0] = (byte)GameMath.Clamp(LightHsv[0], 0, ColorUtil.HueQuantities - 1);
		LightHsv[1] = (byte)GameMath.Clamp(LightHsv[1], 0, ColorUtil.SatQuantities - 1);
		LightHsv[2] = (byte)GameMath.Clamp(LightHsv[2], 0, ColorUtil.BrightQuantities - 1);
	}

	private Cuboidf[] CloneArray(Cuboidf[] array)
	{
		if (array == null)
		{
			return null;
		}
		int l = array.Length;
		Cuboidf[] result = new Cuboidf[l];
		for (int i = 0; i < l; i++)
		{
			result[i] = array[i].Clone();
		}
		return result;
	}

	private void ResolveDict(Dictionary<string, bool> sideSolidOpaqueAo, ref Dictionary<string, bool> targetDict)
	{
		bool wasNull = targetDict == null;
		if (wasNull)
		{
			targetDict = new Dictionary<string, bool> { { "all", true } };
		}
		foreach (KeyValuePair<string, bool> val in sideSolidOpaqueAo)
		{
			if (wasNull || !targetDict.ContainsKey(val.Key))
			{
				targetDict[val.Key] = val.Value;
			}
		}
		ResolveStringBoolDictFaces(targetDict);
	}

	public void InitBlock(IClassRegistryAPI instancer, ILogger logger, Block block, Vintagestory.API.Datastructures.OrderedDictionary<string, string> searchReplace)
	{
		CollectibleBehaviorType[] behaviorTypes = Behaviors;
		if (behaviorTypes != null)
		{
			List<BlockBehavior> blockbehaviors = reusableBehaviorList ?? (reusableBehaviorList = new List<BlockBehavior>());
			List<CollectibleBehavior> collbehaviors = reusableCollectibleBehaviorList ?? (reusableCollectibleBehaviorList = new List<CollectibleBehavior>());
			blockbehaviors.Clear();
			collbehaviors.Clear();
			foreach (CollectibleBehaviorType behaviorType in behaviorTypes)
			{
				CollectibleBehavior behavior = null;
				if (instancer.GetCollectibleBehaviorClass(behaviorType.name) != null)
				{
					behavior = instancer.CreateCollectibleBehavior(block, behaviorType.name);
				}
				if (instancer.GetBlockBehaviorClass(behaviorType.name) != null)
				{
					behavior = instancer.CreateBlockBehavior(block, behaviorType.name);
				}
				if (behavior == null)
				{
					logger.Warning(Lang.Get("Block or Collectible behavior {0} for block {1} not found", behaviorType.name, block.Code));
					continue;
				}
				if (behaviorType.properties == null)
				{
					behaviorType.properties = new JsonObject(new JObject());
				}
				try
				{
					behavior.Initialize(behaviorType.properties);
				}
				catch (Exception e)
				{
					logger.Error("Failed calling Initialize() on collectible or block behavior {0} for block {1}, using properties {2}. Will continue anyway. Exception", behaviorType.name, block.Code, behaviorType.properties.ToString());
					logger.Error(e);
				}
				collbehaviors.Add(behavior);
				if (behavior is BlockBehavior bbh)
				{
					blockbehaviors.Add(bbh);
				}
			}
			block.BlockBehaviors = blockbehaviors.ToArray();
			block.CollectibleBehaviors = collbehaviors.ToArray();
			blockbehaviors.Clear();
			collbehaviors.Clear();
		}
		if (CropProps != null)
		{
			block.CropProps = new BlockCropProperties();
			block.CropProps.GrowthStages = CropProps.GrowthStages;
			block.CropProps.HarvestGrowthStageLoss = CropProps.HarvestGrowthStageLoss;
			block.CropProps.MultipleHarvests = CropProps.MultipleHarvests;
			block.CropProps.NutrientConsumption = CropProps.NutrientConsumption;
			block.CropProps.RequiredNutrient = CropProps.RequiredNutrient;
			block.CropProps.TotalGrowthDays = CropProps.TotalGrowthDays;
			block.CropProps.TotalGrowthMonths = CropProps.TotalGrowthMonths;
			block.CropProps.ColdDamageBelow = CropProps.ColdDamageBelow;
			block.CropProps.HeatDamageAbove = CropProps.HeatDamageAbove;
			block.CropProps.DamageGrowthStuntMul = CropProps.DamageGrowthStuntMul;
			block.CropProps.ColdDamageRipeMul = CropProps.ColdDamageRipeMul;
			if (CropProps.Behaviors != null)
			{
				block.CropProps.Behaviors = new CropBehavior[CropProps.Behaviors.Length];
				for (int j = 0; j < CropProps.Behaviors.Length; j++)
				{
					CropBehaviorType behaviorType2 = CropProps.Behaviors[j];
					CropBehavior behavior2 = instancer.CreateCropBehavior(block, behaviorType2.name);
					if (behaviorType2.properties != null)
					{
						behavior2.Initialize(behaviorType2.properties);
					}
					block.CropProps.Behaviors[j] = behavior2;
				}
			}
		}
		if (block.Drops == null)
		{
			block.Drops = new BlockDropItemStack[1]
			{
				new BlockDropItemStack
				{
					Code = block.Code,
					Type = EnumItemClass.Block,
					Quantity = NatFloat.One
				}
			};
		}
		block.CreativeInventoryTabs = GetCreativeTabs(block.Code, CreativeInventory, searchReplace);
		if (SideOpaque != null && SideOpaque.Count > 0)
		{
			block.SideOpaque = new SmallBoolArray(63);
		}
		if (SideAo != null && SideAo.Count > 0)
		{
			block.SideAo = new SmallBoolArray(63);
		}
		BlockFacing[] aLLFACES = BlockFacing.ALLFACES;
		foreach (BlockFacing facing in aLLFACES)
		{
			if (SideAo != null && SideAo.TryGetValue(facing.Code, out var sideAoValue))
			{
				block.SideAo[facing.Index] = sideAoValue;
			}
			if (EmitSideAo != null && EmitSideAo.TryGetValue(facing.Code, out var emitSideAoValue) && emitSideAoValue)
			{
				block.EmitSideAo |= facing.Flag;
			}
			if (SideSolid != null && SideSolid.TryGetValue(facing.Code, out var sideSolidValue))
			{
				block.SideSolid[facing.Index] = sideSolidValue;
			}
			if (SideOpaque != null && SideOpaque.TryGetValue(facing.Code, out var sideOpaqueValue))
			{
				block.SideOpaque[facing.Index] = sideOpaqueValue;
			}
		}
		if (HeldRightReadyAnimation != null && HeldRightReadyAnimation == HeldRightTpIdleAnimation)
		{
			logger.Error("Block {0} HeldRightReadyAnimation and HeldRightTpIdleAnimation is set to the same animation {1}. This invalid and breaks stuff. Will set HeldRightReadyAnimation to null", Code, HeldRightTpIdleAnimation);
			HeldRightReadyAnimation = null;
		}
	}

	public static string[] GetCreativeTabs(AssetLocation code, Dictionary<string, string[]> CreativeInventory, Vintagestory.API.Datastructures.OrderedDictionary<string, string> searchReplace)
	{
		List<string> tabs = reusableStringList ?? (reusableStringList = new List<string>());
		tabs.Clear();
		foreach (KeyValuePair<string, string[]> val in CreativeInventory)
		{
			for (int i = 0; i < val.Value.Length; i++)
			{
				if (WildcardUtil.Match(RegistryObject.FillPlaceHolder(val.Value[i], searchReplace), code.Path))
				{
					string tabCode = val.Key;
					tabs.Add(string.Intern(tabCode));
				}
			}
		}
		string[] result = tabs.ToArray();
		tabs.Clear();
		return result;
	}

	private void ResolveStringBoolDictFaces(Dictionary<string, bool> stringBoolDict)
	{
		if (stringBoolDict == null)
		{
			return;
		}
		BlockFacing[] hORIZONTALS;
		if (stringBoolDict.ContainsKey("horizontals"))
		{
			hORIZONTALS = BlockFacing.HORIZONTALS;
			foreach (BlockFacing facing in hORIZONTALS)
			{
				if (!stringBoolDict.ContainsKey(facing.Code))
				{
					stringBoolDict[facing.Code] = stringBoolDict["horizontals"];
				}
			}
		}
		if (stringBoolDict.ContainsKey("verticals"))
		{
			hORIZONTALS = BlockFacing.VERTICALS;
			foreach (BlockFacing facing2 in hORIZONTALS)
			{
				if (!stringBoolDict.ContainsKey(facing2.Code))
				{
					stringBoolDict[facing2.Code] = stringBoolDict["verticals"];
				}
			}
		}
		if (!stringBoolDict.ContainsKey("all"))
		{
			return;
		}
		hORIZONTALS = BlockFacing.ALLFACES;
		foreach (BlockFacing facing3 in hORIZONTALS)
		{
			if (!stringBoolDict.ContainsKey(facing3.Code))
			{
				stringBoolDict[facing3.Code] = stringBoolDict["all"];
			}
		}
	}
}
