using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VSEssentialsMod.Entity.AI.Task;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public abstract class AiTaskBaseTargetable : AiTaskBase, IWorldIntersectionSupplier
{
	[JsonProperty]
	protected ComplexTagCondition<TagSetFast> EntityTags;

	[JsonProperty]
	protected float minTargetWeight;

	[JsonProperty]
	protected float maxTargetWeight = float.MaxValue;

	[JsonProperty]
	protected float minTargetInjury;

	[JsonProperty]
	protected float maxTargetInjury = 1f;

	protected string[] targetEntityCodesBeginsWith = Array.Empty<string>();

	protected string[] targetEntityCodesExact;

	protected AssetLocation[]? skipEntityCodes;

	protected string targetEntityFirstLetters = "";

	protected EnumCreatureHostility creatureHostility;

	[JsonProperty]
	protected bool friendlyTarget;

	public Entity? targetEntity;

	protected Entity? attackedByEntity;

	protected long attackedByEntityMs;

	[JsonProperty]
	protected bool retaliateAttacks = true;

	[JsonProperty]
	public string? triggerEmotionState;

	[JsonProperty]
	protected float tamingGenerations = 10f;

	protected EntityPartitioning partitionUtil;

	protected EntityBehaviorControlledPhysics? bhPhysics;

	[JsonProperty]
	public bool TargetOnlyInteractableEntities = true;

	protected BlockSelection? blockSel = new BlockSelection();

	protected EntitySelection? entitySel = new EntitySelection();

	protected readonly Vec3d rayTraceFrom = new Vec3d();

	protected readonly Vec3d rayTraceTo = new Vec3d();

	protected readonly Vec3d tmpPos = new Vec3d();

	private Vec3d tmpVec = new Vec3d();

	protected Vec3d collTmpVec = new Vec3d();

	protected float stepHeight;

	public virtual bool AggressiveTargeting => true;

	public Entity? TargetEntity => targetEntity;

	protected bool noEntityCodes
	{
		get
		{
			if (targetEntityCodesExact.Length == 0)
			{
				return targetEntityCodesBeginsWith.Length == 0;
			}
			return false;
		}
	}

	protected bool RecentlyAttacked => entity.World.ElapsedMilliseconds - attackedByEntityMs < 30000;

	public Vec3i MapSize => entity.World.BlockAccessor.MapSize;

	public IBlockAccessor blockAccessor => entity.World.BlockAccessor;

	protected AiTaskBaseTargetable(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig)
		: base(entity, taskConfig, aiConfig)
	{
		partitionUtil = entity.Api.ModLoader.GetModSystem<EntityPartitioning>();
		creatureHostility = entity.World.Config.GetString("creatureHostility") switch
		{
			"aggressive" => EnumCreatureHostility.Aggressive, 
			"passive" => EnumCreatureHostility.Passive, 
			"off" => EnumCreatureHostility.NeverHostile, 
			_ => EnumCreatureHostility.Aggressive, 
		};
		skipEntityCodes = taskConfig["skipEntityCodes"].AsArray<string>()?.Select((string str) => AssetLocation.Create(str, entity.Code.Domain)).ToArray();
		string[] codes = taskConfig["entityCodes"].AsArray(new string[1] { "player" });
		for (int i = 0; i < codes.Length; i++)
		{
			if (codes[i] == null)
			{
				throw new ArgumentNullException(string.Concat(new string[5]
				{
					GetType().ToString(),
					" on entity ",
					entity.Code,
					" was initialized with a null target code at index ",
					i.ToString()
				}));
			}
		}
		InitializeTargetCodes(codes, ref targetEntityCodesExact, ref targetEntityCodesBeginsWith, ref targetEntityFirstLetters);
		JsonObject tags = taskConfig["entityTags"];
		if (tags.Exists)
		{
			EntityTags = GenericComplexConditionConverter.EntityConverter.ReadJson(tags.Token);
		}
	}

	public static void InitializeTargetCodes(string[] codes, ref string[]? targetEntityCodesExact, ref string[] targetEntityCodesBeginsWith, ref string targetEntityFirstLetters)
	{
		List<string> targetEntityCodesList = new List<string>();
		List<string> beginswith = new List<string>();
		foreach (string code in codes)
		{
			if (code.EndsWith('*'))
			{
				beginswith.Add(code.Substring(0, code.Length - 1));
			}
			else
			{
				targetEntityCodesList.Add(code);
			}
		}
		targetEntityCodesBeginsWith = beginswith.ToArray();
		targetEntityCodesExact = new string[targetEntityCodesList.Count];
		int j = 0;
		foreach (string code2 in targetEntityCodesList)
		{
			if (code2.Length != 0)
			{
				targetEntityCodesExact[j++] = code2;
				char c = code2[0];
				if (targetEntityFirstLetters.IndexOf(c) < 0)
				{
					targetEntityFirstLetters += c;
				}
			}
		}
		string[] array = targetEntityCodesBeginsWith;
		foreach (string code3 in array)
		{
			if (code3.Length == 0)
			{
				targetEntityFirstLetters = "";
				break;
			}
			char c2 = code3[0];
			if (targetEntityFirstLetters.IndexOf(c2) < 0)
			{
				targetEntityFirstLetters += c2;
			}
		}
	}

	public override void AfterInitialize()
	{
		bhPhysics = entity.GetBehavior<EntityBehaviorControlledPhysics>();
	}

	public override void StartExecute()
	{
		stepHeight = bhPhysics?.StepHeight ?? 0.6f;
		base.StartExecute();
		if (triggerEmotionState != null)
		{
			entity.GetBehavior<EntityBehaviorEmotionStates>()?.TryTriggerState(triggerEmotionState, 1.0, targetEntity?.EntityId ?? 0);
		}
		EntityBehaviorControlledPhysics physics = entity.GetBehavior<EntityBehaviorControlledPhysics>();
		if (physics != null)
		{
			stepHeight = physics.StepHeight;
		}
	}

	protected virtual bool CheckTargetWeight(float weight)
	{
		float weightFraction = ((entity.Properties.Weight > 0f) ? (weight / entity.Properties.Weight) : float.MaxValue);
		if (minTargetWeight > 0f && weightFraction < minTargetWeight)
		{
			return false;
		}
		if (maxTargetWeight > 0f && weightFraction > maxTargetWeight)
		{
			return false;
		}
		return true;
	}

	protected virtual bool CheckTargetTags(TagSetFast targetTags)
	{
		return EntityTags.Matches(in targetTags);
	}

	[Obsolete("Use IsTargetableEntity instead")]
	public virtual bool IsTargetableEntityWithTags(Entity e, float range)
	{
		return IsTargetableEntity(e, range);
	}

	public virtual bool IsTargetableEntityNoTagsNoAll(Entity e, float range)
	{
		if (IsTargetEntity(e.Code.Path))
		{
			if (e.Alive)
			{
				return CanSense(e, range);
			}
			return false;
		}
		return false;
	}

	public virtual bool IsTargetableEntityNoTagsAll(Entity e, float range)
	{
		if (e.Alive)
		{
			return CanSense(e, range);
		}
		return false;
	}

	public virtual bool IsTargetableEntity(Entity e, float range)
	{
		if (!e.Alive)
		{
			return false;
		}
		if (minTargetInjury > 0f || maxTargetInjury < 1f)
		{
			EntityBehaviorHealth healthBehavior = e.GetBehavior<EntityBehaviorHealth>();
			float injury = ((healthBehavior == null) ? 0f : (1f - healthBehavior.Health / healthBehavior.MaxHealth));
			if (injury < minTargetInjury || injury > maxTargetInjury)
			{
				return false;
			}
		}
		if (!EntityTags.IsEmpty && CheckTargetTags(e.Tags) && CheckTargetWeight(e.Properties.Weight))
		{
			return CanSense(e, range);
		}
		if (targetEntityFirstLetters.Length == 0 || IsTargetEntity(e.Code.Path))
		{
			return CanSense(e, range);
		}
		return false;
	}

	protected bool IsTargetEntity(string testPath)
	{
		if (targetEntityFirstLetters.IndexOf(testPath[0]) < 0)
		{
			return false;
		}
		string[] targetEntityCodes = targetEntityCodesExact;
		for (int i = 0; i < targetEntityCodes.Length; i++)
		{
			if (testPath == targetEntityCodes[i])
			{
				return true;
			}
		}
		targetEntityCodes = targetEntityCodesBeginsWith;
		for (int j = 0; j < targetEntityCodes.Length; j++)
		{
			if (testPath.StartsWithFast(targetEntityCodes[j]))
			{
				return true;
			}
		}
		return false;
	}

	public virtual bool CanSense(Entity e, double range)
	{
		if (e.EntityId == entity.EntityId || (TargetOnlyInteractableEntities && !e.IsInteractable))
		{
			return false;
		}
		if (e is EntityPlayer eplr)
		{
			return CanSensePlayer(eplr, range);
		}
		if (skipEntityCodes != null)
		{
			for (int i = 0; i < skipEntityCodes.Length; i++)
			{
				if (WildcardUtil.Match(skipEntityCodes[i], e.Code))
				{
					return false;
				}
			}
		}
		return true;
	}

	public virtual bool CanSensePlayer(EntityPlayer eplr, double range)
	{
		if (!friendlyTarget && AggressiveTargeting)
		{
			if (creatureHostility == EnumCreatureHostility.NeverHostile)
			{
				return false;
			}
			if (creatureHostility == EnumCreatureHostility.Passive && (bhEmo == null || (!IsInEmotionState("aggressiveondamage") && !IsInEmotionState("aggressivearoundentities"))))
			{
				return false;
			}
		}
		float rangeMul = eplr.Stats.GetBlended("animalSeekingRange");
		IPlayer player = eplr.Player;
		if (eplr.Controls.Sneak && eplr.OnGround)
		{
			rangeMul *= 0.6f;
		}
		if ((rangeMul == 1f || entity.Pos.DistanceTo(eplr.Pos) < range * (double)rangeMul) && targetablePlayerMode(player) && entity.Pos.Dimension == eplr.Pos.Dimension)
		{
			return true;
		}
		return false;
	}

	protected virtual bool targetablePlayerMode(IPlayer? player)
	{
		if (player != null)
		{
			if (player.WorldData.CurrentGameMode != EnumGameMode.Creative && player.WorldData.CurrentGameMode != EnumGameMode.Spectator)
			{
				IServerPlayer obj = player as IServerPlayer;
				if (obj == null)
				{
					return false;
				}
				return obj.ConnectionState == EnumClientState.Playing;
			}
			return false;
		}
		return true;
	}

	protected virtual bool hasDirectContact(Entity targetEntity, float minDist, float minVerDist)
	{
		if (targetEntity.Pos.Dimension != entity.Pos.Dimension)
		{
			return false;
		}
		Cuboidd cuboidd = targetEntity.SelectionBox.ToDouble().Translate(targetEntity.Pos.X, targetEntity.Pos.Y, targetEntity.Pos.Z);
		tmpPos.Set(entity.Pos).Add(0.0, entity.SelectionBox.Y2 / 2f, 0.0).Ahead(entity.SelectionBox.XSize / 2f, 0f, entity.Pos.Yaw);
		double dist = cuboidd.ShortestDistanceFrom(tmpPos);
		double vertDist = Math.Abs(cuboidd.ShortestVerticalDistanceFrom(tmpPos.Y));
		if (dist >= (double)minDist || vertDist >= (double)minVerDist)
		{
			return false;
		}
		rayTraceFrom.Set(entity.Pos);
		rayTraceFrom.Y += 1.0 / 32.0;
		rayTraceTo.Set(targetEntity.Pos);
		rayTraceTo.Y += 1.0 / 32.0;
		bool directContact = false;
		entity.World.RayTraceForSelection(this, rayTraceFrom, rayTraceTo, ref blockSel, ref entitySel);
		directContact = blockSel == null;
		if (!directContact)
		{
			rayTraceFrom.Y += entity.SelectionBox.Y2 * 7f / 16f;
			rayTraceTo.Y += targetEntity.SelectionBox.Y2 * 7f / 16f;
			entity.World.RayTraceForSelection(this, rayTraceFrom, rayTraceTo, ref blockSel, ref entitySel);
			directContact = blockSel == null;
		}
		if (!directContact)
		{
			rayTraceFrom.Y += entity.SelectionBox.Y2 * 7f / 16f;
			rayTraceTo.Y += targetEntity.SelectionBox.Y2 * 7f / 16f;
			entity.World.RayTraceForSelection(this, rayTraceFrom, rayTraceTo, ref blockSel, ref entitySel);
			directContact = blockSel == null;
		}
		if (!directContact)
		{
			return false;
		}
		return true;
	}

	protected void updateTargetPosFleeMode(Vec3d targetPos, float yaw)
	{
		tmpVec = tmpVec.Set(entity.Pos.X, entity.Pos.Y, entity.Pos.Z);
		tmpVec.Ahead(0.9, 0f, yaw);
		if (traversable(tmpVec))
		{
			targetPos.Set(entity.Pos.X, entity.Pos.Y, entity.Pos.Z).Ahead(10.0, 0f, yaw);
			return;
		}
		tmpVec = tmpVec.Set(entity.Pos.X, entity.Pos.Y, entity.Pos.Z);
		tmpVec.Ahead(0.9, 0f, yaw - (float)Math.PI / 2f);
		if (traversable(tmpVec))
		{
			targetPos.Set(entity.Pos.X, entity.Pos.Y, entity.Pos.Z).Ahead(10.0, 0f, yaw - (float)Math.PI / 2f);
			return;
		}
		tmpVec = tmpVec.Set(entity.Pos.X, entity.Pos.Y, entity.Pos.Z);
		tmpVec.Ahead(0.9, 0f, yaw + (float)Math.PI / 2f);
		if (traversable(tmpVec))
		{
			targetPos.Set(entity.Pos.X, entity.Pos.Y, entity.Pos.Z).Ahead(10.0, 0f, yaw + (float)Math.PI / 2f);
			return;
		}
		tmpVec = tmpVec.Set(entity.Pos.X, entity.Pos.Y, entity.Pos.Z);
		tmpVec.Ahead(0.9, 0f, yaw + (float)Math.PI);
		targetPos.Set(entity.Pos.X, entity.Pos.Y, entity.Pos.Z).Ahead(10.0, 0f, yaw + (float)Math.PI);
	}

	protected bool traversable(Vec3d pos)
	{
		if (world.CollisionTester.IsColliding(world.BlockAccessor, entity.SelectionBox, pos, alsoCheckTouch: false))
		{
			return !world.CollisionTester.IsColliding(world.BlockAccessor, entity.SelectionBox, collTmpVec.Set(pos).Add(0.0, Math.Min(1f, stepHeight), 0.0), alsoCheckTouch: false);
		}
		return true;
	}

	public Block GetBlock(BlockPos pos)
	{
		return entity.World.BlockAccessor.GetBlock(pos);
	}

	public Cuboidf[] GetBlockIntersectionBoxes(BlockPos pos)
	{
		return entity.World.BlockAccessor.GetBlock(pos).GetCollisionBoxes(entity.World.BlockAccessor, pos);
	}

	public bool IsValidPos(BlockPos pos)
	{
		return entity.World.BlockAccessor.IsValidPos(pos);
	}

	public Entity[] GetEntitiesAround(Vec3d position, float horRange, float vertRange, ActionConsumable<Entity>? matches = null)
	{
		return Array.Empty<Entity>();
	}

	public Entity? GetGuardedEntity()
	{
		string uid = entity.WatchedAttributes.GetString("guardedPlayerUid");
		if (uid != null)
		{
			return entity.World.PlayerByUid(uid)?.Entity;
		}
		long id = entity.WatchedAttributes.GetLong("guardedEntityId", 0L);
		return entity.World.GetEntityById(id);
	}

	public int GetOwnGeneration()
	{
		int generation = entity.WatchedAttributes.GetInt("generation");
		JsonObject attributes = entity.Properties.Attributes;
		if (attributes != null && attributes.IsTrue("tamed"))
		{
			generation += 10;
		}
		return generation;
	}

	protected bool isNonAttackingPlayer(Entity e)
	{
		if (attackedByEntity == null || (attackedByEntity != null && attackedByEntity.EntityId != e.EntityId))
		{
			return e is EntityPlayer;
		}
		return false;
	}

	public override void OnEntityHurt(DamageSource source, float damage)
	{
		attackedByEntity = source.GetCauseEntity();
		attackedByEntityMs = entity.World.ElapsedMilliseconds;
		base.OnEntityHurt(source, damage);
	}

	public void ClearAttacker()
	{
		attackedByEntity = null;
		attackedByEntityMs = -9999L;
	}

	protected virtual bool ShouldRetaliateForRange(float range)
	{
		if (retaliateAttacks && attackedByEntity != null && attackedByEntity.Alive && attackedByEntity.IsInteractable && CanSense(attackedByEntity, range))
		{
			return !IgnoreDamageFrom(attackedByEntity);
		}
		return false;
	}

	public virtual bool IgnoreDamageFrom(Entity attacker)
	{
		return entity.ToleratesDamageFrom(attacker);
	}
}
