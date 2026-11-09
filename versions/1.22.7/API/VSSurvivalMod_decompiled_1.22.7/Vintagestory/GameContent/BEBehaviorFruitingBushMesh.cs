using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BEBehaviorFruitingBushMesh : BlockEntityBehavior, ITexPositionSource
{
	protected ICoreClientAPI capi;

	protected string randomVariant;

	protected string healthAndBerryState;

	private MeshData? bushMesh;

	private Dictionary<string, CompositeTexture>? textureMapping;

	public Size2i AtlasSize => capi.BlockTextureAtlas.Size;

	public TextureAtlasPosition this[string textureCode]
	{
		get
		{
			textureMapping.TryGetValue(textureCode, out CompositeTexture compositeTexture);
			capi.BlockTextureAtlas.GetOrInsertTexture(compositeTexture.Base, out var _, out var texPos);
			return texPos;
		}
	}

	public FruitingBushState BState => Blockentity.GetBehavior<BEBehaviorFruitingBush>().BState;

	protected virtual string meshCacheKey => string.Concat(new string[5]
	{
		base.Block.Code,
		"-",
		healthAndBerryState,
		"-",
		randomVariant
	});

	public BEBehaviorFruitingBushMesh(BlockEntity blockentity)
		: base(blockentity)
	{
	}

	public override void Initialize(ICoreAPI api, JsonObject properties)
	{
		base.Initialize(api, properties);
		capi = api as ICoreClientAPI;
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		ensureMeshExists();
		mesher.AddMeshData(bushMesh);
		return true;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		base.FromTreeAttributes(tree, worldAccessForResolve);
		if (BState.MeshDirty)
		{
			BState.MeshDirty = false;
			bushMesh = null;
			Blockentity.MarkDirty(redrawOnClient: true);
		}
	}

	protected void ensureMeshExists()
	{
		if (bushMesh != null)
		{
			return;
		}
		bool hideBerries = false;
		bool isYoung = false;
		string berryStage = "";
		string healthState = (healthAndBerryState = Blockentity.GetBehavior<BEBehaviorFruitingBush>().GetHealthState().ToString()
			.ToLowerInvariant());
		if (healthState == "barren")
		{
			hideBerries = true;
			healthAndBerryState = "empty";
		}
		switch (BState.Growthstate)
		{
		case EnumFruitingBushGrowthState.Flowering:
			berryStage = "flowering";
			if (healthAndBerryState != "empty")
			{
				healthAndBerryState = healthAndBerryState + "-" + berryStage;
			}
			break;
		case EnumFruitingBushGrowthState.Ripening:
			berryStage = "unripe";
			if (healthAndBerryState != "empty")
			{
				healthAndBerryState = healthAndBerryState + "-" + berryStage;
			}
			break;
		case EnumFruitingBushGrowthState.Ripe:
			berryStage = "ripe";
			if (healthAndBerryState != "empty")
			{
				healthAndBerryState = healthAndBerryState + "-" + berryStage;
			}
			break;
		case EnumFruitingBushGrowthState.Mature:
			hideBerries = true;
			healthAndBerryState = "empty";
			break;
		case EnumFruitingBushGrowthState.Dormant:
			hideBerries = true;
			healthAndBerryState = "dormant";
			break;
		case EnumFruitingBushGrowthState.Young:
			hideBerries = true;
			isYoung = true;
			healthAndBerryState = "young";
			break;
		}
		int posHash = GameMath.MurmurHash3(base.Pos.X, base.Pos.Y, base.Pos.Z);
		int textureVariant = GameMath.Mod(posHash, base.Block.Attributes[isYoung ? "youngTextureVariants" : "textureVariants"].AsInt(1));
		randomVariant = textureVariant.ToString() ?? "";
		CompositeShape cshape = base.Block.Shape;
		if (isYoung)
		{
			cshape = base.Block.Attributes["youngShape"].AsObject<CompositeShape>();
			cshape.LoadAlternates(capi.Assets, capi.Logger);
		}
		if (cshape.BakedAlternates != null)
		{
			int variant = GameMath.Mod(posHash, cshape.BakedAlternates.Length);
			randomVariant = randomVariant + "-" + variant;
			cshape = cshape.BakedAlternates[variant];
		}
		bushMesh = ObjectCacheUtil.GetOrCreate(capi, meshCacheKey, delegate
		{
			string[] array = null;
			if (hideBerries)
			{
				array = new string[1] { "Berries/*" };
			}
			textureMapping = base.Block.Attributes[isYoung ? "youngTextureMapping" : "textureMapping"].AsObject<Dictionary<string, CompositeTexture>>();
			foreach (var (key, compositeTexture2) in textureMapping)
			{
				compositeTexture2.Base.Path = compositeTexture2.Base.Path.Replace("{berrystage}", berryStage).Replace("{healthstate}", healthState);
				if (compositeTexture2.Alternates != null)
				{
					CompositeTexture[] alternates = compositeTexture2.Alternates;
					foreach (CompositeTexture compositeTexture3 in alternates)
					{
						compositeTexture3.Base.Path = compositeTexture3.Base.Path.Replace("{berrystage}", berryStage).Replace("{healthstate}", healthState);
					}
				}
				CompositeTexture.Bake(capi.Assets, compositeTexture2);
				CompositeTexture[] alternates2 = compositeTexture2.Alternates;
				int num = ((alternates2 != null) ? alternates2.Length : 0);
				if (num > 0)
				{
					int num2 = GameMath.Mod(textureVariant, num + 1);
					if (num2 > 0)
					{
						textureMapping[key] = compositeTexture2.Alternates[num2 - 1];
					}
				}
			}
			AssetLocation assetLocation = cshape.Base.Clone();
			Shape shapeBase = capi.Assets.Get<Shape>(assetLocation.WithPathPrefixOnce("shapes/").WithPathAppendixOnce(".json"));
			if (BState.Growthstate == EnumFruitingBushGrowthState.Dormant && base.Block.Variant["type"] != "strawberry")
			{
				array = array.Append(new string[1] { "Leaves/*" });
			}
			capi.Tesselator.TesselateShape(new TesselationMetaData
			{
				TexSource = this,
				UsesColorMap = true,
				IgnoreElements = array,
				Rotation = cshape.RotateXYZCopy
			}, shapeBase, out var modeldata);
			for (int j = 0; j < modeldata.ColorMapIdsCount; j++)
			{
				if (modeldata.ClimateColorMapIds[j] > 0)
				{
					modeldata.ClimateColorMapIds[j] = (byte)(base.Block.ClimateColorMapResolved.RectIndex + 1);
				}
				if (modeldata.SeasonColorMapIds[j] > 0)
				{
					modeldata.SeasonColorMapIds[j] = (byte)(base.Block.SeasonColorMapResolved.RectIndex + 1);
				}
			}
			return modeldata;
		});
	}
}
