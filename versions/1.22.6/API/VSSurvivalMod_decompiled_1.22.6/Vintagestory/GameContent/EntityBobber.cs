using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.ServerMods;

namespace Vintagestory.GameContent;

public class EntityBobber : EntityProjectile, IRopeRippedListener
{
	protected float swimmingAccum;

	protected float accum;

	protected float catchAccum;

	protected float junkCatchChance;

	protected EnumBobberState bobberState;

	public EntityFish caughtFish;

	protected EntityPartitioning ep;

	protected bool wasSwimming;

	private bool ripSet;

	private BlockPos tmpPos = new BlockPos(0);

	private Queue<FastVec3i> bfsQueue = new Queue<FastVec3i>();

	private HashSet<FastVec3i> visited = new HashSet<FastVec3i>();

	private int pondSize = -1;

	public override float MaterialDensity => 50f;

	public override bool ApplyGravity => true;

	public ItemStack BaitStack { get; set; }

	public long AttachedToEntityId
	{
		get
		{
			return WatchedAttributes.GetLong("attachedToEntityId", 0L);
		}
		set
		{
			WatchedAttributes.SetLong("attachedToEntityId", value);
		}
	}

	public double CastTotalHours
	{
		get
		{
			return WatchedAttributes.GetDouble("castTotalHours");
		}
		set
		{
			WatchedAttributes.SetDouble("castTotalHours", value);
		}
	}

	public override double SwimmingOffsetY => 0.20000000298023224;

	public override bool CanCollect(Entity byEntity)
	{
		return false;
	}

	protected override bool TryAttackEntity(double impactSpeed)
	{
		return true;
	}

	public override void Initialize(EntityProperties properties, ICoreAPI api, long InChunkIndex3d)
	{
		base.Initialize(properties, api, InChunkIndex3d);
		ep = api.ModLoader.GetModSystem<EntityPartitioning>();
		junkCatchChance = properties.Attributes["junkCatchChance"].AsFloat();
		impactSound = null;
		BaitStack?.ResolveBlockOrItem(Api.World);
		ClothManager cm = api.ModLoader.GetModSystem<ClothManager>();
		ClothSystem cs = cm.GetClothSystemAttachedToEntity(EntityId);
		if (api.World.GetEntityById(AttachedToEntityId) == null)
		{
			if (cs != null)
			{
				cm.UnregisterCloth(cs.ClothId);
			}
			Die();
		}
	}

	public override void OnGameTick(float dt)
	{
		if (World.Side == EnumAppSide.Server)
		{
			onServertick(dt);
		}
		if (!ripSet && Api.Side == EnumAppSide.Client)
		{
			ClothSystem cs = Api.ModLoader.GetModSystem<ClothManager>().GetClothSystemAttachedToEntity(EntityId);
			if (cs != null)
			{
				cs.CanRip = true;
			}
			ripSet = true;
		}
		if (Swimming)
		{
			base.Pos.Pitch *= 0.95f;
			base.Pos.Roll *= 0.95f;
			swimmingAccum += dt;
			if (World.EntityDebugMode)
			{
				DebugAttributes.SetString("AttachedToEntityId", AttachedToEntityId.ToString() ?? "");
				UpdateDebugAttributes();
				DebugAttributes.MarkAllDirty();
			}
		}
		else
		{
			base.OnGameTick(dt);
		}
	}

