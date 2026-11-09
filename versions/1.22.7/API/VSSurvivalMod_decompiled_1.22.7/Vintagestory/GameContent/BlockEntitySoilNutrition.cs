using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockEntitySoilNutrition : BlockEntityFastForwardGrowth, ITexPositionSource
{
	protected enum EnumWaterSearchResult
	{
		Found,
		NotFound,
		Deferred
	}

	public static Vintagestory.API.Datastructures.OrderedDictionary<string, float> Fertilities = new Vintagestory.API.Datastructures.OrderedDictionary<string, float>
	{
		{ "verylow", 5f },
		{ "low", 25f },
		{ "medium", 50f },
		{ "compost", 65f },
		{ "high", 80f }
	};

	protected HashSet<string> PermaBoosts = new HashSet<string>();

	protected float totalHoursWaterRetention;

	protected float[] nutrients = new float[3];

	protected float[] slowReleaseNutrients = new float[3];

	protected Dictionary<string, float> fertilizerOverlayStrength;

	protected float moistureLevel;

	protected double lastWaterSearchedTotalHours;

	protected int[] originalFertility = new int[3];

	protected bool saltExposed;

	protected bool farmlandIsAtChunkEdge;

	protected float[] damageAccum = new float[Enum.GetValues(typeof(EnumCropStressType)).Length];

	protected Vec3d tmpPos = new Vec3d();

	protected float lastWaterDistance = 99f;

	protected double lastMoistureLevelUpdateTotalDays;

	protected float fertilityRecoverySpeed = 0.25f;

	protected float growthRateMul = 1f;

	protected TextureAtlasPosition fertilizerTexturePos;

	protected ICoreClientAPI capi;

	public MeshData FertilizerQuad;

	public int Roomness => roomness;

	public bool IsVisiblyMoist => (double)moistureLevel > 0.1;

	protected virtual bool RecoverFertility => true;

	protected virtual bool ConsidersMoistureLevels => true;

	public float[] Nutrients => nutrients;

	public float MoistureLevel => moistureLevel;

	public int[] OriginalFertility => originalFertility;

	public BlockPos UpPos => upPos;

	public Size2i AtlasSize => capi.BlockTextureAtlas.Size;

	public TextureAtlasPosition this[string textureCode] => fertilizerTexturePos;

	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
		capi = api as ICoreClientAPI;
		totalHoursWaterRetention = Api.World.Calendar.HoursPerDay * 4f;
		fertilityRecoverySpeed = Api.World.Config.GetFloat("fertilityRecoverySpeed", fertilityRecoverySpeed);
		growthRateMul = (float)Api.World.Config.GetDecimal("cropGrowthRateMul", growthRateMul);
		updateFertilizerQuad();
	}

	public override void OnCreatedFromSoil(Block block, TreeAttribute existingFertilityData = null)
	{
		base.OnCreatedFromSoil(block, existingFertilityData);
		if (existingFertilityData == null)
		{
			string strfertility = block.Variant["fertility"];
			float val = 0f;
			if (strfertility == null || !Fertilities.TryGetValue(strfertility, out val))
			{
				val = 25f;
			}
			originalFertility[0] = (int)val;
			originalFertility[1] = (int)val;
			originalFertility[2] = (int)val;
			nutrients[0] = originalFertility[0];
			nutrients[1] = originalFertility[1];
			nutrients[2] = originalFertility[2];
		}
		else
		{
			loadEnvData(existingFertilityData);
		}
		tryUpdateMoistureLevel(Api.World.Calendar.TotalDays, searchNearbyWater: true);
	}

	public bool OnBlockInteract(IPlayer byPlayer)
	{
		ItemStack stack = byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack;
		JsonObject obj = stack?.Collectible?.Attributes?["fertilizerProps"];
		if (obj == null || !obj.Exists)
		{
			return false;
		}
		FertilizerProps props = obj.AsObject<FertilizerProps>();
		if (props == null)
		{
			return false;
		}
		float nAdd = Math.Min(Math.Max(0f, 150f - slowReleaseNutrients[0]), props.N);
		float pAdd = Math.Min(Math.Max(0f, 150f - slowReleaseNutrients[1]), props.P);
		float kAdd = Math.Min(Math.Max(0f, 150f - slowReleaseNutrients[2]), props.K);
		slowReleaseNutrients[0] += nAdd;
		slowReleaseNutrients[1] += pAdd;
		slowReleaseNutrients[2] += kAdd;
		if (props.PermaBoost != null && !PermaBoosts.Contains(props.PermaBoost.Code))
		{
			originalFertility[0] += props.PermaBoost.N;
			originalFertility[1] += props.PermaBoost.P;
			originalFertility[2] += props.PermaBoost.K;
			PermaBoosts.Add(props.PermaBoost.Code);
		}
		string fertCode = stack.Collectible.Attributes["fertilizerTextureCode"].AsString();
		if (fertCode != null)
		{
			if (fertilizerOverlayStrength == null)
			{
				fertilizerOverlayStrength = new Dictionary<string, float>();
			}
			fertilizerOverlayStrength.TryGetValue(fertCode, out var prevValue);
			fertilizerOverlayStrength[fertCode] = prevValue + Math.Max(nAdd, Math.Max(kAdd, pAdd));
		}
		updateFertilizerQuad();
		byPlayer.InventoryManager.ActiveHotbarSlot.TakeOut(1);
		byPlayer.InventoryManager.ActiveHotbarSlot.MarkDirty();
		(byPlayer as IClientPlayer)?.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
		SoundAttributes? obj2 = Api.World.BlockAccessor.GetBlock(Pos).Attributes?["fertilizedSound"].AsObject<SoundAttributes?>(null, base.Block.Code.Domain, randomPitch: true);
		Api.World.PlaySoundAt(obj2 ?? new SoundAttributes(AssetLocation.Create("sounds/block/dirt"), withRandomPitch: true)
		{
			Range = 12f
		}, Pos, 0.25, byPlayer);
		MarkDirty();
		return true;
	}

	public virtual void OnCropBlockBroken()
	{
		for (int i = 0; i < damageAccum.Length; i++)
		{
			damageAccum[i] = 0f;
		}
		MarkDirty(redrawOnClient: true);
	}

	protected float GetNearbyWaterDistance(out EnumWaterSearchResult result, float hoursPassed)
	{
		float waterDistance = 99f;
		farmlandIsAtChunkEdge = false;
		bool saltWater = false;
		Api.World.BlockAccessor.SearchFluidBlocks(new BlockPos(Pos.X - 4, Pos.Y, Pos.Z - 4), new BlockPos(Pos.X + 4, Pos.Y, Pos.Z + 4), delegate(Block block, BlockPos pos)
		{
			if (block.LiquidCode == "water")
			{
				waterDistance = Math.Min(waterDistance, Math.Max(Math.Abs(pos.X - Pos.X), Math.Abs(pos.Z - Pos.Z)));
			}
			if (block.LiquidCode == "saltwater")
			{
				saltWater = true;
			}
			return true;
		}, delegate
		{
			farmlandIsAtChunkEdge = true;
		});
		if (saltWater)
		{
			damageAccum[4] += hoursPassed;
		}
		result = EnumWaterSearchResult.Deferred;
		if (farmlandIsAtChunkEdge)
		{
			return 99f;
		}
		lastWaterSearchedTotalHours = Api.World.Calendar.TotalHours;
		if (waterDistance < 4f)
		{
			result = EnumWaterSearchResult.Found;
			return waterDistance;
		}
		result = EnumWaterSearchResult.NotFound;
		return 99f;
	}

	protected bool tryUpdateMoistureLevel(double totalDays, bool searchNearbyWater)
	{
		float dist = 99f;
		if (searchNearbyWater)
		{
			dist = GetNearbyWaterDistance(out var res, 0f);
			switch (res)
			{
			case EnumWaterSearchResult.Deferred:
				return false;
			default:
				dist = 99f;
				break;
			case EnumWaterSearchResult.Found:
				break;
			}
			lastWaterDistance = dist;
		}
		if (updateMoistureLevel(totalDays, dist))
		{
			UpdateFarmlandBlock();
		}
		return true;
	}

	protected bool updateMoistureLevel(double totalDays, float waterDistance)
	{
		bool skyExposed = Api.World.BlockAccessor.GetRainMapHeightAt(Pos.X, Pos.Z) <= Pos.Y + RainHeightOffset;
		return updateMoistureLevel(totalDays, waterDistance, skyExposed);
	}

	protected bool updateMoistureLevel(double totalDays, float waterDistance, bool skyExposed, ClimateCondition baseClimate = null)
	{
		float minMoisture = GameMath.Clamp(1f - waterDistance / 4f, 0f, 1f);
		double hoursPassed = Math.Min((totalDays - lastMoistureLevelUpdateTotalDays) * (double)Api.World.Calendar.HoursPerDay, totalHoursWaterRetention);
		if (hoursPassed < 0.029999999329447746)
		{
			if (hoursPassed < 0.0)
			{
				lastMoistureLevelUpdateTotalDays = totalDays;
				return false;
			}
			moistureLevel = Math.Max(moistureLevel, minMoisture);
			return false;
		}
		tmpPos.Set((double)Pos.X + 0.5, (double)Pos.Y + 0.5, (double)Pos.Z + 0.5);
		moistureLevel = Math.Max(minMoisture, moistureLevel - (float)hoursPassed / totalHoursWaterRetention);
		if (skyExposed)
		{
			if (baseClimate == null && hoursPassed > 0.0)
			{
				baseClimate = Api.World.BlockAccessor.GetClimateAt(Pos, EnumGetClimateMode.WorldGenValues, totalDays - hoursPassed / (double)Api.World.Calendar.HoursPerDay / 2.0);
			}
			while (hoursPassed > 0.0)
			{
				double rainLevel = msFarming.Wsys.GetPrecipitation(Pos, totalDays - hoursPassed / (double)Api.World.Calendar.HoursPerDay, baseClimate);
				moistureLevel = GameMath.Clamp(moistureLevel + (float)rainLevel / 3f, 0f, 1f);
				hoursPassed -= 1.0;
			}
		}
		lastMoistureLevelUpdateTotalDays = totalDays;
		return true;
	}

	protected override void ShortUpdate(float dt)
	{
		bool skyExposed = Api.World.BlockAccessor.GetRainMapHeightAt(Pos.X, Pos.Z) <= Pos.Y + RainHeightOffset;
		if (updateMoistureLevel(Api.World.Calendar.TotalDays, lastWaterDistance, skyExposed))
		{
			UpdateFarmlandBlock();
		}
	}

	protected override void beginIntervalledUpdate(out FarmlandFastForwardUpdate onInterval, out FarmlandUpdateEnd onEnd)
	{
		bool nearbyWaterTested = false;
		bool growTallGrass = false;
		float waterDistance = 99f;
		Block upblock = Api.World.BlockAccessor.GetBlock(upPos);
		bool skyExposed = Api.World.BlockAccessor.GetRainMapHeightAt(Pos.X, Pos.Z) <= Pos.Y + RainHeightOffset;
		EnumSoilNutrient? currentlyConsumedNutrient = null;
		if (upblock.CropProps != null)
		{
			currentlyConsumedNutrient = upblock.CropProps.RequiredNutrient;
		}
		onInterval = delegate(double hourIntervall, ClimateCondition conds, double lightGrowthSpeedFactor, bool growthPaused)
		{
			if (!nearbyWaterTested)
			{
				waterDistance = GetNearbyWaterDistance(out var result, (float)hourIntervall);
				switch (result)
				{
				case EnumWaterSearchResult.Deferred:
					return;
				case EnumWaterSearchResult.NotFound:
					waterDistance = 99f;
					break;
				}
				nearbyWaterTested = true;
				lastWaterDistance = waterDistance;
			}
			updateMoistureLevel(totalHoursLastUpdate / (double)Api.World.Calendar.HoursPerDay, waterDistance, skyExposed, conds);
			if (!growthPaused)
			{
				updateSoilFertility(currentlyConsumedNutrient, RecoverFertility);
			}
			growTallGrass |= BlockEntityFastForwardGrowth.rand.NextDouble() < 0.006;
		};
		onEnd = delegate
		{
			if (growTallGrass && upblock.BlockMaterial == EnumBlockMaterial.Air)
			{
				double num = BlockEntityFastForwardGrowth.rand.NextDouble() * (double)msFarming.Config.TotalWeedChance;
				for (int i = 0; i < msFarming.Config.WeedBlockCodes.Length; i++)
				{
					num -= (double)msFarming.Config.WeedBlockCodes[i].Chance;
					if (num <= 0.0)
					{
						Block block = Api.World.GetBlock(msFarming.Config.WeedBlockCodes[i].Code);
						if (block != null)
						{
							Api.World.BlockAccessor.SetBlock(block.BlockId, upPos);
						}
						break;
					}
				}
			}
			updateFertilizerQuad();
			UpdateFarmlandBlock();
			Api.World.BlockAccessor.MarkBlockEntityDirty(Pos);
		};
	}

	protected override void onRollback(double hoursrolledback)
	{
		base.onRollback(hoursrolledback);
		lastMoistureLevelUpdateTotalDays -= hoursrolledback;
		lastWaterSearchedTotalHours -= hoursrolledback;
	}

	protected void updateSoilFertility(EnumSoilNutrient? currentlyConsumedNutrient, bool recoverFertility)
	{
		float[] npkRegain = new float[3]
		{
			recoverFertility ? fertilityRecoverySpeed : 0f,
			recoverFertility ? fertilityRecoverySpeed : 0f,
			recoverFertility ? fertilityRecoverySpeed : 0f
		};
		if (currentlyConsumedNutrient.HasValue)
		{
			npkRegain[(int)currentlyConsumedNutrient.Value] /= 3f;
		}
		for (int i = 0; i < 3; i++)
		{
			nutrients[i] += Math.Max(0f, npkRegain[i] + Math.Min(0f, (float)originalFertility[i] - nutrients[i] - npkRegain[i]));
			if (slowReleaseNutrients[i] > 0f)
			{
				float release = Math.Min(0.25f, slowReleaseNutrients[i]);
				nutrients[i] = Math.Min(100f, nutrients[i] + release);
				slowReleaseNutrients[i] = Math.Max(0f, slowReleaseNutrients[i] - release);
			}
			else if (nutrients[i] > (float)originalFertility[i])
			{
				nutrients[i] = Math.Max(originalFertility[i], nutrients[i] - 0.05f);
			}
		}
		if (fertilizerOverlayStrength == null || fertilizerOverlayStrength.Count <= 0)
		{
			return;
		}
		string[] array = fertilizerOverlayStrength.Keys.ToArray();
		foreach (string code in array)
		{
			float newStr = fertilizerOverlayStrength[code] - fertilityRecoverySpeed;
			if (newStr < 0f)
			{
				fertilizerOverlayStrength.Remove(code);
			}
			else
			{
				fertilizerOverlayStrength[code] = newStr;
			}
		}
	}

	public float GetGrowthRate(EnumSoilNutrient nutrient)
	{
		float moistFactor = (float)Math.Pow(Math.Max(0.01, (double)(moistureLevel * 100f / 70f) - 0.143), 0.35);
		float num = nutrients[(int)nutrient];
		if (!(num > 75f))
		{
			if (!(num > 50f))
			{
				if (!(num > 35f))
				{
					if (!(num > 20f))
					{
						if (num > 5f)
						{
							return moistFactor * 0.3f;
						}
						return moistFactor * 0.1f;
					}
					return moistFactor * 0.6f;
				}
				return moistFactor * 0.9f;
			}
			return moistFactor * 1f;
		}
		return moistFactor * 1.1f;
	}

	public void ConsumeNutrients(EnumSoilNutrient nutrient, float amount)
	{
		nutrients[(int)nutrient] = Math.Max(0f, nutrients[(int)nutrient] - amount);
		UpdateFarmlandBlock();
		MarkDirty(redrawOnClient: true);
	}

	protected virtual void UpdateFarmlandBlock()
	{
	}

	public int GetFertilityLevel(float fertiltyValue)
	{
		int i = 0;
		foreach (KeyValuePair<string, float> fertility in Fertilities)
		{
			if (fertility.Value >= fertiltyValue)
			{
				return i;
			}
			i++;
		}
		return Fertilities.Count - 1;
	}

	protected void updateFertilizerQuad()
	{
		if (capi == null)
		{
			return;
		}
		AssetLocation loc = new AssetLocation();
		if (fertilizerOverlayStrength == null || fertilizerOverlayStrength.Count == 0)
		{
			bool num = FertilizerQuad != null;
			FertilizerQuad = null;
			if (num)
			{
				MarkDirty(redrawOnClient: true);
			}
			return;
		}
		int i = 0;
		foreach (KeyValuePair<string, float> val in fertilizerOverlayStrength)
		{
			string intensity = "low";
			if (val.Value > 50f)
			{
				intensity = "med";
			}
			if (val.Value > 100f)
			{
				intensity = "high";
			}
			if (i > 0)
			{
				loc.Path += "++0~";
			}
			AssetLocation assetLocation = loc;
			assetLocation.Path = assetLocation.Path + "block/soil/farmland/fertilizer/" + val.Key + "-" + intensity;
			i++;
		}
		capi.BlockTextureAtlas.GetOrInsertTexture(loc, out var _, out var newFertilizerTexturePos);
		if (fertilizerTexturePos != newFertilizerTexturePos)
		{
			fertilizerTexturePos = newFertilizerTexturePos;
			genFertilizerQuad();
			MarkDirty(redrawOnClient: true);
		}
	}

	protected virtual void genFertilizerQuad()
	{
		Shape shape = capi.Assets.TryGet(new AssetLocation("shapes/block/farmland-fertilizer.json")).ToObject<Shape>();
		capi.Tesselator.TesselateShape(new TesselationMetaData
		{
			TypeForLogging = "farmland fertilizer quad",
			TexSource = this
		}, shape, out FertilizerQuad);
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
	{
		base.FromTreeAttributes(tree, worldForResolving);
		loadEnvData(tree);
		updateFertilizerQuad();
	}

	protected void loadEnvData(ITreeAttribute tree)
	{
		nutrients[0] = tree.GetFloat("n");
		nutrients[1] = tree.GetFloat("p");
		nutrients[2] = tree.GetFloat("k");
		slowReleaseNutrients[0] = tree.GetFloat("slowN");
		slowReleaseNutrients[1] = tree.GetFloat("slowP");
		slowReleaseNutrients[2] = tree.GetFloat("slowK");
		moistureLevel = tree.GetFloat("moistureLevel");
		lastWaterSearchedTotalHours = tree.GetDouble("lastWaterSearchedTotalHours");
		if (!tree.HasAttribute("originalFertilityN"))
		{
			originalFertility[0] = tree.GetInt("originalFertility");
			originalFertility[1] = tree.GetInt("originalFertility");
			originalFertility[2] = tree.GetInt("originalFertility");
		}
		else
		{
			originalFertility[0] = tree.GetInt("originalFertilityN");
			originalFertility[1] = tree.GetInt("originalFertilityP");
			originalFertility[2] = tree.GetInt("originalFertilityK");
		}
		lastMoistureLevelUpdateTotalDays = tree.GetDouble("lastMoistureLevelUpdateTotalDays");
		lastWaterDistance = tree.GetFloat("lastWaterDistance");
		saltExposed = tree.GetBool("saltExposed");
		string[] permaboosts = (tree as TreeAttribute).GetStringArray("permaBoosts");
		if (permaboosts != null)
		{
			PermaBoosts.AddRange(permaboosts);
		}
		ITreeAttribute ftree = tree.GetTreeAttribute("fertilizerOverlayStrength");
		if (ftree != null)
		{
			fertilizerOverlayStrength = new Dictionary<string, float>();
			{
				foreach (KeyValuePair<string, IAttribute> val in ftree)
				{
					fertilizerOverlayStrength[val.Key] = (val.Value as FloatAttribute).value;
				}
				return;
			}
		}
		fertilizerOverlayStrength = null;
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetFloat("n", nutrients[0]);
		tree.SetFloat("p", nutrients[1]);
		tree.SetFloat("k", nutrients[2]);
		tree.SetFloat("slowN", slowReleaseNutrients[0]);
		tree.SetFloat("slowP", slowReleaseNutrients[1]);
		tree.SetFloat("slowK", slowReleaseNutrients[2]);
		tree.SetFloat("moistureLevel", moistureLevel);
		tree.SetDouble("lastWaterSearchedTotalHours", lastWaterSearchedTotalHours);
		tree.SetInt("originalFertilityN", originalFertility[0]);
		tree.SetInt("originalFertilityP", originalFertility[1]);
		tree.SetInt("originalFertilityK", originalFertility[2]);
		tree.SetBool("saltExposed", damageAccum[4] > 1f);
		tree.SetDouble("lastMoistureLevelUpdateTotalDays", lastMoistureLevelUpdateTotalDays);
		tree.SetFloat("lastWaterDistance", lastWaterDistance);
		(tree as TreeAttribute).SetStringArray("permaBoosts", PermaBoosts.ToArray());
		if (fertilizerOverlayStrength == null)
		{
			return;
		}
		TreeAttribute ftree = (TreeAttribute)(tree["fertilizerOverlayStrength"] = new TreeAttribute());
		foreach (KeyValuePair<string, float> val in fertilizerOverlayStrength)
		{
			ftree.SetFloat(val.Key, val.Value);
		}
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
		dsc.AppendLine(Lang.Get("farmland-nutrientlevels", Math.Round(nutrients[0], 1), Math.Round(nutrients[1], 1), Math.Round(nutrients[2], 1)));
		float snn = (float)Math.Round(slowReleaseNutrients[0], 1);
		float snp = (float)Math.Round(slowReleaseNutrients[1], 1);
		float snk = (float)Math.Round(slowReleaseNutrients[2], 1);
		if (snn > 0f || snp > 0f || snk > 0f)
		{
			List<string> nutrs = new List<string>();
			if (snn > 0f)
			{
				nutrs.Add(Lang.Get("+{0}% N", snn));
			}
			if (snp > 0f)
			{
				nutrs.Add(Lang.Get("+{0}% P", snp));
			}
			if (snk > 0f)
			{
				nutrs.Add(Lang.Get("+{0}% K", snk));
			}
			dsc.AppendLine(Lang.Get("farmland-activefertilizer", string.Join(", ", nutrs)));
		}
		if (ConsidersMoistureLevels)
		{
			float moisture = (float)Math.Round(moistureLevel * 100f, 0);
			string colorm = ColorUtil.Int2Hex(GuiStyle.DamageColorGradient[(int)Math.Min(99f, moisture)]);
			dsc.AppendLine(Lang.Get("farmland-moisture", colorm, moisture));
		}
		if (roomness > 0)
		{
			dsc.AppendLine(Lang.Get("greenhousetempbonus"));
		}
		if (saltExposed)
		{
			dsc.AppendLine(Lang.Get("farmland-saltdamage"));
		}
	}

	public void WaterFarmland(float dt, bool waterNeightbours = true)
	{
		float prevLevel = moistureLevel;
		moistureLevel = Math.Min(1f, moistureLevel + dt / 2f);
		if (waterNeightbours)
		{
			BlockFacing[] hORIZONTALS = BlockFacing.HORIZONTALS;
			foreach (BlockFacing neib in hORIZONTALS)
			{
				BlockPos npos = Pos.AddCopy(neib);
				if (Api.World.BlockAccessor.GetBlockEntity(npos) is BlockEntityFarmland bef)
				{
					bef.WaterFarmland(dt / 3f, waterNeightbours: false);
				}
			}
		}
		updateMoistureLevel(Api.World.Calendar.TotalDays, lastWaterDistance);
		UpdateFarmlandBlock();
		if ((double)(moistureLevel - prevLevel) > 0.05)
		{
			MarkDirty(redrawOnClient: true);
		}
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		mesher.AddMeshData(FertilizerQuad);
		return false;
	}
}
