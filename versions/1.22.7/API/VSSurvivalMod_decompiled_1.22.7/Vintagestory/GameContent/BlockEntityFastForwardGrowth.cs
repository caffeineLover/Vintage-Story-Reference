using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.GameContent;

public class BlockEntityFastForwardGrowth : BlockEntity
{
	protected static Random rand = new Random();

	protected double totalHoursLastUpdate;

	protected bool allowundergroundfarming;

	protected double previousHourInterval;

	protected BlockPos upPos;

	protected ModSystemFarming msFarming;

	protected int roomness;

	protected virtual int RainHeightOffset => 0;

	public double TotalHoursLastUpdate => totalHoursLastUpdate;

	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
		msFarming = api.ModLoader.GetModSystem<ModSystemFarming>();
		allowundergroundfarming = Api.World.Config.GetBool("allowUndergroundFarming");
		upPos = Pos.UpCopy();
		registerTickListener(api);
	}

	protected virtual void registerTickListener(ICoreAPI api)
	{
		if (api.Side != EnumAppSide.Server || !Api.World.Config.GetBool("processCrops", defaultValue: true))
		{
			return;
		}
		RegisterGameTickListener(delegate(float dt)
		{
			if ((Api as ICoreServerAPI).World.IsFullyLoadedChunk(Pos))
			{
				Update(dt);
			}
		}, 4500, rand.Next(4500));
	}

	protected virtual void Update(float dt)
	{
		double hoursSinceLastUpdate = Api.World.Calendar.TotalHours - totalHoursLastUpdate;
		double hourIntervall = 3.0 + rand.NextDouble();
		if (hoursSinceLastUpdate < hourIntervall)
		{
			if (hoursSinceLastUpdate < 0.0)
			{
				onRollback(0.0 - hoursSinceLastUpdate);
				totalHoursLastUpdate = Api.World.Calendar.TotalHours;
			}
			ShortUpdate(dt);
			return;
		}
		IWorldAccessor world = Api.World;
		IBlockAccessor blockAccessor = world.BlockAccessor;
		float hoursPerDay = world.Calendar.HoursPerDay;
		if (hoursSinceLastUpdate > (double)((float)world.Calendar.DaysPerYear * hoursPerDay))
		{
			float oneYear = (float)world.Calendar.DaysPerYear * hoursPerDay;
			totalHoursLastUpdate += hoursSinceLastUpdate - (double)oneYear;
			hoursSinceLastUpdate = oneYear;
		}
		ClimateCondition conds = blockAccessor.GetClimateAt(Pos, EnumGetClimateMode.ForSuppliedDate_TemperatureRainfallOnly, totalHoursLastUpdate / (double)hoursPerDay);
		if (conds == null)
		{
			return;
		}
		int lightpenalty = 0;
		if (!allowundergroundfarming)
		{
			lightpenalty = Math.Max(0, world.SeaLevel - Pos.Y);
		}
		int sunlight = blockAccessor.GetLightLevel(upPos, (!allowundergroundfarming) ? EnumLightLevelType.OnlySunLight : EnumLightLevelType.MaxLight);
		double lightGrowthSpeedFactor = GameMath.Clamp(1f - (float)(msFarming.Config.DelayGrowthBelowSunLight - (sunlight - lightpenalty)) * msFarming.Config.LossPerLevel, 0f, 1f);
		if (blockAccessor.GetRainMapHeightAt(Pos.X, Pos.Z) > Pos.Y + RainHeightOffset)
		{
			if (hoursSinceLastUpdate > 12.0 || ((((int)(totalHoursLastUpdate + hoursSinceLastUpdate) / 12) ^ ((int)totalHoursLastUpdate / 12)) & 1) == 1)
			{
				roomness = GetRoomness();
			}
		}
		else
		{
			roomness = 0;
		}
		beginIntervalledUpdate(out var intervalCallback, out var endCallback);
		float delayGrowthBelowTemperature = msFarming.Config.DelayGrowthBelowTemperature;
		float lossPerDegree = msFarming.Config.LossPerDegree;
		while (hoursSinceLastUpdate > hourIntervall)
		{
			hoursSinceLastUpdate -= hourIntervall;
			totalHoursLastUpdate += hourIntervall;
			previousHourInterval = hourIntervall;
			hourIntervall = 3.0 + rand.NextDouble();
			blockAccessor.GetClimateAt(Pos, conds, EnumGetClimateMode.ForSuppliedDate_TemperatureRainfallOnly, totalHoursLastUpdate / (double)hoursPerDay);
			if (roomness > 0)
			{
				conds.Temperature += 5f;
			}
			double growthChance = 1f + (conds.Temperature - delayGrowthBelowTemperature) * lossPerDegree;
			bool growthPaused = rand.NextDouble() > growthChance;
			intervalCallback?.Invoke(hourIntervall, conds, lightGrowthSpeedFactor, growthPaused);
		}
		endCallback?.Invoke();
	}

	protected virtual int GetRoomness()
	{
		Room room = msFarming.Roomreg?.GetRoomForPosition(upPos);
		if (room == null || room.SkylightCount <= room.NonSkylightCount || room.ExitCount != 0)
		{
			return 0;
		}
		return 1;
	}

	protected virtual void ShortUpdate(float dt)
	{
	}

	protected virtual void onRollback(double hoursrolledback)
	{
	}

	protected virtual void beginIntervalledUpdate(out FarmlandFastForwardUpdate onInterval, out FarmlandUpdateEnd onEnd)
	{
		onInterval = null;
		onEnd = null;
	}

	public override void OnBlockPlaced(ItemStack byItemStack = null)
	{
		base.OnBlockPlaced(byItemStack);
		totalHoursLastUpdate = Api.World.Calendar.TotalHours;
	}

	public virtual void OnCreatedFromSoil(Block block, TreeAttribute existingFertilityData = null)
	{
		totalHoursLastUpdate = Api.World.Calendar.TotalHours;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldAccessForResolve)
	{
		base.FromTreeAttributes(tree, worldAccessForResolve);
		if (tree.HasAttribute("totalHoursFertilityCheck"))
		{
			totalHoursLastUpdate = tree.GetDouble("totalHoursFertilityCheck");
		}
		else
		{
			totalHoursLastUpdate = tree.GetDouble("totalDaysFertilityCheck") * 24.0;
		}
		roomness = tree.GetInt("roomness");
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
		tree.SetDouble("totalHoursFertilityCheck", totalHoursLastUpdate);
		tree.SetInt("roomness", roomness);
	}

	public virtual void SendFullUpdateToClient(ICoreServerAPI sapi, IServerPlayer player)
	{
		int oldRoomness = roomness;
		bool skyExposed = sapi.World.BlockAccessor.GetRainMapHeightAt(Pos.X, Pos.Z) <= Pos.Y + RainHeightOffset;
		roomness = ((!skyExposed) ? GetRoomness() : 0);
		if (roomness != oldRoomness)
		{
			MarkDirty();
		}
	}
}
