using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

[DocumentAsJson]
[AddDocumentationProperty("minGenerationToAllowHealing", "If this is an animal, specifies the minimum generation required to be healed by player. Ignored if -1", "System.Int32", "Optional", "-1", true)]
public class EntityBehaviorHealth : EntityBehavior
{
	public const double FallDamageYMotionThreshold = -0.19;

	public const float FallDamageFallenDistanceThreshold = 3.5f;

	protected Dictionary<string, float>? maxHealthModifiers;

	[DocumentAsJson("Optional", "hurt", false)]
	protected string? HurtAnimationCode = "hurt";

	protected string HurtEntitySoundCode = "hurt";

	protected string SmallHurtEntitySoundCode = "hurt";

	protected float timeSinceLastDoTTickSec;

	[DocumentAsJson("Optional", "0.5", false)]
	protected float timeBetweenDoTTicksSec = 0.5f;

	private float secondsSinceLastUpdate;

	[DocumentAsJson("Optional", "MaxHealth", false)]
	public float Health
	{
		get
		{
			return healthTree.GetFloat("currenthealth");
		}
		set
		{
			healthTree.SetFloat("currenthealth", value);
			entity.WatchedAttributes.MarkPathDirty("health");
		}
	}

	public float? FutureHealth
	{
		get
		{
			return healthTree.GetFloat("futureHealth");
		}
		set
		{
			if (!value.HasValue)
			{
				healthTree.RemoveAttribute("futureHealth");
			}
			else
			{
				healthTree.SetFloat("futureHealth", value.Value);
			}
			entity.WatchedAttributes.MarkPathDirty("health");
		}
	}

	public float PreviousHealth
	{
		get
		{
			return healthTree.GetFloat("previousHealthValue");
		}
		set
		{
			healthTree.SetFloat("previousHealthValue", value);
			entity.WatchedAttributes.MarkPathDirty("health");
		}
	}

	public float HealthChangeRate
	{
		get
		{
			return healthTree.GetFloat("healthChangeRate");
		}
		set
		{
			healthTree.SetFloat("healthChangeRate", value);
			entity.WatchedAttributes.MarkPathDirty("health");
		}
	}

	public float BaseMaxHealth
	{
		get
		{
			return healthTree.GetFloat("basemaxhealth");
		}
		set
		{
			healthTree.SetFloat("basemaxhealth", value);
			entity.WatchedAttributes.MarkPathDirty("health");
		}
	}

	[DocumentAsJson("Recommended", "20", false)]
	public float MaxHealth
	{
		get
		{
			return healthTree.GetFloat("maxhealth");
		}
		set
		{
			healthTree.SetFloat("maxhealth", value);
			entity.WatchedAttributes.MarkPathDirty("health");
		}
	}

	public List<DamageOverTimeEffect> ActiveDoTEffects { get; } = new List<DamageOverTimeEffect>();

	[Obsolete("Please call SetMaxHealthModifiers() instead of writing to it directly.")]
	public Dictionary<string, float>? MaxHealthModifiers { get; set; }

	protected virtual float HealthUpdateCooldownSec => 1f;

	[DocumentAsJson("Optional", "false", false)]
	protected virtual bool ReceiveHailDamage { get; set; }

	[DocumentAsJson("Optional", "0.75", false)]
	protected virtual float AutoRegenSaturationThreshold { get; set; } = 0.75f;

	[DocumentAsJson("Optional", "150", false)]
	protected virtual float SaturationPerHealthPoint { get; set; } = 75f;

	private ITreeAttribute healthTree => entity.WatchedAttributes.GetTreeAttribute("health");

	public event OnDamagedDelegate onDamaged = (float dmg, DamageSource dmgSource) => dmg;