	private void onServertick(float dt)
	{
		if (Swimming && !wasSwimming)
		{
			wasSwimming = true;
			if (Api.World.EntityDebugMode)
			{
				printLocationDebugInfo();
			}
		}
		if (swimmingAccum > 1f)
		{
			if (bobberState == EnumBobberState.Baiting)
			{
				Entity efish = ep.GetNearestEntity(base.Pos.XYZ, 20.0, (Entity e) => e is EntityFish, EnumEntitySearchType.Creatures);
				bobberState = ((efish == null) ? EnumBobberState.NoFishNearby : EnumBobberState.FishNearby);
			}
			if (bobberState == EnumBobberState.FishNearby && swimmingAccum > 15f)
			{
				bobberState = EnumBobberState.NoFishNearby;
			}
			float catchLikelihood;
			bool hasCatchable = HasCatchable(BaitStack, out catchLikelihood);
			if (bobberState == EnumBobberState.NoFishNearby && catchLikelihood > 0f && (double)swimmingAccum > 5.0 / Math.Max(0.04, catchLikelihood))
			{
				if (Api.World.Rand.NextDouble() < (double)junkCatchChance)
				{
					bobberState = EnumBobberState.JunkCatch;
					catchAccum += dt;
					playCatchEffects();
				}
				else
				{
					bobberState = EnumBobberState.NoEntityFishCatch;
					catchAccum += dt;
					if (hasCatchable)
					{
						playCatchEffects();
					}
					else
					{
						bobberState = EnumBobberState.NoCatch;
						catchAccum = 0f;
						swimmingAccum = 0f;
					}
				}
			}
		}
		if (catchAccum == 0f)
		{
			accum += dt;
			if (accum > 0.2f)
			{
				Entity efish2 = ep.GetNearestEntity(base.Pos.XYZ, 1.0, (Entity e) => e is EntityFish, EnumEntitySearchType.Creatures);
				if (efish2 != null)
				{
					caughtFish = efish2 as EntityFish;
					catchAccum += dt;
					playCatchEffects();
				}
			}
		}
		else
		{
			if (!(catchAccum > 0.7f))
			{
				return;
			}
			catchAccum = 0f;
			caughtFish = null;
			bobberState = EnumBobberState.Baiting;
			if (caughtFish != null && caughtFish.Pos.DistanceTo(base.Pos) < 1.0)
			{
				AiTaskManager tm = caughtFish.GetBehavior<EntityBehaviorTaskAI>().TaskManager;
				IAiTask task = tm?.GetTask("fleebobber");
				if (task != null && task.ShouldExecute())
				{
					tm.ExecuteTask(task, task.Slot);
				}
			}
		}
	}

	public bool HasCatchable(ItemStack baitStack, out float abundanceValue)
	{
		if (caughtFish != null && caughtFish.Alive)
		{
			abundanceValue = 1f;
			return true;
		}
		return getRandomFishEntityProperties(baitStack, out abundanceValue) != null;
	}

	public void TryCatchFish(EntityAgent entityCatcher)
	{
		if (caughtFish != null && caughtFish.Alive)
		{
			BaitStack = null;
			WatchedAttributes.MarkPathDirty("baitStack");
			caughtFish.Die(EnumDespawnReason.Expire);
			ItemStack[] dropStacks = caughtFish.GetDrops(World, caughtFish.Pos.XYZInt.AsBlockPos, (entityCatcher as EntityPlayer)?.Player);
			if (dropStacks == null)
			{
				return;
			}
			ItemStack[] array = dropStacks;
			foreach (ItemStack dropStack in array)
			{
				if (!entityCatcher.TryGiveItemStack(dropStack))
				{
					World.SpawnItemEntity(dropStack, entityCatcher.Pos.XYZ);
				}
			}
			return;
		}
		if (bobberState == EnumBobberState.NoEntityFishCatch)
		{
			float abundancevalue;
			EntityProperties etype = getRandomFishEntityProperties(BaitStack, out abundancevalue);
			if (etype == null)
			{
				return;
			}
			BaitStack = null;
			WatchedAttributes.MarkPathDirty("baitStack");
			CollectibleObject collObj = etype.Drops[0].ResolvedItemstack.Collectible;
			string age = ((Api.World.Rand.NextDouble() > (double)abundancevalue) ? "adult" : "juvenile");
			CollectibleObject deadFishItem = Api.World.GetItem(collObj.CodeWithVariant("age", age));
			if (deadFishItem == null)
			{
				deadFishItem = collObj;
			}
			ItemStack dropStack2;
			if (deadFishItem != null)
			{
				dropStack2 = new ItemStack(deadFishItem);
				dropStack2.ResolveBlockOrItem(Api.World);
			}
			else
			{
				dropStack2 = etype.Drops[0].ResolvedItemstack.Clone();
			}
			if (!entityCatcher.TryGiveItemStack(dropStack2))
			{
				World.SpawnItemEntity(dropStack2, entityCatcher.Pos.XYZ);
			}
			Api.ModLoader.GetModSystem<ModSystemFishDepletion>().AddHarvest(base.Pos.XYZ.AsBlockPos, 1);
		}
		if (bobberState != EnumBobberState.JunkCatch)
		{
			return;
		}
		BaitStack = null;
		WatchedAttributes.MarkPathDirty("baitStack");
		WeightedBlockDropItemstack[] drops = base.Properties.Attributes["junkCatches"].AsObject<WeightedBlockDropItemstack[]>();
		float totalWeight = 0f;
		WeightedBlockDropItemstack[] array2 = drops;
		foreach (WeightedBlockDropItemstack drop in array2)
		{
			totalWeight += drop.Weight;
		}
		double rndval = Api.World.Rand.NextDouble() * (double)totalWeight;
		drops.Shuffle(Api.World.Rand);
		array2 = drops;
		foreach (WeightedBlockDropItemstack drop2 in array2)
		{
			rndval -= (double)drop2.Weight;
			if (rndval < 0.0)
			{
				drop2.Resolve(Api.World, "bobber junk catch", Code);
				ItemStack dropStack3 = drop2.ResolvedItemstack.Clone();
				if (!entityCatcher.TryGiveItemStack(dropStack3))
				{
					World.SpawnItemEntity(dropStack3, entityCatcher.Pos.XYZ);
				}
				break;
			}
		}
	}

