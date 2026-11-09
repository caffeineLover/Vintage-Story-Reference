using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Colliders;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000123 RID: 291
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EntityDamageModelBehavior : EntityBehavior, IEntityDamageModel
	{
		// Token: 0x06000E01 RID: 3585 RVA: 0x00042FB8 File Offset: 0x000411B8
		public EntityDamageModelBehavior(Entity entity)
		{
			Dictionary<ColliderTypes, float> dictionary = new Dictionary<ColliderTypes, float>();
			dictionary.Add(ColliderTypes.Torso, 1f);
			dictionary.Add(ColliderTypes.Arm, 1f);
			dictionary.Add(ColliderTypes.Leg, 1f);
			dictionary.Add(ColliderTypes.Head, 1f);
			dictionary.Add(ColliderTypes.Critical, 1f);
			dictionary.Add(ColliderTypes.Resistant, 0f);
			this.DamageMultipliers = dictionary;
			this.Resists = new DamageResistData();
			this.ResistsForColliders = new Dictionary<ColliderTypes, DamageResistData>();
			this.HitSounds = new Dictionary<ColliderTypes, SoundEffectData>();
			this.HitParticles = new Dictionary<ColliderTypes, string>();
			this.ScaleParticlesCountWithDamage = true;
			base..ctor(entity);
			this._animationsSystem = entity.Api.ModLoader.GetModSystem<CombatOverhaulAnimationsSystem>(true);
			this._system = entity.Api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
		}

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x06000E02 RID: 3586 RVA: 0x00043080 File Offset: 0x00041280
		// (remove) Token: 0x06000E03 RID: 3587 RVA: 0x000430B8 File Offset: 0x000412B8
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event OnEntityReceiveDamageDelegate OnReceiveDamage;

		// Token: 0x06000E04 RID: 3588 RVA: 0x000430ED File Offset: 0x000412ED
		public override string PropertyName()
		{
			return "EntityDamageModel";
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06000E05 RID: 3589 RVA: 0x000430F4 File Offset: 0x000412F4
		// (set) Token: 0x06000E06 RID: 3590 RVA: 0x000430FC File Offset: 0x000412FC
		public Dictionary<ColliderTypes, float> DamageMultipliers { get; private set; }

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000E07 RID: 3591 RVA: 0x00043105 File Offset: 0x00041305
		// (set) Token: 0x06000E08 RID: 3592 RVA: 0x0004310D File Offset: 0x0004130D
		public DamageResistData Resists { get; set; }

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000E09 RID: 3593 RVA: 0x00043116 File Offset: 0x00041316
		// (set) Token: 0x06000E0A RID: 3594 RVA: 0x0004311E File Offset: 0x0004131E
		public Dictionary<ColliderTypes, DamageResistData> ResistsForColliders { get; private set; }

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000E0B RID: 3595 RVA: 0x00043127 File Offset: 0x00041327
		// (set) Token: 0x06000E0C RID: 3596 RVA: 0x0004312F File Offset: 0x0004132F
		public Dictionary<ColliderTypes, SoundEffectData> HitSounds { get; private set; }

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000E0D RID: 3597 RVA: 0x00043138 File Offset: 0x00041338
		// (set) Token: 0x06000E0E RID: 3598 RVA: 0x00043140 File Offset: 0x00041340
		public Dictionary<ColliderTypes, string> HitParticles { get; private set; }

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000E0F RID: 3599 RVA: 0x00043149 File Offset: 0x00041349
		// (set) Token: 0x06000E10 RID: 3600 RVA: 0x00043151 File Offset: 0x00041351
		public bool ScaleParticlesCountWithDamage { get; private set; }

		// Token: 0x06000E11 RID: 3601 RVA: 0x0004315C File Offset: 0x0004135C
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			if (attributes.KeyExists("damageModel"))
			{
				EntityDamageModelJson stats = attributes["damageModel"].AsObject<EntityDamageModelJson>(null);
				this.Resists = new DamageResistData(Enumerable.ToDictionary<KeyValuePair<string, float>, EnumDamageType, float>(stats.DefaultResists, (KeyValuePair<string, float> entry) => Enum.Parse<EnumDamageType>(entry.Key), (KeyValuePair<string, float> entry) => entry.Value));
				this.ResistsForColliders = Enumerable.ToDictionary<KeyValuePair<string, Dictionary<string, float>>, ColliderTypes, DamageResistData>(stats.ResistsForColliders, (KeyValuePair<string, Dictionary<string, float>> entry) => Enum.Parse<ColliderTypes>(entry.Key), (KeyValuePair<string, Dictionary<string, float>> entry) => new DamageResistData(Enumerable.ToDictionary<KeyValuePair<string, float>, EnumDamageType, float>(entry.Value, (KeyValuePair<string, float> entry) => Enum.Parse<EnumDamageType>(entry.Key), (KeyValuePair<string, float> entry) => entry.Value)));
				Dictionary<ColliderTypes, float> dictionary = new Dictionary<ColliderTypes, float>();
				dictionary.Add(ColliderTypes.Torso, stats.TorsoDamageMultiplier);
				dictionary.Add(ColliderTypes.Arm, stats.LimbsDamageMultiplier);
				dictionary.Add(ColliderTypes.Leg, stats.LimbsDamageMultiplier);
				dictionary.Add(ColliderTypes.Head, stats.HeadDamageMultiplier);
				dictionary.Add(ColliderTypes.Critical, stats.CriticalDamageMultiplier);
				dictionary.Add(ColliderTypes.Resistant, stats.ResistantDamageMultiplier);
				this.DamageMultipliers = dictionary;
				this.HitSounds = Enumerable.ToDictionary<KeyValuePair<string, SoundEffectData>, ColliderTypes, SoundEffectData>(stats.HitSounds, (KeyValuePair<string, SoundEffectData> entry) => Enum.Parse<ColliderTypes>(entry.Key), (KeyValuePair<string, SoundEffectData> entry) => entry.Value);
				this.HitParticles = Enumerable.ToDictionary<KeyValuePair<string, string>, ColliderTypes, string>(stats.HitParticles, (KeyValuePair<string, string> entry) => Enum.Parse<ColliderTypes>(entry.Key), (KeyValuePair<string, string> entry) => entry.Value);
				this.ScaleParticlesCountWithDamage = stats.ScaleParticlesCountWithDamage;
			}
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00043334 File Offset: 0x00041534
		public override void GetInfoText(StringBuilder infotext)
		{
			if (!Enumerable.Any<float>(this.Resists.Resists.Values, (float x) => x > 0f))
			{
				return;
			}
			if (this._system.Settings.ShortEntityInfo)
			{
				int piercing = 0;
				int slashing = 0;
				int blunt = 0;
				float piercingValue;
				if (this.Resists.Resists.TryGetValue(4, ref piercingValue))
				{
					piercing = (int)piercingValue;
				}
				float slashingValue;
				if (this.Resists.Resists.TryGetValue(3, ref slashingValue))
				{
					slashing = (int)slashingValue;
				}
				float bluntValue;
				if (this.Resists.Resists.TryGetValue(2, ref bluntValue))
				{
					blunt = (int)bluntValue;
				}
				infotext.AppendLine(Lang.Get("combatoverhaul:damage-short-protection-info", new object[]
				{
					piercing,
					slashing,
					blunt
				}));
				return;
			}
			infotext.AppendLine(Lang.Get("combatoverhaul:damage-protection-info", Array.Empty<object>()));
			foreach (KeyValuePair<EnumDamageType, float> keyValuePair in this.Resists.Resists)
			{
				EnumDamageType enumDamageType;
				float num;
				keyValuePair.Deconstruct(ref enumDamageType, ref num);
				EnumDamageType type = enumDamageType;
				float value = num;
				if (value > 0f)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
					defaultInterpolatedStringHandler.AppendLiteral("combatoverhaul:damage-type-");
					defaultInterpolatedStringHandler.AppendFormatted<EnumDamageType>(type);
					string damageType = Lang.Get(defaultInterpolatedStringHandler.ToStringAndClear(), Array.Empty<object>());
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(4, 2, infotext);
					appendInterpolatedStringHandler.AppendLiteral("  ");
					appendInterpolatedStringHandler.AppendFormatted(damageType);
					appendInterpolatedStringHandler.AppendLiteral(": ");
					appendInterpolatedStringHandler.AppendFormatted<float>(value);
					infotext.AppendLine(ref appendInterpolatedStringHandler);
				}
			}
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00043504 File Offset: 0x00041704
		public override void AfterInitialized(bool onFirstSpawn)
		{
			this._colliders = this.entity.GetBehavior<CollidersEntityBehavior>();
			EntityBehaviorHealth healthBehavior = this.entity.GetBehavior<EntityBehaviorHealth>();
			if (healthBehavior != null)
			{
				healthBehavior.onDamaged += new OnDamagedDelegate(this.OnReceiveDamageHandler);
			}
			CollidersEntityBehavior colliders = this._colliders;
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x0004354C File Offset: 0x0004174C
		private float OnReceiveDamageHandler(float damage, DamageSource damageSource)
		{
			ColliderTypes colliderType = ColliderTypes.Torso;
			string collider = null;
			Vector3d position = default(Vector3d);
			if (this._colliders != null)
			{
				ILocationalDamage locationalDamageSource = damageSource as ILocationalDamage;
				if (locationalDamageSource != null)
				{
					if (this._colliders.CollidersTypes.ContainsKey(locationalDamageSource.Collider))
					{
						colliderType = this._colliders.CollidersTypes[locationalDamageSource.Collider];
					}
					collider = locationalDamageSource.Collider;
					float multiplier = this.DamageMultipliers[colliderType];
					damage *= multiplier;
					position = locationalDamageSource.Position;
				}
			}
			ITypedDamage typedDamage = damageSource as ITypedDamage;
			if (typedDamage != null)
			{
				if (this.ResistsForColliders.ContainsKey(colliderType))
				{
					typedDamage.DamageTypeData = this.ResistsForColliders[colliderType].ApplyNonPlayerResist(typedDamage.DamageTypeData, ref damage);
				}
				else
				{
					typedDamage.DamageTypeData = this.Resists.ApplyNonPlayerResist(typedDamage.DamageTypeData, ref damage);
				}
			}
			else
			{
				DamageData damageData = new DamageData(damageSource.Type, damageSource.DamageTier, 0);
				this.Resists.ApplyNonPlayerResist(damageData, ref damage);
			}
			SoundEffectData value;
			if (this.HitSounds.TryGetValue(colliderType, ref value))
			{
				this.entity.Api.World.PlaySoundAt(new AssetLocation(value.Code), this.entity, null, value.RandomizePitch, value.Range, value.Volume);
			}
			string particlesEffect;
			if (this.HitParticles.TryGetValue(colliderType, ref particlesEffect))
			{
				float intensity = this.ScaleParticlesCountWithDamage ? MathF.Sqrt(damage) : 1f;
				if (damage <= 0f)
				{
					intensity = 1f;
				}
				ParticleEffectsManager particleEffectsManager = this._animationsSystem.ParticleEffectsManager;
				if (particleEffectsManager != null)
				{
					particleEffectsManager.Spawn(particlesEffect, position, Vector3.Zero, intensity);
				}
			}
			OnEntityReceiveDamageDelegate onReceiveDamage = this.OnReceiveDamage;
			if (onReceiveDamage != null)
			{
				onReceiveDamage(ref damage, damageSource, colliderType, collider);
			}
			return damage;
		}

		// Token: 0x040006E0 RID: 1760
		[Nullable(2)]
		private CollidersEntityBehavior _colliders;

		// Token: 0x040006E1 RID: 1761
		private readonly CombatOverhaulAnimationsSystem _animationsSystem;

		// Token: 0x040006E2 RID: 1762
		private readonly CombatOverhaulSystem _system;
	}
}