	public EntityBehaviorHealth(Entity entity)
		: base(entity)
	{
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		entity.WatchedAttributes.SetFloat("onHurt", 0f);
		int onHurtCounter = entity.WatchedAttributes.GetInt("onHurtCounter");
		entity.WatchedAttributes.RegisterModifiedListener("onHurt", delegate
		{
			float num = entity.WatchedAttributes.GetFloat("onHurt");
			if (num != 0f)
			{
				int num2 = entity.WatchedAttributes.GetInt("onHurtCounter");
				if (num2 != onHurtCounter)
				{
					onHurtCounter = num2;
					if (entity.Attributes.GetInt("dmgkb") == 0)
					{
						entity.Attributes.SetInt("dmgkb", 1);
					}
					if ((double)num > 0.05)
					{
						entity.SetActivityRunning("invulnerable", 500);
						if (entity.World.Side == EnumAppSide.Client)
						{
							entity.OnHurt(null, entity.WatchedAttributes.GetFloat("onHurt"));
						}
					}
				}
			}
		});
		ITreeAttribute entityHealthTree = entity.WatchedAttributes.GetTreeAttribute("health");
		if (entityHealthTree == null)
		{
			entity.WatchedAttributes.SetAttribute("health", entityHealthTree = new TreeAttribute());
			BaseMaxHealth = attributes["maxhealth"].AsFloat(20f);
			Health = attributes["currenthealth"].AsFloat(BaseMaxHealth);
			PreviousHealth = Health;
			MarkDirty();
			return;
		}
		if (entityHealthTree.GetFloat("basemaxhealth") == 0f)
		{
			BaseMaxHealth = attributes["maxhealth"].AsFloat(20f);
			MarkDirty();
		}
		secondsSinceLastUpdate = (float)entity.World.Rand.NextDouble();
		ReceiveHailDamage = attributes["receiveHailDamage"].AsBool(entity is EntityPlayer);
		AutoRegenSaturationThreshold = attributes["autoRegenSaturationThreshold"].AsFloat(AutoRegenSaturationThreshold);
		SaturationPerHealthPoint = attributes["saturationPerHealthPoint"].AsFloat(SaturationPerHealthPoint);
		HurtAnimationCode = attributes["hurtAnimationCode"].AsString(HurtAnimationCode);
		HurtEntitySoundCode = attributes["hurtSoundCode"].AsString(HurtEntitySoundCode);
		SmallHurtEntitySoundCode = attributes["smallHurtSoundCode"].AsString(SmallHurtEntitySoundCode);
		timeBetweenDoTTicksSec = attributes["timeBetweenTicksSec"].AsFloat(0.5f);
		TimeSpan currentTime = TimeSpan.FromMilliseconds(entity.World.ElapsedMilliseconds);
		byte[] data = entity.WatchedAttributes.GetBytes("damageovertime-activeeffects");
		if (data == null)
		{
			return;
		}
		ActiveDoTEffectsFromBytes(data);
		foreach (DamageOverTimeEffect activeDoTEffect in ActiveDoTEffects)
		{
			activeDoTEffect.PreviousTickTime = currentTime;
		}
	}

	public override void OnGameTick(float deltaTime)
	{
		if (entity.World.Side == EnumAppSide.Client)
		{
			return;
		}
		ProcessDoTEffects(deltaTime);
		if (entity.Alive)
		{
			DamageIfFallingIntoVoid();
			secondsSinceLastUpdate += deltaTime;
			if (!(secondsSinceLastUpdate < HealthUpdateCooldownSec))
			{
				ApplyRegenAndHunger();
				ApplyHailDamage();
				secondsSinceLastUpdate = 0f;
			}
		}
	}

	public override void OnEntityDeath(DamageSource damageSourceForDeath)
	{
		base.OnEntityDeath(damageSourceForDeath);
		Health = 0f;
	}