	private void printLocationDebugInfo()
	{
		getRandomFishEntityProperties(null, out var _, printDebug: true);
	}

	private EntityProperties getRandomFishEntityProperties(ItemStack baitStack, out float abundanceValue, bool printDebug = false)
	{
		Vec3d pos = base.Pos.XYZ;
		tmpPos.Set(pos.XInt, (int)((double)World.SeaLevel * 1.09), pos.ZInt);
		abundanceValue = 0f;
		ClimateCondition climate = World.BlockAccessor.GetClimateAt(tmpPos, EnumGetClimateMode.WorldGenValues);
		if (climate == null)
		{
			return null;
		}
		List<KeyValuePair<string, EntityProperties>> suitableFishPropsWithMapCode = new List<KeyValuePair<string, EntityProperties>>();
		foreach (EntityProperties etype in World.EntityTypes)
		{
			string mapcode = etype.Server.SpawnConditions?.Climate?.MapCode ?? etype.Server.SpawnConditions?.Runtime?.MapCode ?? etype.Server.SpawnConditions?.Worldgen?.MapCode;
			if (mapcode != null && ((ClimateSpawnCondition)(etype.Server.SpawnConditions?.Climate ?? ((object)etype.Server.SpawnConditions?.Runtime) ?? ((object)etype.Server.SpawnConditions?.Worldgen))).MatchesClimate(climate))
			{
				tmpPos.Set(pos.XInt, pos.YInt, pos.ZInt);
				object obj = ((object)etype.Server.SpawnConditions?.Runtime) ?? ((object)etype.Server.SpawnConditions?.Worldgen);
				Block liquidBlock = World.BlockAccessor.GetBlock(tmpPos, 2);
				if (((BaseSpawnConditions)obj).CanSpawnInside(liquidBlock))
				{
					suitableFishPropsWithMapCode.Add(new KeyValuePair<string, EntityProperties>(mapcode, etype));
				}
			}
		}
		int regionSize = World.BlockAccessor.RegionSize;
		IMapRegion mapRegion = World.BlockAccessor.GetMapRegion(pos.XInt / regionSize, pos.ZInt / regionSize);
		int noiseSizeDensityMap = regionSize / TerraGenConfig.animalMapScale;
		int posX = pos.XInt;
		int posZ = pos.ZInt;
		List<EntityProperties> suitableFishProps = new List<EntityProperties>();
		foreach (KeyValuePair<string, ByteDataMap2D> val in mapRegion.AnimalSpawnMaps)
		{
			int num = posX % regionSize;
			int lz = posZ % regionSize;
			float posXInRegionOre = GameMath.Clamp((float)num / (float)regionSize * (float)noiseSizeDensityMap, 0f, noiseSizeDensityMap - 1);
			float posZInRegionOre = GameMath.Clamp((float)lz / (float)regionSize * (float)noiseSizeDensityMap, 0f, noiseSizeDensityMap - 1);
			if (val.Value.GetUnpaddedLerped(posXInRegionOre, posZInRegionOre) > 128f)
			{
				EntityProperties eprops = suitableFishPropsWithMapCode.FirstOrDefault((KeyValuePair<string, EntityProperties> fprops) => fprops.Key == val.Key).Value;
				if (eprops != null)
				{
					suitableFishProps.Add(eprops);
				}
			}
		}
		if (suitableFishProps.Count == 0)
		{
			return null;
		}
		double noiseval = (Api.ModLoader.GetModSystem<FishingSupportModSystem>().NoiseGen.Noise(pos.X, pos.Z) - 0.4000000059604645) * 3.0;
		abundanceValue = (float)GameMath.Clamp(noiseval, 0.20000000298023224, 1.0);
		if (abundanceValue <= 0f)
		{
			return null;
		}
		if (pondSize < 0)
		{
			pondSize = getPondSize();
		}
		if (pondSize < 100)
		{
			return null;
		}
		abundanceValue *= (float)pondSize / 1200f;
		float harvestedHere = Api.ModLoader.GetModSystem<ModSystemFishDepletion>().GetHarvestAmount(base.Pos.XYZ.AsBlockPos);
		float max = (float)ModSystemFishDepletion.MaxHarvestablePerLocation * 0.8f;
		float mul = 1f - GameMath.Clamp(harvestedHere / max - 0.2f, 0f, 1f);
		abundanceValue *= mul;
		string baitTag = BaitStack?.Collectible.Attributes?["baitTag"].AsString() ?? "nobait";
		for (int i = 0; i < suitableFishProps.Count; i++)
		{
			if (!suitableFishProps[i].Attributes["baitTags"].AsArray<string>().Contains<string>(baitTag))
			{
				suitableFishProps.RemoveAt(i);
				i--;
			}
		}
		if (suitableFishProps.Count == 0)
		{
			return null;
		}
		return suitableFishProps[Api.World.Rand.Next(suitableFishProps.Count)];
	}

