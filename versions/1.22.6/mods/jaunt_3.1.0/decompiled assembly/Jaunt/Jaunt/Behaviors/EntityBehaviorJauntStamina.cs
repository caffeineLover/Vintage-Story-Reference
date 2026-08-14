using System;
using System.Linq;
using System.Text;
using Jaunt.Systems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace Jaunt.Behaviors
{
	// Token: 0x02000015 RID: 21
	public class EntityBehaviorJauntStamina : EntityBehavior
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00005746 File Offset: 0x00003946
		public static JauntModSystem ModSystem
		{
			get
			{
				return JauntModSystem.Instance;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000CC RID: 204 RVA: 0x00005750 File Offset: 0x00003950
		// (remove) Token: 0x060000CD RID: 205 RVA: 0x00005788 File Offset: 0x00003988
		public event OnFatiguedDelegate OnFatigued = (float ftg, FatigueSource ftgSource) => ftg;

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000CE RID: 206 RVA: 0x000057BD File Offset: 0x000039BD
		protected static bool DebugMode
		{
			get
			{
				return EntityBehaviorJauntStamina.ModSystem.DebugMode;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000CF RID: 207 RVA: 0x000057C9 File Offset: 0x000039C9
		private ITreeAttribute StaminaTree
		{
			get
			{
				return this.entity.WatchedAttributes.GetTreeAttribute(EntityBehaviorJauntStamina.AttributeKey);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000057E0 File Offset: 0x000039E0
		private static string AttributeKey
		{
			get
			{
				return EntityBehaviorJauntStamina.ModSystem.ModId + ":stamina";
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x000057F6 File Offset: 0x000039F6
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x000057FE File Offset: 0x000039FE
		public bool ActivelyFatiguing { get; set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00005807 File Offset: 0x00003A07
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x0000581A File Offset: 0x00003A1A
		public bool Exhausted
		{
			get
			{
				return this.StaminaTree.GetBool("exhausted", false);
			}
			set
			{
				this.StaminaTree.SetBool("exhausted", value);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x0000582D File Offset: 0x00003A2D
		// (set) Token: 0x060000D6 RID: 214 RVA: 0x00005844 File Offset: 0x00003A44
		public float Stamina
		{
			get
			{
				return this.StaminaTree.GetFloat("currentstamina", 0f);
			}
			set
			{
				this.StaminaTree.SetFloat("currentstamina", value);
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00005857 File Offset: 0x00003A57
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x0000586E File Offset: 0x00003A6E
		public float MaxStamina
		{
			get
			{
				return this.StaminaTree.GetFloat("maxstamina", 0f);
			}
			set
			{
				this.StaminaTree.SetFloat("maxstamina", value);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00005881 File Offset: 0x00003A81
		public float AdjustedMaxStamina
		{
			get
			{
				return this.MaxStamina * EntityBehaviorJauntStamina.ModSystem.Config.GlobalMaxStaminaMultiplier;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00005899 File Offset: 0x00003A99
		// (set) Token: 0x060000DB RID: 219 RVA: 0x000058B0 File Offset: 0x00003AB0
		public float SprintFatigue
		{
			get
			{
				return this.StaminaTree.GetFloat("sprintfatigue", 0f);
			}
			set
			{
				this.StaminaTree.SetFloat("sprintfatigue", value);
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000058C3 File Offset: 0x00003AC3
		// (set) Token: 0x060000DD RID: 221 RVA: 0x000058DA File Offset: 0x00003ADA
		public float SwimFatigue
		{
			get
			{
				return this.StaminaTree.GetFloat("swimfatigue", 0f);
			}
			set
			{
				this.StaminaTree.SetFloat("swimfatigue", value);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000058ED File Offset: 0x00003AED
		// (set) Token: 0x060000DF RID: 223 RVA: 0x00005904 File Offset: 0x00003B04
		public float BaseFatigueRate
		{
			get
			{
				return this.StaminaTree.GetFloat("basefatiguerate", 0f);
			}
			set
			{
				this.StaminaTree.SetFloat("basefatiguerate", value);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00005917 File Offset: 0x00003B17
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x0000592E File Offset: 0x00003B2E
		public float StaminaRegenRate
		{
			get
			{
				return this.StaminaTree.GetFloat("staminaregenrate", 0f);
			}
			set
			{
				this.StaminaTree.SetFloat("staminaregenrate", value);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00005941 File Offset: 0x00003B41
		// (set) Token: 0x060000E3 RID: 227 RVA: 0x00005958 File Offset: 0x00003B58
		public float RegenPenaltyWounded
		{
			get
			{
				return this.StaminaTree.GetFloat("regenpenaltywounded", 0f);
			}
			set
			{
				this.StaminaTree.SetFloat("regenpenaltywounded", value);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x0000596B File Offset: 0x00003B6B
		// (set) Token: 0x060000E5 RID: 229 RVA: 0x00005982 File Offset: 0x00003B82
		public float RegenPenaltyMounted
		{
			get
			{
				return this.StaminaTree.GetFloat("regenpenaltymounted", 0f);
			}
			set
			{
				this.StaminaTree.SetFloat("regenpenaltymounted", value);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00005995 File Offset: 0x00003B95
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x000059A8 File Offset: 0x00003BA8
		public bool Sprinting
		{
			get
			{
				return this.StaminaTree.GetBool("sprinting", false);
			}
			set
			{
				this.StaminaTree.SetBool("sprinting", value);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x000059BB File Offset: 0x00003BBB
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x000059CE File Offset: 0x00003BCE
		public bool Fleeing
		{
			get
			{
				return this.StaminaTree.GetBool("fleeing", false);
			}
			set
			{
				this.StaminaTree.SetBool("fleeing", value);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000EA RID: 234 RVA: 0x000059E1 File Offset: 0x00003BE1
		// (set) Token: 0x060000EB RID: 235 RVA: 0x000059F4 File Offset: 0x00003BF4
		public bool DontFleeWhenExhausted
		{
			get
			{
				return this.StaminaTree.GetBool("dontfleewhenexhausted", false);
			}
			set
			{
				this.StaminaTree.SetBool("dontfleewhenexhausted", value);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00005A07 File Offset: 0x00003C07
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00005A1E File Offset: 0x00003C1E
		public float ExhaustionThreshold
		{
			get
			{
				return this.StaminaTree.GetFloat("exhaustionthreshold", 0f);
			}
			set
			{
				this.StaminaTree.GetFloat("exhaustionthreshold", value);
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00005A32 File Offset: 0x00003C32
		public EntityBehaviorJauntStamina(Entity entity) : base(entity)
		{
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005A60 File Offset: 0x00003C60
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			if (EntityBehaviorJauntStamina.DebugMode)
			{
				EntityBehaviorJauntStamina.ModSystem.Logger.Notification(Lang.Get("jaunt:debug-stamina-init", new object[]
				{
					this.entity.EntityId
				}));
			}
			this.SprintFatigueSource = new FatigueSource
			{
				Source = EnumFatigueSource.Run,
				SourceEntity = this.entity
			};
			this.SwimFatigueSource = new FatigueSource
			{
				Source = EnumFatigueSource.Swim,
				SourceEntity = this.entity
			};
			if (this.entity.WatchedAttributes.GetTreeAttribute(EntityBehaviorJauntStamina.AttributeKey) == null)
			{
				this.entity.WatchedAttributes.SetAttribute(EntityBehaviorJauntStamina.AttributeKey, new TreeAttribute());
				this.MaxStamina = attributes["maxstamina"].AsFloat(EntityBehaviorJauntStamina.ModSystem.Config.DefaultMaxStamina);
				this.Stamina = attributes["currentstamina"].AsFloat(this.MaxStamina);
				this.Sprinting = false;
			}
			this.MaxStamina = attributes["maxstamina"].AsFloat(EntityBehaviorJauntStamina.ModSystem.Config.DefaultMaxStamina);
			this.Stamina = Math.Clamp(this.Stamina, 0f, this.MaxStamina);
			this.SprintFatigue = attributes["sprintfatigue"].AsFloat(EntityBehaviorJauntStamina.ModSystem.Config.DefaultSprintFatigue);
			this.SwimFatigue = attributes["swimfatigue"].AsFloat(EntityBehaviorJauntStamina.ModSystem.Config.DefaultSwimFatigue);
			this.BaseFatigueRate = attributes["basefatiguerate"].AsFloat(EntityBehaviorJauntStamina.ModSystem.Config.DefaultBaseFatigueRate);
			this.StaminaRegenRate = attributes["staminaregenrate"].AsFloat(EntityBehaviorJauntStamina.ModSystem.Config.DefaultStaminaRegenRate);
			this.RegenPenaltyWounded = attributes["regenpenaltywounded"].AsFloat(EntityBehaviorJauntStamina.ModSystem.Config.DefaultRegenPenaltyWounded);
			this.RegenPenaltyMounted = attributes["regenpenaltymounted"].AsFloat(EntityBehaviorJauntStamina.ModSystem.Config.DefaultRegenPenaltyMounted);
			this.ExhaustionThreshold = attributes["exhaustionthreshold"].AsFloat(EntityBehaviorJauntStamina.ModSystem.Config.DefaultExhaustionThreshold);
			this.DontFleeWhenExhausted = attributes["dontfleewhenexhausted"].AsBool(EntityBehaviorJauntStamina.ModSystem.Config.DefaultDontFleeWhenExhausted);
			this.MarkDirty();
			if (this.MaxStamina <= 0f)
			{
				this.MaxStamina = attributes["maxstamina"].AsFloat(EntityBehaviorJauntStamina.ModSystem.Config.DefaultMaxStamina);
				this.MarkDirty();
			}
			this.timeSinceLastUpdate = (float)this.entity.World.Rand.NextDouble();
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005D20 File Offset: 0x00003F20
		public override void OnGameTick(float dt)
		{
			if (this.entity.World.Side == 2)
			{
				return;
			}
			EntityBehaviorTaskAI ebtai = this.entity.GetBehavior<EntityBehaviorTaskAI>();
			this.Fleeing = Enumerable.Any<IAiTask>(ebtai.TaskManager.ActiveTasksBySlot, (IAiTask task) => task is AiTaskFleeEntity);
			this.timeSinceLastUpdate += dt;
			this.timeSinceLastLog += dt;
			if (this.timeSinceLastLog > 1f && EntityBehaviorJauntStamina.DebugMode)
			{
				this.timeSinceLastLog = 0f;
			}
			if (this.timeSinceLastUpdate >= 0.25f)
			{
				if (this.entity.Alive)
				{
					if (this.Fleeing && this.Exhausted && this.DontFleeWhenExhausted)
					{
						ebtai.TaskManager.StopTask(typeof(AiTaskFleeEntity));
					}
					if (this.entity.Swimming)
					{
						this.ActivelyFatiguing = this.ApplyFatigue(this.SwimFatigue * EntityBehaviorJauntStamina.ModSystem.Config.GlobalSwimStaminaCostMultiplier * this.CalculateElapsedMultiplier(this.timeSinceLastUpdate), EnumFatigueSource.Swim);
					}
					if (this.Fleeing)
					{
						this.ActivelyFatiguing = this.ApplyFatigue(this.SprintFatigue * EntityBehaviorJauntStamina.ModSystem.Config.GlobalSprintStaminaCostMultiplier * this.CalculateElapsedMultiplier(this.timeSinceLastUpdate), EnumFatigueSource.Run);
					}
					if (!this.ActivelyFatiguing)
					{
						this.RegenerateStamina(this.timeSinceLastUpdate);
					}
				}
				this.Exhausted = (this.Stamina / this.MaxStamina <= this.ExhaustionThreshold);
				this.MarkDirty();
				this.timeSinceLastUpdate = 0f;
				this.ActivelyFatiguing = false;
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005EC9 File Offset: 0x000040C9
		private float CalculateElapsedMultiplier(float elapsedTime)
		{
			return elapsedTime * this.entity.Api.World.Calendar.SpeedOfTime * this.entity.Api.World.Calendar.CalendarSpeedMul;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005F04 File Offset: 0x00004104
		public void RegenerateStamina(float elapsedTime)
		{
			float stamina = this.Stamina;
			float maxStamina = this.AdjustedMaxStamina;
			EntityBehaviorHealth ebh = this.entity.GetBehavior<EntityBehaviorHealth>();
			float currentWoundedPenalty = (ebh.Health < ebh.MaxHealth * 0.7f) ? this.RegenPenaltyWounded : 0f;
			EntityBehaviorRideable behavior = this.entity.GetBehavior<EntityBehaviorRideable>();
			float totalPenalty = ((behavior != null && behavior.AnyMounted()) ? this.RegenPenaltyMounted : 0f) + currentWoundedPenalty;
			float staminaRegenRate = (this.StaminaRegenRate - totalPenalty) * EntityBehaviorJauntStamina.ModSystem.Config.GlobalStaminaRegenMultiplier;
			if (stamina < maxStamina)
			{
				float staminaRegenPerQuarterSecond = 0.25f * staminaRegenRate;
				if (EntityBehaviorJauntStamina.ModSystem.api.World.Calendar != null)
				{
					float multiplierPerGameSec = elapsedTime * EntityBehaviorJauntStamina.ModSystem.api.World.Calendar.SpeedOfTime * EntityBehaviorJauntStamina.ModSystem.api.World.Calendar.CalendarSpeedMul;
					this.Stamina = Math.Min(stamina + multiplierPerGameSec * staminaRegenPerQuarterSecond, maxStamina);
					this.MarkDirty();
				}
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006008 File Offset: 0x00004208
		private bool ApplyFatigue(float fatigueAmount, EnumFatigueSource source)
		{
			if (fatigueAmount <= 0f)
			{
				return false;
			}
			FatigueSource fs = new FatigueSource
			{
				Source = source,
				SourceEntity = this.entity,
				CauseEntity = this.entity,
				SourceBlock = null,
				SourcePos = this.entity.Pos.XYZ
			};
			this.FatigueEntity(fatigueAmount, fs);
			return true;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000606C File Offset: 0x0000426C
		public void OnEntityFatigued(FatigueSource fatigueSource, ref float fatigue)
		{
			if (this.entity.World.Side == 2)
			{
				return;
			}
			if (this.OnFatigued != null)
			{
				foreach (OnFatiguedDelegate dele in Enumerable.Cast<OnFatiguedDelegate>(this.OnFatigued.GetInvocationList()))
				{
					fatigue = dele(fatigue, fatigueSource);
				}
			}
			this.FatigueEntity(fatigue, fatigueSource);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000060EC File Offset: 0x000042EC
		public void FatigueEntity(float fatigue, FatigueSource fs)
		{
			this.ActivelyFatiguing = true;
			if (this.entity.World.Side == 2)
			{
				return;
			}
			if (!this.entity.Alive)
			{
				return;
			}
			if (fatigue <= 0f)
			{
				return;
			}
			float fatigueRate = this.BaseFatigueRate * fatigue;
			this.Stamina = GameMath.Clamp(this.Stamina - fatigueRate, 0f, this.AdjustedMaxStamina);
			this.MarkDirty();
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00006158 File Offset: 0x00004358
		public override void GetInfoText(StringBuilder infotext)
		{
			ICoreClientAPI capi = this.entity.Api as ICoreClientAPI;
			if (capi == null)
			{
				return;
			}
			IClientPlayer player = capi.World.Player;
			bool flag;
			if (player == null)
			{
				flag = false;
			}
			else
			{
				IWorldPlayerData worldData = player.WorldData;
				flag = (((worldData != null) ? new EnumGameMode?(worldData.CurrentGameMode) : default(EnumGameMode?)).GetValueOrDefault() == 2);
			}
			if (flag || capi.Settings.Bool["extendedDebugInfo"])
			{
				infotext.AppendLine(Lang.Get("jaunt:infotext-stamina-state", new object[]
				{
					this.Stamina,
					this.AdjustedMaxStamina
				}));
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00006201 File Offset: 0x00004401
		public override string PropertyName()
		{
			return EntityBehaviorJauntStamina.AttributeKey;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00006208 File Offset: 0x00004408
		public void MarkDirty()
		{
			this.entity.WatchedAttributes.MarkPathDirty(EntityBehaviorJauntStamina.AttributeKey);
		}

		// Token: 0x04000064 RID: 100
		protected float timeSinceLastUpdate;

		// Token: 0x04000065 RID: 101
		protected float timeSinceLastLog;

		// Token: 0x04000067 RID: 103
		public FatigueSource SprintFatigueSource;

		// Token: 0x04000068 RID: 104
		public FatigueSource SwimFatigueSource;
	}
}
