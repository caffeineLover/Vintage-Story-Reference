using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Animations;
using CombatOverhaul.Colliders;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.Inputs;
using CombatOverhaul.Integration;
using CombatOverhaul.MeleeSystems;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000095 RID: 149
	[NullableContext(1)]
	[Nullable(0)]
	public class StanceBasedMeleeWeaponClient : IClientWeaponLogic, IHasDynamicIdleAnimations, IOnGameTick, IRestrictAction
	{
		// Token: 0x060007F9 RID: 2041 RVA: 0x0002E634 File Offset: 0x0002C834
		public StanceBasedMeleeWeaponClient(ICoreClientAPI api, Item item)
		{
			this.Item = item;
			this.Api = api;
			CombatOverhaulSystem system = api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			MeleeBlockSystemClient clientBlockSystem = system.ClientBlockSystem;
			if (clientBlockSystem == null)
			{
				throw new Exception();
			}
			this.MeleeBlockSystem = clientBlockSystem;
			SoundsSynchronizerClient clientSoundsSynchronizer = system.ClientSoundsSynchronizer;
			if (clientSoundsSynchronizer == null)
			{
				throw new Exception();
			}
			this.SoundsSystem = clientSoundsSynchronizer;
			this.Settings = system.Settings;
			this.Stats = item.Attributes.AsObject<StanceBasedMeleeWeaponStats>(null);
			if (this.Stats.OneHanded != null)
			{
				this.OneHandedStats = new GripSpecificStats(api, item, this.Stats.OneHanded);
			}
			if (this.Stats.TwoHanded != null)
			{
				this.TwoHandedStats = new GripSpecificStats(api, item, this.Stats.TwoHanded);
			}
			if (this.Stats.OffHand != null)
			{
				this.OffHandStats = new GripSpecificStats(api, item, this.Stats.OffHand);
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x0002E722 File Offset: 0x0002C922
		public int ItemId
		{
			get
			{
				return this.Item.Id;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x0002E72F File Offset: 0x0002C92F
		// (set) Token: 0x060007FC RID: 2044 RVA: 0x0002E737 File Offset: 0x0002C937
		public virtual DirectionsConfiguration DirectionsType { get; protected set; } = DirectionsConfiguration.None;

		// Token: 0x060007FD RID: 2045 RVA: 0x0002E740 File Offset: 0x0002C940
		public AnimationRequestByCode? GetIdleAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			GripSpecificStats stats = this.GetGripSpecificStats(mainHand, ((playerBehavior != null) ? playerBehavior.entity : null) as EntityPlayer);
			return new AnimationRequestByCode?(new AnimationRequestByCode(((stats != null) ? stats.Stats.StanceAnimations[stats.Stats.InitialStance] : null) ?? "", 1f, 1f, StanceBasedMeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null));
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0002E7D0 File Offset: 0x0002C9D0
		public AnimationRequestByCode? GetReadyAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			GripSpecificStats stats = this.GetGripSpecificStats(mainHand, ((playerBehavior != null) ? playerBehavior.entity : null) as EntityPlayer);
			return new AnimationRequestByCode?(new AnimationRequestByCode(((stats != null) ? stats.Stats.StanceAnimations[stats.Stats.InitialStance] : null) ?? "", 1f, 1f, StanceBasedMeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null));
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x0002E860 File Offset: 0x0002CA60
		public virtual void OnSelected(ItemSlot slot, EntityPlayer player, bool mainHand, ref int state)
		{
			GripSpecificStats stats = this.GetGripSpecificStats(mainHand, player);
			if (stats == null)
			{
				return;
			}
			stats.CurrentStance = stats.InitialStance;
			this.DirectionsType = stats.DirectionsType;
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				bool mainHand2 = mainHand;
				string animation = stats.Stats.StanceAnimations[stats.CurrentStance.ToString()];
				float animationSpeed = this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, 0.5f, 2f);
				animationBehavior.Play(mainHand2, animation, StanceBasedMeleeWeaponClient.AnimationCategory(mainHand), animationSpeed, 1f, null, delegate(string code)
				{
					this.StanceAnimationCallbackHandler(code, mainHand, stats, stats.CurrentStance);
				}, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				bool mainHand3 = mainHand;
				string animation2 = stats.Stats.StanceAnimations[stats.CurrentStance.ToString()];
				float animationSpeed = this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, 0.5f, 2f);
				tpAnimationBehavior.Play(mainHand3, animation2, StanceBasedMeleeWeaponClient.AnimationCategory(mainHand), animationSpeed, 1f, true);
			}
			this.SetState<MeleeWeaponState>(MeleeWeaponState.Cooldown, mainHand);
			if (stats.Blocks[stats.CurrentStance] != null)
			{
				this.MeleeBlockSystem.StartBlock(stats.Blocks[stats.CurrentStance], mainHand, EnumDamageBlockKind.Block);
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0002EA10 File Offset: 0x0002CC10
		public virtual void OnDeselected(EntityPlayer player, bool mainHand, ref int state)
		{
			this.MeleeBlockSystem.StopBlock(mainHand);
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.StopSpeedModifier();
			}
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior != null)
			{
				playerBehavior.SetStat("walkspeed", mainHand ? "CombatOverhaul:held-item-mainhand" : "CombatOverhaul:held-item-offhand", 0f);
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.StopAllVanillaAnimations(mainHand);
			}
			GripSpecificStats stats = this.GetGripSpecificStats(mainHand, player);
			if (stats != null)
			{
				stats.CurrentStance = stats.InitialStance;
			}
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x0002EA90 File Offset: 0x0002CC90
		public virtual void OnRegistered(ActionsManagerPlayerBehavior behavior, ICoreClientAPI api)
		{
			this.PlayerBehavior = behavior;
			this.AnimationBehavior = behavior.entity.GetBehavior<FirstPersonAnimationsBehavior>();
			this.TpAnimationBehavior = behavior.entity.GetBehavior<ThirdPersonAnimationsBehavior>();
			if (this.OneHandedStats != null)
			{
				this.OneHandedStats.GripController = new GripController(this.AnimationBehavior);
			}
			if (this.TwoHandedStats != null)
			{
				this.TwoHandedStats.GripController = new GripController(this.AnimationBehavior);
			}
			if (this.OffHandStats != null)
			{
				this.OffHandStats.GripController = new GripController(this.AnimationBehavior);
			}
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0002EB20 File Offset: 0x0002CD20
		public virtual void RenderDebugCollider(ItemSlot inSlot, IClientPlayer byPlayer)
		{
			if (DebugWindowManager._currentCollider != null)
			{
				ICollider collider = DebugWindowManager._currentCollider.Value.Transform(byPlayer.Entity.Pos, byPlayer.Entity, inSlot, this.Api, true);
				if (collider == null)
				{
					return;
				}
				collider.Render(this.Api, byPlayer.Entity, ColorUtil.ColorFromRgba(255, 125, 125, 255));
			}
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0002EB90 File Offset: 0x0002CD90
		public virtual bool OnMouseWheel(ItemSlot slot, IClientPlayer byPlayer, float delta)
		{
			if (!this.IsRightMouseDown(byPlayer))
			{
				return false;
			}
			bool mainHand = StanceBasedMeleeWeaponClient.IsMainHandSlot(slot, byPlayer.Entity);
			GripSpecificStats stats = this.GetGripSpecificStats(mainHand, byPlayer.Entity);
			if (stats == null)
			{
				return false;
			}
			StanceBasedMeleeWeaponGripStats stats2 = stats.Stats;
			float canChangeGrip = (stats2 != null) ? stats2.GripLengthFactor : 0f;
			if (canChangeGrip != 0f && this.Stats != null)
			{
				GripController gripController = stats.GripController;
				if (gripController != null)
				{
					gripController.ChangeGrip(delta, mainHand, canChangeGrip, stats.Stats.GripMinLength, stats.Stats.GripMaxLength);
				}
				return true;
			}
			GripController gripController2 = stats.GripController;
			if (gripController2 != null)
			{
				gripController2.ResetGrip(mainHand);
			}
			return false;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0002EC30 File Offset: 0x0002CE30
		private bool IsRightMouseDown(IClientPlayer byPlayer)
		{
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior == null || !playerBehavior.ActionListener.IsActive(10))
			{
				ActionsManagerPlayerBehavior playerBehavior2 = this.PlayerBehavior;
				if ((playerBehavior2 == null || !playerBehavior2.ActionListener.IsActive(16)) && !byPlayer.Entity.Controls.RightMouseDown && !byPlayer.Entity.ServerControls.RightMouseDown && !this.Api.Input.InWorldMouseButton.Right)
				{
					return this.Api.Input.MouseButton.Right;
				}
			}
			return true;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0002ECC5 File Offset: 0x0002CEC5
		private static bool IsMainHandSlot(ItemSlot slot, EntityPlayer player)
		{
			return slot != player.LeftHandItemSlot;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0002ECD4 File Offset: 0x0002CED4
		public virtual void OnGameTick(ItemSlot slot, EntityPlayer player, ref int state, bool mainHand)
		{
			GripSpecificStats stats = this.GetGripSpecificStats(mainHand, player);
			if (((stats != null) ? stats.CurrentAttack : null) == null && ((stats != null) ? stats.CurrentHandleAttack : null) == null)
			{
				return;
			}
			switch (this.GetState<StanceBasedMeleeWeaponState>(mainHand))
			{
			case StanceBasedMeleeWeaponState.Idle:
			case StanceBasedMeleeWeaponState.WindUp:
			case StanceBasedMeleeWeaponState.Cooldown:
				break;
			case StanceBasedMeleeWeaponState.Attack:
				this.TryAttack(stats.CurrentAttack, stats.CurrentHandleAttack, stats, slot, player, mainHand);
				break;
			default:
				return;
			}
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0002ED3F File Offset: 0x0002CF3F
		public void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x0002ED41 File Offset: 0x0002CF41
		public bool RestrictRightHandAction()
		{
			return !this.CheckState<StanceBasedMeleeWeaponState>(false, new StanceBasedMeleeWeaponState[1]);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0002ED53 File Offset: 0x0002CF53
		public bool RestrictLeftHandAction()
		{
			return !this.CheckState<StanceBasedMeleeWeaponState>(true, new StanceBasedMeleeWeaponState[1]);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x0002ED68 File Offset: 0x0002CF68
		[ActionEventHandler(9, ActionState.Active)]
		protected virtual bool LeftClickAttack(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (eventData.AltPressed)
			{
				return false;
			}
			if (!mainHand && this.CanLeftClickAttackWithOtherHand(player, mainHand))
			{
				return false;
			}
			if (StanceBasedMeleeWeaponClient.ActionRestricted(player, mainHand))
			{
				return false;
			}
			if (this.GetState<StanceBasedMeleeWeaponState>(mainHand) != StanceBasedMeleeWeaponState.Idle)
			{
				return false;
			}
			GripSpecificStats stats = this.GetGripSpecificStats(mainHand, player);
			if (stats == null)
			{
				return false;
			}
			MeleeAttack attack = stats.LeftClickAttacks[new ValueTuple<AttackDirection, AttackDirection>(stats.CurrentStance, direction)];
			MeleeAttack handle = stats.LeftClickHandleAttacks[new ValueTuple<AttackDirection, AttackDirection>(stats.CurrentStance, direction)];
			StanceBasedMeleeWeaponAttackStats attackStats = stats.LeftClickAttackStats[new ValueTuple<AttackDirection, AttackDirection>(stats.CurrentStance, direction)];
			stats.CurrentAttack = attack;
			stats.CurrentHandleAttack = handle;
			stats.CurrentAttackStats = attackStats;
			this.DirectionsType = stats.DirectionsType;
			if (stats.CurrentStance != direction)
			{
				stats.AttackCounter = 0;
			}
			string[] animations = stats.LeftClickAttacksAnimations[new ValueTuple<AttackDirection, AttackDirection>(stats.CurrentStance, direction)];
			string attackAnimation = animations[stats.AttackCounter % animations.Length];
			GripSpecificStats stats2 = stats;
			int attackCounter = stats2.AttackCounter;
			stats2.AttackCounter = attackCounter + 1;
			this.MeleeBlockSystem.StopBlock(mainHand);
			this.SetState<StanceBasedMeleeWeaponState>(StanceBasedMeleeWeaponState.WindUp, mainHand);
			attack.Start(player.Player);
			if (handle != null)
			{
				handle.Start(player.Player);
			}
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				bool mainHand2 = mainHand;
				string animation = attackAnimation;
				float animationSpeed = this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, 0.5f, 2f);
				animationBehavior.Play(mainHand2, animation, StanceBasedMeleeWeaponClient.AnimationCategory(mainHand), animationSpeed, 1f, () => this.LeftClickAttackCallback(mainHand, player, direction), delegate(string code)
				{
					this.LeftClickAttackCallbackHandler(code, mainHand, stats, direction);
				}, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				bool mainHand3 = mainHand;
				string animation2 = attackAnimation;
				float animationSpeed = this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, 0.5f, 2f);
				tpAnimationBehavior.Play(mainHand3, animation2, StanceBasedMeleeWeaponClient.AnimationCategory(mainHand), animationSpeed, 1f, true);
			}
			stats.HandleHitTerrain = false;
			return true;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0002F02C File Offset: 0x0002D22C
		protected virtual bool LeftClickAttackCallback(bool mainHand, EntityPlayer player, AttackDirection direction)
		{
			this.SetState<StanceBasedMeleeWeaponState>(StanceBasedMeleeWeaponState.Idle, mainHand);
			return true;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x0002F038 File Offset: 0x0002D238
		protected virtual void LeftClickAttackCallbackHandler(string callbackCode, bool mainHand, GripSpecificStats stats, AttackDirection stance)
		{
			if (callbackCode == "start")
			{
				this.SetState<StanceBasedMeleeWeaponState>(StanceBasedMeleeWeaponState.Attack, mainHand);
				return;
			}
			if (!(callbackCode == "stop"))
			{
				if (!(callbackCode == "startParry"))
				{
					if (callbackCode == "stopParry")
					{
						this.MeleeBlockSystem.StopBlock(mainHand);
						return;
					}
					if (!(callbackCode == "ready"))
					{
						return;
					}
					this.SetState<StanceBasedMeleeWeaponState>(StanceBasedMeleeWeaponState.Idle, mainHand);
					stats.CurrentStance = stance;
					if (((stats != null) ? stats.Blocks[stats.CurrentStance] : null) != null)
					{
						this.MeleeBlockSystem.StartBlock(stats.Blocks[stats.CurrentStance], mainHand, EnumDamageBlockKind.Block);
					}
				}
				else
				{
					DamageBlockJson parry = stats.LeftClickParries[new ValueTuple<AttackDirection, AttackDirection>(stats.CurrentStance, stance)];
					if (parry != null)
					{
						this.MeleeBlockSystem.StartBlock(parry, mainHand, EnumDamageBlockKind.Parry);
						return;
					}
				}
				return;
			}
			this.SetState<StanceBasedMeleeWeaponState>(StanceBasedMeleeWeaponState.Cooldown, mainHand);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x0002F118 File Offset: 0x0002D318
		[ActionEventHandler(10, ActionState.Active)]
		protected virtual bool RightClickAttack(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (GrindingWheelCompat.PlayerTriesToUseGrindingWheel(player, slot, eventData, mainHand))
			{
				return false;
			}
			if (eventData.AltPressed)
			{
				return false;
			}
			if (!mainHand && this.CanLeftClickAttackWithOtherHand(player, mainHand))
			{
				return false;
			}
			if (StanceBasedMeleeWeaponClient.ActionRestricted(player, mainHand))
			{
				return false;
			}
			if (this.GetState<StanceBasedMeleeWeaponState>(mainHand) != StanceBasedMeleeWeaponState.Idle)
			{
				return false;
			}
			GripSpecificStats stats = this.GetGripSpecificStats(mainHand, player);
			if (stats == null)
			{
				return false;
			}
			MeleeAttack attack = stats.RightClickAttacks[new ValueTuple<AttackDirection, AttackDirection>(stats.CurrentStance, direction)];
			MeleeAttack handle = stats.RightClickHandleAttacks[new ValueTuple<AttackDirection, AttackDirection>(stats.CurrentStance, direction)];
			StanceBasedMeleeWeaponAttackStats attackStats = stats.RightClickAttackStats[new ValueTuple<AttackDirection, AttackDirection>(stats.CurrentStance, direction)];
			stats.CurrentAttack = attack;
			stats.CurrentHandleAttack = handle;
			stats.CurrentAttackStats = attackStats;
			this.DirectionsType = stats.DirectionsType;
			if (stats.CurrentStance != direction)
			{
				stats.AttackCounter = 0;
			}
			string[] animations = stats.RightClickAttacksAnimations[new ValueTuple<AttackDirection, AttackDirection>(stats.CurrentStance, direction)];
			string attackAnimation = animations[stats.AttackCounter % animations.Length];
			GripSpecificStats stats2 = stats;
			int attackCounter = stats2.AttackCounter;
			stats2.AttackCounter = attackCounter + 1;
			this.MeleeBlockSystem.StopBlock(mainHand);
			this.SetState<StanceBasedMeleeWeaponState>(StanceBasedMeleeWeaponState.WindUp, mainHand);
			attack.Start(player.Player);
			if (handle != null)
			{
				handle.Start(player.Player);
			}
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				bool mainHand2 = mainHand;
				string animation = attackAnimation;
				float animationSpeed = this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, 0.5f, 2f);
				animationBehavior.Play(mainHand2, animation, StanceBasedMeleeWeaponClient.AnimationCategory(mainHand), animationSpeed, 1f, () => this.RightClickAttackCallback(mainHand, player, direction), delegate(string code)
				{
					this.RightClickAttackCallbackHandler(code, mainHand, stats, direction);
				}, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				bool mainHand3 = mainHand;
				string animation2 = attackAnimation;
				float animationSpeed = this.GetAnimationSpeed(player, this.Stats.ProficiencyStat, 0.5f, 2f);
				tpAnimationBehavior.Play(mainHand3, animation2, StanceBasedMeleeWeaponClient.AnimationCategory(mainHand), animationSpeed, 1f, true);
			}
			stats.HandleHitTerrain = false;
			return true;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x0002F3F4 File Offset: 0x0002D5F4
		protected virtual bool RightClickAttackCallback(bool mainHand, EntityPlayer player, AttackDirection direction)
		{
			this.SetState<StanceBasedMeleeWeaponState>(StanceBasedMeleeWeaponState.Idle, mainHand);
			return true;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0002F400 File Offset: 0x0002D600
		protected virtual void RightClickAttackCallbackHandler(string callbackCode, bool mainHand, GripSpecificStats stats, AttackDirection stance)
		{
			if (callbackCode == "start")
			{
				this.SetState<StanceBasedMeleeWeaponState>(StanceBasedMeleeWeaponState.Attack, mainHand);
				return;
			}
			if (!(callbackCode == "stop"))
			{
				if (!(callbackCode == "startParry"))
				{
					if (callbackCode == "stopParry")
					{
						this.MeleeBlockSystem.StopBlock(mainHand);
						return;
					}
					if (!(callbackCode == "ready"))
					{
						return;
					}
					this.SetState<StanceBasedMeleeWeaponState>(StanceBasedMeleeWeaponState.Idle, mainHand);
					stats.CurrentStance = stance;
					if (((stats != null) ? stats.Blocks[stats.CurrentStance] : null) != null)
					{
						this.MeleeBlockSystem.StartBlock(stats.Blocks[stats.CurrentStance], mainHand, EnumDamageBlockKind.Block);
					}
				}
				else
				{
					DamageBlockJson parry = stats.RightClickParries[new ValueTuple<AttackDirection, AttackDirection>(stats.CurrentStance, stance)];
					if (parry != null)
					{
						this.MeleeBlockSystem.StartBlock(parry, mainHand, EnumDamageBlockKind.Parry);
						return;
					}
				}
				return;
			}
			this.SetState<StanceBasedMeleeWeaponState>(StanceBasedMeleeWeaponState.Cooldown, mainHand);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x0002F4E0 File Offset: 0x0002D6E0
		protected virtual void StanceAnimationCallbackHandler(string callbackCode, bool mainHand, GripSpecificStats stats, AttackDirection stance)
		{
			if (!(callbackCode == "startBlock"))
			{
				if (callbackCode == "stopBlock")
				{
					this.MeleeBlockSystem.StopBlock(mainHand);
					return;
				}
				if (!(callbackCode == "ready"))
				{
					return;
				}
				this.SetState<StanceBasedMeleeWeaponState>(StanceBasedMeleeWeaponState.Idle, mainHand);
			}
			else if (stats.Blocks[stance] != null)
			{
				this.MeleeBlockSystem.StartBlock(stats.Blocks[stance], mainHand, EnumDamageBlockKind.Block);
				return;
			}
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x0002F554 File Offset: 0x0002D754
		protected virtual void TryAttack([Nullable(2)] MeleeAttack attack, [Nullable(2)] MeleeAttack handle, GripSpecificStats stats, ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			StanceBasedMeleeWeaponAttackStats attackStats = stats.CurrentAttackStats;
			ItemStackMeleeWeaponStats stackStats = ItemStackMeleeWeaponStats.FromItemStack(slot.Itemstack);
			if (handle != null)
			{
				IEnumerable<ValueTuple<Block, Vector3d>> handleTerrainCollision;
				IEnumerable<ValueTuple<Entity, Vector3d>> handleEntitiesCollision;
				handle.Attack(player.Player, slot, mainHand, out handleTerrainCollision, out handleEntitiesCollision, stackStats, AttackDirection.Top, this.Settings.MeleeWeaponStopOnTerrainHit);
				bool handleHitTerrain = Enumerable.Any<ValueTuple<Block, Vector3d>>(handleTerrainCollision);
				if (handleHitTerrain && this.Settings.MeleeWeaponIgnoreTerrainBehind)
				{
					handleHitTerrain = !StanceBasedMeleeWeaponClient.HasTerrainCollisionBehindPlayer(player, handleTerrainCollision);
				}
				if (!stats.HandleHitTerrain && handleHitTerrain)
				{
					if (attackStats != null && attackStats.HandleHitSound != null)
					{
						this.SoundsSystem.Play(attackStats.HandleHitSound, false, 32f, 1f, true);
					}
					stats.HandleHitTerrain = true;
				}
				if (handleHitTerrain)
				{
					return;
				}
			}
			if (attack == null)
			{
				return;
			}
			IEnumerable<ValueTuple<Block, Vector3d>> terrainCollision;
			IEnumerable<ValueTuple<Entity, Vector3d>> entitiesCollision;
			attack.Attack(player.Player, slot, mainHand, out terrainCollision, out entitiesCollision, stackStats, AttackDirection.Top, this.Settings.MeleeWeaponStopOnTerrainHit);
			if (handle != null)
			{
				handle.AddAttackedEntities(attack);
			}
			if (Enumerable.Any<ValueTuple<Entity, Vector3d>>(entitiesCollision) && attackStats != null && attackStats.AttackHitSound != null)
			{
				this.SoundsSystem.Play(attackStats.AttackHitSound, false, 32f, 1f, true);
			}
			if (Enumerable.Any<ValueTuple<Entity, Vector3d>>(entitiesCollision) && this.Stats.AnimationStaggerOnHitDurationMs > 0f)
			{
				FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
				if (animationBehavior == null)
				{
					return;
				}
				animationBehavior.SetSpeedModifier(new AnimationSpeedModifierDelegate(this.AttackImpactFunction));
			}
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0002F6A0 File Offset: 0x0002D8A0
		private static bool HasTerrainCollisionBehindPlayer(EntityPlayer player, [TupleElementNames(new string[]
		{
			"block",
			"point"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1
		})] IEnumerable<ValueTuple<Block, Vector3d>> terrainCollision)
		{
			Vector3d playerPosition = player.Pos.XYZ.ToOpenTK();
			Vector3d eyesPosition = player.LocalEyePos.ToOpenTK() + playerPosition;
			Vector3d viewDirection = player.Pos.GetViewVector().ToVec3d().ToOpenTK();
			double eyesProjection = Vector3d.Dot(viewDirection, eyesPosition);
			foreach (ValueTuple<Block, Vector3d> valueTuple in terrainCollision)
			{
				Vector3d point = valueTuple.Item2;
				if (Vector3d.Dot(viewDirection, point) < eyesProjection)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0002F744 File Offset: 0x0002D944
		protected static void RegisterCollider(string item, string type, MeleeAttack attack)
		{
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0002F748 File Offset: 0x0002D948
		protected float GetAnimationSpeed(Entity player, string proficiencyStat, float min = 0.5f, float max = 2f)
		{
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			float num = (playerBehavior != null) ? playerBehavior.ManipulationSpeed : 1f;
			float proficiencyBonus = (proficiencyStat == "") ? 0f : (player.Stats.GetBlended(proficiencyStat) - 1f);
			return Math.Clamp(num + proficiencyBonus, min, max);
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0002F79C File Offset: 0x0002D99C
		protected bool CheckState<[Nullable(2)] TState>(bool mainHand, params TState[] statesToCheck)
		{
			ReadOnlySpan<TState> readOnlySpan = statesToCheck;
			Type typeFromHandle = typeof(TState);
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			return MemoryExtensions.Contains<TState>(readOnlySpan, (TState)((object)Enum.ToObject(typeFromHandle, (playerBehavior != null) ? playerBehavior.GetState(mainHand) : 0)), null);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0002F7D1 File Offset: 0x0002D9D1
		protected void SetState<[Nullable(2)] TState>(TState state, bool mainHand)
		{
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior == null)
			{
				return;
			}
			playerBehavior.SetState((int)Enum.ToObject(typeof(TState), state), mainHand);
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0002F7FE File Offset: 0x0002D9FE
		protected TState GetState<[Nullable(2)] TState>(bool mainHand)
		{
			Type typeFromHandle = typeof(TState);
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			return (TState)((object)Enum.ToObject(typeFromHandle, (playerBehavior != null) ? playerBehavior.GetState(mainHand) : 0));
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0002F827 File Offset: 0x0002DA27
		protected static string AnimationCategory(bool mainHand = true)
		{
			if (!mainHand)
			{
				return "mainOffhand";
			}
			return "main";
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0002F837 File Offset: 0x0002DA37
		protected GripType GetGripType(bool mainHand, EntityPlayer player)
		{
			if (!mainHand)
			{
				return GripType.OffHanded;
			}
			if (player.LeftHandItemSlot.Empty)
			{
				return GripType.TwoHanded;
			}
			return GripType.OneHanded;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0002F850 File Offset: 0x0002DA50
		[return: Nullable(2)]
		public GripSpecificStats GetGripSpecificStats(bool mainHand, EntityPlayer player)
		{
			GripSpecificStats result;
			switch (this.GetGripType(mainHand, player))
			{
			case GripType.OneHanded:
				result = this.OneHandedStats;
				break;
			case GripType.TwoHanded:
				result = (this.TwoHandedStats ?? this.OneHandedStats);
				break;
			case GripType.OffHanded:
				result = this.OffHandStats;
				break;
			default:
				throw new Exception();
			}
			return result;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0002F8A6 File Offset: 0x0002DAA6
		protected bool CanRightClickAttackWithOtherHand(EntityPlayer player, bool mainHand = true)
		{
			ItemStack itemstack = (mainHand ? player.LeftHandItemSlot : player.RightHandItemSlot).Itemstack;
			IHasMeleeWeaponActions hasMeleeWeaponActions = ((itemstack != null) ? itemstack.Item : null) as IHasMeleeWeaponActions;
			return hasMeleeWeaponActions != null && hasMeleeWeaponActions.CanAttack(player, !mainHand);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0002F8DF File Offset: 0x0002DADF
		protected bool CanLeftClickAttackWithOtherHand(EntityPlayer player, bool mainHand = true)
		{
			ItemStack itemstack = (mainHand ? player.LeftHandItemSlot : player.RightHandItemSlot).Itemstack;
			IHasMeleeWeaponActions hasMeleeWeaponActions = ((itemstack != null) ? itemstack.Item : null) as IHasMeleeWeaponActions;
			return hasMeleeWeaponActions != null && hasMeleeWeaponActions.CanBlock(player, !mainHand);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0002F918 File Offset: 0x0002DB18
		protected virtual bool AttackImpactFunction(TimeSpan duration, ref TimeSpan delta)
		{
			TimeSpan totalDuration = TimeSpan.FromMilliseconds((double)this.Stats.AnimationStaggerOnHitDurationMs);
			delta = TimeSpan.Zero;
			return duration < totalDuration;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0002F94C File Offset: 0x0002DB4C
		protected static bool ActionRestricted(EntityPlayer player, bool mainHand = true)
		{
			if (mainHand)
			{
				ItemStack itemstack = player.LeftHandItemSlot.Itemstack;
				IRestrictAction restrictAction = ((itemstack != null) ? itemstack.Item : null) as IRestrictAction;
				return restrictAction != null && restrictAction.RestrictRightHandAction();
			}
			ItemStack itemstack2 = player.RightHandItemSlot.Itemstack;
			IRestrictAction restrictAction2 = ((itemstack2 != null) ? itemstack2.Item : null) as IRestrictAction;
			return restrictAction2 != null && restrictAction2.RestrictLeftHandAction();
		}

		// Token: 0x040003A6 RID: 934
		protected readonly Item Item;

		// Token: 0x040003A7 RID: 935
		protected readonly ICoreClientAPI Api;

		// Token: 0x040003A8 RID: 936
		protected readonly StanceBasedMeleeWeaponStats Stats;

		// Token: 0x040003A9 RID: 937
		protected readonly MeleeBlockSystemClient MeleeBlockSystem;

		// Token: 0x040003AA RID: 938
		[Nullable(2)]
		protected FirstPersonAnimationsBehavior AnimationBehavior;

		// Token: 0x040003AB RID: 939
		[Nullable(2)]
		protected ThirdPersonAnimationsBehavior TpAnimationBehavior;

		// Token: 0x040003AC RID: 940
		[Nullable(2)]
		protected ActionsManagerPlayerBehavior PlayerBehavior;

		// Token: 0x040003AD RID: 941
		protected SoundsSynchronizerClient SoundsSystem;

		// Token: 0x040003AE RID: 942
		protected Settings Settings;

		// Token: 0x040003AF RID: 943
		protected const string PlayerStatsMainHandCategory = "CombatOverhaul:held-item-mainhand";

		// Token: 0x040003B0 RID: 944
		protected const string PlayerStatsOffHandCategory = "CombatOverhaul:held-item-offhand";

		// Token: 0x040003B1 RID: 945
		[Nullable(2)]
		protected readonly GripSpecificStats OneHandedStats;

		// Token: 0x040003B2 RID: 946
		[Nullable(2)]
		protected readonly GripSpecificStats TwoHandedStats;

		// Token: 0x040003B3 RID: 947
		[Nullable(2)]
		protected readonly GripSpecificStats OffHandStats;
	}
}
