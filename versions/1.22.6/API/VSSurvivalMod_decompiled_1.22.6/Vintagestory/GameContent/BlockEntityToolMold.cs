using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockEntityToolMold : BlockEntity, ILiquidMetalSink, ITemperatureSensitive, ITexPositionSource, IRotatable
{
	protected ToolMoldRenderer renderer;

	public MeshData MoldMesh;

	protected Cuboidf[] fillQuadsByLevel;

	protected int requiredUnits = 100;

	protected float fillHeight = 1f;

	protected bool breaksWhenFilled;

	public ItemStack MetalContent;

	public int FillLevel;

	public bool FillSide;

	public bool Shattered;

	private ICoreClientAPI capi;

	public float MeshAngle;

	private bool hasMeshAngle = true;

	private ITexPositionSource tmpTextureSource;

	private AssetLocation metalTexLoc;

	private MeshData shatteredMesh;

	public float Temperature => MetalContent?.Collectible.GetTemperature(Api.World, MetalContent) ?? 0f;

	public bool IsHardened => Temperature < 0.3f * MetalContent?.Collectible.GetMeltingPoint(Api.World, null, new DummySlot(MetalContent));

	public bool IsLiquid => Temperature > 0.8f * MetalContent?.Collectible.GetMeltingPoint(Api.World, null, new DummySlot(MetalContent));

	public bool IsFull => FillLevel >= requiredUnits;

	public bool CanReceiveAny
	{
		get
		{
			if (!Shattered)
			{
				if (!(base.Block.Variant["materialtype"] == "fired"))
				{
					return base.Block.Code.Path.Contains("burned");
				}
				return true;
			}
			return false;
		}
	}

	public bool IsHot => Temperature >= 200f;

	public bool BreaksWhenFilled => breaksWhenFilled;

	public Size2i AtlasSize => capi.BlockTextureAtlas.Size;

	public TextureAtlasPosition this[string textureCode]
	{
		get
		{
			if (textureCode == "metal")
			{
				return capi.BlockTextureAtlas[metalTexLoc];
			}
			return tmpTextureSource[textureCode];
		}
	}

	public bool BeingChiseled { get; set; }

	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
		if (api is ICoreServerAPI sapi)
		{
			OnLoadWithoutMeshAngle(sapi);
		}
		if (base.Block == null || base.Block.Code == null || base.Block.Attributes == null)
		{
			return;
		}
		fillHeight = base.Block.Attributes["fillHeight"].AsFloat(1f);
		requiredUnits = base.Block.Attributes["requiredUnits"].AsInt(100);
		breaksWhenFilled = base.Block.Attributes["breaksWhenFilled"].AsBool();
		if (base.Block.Attributes["fillQuadsByLevel"].Exists)
		{
			fillQuadsByLevel = base.Block.Attributes["fillQuadsByLevel"].AsObject<Cuboidf[]>();
		}
		if (fillQuadsByLevel == null)
		{
			fillQuadsByLevel = new Cuboidf[1]
			{
				new Cuboidf(2f, 0f, 2f, 14f, 0f, 14f)
			};
		}
		capi = api as ICoreClientAPI;
		if (capi != null && !Shattered)
		{
			capi.Event.RegisterRenderer(renderer = new ToolMoldRenderer(this, capi, fillQuadsByLevel), EnumRenderStage.Opaque, "toolmoldrenderer");
			UpdateRenderer();
			if (MoldMesh == null)
			{
				GenMeshes();
			}
		}
		if (!Shattered)
		{
			RegisterGameTickListener(OnGameTick, 50);
		}
	}

	protected virtual void OnLoadWithoutMeshAngle(ICoreServerAPI sapi)
	{
		if (hasMeshAngle || (base.Block.Code.Domain != "game" && base.Block.FirstCodePart() != "toolmold"))
		{
			return;
		}
		string code = base.Block.Code.SecondCodePart();
		AssetLocation loc = null;
		if (!(code == "burned"))
		{
			loc = ((!(code != "blue")) ? base.Block.Code : new AssetLocation("toolmold-blue-" + base.Block.CodeEndWithoutParts(2)));
		}
		else
		{
			code = base.Block.LastCodePart();
			loc = new AssetLocation(base.Block.Code.ToShortString());
			switch (code)
			{
			case "east":
			case "west":
			case "south":
				loc.WithoutPathAppendix("-" + code);
				break;
			}
			loc.WithPathAppendixOnce("-north");
		}
		int blockID = sapi.World.BlockAccessor.GetBlock(loc)?.BlockId ?? 0;
		if (blockID != 0)
		{
			sapi.World.BlockAccessor.ExchangeBlock(blockID, Pos);
			switch (code)
			{
			case "gray":
			case "east":
				MeshAngle = -(float)Math.PI / 2f;
				break;
			case "blue":
			case "north":
				MeshAngle = 0f;
				break;
			case "west":
			case "brown":
				MeshAngle = (float)Math.PI / 2f;
				break;
			case "south":
			case "tan":
				MeshAngle = (float)Math.PI;
				break;
			}
		}
		hasMeshAngle = true;
	}

	private void OnGameTick(float dt)
	{
		if (renderer != null)
		{
			renderer.Level = (float)FillLevel * fillHeight / (float)requiredUnits;
		}
		if (MetalContent != null && renderer != null)
		{
			renderer.stack = MetalContent;
			renderer.Temperature = Math.Min(1300f, MetalContent.Collectible.GetTemperature(Api.World, MetalContent));
		}
	}

	public bool CanReceive(ItemStack metal)
	{
		if (MetalContent == null || (MetalContent.Collectible.Equals(MetalContent, metal, GlobalConstants.IgnoredStackAttributes) && !IsFull))
		{
			ItemStack[] moldedStacks = GetMoldedStacks(metal);
			if (moldedStacks != null && moldedStacks.Length != 0)
			{
				return !Shattered;
			}
		}
		return false;
	}

	public void BeginFill(Vec3d hitPosition)
	{
		FillSide = hitPosition.X >= 0.5;
	}

	public bool OnPlayerInteract(IPlayer byPlayer, BlockFacing onFace, Vec3d hitPosition)
	{
		if (Shattered)
		{
			return false;
		}
		if (!byPlayer.Entity.Controls.ShiftKey)
		{
			if (byPlayer.Entity.Controls.HandUse != EnumHandInteract.None)
			{
				return false;
			}
			bool handled = TryTakeContents(byPlayer);
			if (!handled && FillLevel == 0)
			{
				ItemStack activeStack = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack;
				if (activeStack != null)
				{
					CollectibleObject collectible = activeStack.Collectible;
					if (!(collectible is BlockToolMold) && !(collectible is BlockIngotMold))
					{
						return handled;
					}
				}
				ItemStack itemStack = new ItemStack(base.Block);
				if (!byPlayer.InventoryManager.TryGiveItemstack(itemStack))
				{
					Api.World.SpawnItemEntity(itemStack, Pos.ToVec3d().Add(0.5, 0.2, 0.5));
				}
				Api.World.Logger.Audit("{0} Took 1x{1} from Tool mold at {2}.", byPlayer.PlayerName, itemStack.Collectible.Code, Pos);
				Api.World.BlockAccessor.SetBlock(0, Pos);
				if (base.Block.Sounds?.Place != null)
				{
					Api.World.PlaySoundAt(base.Block.Sounds.Place, Pos, -0.5, byPlayer);
				}
				handled = true;
			}
			return handled;
		}
		return false;
	}

	protected virtual bool TryTakeContents(IPlayer byPlayer)
	{
		if (Shattered || MetalContent == null || FillLevel == 0)
		{
			return false;
		}
		if (BreaksWhenFilled)
		{
			(Api as ICoreClientAPI)?.TriggerIngameError(this, "breakswhenfilledrightclicked", Lang.Get("toolmold-breakswhenfilled-error"));
			return false;
		}
		if (Api is ICoreServerAPI)
		{
			MarkDirty();
		}
		if (IsFull && IsHardened)
		{
			Api.World.PlaySoundAt(new AssetLocation("sounds/block/ingot"), Pos, -0.5, byPlayer, randomizePitch: false);
			if (Api is ICoreServerAPI)
			{
				ItemStack[] outstacks = GetStateAwareMoldedStacks();
				if (outstacks != null)
				{
					ItemStack[] array = outstacks;
					foreach (ItemStack outstack in array)
					{
						int quantity = outstack.StackSize;
						if (!byPlayer.InventoryManager.TryGiveItemstack(outstack))
						{
							Api.World.SpawnItemEntity(outstack, Pos.ToVec3d().Add(0.5, 0.2, 0.5));
						}
						Api.World.Logger.Audit("{0} Took {1}x{2} from Tool mold at {3}.", byPlayer.PlayerName, quantity, outstack.Collectible.Code, Pos);
					}
					MetalContent = null;
					FillLevel = 0;
				}
			}
			UpdateRenderer();
			return true;
		}
		return false;
	}

	public void UpdateRenderer()
	{
		if (renderer == null)
		{
			return;
		}
		if (Shattered && renderer != null)
		{
			(Api as ICoreClientAPI).Event.UnregisterRenderer(renderer, EnumRenderStage.Opaque);
			renderer = null;
			return;
		}
		renderer.Level = (float)FillLevel * fillHeight / (float)requiredUnits;
		if (MetalContent?.Collectible != null)
		{
			renderer.TextureName = new AssetLocation("block/metal/ingot/" + MetalContent.Collectible.LastCodePart() + ".png");
		}
		else
		{
			renderer.TextureName = null;
		}
	}

	public void ReceiveLiquidMetal(ItemStack metal, ref int amount, float temperature)
	{
		if (!IsFull && (MetalContent == null || metal.Collectible.Equals(MetalContent, metal, GlobalConstants.IgnoredStackAttributes)))
		{
			if (MetalContent == null)
			{
				MetalContent = metal.Clone();
				MetalContent.ResolveBlockOrItem(Api.World);
				MetalContent.Collectible.SetTemperature(Api.World, MetalContent, temperature, delayCooldown: false);
				MetalContent.StackSize = 1;
				(MetalContent.Attributes["temperature"] as ITreeAttribute)?.SetFloat("cooldownSpeed", 300f);
			}
			else
			{
				MetalContent.Collectible.SetTemperature(Api.World, MetalContent, temperature, delayCooldown: false);
			}
			int amountToFill = Math.Min(amount, requiredUnits - FillLevel);
			FillLevel += amountToFill;
			amount -= amountToFill;
			UpdateRenderer();
		}
	}

	public void OnPourOver()
	{
		MarkDirty(redrawOnClient: true);
	}

	public override void OnBlockRemoved()
	{
		base.OnBlockRemoved();
		renderer?.Dispose();
		renderer = null;
	}

	public override void OnBlockUnloaded()
	{
		base.OnBlockUnloaded();
		renderer?.Dispose();
		renderer = null;
	}

	public ItemStack[] GetStateAwareMold()
	{
		List<ItemStack> mold = new List<ItemStack>();
		if (!Shattered)
		{
			if (!BreaksWhenFilled || FillLevel <= 0)
			{
				mold.Add(new ItemStack(base.Block));
			}
		}
		else
		{
			BlockDropItemStack[] shatteredDrops = base.Block.Attributes?["shatteredDrops"].AsObject<BlockDropItemStack[]>();
			if (shatteredDrops != null)
			{
				for (int i = 0; i < shatteredDrops.Length; i++)
				{
					shatteredDrops[i].Resolve(Api.World, "shatteredDrops[" + i + "] for", base.Block.Code);
					ItemStack stack = shatteredDrops[i].GetNextItemStack();
					if (stack != null)
					{
						mold.Add(stack);
						if (shatteredDrops[i].LastDrop)
						{
							break;
						}
					}
				}
			}
		}
		return mold.ToArray();
	}

	public ItemStack[] GetStateAwareMoldedStacks()
	{
		if (MetalContent?.Collectible != null && IsHardened)
		{
			if (Shattered)
			{
				JsonItemStack shatteredStack = MetalContent.Collectible.Attributes?["shatteredStack"].AsObject<JsonItemStack>();
				if (shatteredStack != null)
				{
					shatteredStack.Resolve(Api.World, "shatteredStack for" + MetalContent.Collectible.Code);
					ItemStack stack = shatteredStack.ResolvedItemstack;
					if (stack != null)
					{
						stack.StackSize = (int)((float)FillLevel / 5f);
						return new ItemStack[1] { stack };
					}
				}
			}
			if (IsFull)
			{
				return GetMoldedStacks(MetalContent);
			}
		}
		return null;
	}

	public ItemStack GetChiseledStack()
	{
		if (MetalContent != null && FillLevel > 0 && !Shattered && IsHardened)
		{
			JsonItemStack? obj = MetalContent.Collectible.Attributes?["shatteredStack"].AsObject<JsonItemStack>();
			obj?.Resolve(Api.World, "chiseledStack for" + MetalContent.Collectible.Code);
			ItemStack stack = obj?.ResolvedItemstack;
			if (stack != null)
			{
				stack.StackSize = (int)((float)FillLevel / 5f);
				return stack;
			}
		}
		return null;
	}

	public ItemStack[] GetMoldedStacks(ItemStack fromMetal)
	{
		try
		{
			if (base.Block.Attributes["drop"].Exists)
			{
				JsonItemStack jstack = base.Block.Attributes["drop"].AsObject<JsonItemStack>(null, base.Block.Code.Domain);
				if (jstack == null)
				{
					return null;
				}
				ItemStack stack = stackFromCode(jstack, fromMetal);
				if (stack == null)
				{
					return Array.Empty<ItemStack>();
				}
				if (MetalContent != null)
				{
					stack.Collectible.SetTemperature(Api.World, stack, MetalContent.Collectible.GetTemperature(Api.World, MetalContent));
				}
				return new ItemStack[1] { stack };
			}
			JsonItemStack[]? array = base.Block.Attributes["drops"].AsObject<JsonItemStack[]>(null, base.Block.Code.Domain);
			List<ItemStack> stacks = new List<ItemStack>();
			JsonItemStack[] array2 = array;
			foreach (JsonItemStack jstack2 in array2)
			{
				ItemStack stack2 = stackFromCode(jstack2, fromMetal);
				if (MetalContent != null)
				{
					stack2.Collectible.SetTemperature(Api.World, stack2, MetalContent.Collectible.GetTemperature(Api.World, MetalContent));
				}
				if (stack2 != null)
				{
					stacks.Add(stack2);
				}
			}
			return stacks.ToArray();
		}
		catch (JsonReaderException)
		{
			Api.World.Logger.Error("Failed getting molded stacks from tool mold of block {0}, probably unable to parse drop or drops attribute", base.Block.Code);
			throw;
		}
	}

	public ItemStack stackFromCode(JsonItemStack jstack, ItemStack fromMetal)
	{
		string metaltype = fromMetal.Collectible.LastCodePart();
		jstack.Code.Path = jstack.Code.Path.Replace("{metal}", metaltype);
		jstack.Resolve(Api.World, "tool mold drop for " + base.Block.Code);
		return jstack.ResolvedItemstack;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolve)
	{
		base.FromTreeAttributes(tree, worldForResolve);
		MetalContent = tree.GetItemstack("contents");
		FillLevel = tree.GetInt("fillLevel");
		Shattered = tree.GetBool("shattered");
		if (worldForResolve != null && MetalContent != null)
		{
			MetalContent.ResolveBlockOrItem(worldForResolve);
		}
		hasMeshAngle = tree.HasAttribute("meshAngle");
		MeshAngle = tree.GetFloat("meshAngle");
		UpdateRenderer();
		ICoreAPI api = Api;
		if (api != null && api.Side == EnumAppSide.Client)
		{
			Api.World.BlockAccessor.MarkBlockDirty(Pos);
		}
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetItemstack("contents", MetalContent);
		tree.SetInt("fillLevel", FillLevel);
		tree.SetBool("shattered", Shattered);
		tree.SetFloat("meshAngle", MeshAngle);
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		if (!Shattered)
		{
			string state = (IsLiquid ? Lang.Get("liquid") : (IsHardened ? Lang.Get("hardened") : Lang.Get("soft")));
			string matkey = "material-" + MetalContent?.Collectible.Variant["metal"];
			string mat = (Lang.HasTranslation(matkey) ? Lang.Get(matkey) : MetalContent?.GetName());
			string temp = ((Temperature < 21f) ? Lang.Get("Cold") : Lang.Get("{0}°C", (int)Temperature));
			string contents = Lang.GetWithFallback("metalmold-blockinfo-unitsofmetal", "{0}/{4} units of {1} {2} ({3})", FillLevel, state, mat, temp, requiredUnits);
			dsc.AppendLine(((MetalContent != null) ? contents : Lang.GetWithFallback("metalmold-blockinfo-emptymold", "0/{0} units of metal", requiredUnits)) + "\n");
		}
		else
		{
			ItemStack[] stateAwareMoldedStacks = GetStateAwareMoldedStacks();
			ItemStack shatteredStack = ((stateAwareMoldedStacks != null) ? stateAwareMoldedStacks[0] : null);
			if (shatteredStack != null)
			{
				dsc.AppendLine(Lang.Get("metalmold-blockinfo-shatteredmetal", shatteredStack.StackSize, shatteredStack.GetName().ToLower()) + "\n");
			}
		}
	}

	public override void OnStoreCollectibleMappings(Dictionary<int, AssetLocation> blockIdMapping, Dictionary<int, AssetLocation> itemIdMapping)
	{
		MetalContent?.Collectible.OnStoreCollectibleMappings(Api.World, new DummySlot(MetalContent), blockIdMapping, itemIdMapping);
	}

	public override void OnLoadCollectibleMappings(IWorldAccessor worldForResolve, Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, int schematicSeed, bool resolveImports)
	{
		ItemStack metalContent = MetalContent;
		if (metalContent != null)
		{
			metalContent.FixMapping(oldBlockIdMapping, oldItemIdMapping, worldForResolve);
			if (0 == 0)
			{
				goto IL_0020;
			}
		}
		MetalContent = null;
		goto IL_0020;
		IL_0020:
		ITreeAttribute obj = MetalContent?.Attributes["temperature"] as ITreeAttribute;
		if (obj != null && obj.HasAttribute("temperatureLastUpdate"))
		{
			((ITreeAttribute)MetalContent.Attributes["temperature"]).SetDouble("temperatureLastUpdate", worldForResolve.Calendar.TotalHours);
		}
	}

	public void ShatterMold()
	{
		Api.World.PlaySoundAt(new AssetLocation("sounds/block/ceramicbreak"), Pos, -0.4);
		Shattered = true;
		base.Block.SpawnBlockBrokenParticles(Pos);
		base.Block.SpawnBlockBrokenParticles(Pos);
		MarkDirty(redrawOnClient: true);
	}

	public void CoolNow(float amountRel, OnStackToCool onStackToCoolCallback)
	{
		float breakchance = Math.Max(0f, amountRel - 0.6f) * Math.Max(Temperature - 250f, 0f) / 5000f;
		if (Api.World.Rand.NextDouble() < (double)breakchance)
		{
			ShatterMold();
			MetalContent.Collectible.SetTemperature(Api.World, MetalContent, 20f, delayCooldown: false);
			FillLevel = (int)((double)FillLevel * (0.699999988079071 + Api.World.Rand.NextDouble() * 0.10000000149011612));
		}
		else if (MetalContent != null)
		{
			float temp = Temperature;
			if (temp > 120f)
			{
				Api.World.PlaySoundAt(new AssetLocation("sounds/effect/extinguish"), Pos, -0.5, null, randomizePitch: false, 16f);
			}
			MetalContent.Collectible.SetTemperature(Api.World, MetalContent, Math.Max(20f, temp - amountRel * 20f), delayCooldown: false);
			MarkDirty(redrawOnClient: true);
		}
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		if (Shattered)
		{
			EnsureShatteredMeshesLoaded();
		}
		float[] tfMat = Mat4f.Create();
		Mat4f.Translate(tfMat, tfMat, 0.5f, 0f, 0.5f);
		Mat4f.RotateY(tfMat, tfMat, MeshAngle);
		Mat4f.Translate(tfMat, tfMat, -0.5f, -0f, -0.5f);
		mesher.AddMeshData(Shattered ? shatteredMesh : MoldMesh, tfMat);
		return true;
	}

	private void EnsureShatteredMeshesLoaded()
	{
		if (Shattered && shatteredMesh == null)
		{
			metalTexLoc = ((MetalContent == null) ? new AssetLocation("block/transparent") : new AssetLocation("block/metal/ingot/" + MetalContent.Collectible.LastCodePart()));
			capi.Tesselator.TesselateShape("shatteredmold", getShatteredShape(base.Block), out shatteredMesh, this, null, 0, 0, 0);
		}
	}

	private Shape getShatteredShape(Block block)
	{
		tmpTextureSource = capi.Tesselator.GetTextureSource(block);
		CompositeShape cshape = block.Attributes["shatteredShape"].AsObject<CompositeShape>();
		cshape.Base.WithPathAppendixOnce(".json").WithPathPrefixOnce("shapes/");
		return Shape.TryGet(Api, cshape.Base);
	}

	private void GenMeshes()
	{
		MoldMesh = ObjectCacheUtil.GetOrCreate(Api, base.Block.Code.ToString(), delegate
		{
			CompositeShape shape = base.Block.Shape;
			ITexPositionSource textureSource = ((ICoreClientAPI)Api).Tesselator.GetTextureSource(base.Block);
			((ICoreClientAPI)Api).Tesselator.TesselateShape(shapeBase: Shape.TryGet(Api, base.Block.Shape.Base.Clone().WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json")), typeForLogging: base.Block.Code.ToString(), modeldata: out var modeldata, texSource: textureSource, meshRotationDeg: new Vec3f(shape.rotateX, shape.rotateY, shape.rotateZ), generalGlowLevel: 0, climateColorMapId: 0, seasonColorMapId: 0);
			return modeldata;
		});
	}

	public MeshData GetCurrentDecalMesh(ITexPositionSource texPositionSource)
	{
		if (tmpTextureSource == null)
		{
			tmpTextureSource = capi.Tesselator.GetTextureSource(base.Block);
		}
		if (BeingChiseled)
		{
			int voxelY = (int)GameMath.Clamp(renderer.Level, 0f, fillQuadsByLevel.Length - 1);
			MeshData modeldata = QuadMeshUtil.GetQuad();
			modeldata.Rgba = new byte[16];
			modeldata.Rgba.Fill(byte.MaxValue);
			modeldata.Flags = new int[16];
			Cuboidf size = fillQuadsByLevel[voxelY];
			modeldata.Uv = new float[8]
			{
				size.X2 / 64f,
				size.Z2 / 64f,
				size.X1 / 64f,
				size.Z2 / 64f,
				size.X1 / 64f,
				size.Z1 / 64f,
				size.X2 / 64f,
				size.Z1 / 64f
			};
			float[] matrix = new Matrixf().Identity().Translate(0.5f, 0.0625f + Math.Max(0f, renderer.Level / 16f - 1f / 48f), 0.5f).RotateX((float)Math.PI / 2f)
				.Scale(0.5f * size.Width / 16f, 0.5f * size.Length / 16f, 0.5f)
				.Values;
			modeldata.MatrixTransform(matrix);
			return modeldata.Clone();
		}
		MeshData mesh;
		if (Shattered)
		{
			CompositeShape cshape = base.Block.Attributes["shatteredShape"].AsObject<CompositeShape>();
			cshape.Base.WithPathAppendixOnce(".json").WithPathPrefixOnce("shapes/");
			capi.Tesselator.TesselateShape("shatteredmold", Shape.TryGet(Api, cshape.Base), out mesh, texPositionSource, null, 0, 0, 0);
		}
		else
		{
			CompositeShape cShape = base.Block.Shape;
			Shape shape = Shape.TryGet(Api, base.Block.Shape.Base.Clone().WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json"));
			((ICoreClientAPI)Api).Tesselator.TesselateShape(base.Block.Code.ToString(), shape, out mesh, texPositionSource, new Vec3f(cShape.rotateX, cShape.rotateY, cShape.rotateZ), 0, 0, 0);
		}
		return mesh;
	}

	public void OnTransformed(IWorldAccessor worldAccessor, ITreeAttribute tree, int degreeRotation, Dictionary<int, AssetLocation> oldBlockIdMapping, Dictionary<int, AssetLocation> oldItemIdMapping, EnumAxis? flipAxis)
	{
		MeshAngle = tree.GetFloat("meshAngle");
		MeshAngle -= (float)degreeRotation * ((float)Math.PI / 180f);
		tree.SetFloat("meshAngle", MeshAngle);
	}
}