	private int getPondSize()
	{
		IBlockAccessor ba = Api.World.BlockAccessor;
		_ = base.Pos.XYZ.AsBlockPos;
		visited.Clear();
		bfsQueue.Clear();
		BlockPos curPos = base.Pos.AsBlockPos;
		bfsQueue.Enqueue(new FastVec3i(curPos.X, curPos.Y, curPos.Z));
		BlockFacing[] faces = new BlockFacing[5]
		{
			BlockFacing.NORTH,
			BlockFacing.EAST,
			BlockFacing.SOUTH,
			BlockFacing.WEST,
			BlockFacing.DOWN
		};
		int waterBlocks = 0;
		while (bfsQueue.Count > 0)
		{
			FastVec3i bpos = bfsQueue.Dequeue();
			BlockFacing[] array = faces;
			foreach (BlockFacing facing in array)
			{
				curPos.Set(bpos.X + facing.Normali.X, bpos.Y + facing.Normali.Y, bpos.Z + facing.Normali.Z);
				FastVec3i ipos = new FastVec3i(curPos);
				if (!visited.Contains(ipos))
				{
					visited.Add(ipos);
					if (waterBlocks > 1200)
					{
						return waterBlocks;
					}
					if (ba.GetBlock(curPos, 2).Id != 0)
					{
						bfsQueue.Enqueue(ipos);
						waterBlocks++;
					}
				}
			}
		}
		return waterBlocks;
	}

	private void playCatchEffects()
	{
		base.Pos.Motion.Y -= 0.10000000149011612;
		World.PlaySoundAt("sounds/environment/mediumsplash", this);
		EntityPos pos = base.Pos;
		double width = SelectionBox.XSize;
		_ = SelectionBox.YSize;
		Entity.SplashParticleProps.BasePos.Set(pos.X - width / 2.0, pos.Y - 0.75, pos.Z - width / 2.0);
		Entity.SplashParticleProps.AddPos.Set(width, 0.75, width);
		Entity.SplashParticleProps.AddVelocity.Set((float)GameMath.Clamp(pos.Motion.X * 30.0, -2.0, 2.0), 1f, (float)GameMath.Clamp(pos.Motion.Z * 30.0, -2.0, 2.0));
		Entity.SplashParticleProps.QuantityMul = 5f;
		World.SpawnParticles(Entity.SplashParticleProps);
	}

	public override void FromBytes(BinaryReader reader, bool isSync)
	{
		base.FromBytes(reader, isSync);
		BaitStack = WatchedAttributes.GetItemstack("baitStack");
		if (Api?.World != null)
		{
			BaitStack?.ResolveBlockOrItem(Api.World);
		}
	}

	public override void ToBytes(BinaryWriter writer, bool forClient)
	{
		WatchedAttributes.SetItemstack("baitStack", BaitStack);
		base.ToBytes(writer, forClient);
	}

	public void OnRopeRipped(ClothSystem cs)
	{
		Die();
	}
}
