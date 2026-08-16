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
	// Token: 0x02000118 RID: 280
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EntityDamageModelBehavior : EntityBehavior, IEntityDamageModel
	{
		// Token: 0x06000DA5 RID: 3493 RVA: 0x00041F94 File Offset: 0x00040194
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
		// (add) Token: 0x06000DA6 RID: 3494 RVA: 0x0004205C File Offset: 0x0004025C
		// (remove) Token: 0x06000DA7 RID: 3495 RVA: 0x00042094 File Offset: 0x00040294
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event OnEntityReceiveDamageDelegate OnReceiveDamage;

		// Token: 0x06000DA8 RID: 3496 RVA: 0x000420C9 File Offset: 0x000402C9
		public override string PropertyName()
		{
			return "EntityDamageModel";
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x000420D0 File Offset: 0x000402D0
		// (set) Token: 0x06000DAA RID: 3498 RVA: 0x000420D8 File Offset: 0x000402D8
		public Dictionary<ColliderTypes, float> DamageMultipliers { get; private set; }

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000DAB RID: 3499 RVA: 0x000420E1 File Offset: 0x000402E1
		// (set) Token: 0x06000DAC RID: 3500 RVA: 0x000420E9 File Offset: 0x000402E9
		public DamageResistData Resists { get; set; }

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000DAD RID: 3501 RVA: 0x000420F2 File Offset: 0x000402F2
		// (set) Token: 0x06000DAE RID: 3502 RVA: 0x000420FA File Offset: 0x000402FA
		public Dictionary<ColliderTypes, DamageResistData> ResistsForColliders { get; private set; }

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000DAF RID: 3503 RVA: 0x00042103 File Offset: 0x00040303
		// (set) Token: 0x06000DB0 RID: 3504 RVA: 0x0004210B File Offset: 0x0004030B
		public Dictionary<ColliderTypes, SoundEffectData> HitSounds { get; private set; }

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x00042114 File Offset: 0x00040314
		// (set) Token: 0x06000DB2 RID: 3506 RVA: 0x0004211C File Offset: 0x0004031C
		public Dictionary<ColliderTypes, string> HitParticles { get; private set; }

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x00042125 File Offset: 0x00040325
		// (set) Token: 0x06000DB4 RID: 3508 RVA: 0x0004212D File Offset: 0x0004032D
		public bool ScaleParticlesCountWithDamage { get; private set; }

		// Token: 0x06000DB5 RID: 3509 RVA: 0x00042138 File Offset: 0x00040338
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

		// Token: 0x06000DB6 RID: 3510 RVA: 0x00042310 File Offset: 0x00040510
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

		// Token: 0x06000DB7 RID: 3511 RVA: 0x000424E0 File Offset: 0x000406E0
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

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00042528 File Offset: 0x00040728
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

		// Token: 0x040006BF RID: 1727
		[Nullable(2)]
		private CollidersEntityBehavior _colliders;

		// Token: 0x040006C0 RID: 1728
		private readonly CombatOverhaulAnimationsSystem _animationsSystem;

		// Token: 0x040006C1 RID: 1729
		private readonly CombatOverhaulSystem _system;
	}
}
