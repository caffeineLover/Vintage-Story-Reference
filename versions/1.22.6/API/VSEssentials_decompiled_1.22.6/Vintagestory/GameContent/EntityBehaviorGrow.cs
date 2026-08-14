using System;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class EntityBehaviorGrow : EntityBehavior
{
	private ITreeAttribute growTree;

	private long callbackId;

	[DocumentAsJson("Required", "", false)]
	public AssetLocation[] AdultEntityCodes;

	[DocumentAsJson("Optional", "None", false)]
	public AssetLocation[] FedAdultEntityCodes;

	[DocumentAsJson("Optional", "96", false)]
	public float HoursToGrow { get; set; }

	[DocumentAsJson("Optional", "12", false)]
	public float OrPortionsEatenForGrowing { get; set; }

	protected double SpawnedTotalHours
	{
		get
		{
			return growTree.GetDouble("timeSpawned");
		}
		set
		{
			growTree.SetDouble("timeSpawned", value);
		}
	}

	public EntityBehaviorGrow(Entity entity)
		: base(entity)
	{
	}

	public override void Initialize(EntityProperties properties, JsonObject typeAttributes)
	{
		base.Initialize(properties, typeAttributes);
		AdultEntityCodes = AssetLocation.toLocations(typeAttributes["adultEntityCodes"].AsArray(Array.Empty<string>()));
		FedAdultEntityCodes = AssetLocation.toLocations(typeAttributes["fedAdultEntityCodes"].AsArray(Array.Empty<string>()));
		HoursToGrow = typeAttributes["hoursToGrow"].AsFloat(96f);
		OrPortionsEatenForGrowing = typeAttributes["orPortionsEatenForGrowing"].AsFloat(12f);
		growTree = entity.WatchedAttributes.GetTreeAttribute("grow");
		if (growTree == null && entity.Api.Side == EnumAppSide.Server)
		{
			entity.WatchedAttributes.SetAttribute("grow", growTree = new TreeAttribute());
			SpawnedTotalHours = entity.World.Calendar.TotalHours;
		}
		if (FedAdultEntityCodes.Length != 0)
		{
			double? totalDaysReleased;
			double? num = (totalDaysReleased = entity.Attributes.TryGetDouble("totalDaysReleased"));
			if (num.HasValue)
			{
				double tamingHours = 216f * (OrPortionsEatenForGrowing - (entity.WatchedAttributes.GetTreeAttribute("hunger")?.TryGetFloat("saturation")).GetValueOrDefault());
				double minSpawnedHours = totalDaysReleased.Value * (double)entity.World.Calendar.HoursPerDay + tamingHours - (double)HoursToGrow;
				if (SpawnedTotalHours < minSpawnedHours)
				{
					SpawnedTotalHours = minSpawnedHours;
				}
			}
		}
		callbackId = entity.World.RegisterCallback(CheckGrowth, 3000);
	}

	private void CheckGrowth(float dt)
	{
		callbackId = 0L;
		if (!entity.Alive)
		{
			return;
		}
		ITreeAttribute tree = entity.WatchedAttributes.GetTreeAttribute("hunger");
		bool wasFedToAdulthood = FedAdultEntityCodes.Length != 0 && tree != null && tree.GetFloat("saturation") >= OrPortionsEatenForGrowing;
		if (entity.World.Calendar.TotalHours >= SpawnedTotalHours + (double)HoursToGrow || wasFedToAdulthood)
		{
			AssetLocation[] entityCodes = (wasFedToAdulthood ? FedAdultEntityCodes : AdultEntityCodes);
			if (entityCodes.Length == 0)
			{
				return;
			}
			AssetLocation code = entityCodes[entity.World.Rand.Next(entityCodes.Length)];
			EntityProperties adultType = entity.World.GetEntityType(code);
			if (adultType == null)
			{
				entity.World.Logger.Error("Misconfigured entity. Entity with code '{0}' is configured (via Grow behavior) to grow into '{1}', but no such entity type was registered.", entity.Code, code);
				return;
			}
			Cuboidf collisionBox = adultType.SpawnCollisionBox;
			if (entity.World.CollisionTester.IsColliding(entity.World.BlockAccessor, collisionBox, entity.Pos.XYZ, alsoCheckTouch: false))
			{
				callbackId = entity.World.RegisterCallback(CheckGrowth, 3000);
				return;
			}
			Entity adult = entity.World.ClassRegistry.CreateEntity(adultType);
			adult.Pos.SetFrom(entity.Pos);
			bool keepTextureIndex = entity.Properties.Client != null && entity.Properties.Client.TexturesAlternatesCount > 0 && adultType.Client != null && entity.Properties.Client.TexturesAlternatesCount == adultType.Client.TexturesAlternatesCount;
			adult.Attributes.SetBool("wasFedToAdulthood", wasFedToAdulthood);
			BecomeAdult(adult, keepTextureIndex);
		}
		else
		{
			callbackId = entity.World.RegisterCallback(CheckGrowth, 3000);
			double age = entity.World.Calendar.TotalHours - SpawnedTotalHours;
			if (age >= 0.1 * (double)HoursToGrow)
			{
				float newAge = (float)(age / (double)HoursToGrow - 0.1);
				if (newAge >= 1.01f * growTree.GetFloat("age"))
				{
					growTree.SetFloat("age", newAge);
					entity.WatchedAttributes.MarkPathDirty("grow");
				}
			}
		}
		entity.World.FrameProfiler.Mark("checkgrowth");
	}

	protected virtual void BecomeAdult(Entity adult, bool keepTextureIndex)
	{
		adult.WatchedAttributes.SetInt("generation", entity.WatchedAttributes.GetInt("generation"));
		adult.WatchedAttributes.SetDouble("birthTotalDays", entity.World.Calendar.TotalDays);
		if (keepTextureIndex && entity.WatchedAttributes.HasAttribute("textureIndex"))
		{
			adult.WatchedAttributes.SetAttribute("textureIndex", entity.WatchedAttributes.GetAttribute("textureIndex"));
		}
		entity.Die(EnumDespawnReason.Expire);
		entity.World.SpawnEntity(adult);
	}

	public override void OnEntityDespawn(EntityDespawnData despawn)
	{
		entity.World.UnregisterCallback(callbackId);
	}

	public override string PropertyName()
	{
		return "grow";
	}
}