	public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage)
	{
		if (entity.World.Side == EnumAppSide.Client || TurnIntoDoTEffect(damageSource, damage))
		{
			return;
		}
		float damageBeforeDelegatesApplied = damage;
		ApplyOnDamageDelegates(damageSource, ref damage);
		if (damageSource.Type == EnumDamageType.Heal)
		{
			ApplyHealing(damageSource, damage);
			entity.OnHurt(damageSource, damage);
			UpdateMaxHealth();
		}
		else
		{
			if (!entity.Alive || damage <= 0f)
			{
				return;
			}
			LogPlayerToPlayerDamage(damageSource, damage, damageBeforeDelegatesApplied);
			PreviousHealth = Health;
			Health -= damage;
			entity.OnHurt(damageSource, damage);
			UpdateMaxHealth();
			if (Health <= 0f)
			{
				Health = 0f;
				entity.Die(EnumDespawnReason.Death, damageSource);
				return;
			}
			if (damage > 1f && HurtAnimationCode != null)
			{
				entity.AnimManager.StartAnimation(HurtAnimationCode);
			}
			bool playLoudly = damage > 2f && damageSource.Type != EnumDamageType.Poison && damageSource.Type != EnumDamageType.Frost;
			entity.PlayEntitySound(playLoudly ? HurtEntitySoundCode : SmallHurtEntitySoundCode);
		}
	}

	public override void OnFallToGround(Vec3d lastTerrainContact, double withYMotion)
	{
		if (!entity.Properties.FallDamage)
		{
			return;
		}
		bool gliding = (entity as EntityAgent)?.ServerControls.Gliding ?? false;
		double yDistance = Math.Abs(lastTerrainContact.Y - entity.Pos.Y);
		double fallDamageFallenDistanceThreshold = 3.5f * entity.Stats.GetBlended("fallDamageThreshold");
		double fallDamageYMotionThreshold = -0.19 * (double)entity.Stats.GetBlended("fallDamageThreshold");
		if (yDistance < fallDamageFallenDistanceThreshold)
		{
			return;
		}
		if (gliding)
		{
			yDistance = Math.Min(yDistance / 2.0, Math.Min(14.0, yDistance));
			withYMotion /= 2.0;
			if ((double)entity.Pos.Pitch < 3.9269909262657166)
			{
				yDistance = 0.0;
			}
		}
		if (!(withYMotion > fallDamageYMotionThreshold))
		{
			yDistance *= (double)entity.Properties.FallDamageMultiplier;
			double fallDamage = Math.Max(0.0, yDistance - fallDamageFallenDistanceThreshold);
			double expectedYMotion = -0.04100000113248825 * Math.Pow(fallDamage, 0.75) - 0.2199999988079071;
			double yMotionLoss = Math.Max(0.0, 0.0 - expectedYMotion + withYMotion);
			fallDamage -= 20.0 * yMotionLoss;
			if (!(fallDamage <= 0.0))
			{
				fallDamage *= (double)entity.Stats.GetBlended("fallDamageFactor");
				entity.ReceiveDamage(new DamageSource
				{
					Source = EnumDamageSource.Fall,
					Type = EnumDamageType.Gravity,
					IgnoreInvFrames = true
				}, (float)fallDamage);
			}
		}
	}

	public override void GetInfoText(StringBuilder infotext)
	{
		ICoreClientAPI obj = entity.Api as ICoreClientAPI;
		if (obj != null && obj.World.Player?.WorldData?.CurrentGameMode == EnumGameMode.Creative)
		{
			infotext.AppendLine(Lang.Get("Health: {0}/{1}", Health, MaxHealth));
		}
	}

	public override WorldInteraction[]? GetInteractionHelp(IClientWorldAccessor world, EntitySelection es, IClientPlayer player, ref EnumHandling handled)
	{
		if (IsHealable(player.Entity))
		{
			ICanHealCreature canHealCreature = player.InventoryManager.ActiveHotbarSlot?.Itemstack?.Collectible?.GetCollectibleInterface<ICanHealCreature>();
			if (canHealCreature != null && canHealCreature.CanHeal(entity))
			{
				return canHealCreature.GetHealInteractionHelp(world, es, player).Append<WorldInteraction>(base.GetInteractionHelp(world, es, player, ref handled));
			}
		}
		return base.GetInteractionHelp(world, es, player, ref handled);
	}

	public override string PropertyName()
	{
		return "health";
	}

	public override void ToBytes(bool forClient)
	{
		base.ToBytes(forClient);
		entity.WatchedAttributes.SetBytes("damageovertime-activeeffects", ActiveDoTEffectsToBytes());
	}

	public void SetMaxHealthModifiers(string key, float value)
	{
		bool dirty = true;
		float oldValue;
		if (maxHealthModifiers == null)
		{
			maxHealthModifiers = new Dictionary<string, float>();
			if (value == 0f)
			{
				dirty = false;
			}
		}
		else if (maxHealthModifiers.TryGetValue(key, out oldValue) && oldValue == value)
		{
			dirty = false;
		}
		maxHealthModifiers[key] = value;
		if (dirty)
		{
			MarkDirty();
		}
	}

	public void MarkDirty()
	{
		UpdateMaxHealth();
		entity.WatchedAttributes.MarkPathDirty("health");
	}

	public void UpdateMaxHealth()
	{
		float totalMaxHealth = BaseMaxHealth;
		Dictionary<string, float> MaxHealthModifiers = maxHealthModifiers;
		if (MaxHealthModifiers != null)
		{
			foreach (KeyValuePair<string, float> item in MaxHealthModifiers)
			{
				totalMaxHealth += item.Value;
			}
		}
		totalMaxHealth += entity.Stats.GetBlended("maxhealthExtraPoints") - 1f;
		bool num = Health >= MaxHealth;
		MaxHealth = totalMaxHealth;
		if (num)
		{
			Health = MaxHealth;
		}
	}

	public static ItemStack[] GetAllHealingItems(ICoreAPI api)
	{
		return ObjectCacheUtil.GetOrCreate(api, "poulticeStacks", delegate
		{
			List<ItemStack> list = new List<ItemStack>();
			foreach (CollectibleObject current in api.World.Collectibles)
			{
				if (current.GetCollectibleInterface<ICanHealCreature>() != null)
				{
					list.Add(new ItemStack(current));
				}
			}
			return list.ToArray();
		});
	}

	public bool IsHealable(EntityAgent byEntity, ItemSlot? slot = null)
	{
		if (Health >= MaxHealth)
		{
			return false;
		}
		if (entity is EntityPlayer)
		{
			return true;
		}
		int minGenerationToAllowHealing = entity.Properties.Attributes?["minGenerationToAllowHealing"].AsInt(-1) ?? (-1);
		if (entity.Alive)
		{
			if (minGenerationToAllowHealing >= 0)
			{
				return minGenerationToAllowHealing >= entity.WatchedAttributes.GetInt("generation");
			}
			return false;
		}
		return false;
	}

	public virtual int ApplyDoTEffect(EnumDamageSource damageSource, EnumDamageType damageType, int damageTier, float totalDamage, TimeSpan totalTime, int ticksNumber, EnumDamageOverTimeEffectType effectType = EnumDamageOverTimeEffectType.Unknown)
	{
		return ApplyDoTEffect(damageSource, damageType, damageTier, totalDamage, totalTime, ticksNumber, (int)effectType);
	}

	public virtual int ApplyDoTEffect(EnumDamageSource damageSource, EnumDamageType damageType, int damageTier, float totalDamage, TimeSpan totalTime, int ticksNumber, int effectType = 0)
	{
		DamageOverTimeEffect effect = new DamageOverTimeEffect
		{
			DamageSource = damageSource,
			DamageType = damageType,
			DamageTier = damageTier,
			Damage = totalDamage / (float)ticksNumber,
			TickDuration = totalTime / ticksNumber,
			PreviousTickTime = TimeSpan.FromMilliseconds(entity.World.ElapsedMilliseconds),
			TicksLeft = ticksNumber,
			EffectType = effectType
		};
		ActiveDoTEffects.Add(effect);
		return ActiveDoTEffects.Count - 1;
	}

	public virtual void StopDoTEffect(int effectType, int amount = int.MaxValue)
	{
		int count = 0;
		foreach (DamageOverTimeEffect item in ActiveDoTEffects.Where((DamageOverTimeEffect effect) => effect.EffectType == effectType))
		{
			item.TicksLeft = 0;
			count++;
			if (count >= amount)
			{
				break;
			}
		}
	}

	protected virtual void DamageIfFallingIntoVoid()
	{
		if (entity.Pos.Y < -30.0)
		{
			entity.ReceiveDamage(new DamageSource
			{
				Source = EnumDamageSource.Void,
				Type = EnumDamageType.Gravity
			}, 4f);
		}
	}

	protected virtual void ApplyHailDamage()
	{
		if (!ReceiveHailDamage)
		{
			return;
		}
		int rainy = entity.World.BlockAccessor.GetRainMapHeightAt((int)entity.Pos.X, (int)entity.Pos.Z);
		if (entity.Pos.Y >= (double)rainy)
		{
			PrecipitationState state = entity.Api.ModLoader.GetModSystem<WeatherSystemBase>().GetPrecipitationState(entity.Pos.XYZ);
			if (state != null && state.ParticleSize >= 0.5 && state.Type == EnumPrecipitationType.Hail && entity.World.Rand.NextDouble() < state.Level / 2.0)
			{
				entity.ReceiveDamage(new DamageSource
				{
					Source = EnumDamageSource.Weather,
					Type = EnumDamageType.BluntAttack
				}, (float)state.ParticleSize / 15f);
			}
		}
	}

	protected virtual void ApplyRegenAndHunger()
	{
		float health = Health;
		float maxHealth = MaxHealth;
		if (health >= maxHealth)
		{
			return;
		}
		float healthRegenSpeed = ((entity is EntityPlayer) ? entity.Api.World.Config.GetString("playerHealthRegenSpeed", "1").ToFloat() : entity.WatchedAttributes.GetFloat("regenSpeed", 1f));
		float healthRegenPerGameSecond = 0.000333333f * healthRegenSpeed;
		float multiplierPerGameSec = secondsSinceLastUpdate * entity.Api.World.Calendar.SpeedOfTime * entity.Api.World.Calendar.CalendarSpeedMul;
		if (entity is EntityPlayer player)
		{
			EntityBehaviorHunger hungerBehavior = entity.GetBehavior<EntityBehaviorHunger>();
			if (hungerBehavior != null && player.Player.WorldData.CurrentGameMode != EnumGameMode.Creative)
			{
				healthRegenPerGameSecond = GameMath.Clamp(healthRegenPerGameSecond * hungerBehavior.Saturation / hungerBehavior.MaxSaturation * 1f / AutoRegenSaturationThreshold, 0f, healthRegenPerGameSecond);
				hungerBehavior.ConsumeSaturation(SaturationPerHealthPoint * multiplierPerGameSec * healthRegenPerGameSecond);
			}
		}
		Health = Math.Min(health + multiplierPerGameSec * healthRegenPerGameSecond, maxHealth);
	}

	protected virtual void ApplyOnDamageDelegates(DamageSource damageSource, ref float damage)
	{
		if (this.onDamaged == null)
		{
			return;
		}
		foreach (OnDamagedDelegate dele in this.onDamaged.GetInvocationList().OfType<OnDamagedDelegate>())
		{
			damage = dele(damage, damageSource);
		}
	}

	protected virtual void ApplyHealing(DamageSource damageSource, float damage)
	{
		if (damageSource.Source != EnumDamageSource.Revive)
		{
			Health = Math.Min(Health + damage, MaxHealth);
			return;
		}
		damage = Math.Min(damage, MaxHealth);
		Health = damage;
	}

	protected virtual void LogPlayerToPlayerDamage(DamageSource damageSource, float damage, float damageBeforeDelegatesApplied)
	{
		if (entity is EntityPlayer player && damageSource.GetCauseEntity() is EntityPlayer otherPlayer)
		{
			string weapon = ((damageSource.SourceEntity == otherPlayer) ? (otherPlayer.Player.InventoryManager.ActiveHotbarSlot.Itemstack?.Collectible.Code.ToString() ?? "hands") : damageSource.SourceEntity.Code.ToString());
			entity.Api.Logger.Audit($"{player.Player.PlayerName} at {entity.Pos.AsBlockPos} got {damage}/{damageBeforeDelegatesApplied} damage {damageSource.Type.ToString().ToLowerInvariant()} {weapon} by {otherPlayer.GetName()}");
		}
	}

	protected virtual void ProcessDoTEffects(float dt)
	{
		if (!entity.Alive)
		{
			ActiveDoTEffects.Clear();
			return;
		}
		if (timeSinceLastDoTTickSec < timeBetweenDoTTicksSec)
		{
			timeSinceLastDoTTickSec += dt;
			return;
		}
		timeSinceLastDoTTickSec = 0f;
		TimeSpan currentTime = TimeSpan.FromMilliseconds(entity.World.ElapsedMilliseconds);
		float totalHealthFutureGain = 0f;
		float healthVelocity = 0f;
		for (int index = ActiveDoTEffects.Count - 1; index >= 0; index--)
		{
			DamageOverTimeEffect effect = ActiveDoTEffects[index];
			if (currentTime - effect.PreviousTickTime >= effect.TickDuration)
			{
				entity.ReceiveDamage(new DamageSource
				{
					Source = effect.DamageSource,
					Type = effect.DamageType,
					DamageTier = effect.DamageTier
				}, effect.Damage);
				effect.TicksLeft--;
				if (effect.TicksLeft <= 0)
				{
					ActiveDoTEffects.RemoveAt(index);
				}
				else
				{
					effect.PreviousTickTime = currentTime;
				}
			}
			if (effect.DamageType == EnumDamageType.Heal)
			{
				totalHealthFutureGain += effect.Damage * (float)effect.TicksLeft;
				healthVelocity += effect.Damage * (float)effect.TicksLeft;
			}
			else
			{
				healthVelocity -= effect.Damage * (float)effect.TicksLeft;
			}
		}
		if ((double)Math.Abs(totalHealthFutureGain) > 0.1)
		{
			FutureHealth = Health + totalHealthFutureGain;
		}
		else
		{
			FutureHealth = null;
		}
		HealthChangeRate = healthVelocity;
	}

	protected virtual byte[] ActiveDoTEffectsToBytes()
	{
		MemoryStream stream = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(stream);
		writer.Write(ActiveDoTEffects.Count);
		foreach (DamageOverTimeEffect activeDoTEffect in ActiveDoTEffects)
		{
			activeDoTEffect.ToBytes(writer);
		}
		return stream.ToArray();
	}

	protected virtual void ActiveDoTEffectsFromBytes(byte[] bytes)
	{
		BinaryReader reader = new BinaryReader(new MemoryStream(bytes));
		ActiveDoTEffects.Clear();
		int count = reader.ReadInt32();
		for (int i = 0; i < count; i++)
		{
			ActiveDoTEffects.Add(DamageOverTimeEffect.FromBytes(reader));
		}
	}

	protected virtual bool TurnIntoDoTEffect(DamageSource damageSource, float damage)
	{
		if (damageSource.Duration <= TimeSpan.Zero)
		{
			return false;
		}
		ApplyDoTEffect(damageSource.Source, damageSource.Type, damageSource.DamageTier, damage, damageSource.Duration, damageSource.TicksPerDuration, damageSource.DamageOverTimeType);
		return true;
	}
}
