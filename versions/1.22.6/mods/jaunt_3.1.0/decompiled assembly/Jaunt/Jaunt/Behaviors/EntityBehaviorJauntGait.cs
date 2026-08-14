using System;
using Jaunt.Systems;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace Jaunt.Behaviors
{
	// Token: 0x02000011 RID: 17
	public class EntityBehaviorJauntGait : EntityBehaviorGait
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x0000469B File Offset: 0x0000289B
		public static JauntModSystem ModSystem
		{
			get
			{
				return JauntModSystem.Instance;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000046A2 File Offset: 0x000028A2
		private static string AttributeKey
		{
			get
			{
				return EntityBehaviorJauntGait.ModSystem.ModId + ":gait";
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000046B8 File Offset: 0x000028B8
		public override string PropertyName()
		{
			return EntityBehaviorJauntGait.AttributeKey;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000046BF File Offset: 0x000028BF
		public EntityBehaviorJauntGait(Entity entity) : base(entity)
		{
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000046C8 File Offset: 0x000028C8
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			base.Initialize(properties, attributes);
			foreach (JauntGaitMeta gait in attributes["gaits"].AsArray<JauntGaitMeta>(null, "game"))
			{
				this.Gaits[gait.Code] = gait;
				AssetLocation iconTexture = gait.IconTexture;
				if (iconTexture != null)
				{
					iconTexture.WithPathPrefixOnce("textures/");
				}
				if (this.entity.Api.Side == 2)
				{
					EntityBehaviorJauntGait.ModSystem.hudIconRenderer.RegisterTexture(gait.IconTexture);
				}
			}
			foreach (EnumHabitat key in this.IdleGaits.Keys)
			{
				this.IdleGaits[key] = this.Gaits[this.IdleGaits[key].Code];
			}
			this.EnableDamageHandler = attributes["enableDamageHandler"].AsBool(false);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x000047D8 File Offset: 0x000029D8
		public override void AfterInitialized(bool onFirstSpawn)
		{
			base.AfterInitialized(onFirstSpawn);
			this.staminaBehavior = this.entity.GetBehavior<EntityBehaviorJauntStamina>();
			EntityBehaviorHealth ebh = this.eagent.GetBehavior<EntityBehaviorHealth>();
			if (ebh != null)
			{
				ebh.onDamaged += ((float dmg, DamageSource dmgSource) => this.HandleDamaged(this.eagent, dmg, dmgSource));
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004820 File Offset: 0x00002A20
		public void ApplyGaitFatigue(float dt)
		{
			if (this.entity.Api.Side != 1 || this.staminaBehavior == null)
			{
				return;
			}
			this.timeSinceLastGaitFatigue += dt;
			if (this.timeSinceLastGaitFatigue >= 0.25f && base.CurrentGait.StaminaCost > 0f && !this.entity.Swimming)
			{
				EntityBehaviorJauntStamina entityBehaviorJauntStamina = this.staminaBehavior;
				float staminaCost = base.CurrentGait.StaminaCost;
				FatigueSource fatigueSource = new FatigueSource();
				fatigueSource.Source = EnumFatigueSource.Mounted;
				EntityAgent entityAgent = this.entity as EntityAgent;
				Entity entity;
				if (entityAgent == null)
				{
					entity = null;
				}
				else
				{
					IMountableSeat mountedOn = entityAgent.MountedOn;
					entity = ((mountedOn != null) ? mountedOn.Passenger : null);
				}
				fatigueSource.SourceEntity = (entity ?? this.entity);
				entityBehaviorJauntStamina.FatigueEntity(staminaCost, fatigueSource);
				if (this.entity.World.Rand.NextDouble() < (double)EntityBehaviorJauntGait.GetStaminaDeficitMultiplier(this.staminaBehavior.Stamina, this.staminaBehavior.MaxStamina))
				{
					base.CurrentGait = ((this.staminaBehavior.Stamina < 10f) ? base.CascadingFallbackGait(2) : base.FallbackGait);
				}
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004940 File Offset: 0x00002B40
		public static float GetStaminaDeficitMultiplier(float currentStamina, float maxStamina)
		{
			float midpoint = maxStamina * 0.5f;
			if (currentStamina >= midpoint)
			{
				return 0f;
			}
			float num = 1f - currentStamina / midpoint;
			return num * num;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000496A File Offset: 0x00002B6A
		public override void OnGameTick(float dt)
		{
			base.OnGameTick(dt);
			this.UpdateClimbAnimation();
			this.ApplyGaitFatigue(dt);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004980 File Offset: 0x00002B80
		public override void UpdateGaitForEnvironment()
		{
			EnumHabitat targetEnvironment = 1;
			if (this.eagent.Controls.IsFlying || (this.entity.Api.Side == 1 && base.CurrentGait.Environment == 2 && !this.entity.OnGround))
			{
				targetEnvironment = 2;
			}
			if (this.entity.Swimming)
			{
				targetEnvironment = 0;
			}
			if (base.CurrentGait.Environment == targetEnvironment)
			{
				return;
			}
			GaitMeta closest = null;
			foreach (GaitMeta gait in this.Gaits.Values)
			{
				if (gait.Natural && gait.Environment == targetEnvironment && gait.Direction == base.CurrentGait.Direction && (closest == null || Math.Abs(gait.MoveSpeed - base.CurrentGait.MoveSpeed) < Math.Abs(closest.MoveSpeed - base.CurrentGait.MoveSpeed)))
				{
					closest = gait;
				}
			}
			base.CurrentGait = closest;
			EntityBehaviorJauntRideable behavior = this.entity.GetBehavior<EntityBehaviorJauntRideable>();
			if (behavior == null)
			{
				return;
			}
			behavior.OnGaitChangedForEnvironment();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004AA4 File Offset: 0x00002CA4
		protected override void Move(float dt)
		{
			base.Move(dt);
			if (this.eagent.Controls.IsFlying)
			{
				this.eagent.Controls.FlyVector.Set(this.eagent.Controls.WalkVector);
				this.eagent.Pos.Motion.Y = this.VerticalSpeed;
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004B0C File Offset: 0x00002D0C
		protected virtual void UpdateClimbAnimation()
		{
			string nowClimbAnim = null;
			if (this.eagent.Controls.IsFlying && !base.CurrentGait.HasBackwardMotion)
			{
				if (this.VerticalSpeed > 0.001)
				{
					nowClimbAnim = "fly-up";
				}
				else if (this.VerticalSpeed < -0.001)
				{
					nowClimbAnim = "fly-down";
				}
			}
			if (nowClimbAnim != this.currentClimbAnimation)
			{
				if (this.currentClimbAnimation != null)
				{
					this.eagent.StopAnimation(this.currentClimbAnimation);
					this.eagent.AnimManager.AnimationsDirty = true;
				}
				this.currentClimbAnimation = nowClimbAnim;
				if (nowClimbAnim != null)
				{
					this.eagent.StartAnimation(nowClimbAnim);
				}
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004BB8 File Offset: 0x00002DB8
		public float HandleDamaged(EntityAgent eagent, float damage, DamageSource damageSource)
		{
			if (this.entity.Properties.FallDamageMultiplier == 0f)
			{
				return damage;
			}
			if (base.CurrentGait.Environment == 2 && this.EnableDamageHandler)
			{
				return 0f;
			}
			return damage;
		}

		// Token: 0x04000054 RID: 84
		public double VerticalSpeed;

		// Token: 0x04000055 RID: 85
		public string currentClimbAnimation;

		// Token: 0x04000056 RID: 86
		public bool EnableDamageHandler;

		// Token: 0x04000057 RID: 87
		private float timeSinceLastGaitFatigue;

		// Token: 0x04000058 RID: 88
		protected EntityBehaviorJauntStamina staminaBehavior;
	}
}
