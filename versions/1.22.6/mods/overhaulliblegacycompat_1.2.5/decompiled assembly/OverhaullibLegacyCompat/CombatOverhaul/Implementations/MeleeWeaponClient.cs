using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Animations;
using CombatOverhaul.Colliders;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.Inputs;
using CombatOverhaul.MeleeSystems;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.RangedSystems.Aiming;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.Common;
using Vintagestory.GameContent;

namespace CombatOverhaul.Implementations
{
	// Token: 0x0200007D RID: 125
	[NullableContext(1)]
	[Nullable(0)]
	public class MeleeWeaponClient : IClientWeaponLogic, IHasDynamicMoveAnimations, IHasDynamicIdleAnimations, IOnGameTick, IRestrictAction
	{
		// Token: 0x060005DF RID: 1503 RVA: 0x00022128 File Offset: 0x00020328
		public MeleeWeaponClient(ICoreClientAPI api, Item item, MeleeWeaponStats stats)
		{
			this.Item = item;
			this.Api = api;
			CombatOverhaulSystem system = api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			MeleeSystemClient clientMeleeSystem = system.ClientMeleeSystem;
			if (clientMeleeSystem == null)
			{
				throw new Exception();
			}
			this.MeleeAttackSystem = clientMeleeSystem;
			this.ImpaleSystem = system.ClientImpaleSystem;
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
			RangedWeaponSystemClient clientRangedWeaponSystem = system.ClientRangedWeaponSystem;
			if (clientRangedWeaponSystem == null)
			{
				throw new Exception();
			}
			this.RangedWeaponSystem = clientRangedWeaponSystem;
			ClientAimingSystem aimingSystem = system.AimingSystem;
			if (aimingSystem == null)
			{
				throw new Exception();
			}
			this.AimingSystem = aimingSystem;
			this.Settings = system.Settings;
			this.Stats = stats;
			ThrowWeaponStats throwAttack = this.Stats.ThrowAttack;
			this.AimingStats = ((throwAttack != null) ? throwAttack.Aiming.ToStats() : null);
			if (this.AimingStats != null)
			{
				this.DefaultVerticalLimit = this.AimingStats.VerticalLimit;
				this.DefaultHorizontalLimit = this.AimingStats.HorizontalLimit;
			}
			this.InitializeConfiguredAttacks(api, item.Code.ToString());
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x000222CC File Offset: 0x000204CC
		private void InitializeConfiguredAttacks(ICoreClientAPI api, string itemCode)
		{
			this.InitializeStanceAttacks(api, itemCode, this.Stats.OneHandedStance, "onehanded", ref this.OneHandedAttack, ref this.OneHandedRiposte, out this.DirectionalOneHandedAttacks);
			this.InitializeStanceAttacks(api, itemCode, this.Stats.TwoHandedStance, "twohanded", ref this.TwoHandedAttack, ref this.TwoHandedRiposte, out this.DirectionalTwoHandedAttacks);
			this.InitializeStanceAttacks(api, itemCode, this.Stats.OffHandStance, "offhand", ref this.OffHandAttack, ref this.OffHandRiposte, out this.DirectionalOffHandAttacks);
			this.InitializeDualWieldAttacks(api, this.Stats.MainHandDualWieldStances, this.MainHandDualWieldAttacks, this.DirectionalMainHandDualWieldAttacks);
			this.InitializeDualWieldAttacks(api, this.Stats.OffHandDualWieldStances, this.OffHandDualWieldAttacks, this.DirectionalOffHandDualWieldAttacks);
			this.InitializeHandleAttack(api, itemCode, this.Stats.OneHandedStance, "onehanded-handle-", ref this.OneHandedHandleAttack);
			this.InitializeHandleAttack(api, itemCode, this.Stats.TwoHandedStance, "twohanded-handle-", ref this.TwoHandedHandleAttack);
			this.InitializeHandleAttack(api, itemCode, this.Stats.OffHandStance, "offhand-handle-", ref this.OffHandHandleAttack);
			this.InitializeDualWieldHandles(api, this.Stats.MainHandDualWieldStances, this.MainHandDualWieldHandleAttacks);
			this.InitializeDualWieldHandles(api, this.Stats.OffHandDualWieldStances, this.OffHandDualWieldHandleAttacks);
			this.InitializeBlockBashes(api, itemCode, this.Stats.OneHandedStance, "onehanded-blockbash-", ref this.OneHandedBlockBash, out this.DirectionalOneHandedBlockBashes);
			this.InitializeBlockBashes(api, itemCode, this.Stats.TwoHandedStance, "twohanded-blockbash-", ref this.TwoHandedBlockBash, out this.DirectionalTwoHandedBlockBashes);
			this.InitializeBlockBashes(api, itemCode, this.Stats.OffHandStance, "offhand-blockbash-", ref this.OffHandBlockBash, out this.DirectionalOffHandBlockBashes);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0002248C File Offset: 0x0002068C
		private void InitializeStanceAttacks(ICoreClientAPI api, string itemCode, [Nullable(2)] StanceStats stance, string colliderPrefix, [Nullable(2)] ref MeleeAttack attack, [Nullable(2)] ref MeleeAttack riposte, [Nullable(new byte[]
		{
			2,
			1
		})] out Dictionary<AttackDirection, MeleeAttack> directionalAttacks)
		{
			directionalAttacks = null;
			if (((stance != null) ? stance.Attack : null) != null)
			{
				attack = new MeleeAttack(api, stance.Attack);
				MeleeWeaponClient.RegisterCollider(itemCode, colliderPrefix + "-", attack);
			}
			if (((stance != null) ? stance.Riposte : null) != null)
			{
				riposte = new MeleeAttack(api, stance.Riposte);
				MeleeWeaponClient.RegisterCollider(itemCode, colliderPrefix + "-riposte-", riposte);
				return;
			}
			if (((stance != null) ? stance.DirectionalAttacks : null) != null)
			{
				directionalAttacks = this.CreateDirectionalAttacks(api, itemCode, colliderPrefix + "-", stance.DirectionalAttacks, true);
			}
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00022530 File Offset: 0x00020730
		private void InitializeBlockBashes(ICoreClientAPI api, string itemCode, [Nullable(2)] StanceStats stance, string colliderPrefix, [Nullable(2)] ref MeleeAttack blockBash, [Nullable(new byte[]
		{
			2,
			1
		})] out Dictionary<AttackDirection, MeleeAttack> directionalBlockBashes)
		{
			directionalBlockBashes = null;
			if (((stance != null) ? stance.BlockBash : null) != null)
			{
				blockBash = new MeleeAttack(api, stance.BlockBash);
				MeleeWeaponClient.RegisterCollider(itemCode, colliderPrefix, blockBash);
				return;
			}
			if (((stance != null) ? stance.DirectionalBlockBashes : null) != null)
			{
				directionalBlockBashes = this.CreateDirectionalAttacks(api, itemCode, colliderPrefix, stance.DirectionalBlockBashes, true);
			}
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0002258C File Offset: 0x0002078C
		private void InitializeHandleAttack(ICoreClientAPI api, string itemCode, [Nullable(2)] StanceStats stance, string colliderPrefix, [Nullable(2)] ref MeleeAttack handleAttack)
		{
			if (((stance != null) ? stance.HandleAttack : null) == null)
			{
				return;
			}
			handleAttack = new MeleeAttack(api, stance.HandleAttack);
			MeleeWeaponClient.RegisterCollider(itemCode, colliderPrefix, handleAttack);
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x000225B8 File Offset: 0x000207B8
		private void InitializeDualWieldAttacks(ICoreClientAPI api, Dictionary<string, StanceStats> stances, Dictionary<string, MeleeAttack> attacks, Dictionary<string, Dictionary<AttackDirection, MeleeAttack>> directionalAttacks)
		{
			foreach (KeyValuePair<string, StanceStats> keyValuePair in stances)
			{
				string text;
				StanceStats stanceStats;
				keyValuePair.Deconstruct(ref text, ref stanceStats);
				string wildcard = text;
				StanceStats stance = stanceStats;
				if (stance.Attack != null)
				{
					attacks.Add(wildcard, new MeleeAttack(api, stance.Attack));
				}
				if (stance.DirectionalAttacks != null)
				{
					directionalAttacks[wildcard] = this.CreateDirectionalAttacks(api, "", "", stance.DirectionalAttacks, false);
				}
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00022654 File Offset: 0x00020854
		private void InitializeDualWieldHandles(ICoreClientAPI api, Dictionary<string, StanceStats> stances, Dictionary<string, MeleeAttack> handleAttacks)
		{
			foreach (KeyValuePair<string, StanceStats> keyValuePair in stances)
			{
				string text;
				StanceStats stanceStats;
				keyValuePair.Deconstruct(ref text, ref stanceStats);
				string wildcard = text;
				StanceStats stance = stanceStats;
				if (stance.HandleAttack != null)
				{
					handleAttacks.Add(wildcard, new MeleeAttack(api, stance.HandleAttack));
				}
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x000226C8 File Offset: 0x000208C8
		private Dictionary<AttackDirection, MeleeAttack> CreateDirectionalAttacks(ICoreClientAPI api, string itemCode, string colliderPrefix, Dictionary<string, MeleeAttackStats> attacks, bool registerColliders)
		{
			Dictionary<AttackDirection, MeleeAttack> result = new Dictionary<AttackDirection, MeleeAttack>();
			foreach (KeyValuePair<string, MeleeAttackStats> keyValuePair in attacks)
			{
				string text;
				MeleeAttackStats meleeAttackStats;
				keyValuePair.Deconstruct(ref text, ref meleeAttackStats);
				string direction = text;
				MeleeAttackStats attackStats = meleeAttackStats;
				AttackDirection attackDirection = Enum.Parse<AttackDirection>(direction);
				MeleeAttack attack = new MeleeAttack(api, attackStats);
				result.Add(attackDirection, attack);
				if (registerColliders)
				{
					MeleeWeaponClient.RegisterCollider(itemCode, colliderPrefix + direction + "-", attack);
				}
			}
			return result;
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x0002275C File Offset: 0x0002095C
		// (set) Token: 0x060005E8 RID: 1512 RVA: 0x00022764 File Offset: 0x00020964
		public bool Active { get; set; } = true;

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x0002276D File Offset: 0x0002096D
		public int ItemId
		{
			get
			{
				return this.Item.Id;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x0002277A File Offset: 0x0002097A
		// (set) Token: 0x060005EB RID: 1515 RVA: 0x00022781 File Offset: 0x00020981
		public static long GlobalCooldownUntilMs { get; set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x00022789 File Offset: 0x00020989
		// (set) Token: 0x060005ED RID: 1517 RVA: 0x00022791 File Offset: 0x00020991
		public virtual DirectionsConfiguration DirectionsType { get; protected set; } = DirectionsConfiguration.None;

		// Token: 0x060005EE RID: 1518 RVA: 0x0002279C File Offset: 0x0002099C
		public AnimationRequestByCode? GetIdleAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			this.EnsureStance(player, mainHand);
			AnimationRequestByCode? result;
			switch (this.GetStance<MeleeWeaponStance>(mainHand))
			{
			case MeleeWeaponStance.MainHand:
			{
				MeleeWeaponStats stats = this.Stats;
				bool flag;
				if (stats == null)
				{
					flag = (null != null);
				}
				else
				{
					StanceStats oneHandedStance = stats.OneHandedStance;
					flag = (((oneHandedStance != null) ? oneHandedStance.IdleAnimation : null) != null);
				}
				result = ((!flag) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.OneHandedStance.IdleAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			case MeleeWeaponStance.OffHand:
			{
				MeleeWeaponStats stats2 = this.Stats;
				bool flag2;
				if (stats2 == null)
				{
					flag2 = (null != null);
				}
				else
				{
					StanceStats offHandStance = stats2.OffHandStance;
					flag2 = (((offHandStance != null) ? offHandStance.IdleAnimation : null) != null);
				}
				result = ((!flag2) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.OffHandStance.IdleAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			case MeleeWeaponStance.TwoHanded:
			{
				MeleeWeaponStats stats3 = this.Stats;
				bool flag3;
				if (stats3 == null)
				{
					flag3 = (null != null);
				}
				else
				{
					StanceStats twoHandedStance = stats3.TwoHandedStance;
					flag3 = (((twoHandedStance != null) ? twoHandedStance.IdleAnimation : null) != null);
				}
				result = ((!flag3) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.TwoHandedStance.IdleAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			default:
				result = default(AnimationRequestByCode?);
				break;
			}
			return result;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00022948 File Offset: 0x00020B48
		public AnimationRequestByCode? GetReadyAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			this.EnsureStance(player, mainHand);
			AnimationRequestByCode? result;
			switch (this.GetStance<MeleeWeaponStance>(mainHand))
			{
			case MeleeWeaponStance.MainHand:
			{
				MeleeWeaponStats stats = this.Stats;
				bool flag;
				if (stats == null)
				{
					flag = (null != null);
				}
				else
				{
					StanceStats oneHandedStance = stats.OneHandedStance;
					flag = (((oneHandedStance != null) ? oneHandedStance.ReadyAnimation : null) != null);
				}
				result = ((!flag) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.OneHandedStance.ReadyAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			case MeleeWeaponStance.OffHand:
			{
				MeleeWeaponStats stats2 = this.Stats;
				bool flag2;
				if (stats2 == null)
				{
					flag2 = (null != null);
				}
				else
				{
					StanceStats offHandStance = stats2.OffHandStance;
					flag2 = (((offHandStance != null) ? offHandStance.ReadyAnimation : null) != null);
				}
				result = ((!flag2) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.OffHandStance.ReadyAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			case MeleeWeaponStance.TwoHanded:
			{
				MeleeWeaponStats stats3 = this.Stats;
				bool flag3;
				if (stats3 == null)
				{
					flag3 = (null != null);
				}
				else
				{
					StanceStats twoHandedStance = stats3.TwoHandedStance;
					flag3 = (((twoHandedStance != null) ? twoHandedStance.ReadyAnimation : null) != null);
				}
				result = ((!flag3) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.TwoHandedStance.ReadyAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			default:
				result = default(AnimationRequestByCode?);
				break;
			}
			return result;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00022AF4 File Offset: 0x00020CF4
		public AnimationRequestByCode? GetWalkAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			this.EnsureStance(player, mainHand);
			AnimationRequestByCode? result;
			switch (this.GetStance<MeleeWeaponStance>(mainHand))
			{
			case MeleeWeaponStance.MainHand:
			{
				MeleeWeaponStats stats = this.Stats;
				bool flag;
				if (stats == null)
				{
					flag = (null != null);
				}
				else
				{
					StanceStats oneHandedStance = stats.OneHandedStance;
					flag = (((oneHandedStance != null) ? oneHandedStance.WalkAnimation : null) != null);
				}
				result = ((!flag) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.OneHandedStance.WalkAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			case MeleeWeaponStance.OffHand:
			{
				MeleeWeaponStats stats2 = this.Stats;
				bool flag2;
				if (stats2 == null)
				{
					flag2 = (null != null);
				}
				else
				{
					StanceStats offHandStance = stats2.OffHandStance;
					flag2 = (((offHandStance != null) ? offHandStance.WalkAnimation : null) != null);
				}
				result = ((!flag2) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.OffHandStance.WalkAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			case MeleeWeaponStance.TwoHanded:
			{
				MeleeWeaponStats stats3 = this.Stats;
				bool flag3;
				if (stats3 == null)
				{
					flag3 = (null != null);
				}
				else
				{
					StanceStats twoHandedStance = stats3.TwoHandedStance;
					flag3 = (((twoHandedStance != null) ? twoHandedStance.WalkAnimation : null) != null);
				}
				result = ((!flag3) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.TwoHandedStance.WalkAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			default:
				result = default(AnimationRequestByCode?);
				break;
			}
			return result;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00022CA0 File Offset: 0x00020EA0
		public AnimationRequestByCode? GetRunAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			this.EnsureStance(player, mainHand);
			AnimationRequestByCode? result;
			switch (this.GetStance<MeleeWeaponStance>(mainHand))
			{
			case MeleeWeaponStance.MainHand:
			{
				MeleeWeaponStats stats = this.Stats;
				bool flag;
				if (stats == null)
				{
					flag = (null != null);
				}
				else
				{
					StanceStats oneHandedStance = stats.OneHandedStance;
					flag = (((oneHandedStance != null) ? oneHandedStance.RunAnimation : null) != null);
				}
				result = ((!flag) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.OneHandedStance.RunAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			case MeleeWeaponStance.OffHand:
			{
				MeleeWeaponStats stats2 = this.Stats;
				bool flag2;
				if (stats2 == null)
				{
					flag2 = (null != null);
				}
				else
				{
					StanceStats offHandStance = stats2.OffHandStance;
					flag2 = (((offHandStance != null) ? offHandStance.RunAnimation : null) != null);
				}
				result = ((!flag2) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.OffHandStance.RunAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			case MeleeWeaponStance.TwoHanded:
			{
				MeleeWeaponStats stats3 = this.Stats;
				bool flag3;
				if (stats3 == null)
				{
					flag3 = (null != null);
				}
				else
				{
					StanceStats twoHandedStance = stats3.TwoHandedStance;
					flag3 = (((twoHandedStance != null) ? twoHandedStance.RunAnimation : null) != null);
				}
				result = ((!flag3) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.TwoHandedStance.RunAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			default:
				result = default(AnimationRequestByCode?);
				break;
			}
			return result;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00022E4C File Offset: 0x0002104C
		public AnimationRequestByCode? GetSwimAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			this.EnsureStance(player, mainHand);
			AnimationRequestByCode? result;
			switch (this.GetStance<MeleeWeaponStance>(mainHand))
			{
			case MeleeWeaponStance.MainHand:
			{
				MeleeWeaponStats stats = this.Stats;
				bool flag;
				if (stats == null)
				{
					flag = (null != null);
				}
				else
				{
					StanceStats oneHandedStance = stats.OneHandedStance;
					flag = (((oneHandedStance != null) ? oneHandedStance.SwimAnimation : null) != null);
				}
				result = ((!flag) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.OneHandedStance.SwimAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			case MeleeWeaponStance.OffHand:
			{
				MeleeWeaponStats stats2 = this.Stats;
				bool flag2;
				if (stats2 == null)
				{
					flag2 = (null != null);
				}
				else
				{
					StanceStats offHandStance = stats2.OffHandStance;
					flag2 = (((offHandStance != null) ? offHandStance.SwimAnimation : null) != null);
				}
				result = ((!flag2) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.OffHandStance.SwimAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			case MeleeWeaponStance.TwoHanded:
			{
				MeleeWeaponStats stats3 = this.Stats;
				bool flag3;
				if (stats3 == null)
				{
					flag3 = (null != null);
				}
				else
				{
					StanceStats twoHandedStance = stats3.TwoHandedStance;
					flag3 = (((twoHandedStance != null) ? twoHandedStance.SwimAnimation : null) != null);
				}
				result = ((!flag3) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.TwoHandedStance.SwimAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			default:
				result = default(AnimationRequestByCode?);
				break;
			}
			return result;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00022FF8 File Offset: 0x000211F8
		public AnimationRequestByCode? GetSwimIdleAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			this.EnsureStance(player, mainHand);
			AnimationRequestByCode? result;
			switch (this.GetStance<MeleeWeaponStance>(mainHand))
			{
			case MeleeWeaponStance.MainHand:
			{
				MeleeWeaponStats stats = this.Stats;
				bool flag;
				if (stats == null)
				{
					flag = (null != null);
				}
				else
				{
					StanceStats oneHandedStance = stats.OneHandedStance;
					flag = (((oneHandedStance != null) ? oneHandedStance.SwimIdleAnimation : null) != null);
				}
				result = ((!flag) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.OneHandedStance.SwimIdleAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			case MeleeWeaponStance.OffHand:
			{
				MeleeWeaponStats stats2 = this.Stats;
				bool flag2;
				if (stats2 == null)
				{
					flag2 = (null != null);
				}
				else
				{
					StanceStats offHandStance = stats2.OffHandStance;
					flag2 = (((offHandStance != null) ? offHandStance.SwimIdleAnimation : null) != null);
				}
				result = ((!flag2) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.OffHandStance.SwimIdleAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			case MeleeWeaponStance.TwoHanded:
			{
				MeleeWeaponStats stats3 = this.Stats;
				bool flag3;
				if (stats3 == null)
				{
					flag3 = (null != null);
				}
				else
				{
					StanceStats twoHandedStance = stats3.TwoHandedStance;
					flag3 = (((twoHandedStance != null) ? twoHandedStance.SwimIdleAnimation : null) != null);
				}
				result = ((!flag3) ? default(AnimationRequestByCode?) : new AnimationRequestByCode?(new AnimationRequestByCode(this.Stats.TwoHandedStance.SwimIdleAnimation, 1f, 1f, MeleeWeaponClient.AnimationCategory(mainHand), TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null)));
				break;
			}
			default:
				result = default(AnimationRequestByCode?);
				break;
			}
			return result;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x000231A2 File Offset: 0x000213A2
		public virtual void OnSelected(ItemSlot slot, EntityPlayer player, bool mainHand, ref int state)
		{
			this.EnsureStance(player, mainHand);
			this.SetState<MeleeWeaponState>(MeleeWeaponState.Idle, mainHand);
			this.SetSpeedPenalty(mainHand, player);
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x000231BC File Offset: 0x000213BC
		public virtual void OnDeselected(EntityPlayer player, bool mainHand, ref int state)
		{
			if (this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[]
			{
				MeleeWeaponState.Attacking,
				MeleeWeaponState.Cooldown,
				MeleeWeaponState.Blocking,
				MeleeWeaponState.Parrying,
				MeleeWeaponState.BlockBashAttacking,
				MeleeWeaponState.BlockBashWindingUp,
				MeleeWeaponState.BlockBashCooldown
			}))
			{
				MeleeWeaponClient.SetGlobalCooldown(this.Api, (long)this.Settings.GlobalAttackCooldownMs);
			}
			if (this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[]
			{
				MeleeWeaponState.WindingUp,
				MeleeWeaponState.Attacking,
				MeleeWeaponState.Cooldown,
				MeleeWeaponState.BlockBashAttacking,
				MeleeWeaponState.BlockBashWindingUp,
				MeleeWeaponState.BlockBashCooldown
			}))
			{
				this.MeleeAttackSystem.UpdateAttackStatus(player, MeleeAttackStatus.End, mainHand);
			}
			if (this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[]
			{
				MeleeWeaponState.Aiming,
				MeleeWeaponState.StartingAim
			}))
			{
				this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndAiming, mainHand);
			}
			this.MeleeBlockSystem.StopBlock(mainHand);
			this.StopAttackCooldown(mainHand);
			this.StopBlockCooldown(mainHand);
			GripController gripController = this.GripController;
			if (gripController != null)
			{
				gripController.StopAnimation(mainHand);
			}
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.StopSpeedModifier();
			}
			ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
			if (playerActionsBehavior != null)
			{
				playerActionsBehavior.SetStat("walkspeed", mainHand ? "CombatOverhaul:held-item-mainhand" : "CombatOverhaul:held-item-offhand", 0f);
			}
			AimingAnimationController aimingAnimationController = this.AimingAnimationController;
			if (aimingAnimationController != null)
			{
				aimingAnimationController.Stop(mainHand);
			}
			this.AimingSystem.StopAiming();
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 == null)
			{
				return;
			}
			animationBehavior2.StopAllVanillaAnimations(mainHand);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x000232E0 File Offset: 0x000214E0
		public virtual void OnRegistered(ActionsManagerPlayerBehavior behavior, ICoreClientAPI api)
		{
			this.PlayerActionsBehavior = behavior;
			this.AnimationBehavior = behavior.entity.GetBehavior<FirstPersonAnimationsBehavior>();
			this.TpAnimationBehavior = behavior.entity.GetBehavior<ThirdPersonAnimationsBehavior>();
			this.GripController = new GripController(this.AnimationBehavior);
			if (this.AimingStats != null)
			{
				this.AimingAnimationController = new AimingAnimationController(this.AimingSystem, this.AnimationBehavior, this.AimingStats);
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0002334C File Offset: 0x0002154C
		public virtual void RenderDebugCollider(ItemSlot inSlot, IClientPlayer byPlayer)
		{
			if (DebugWindowManager._currentCollider == null)
			{
				MeleeAttack attack = this.GetStanceAttack(byPlayer.Entity, true, this.CurrentMainHandDirection, false);
				if (attack != null)
				{
					MeleeDamageType[] damageTypes = attack.DamageTypes;
					for (int i = 0; i < damageTypes.Length; i++)
					{
						ICollider collider = damageTypes[i].RelativeCollider.Transform(byPlayer.Entity.Pos, byPlayer.Entity, inSlot, this.Api, true);
						if (collider != null)
						{
							collider.Render(this.Api, byPlayer.Entity, -1);
						}
					}
				}
				MeleeAttack handle = this.GetStanceHandleAttack(byPlayer.Entity, true);
				if (handle != null)
				{
					MeleeDamageType[] damageTypes = handle.DamageTypes;
					for (int i = 0; i < damageTypes.Length; i++)
					{
						ICollider collider2 = damageTypes[i].RelativeCollider.Transform(byPlayer.Entity.Pos, byPlayer.Entity, inSlot, this.Api, true);
						if (collider2 != null)
						{
							collider2.Render(this.Api, byPlayer.Entity, ColorUtil.ColorFromRgba(150, 150, 150, 255));
						}
					}
				}
				return;
			}
			ICollider collider3 = DebugWindowManager._currentCollider.Value.Transform(byPlayer.Entity.Pos, byPlayer.Entity, inSlot, this.Api, true);
			if (collider3 == null)
			{
				return;
			}
			collider3.Render(this.Api, byPlayer.Entity, ColorUtil.ColorFromRgba(255, 125, 125, 255));
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x000234B4 File Offset: 0x000216B4
		public virtual bool OnMouseWheel(ItemSlot slot, IClientPlayer byPlayer, float delta)
		{
			if (!this.IsRightMouseDown(byPlayer))
			{
				return false;
			}
			bool mainHand = MeleeWeaponClient.IsMainHandSlot(slot, byPlayer.Entity);
			this.EnsureStance(byPlayer.Entity, mainHand);
			StanceStats stance = this.GetGripStanceStats(byPlayer.Entity, mainHand);
			if (MeleeWeaponClient.CanChangeGrip(stance))
			{
				GripController gripController = this.GripController;
				if (gripController != null)
				{
					gripController.ChangeGrip(delta, mainHand, stance.GripLengthFactor, stance.GripMinLength, stance.GripMaxLength);
				}
				return true;
			}
			GripController gripController2 = this.GripController;
			if (gripController2 != null)
			{
				gripController2.ResetGrip(mainHand);
			}
			return false;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00023538 File Offset: 0x00021738
		[return: Nullable(2)]
		private StanceStats GetGripStanceStats(EntityPlayer player, bool mainHand)
		{
			StanceStats stance = this.GetStanceStats(player, mainHand);
			if (MeleeWeaponClient.CanChangeGrip(stance))
			{
				return stance;
			}
			if (mainHand && this.CheckForOtherHandEmptyNoError(mainHand, player) && MeleeWeaponClient.CanChangeGrip(this.Stats.TwoHandedStance))
			{
				return this.Stats.TwoHandedStance;
			}
			if (mainHand && MeleeWeaponClient.CanChangeGrip(this.Stats.OneHandedStance))
			{
				return this.Stats.OneHandedStance;
			}
			if (!mainHand && MeleeWeaponClient.CanChangeGrip(this.Stats.OffHandStance))
			{
				return this.Stats.OffHandStance;
			}
			return stance;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x000235C6 File Offset: 0x000217C6
		[NullableContext(2)]
		private static bool CanChangeGrip(StanceStats stance)
		{
			return stance != null && stance.GripLengthFactor != 0f && stance.GripMinLength != stance.GripMaxLength;
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x000235EC File Offset: 0x000217EC
		private bool IsRightMouseDown(IClientPlayer byPlayer)
		{
			ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
			if (playerActionsBehavior == null || !playerActionsBehavior.ActionListener.IsActive(10))
			{
				ActionsManagerPlayerBehavior playerActionsBehavior2 = this.PlayerActionsBehavior;
				if ((playerActionsBehavior2 == null || !playerActionsBehavior2.ActionListener.IsActive(16)) && !byPlayer.Entity.Controls.RightMouseDown && !byPlayer.Entity.ServerControls.RightMouseDown && !this.Api.Input.InWorldMouseButton.Right)
				{
					return this.Api.Input.MouseButton.Right;
				}
			}
			return true;
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00023681 File Offset: 0x00021881
		private static bool IsMainHandSlot(ItemSlot slot, EntityPlayer player)
		{
			return slot != player.LeftHandItemSlot;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0002368F File Offset: 0x0002188F
		public bool CanAttack(EntityPlayer player, bool mainHand = true)
		{
			StanceStats stanceStats = this.GetStanceStats(player, mainHand);
			return stanceStats != null && stanceStats.CanAttack;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000236A4 File Offset: 0x000218A4
		public bool CanBlock(EntityPlayer player, bool mainHand = true)
		{
			StanceStats stanceStats = this.GetStanceStats(player, mainHand);
			return stanceStats != null && stanceStats.CanBlock;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x000236B9 File Offset: 0x000218B9
		public bool CanParry(EntityPlayer player, bool mainHand = true)
		{
			StanceStats stanceStats = this.GetStanceStats(player, mainHand);
			return stanceStats != null && stanceStats.CanParry;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x000236CE File Offset: 0x000218CE
		public bool CanThrow(EntityPlayer player, bool mainHand = true)
		{
			StanceStats stanceStats = this.GetStanceStats(player, mainHand);
			return stanceStats != null && stanceStats.CanThrow;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x000236E3 File Offset: 0x000218E3
		public bool CanBash(EntityPlayer player, bool mainHand = true)
		{
			StanceStats stanceStats = this.GetStanceStats(player, mainHand);
			return stanceStats != null && stanceStats.CanBash;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x000236F8 File Offset: 0x000218F8
		public virtual void OnGameTick(ItemSlot slot, EntityPlayer player, ref int state, bool mainHand)
		{
			if (!this.CanAttack(player, mainHand) && !this.CanBash(player, mainHand))
			{
				return;
			}
			this.EnsureStance(player, mainHand);
			if (!this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[]
			{
				MeleeWeaponState.BlockBashAttacking,
				MeleeWeaponState.Attacking
			}))
			{
				return;
			}
			MeleeAttack attack = this.GetStanceAttack(player, mainHand, mainHand ? this.CurrentMainHandDirection : this.CurrentOffHandDirection, mainHand ? this.CurrentMainHandAttackIsRiposte : this.CurrentOffHandAttackIsRiposte);
			MeleeAttack bash = this.GetStanceBlockBash(player, mainHand, mainHand ? this.CurrentMainHandDirection : this.CurrentOffHandDirection);
			StanceStats stats = this.GetStanceStats(player, mainHand);
			MeleeAttack handle = this.GetStanceHandleAttack(player, mainHand);
			if (stats == null)
			{
				return;
			}
			MeleeWeaponState state2 = this.GetState<MeleeWeaponState>(mainHand);
			if (state2 != MeleeWeaponState.Attacking)
			{
				if (state2 == MeleeWeaponState.BlockBashAttacking && bash != null)
				{
					this.TryBash(bash, handle, stats, slot, player, mainHand);
					return;
				}
			}
			else if (attack != null)
			{
				bool hitTerrain;
				this.TryAttack(attack, handle, stats, slot, player, mainHand, out hitTerrain, this.Settings.MeleeWeaponIgnoreTerrainBehind, this.Settings.MeleeWeaponStopOnTerrainHit);
				if (hitTerrain && this.Settings.MeleeWeaponStopOnTerrainHit)
				{
					this.Api.World.AddCameraShake(this.Stats.ScreenShakeStrength);
					if (stats.AttackHitSound != null)
					{
						this.SoundsSystem.Play(stats.AttackHitSound, false, 32f, 1f, true);
					}
					this.SetState<MeleeWeaponState>(MeleeWeaponState.Idle, mainHand);
					FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
					if (animationBehavior != null)
					{
						animationBehavior.PlayReadyAnimation(mainHand);
					}
					ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
					if (tpAnimationBehavior != null)
					{
						tpAnimationBehavior.PlayReadyAnimation(mainHand);
					}
					this.StartAttackCooldown(mainHand, TimeSpan.FromSeconds(0.5));
					this.MeleeAttackSystem.UpdateAttackStatus(player, MeleeAttackStatus.End, mainHand);
				}
			}
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x000238A4 File Offset: 0x00021AA4
		public void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			if (this.Stats == null)
			{
				return;
			}
			string[] stats = this.Stats.ProficiencyStats;
			if (this.Stats.ProficiencyStat != "")
			{
				stats = Enumerable.ToArray<string>(Enumerable.Prepend<string>(stats, this.Stats.ProficiencyStat));
			}
			if (stats.Length != 0)
			{
				string statsList = Enumerable.Aggregate<string>(Enumerable.Select<string, string>(stats, (string statName) => Lang.Get("combatoverhaul:proficiency-" + statName, Array.Empty<object>())), (string f, string s) => f + ", " + s);
				string description = Lang.Get("combatoverhaul:iteminfo-proficiency", new object[]
				{
					statsList
				});
				dsc.AppendLine(description);
			}
			StanceStats oneHandedStance = this.Stats.OneHandedStance;
			if (((oneHandedStance != null) ? oneHandedStance.Attack : null) != null && this.Stats.OneHandedStance.Attack.DamageTypes.Length != 0)
			{
				string description2 = this.GetAttackStatsDescription(inSlot, Enumerable.Select<MeleeDamageTypeJson, DamageDataJson>(this.Stats.OneHandedStance.Attack.DamageTypes, (MeleeDamageTypeJson element) => element.Damage), "combatoverhaul:iteminfo-melee-weapon-onehanded");
				dsc.AppendLine(description2);
			}
			else
			{
				StanceStats oneHandedStance2 = this.Stats.OneHandedStance;
				if (((oneHandedStance2 != null) ? oneHandedStance2.DirectionalAttacks : null) != null && this.Stats.OneHandedStance.DirectionalAttacks.Count > 0)
				{
					foreach (string description3 in this.GetDirectionalAttackStatsDescriptions(inSlot, this.Stats.OneHandedStance.DirectionalAttacks, "combatoverhaul:iteminfo-melee-weapon-onehanded"))
					{
						dsc.AppendLine(description3);
					}
				}
			}
			StanceStats twoHandedStance = this.Stats.TwoHandedStance;
			if (((twoHandedStance != null) ? twoHandedStance.Attack : null) != null && this.Stats.TwoHandedStance.Attack.DamageTypes.Length != 0)
			{
				string description4 = this.GetAttackStatsDescription(inSlot, Enumerable.Select<MeleeDamageTypeJson, DamageDataJson>(this.Stats.TwoHandedStance.Attack.DamageTypes, (MeleeDamageTypeJson element) => element.Damage), "combatoverhaul:iteminfo-melee-weapon-twohanded");
				dsc.AppendLine(description4);
			}
			else
			{
				StanceStats twoHandedStance2 = this.Stats.TwoHandedStance;
				if (((twoHandedStance2 != null) ? twoHandedStance2.DirectionalAttacks : null) != null && this.Stats.TwoHandedStance.DirectionalAttacks.Count > 0)
				{
					foreach (string description5 in this.GetDirectionalAttackStatsDescriptions(inSlot, this.Stats.TwoHandedStance.DirectionalAttacks, "combatoverhaul:iteminfo-melee-weapon-twohanded"))
					{
						dsc.AppendLine(description5);
					}
				}
			}
			StanceStats offHandStance = this.Stats.OffHandStance;
			if (((offHandStance != null) ? offHandStance.Attack : null) != null && this.Stats.OffHandStance.Attack.DamageTypes.Length != 0)
			{
				string description6 = this.GetAttackStatsDescription(inSlot, Enumerable.Select<MeleeDamageTypeJson, DamageDataJson>(this.Stats.OffHandStance.Attack.DamageTypes, (MeleeDamageTypeJson element) => element.Damage), "combatoverhaul:iteminfo-melee-weapon-offhanded");
				dsc.AppendLine(description6);
			}
			else
			{
				StanceStats offHandStance2 = this.Stats.OffHandStance;
				if (((offHandStance2 != null) ? offHandStance2.DirectionalAttacks : null) != null && this.Stats.OffHandStance.DirectionalAttacks.Count > 0)
				{
					foreach (string description7 in this.GetDirectionalAttackStatsDescriptions(inSlot, this.Stats.OffHandStance.DirectionalAttacks, "combatoverhaul:iteminfo-melee-weapon-offhanded"))
					{
						dsc.AppendLine(description7);
					}
				}
			}
			StanceStats oneHandedStance3 = this.Stats.OneHandedStance;
			if (((oneHandedStance3 != null) ? oneHandedStance3.BlockBash : null) != null && this.Stats.OneHandedStance.BlockBash.DamageTypes.Length != 0)
			{
				string description8 = this.GetAttackStatsDescription(inSlot, Enumerable.Select<MeleeDamageTypeJson, DamageDataJson>(this.Stats.OneHandedStance.BlockBash.DamageTypes, (MeleeDamageTypeJson element) => element.Damage), "combatoverhaul:iteminfo-melee-weapon-onehanded-bash");
				dsc.AppendLine(description8);
			}
			else
			{
				StanceStats oneHandedStance4 = this.Stats.OneHandedStance;
				if (((oneHandedStance4 != null) ? oneHandedStance4.DirectionalBlockBashes : null) != null && this.Stats.OneHandedStance.DirectionalBlockBashes.Count > 0)
				{
					IEnumerable<DamageDataJson> damageTypes = Enumerable.Select<MeleeDamageTypeJson, DamageDataJson>(Enumerable.SelectMany<MeleeAttackStats, MeleeDamageTypeJson>(this.Stats.OneHandedStance.DirectionalBlockBashes.Values, (MeleeAttackStats element) => element.DamageTypes), (MeleeDamageTypeJson element) => element.Damage);
					string description9 = this.GetAttackStatsDescription(inSlot, damageTypes, "combatoverhaul:iteminfo-melee-weapon-onehanded-bash");
					dsc.AppendLine(description9);
				}
			}
			StanceStats twoHandedStance3 = this.Stats.TwoHandedStance;
			if (((twoHandedStance3 != null) ? twoHandedStance3.BlockBash : null) != null && this.Stats.TwoHandedStance.BlockBash.DamageTypes.Length != 0)
			{
				string description10 = this.GetAttackStatsDescription(inSlot, Enumerable.Select<MeleeDamageTypeJson, DamageDataJson>(this.Stats.TwoHandedStance.BlockBash.DamageTypes, (MeleeDamageTypeJson element) => element.Damage), "combatoverhaul:iteminfo-melee-weapon-twohanded-bash");
				dsc.AppendLine(description10);
			}
			else
			{
				StanceStats twoHandedStance4 = this.Stats.TwoHandedStance;
				if (((twoHandedStance4 != null) ? twoHandedStance4.DirectionalBlockBashes : null) != null && this.Stats.TwoHandedStance.DirectionalBlockBashes.Count > 0)
				{
					IEnumerable<DamageDataJson> damageTypes2 = Enumerable.Select<MeleeDamageTypeJson, DamageDataJson>(Enumerable.SelectMany<MeleeAttackStats, MeleeDamageTypeJson>(this.Stats.TwoHandedStance.DirectionalBlockBashes.Values, (MeleeAttackStats element) => element.DamageTypes), (MeleeDamageTypeJson element) => element.Damage);
					string description11 = this.GetAttackStatsDescription(inSlot, damageTypes2, "combatoverhaul:iteminfo-melee-weapon-twohanded-bash");
					dsc.AppendLine(description11);
				}
			}
			StanceStats offHandStance3 = this.Stats.OffHandStance;
			if (((offHandStance3 != null) ? offHandStance3.BlockBash : null) != null && this.Stats.OffHandStance.BlockBash.DamageTypes.Length != 0)
			{
				string description12 = this.GetAttackStatsDescription(inSlot, Enumerable.Select<MeleeDamageTypeJson, DamageDataJson>(this.Stats.OffHandStance.BlockBash.DamageTypes, (MeleeDamageTypeJson element) => element.Damage), "combatoverhaul:iteminfo-melee-weapon-offhanded-bash");
				dsc.AppendLine(description12);
			}
			else
			{
				StanceStats offHandStance4 = this.Stats.OffHandStance;
				if (((offHandStance4 != null) ? offHandStance4.DirectionalBlockBashes : null) != null && this.Stats.OffHandStance.DirectionalBlockBashes.Count > 0)
				{
					IEnumerable<DamageDataJson> damageTypes3 = Enumerable.Select<MeleeDamageTypeJson, DamageDataJson>(Enumerable.SelectMany<MeleeAttackStats, MeleeDamageTypeJson>(this.Stats.OffHandStance.DirectionalBlockBashes.Values, (MeleeAttackStats element) => element.DamageTypes), (MeleeDamageTypeJson element) => element.Damage);
					string description13 = this.GetAttackStatsDescription(inSlot, damageTypes3, "combatoverhaul:iteminfo-melee-weapon-offhanded-bash");
					dsc.AppendLine(description13);
				}
			}
			ItemStackMeleeWeaponStats stackStats = ItemStackMeleeWeaponStats.FromItemStack(inSlot.Itemstack);
			StanceStats oneHandedStance5 = this.Stats.OneHandedStance;
			bool flag;
			if (oneHandedStance5 == null)
			{
				flag = (null != null);
			}
			else
			{
				DamageBlockJson block = oneHandedStance5.Block;
				flag = (((block != null) ? block.BlockTier : null) != null);
			}
			string text;
			int num;
			if (flag)
			{
				dsc.AppendLine();
				float blockTier = 0f;
				foreach (KeyValuePair<string, int> keyValuePair in this.Stats.OneHandedStance.Block.BlockTier)
				{
					keyValuePair.Deconstruct(ref text, ref num);
					float tier = (float)num;
					blockTier = Math.Max(blockTier, tier) + (float)stackStats.BlockTierBonus;
				}
				string text2;
				if (this.Stats.OneHandedStance.Block.Zones.Length != 12)
				{
					text2 = Enumerable.Aggregate<string>(Enumerable.Select<string, string>(this.Stats.OneHandedStance.Block.Zones, (string zone) => Lang.Get("combatoverhaul:detailed-damage-zone-" + zone, Array.Empty<object>())), (string first, string second) => first + ", " + second);
				}
				else
				{
					text2 = Lang.Get("combatoverhaul:detailed-damage-zone-All", Array.Empty<object>());
				}
				string bodyParts = text2;
				string text3 = "combatoverhaul:iteminfo-melee-weapon-blockStats";
				object[] array = new object[3];
				int num2 = 0;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<float>(blockTier, "F0");
				array[num2] = defaultInterpolatedStringHandler.ToStringAndClear();
				array[1] = bodyParts;
				array[2] = Lang.Get("combatoverhaul:iteminfo-melee-weapon-onehanded-block", Array.Empty<object>());
				dsc.AppendLine(Lang.Get(text3, array));
			}
			StanceStats oneHandedStance6 = this.Stats.OneHandedStance;
			bool flag2;
			if (oneHandedStance6 == null)
			{
				flag2 = (null != null);
			}
			else
			{
				DamageBlockJson parry = oneHandedStance6.Parry;
				flag2 = (((parry != null) ? parry.BlockTier : null) != null);
			}
			if (flag2)
			{
				dsc.AppendLine();
				float blockTier2 = 0f;
				foreach (KeyValuePair<string, int> keyValuePair in this.Stats.OneHandedStance.Parry.BlockTier)
				{
					keyValuePair.Deconstruct(ref text, ref num);
					float tier2 = (float)num;
					blockTier2 = Math.Max(blockTier2, tier2) + (float)stackStats.ParryTierBonus;
				}
				string text4;
				if (this.Stats.OneHandedStance.Parry.Zones.Length != 12)
				{
					text4 = Enumerable.Aggregate<string>(Enumerable.Select<string, string>(this.Stats.OneHandedStance.Parry.Zones, (string zone) => Lang.Get("combatoverhaul:detailed-damage-zone-" + zone, Array.Empty<object>())), (string first, string second) => first + ", " + second);
				}
				else
				{
					text4 = Lang.Get("combatoverhaul:detailed-damage-zone-All", Array.Empty<object>());
				}
				string bodyParts2 = text4;
				string text5 = "combatoverhaul:iteminfo-melee-weapon-parryStats";
				object[] array2 = new object[3];
				int num3 = 0;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(0, 1);
				defaultInterpolatedStringHandler2.AppendFormatted<float>(blockTier2, "F0");
				array2[num3] = defaultInterpolatedStringHandler2.ToStringAndClear();
				array2[1] = bodyParts2;
				array2[2] = Lang.Get("combatoverhaul:iteminfo-melee-weapon-onehanded-block", Array.Empty<object>());
				dsc.AppendLine(Lang.Get(text5, array2));
			}
			StanceStats twoHandedStance5 = this.Stats.TwoHandedStance;
			bool flag3;
			if (twoHandedStance5 == null)
			{
				flag3 = (null != null);
			}
			else
			{
				DamageBlockJson block2 = twoHandedStance5.Block;
				flag3 = (((block2 != null) ? block2.BlockTier : null) != null);
			}
			if (flag3)
			{
				dsc.AppendLine();
				float blockTier3 = 0f;
				foreach (KeyValuePair<string, int> keyValuePair in this.Stats.TwoHandedStance.Block.BlockTier)
				{
					keyValuePair.Deconstruct(ref text, ref num);
					float tier3 = (float)num;
					blockTier3 = Math.Max(blockTier3, tier3) + (float)stackStats.BlockTierBonus;
				}
				string text6;
				if (this.Stats.TwoHandedStance.Block.Zones.Length != 12)
				{
					text6 = Enumerable.Aggregate<string>(Enumerable.Select<string, string>(this.Stats.TwoHandedStance.Block.Zones, (string zone) => Lang.Get("combatoverhaul:detailed-damage-zone-" + zone, Array.Empty<object>())), (string first, string second) => first + ", " + second);
				}
				else
				{
					text6 = Lang.Get("combatoverhaul:detailed-damage-zone-All", Array.Empty<object>());
				}
				string bodyParts3 = text6;
				string text7 = "combatoverhaul:iteminfo-melee-weapon-blockStats";
				object[] array3 = new object[3];
				int num4 = 0;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
				defaultInterpolatedStringHandler3..ctor(0, 1);
				defaultInterpolatedStringHandler3.AppendFormatted<float>(blockTier3, "F0");
				array3[num4] = defaultInterpolatedStringHandler3.ToStringAndClear();
				array3[1] = bodyParts3;
				array3[2] = Lang.Get("combatoverhaul:iteminfo-melee-weapon-twohanded-block", Array.Empty<object>());
				dsc.AppendLine(Lang.Get(text7, array3));
			}
			StanceStats twoHandedStance6 = this.Stats.TwoHandedStance;
			bool flag4;
			if (twoHandedStance6 == null)
			{
				flag4 = (null != null);
			}
			else
			{
				DamageBlockJson parry2 = twoHandedStance6.Parry;
				flag4 = (((parry2 != null) ? parry2.BlockTier : null) != null);
			}
			if (flag4)
			{
				dsc.AppendLine();
				float blockTier4 = 0f;
				foreach (KeyValuePair<string, int> keyValuePair in this.Stats.TwoHandedStance.Parry.BlockTier)
				{
					keyValuePair.Deconstruct(ref text, ref num);
					float tier4 = (float)num;
					blockTier4 = Math.Max(blockTier4, tier4) + (float)stackStats.ParryTierBonus;
				}
				string text8;
				if (this.Stats.TwoHandedStance.Parry.Zones.Length != 12)
				{
					text8 = Enumerable.Aggregate<string>(Enumerable.Select<string, string>(this.Stats.TwoHandedStance.Parry.Zones, (string zone) => Lang.Get("combatoverhaul:detailed-damage-zone-" + zone, Array.Empty<object>())), (string first, string second) => first + ", " + second);
				}
				else
				{
					text8 = Lang.Get("combatoverhaul:detailed-damage-zone-All", Array.Empty<object>());
				}
				string bodyParts4 = text8;
				string text9 = "combatoverhaul:iteminfo-melee-weapon-parryStats";
				object[] array4 = new object[3];
				int num5 = 0;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4;
				defaultInterpolatedStringHandler4..ctor(0, 1);
				defaultInterpolatedStringHandler4.AppendFormatted<float>(blockTier4, "F0");
				array4[num5] = defaultInterpolatedStringHandler4.ToStringAndClear();
				array4[1] = bodyParts4;
				array4[2] = Lang.Get("combatoverhaul:iteminfo-melee-weapon-twohanded-block", Array.Empty<object>());
				dsc.AppendLine(Lang.Get(text9, array4));
			}
			StanceStats offHandStance5 = this.Stats.OffHandStance;
			bool flag5;
			if (offHandStance5 == null)
			{
				flag5 = (null != null);
			}
			else
			{
				DamageBlockJson block3 = offHandStance5.Block;
				flag5 = (((block3 != null) ? block3.BlockTier : null) != null);
			}
			if (flag5)
			{
				dsc.AppendLine();
				float blockTier5 = 0f;
				foreach (KeyValuePair<string, int> keyValuePair in this.Stats.OffHandStance.Block.BlockTier)
				{
					keyValuePair.Deconstruct(ref text, ref num);
					float tier5 = (float)num;
					blockTier5 = Math.Max(blockTier5, tier5) + (float)stackStats.BlockTierBonus;
				}
				string text10;
				if (this.Stats.OffHandStance.Block.Zones.Length != 12)
				{
					text10 = Enumerable.Aggregate<string>(Enumerable.Select<string, string>(this.Stats.OffHandStance.Block.Zones, (string zone) => Lang.Get("combatoverhaul:detailed-damage-zone-" + zone, Array.Empty<object>())), (string first, string second) => first + ", " + second);
				}
				else
				{
					text10 = Lang.Get("combatoverhaul:detailed-damage-zone-All", Array.Empty<object>());
				}
				string bodyParts5 = text10;
				string text11 = "combatoverhaul:iteminfo-melee-weapon-blockStats";
				object[] array5 = new object[3];
				int num6 = 0;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5;
				defaultInterpolatedStringHandler5..ctor(0, 1);
				defaultInterpolatedStringHandler5.AppendFormatted<float>(blockTier5, "F0");
				array5[num6] = defaultInterpolatedStringHandler5.ToStringAndClear();
				array5[1] = bodyParts5;
				array5[2] = Lang.Get("combatoverhaul:iteminfo-melee-weapon-offhanded-block", Array.Empty<object>());
				dsc.AppendLine(Lang.Get(text11, array5));
			}
			StanceStats offHandStance6 = this.Stats.OffHandStance;
			bool flag6;
			if (offHandStance6 == null)
			{
				flag6 = (null != null);
			}
			else
			{
				DamageBlockJson parry3 = offHandStance6.Parry;
				flag6 = (((parry3 != null) ? parry3.BlockTier : null) != null);
			}
			if (flag6)
			{
				dsc.AppendLine();
				float blockTier6 = 0f;
				foreach (KeyValuePair<string, int> keyValuePair in this.Stats.OffHandStance.Parry.BlockTier)
				{
					keyValuePair.Deconstruct(ref text, ref num);
					float tier6 = (float)num;
					blockTier6 = Math.Max(blockTier6, tier6) + (float)stackStats.ParryTierBonus;
				}
				string text12;
				if (this.Stats.OffHandStance.Parry.Zones.Length != 12)
				{
					text12 = Enumerable.Aggregate<string>(Enumerable.Select<string, string>(this.Stats.OffHandStance.Parry.Zones, (string zone) => Lang.Get("combatoverhaul:detailed-damage-zone-" + zone, Array.Empty<object>())), (string first, string second) => first + ", " + second);
				}
				else
				{
					text12 = Lang.Get("combatoverhaul:detailed-damage-zone-All", Array.Empty<object>());
				}
				string bodyParts6 = text12;
				string text13 = "combatoverhaul:iteminfo-melee-weapon-parryStats";
				object[] array6 = new object[3];
				int num7 = 0;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler6;
				defaultInterpolatedStringHandler6..ctor(0, 1);
				defaultInterpolatedStringHandler6.AppendFormatted<float>(blockTier6, "F0");
				array6[num7] = defaultInterpolatedStringHandler6.ToStringAndClear();
				array6[1] = bodyParts6;
				array6[2] = Lang.Get("combatoverhaul:iteminfo-melee-weapon-offhanded-block", Array.Empty<object>());
				dsc.AppendLine(Lang.Get(text13, array6));
			}
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0002492C File Offset: 0x00022B2C
		public bool RestrictRightHandAction()
		{
			return !this.CheckState<MeleeWeaponState>(true, new MeleeWeaponState[]
			{
				MeleeWeaponState.Idle,
				MeleeWeaponState.Aiming,
				MeleeWeaponState.StartingAim,
				MeleeWeaponState.Cooldown
			}) && this.GetStance<MeleeWeaponStance>(true) != MeleeWeaponStance.OffHandDualWield;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00024957 File Offset: 0x00022B57
		public bool RestrictLeftHandAction()
		{
			return !this.CheckState<MeleeWeaponState>(false, new MeleeWeaponState[]
			{
				MeleeWeaponState.Idle,
				MeleeWeaponState.Aiming,
				MeleeWeaponState.StartingAim,
				MeleeWeaponState.Cooldown
			}) && this.GetStance<MeleeWeaponStance>(false) != MeleeWeaponStance.MainHandDualWield;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00024982 File Offset: 0x00022B82
		public void PlayReadyAnimation(bool mainHand)
		{
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayReadyAnimation(mainHand);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior == null)
			{
				return;
			}
			tpAnimationBehavior.PlayReadyAnimation(mainHand);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x000249A7 File Offset: 0x00022BA7
		public static void SetGlobalCooldown(ICoreAPI api, long cooldownMs = 1000L)
		{
			MeleeWeaponClient.GlobalCooldownUntilMs = api.World.ElapsedMilliseconds + cooldownMs;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x000249BC File Offset: 0x00022BBC
		[ActionEventHandler(9, ActionState.Active)]
		protected virtual bool Attack(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (!this.Active)
			{
				return false;
			}
			this.EnsureStance(player, mainHand);
			if (this.TryThrowImpaledTarget(slot, player, mainHand, direction))
			{
				return true;
			}
			bool canRiposte = MeleeWeaponClient.CanRiposteMainHand || MeleeWeaponClient.CanRiposteOffHand;
			if (this.IsAttackOnCooldown(mainHand))
			{
				return false;
			}
			if (InteractionsTester.PlayerTriesToInteract(player, mainHand, eventData))
			{
				return false;
			}
			if (!this.CanAttack(player, mainHand))
			{
				return false;
			}
			if (this.ItemInOtherHandBlocksAttack(player, mainHand) && !canRiposte)
			{
				return false;
			}
			if (this.ActionRestricted(player, mainHand))
			{
				return false;
			}
			StanceStats stats = this.GetStanceStats(player, mainHand);
			MeleeAttack handle = this.GetStanceHandleAttack(player, mainHand);
			if (stats == null)
			{
				return false;
			}
			MeleeAttack attack = this.GetStanceAttack(player, mainHand, direction, stats.CanRiposte && canRiposte);
			if (attack == null)
			{
				return false;
			}
			switch (this.GetState<MeleeWeaponState>(mainHand))
			{
			case MeleeWeaponState.Idle:
				this.TurnOffRiposte(mainHand);
				this.StartAttack(slot, player, mainHand, direction, attack, handle, stats, stats.CanRiposte && canRiposte);
				this.ResetRiposte(mainHand);
				return true;
			case MeleeWeaponState.WindingUp:
			case MeleeWeaponState.Attacking:
			case MeleeWeaponState.Cooldown:
				return true;
			case MeleeWeaponState.Parrying:
				if (!stats.CanRiposte || !canRiposte)
				{
					return false;
				}
				this.TurnOnRiposte(mainHand);
				this.StartAttack(slot, player, mainHand, direction, attack, handle, stats, true);
				this.ResetRiposte(mainHand);
				return true;
			}
			return false;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00024B04 File Offset: 0x00022D04
		protected virtual bool TryThrowImpaledTarget(ItemSlot slot, EntityPlayer player, bool mainHand, AttackDirection direction)
		{
			ImpaleSystemClient impaleSystem = this.ImpaleSystem;
			if (impaleSystem == null || !impaleSystem.HasImpaled(player.EntityId, mainHand))
			{
				return false;
			}
			if (this.IsAttackOnCooldown(mainHand))
			{
				return true;
			}
			if (!this.CanAttack(player, mainHand))
			{
				return false;
			}
			if (this.ItemInOtherHandBlocksAttack(player, mainHand))
			{
				return false;
			}
			if (this.ActionRestricted(player, mainHand))
			{
				return false;
			}
			if (!this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[]
			{
				MeleeWeaponState.Idle,
				MeleeWeaponState.Cooldown
			}))
			{
				return true;
			}
			StanceStats stats = this.GetStanceStats(player, mainHand);
			if (stats == null)
			{
				return false;
			}
			if (this.GetStanceAttack(player, mainHand, direction, false) == null)
			{
				return false;
			}
			if (mainHand)
			{
				this.CurrentMainHandDirection = direction;
				this.CurrentMainHandAttackIsRiposte = false;
			}
			else
			{
				this.CurrentOffHandDirection = direction;
				this.CurrentOffHandAttackIsRiposte = false;
			}
			int counter = mainHand ? this.MainHandAttackCounter : this.OffHandAttackCounter;
			string impaleThrowAnimation = this.GetImpaleThrowAnimation(stats, direction, counter);
			if (impaleThrowAnimation == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(66, 1);
				defaultInterpolatedStringHandler.AppendLiteral("[Combat Overhaul] Item '");
				defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(this.Item.Code);
				defaultInterpolatedStringHandler.AppendLiteral("' does not have attack animation specified");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			string attackAnimation = impaleThrowAnimation;
			float animationSpeed = this.GetAnimationSpeed(player, this.Stats, 0.5f, 2f) * ItemStackMeleeWeaponStats.GetAttackSpeed(slot.Itemstack) * stats.AttackSpeedMultiplier * this.Settings.MeleeWeaponAttackSpeedMultiplier;
			this.MeleeBlockSystem.StopBlock(mainHand);
			this.SetState<MeleeWeaponState>(MeleeWeaponState.WindingUp, mainHand);
			this.MeleeAttackSystem.UpdateAttackStatus(player, MeleeAttackStatus.Start, mainHand);
			this.ImpaleSystem.RequestThrow(mainHand, player.Pos.GetViewVector());
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				bool mainHand2 = mainHand;
				string animation = attackAnimation;
				float animationSpeed2 = animationSpeed;
				animationBehavior.Play(mainHand2, animation, MeleeWeaponClient.AnimationCategory(mainHand), animationSpeed2, 1f, () => this.ImpaleThrowAnimationCallback(player, mainHand, stats), delegate(string code)
				{
					this.ImpaleThrowAnimationCallbackHandler(player, code, mainHand, stats);
				}, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				bool mainHand3 = mainHand;
				string animation2 = attackAnimation;
				float animationSpeed2 = animationSpeed;
				tpAnimationBehavior.Play(mainHand3, animation2, MeleeWeaponClient.AnimationCategory(mainHand), animationSpeed2, 1f, true);
			}
			if (mainHand)
			{
				this.MainHandAttackCounter++;
			}
			else
			{
				this.OffHandAttackCounter++;
			}
			return true;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00024DC4 File Offset: 0x00022FC4
		[return: Nullable(2)]
		protected virtual string GetImpaleThrowAnimation(StanceStats stats, AttackDirection direction, int counter)
		{
			if (stats.AttackAnimation.Count == 0)
			{
				return null;
			}
			string directionKey = direction.ToString();
			string[] directionalAnimations;
			string[] animations;
			string[] mainAnimations;
			if (this.DirectionsType != DirectionsConfiguration.None && stats.AttackAnimation.TryGetValue(directionKey, ref directionalAnimations))
			{
				animations = directionalAnimations;
			}
			else if (stats.AttackAnimation.TryGetValue("Main", ref mainAnimations))
			{
				animations = mainAnimations;
			}
			else
			{
				animations = Enumerable.FirstOrDefault<string[]>(stats.AttackAnimation.Values);
			}
			if (animations == null || animations.Length == 0)
			{
				return null;
			}
			return animations[counter % animations.Length];
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00024E48 File Offset: 0x00023048
		protected virtual bool ImpaleThrowAnimationCallback(EntityPlayer player, bool mainHand, StanceStats stats)
		{
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayReadyAnimation(mainHand);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.PlayReadyAnimation(mainHand);
			}
			this.SetState<MeleeWeaponState>(MeleeWeaponState.Idle, mainHand);
			this.StartAttackCooldown(mainHand, TimeSpan.FromMilliseconds((double)((stats.AttackCooldownMs > 0f) ? stats.AttackCooldownMs : 500f)));
			this.MeleeAttackSystem.UpdateAttackStatus(player, MeleeAttackStatus.End, mainHand);
			return true;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00024EB8 File Offset: 0x000230B8
		protected virtual void ImpaleThrowAnimationCallbackHandler(EntityPlayer player, string callbackCode, bool mainHand, StanceStats stats)
		{
			if (callbackCode == "start")
			{
				this.SetState<MeleeWeaponState>(MeleeWeaponState.WindingUp, mainHand);
				return;
			}
			if (callbackCode == "stop")
			{
				this.SetState<MeleeWeaponState>(MeleeWeaponState.Cooldown, mainHand);
				return;
			}
			if (!(callbackCode == "ready"))
			{
				return;
			}
			this.ImpaleThrowAnimationCallback(player, mainHand, stats);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00024F0C File Offset: 0x0002310C
		protected virtual void StartAttack(ItemSlot slot, EntityPlayer player, bool mainHand, AttackDirection direction, MeleeAttack attack, [Nullable(2)] MeleeAttack handle, StanceStats stats, bool riposte = false)
		{
			if (mainHand)
			{
				this.CurrentMainHandDirection = direction;
				this.CurrentMainHandAttackIsRiposte = riposte;
			}
			else
			{
				this.CurrentOffHandDirection = direction;
				this.CurrentOffHandAttackIsRiposte = riposte;
			}
			int counter = mainHand ? this.MainHandAttackCounter : this.OffHandAttackCounter;
			string text = riposte ? stats.RiposteAnimation : this.GetAttackAnimationForDirection(stats, direction, counter);
			if (text == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(66, 1);
				defaultInterpolatedStringHandler.AppendLiteral("[Combat Overhaul] Item '");
				defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(this.Item.Code);
				defaultInterpolatedStringHandler.AppendLiteral("' does not have attack animation specified");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			string attackAnimation = text;
			float animationSpeed = this.GetAnimationSpeed(player, this.Stats, 0.5f, 2f) * ItemStackMeleeWeaponStats.GetAttackSpeed(slot.Itemstack) * stats.AttackSpeedMultiplier * this.Settings.MeleeWeaponAttackSpeedMultiplier;
			this.MeleeBlockSystem.StopBlock(mainHand);
			this.SetState<MeleeWeaponState>(MeleeWeaponState.WindingUp, mainHand);
			this.MeleeAttackSystem.UpdateAttackStatus(player, MeleeAttackStatus.Start, mainHand);
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
				float animationSpeed2 = animationSpeed;
				animationBehavior.Play(mainHand2, animation, MeleeWeaponClient.AnimationCategory(mainHand), animationSpeed2, 1f, () => this.AttackAnimationCallback(player, mainHand), delegate(string code)
				{
					this.AttackAnimationCallbackHandler(player, code, mainHand);
				}, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				bool mainHand3 = mainHand;
				string animation2 = attackAnimation;
				float animationSpeed2 = animationSpeed;
				tpAnimationBehavior.Play(mainHand3, animation2, MeleeWeaponClient.AnimationCategory(mainHand), animationSpeed2, 1f, true);
			}
			if (mainHand && this.IsCurrentAttackOneHanded(stats))
			{
				this.TryPlayLinkedOffhandDaggerAnimation(player, attackAnimation, animationSpeed);
			}
			if (mainHand)
			{
				this.MainHandAttackCounter++;
			}
			else
			{
				this.OffHandAttackCounter++;
			}
			this.HandleHitTerrain = false;
			if (this.Settings.FlipDirectionAfterAttack)
			{
				ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
				if (playerActionsBehavior == null)
				{
					return;
				}
				playerActionsBehavior.FlipDirectionToOpposite();
			}
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0002514C File Offset: 0x0002334C
		protected virtual void TryAttack(MeleeAttack attack, [Nullable(2)] MeleeAttack handle, StanceStats stats, ItemSlot slot, EntityPlayer player, bool mainHand, out bool hitTerrain, bool ignoreTerrainBehind, bool collideWithTerrain)
		{
			hitTerrain = false;
			ItemStackMeleeWeaponStats stackStats = ItemStackMeleeWeaponStats.FromItemStack(slot.Itemstack);
			AttackDirection attackDirection = mainHand ? this.CurrentMainHandDirection : this.CurrentOffHandDirection;
			bool handleAttacked = false;
			if (handle != null)
			{
				IEnumerable<ValueTuple<Block, Vector3d>> handleTerrainCollision;
				IEnumerable<ValueTuple<Entity, Vector3d>> handleEntitiesCollision;
				handleAttacked = handle.Attack(player.Player, slot, mainHand, out handleTerrainCollision, out handleEntitiesCollision, stackStats, attackDirection, collideWithTerrain);
				if (Enumerable.Any<ValueTuple<Block, Vector3d>>(handleTerrainCollision) && !handleAttacked)
				{
					if (this.Settings.DebugHitParticles)
					{
						foreach (ValueTuple<Block, Vector3d> valueTuple in handleTerrainCollision)
						{
							Vector3d point = valueTuple.Item2;
							Vec3d pos8 = new Vec3d(point.X, point.Y, point.Z);
							player.Api.World.SpawnParticles(1f, ColorUtil.ColorFromRgba(125, 255, 125, 125), pos8, pos8, new Vec3f(), new Vec3f(), 1f, 0f, 1f, 1, null);
						}
					}
					hitTerrain = true;
					if (ignoreTerrainBehind)
					{
						Vector3d playerPosition = player.Pos.XYZ.ToOpenTK();
						Vector3d eyesPosition = player.LocalEyePos.ToOpenTK() + playerPosition;
						Vector3d viewDirection = player.Pos.GetViewVector().ToVec3d().ToOpenTK();
						double eyesProjection = Vector3d.Dot(viewDirection, eyesPosition);
						foreach (ValueTuple<Block, Vector3d> valueTuple2 in handleTerrainCollision)
						{
							Vector3d point2 = valueTuple2.Item2;
							if (Vector3d.Dot(viewDirection, point2) < eyesProjection)
							{
								hitTerrain = false;
								break;
							}
						}
					}
					if (hitTerrain)
					{
						if (!this.HandleHitTerrain)
						{
							if (stats.HandleHitSound != null)
							{
								this.SoundsSystem.Play(stats.HandleHitSound, false, 32f, 1f, true);
							}
							this.HandleHitTerrain = true;
						}
						return;
					}
				}
				if (this.Settings.DebugHitParticles && Enumerable.Any<ValueTuple<Entity, Vector3d>>(handleEntitiesCollision) && handleAttacked)
				{
					foreach (ValueTuple<Entity, Vector3d> valueTuple3 in handleEntitiesCollision)
					{
						Vector3d point3 = valueTuple3.Item2;
						Vec3d pos9 = new Vec3d(point3.X, point3.Y, point3.Z);
						player.Api.World.SpawnParticles(1f, ColorUtil.ColorFromRgba(125, 125, 255, 125), pos9, pos9, new Vec3f(), new Vec3f(), 1f, 0f, 1f, 1, null);
					}
				}
			}
			IEnumerable<ValueTuple<Block, Vector3d>> terrainCollision;
			IEnumerable<ValueTuple<Entity, Vector3d>> entitiesCollision;
			bool attacked = attack.Attack(player.Player, slot, mainHand, out terrainCollision, out entitiesCollision, stackStats, attackDirection, collideWithTerrain);
			if (this.Settings.DebugHitParticles && Enumerable.Any<ValueTuple<Block, Vector3d>>(terrainCollision) && !attacked)
			{
				foreach (ValueTuple<Block, Vector3d> valueTuple4 in terrainCollision)
				{
					Vector3d point4 = valueTuple4.Item2;
					Vec3d pos10 = new Vec3d(point4.X, point4.Y, point4.Z);
					player.Api.World.SpawnParticles(1f, ColorUtil.ColorFromRgba(0, 255, 0, 125), pos10, pos10, new Vec3f(), new Vec3f(), 1f, 0f, 1f, 1, null);
				}
			}
			if (this.Settings.DebugHitParticles && Enumerable.Any<ValueTuple<Entity, Vector3d>>(entitiesCollision) && attacked)
			{
				foreach (ValueTuple<Entity, Vector3d> valueTuple5 in entitiesCollision)
				{
					Vector3d point5 = valueTuple5.Item2;
					Vec3d pos11 = new Vec3d(point5.X, point5.Y, point5.Z);
					player.Api.World.SpawnParticles(1f, ColorUtil.ColorFromRgba(0, 0, 255, 125), pos11, pos11, new Vec3f(), new Vec3f(), 1f, 0f, 1f, 1, null);
				}
			}
			if (attacked && mainHand && this.IsCurrentAttackOneHanded(stats) && !this.IsDualDaggerLoadout(player))
			{
				this.TrySendLinkedOffhandDaggerDamage(player, attack);
			}
			if (handle != null && !this.HandleHitTerrain && handleAttacked && !attacked)
			{
				if (stats.HandleHitSound != null)
				{
					this.SoundsSystem.Play(stats.HandleHitSound, false, 32f, 1f, true);
				}
				this.HandleHitTerrain = true;
			}
			if (handle != null)
			{
				handle.AddAttackedEntities(attack, player.EntityId);
			}
			if (Enumerable.Any<ValueTuple<Block, Vector3d>>(terrainCollision))
			{
				hitTerrain = true;
			}
			if (attacked && stats.AttackHitSound != null)
			{
				this.SoundsSystem.Play(stats.AttackHitSound, false, 32f, 1f, true);
			}
			if (attacked && this.Stats.AnimationStaggerOnHitDurationMs > 0f)
			{
				FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
				if (animationBehavior != null)
				{
					animationBehavior.SetSpeedModifier(new AnimationSpeedModifierDelegate(this.AttackImpactFunction));
				}
				this.Api.World.AddCameraShake(this.Stats.ScreenShakeStrength);
			}
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00025684 File Offset: 0x00023884
		protected virtual bool IsCurrentAttackOneHanded(StanceStats stats)
		{
			MeleeWeaponStance stance = this.GetStance<MeleeWeaponStance>(true);
			if (stance == MeleeWeaponStance.MainHand || stance == MeleeWeaponStance.MainHandDualWield)
			{
				return true;
			}
			if (stats == this.Stats.OneHandedStance)
			{
				return true;
			}
			foreach (StanceStats dualWieldStats in this.Stats.MainHandDualWieldStances.Values)
			{
				if (stats == dualWieldStats)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00025708 File Offset: 0x00023908
		protected virtual void TryPlayLinkedOffhandDaggerAnimation(EntityPlayer player, string mainHandAnimation, float mainHandAnimationSpeed)
		{
			ItemStack offhandStack;
			if (!this.TryGetOffhandDaggerStack(player, out offhandStack) || offhandStack == null)
			{
				return;
			}
			MeleeWeaponStats daggerStats = this.TryGetMeleeWeaponStats(offhandStack);
			StanceStats animationStats = this.GetLinkedOffhandDaggerAnimationStance(player, daggerStats);
			if (animationStats == null)
			{
				return;
			}
			string linkedOffhandAnimation = this.GetLinkedOffhandAttackAnimation(animationStats, this.CurrentMainHandDirection, this.OffHandAttackCounter) ?? mainHandAnimation;
			float animationSpeed = mainHandAnimationSpeed * ItemStackMeleeWeaponStats.GetAttackSpeed(offhandStack) * animationStats.AttackSpeedMultiplier;
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				bool mainHand = false;
				string animation = linkedOffhandAnimation;
				float animationSpeed2 = animationSpeed;
				animationBehavior.Play(mainHand, animation, MeleeWeaponClient.AnimationCategory(false), animationSpeed2, 1f, () => this.LinkedOffhandDaggerAnimationCallback(player), null, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				bool mainHand2 = false;
				string animation2 = linkedOffhandAnimation;
				float animationSpeed2 = animationSpeed;
				tpAnimationBehavior.Play(mainHand2, animation2, MeleeWeaponClient.AnimationCategory(false), animationSpeed2, 1f, true);
			}
			this.OffHandAttackCounter++;
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x000257E9 File Offset: 0x000239E9
		protected virtual bool LinkedOffhandDaggerAnimationCallback(EntityPlayer player)
		{
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayReadyAnimation(false);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.PlayReadyAnimation(false);
			}
			return true;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00025810 File Offset: 0x00023A10
		protected virtual void TrySendLinkedOffhandDaggerDamage(EntityPlayer player, MeleeAttack attack)
		{
			ItemStack offhandStack;
			if (!this.TryGetOffhandDaggerStack(player, out offhandStack) || offhandStack == null)
			{
				return;
			}
			MeleeDamagePacket[] sourcePackets = attack.LastDamagePackets;
			if (sourcePackets.Length == 0)
			{
				return;
			}
			MeleeWeaponStats daggerStats = this.TryGetMeleeWeaponStats(offhandStack);
			MeleeAttackStats daggerAttack = this.GetLinkedOffhandDaggerDamageAttack(player, daggerStats);
			if (daggerAttack == null || daggerAttack.DamageTypes.Length == 0)
			{
				return;
			}
			ItemStackMeleeWeaponStats offhandStackStats = ItemStackMeleeWeaponStats.FromItemStack(offhandStack);
			List<MeleeDamagePacket> offhandPackets = new List<MeleeDamagePacket>();
			HashSet<long> alreadyProcessedTargets = new HashSet<long>();
			foreach (MeleeDamagePacket sourcePacket in sourcePackets)
			{
				if (alreadyProcessedTargets.Add(sourcePacket.TargetEntityId))
				{
					Entity target = this.Api.World.GetEntityById(sourcePacket.TargetEntityId);
					if (target != null)
					{
						Vector3d position = (sourcePacket.Position.Length >= 3) ? new Vector3d(sourcePacket.Position[0], sourcePacket.Position[1], sourcePacket.Position[2]) : target.Pos.XYZ.ToOpenTK();
						ColliderTypes colliderType = (ColliderTypes)sourcePacket.ColliderType;
						MeleeDamageTypeJson[] damageTypes = daggerAttack.DamageTypes;
						for (int j = 0; j < damageTypes.Length; j++)
						{
							MeleeDamagePacket offhandPacket = damageTypes[j].ToDamageType().CreateDamagePacket(player, target, position, sourcePacket.Collider, false, colliderType, offhandStackStats, 0.75f, 0.75f, -1, AttackDirection.Top);
							offhandPackets.Add(offhandPacket);
						}
					}
				}
			}
			if (offhandPackets.Count > 0)
			{
				this.MeleeAttackSystem.SendPackets(offhandPackets);
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00025978 File Offset: 0x00023B78
		[NullableContext(2)]
		protected virtual StanceStats GetLinkedOffhandDaggerAnimationStance([Nullable(1)] EntityPlayer player, MeleeWeaponStats daggerStats)
		{
			ItemSlot rightHandItemSlot = player.RightHandItemSlot;
			string text;
			if (rightHandItemSlot == null)
			{
				text = null;
			}
			else
			{
				ItemStack itemstack = rightHandItemSlot.Itemstack;
				if (itemstack == null)
				{
					text = null;
				}
				else
				{
					CollectibleObject collectible = itemstack.Collectible;
					if (collectible == null)
					{
						text = null;
					}
					else
					{
						AssetLocation code = collectible.Code;
						text = ((code != null) ? code.ToString() : null);
					}
				}
			}
			string mainHandCode = text ?? "";
			if (mainHandCode != "" && ((daggerStats != null) ? daggerStats.OffHandDualWieldStances : null) != null && daggerStats.OffHandDualWieldStances.Count > 0)
			{
				foreach (KeyValuePair<string, StanceStats> keyValuePair in daggerStats.OffHandDualWieldStances)
				{
					string text2;
					StanceStats stanceStats;
					keyValuePair.Deconstruct(ref text2, ref stanceStats);
					string text3 = text2;
					StanceStats stance = stanceStats;
					if (WildcardUtil.Match(text3, mainHandCode) && ((stance != null) ? stance.AttackAnimation : null) != null && stance.AttackAnimation.Count > 0)
					{
						return stance;
					}
				}
			}
			bool flag;
			if (daggerStats == null)
			{
				flag = (null != null);
			}
			else
			{
				StanceStats offHandStance = daggerStats.OffHandStance;
				flag = (((offHandStance != null) ? offHandStance.AttackAnimation : null) != null);
			}
			if (flag && daggerStats.OffHandStance.AttackAnimation.Count > 0)
			{
				return daggerStats.OffHandStance;
			}
			if (daggerStats == null)
			{
				return null;
			}
			return daggerStats.OneHandedStance;
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00025AAC File Offset: 0x00023CAC
		[return: Nullable(2)]
		protected virtual string GetLinkedOffhandAttackAnimation(StanceStats stats, AttackDirection direction, int counter)
		{
			if (stats.AttackAnimation == null || stats.AttackAnimation.Count == 0)
			{
				return null;
			}
			string directionKey = direction.ToString();
			string[] directionalAnimations;
			if (stats.AttackDirectionsType != "None" && stats.AttackAnimation.TryGetValue(directionKey, ref directionalAnimations) && directionalAnimations != null && directionalAnimations.Length != 0)
			{
				return directionalAnimations[counter % directionalAnimations.Length];
			}
			string[] mainAnimations;
			if (stats.AttackAnimation.TryGetValue("Main", ref mainAnimations) && mainAnimations != null && mainAnimations.Length != 0)
			{
				return mainAnimations[counter % mainAnimations.Length];
			}
			string[] fallbackAnimations = Enumerable.FirstOrDefault<string[]>(stats.AttackAnimation.Values, (string[] animations) => animations != null && animations.Length != 0);
			if (fallbackAnimations == null || fallbackAnimations.Length == 0)
			{
				return null;
			}
			return fallbackAnimations[counter % fallbackAnimations.Length];
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00025B74 File Offset: 0x00023D74
		[NullableContext(2)]
		protected virtual MeleeAttackStats GetLinkedOffhandDaggerDamageAttack([Nullable(1)] EntityPlayer player, MeleeWeaponStats daggerStats)
		{
			ItemSlot rightHandItemSlot = player.RightHandItemSlot;
			string text;
			if (rightHandItemSlot == null)
			{
				text = null;
			}
			else
			{
				ItemStack itemstack = rightHandItemSlot.Itemstack;
				if (itemstack == null)
				{
					text = null;
				}
				else
				{
					CollectibleObject collectible = itemstack.Collectible;
					if (collectible == null)
					{
						text = null;
					}
					else
					{
						AssetLocation code = collectible.Code;
						text = ((code != null) ? code.ToString() : null);
					}
				}
			}
			string mainHandCode = text ?? "";
			if (mainHandCode != "" && ((daggerStats != null) ? daggerStats.OffHandDualWieldStances : null) != null && daggerStats.OffHandDualWieldStances.Count > 0)
			{
				foreach (KeyValuePair<string, StanceStats> keyValuePair in daggerStats.OffHandDualWieldStances)
				{
					string text2;
					StanceStats stanceStats;
					keyValuePair.Deconstruct(ref text2, ref stanceStats);
					string text3 = text2;
					StanceStats stance = stanceStats;
					if (WildcardUtil.Match(text3, mainHandCode) && ((stance != null) ? stance.Attack : null) != null)
					{
						return stance.Attack;
					}
				}
			}
			MeleeAttackStats meleeAttackStats;
			if (daggerStats == null)
			{
				meleeAttackStats = null;
			}
			else
			{
				StanceStats oneHandedStance = daggerStats.OneHandedStance;
				meleeAttackStats = ((oneHandedStance != null) ? oneHandedStance.Attack : null);
			}
			MeleeAttackStats result;
			if ((result = meleeAttackStats) == null)
			{
				if (daggerStats == null)
				{
					return null;
				}
				StanceStats offHandStance = daggerStats.OffHandStance;
				if (offHandStance == null)
				{
					return null;
				}
				result = offHandStance.Attack;
			}
			return result;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00025C90 File Offset: 0x00023E90
		protected virtual bool TryGetOffhandDaggerStack(EntityPlayer player, [Nullable(2)] out ItemStack stack)
		{
			ItemSlot leftHandItemSlot = player.LeftHandItemSlot;
			stack = ((leftHandItemSlot != null) ? leftHandItemSlot.Itemstack : null);
			return this.IsDaggerStack(stack);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00025CAE File Offset: 0x00023EAE
		protected virtual bool IsDualDaggerLoadout(EntityPlayer player)
		{
			ItemSlot rightHandItemSlot = player.RightHandItemSlot;
			if (this.IsDaggerStack((rightHandItemSlot != null) ? rightHandItemSlot.Itemstack : null))
			{
				ItemSlot leftHandItemSlot = player.LeftHandItemSlot;
				return this.IsDaggerStack((leftHandItemSlot != null) ? leftHandItemSlot.Itemstack : null);
			}
			return false;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x00025CE4 File Offset: 0x00023EE4
		[NullableContext(2)]
		protected virtual bool IsDaggerStack(ItemStack stack)
		{
			return CollectibleClassifier.IsDagger(stack);
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00025CEC File Offset: 0x00023EEC
		[return: Nullable(2)]
		protected virtual MeleeWeaponStats TryGetMeleeWeaponStats(ItemStack stack)
		{
			MeleeWeaponStats result;
			try
			{
				CollectibleObject collectible = stack.Collectible;
				MeleeWeaponStats meleeWeaponStats;
				if (collectible == null)
				{
					meleeWeaponStats = null;
				}
				else
				{
					JsonObject attributes = collectible.Attributes;
					meleeWeaponStats = ((attributes != null) ? attributes.AsObject<MeleeWeaponStats>(null) : null);
				}
				result = meleeWeaponStats;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00025D34 File Offset: 0x00023F34
		[return: Nullable(2)]
		protected virtual string GetAttackAnimationForDirection(StanceStats stats, AttackDirection direction, int counter)
		{
			if (stats.AttackAnimation == null || stats.AttackAnimation.Count == 0)
			{
				return null;
			}
			string directionKey = direction.ToString();
			string[] directionalAnimations;
			string[] animations;
			string[] mainAnimations;
			if (this.DirectionsType != DirectionsConfiguration.None && stats.AttackAnimation.TryGetValue(directionKey, ref directionalAnimations))
			{
				animations = directionalAnimations;
			}
			else if (stats.AttackAnimation.TryGetValue("Main", ref mainAnimations))
			{
				animations = mainAnimations;
			}
			else
			{
				animations = Enumerable.FirstOrDefault<string[]>(stats.AttackAnimation.Values, (string[] value) => value != null && value.Length != 0);
			}
			if (animations == null || animations.Length == 0)
			{
				return null;
			}
			return animations[counter % animations.Length];
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00025DE0 File Offset: 0x00023FE0
		protected virtual bool AttackImpactFunction(TimeSpan duration, ref TimeSpan delta)
		{
			TimeSpan totalDuration = TimeSpan.FromMilliseconds((double)this.Stats.AnimationStaggerOnHitDurationMs);
			delta = TimeSpan.Zero;
			return duration < totalDuration;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00025E14 File Offset: 0x00024014
		protected virtual bool AttackAnimationCallback(EntityPlayer player, bool mainHand)
		{
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayReadyAnimation(mainHand);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.PlayReadyAnimation(mainHand);
			}
			if (this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[]
			{
				MeleeWeaponState.Cooldown,
				MeleeWeaponState.Attacking,
				MeleeWeaponState.WindingUp
			}))
			{
				this.SetState<MeleeWeaponState>(MeleeWeaponState.Idle, mainHand);
			}
			if (!this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[1]))
			{
				this.MeleeAttackSystem.UpdateAttackStatus(player, MeleeAttackStatus.End, mainHand);
			}
			if (mainHand)
			{
				this.MainHandAttackCounter = 0;
			}
			else
			{
				this.OffHandAttackCounter = 0;
			}
			return true;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00025E98 File Offset: 0x00024098
		protected virtual void AttackAnimationCallbackHandler(EntityPlayer player, string callbackCode, bool mainHand)
		{
			if (callbackCode == "start")
			{
				this.StopAttackCooldown(mainHand);
				this.SetState<MeleeWeaponState>(MeleeWeaponState.Attacking, mainHand);
				return;
			}
			if (callbackCode == "stop")
			{
				this.SetState<MeleeWeaponState>(MeleeWeaponState.Cooldown, mainHand);
				return;
			}
			if (!(callbackCode == "ready"))
			{
				return;
			}
			this.MeleeAttackSystem.UpdateAttackStatus(player, MeleeAttackStatus.End, mainHand);
			this.SetState<MeleeWeaponState>(MeleeWeaponState.Idle, mainHand);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00025EFC File Offset: 0x000240FC
		[ActionEventHandler(9, ActionState.Released)]
		protected virtual bool StopAttack(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			bool active = this.Active;
			return false;
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00025F08 File Offset: 0x00024108
		[ActionEventHandler(10, ActionState.Active)]
		protected virtual bool Block(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			MeleeWeaponClient.<>c__DisplayClass138_0 CS$<>8__locals1 = new MeleeWeaponClient.<>c__DisplayClass138_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.mainHand = mainHand;
			CS$<>8__locals1.player = player;
			if (!this.Active)
			{
				return false;
			}
			if (this.CanUseDualWieldPrimedThrow(CS$<>8__locals1.player, CS$<>8__locals1.mainHand) && this.CheckState<MeleeWeaponState>(CS$<>8__locals1.mainHand, new MeleeWeaponState[1]) && this.CanThrow(CS$<>8__locals1.player, CS$<>8__locals1.mainHand) && this.Stats.ThrowAttack != null && this.AimingStats != null)
			{
				return false;
			}
			bool handleEvent = !this.Settings.VanillaActionsWhileBlocking;
			if (InteractionsTester.PlayerTriesToInteract(CS$<>8__locals1.player, CS$<>8__locals1.mainHand, eventData))
			{
				return false;
			}
			if (!this.CanBlock(CS$<>8__locals1.player, CS$<>8__locals1.mainHand) && !this.CanParry(CS$<>8__locals1.player, CS$<>8__locals1.mainHand))
			{
				return false;
			}
			if (!this.CheckState<MeleeWeaponState>(CS$<>8__locals1.mainHand, new MeleeWeaponState[]
			{
				MeleeWeaponState.Idle,
				MeleeWeaponState.WindingUp,
				MeleeWeaponState.Cooldown
			}))
			{
				return handleEvent;
			}
			if (this.IsBlockOnCooldown(CS$<>8__locals1.mainHand))
			{
				return handleEvent;
			}
			this.EnsureStance(CS$<>8__locals1.player, CS$<>8__locals1.mainHand);
			if (CS$<>8__locals1.mainHand && this.CanBlockWithOtherHand(CS$<>8__locals1.player, CS$<>8__locals1.mainHand))
			{
				return handleEvent;
			}
			if (this.ActionRestricted(CS$<>8__locals1.player, CS$<>8__locals1.mainHand))
			{
				return handleEvent;
			}
			CS$<>8__locals1.stats = this.GetStanceStats(CS$<>8__locals1.player, CS$<>8__locals1.mainHand);
			MeleeWeaponClient.<>c__DisplayClass138_0 CS$<>8__locals2 = CS$<>8__locals1;
			StanceStats stats = CS$<>8__locals1.stats;
			DamageBlockJson parryStats;
			if (stats == null)
			{
				parryStats = null;
			}
			else
			{
				DamageBlockJson parry = stats.Parry;
				parryStats = ((parry != null) ? parry.Clone() : null);
			}
			CS$<>8__locals2.parryStats = parryStats;
			MeleeWeaponClient.<>c__DisplayClass138_0 CS$<>8__locals3 = CS$<>8__locals1;
			StanceStats stats2 = CS$<>8__locals1.stats;
			DamageBlockJson blockStats;
			if (stats2 == null)
			{
				blockStats = null;
			}
			else
			{
				DamageBlockJson block = stats2.Block;
				blockStats = ((block != null) ? block.Clone() : null);
			}
			CS$<>8__locals3.blockStats = blockStats;
			ItemStackMeleeWeaponStats stackStats = ItemStackMeleeWeaponStats.FromItemStack(slot.Itemstack);
			DamageBlockJson parryStats2 = CS$<>8__locals1.parryStats;
			string text;
			int num;
			if (((parryStats2 != null) ? parryStats2.BlockTier : null) != null)
			{
				foreach (KeyValuePair<string, int> keyValuePair in CS$<>8__locals1.parryStats.BlockTier)
				{
					keyValuePair.Deconstruct(ref text, ref num);
					float num2 = (float)num;
					string damageType = text;
					Dictionary<string, int> blockTier = CS$<>8__locals1.parryStats.BlockTier;
					text = damageType;
					blockTier[text] += stackStats.ParryTierBonus;
				}
				if (CS$<>8__locals1.player.Api.ModLoader.GetModSystem<CharacterSystem>(true).HasTrait(CS$<>8__locals1.player.Player, "canParryProjectiles"))
				{
					CS$<>8__locals1.parryStats.CanBlockProjectiles = true;
				}
			}
			DamageBlockJson blockStats2 = CS$<>8__locals1.blockStats;
			if (((blockStats2 != null) ? blockStats2.BlockTier : null) != null)
			{
				foreach (KeyValuePair<string, int> keyValuePair in CS$<>8__locals1.blockStats.BlockTier)
				{
					keyValuePair.Deconstruct(ref text, ref num);
					float num3 = (float)num;
					string damageType2 = text;
					Dictionary<string, int> blockTier = CS$<>8__locals1.blockStats.BlockTier;
					text = damageType2;
					blockTier[text] += stackStats.BlockTierBonus;
				}
			}
			if (this.CanParry(CS$<>8__locals1.player, CS$<>8__locals1.mainHand) && CS$<>8__locals1.parryStats != null && CS$<>8__locals1.stats != null)
			{
				if (!this.ParryButtonReleased)
				{
					return handleEvent;
				}
				this.SetState<MeleeWeaponState>(MeleeWeaponState.Parrying, CS$<>8__locals1.mainHand);
				if (CS$<>8__locals1.stats.ParryWithoutDelay)
				{
					this.MeleeBlockSystem.StartBlock(CS$<>8__locals1.parryStats, CS$<>8__locals1.mainHand, EnumDamageBlockKind.Parry, delegate()
					{
						CS$<>8__locals1.<>4__this.RiposteCallback(CS$<>8__locals1.mainHand, CS$<>8__locals1.player);
					});
				}
				FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
				if (animationBehavior != null)
				{
					bool mainHand2 = CS$<>8__locals1.mainHand;
					string animation;
					if ((animation = CS$<>8__locals1.stats.ParryAnimation) == null && (animation = CS$<>8__locals1.stats.BlockAnimation) == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						defaultInterpolatedStringHandler..ctor(75, 1);
						defaultInterpolatedStringHandler.AppendLiteral("[Combat Overhaul] Item '");
						defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(this.Item.Code);
						defaultInterpolatedStringHandler.AppendLiteral("' does not have block or parry animations specified");
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
					float animationSpeed = (playerActionsBehavior != null) ? playerActionsBehavior.ManipulationSpeed : 1f;
					animationBehavior.Play(mainHand2, animation, MeleeWeaponClient.AnimationCategory(CS$<>8__locals1.mainHand), animationSpeed, 1f, () => CS$<>8__locals1.<>4__this.BlockAnimationCallback(CS$<>8__locals1.mainHand, CS$<>8__locals1.player, TimeSpan.FromMilliseconds((double)CS$<>8__locals1.stats.ParryCooldownMs)), delegate(string code)
					{
						CS$<>8__locals1.<>4__this.BlockAnimationCallbackHandler(code, CS$<>8__locals1.mainHand, CS$<>8__locals1.blockStats, CS$<>8__locals1.parryStats, CS$<>8__locals1.player);
					}, true);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
				if (tpAnimationBehavior != null)
				{
					bool mainHand3 = CS$<>8__locals1.mainHand;
					string animation2;
					if ((animation2 = CS$<>8__locals1.stats.ParryAnimation) == null && (animation2 = CS$<>8__locals1.stats.BlockAnimation) == null)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
						defaultInterpolatedStringHandler2..ctor(75, 1);
						defaultInterpolatedStringHandler2.AppendLiteral("[Combat Overhaul] Item '");
						defaultInterpolatedStringHandler2.AppendFormatted<AssetLocation>(this.Item.Code);
						defaultInterpolatedStringHandler2.AppendLiteral("' does not have block or parry animations specified");
						throw new InvalidOperationException(defaultInterpolatedStringHandler2.ToStringAndClear());
					}
					ActionsManagerPlayerBehavior playerActionsBehavior2 = this.PlayerActionsBehavior;
					float animationSpeed = (playerActionsBehavior2 != null) ? playerActionsBehavior2.ManipulationSpeed : 1f;
					tpAnimationBehavior.Play(mainHand3, animation2, MeleeWeaponClient.AnimationCategory(CS$<>8__locals1.mainHand), animationSpeed, 1f, true);
				}
				this.ParryButtonReleased = false;
			}
			else if (this.CanBlock(CS$<>8__locals1.player, CS$<>8__locals1.mainHand) && CS$<>8__locals1.blockStats != null && CS$<>8__locals1.stats != null)
			{
				this.SetState<MeleeWeaponState>(MeleeWeaponState.Blocking, CS$<>8__locals1.mainHand);
				this.MeleeBlockSystem.StartBlock(CS$<>8__locals1.blockStats, CS$<>8__locals1.mainHand, EnumDamageBlockKind.Block);
				if (this.ShouldUseVanillaShieldRaiseAnimation(slot, CS$<>8__locals1.mainHand))
				{
					this.PlayVanillaShieldRaiseAnimation(CS$<>8__locals1.player, CS$<>8__locals1.mainHand);
				}
				else
				{
					FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
					if (animationBehavior2 != null)
					{
						bool mainHand4 = CS$<>8__locals1.mainHand;
						string blockAnimation = CS$<>8__locals1.stats.BlockAnimation;
						if (blockAnimation == null)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
							defaultInterpolatedStringHandler3..ctor(75, 1);
							defaultInterpolatedStringHandler3.AppendLiteral("[Combat Overhaul] Item '");
							defaultInterpolatedStringHandler3.AppendFormatted<AssetLocation>(this.Item.Code);
							defaultInterpolatedStringHandler3.AppendLiteral("' does not have block or parry animations specified");
							throw new InvalidOperationException(defaultInterpolatedStringHandler3.ToStringAndClear());
						}
						float animationSpeed = this.GetAnimationSpeed(CS$<>8__locals1.player, this.Stats, 0.5f, 2f);
						animationBehavior2.Play(mainHand4, blockAnimation, MeleeWeaponClient.AnimationCategory(CS$<>8__locals1.mainHand), animationSpeed, 1f, () => CS$<>8__locals1.<>4__this.BlockAnimationCallback(CS$<>8__locals1.mainHand, CS$<>8__locals1.player, TimeSpan.FromMilliseconds((double)CS$<>8__locals1.stats.BlockCooldownMs)), delegate(string code)
						{
							CS$<>8__locals1.<>4__this.BlockAnimationCallbackHandler(code, CS$<>8__locals1.mainHand, CS$<>8__locals1.blockStats, CS$<>8__locals1.parryStats, CS$<>8__locals1.player);
						}, true);
					}
					ThirdPersonAnimationsBehavior tpAnimationBehavior2 = this.TpAnimationBehavior;
					if (tpAnimationBehavior2 != null)
					{
						bool mainHand5 = CS$<>8__locals1.mainHand;
						string blockAnimation2 = CS$<>8__locals1.stats.BlockAnimation;
						if (blockAnimation2 == null)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4;
							defaultInterpolatedStringHandler4..ctor(75, 1);
							defaultInterpolatedStringHandler4.AppendLiteral("[Combat Overhaul] Item '");
							defaultInterpolatedStringHandler4.AppendFormatted<AssetLocation>(this.Item.Code);
							defaultInterpolatedStringHandler4.AppendLiteral("' does not have block or parry animations specified");
							throw new InvalidOperationException(defaultInterpolatedStringHandler4.ToStringAndClear());
						}
						float animationSpeed = this.GetAnimationSpeed(CS$<>8__locals1.player, this.Stats, 0.5f, 2f);
						tpAnimationBehavior2.Play(mainHand5, blockAnimation2, MeleeWeaponClient.AnimationCategory(CS$<>8__locals1.mainHand), animationSpeed, 1f, true);
					}
				}
			}
			this.SetSpeedPenalty(CS$<>8__locals1.mainHand, CS$<>8__locals1.player);
			return handleEvent;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x000265DC File Offset: 0x000247DC
		protected virtual void BlockAnimationCallbackHandler(string callbackCode, bool mainHand, [Nullable(2)] DamageBlockJson blockStats, [Nullable(2)] DamageBlockJson parryStats, EntityPlayer player)
		{
			if (!(callbackCode == "startParry"))
			{
				if (!(callbackCode == "stopParry"))
				{
					return;
				}
				if (this.CanBlock(player, mainHand) && blockStats != null)
				{
					ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
					if (playerActionsBehavior != null && playerActionsBehavior.ActionListener.IsActive(10))
					{
						this.SetState<MeleeWeaponState>(MeleeWeaponState.Blocking, mainHand);
						this.MeleeBlockSystem.StartBlock(blockStats, mainHand, EnumDamageBlockKind.Block);
						return;
					}
				}
				this.MeleeBlockSystem.StopBlock(mainHand);
			}
			else if (this.CanParry(player, mainHand) && parryStats != null)
			{
				this.SetState<MeleeWeaponState>(MeleeWeaponState.Parrying, mainHand);
				this.MeleeBlockSystem.StartBlock(parryStats, mainHand, EnumDamageBlockKind.Parry, delegate()
				{
					this.RiposteCallback(mainHand, player);
				});
				return;
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x000266D0 File Offset: 0x000248D0
		protected virtual bool BlockAnimationCallback(bool mainHand, EntityPlayer player, TimeSpan cooldown)
		{
			if (!this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[]
			{
				MeleeWeaponState.Parrying
			}))
			{
				return true;
			}
			this.SetState<MeleeWeaponState>(MeleeWeaponState.Idle, mainHand);
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayReadyAnimation(mainHand);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.PlayReadyAnimation(mainHand);
			}
			if (cooldown > TimeSpan.Zero)
			{
				this.StartBlockCooldown(mainHand, cooldown);
			}
			this.SetSpeedPenalty(mainHand, player);
			this.ResetRiposte(mainHand);
			return true;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00026743 File Offset: 0x00024943
		protected virtual void RiposteCallback(bool mainHand, EntityPlayer player)
		{
			if (!this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[]
			{
				MeleeWeaponState.Parrying,
				MeleeWeaponState.Blocking
			}))
			{
				return;
			}
			this.SetRiposte(mainHand, true);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00026765 File Offset: 0x00024965
		protected virtual void SetRiposte(bool mainHand, bool value)
		{
			if (mainHand)
			{
				MeleeWeaponClient.CanRiposteMainHand = value;
				this.Api.World.UnregisterCallback(MeleeWeaponClient.RiposteTimerMainHand);
				return;
			}
			MeleeWeaponClient.CanRiposteOffHand = value;
			this.Api.World.UnregisterCallback(MeleeWeaponClient.RiposteTimerOffHand);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x000267A4 File Offset: 0x000249A4
		protected virtual void ResetRiposte(bool mainHand)
		{
			if (mainHand)
			{
				this.Api.World.UnregisterCallback(MeleeWeaponClient.RiposteTimerMainHand);
				MeleeWeaponClient.RiposteTimerMainHand = this.Api.World.RegisterCallback(delegate(float _)
				{
					this.SetRiposte(mainHand, false);
				}, 300);
				return;
			}
			this.Api.World.UnregisterCallback(MeleeWeaponClient.RiposteTimerOffHand);
			MeleeWeaponClient.RiposteTimerOffHand = this.Api.World.RegisterCallback(delegate(float _)
			{
				this.SetRiposte(mainHand, false);
			}, 300);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00026844 File Offset: 0x00024A44
		protected virtual void TurnOnRiposte(bool mainHand)
		{
			if (mainHand)
			{
				this.RiposteMainHand = true;
				return;
			}
			this.RiposteOffHand = true;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00026858 File Offset: 0x00024A58
		protected virtual void TurnOffRiposte(bool mainHand)
		{
			if (mainHand)
			{
				this.RiposteMainHand = false;
				return;
			}
			this.RiposteOffHand = false;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0002686C File Offset: 0x00024A6C
		protected virtual bool RiposteActive(bool mainHand)
		{
			if (mainHand)
			{
				return this.RiposteMainHand;
			}
			return this.RiposteOffHand;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00026880 File Offset: 0x00024A80
		[ActionEventHandler(10, ActionState.Released)]
		protected virtual bool StopBlock(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (!this.Active)
			{
				return false;
			}
			this.ParryButtonReleased = true;
			if (!this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[]
			{
				MeleeWeaponState.Blocking
			}))
			{
				return false;
			}
			this.MeleeBlockSystem.StopBlock(mainHand);
			if (this.ShouldUseVanillaShieldRaiseAnimation(slot, mainHand))
			{
				this.StopVanillaShieldRaiseAnimation(player, mainHand);
			}
			else
			{
				FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
				if (animationBehavior != null)
				{
					animationBehavior.PlayReadyAnimation(mainHand);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
				if (tpAnimationBehavior != null)
				{
					tpAnimationBehavior.PlayReadyAnimation(mainHand);
				}
			}
			this.SetState<MeleeWeaponState>(MeleeWeaponState.Idle, mainHand);
			StanceStats stanceStats = this.GetStanceStats(player, mainHand);
			float cooldown = (stanceStats != null) ? stanceStats.BlockCooldownMs : 0f;
			if (cooldown != 0f)
			{
				this.StartBlockCooldown(mainHand, TimeSpan.FromMilliseconds((double)cooldown));
			}
			this.SetSpeedPenalty(mainHand, player);
			this.ResetRiposte(mainHand);
			return true;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x0002694C File Offset: 0x00024B4C
		[ActionEventHandler(9, ActionState.Active)]
		protected virtual bool Bash(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (!this.Active)
			{
				return false;
			}
			if (InteractionsTester.PlayerTriesToInteract(player, mainHand, eventData))
			{
				return false;
			}
			this.EnsureStance(player, mainHand);
			if (!this.CanBash(player, mainHand))
			{
				return false;
			}
			if (this.IsAttackOnCooldown(mainHand))
			{
				return false;
			}
			if (this.ActionRestricted(player, mainHand))
			{
				return false;
			}
			if (!mainHand && this.CanThrowWithOtherHand(player, mainHand))
			{
				return false;
			}
			MeleeAttack attack = this.GetStanceBlockBash(player, mainHand, direction);
			StanceStats stats = this.GetStanceStats(player, mainHand);
			MeleeAttack handle = this.GetStanceHandleAttack(player, mainHand);
			if (attack == null || stats == null)
			{
				return false;
			}
			switch (this.GetState<MeleeWeaponState>(mainHand))
			{
			case MeleeWeaponState.Blocking:
			{
				if (mainHand)
				{
					this.CurrentMainHandDirection = direction;
				}
				else
				{
					this.CurrentOffHandDirection = direction;
				}
				int counter = mainHand ? this.MainHandAttackCounter : this.OffHandAttackCounter;
				string attackAnimation = (this.DirectionsType == DirectionsConfiguration.None) ? stats.BlockBashAnimation["Main"][counter % stats.BlockBashAnimation["Main"].Length] : stats.BlockBashAnimation[direction.ToString()][counter % stats.BlockBashAnimation[direction.ToString()].Length];
				float animationSpeed = this.GetAnimationSpeed(player, this.Stats, 0.5f, 2f) * ItemStackMeleeWeaponStats.GetAttackSpeed(slot.Itemstack) * stats.AttackSpeedMultiplier;
				this.SetState<MeleeWeaponState>(MeleeWeaponState.BlockBashWindingUp, mainHand);
				this.ParryButtonReleased = true;
				this.MeleeAttackSystem.UpdateAttackStatus(player, MeleeAttackStatus.Start, mainHand);
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
					float animationSpeed2 = animationSpeed;
					animationBehavior.Play(mainHand2, animation, MeleeWeaponClient.AnimationCategory(mainHand), animationSpeed2, 1f, () => this.BashAnimationCallback(slot, player, mainHand), delegate(string code)
					{
						this.BashAnimationCallbackHandler(player, code, mainHand);
					}, true);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
				if (tpAnimationBehavior != null)
				{
					bool mainHand3 = mainHand;
					string animation2 = attackAnimation;
					float animationSpeed2 = animationSpeed;
					tpAnimationBehavior.Play(mainHand3, animation2, MeleeWeaponClient.AnimationCategory(mainHand), animationSpeed2, 1f, true);
				}
				if (mainHand)
				{
					this.MainHandAttackCounter++;
				}
				else
				{
					this.OffHandAttackCounter++;
				}
				this.HandleHitTerrain = false;
				break;
			}
			case MeleeWeaponState.BlockBashWindingUp:
			case MeleeWeaponState.BlockBashAttacking:
			case MeleeWeaponState.BlockBashCooldown:
				break;
			default:
				return false;
			}
			this.SetSpeedPenalty(mainHand, player);
			return true;
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00026C58 File Offset: 0x00024E58
		protected virtual void TryBash(MeleeAttack attack, [Nullable(2)] MeleeAttack handle, StanceStats stats, ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			ItemStackMeleeWeaponStats stackStats = ItemStackMeleeWeaponStats.FromItemStack(slot.Itemstack);
			if (handle != null)
			{
				IEnumerable<ValueTuple<Block, Vector3d>> handleTerrainCollision;
				IEnumerable<ValueTuple<Entity, Vector3d>> handleEntitiesCollision;
				bool hanldeAttacked = handle.Attack(player.Player, slot, mainHand, out handleTerrainCollision, out handleEntitiesCollision, stackStats, AttackDirection.Top, this.Settings.MeleeWeaponStopOnTerrainHit);
				if (!this.HandleHitTerrain && hanldeAttacked)
				{
					if (stats.HandleHitSound != null)
					{
						this.SoundsSystem.Play(stats.HandleHitSound, false, 32f, 1f, true);
					}
					this.HandleHitTerrain = true;
				}
				if (hanldeAttacked)
				{
					return;
				}
			}
			IEnumerable<ValueTuple<Block, Vector3d>> terrainCollision;
			IEnumerable<ValueTuple<Entity, Vector3d>> entitiesCollision;
			bool flag = attack.Attack(player.Player, slot, mainHand, out terrainCollision, out entitiesCollision, stackStats, AttackDirection.Top, this.Settings.MeleeWeaponStopOnTerrainHit);
			if (handle != null)
			{
				handle.AddAttackedEntities(attack);
			}
			if (flag && stats.BashHitSound != null)
			{
				this.SoundsSystem.Play(stats.BashHitSound, false, 32f, 1f, true);
			}
			if (flag && this.Stats.AnimationStaggerOnHitDurationMs > 0f)
			{
				FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
				if (animationBehavior != null)
				{
					animationBehavior.SetSpeedModifier(new AnimationSpeedModifierDelegate(this.AttackImpactFunction));
				}
				this.Api.World.AddCameraShake(this.Stats.ScreenShakeStrength);
			}
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00026D78 File Offset: 0x00024F78
		protected virtual bool BashAnimationCallback(ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
			bool flag = playerActionsBehavior != null && playerActionsBehavior.ActionListener.IsActive(10);
			this.MeleeAttackSystem.UpdateAttackStatus(player, MeleeAttackStatus.End, mainHand);
			if (!flag)
			{
				if (this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[]
				{
					MeleeWeaponState.BlockBashCooldown,
					MeleeWeaponState.BlockBashAttacking,
					MeleeWeaponState.BlockBashWindingUp
				}))
				{
					this.SetState<MeleeWeaponState>(MeleeWeaponState.Idle, mainHand);
				}
				if (this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[1]))
				{
					FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
					if (animationBehavior != null)
					{
						animationBehavior.PlayReadyAnimation(mainHand);
					}
					ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
					if (tpAnimationBehavior != null)
					{
						tpAnimationBehavior.PlayReadyAnimation(mainHand);
					}
				}
				this.MeleeBlockSystem.StopBlock(mainHand);
			}
			else
			{
				if (this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[]
				{
					MeleeWeaponState.BlockBashCooldown,
					MeleeWeaponState.BlockBashAttacking,
					MeleeWeaponState.BlockBashWindingUp
				}))
				{
					this.SetState<MeleeWeaponState>(MeleeWeaponState.Blocking, mainHand);
				}
				this.ReturnToBlock(slot, player, mainHand);
			}
			if (mainHand)
			{
				this.MainHandAttackCounter = 0;
			}
			else
			{
				this.OffHandAttackCounter = 0;
			}
			return true;
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00026E50 File Offset: 0x00025050
		protected virtual void BashAnimationCallbackHandler(EntityPlayer player, string callbackCode, bool mainHand)
		{
			ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
			bool rightMouseDown = playerActionsBehavior != null && playerActionsBehavior.ActionListener.IsActive(10);
			if (callbackCode == "start")
			{
				this.SetState<MeleeWeaponState>(MeleeWeaponState.BlockBashAttacking, mainHand);
				return;
			}
			if (callbackCode == "stop")
			{
				this.SetState<MeleeWeaponState>(MeleeWeaponState.BlockBashCooldown, mainHand);
				return;
			}
			if (!(callbackCode == "ready"))
			{
				return;
			}
			this.MeleeAttackSystem.UpdateAttackStatus(player, MeleeAttackStatus.End, mainHand);
			this.SetState<MeleeWeaponState>(rightMouseDown ? MeleeWeaponState.BlockBashCooldown : MeleeWeaponState.Idle, mainHand);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00026ED0 File Offset: 0x000250D0
		protected virtual void ReturnToBlock(ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			MeleeWeaponClient.<>c__DisplayClass152_0 CS$<>8__locals1 = new MeleeWeaponClient.<>c__DisplayClass152_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.mainHand = mainHand;
			CS$<>8__locals1.player = player;
			CS$<>8__locals1.stats = this.GetStanceStats(CS$<>8__locals1.player, CS$<>8__locals1.mainHand);
			MeleeWeaponClient.<>c__DisplayClass152_0 CS$<>8__locals2 = CS$<>8__locals1;
			StanceStats stats = CS$<>8__locals1.stats;
			DamageBlockJson parryStats;
			if (stats == null)
			{
				parryStats = null;
			}
			else
			{
				DamageBlockJson parry = stats.Parry;
				parryStats = ((parry != null) ? parry.Clone() : null);
			}
			CS$<>8__locals2.parryStats = parryStats;
			MeleeWeaponClient.<>c__DisplayClass152_0 CS$<>8__locals3 = CS$<>8__locals1;
			StanceStats stats2 = CS$<>8__locals1.stats;
			DamageBlockJson blockStats;
			if (stats2 == null)
			{
				blockStats = null;
			}
			else
			{
				DamageBlockJson block = stats2.Block;
				blockStats = ((block != null) ? block.Clone() : null);
			}
			CS$<>8__locals3.blockStats = blockStats;
			ItemStackMeleeWeaponStats stackStats = ItemStackMeleeWeaponStats.FromItemStack(slot.Itemstack);
			DamageBlockJson parryStats2 = CS$<>8__locals1.parryStats;
			string text;
			int num;
			if (((parryStats2 != null) ? parryStats2.BlockTier : null) != null)
			{
				foreach (KeyValuePair<string, int> keyValuePair in CS$<>8__locals1.parryStats.BlockTier)
				{
					keyValuePair.Deconstruct(ref text, ref num);
					float num2 = (float)num;
					string damageType = text;
					Dictionary<string, int> blockTier = CS$<>8__locals1.parryStats.BlockTier;
					text = damageType;
					blockTier[text] += stackStats.ParryTierBonus;
				}
			}
			DamageBlockJson blockStats2 = CS$<>8__locals1.blockStats;
			if (((blockStats2 != null) ? blockStats2.BlockTier : null) != null)
			{
				foreach (KeyValuePair<string, int> keyValuePair in CS$<>8__locals1.blockStats.BlockTier)
				{
					keyValuePair.Deconstruct(ref text, ref num);
					float num3 = (float)num;
					string damageType2 = text;
					Dictionary<string, int> blockTier = CS$<>8__locals1.blockStats.BlockTier;
					text = damageType2;
					blockTier[text] += stackStats.BlockTierBonus;
				}
			}
			if (this.CanBlock(CS$<>8__locals1.player, CS$<>8__locals1.mainHand) && CS$<>8__locals1.blockStats != null && CS$<>8__locals1.stats != null)
			{
				this.SetState<MeleeWeaponState>(MeleeWeaponState.Blocking, CS$<>8__locals1.mainHand);
				this.MeleeBlockSystem.StartBlock(CS$<>8__locals1.blockStats, CS$<>8__locals1.mainHand, EnumDamageBlockKind.Block);
				FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
				if (animationBehavior != null)
				{
					bool mainHand2 = CS$<>8__locals1.mainHand;
					string blockAnimation = CS$<>8__locals1.stats.BlockAnimation;
					float animationSpeed = this.GetAnimationSpeed(CS$<>8__locals1.player, this.Stats, 0.5f, 2f);
					animationBehavior.Play(mainHand2, blockAnimation, MeleeWeaponClient.AnimationCategory(CS$<>8__locals1.mainHand), animationSpeed, 1f, () => CS$<>8__locals1.<>4__this.BlockAnimationCallback(CS$<>8__locals1.mainHand, CS$<>8__locals1.player, TimeSpan.FromMilliseconds((double)CS$<>8__locals1.stats.BlockCooldownMs)), delegate(string code)
					{
						CS$<>8__locals1.<>4__this.BlockAnimationCallbackHandler(code, CS$<>8__locals1.mainHand, CS$<>8__locals1.blockStats, CS$<>8__locals1.parryStats, CS$<>8__locals1.player);
					}, true);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
				if (tpAnimationBehavior != null)
				{
					bool mainHand3 = CS$<>8__locals1.mainHand;
					string blockAnimation2 = CS$<>8__locals1.stats.BlockAnimation;
					float animationSpeed = this.GetAnimationSpeed(CS$<>8__locals1.player, this.Stats, 0.5f, 2f);
					tpAnimationBehavior.Play(mainHand3, blockAnimation2, MeleeWeaponClient.AnimationCategory(CS$<>8__locals1.mainHand), animationSpeed, 1f, true);
				}
			}
			this.SetSpeedPenalty(CS$<>8__locals1.mainHand, CS$<>8__locals1.player);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x000271AC File Offset: 0x000253AC
		protected virtual bool TryStartDualWieldPrimedThrow(ItemSlot slot, EntityPlayer player, ActionEventData eventData, bool mainHand)
		{
			if (!this.CanUseDualWieldPrimedThrow(player, mainHand))
			{
				return false;
			}
			if (InteractionsTester.PlayerTriesToInteract(player, mainHand, eventData))
			{
				return false;
			}
			if (!this.CanThrow(player, mainHand) || this.Stats.ThrowAttack == null || this.AimingStats == null)
			{
				return false;
			}
			if (!this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[1]))
			{
				return false;
			}
			this.MainHandPrimedDualWieldThrow = true;
			this.StartThrowAiming(slot, player, mainHand);
			return true;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00027219 File Offset: 0x00025419
		protected virtual bool CanUseDualWieldPrimedThrow(EntityPlayer player, bool mainHand)
		{
			return mainHand && this.IsDualDaggerLoadout(player);
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00027227 File Offset: 0x00025427
		protected virtual void ResetPrimedDualWieldThrow(bool mainHand)
		{
			if (mainHand)
			{
				this.MainHandPrimedDualWieldThrow = false;
			}
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x00027234 File Offset: 0x00025434
		protected virtual void StartThrowAiming(ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			this.SetState<MeleeWeaponState>(MeleeWeaponState.StartingAim, mainHand);
			this.AimingSystem.AimingState = WeaponAimingState.Blocked;
			AimingAnimationController aimingAnimationController = this.AimingAnimationController;
			if (aimingAnimationController != null)
			{
				aimingAnimationController.Play(mainHand);
			}
			this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.StartAiming, mainHand);
			ItemStackMeleeWeaponStats stackStats = ItemStackMeleeWeaponStats.FromItemStack(slot.Itemstack);
			this.AimingStats.Clone().AimDifficulty *= stackStats.ThrownAimingDifficulty;
			this.AimingStats.CursorType = Enum.Parse<AimingCursorType>(this.Settings.ThrownWeaponsCursorType, true);
			this.AimingStats.VerticalLimit = this.Settings.ThrownWeaponsAimingVerticalLimit * this.DefaultVerticalLimit;
			this.AimingStats.HorizontalLimit = this.Settings.ThrownWeaponsAimingHorizontalLimit * this.DefaultHorizontalLimit;
			this.AimingSystem.StartAiming(this.AimingStats);
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.Play(mainHand, this.Stats.ThrowAttack.AimAnimation, "main", this.GetAnimationSpeed(player, this.Stats, 0.5f, 2f), 1f, () => this.AimAnimationCallback(slot, mainHand), null, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.Play(mainHand, this.Stats.ThrowAttack.AimAnimation, "main", this.GetAnimationSpeed(player, this.Stats, 0.5f, 2f), 1f, true);
			}
			if (this.TpAnimationBehavior == null)
			{
				FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
				if (animationBehavior2 == null)
				{
					return;
				}
				animationBehavior2.PlayVanillaAnimation(this.Stats.ThrowAttack.TpAimAnimation, mainHand);
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00027404 File Offset: 0x00025604
		protected virtual void TryRefillAfterPrimedThrow(EntityPlayer player, ItemSlot thrownSlot, bool mainHand, [Nullable(2)] ItemStack thrownReferenceStack = null)
		{
			if (!mainHand || !this.MainHandPrimedDualWieldThrow)
			{
				return;
			}
			if (((thrownSlot != null) ? thrownSlot.Itemstack : null) != null && thrownSlot.Itemstack.StackSize > 0)
			{
				return;
			}
			ItemSlot replacement = this.FindInventoryReplacementForThrown(player, thrownSlot, thrownReferenceStack);
			if (replacement != null)
			{
				thrownSlot.TryFlipWith(replacement);
				thrownSlot.MarkDirty();
				replacement.MarkDirty();
				return;
			}
			ItemSlot offhandSlot = player.LeftHandItemSlot;
			if (this.IsDaggerStack((offhandSlot != null) ? offhandSlot.Itemstack : null))
			{
				thrownSlot.TryFlipWith(offhandSlot);
				thrownSlot.MarkDirty();
				offhandSlot.MarkDirty();
			}
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x0002748C File Offset: 0x0002568C
		[return: Nullable(2)]
		protected virtual ItemSlot FindInventoryReplacementForThrown(EntityPlayer player, ItemSlot thrownSlot, [Nullable(2)] ItemStack thrownReferenceStack = null)
		{
			MeleeWeaponClient.<>c__DisplayClass158_0 CS$<>8__locals1;
			CS$<>8__locals1.thrownSlot = thrownSlot;
			CS$<>8__locals1.player = player;
			CS$<>8__locals1.<>4__this = this;
			ItemStack itemStack = thrownReferenceStack;
			if (thrownReferenceStack == null)
			{
				ItemSlot rightHandItemSlot = CS$<>8__locals1.player.RightHandItemSlot;
				itemStack = (((rightHandItemSlot != null) ? rightHandItemSlot.Itemstack : null) ?? CS$<>8__locals1.thrownSlot.Itemstack);
			}
			ItemStack referenceStack = itemStack;
			string text;
			if (referenceStack == null)
			{
				text = null;
			}
			else
			{
				CollectibleObject collectible = referenceStack.Collectible;
				if (collectible == null)
				{
					text = null;
				}
				else
				{
					AssetLocation code = collectible.Code;
					text = ((code != null) ? code.ToString() : null);
				}
			}
			CS$<>8__locals1.referenceCode = (text ?? "");
			CS$<>8__locals1.anyDaggerReplacementAllowed = this.IsDaggerStack(referenceStack);
			if (CS$<>8__locals1.referenceCode == "" && !CS$<>8__locals1.anyDaggerReplacementAllowed)
			{
				return null;
			}
			CS$<>8__locals1.replacementSlot = null;
			CS$<>8__locals1.scannedSlots = new HashSet<ItemSlot>();
			IPlayer player2 = CS$<>8__locals1.player.Player;
			IPlayerInventoryManager inventoryManager = (player2 != null) ? player2.InventoryManager : null;
			this.<FindInventoryReplacementForThrown>g__ScanInventory|158_0((inventoryManager != null) ? inventoryManager.GetOwnInventory("hotbar") : null, ref CS$<>8__locals1);
			this.<FindInventoryReplacementForThrown>g__ScanInventory|158_0((inventoryManager != null) ? inventoryManager.GetOwnInventory("backpack") : null, ref CS$<>8__locals1);
			EntityBehaviorPlayerInventory behavior = CS$<>8__locals1.player.GetBehavior<EntityBehaviorPlayerInventory>();
			this.<FindInventoryReplacementForThrown>g__ScanInventory|158_0((behavior != null) ? behavior.Inventory : null, ref CS$<>8__locals1);
			if (((inventoryManager != null) ? inventoryManager.Inventories : null) != null)
			{
				foreach (KeyValuePair<string, IInventory> keyValuePair in inventoryManager.Inventories)
				{
					string text2;
					IInventory inventory2;
					keyValuePair.Deconstruct(ref text2, ref inventory2);
					IInventory inventory = inventory2;
					if (CS$<>8__locals1.replacementSlot != null)
					{
						break;
					}
					if (inventory != null && !(inventory is InventoryPlayerCreative))
					{
						this.<FindInventoryReplacementForThrown>g__ScanInventory|158_0(inventory, ref CS$<>8__locals1);
					}
				}
			}
			return CS$<>8__locals1.replacementSlot;
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x0002763C File Offset: 0x0002583C
		[ActionEventHandler(10, ActionState.Active)]
		protected virtual bool Aim(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (!this.Active)
			{
				return false;
			}
			if (InteractionsTester.PlayerTriesToInteract(player, mainHand, eventData))
			{
				return false;
			}
			if (!this.CanThrow(player, mainHand) || this.Stats.ThrowAttack == null || this.AimingStats == null)
			{
				return false;
			}
			if (!this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[1]))
			{
				return false;
			}
			if (mainHand && this.CanBlockWithOtherHand(player, mainHand) && !this.CanUseDualWieldPrimedThrow(player, mainHand))
			{
				return false;
			}
			this.StartThrowAiming(slot, player, mainHand);
			return true;
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x000276BC File Offset: 0x000258BC
		protected virtual bool AimAnimationCallback(ItemSlot slot, bool mainHand)
		{
			this.SetState<MeleeWeaponState>(MeleeWeaponState.Aiming, mainHand);
			this.AimingSystem.AimingState = WeaponAimingState.FullCharge;
			return true;
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x000276D4 File Offset: 0x000258D4
		[ActionEventHandler(9, ActionState.Active)]
		protected virtual bool AimWhenBlocking(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (!this.Active)
			{
				return false;
			}
			if (!Enumerable.Contains<EnumEntityAction>(eventData.Modifiers, 10))
			{
				return false;
			}
			if (InteractionsTester.PlayerTriesToInteract(player, mainHand, eventData))
			{
				return false;
			}
			if (!this.CanThrow(player, mainHand) || this.Stats.ThrowAttack == null || this.AimingStats == null)
			{
				return false;
			}
			if (!this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[1]))
			{
				return false;
			}
			if (mainHand && !this.CanBlockWithOtherHand(player, mainHand))
			{
				return false;
			}
			this.StartThrowAiming(slot, player, mainHand);
			return true;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0002775C File Offset: 0x0002595C
		[ActionEventHandler(9, ActionState.Pressed)]
		protected virtual bool Throw(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (!this.Active)
			{
				return false;
			}
			if (this.Stats.ThrowAttack == null)
			{
				return false;
			}
			if (!this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[]
			{
				MeleeWeaponState.Aiming
			}))
			{
				return false;
			}
			this.SetState<MeleeWeaponState>(MeleeWeaponState.Throwing, mainHand);
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.Play(mainHand, this.Stats.ThrowAttack.ThrowAnimation, "main", this.GetAnimationSpeed(player, this.Stats, 0.5f, 2f), 1f, () => this.ThrowAnimationCallback(slot, player, mainHand), null, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.Play(mainHand, this.Stats.ThrowAttack.ThrowAnimation, "main", this.GetAnimationSpeed(player, this.Stats, 0.5f, 2f), 1f, true);
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.StopVanillaAnimation(this.Stats.ThrowAttack.TpAimAnimation, mainHand);
			}
			if (this.TpAnimationBehavior == null)
			{
				FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
				if (animationBehavior3 != null)
				{
					animationBehavior3.PlayVanillaAnimation(this.Stats.ThrowAttack.TpThrowAnimation, mainHand);
				}
			}
			this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.TriggeredShot, mainHand);
			this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndAiming, mainHand);
			return true;
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00027900 File Offset: 0x00025B00
		protected virtual bool ThrowAnimationCallback(ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			if (this.Stats.ThrowAttack == null)
			{
				return false;
			}
			this.SetState<MeleeWeaponState>(MeleeWeaponState.Idle, mainHand);
			this.AimingSystem.AimingState = WeaponAimingState.FullCharge;
			Vec3d position = player.LocalEyePos + player.Pos.XYZ;
			Vector3 targetDirection = this.AimingSystem.TargetVec;
			targetDirection = ClientAimingSystem.Zeroing(targetDirection, this.Stats.ThrowAttack.Zeroing);
			this.RangedWeaponSystem.Shoot(slot, 1, new Vector3((float)position.X, (float)position.Y, (float)position.Z), new Vector3(targetDirection.X, targetDirection.Y, targetDirection.Z), mainHand, delegate(bool _)
			{
			}, null);
			this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.SpawnedProjectile, mainHand);
			ItemStack itemstack = slot.Itemstack;
			ItemStack thrownReferenceStack = (itemstack != null) ? itemstack.Clone() : null;
			slot.TakeOut(1);
			this.TryRefillAfterPrimedThrow(player, slot, mainHand, thrownReferenceStack);
			this.ResetPrimedDualWieldThrow(mainHand);
			this.Api.World.AddCameraShake(this.Stats.ThrowScreenShakeStrength);
			return true;
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x00027A2C File Offset: 0x00025C2C
		[ActionEventHandler(10, ActionState.Released)]
		protected virtual bool Ease(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (!this.Active)
			{
				return false;
			}
			if (!this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[]
			{
				MeleeWeaponState.StartingAim,
				MeleeWeaponState.Aiming
			}))
			{
				return false;
			}
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayReadyAnimation(mainHand);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.PlayReadyAnimation(mainHand);
			}
			this.SetState<MeleeWeaponState>(MeleeWeaponState.Idle, mainHand);
			AimingAnimationController aimingAnimationController = this.AimingAnimationController;
			if (aimingAnimationController != null)
			{
				aimingAnimationController.Stop(mainHand);
			}
			this.AimingSystem.StopAiming();
			this.ResetPrimedDualWieldThrow(mainHand);
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				ThrowWeaponStats throwAttack = this.Stats.ThrowAttack;
				animationBehavior2.StopVanillaAnimation(((throwAttack != null) ? throwAttack.TpAimAnimation : null) ?? "", mainHand);
			}
			this.RangedWeaponSystem.SendStatusChange(player, RangedWeaponStatus.EndAiming, mainHand);
			return true;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00027AF8 File Offset: 0x00025CF8
		protected virtual void StartAttackCooldown(bool mainHand, TimeSpan time)
		{
			this.StopAttackCooldown(mainHand);
			double totalMilliseconds;
			float? num;
			if (mainHand)
			{
				long elapsedMilliseconds = this.Api.World.ElapsedMilliseconds;
				totalMilliseconds = time.TotalMilliseconds;
				ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
				num = ((playerActionsBehavior != null) ? new float?(playerActionsBehavior.ManipulationSpeed) : default(float?));
				this.MainHandAttackCooldownUntilMs = elapsedMilliseconds + (long)(totalMilliseconds / ((num != null) ? new double?((double)num.GetValueOrDefault()) : default(double?))).GetValueOrDefault(1.0);
				return;
			}
			long elapsedMilliseconds2 = this.Api.World.ElapsedMilliseconds;
			totalMilliseconds = time.TotalMilliseconds;
			ActionsManagerPlayerBehavior playerActionsBehavior2 = this.PlayerActionsBehavior;
			num = ((playerActionsBehavior2 != null) ? new float?(playerActionsBehavior2.ManipulationSpeed) : default(float?));
			this.OffHandAttackCooldownUntilMs = elapsedMilliseconds2 + (long)(totalMilliseconds / ((num != null) ? new double?((double)num.GetValueOrDefault()) : default(double?))).GetValueOrDefault(1.0);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00027C43 File Offset: 0x00025E43
		protected virtual void StopAttackCooldown(bool mainHand)
		{
			if (mainHand)
			{
				this.MainHandAttackCooldownUntilMs = 0L;
				return;
			}
			this.OffHandAttackCooldownUntilMs = 0L;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00027C5C File Offset: 0x00025E5C
		protected virtual bool IsAttackOnCooldown(bool mainHand)
		{
			return (mainHand ? (this.Api.World.ElapsedMilliseconds <= this.MainHandAttackCooldownUntilMs) : (this.Api.World.ElapsedMilliseconds <= this.OffHandAttackCooldownUntilMs)) || MeleeWeaponClient.CheckGlobalCooldown(this.Api);
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00027CB3 File Offset: 0x00025EB3
		protected virtual bool IsCooldownStopped(bool mainHand)
		{
			if (!mainHand)
			{
				return this.OffHandAttackCooldownUntilMs == 0L;
			}
			return this.MainHandAttackCooldownUntilMs == 0L;
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00027CD0 File Offset: 0x00025ED0
		protected virtual void StartBlockCooldown(bool mainHand, TimeSpan time)
		{
			this.StopBlockCooldown(mainHand);
			double totalMilliseconds;
			float? num;
			if (mainHand)
			{
				IWorldAccessor world = this.Api.World;
				Action<float> action = delegate(float _)
				{
					this.MainHandBlockCooldownTimer = -1L;
				};
				totalMilliseconds = time.TotalMilliseconds;
				ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
				num = ((playerActionsBehavior != null) ? new float?(playerActionsBehavior.ManipulationSpeed) : default(float?));
				this.MainHandBlockCooldownTimer = world.RegisterCallback(action, (int)(totalMilliseconds / ((num != null) ? new double?((double)num.GetValueOrDefault()) : default(double?))).GetValueOrDefault(1.0));
				return;
			}
			IWorldAccessor world2 = this.Api.World;
			Action<float> action2 = delegate(float _)
			{
				this.OffHandBlockCooldownTimer = -1L;
			};
			totalMilliseconds = time.TotalMilliseconds;
			ActionsManagerPlayerBehavior playerActionsBehavior2 = this.PlayerActionsBehavior;
			num = ((playerActionsBehavior2 != null) ? new float?(playerActionsBehavior2.ManipulationSpeed) : default(float?));
			this.OffHandBlockCooldownTimer = world2.RegisterCallback(action2, (int)(totalMilliseconds / ((num != null) ? new double?((double)num.GetValueOrDefault()) : default(double?))).GetValueOrDefault(1.0));
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00027E34 File Offset: 0x00026034
		protected virtual void StopBlockCooldown(bool mainHand)
		{
			if (mainHand)
			{
				if (this.MainHandBlockCooldownTimer != -1L)
				{
					this.Api.World.UnregisterCallback(this.MainHandBlockCooldownTimer);
					this.MainHandBlockCooldownTimer = -1L;
					return;
				}
			}
			else if (this.OffHandBlockCooldownTimer != -1L)
			{
				this.Api.World.UnregisterCallback(this.OffHandBlockCooldownTimer);
				this.OffHandBlockCooldownTimer = -1L;
			}
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00027E95 File Offset: 0x00026095
		protected virtual bool IsBlockOnCooldown(bool mainHand)
		{
			if (!mainHand)
			{
				return this.OffHandBlockCooldownTimer != -1L;
			}
			return this.MainHandBlockCooldownTimer != -1L;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00027EB8 File Offset: 0x000260B8
		protected virtual void EnsureStance(EntityPlayer player, bool mainHand)
		{
			MeleeWeaponStance currentStance = this.GetStance<MeleeWeaponStance>(mainHand);
			string dualWieldKey = this.GetDualWieldKey(player, mainHand);
			bool dualWield = dualWieldKey != "";
			MeleeWeaponStance newStance;
			if (!mainHand)
			{
				if (dualWield)
				{
					this.SetStance<MeleeWeaponStance>(MeleeWeaponStance.OffHandDualWield, mainHand);
					newStance = MeleeWeaponStance.OffHandDualWield;
				}
				else
				{
					this.SetStance<MeleeWeaponStance>(MeleeWeaponStance.OffHand, mainHand);
					newStance = MeleeWeaponStance.OffHand;
				}
				if (currentStance != newStance)
				{
					FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
					if (animationBehavior != null)
					{
						animationBehavior.PlayReadyAnimation(mainHand);
					}
					ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
					if (tpAnimationBehavior != null)
					{
						tpAnimationBehavior.PlayReadyAnimation(mainHand);
					}
				}
				StanceStats stats = this.GetStanceStats(player, mainHand);
				if (stats != null)
				{
					GripController gripController = this.GripController;
					if (gripController == null)
					{
						return;
					}
					gripController.AdjustGrip(mainHand, stats.GripMinLength, stats.GripMaxLength);
				}
				return;
			}
			if (this.CheckForOtherHandEmptyNoError(mainHand, player) && this.Stats.TwoHandedStance != null)
			{
				this.SetStance<MeleeWeaponStance>(MeleeWeaponStance.TwoHanded, mainHand);
				newStance = MeleeWeaponStance.TwoHanded;
				this.DirectionsType = MeleeWeaponClient.ParseDirectionsType(this.Stats.TwoHandedStance.AttackDirectionsType, DirectionsConfiguration.None);
			}
			else if (dualWield)
			{
				this.SetStance<MeleeWeaponStance>(MeleeWeaponStance.MainHandDualWield, mainHand);
				newStance = MeleeWeaponStance.MainHandDualWield;
				StanceStats dualWieldStats;
				if (this.Stats.MainHandDualWieldStances.TryGetValue(dualWieldKey, ref dualWieldStats) && dualWieldStats != null)
				{
					this.DirectionsType = MeleeWeaponClient.ParseDirectionsType(dualWieldStats.AttackDirectionsType, DirectionsConfiguration.None);
				}
				else if (this.Stats.OneHandedStance != null)
				{
					this.DirectionsType = MeleeWeaponClient.ParseDirectionsType(this.Stats.OneHandedStance.AttackDirectionsType, DirectionsConfiguration.None);
				}
			}
			else
			{
				this.SetStance<MeleeWeaponStance>(MeleeWeaponStance.MainHand, mainHand);
				newStance = MeleeWeaponStance.MainHand;
				if (this.Stats.OneHandedStance != null)
				{
					this.DirectionsType = MeleeWeaponClient.ParseDirectionsType(this.Stats.OneHandedStance.AttackDirectionsType, DirectionsConfiguration.None);
				}
			}
			if (currentStance != newStance)
			{
				FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
				if (animationBehavior2 != null)
				{
					animationBehavior2.PlayReadyAnimation(mainHand);
				}
				ThirdPersonAnimationsBehavior tpAnimationBehavior2 = this.TpAnimationBehavior;
				if (tpAnimationBehavior2 != null)
				{
					tpAnimationBehavior2.PlayReadyAnimation(mainHand);
				}
			}
			if (this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[]
			{
				MeleeWeaponState.Cooldown
			}) && !this.IsAttackOnCooldown(mainHand) && !this.IsCooldownStopped(mainHand))
			{
				this.StopAttackCooldown(mainHand);
				this.SetState<MeleeWeaponState>(MeleeWeaponState.Idle, mainHand);
			}
			StanceStats stanceStats = this.GetStanceStats(player, mainHand);
			if (stanceStats != null)
			{
				GripController gripController2 = this.GripController;
				if (gripController2 == null)
				{
					return;
				}
				gripController2.AdjustGrip(mainHand, stanceStats.GripMinLength, stanceStats.GripMaxLength);
			}
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x000280B8 File Offset: 0x000262B8
		[return: Nullable(2)]
		protected virtual MeleeAttack GetStanceAttack(EntityPlayer player, bool mainHand = true, AttackDirection direction = AttackDirection.Top, bool riposte = false)
		{
			if (!riposte)
			{
				riposte = this.RiposteActive(mainHand);
			}
			MeleeWeaponStance stance = this.GetStance<MeleeWeaponStance>(mainHand);
			string dualWieldKey = this.GetDualWieldKey(player, mainHand);
			bool hasDualWieldKey = !string.IsNullOrEmpty(dualWieldKey);
			MeleeAttack result;
			switch (stance)
			{
			case MeleeWeaponStance.MainHand:
			{
				MeleeAttack meleeAttack;
				if (!riposte)
				{
					if ((meleeAttack = this.OneHandedAttack) == null)
					{
						Dictionary<AttackDirection, MeleeAttack> directionalOneHandedAttacks = this.DirectionalOneHandedAttacks;
						meleeAttack = ((directionalOneHandedAttacks != null) ? CollectionExtensions.GetValueOrDefault<AttackDirection, MeleeAttack>(directionalOneHandedAttacks, direction) : null);
					}
				}
				else
				{
					meleeAttack = this.OneHandedRiposte;
				}
				result = meleeAttack;
				break;
			}
			case MeleeWeaponStance.OffHand:
			{
				MeleeAttack meleeAttack2;
				if (!riposte)
				{
					if ((meleeAttack2 = this.OffHandAttack) == null)
					{
						Dictionary<AttackDirection, MeleeAttack> directionalOffHandAttacks = this.DirectionalOffHandAttacks;
						meleeAttack2 = ((directionalOffHandAttacks != null) ? CollectionExtensions.GetValueOrDefault<AttackDirection, MeleeAttack>(directionalOffHandAttacks, direction) : null);
					}
				}
				else
				{
					meleeAttack2 = this.OffHandRiposte;
				}
				result = meleeAttack2;
				break;
			}
			case MeleeWeaponStance.TwoHanded:
			{
				MeleeAttack meleeAttack3;
				if (!riposte)
				{
					if ((meleeAttack3 = this.TwoHandedAttack) == null)
					{
						Dictionary<AttackDirection, MeleeAttack> directionalTwoHandedAttacks = this.DirectionalTwoHandedAttacks;
						meleeAttack3 = ((directionalTwoHandedAttacks != null) ? CollectionExtensions.GetValueOrDefault<AttackDirection, MeleeAttack>(directionalTwoHandedAttacks, direction) : null);
					}
				}
				else
				{
					meleeAttack3 = this.TwoHandedRiposte;
				}
				result = meleeAttack3;
				break;
			}
			case MeleeWeaponStance.MainHandDualWield:
			{
				MeleeAttack meleeAttack4;
				if (hasDualWieldKey)
				{
					meleeAttack4 = (riposte ? this.OneHandedRiposte : this.GetMainHandDualWieldAttack(dualWieldKey, direction));
				}
				else if (!riposte)
				{
					if ((meleeAttack4 = this.OneHandedAttack) == null)
					{
						Dictionary<AttackDirection, MeleeAttack> directionalOneHandedAttacks2 = this.DirectionalOneHandedAttacks;
						meleeAttack4 = ((directionalOneHandedAttacks2 != null) ? CollectionExtensions.GetValueOrDefault<AttackDirection, MeleeAttack>(directionalOneHandedAttacks2, direction) : null);
					}
				}
				else
				{
					meleeAttack4 = this.OneHandedRiposte;
				}
				result = meleeAttack4;
				break;
			}
			case MeleeWeaponStance.OffHandDualWield:
			{
				MeleeAttack meleeAttack5;
				if (hasDualWieldKey)
				{
					meleeAttack5 = (riposte ? this.OffHandRiposte : this.GetOffHandDualWieldAttack(dualWieldKey, direction));
				}
				else if (!riposte)
				{
					if ((meleeAttack5 = this.OffHandAttack) == null)
					{
						Dictionary<AttackDirection, MeleeAttack> directionalOffHandAttacks2 = this.DirectionalOffHandAttacks;
						meleeAttack5 = ((directionalOffHandAttacks2 != null) ? CollectionExtensions.GetValueOrDefault<AttackDirection, MeleeAttack>(directionalOffHandAttacks2, direction) : null);
					}
				}
				else
				{
					meleeAttack5 = this.OffHandRiposte;
				}
				result = meleeAttack5;
				break;
			}
			default:
				result = this.OneHandedAttack;
				break;
			}
			return result;
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0002822C File Offset: 0x0002642C
		[return: Nullable(2)]
		protected virtual MeleeAttack GetStanceBlockBash(EntityPlayer player, bool mainHand = true, AttackDirection direction = AttackDirection.Top)
		{
			MeleeAttack result;
			switch (this.GetStance<MeleeWeaponStance>(mainHand))
			{
			case MeleeWeaponStance.MainHand:
			{
				MeleeAttack meleeAttack;
				if ((meleeAttack = this.OneHandedBlockBash) == null)
				{
					Dictionary<AttackDirection, MeleeAttack> directionalOneHandedBlockBashes = this.DirectionalOneHandedBlockBashes;
					meleeAttack = ((directionalOneHandedBlockBashes != null) ? CollectionExtensions.GetValueOrDefault<AttackDirection, MeleeAttack>(directionalOneHandedBlockBashes, direction) : null);
				}
				result = meleeAttack;
				break;
			}
			case MeleeWeaponStance.OffHand:
			{
				MeleeAttack meleeAttack2;
				if ((meleeAttack2 = this.OffHandBlockBash) == null)
				{
					Dictionary<AttackDirection, MeleeAttack> directionalOffHandBlockBashes = this.DirectionalOffHandBlockBashes;
					meleeAttack2 = ((directionalOffHandBlockBashes != null) ? CollectionExtensions.GetValueOrDefault<AttackDirection, MeleeAttack>(directionalOffHandBlockBashes, direction) : null);
				}
				result = meleeAttack2;
				break;
			}
			case MeleeWeaponStance.TwoHanded:
			{
				MeleeAttack meleeAttack3;
				if ((meleeAttack3 = this.TwoHandedBlockBash) == null)
				{
					Dictionary<AttackDirection, MeleeAttack> directionalTwoHandedBlockBashes = this.DirectionalTwoHandedBlockBashes;
					meleeAttack3 = ((directionalTwoHandedBlockBashes != null) ? CollectionExtensions.GetValueOrDefault<AttackDirection, MeleeAttack>(directionalTwoHandedBlockBashes, direction) : null);
				}
				result = meleeAttack3;
				break;
			}
			default:
				result = this.OneHandedBlockBash;
				break;
			}
			return result;
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x000282C0 File Offset: 0x000264C0
		[return: Nullable(2)]
		protected virtual MeleeAttack GetStanceHandleAttack(EntityPlayer player, bool mainHand = true)
		{
			MeleeWeaponStance stance = this.GetStance<MeleeWeaponStance>(mainHand);
			string dualWieldKey = this.GetDualWieldKey(player, mainHand);
			bool hasDualWieldKey = !string.IsNullOrEmpty(dualWieldKey);
			MeleeAttack result;
			switch (stance)
			{
			case MeleeWeaponStance.MainHand:
				result = this.OneHandedHandleAttack;
				break;
			case MeleeWeaponStance.OffHand:
				result = this.OffHandHandleAttack;
				break;
			case MeleeWeaponStance.TwoHanded:
				result = this.TwoHandedHandleAttack;
				break;
			case MeleeWeaponStance.MainHandDualWield:
			{
				MeleeAttack mainHandle;
				result = ((!hasDualWieldKey) ? this.OneHandedHandleAttack : (this.MainHandDualWieldHandleAttacks.TryGetValue(dualWieldKey, ref mainHandle) ? mainHandle : this.OneHandedHandleAttack));
				break;
			}
			case MeleeWeaponStance.OffHandDualWield:
			{
				MeleeAttack offHandle;
				result = ((!hasDualWieldKey) ? this.OffHandHandleAttack : (this.OffHandDualWieldHandleAttacks.TryGetValue(dualWieldKey, ref offHandle) ? offHandle : this.OffHandHandleAttack));
				break;
			}
			default:
				result = this.OneHandedHandleAttack;
				break;
			}
			return result;
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00028380 File Offset: 0x00026580
		[return: Nullable(2)]
		protected virtual StanceStats GetStanceStats(EntityPlayer player, bool mainHand = true)
		{
			MeleeWeaponStance stance = this.GetStance<MeleeWeaponStance>(mainHand);
			string dualWieldKey = this.GetDualWieldKey(player, mainHand);
			bool hasDualWieldKey = !string.IsNullOrEmpty(dualWieldKey);
			StanceStats result;
			switch (stance)
			{
			case MeleeWeaponStance.MainHand:
				result = this.Stats.OneHandedStance;
				break;
			case MeleeWeaponStance.OffHand:
				result = this.Stats.OffHandStance;
				break;
			case MeleeWeaponStance.TwoHanded:
				result = this.Stats.TwoHandedStance;
				break;
			case MeleeWeaponStance.MainHandDualWield:
			{
				StanceStats mainDualStats;
				result = ((!hasDualWieldKey) ? this.Stats.OneHandedStance : (this.Stats.MainHandDualWieldStances.TryGetValue(dualWieldKey, ref mainDualStats) ? mainDualStats : this.Stats.OneHandedStance));
				break;
			}
			case MeleeWeaponStance.OffHandDualWield:
			{
				StanceStats offDualStats;
				result = ((!hasDualWieldKey) ? this.Stats.OffHandStance : (this.Stats.OffHandDualWieldStances.TryGetValue(dualWieldKey, ref offDualStats) ? offDualStats : this.Stats.OffHandStance));
				break;
			}
			default:
				result = this.Stats.OneHandedStance;
				break;
			}
			return result;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x00028478 File Offset: 0x00026678
		[return: Nullable(2)]
		protected virtual MeleeAttack GetMainHandDualWieldAttack(string dualWieldKey, AttackDirection direction)
		{
			Dictionary<AttackDirection, MeleeAttack> directional;
			if (this.DirectionalMainHandDualWieldAttacks.TryGetValue(dualWieldKey, ref directional) && directional != null)
			{
				MeleeAttack attack;
				if (directional.TryGetValue(direction, ref attack))
				{
					return attack;
				}
				MeleeAttack anyAttack = Enumerable.FirstOrDefault<MeleeAttack>(directional.Values);
				if (anyAttack != null)
				{
					return anyAttack;
				}
			}
			MeleeAttack handleAttack;
			if (this.MainHandDualWieldHandleAttacks.TryGetValue(dualWieldKey, ref handleAttack))
			{
				return handleAttack;
			}
			return this.OneHandedAttack;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x000284D0 File Offset: 0x000266D0
		[return: Nullable(2)]
		protected virtual MeleeAttack GetOffHandDualWieldAttack(string dualWieldKey, AttackDirection direction)
		{
			Dictionary<AttackDirection, MeleeAttack> directional;
			if (this.DirectionalOffHandDualWieldAttacks.TryGetValue(dualWieldKey, ref directional) && directional != null)
			{
				MeleeAttack attack;
				if (directional.TryGetValue(direction, ref attack))
				{
					return attack;
				}
				MeleeAttack anyAttack = Enumerable.FirstOrDefault<MeleeAttack>(directional.Values);
				if (anyAttack != null)
				{
					return anyAttack;
				}
			}
			MeleeAttack handleAttack;
			if (this.OffHandDualWieldHandleAttacks.TryGetValue(dualWieldKey, ref handleAttack))
			{
				return handleAttack;
			}
			return this.OffHandAttack ?? this.OneHandedAttack;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00028530 File Offset: 0x00026730
		protected virtual string GetDualWieldKey(EntityPlayer player, bool mainHand)
		{
			MeleeWeaponClient.<>c__DisplayClass179_0 CS$<>8__locals1 = new MeleeWeaponClient.<>c__DisplayClass179_0();
			ItemSlot otherHandSlot = mainHand ? player.LeftHandItemSlot : player.RightHandItemSlot;
			MeleeWeaponClient.<>c__DisplayClass179_0 CS$<>8__locals2 = CS$<>8__locals1;
			ItemStack itemstack = otherHandSlot.Itemstack;
			string text;
			if (itemstack == null)
			{
				text = null;
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				if (collectible == null)
				{
					text = null;
				}
				else
				{
					AssetLocation code = collectible.Code;
					text = ((code != null) ? code.ToString() : null);
				}
			}
			CS$<>8__locals2.otherItemCode = (text ?? string.Empty);
			if (CS$<>8__locals1.otherItemCode == "")
			{
				return "";
			}
			return Enumerable.FirstOrDefault<string>(Enumerable.Where<string>(mainHand ? this.Stats.MainHandDualWieldStances.Keys : this.Stats.OffHandDualWieldStances.Keys, (string wildcard) => WildcardUtil.Match(wildcard, CS$<>8__locals1.otherItemCode)), "");
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x000285E6 File Offset: 0x000267E6
		[NullableContext(0)]
		protected virtual bool CheckState<TState>(bool mainHand, [Nullable(new byte[]
		{
			1,
			0
		})] params TState[] statesToCheck) where TState : struct, Enum
		{
			Type typeFromHandle = typeof(TState);
			ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
			return ArrayExtensions.Contains<TState>(statesToCheck, (TState)((object)Enum.ToObject(typeFromHandle, (playerActionsBehavior != null) ? (playerActionsBehavior.GetState(mainHand) % 100) : 0)));
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00028618 File Offset: 0x00026818
		[NullableContext(0)]
		protected virtual void SetStance<TStance>(TStance stance, bool mainHand = true) where TStance : struct, Enum
		{
			ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
			int stateCombined = (playerActionsBehavior != null) ? playerActionsBehavior.GetState(mainHand) : 0;
			int num = stateCombined % 100;
			int stanceInt = (int)Enum.ToObject(typeof(TStance), stance);
			stateCombined = num + 100 * stanceInt;
			ActionsManagerPlayerBehavior playerActionsBehavior2 = this.PlayerActionsBehavior;
			if (playerActionsBehavior2 == null)
			{
				return;
			}
			playerActionsBehavior2.SetState(stateCombined, mainHand);
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00028670 File Offset: 0x00026870
		[NullableContext(0)]
		protected virtual void SetState<TState>(TState state, bool mainHand = true) where TState : struct, Enum
		{
			ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
			int stateCombined = (playerActionsBehavior != null) ? playerActionsBehavior.GetState(mainHand) : 0;
			int stanceInt = stateCombined / 100;
			stateCombined = (int)Enum.ToObject(typeof(TState), state) + 100 * stanceInt;
			ActionsManagerPlayerBehavior playerActionsBehavior2 = this.PlayerActionsBehavior;
			if (playerActionsBehavior2 == null)
			{
				return;
			}
			playerActionsBehavior2.SetState(stateCombined, mainHand);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x000286C8 File Offset: 0x000268C8
		[NullableContext(0)]
		protected virtual void SetStateAndStance<TState, TStance>(TState state, TStance stance, bool mainHand = true) where TState : struct, Enum where TStance : struct, Enum
		{
			int num = (int)Enum.ToObject(typeof(TState), state);
			int stanceInt = (int)Enum.ToObject(typeof(TStance), stance);
			int stateCombined = num + 100 * stanceInt;
			ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
			if (playerActionsBehavior == null)
			{
				return;
			}
			playerActionsBehavior.SetState(stateCombined, mainHand);
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00028722 File Offset: 0x00026922
		[NullableContext(0)]
		protected virtual TState GetState<TState>(bool mainHand = true) where TState : struct, Enum
		{
			Type typeFromHandle = typeof(TState);
			ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
			return (TState)((object)Enum.ToObject(typeFromHandle, (playerActionsBehavior != null) ? (playerActionsBehavior.GetState(mainHand) % 100) : 0));
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x0002874E File Offset: 0x0002694E
		[NullableContext(0)]
		protected virtual TStance GetStance<TStance>(bool mainHand = true) where TStance : struct, Enum
		{
			Type typeFromHandle = typeof(TStance);
			ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
			return (TStance)((object)Enum.ToObject(typeFromHandle, (playerActionsBehavior != null) ? (playerActionsBehavior.GetState(mainHand) / 100) : 0));
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0002877A File Offset: 0x0002697A
		protected virtual bool CanAttackWithOtherHand(EntityPlayer player, bool mainHand = true)
		{
			ItemStack itemstack = (mainHand ? player.LeftHandItemSlot : player.RightHandItemSlot).Itemstack;
			object obj;
			if (itemstack == null)
			{
				obj = null;
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				obj = ((collectible != null) ? collectible.GetCollectibleInterface<IHasMeleeWeaponActions>() : null);
			}
			object obj2 = obj;
			return obj2 != null && obj2.CanAttack(player, !mainHand);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x000287BA File Offset: 0x000269BA
		protected virtual bool CanBlockWithOtherHand(EntityPlayer player, bool mainHand = true)
		{
			ItemStack itemstack = (mainHand ? player.LeftHandItemSlot : player.RightHandItemSlot).Itemstack;
			object obj;
			if (itemstack == null)
			{
				obj = null;
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				obj = ((collectible != null) ? collectible.GetCollectibleInterface<IHasMeleeWeaponActions>() : null);
			}
			object obj2 = obj;
			return obj2 != null && obj2.CanBlock(player, !mainHand);
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x000287FA File Offset: 0x000269FA
		protected virtual bool CanThrowWithOtherHand(EntityPlayer player, bool mainHand = true)
		{
			ItemStack itemstack = (mainHand ? player.LeftHandItemSlot : player.RightHandItemSlot).Itemstack;
			object obj;
			if (itemstack == null)
			{
				obj = null;
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				obj = ((collectible != null) ? collectible.GetCollectibleInterface<IHasMeleeWeaponActions>() : null);
			}
			object obj2 = obj;
			return obj2 != null && obj2.CanThrow(player, !mainHand);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x0002883C File Offset: 0x00026A3C
		protected virtual bool CheckForOtherHandEmpty(bool mainHand, EntityPlayer player)
		{
			if (mainHand && !player.LeftHandItemSlot.Empty)
			{
				ICoreClientAPI coreClientAPI = player.World.Api as ICoreClientAPI;
				if (coreClientAPI != null)
				{
					coreClientAPI.TriggerIngameError(this, "offhandShouldBeEmpty", Lang.Get("Offhand should be empty", Array.Empty<object>()));
				}
				return false;
			}
			if (!mainHand && !player.RightHandItemSlot.Empty)
			{
				ICoreClientAPI coreClientAPI2 = player.World.Api as ICoreClientAPI;
				if (coreClientAPI2 != null)
				{
					coreClientAPI2.TriggerIngameError(this, "mainHandShouldBeEmpty", Lang.Get("Main hand should be empty", Array.Empty<object>()));
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x000288CE File Offset: 0x00026ACE
		protected virtual bool CheckForOtherHandEmptyNoError(bool mainHand, EntityPlayer player)
		{
			return (!mainHand || player.LeftHandItemSlot.Empty) && (mainHand || player.RightHandItemSlot.Empty);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x000288F8 File Offset: 0x00026AF8
		protected virtual void SetSpeedPenalty(bool mainHand, EntityPlayer player)
		{
			float penalty;
			if (this.HasSpeedPenalty(mainHand, out penalty, player))
			{
				ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
				if (playerActionsBehavior == null)
				{
					return;
				}
				playerActionsBehavior.SetStat("walkspeed", mainHand ? "CombatOverhaul:held-item-mainhand" : "CombatOverhaul:held-item-offhand", penalty);
				return;
			}
			else
			{
				ActionsManagerPlayerBehavior playerActionsBehavior2 = this.PlayerActionsBehavior;
				if (playerActionsBehavior2 == null)
				{
					return;
				}
				playerActionsBehavior2.SetStat("walkspeed", mainHand ? "CombatOverhaul:held-item-mainhand" : "CombatOverhaul:held-item-offhand", 0f);
				return;
			}
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00028960 File Offset: 0x00026B60
		protected virtual bool HasSpeedPenalty(bool mainHand, out float penalty, EntityPlayer player)
		{
			penalty = 0f;
			StanceStats stance = this.GetStanceStats(player, mainHand);
			if (stance == null)
			{
				return false;
			}
			if (this.CheckState<MeleeWeaponState>(mainHand, new MeleeWeaponState[]
			{
				MeleeWeaponState.Blocking,
				MeleeWeaponState.Parrying
			}))
			{
				penalty = stance.BlockSpeedPenalty;
			}
			else
			{
				penalty = stance.SpeedPenalty;
			}
			return MathF.Abs(penalty) > 1E-09f;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x000289BC File Offset: 0x00026BBC
		protected virtual float GetAnimationSpeed(Entity player, string proficiencyStat, float min = 0.5f, float max = 2f)
		{
			ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
			float num = (playerActionsBehavior != null) ? playerActionsBehavior.ManipulationSpeed : 1f;
			float proficiencyBonus = (proficiencyStat == "") ? 0f : (player.Stats.GetBlended(proficiencyStat) - 1f);
			return Math.Clamp(num + proficiencyBonus, min, max);
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00028A10 File Offset: 0x00026C10
		protected virtual float GetAnimationSpeed(Entity player, WeaponStats stats, float min = 0.5f, float max = 2f)
		{
			ActionsManagerPlayerBehavior playerActionsBehavior = this.PlayerActionsBehavior;
			float manipulationSpeed = (playerActionsBehavior != null) ? playerActionsBehavior.ManipulationSpeed : 1f;
			float proficiencyBonus = (stats.ProficiencyStat == "") ? 0f : (player.Stats.GetBlended(stats.ProficiencyStat) - 1f);
			foreach (string stat in stats.ProficiencyStats)
			{
				proficiencyBonus += player.Stats.GetBlended(stat) - 1f;
			}
			return Math.Clamp(manipulationSpeed + proficiencyBonus, min, max);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00028AA0 File Offset: 0x00026CA0
		protected virtual bool ActionRestricted(EntityPlayer player, bool mainHand = true)
		{
			ItemStack itemstack = ((!mainHand) ? player.RightHandItemSlot : player.LeftHandItemSlot).Itemstack;
			IRestrictAction restrictAction;
			if (itemstack == null)
			{
				restrictAction = null;
			}
			else
			{
				CollectibleObject collectible = itemstack.Collectible;
				restrictAction = ((collectible != null) ? collectible.GetCollectibleInterface<IRestrictAction>() : null);
			}
			IRestrictAction item = restrictAction;
			if (item == null)
			{
				return false;
			}
			if (mainHand)
			{
				return item.RestrictLeftHandAction();
			}
			return item.RestrictRightHandAction();
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00028AF4 File Offset: 0x00026CF4
		protected virtual string GetAttackStatsDescription(ItemSlot inSlot, IEnumerable<DamageDataJson> damageTypesData, string descriptionLangCode)
		{
			float maxDamage = 0f;
			float minDamage = float.MaxValue;
			float maxTier = 0f;
			float minTier = float.MaxValue;
			int armorPiercingTier = 0;
			HashSet<string> damageTypes = new HashSet<string>();
			ItemStackMeleeWeaponStats stackStats = ItemStackMeleeWeaponStats.FromItemStack(inSlot.Itemstack);
			foreach (DamageDataJson attack in damageTypesData)
			{
				float attackDamage = attack.Damage * stackStats.DamageMultiplier;
				if (attackDamage > maxDamage)
				{
					maxDamage = attackDamage;
				}
				if (attackDamage < minDamage)
				{
					minDamage = attackDamage;
				}
				float currentTier = (float)(attack.Tier + stackStats.DamageTierBonus);
				if (currentTier > maxTier)
				{
					maxTier = currentTier;
				}
				if (currentTier < minTier)
				{
					minTier = currentTier;
				}
				int currentArmorPiercingTier = attack.ArmorPiercingTier;
				if (currentArmorPiercingTier > armorPiercingTier)
				{
					armorPiercingTier = currentArmorPiercingTier;
				}
				damageTypes.Add(attack.DamageType);
			}
			string damageType = Enumerable.Aggregate<string>(Enumerable.Select<string, string>(damageTypes, (string element) => Lang.Get("combatoverhaul:damage-type-" + element, Array.Empty<object>())), (string first, string second) => first + ", " + second);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<float>(maxDamage, "F1");
			string damageString = defaultInterpolatedStringHandler.ToStringAndClear();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
			defaultInterpolatedStringHandler2..ctor(0, 1);
			defaultInterpolatedStringHandler2.AppendFormatted<float>(maxTier, "F0");
			string tierString = defaultInterpolatedStringHandler2.ToStringAndClear();
			return Lang.Get(descriptionLangCode, new object[]
			{
				damageString,
				tierString,
				damageType
			}) + ((armorPiercingTier > 0) ? ("\n" + Lang.Get("combatoverhaul:iteminfo-melee-weapon-armorpiercing", new object[]
			{
				armorPiercingTier
			})) : "");
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00028CAC File Offset: 0x00026EAC
		protected virtual IEnumerable<string> GetDirectionalAttackStatsDescriptions(ItemSlot inSlot, Dictionary<string, MeleeAttackStats> directionalAttacks, string descriptionLangCode)
		{
			HashSet<string> descriptions = new HashSet<string>(StringComparer.Ordinal);
			bool isDagger = this.IsDaggerItemStack(inSlot.Itemstack);
			foreach (KeyValuePair<string, MeleeAttackStats> keyValuePair in directionalAttacks)
			{
				string text;
				MeleeAttackStats meleeAttackStats;
				keyValuePair.Deconstruct(ref text, ref meleeAttackStats);
				string directionKey = text;
				MeleeAttackStats attackStats = meleeAttackStats;
				if (attackStats.DamageTypes != null && attackStats.DamageTypes.Length != 0)
				{
					IEnumerable<DamageDataJson> damageTypesData = Enumerable.Select<MeleeDamageTypeJson, DamageDataJson>(attackStats.DamageTypes, (MeleeDamageTypeJson element) => element.Damage);
					AttackDirection direction;
					if (isDagger && Enum.TryParse<AttackDirection>(directionKey, ref direction) && this.IsTopOrSideDirection(direction))
					{
						damageTypesData = Enumerable.Select<DamageDataJson, DamageDataJson>(damageTypesData, delegate(DamageDataJson data)
						{
							if (!data.DamageType.Equals("Piercing", 5))
							{
								return data;
							}
							return new DamageDataJson
							{
								Damage = data.Damage,
								DamageType = "Slashing",
								Tier = data.Tier,
								ArmorPiercingTier = data.ArmorPiercingTier
							};
						});
					}
					string description = this.GetAttackStatsDescription(inSlot, damageTypesData, descriptionLangCode);
					descriptions.Add(description);
				}
			}
			return descriptions;
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00028DC0 File Offset: 0x00026FC0
		[NullableContext(2)]
		protected virtual bool IsDaggerItemStack(ItemStack stack)
		{
			return this.IsDaggerStack(stack);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00028DC9 File Offset: 0x00026FC9
		protected virtual bool IsTopOrSideDirection(AttackDirection direction)
		{
			return direction == AttackDirection.Top || direction == AttackDirection.Left || direction == AttackDirection.Right || direction == AttackDirection.TopLeft || direction == AttackDirection.TopRight;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00028DE0 File Offset: 0x00026FE0
		protected virtual bool ItemInOtherHandBlocksAttack(EntityPlayer player, bool mainHand)
		{
			return !mainHand && this.CanAttackWithOtherHand(player, mainHand) && this.GetStance<MeleeWeaponStance>(mainHand) != MeleeWeaponStance.OffHandDualWield;
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00028DFE File Offset: 0x00026FFE
		[NullableContext(0)]
		protected static bool CheckState<TState>(int state, [Nullable(new byte[]
		{
			1,
			0
		})] params TState[] statesToCheck) where TState : struct, Enum
		{
			return ArrayExtensions.Contains<TState>(statesToCheck, (TState)((object)Enum.ToObject(typeof(TState), state % 100)));
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00028E1E File Offset: 0x0002701E
		protected virtual bool ShouldUseVanillaShieldRaiseAnimation(ItemSlot slot, bool mainHand)
		{
			if (mainHand)
			{
				return false;
			}
			ItemStack itemstack = slot.Itemstack;
			return CollectibleClassifier.IsVanillaItemShield((itemstack != null) ? itemstack.Item : null);
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00028E3C File Offset: 0x0002703C
		protected virtual void PlayVanillaShieldRaiseAnimation(EntityPlayer player, bool mainHand)
		{
			string side = mainHand ? "right" : "left";
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.PlayVanillaAnimation("raiseshield-" + side, mainHand);
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.PlayVanillaAnimation("raiseshield-" + side + "-fp", mainHand);
			}
			player.StartAnimation("raiseshield-" + side);
			player.StartAnimation("raiseshield-" + side + "-fp");
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00028EC0 File Offset: 0x000270C0
		protected virtual void StopVanillaShieldRaiseAnimation(EntityPlayer player, bool mainHand)
		{
			string side = mainHand ? "right" : "left";
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.StopVanillaAnimation("raiseshield-" + side, mainHand);
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.StopVanillaAnimation("raiseshield-" + side + "-fp", mainHand);
			}
			player.StopAnimation("raiseshield-" + side);
			player.StopAnimation("raiseshield-" + side + "-fp");
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00028F43 File Offset: 0x00027143
		protected static string AnimationCategory(bool mainHand = true)
		{
			if (!mainHand)
			{
				return "mainOffhand";
			}
			return "main";
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00028F54 File Offset: 0x00027154
		[NullableContext(2)]
		protected static DirectionsConfiguration ParseDirectionsType(string value, DirectionsConfiguration fallback)
		{
			DirectionsConfiguration parsed;
			if (!string.IsNullOrWhiteSpace(value) && Enum.TryParse<DirectionsConfiguration>(value, true, ref parsed))
			{
				return parsed;
			}
			return fallback;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00028F77 File Offset: 0x00027177
		protected static void RegisterCollider(string item, string type, MeleeAttack attack)
		{
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00028F79 File Offset: 0x00027179
		protected static bool CheckGlobalCooldown(ICoreAPI api)
		{
			return MeleeWeaponClient.GlobalCooldownUntilMs > api.World.ElapsedMilliseconds;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00028F90 File Offset: 0x00027190
		[NullableContext(2)]
		[CompilerGenerated]
		private void <FindInventoryReplacementForThrown>g__ScanInventory|158_0(IInventory inv, ref MeleeWeaponClient.<>c__DisplayClass158_0 A_2)
		{
			if (A_2.replacementSlot != null)
			{
				return;
			}
			if (inv == null)
			{
				return;
			}
			foreach (ItemSlot candidate in inv)
			{
				if (A_2.scannedSlots.Add(candidate) && candidate != null && candidate != A_2.thrownSlot && candidate != A_2.player.LeftHandItemSlot)
				{
					ItemStack itemstack = candidate.Itemstack;
					AssetLocation assetLocation;
					if (itemstack == null)
					{
						assetLocation = null;
					}
					else
					{
						CollectibleObject collectible = itemstack.Collectible;
						assetLocation = ((collectible != null) ? collectible.Code : null);
					}
					if (!(assetLocation == null) && this.IsDaggerStack(candidate.Itemstack))
					{
						string candidateCode = candidate.Itemstack.Collectible.Code.ToString();
						if (A_2.anyDaggerReplacementAllowed || candidateCode == A_2.referenceCode || WildcardUtil.Match(A_2.referenceCode + "-*", candidateCode) || WildcardUtil.Match(A_2.referenceCode, candidateCode))
						{
							A_2.replacementSlot = candidate;
							break;
						}
					}
				}
			}
		}

		// Token: 0x0400029A RID: 666
		protected readonly Item Item;

		// Token: 0x0400029B RID: 667
		protected readonly ICoreClientAPI Api;

		// Token: 0x0400029C RID: 668
		protected readonly MeleeBlockSystemClient MeleeBlockSystem;

		// Token: 0x0400029D RID: 669
		protected readonly MeleeSystemClient MeleeAttackSystem;

		// Token: 0x0400029E RID: 670
		[Nullable(2)]
		protected readonly ImpaleSystemClient ImpaleSystem;

		// Token: 0x0400029F RID: 671
		protected readonly RangedWeaponSystemClient RangedWeaponSystem;

		// Token: 0x040002A0 RID: 672
		protected readonly ClientAimingSystem AimingSystem;

		// Token: 0x040002A1 RID: 673
		protected readonly Settings Settings;

		// Token: 0x040002A2 RID: 674
		protected readonly float DefaultVerticalLimit;

		// Token: 0x040002A3 RID: 675
		protected readonly float DefaultHorizontalLimit;

		// Token: 0x040002A4 RID: 676
		[Nullable(2)]
		protected FirstPersonAnimationsBehavior AnimationBehavior;

		// Token: 0x040002A5 RID: 677
		[Nullable(2)]
		protected ThirdPersonAnimationsBehavior TpAnimationBehavior;

		// Token: 0x040002A6 RID: 678
		[Nullable(2)]
		protected ActionsManagerPlayerBehavior PlayerActionsBehavior;

		// Token: 0x040002A7 RID: 679
		protected SoundsSynchronizerClient SoundsSystem;

		// Token: 0x040002A8 RID: 680
		[Nullable(2)]
		protected AimingAnimationController AimingAnimationController;

		// Token: 0x040002A9 RID: 681
		[Nullable(2)]
		protected GripController GripController;

		// Token: 0x040002AA RID: 682
		protected const int MaxState = 100;

		// Token: 0x040002AB RID: 683
		protected readonly MeleeWeaponStats Stats;

		// Token: 0x040002AC RID: 684
		protected const string PlayerStatsMainHandCategory = "CombatOverhaul:held-item-mainhand";

		// Token: 0x040002AD RID: 685
		protected const string PlayerStatsOffHandCategory = "CombatOverhaul:held-item-offhand";

		// Token: 0x040002AE RID: 686
		protected bool ParryButtonReleased = true;

		// Token: 0x040002AF RID: 687
		protected long MainHandBlockCooldownTimer = -1L;

		// Token: 0x040002B0 RID: 688
		protected long OffHandBlockCooldownTimer = -1L;

		// Token: 0x040002B1 RID: 689
		protected long MainHandAttackCooldownUntilMs;

		// Token: 0x040002B2 RID: 690
		protected long OffHandAttackCooldownUntilMs;

		// Token: 0x040002B3 RID: 691
		protected int MainHandAttackCounter;

		// Token: 0x040002B4 RID: 692
		protected int OffHandAttackCounter;

		// Token: 0x040002B5 RID: 693
		protected bool HandleHitTerrain;

		// Token: 0x040002B6 RID: 694
		protected const bool EditColliders = false;

		// Token: 0x040002B7 RID: 695
		protected AttackDirection CurrentMainHandDirection;

		// Token: 0x040002B8 RID: 696
		protected AttackDirection CurrentOffHandDirection;

		// Token: 0x040002B9 RID: 697
		protected static bool CanRiposteMainHand;

		// Token: 0x040002BA RID: 698
		protected static bool CanRiposteOffHand;

		// Token: 0x040002BB RID: 699
		protected static long RiposteTimerMainHand;

		// Token: 0x040002BC RID: 700
		protected static long RiposteTimerOffHand;

		// Token: 0x040002BD RID: 701
		protected bool RiposteMainHand;

		// Token: 0x040002BE RID: 702
		protected bool RiposteOffHand;

		// Token: 0x040002BF RID: 703
		protected bool CurrentMainHandAttackIsRiposte;

		// Token: 0x040002C0 RID: 704
		protected bool CurrentOffHandAttackIsRiposte;

		// Token: 0x040002C1 RID: 705
		protected bool MainHandPrimedDualWieldThrow;

		// Token: 0x040002C2 RID: 706
		protected const long GlobalCooldownMs = 1000L;

		// Token: 0x040002C3 RID: 707
		protected const int RiposteGracePeriodMs = 300;

		// Token: 0x040002C4 RID: 708
		[Nullable(2)]
		protected readonly AimingStats AimingStats;

		// Token: 0x040002C5 RID: 709
		[Nullable(2)]
		protected MeleeAttack OneHandedAttack;

		// Token: 0x040002C6 RID: 710
		[Nullable(2)]
		protected MeleeAttack TwoHandedAttack;

		// Token: 0x040002C7 RID: 711
		[Nullable(2)]
		protected MeleeAttack OffHandAttack;

		// Token: 0x040002C8 RID: 712
		protected Dictionary<string, MeleeAttack> MainHandDualWieldAttacks = new Dictionary<string, MeleeAttack>();

		// Token: 0x040002C9 RID: 713
		protected Dictionary<string, MeleeAttack> OffHandDualWieldAttacks = new Dictionary<string, MeleeAttack>();

		// Token: 0x040002CA RID: 714
		[Nullable(new byte[]
		{
			2,
			1
		})]
		protected Dictionary<AttackDirection, MeleeAttack> DirectionalOneHandedAttacks;

		// Token: 0x040002CB RID: 715
		[Nullable(new byte[]
		{
			2,
			1
		})]
		protected Dictionary<AttackDirection, MeleeAttack> DirectionalTwoHandedAttacks;

		// Token: 0x040002CC RID: 716
		[Nullable(new byte[]
		{
			2,
			1
		})]
		protected Dictionary<AttackDirection, MeleeAttack> DirectionalOffHandAttacks;

		// Token: 0x040002CD RID: 717
		protected Dictionary<string, Dictionary<AttackDirection, MeleeAttack>> DirectionalMainHandDualWieldAttacks = new Dictionary<string, Dictionary<AttackDirection, MeleeAttack>>();

		// Token: 0x040002CE RID: 718
		protected Dictionary<string, Dictionary<AttackDirection, MeleeAttack>> DirectionalOffHandDualWieldAttacks = new Dictionary<string, Dictionary<AttackDirection, MeleeAttack>>();

		// Token: 0x040002CF RID: 719
		[Nullable(2)]
		protected MeleeAttack OneHandedRiposte;

		// Token: 0x040002D0 RID: 720
		[Nullable(2)]
		protected MeleeAttack TwoHandedRiposte;

		// Token: 0x040002D1 RID: 721
		[Nullable(2)]
		protected MeleeAttack OffHandRiposte;

		// Token: 0x040002D2 RID: 722
		[Nullable(2)]
		protected MeleeAttack OneHandedBlockBash;

		// Token: 0x040002D3 RID: 723
		[Nullable(2)]
		protected MeleeAttack TwoHandedBlockBash;

		// Token: 0x040002D4 RID: 724
		[Nullable(2)]
		protected MeleeAttack OffHandBlockBash;

		// Token: 0x040002D5 RID: 725
		[Nullable(new byte[]
		{
			2,
			1
		})]
		protected Dictionary<AttackDirection, MeleeAttack> DirectionalOneHandedBlockBashes = new Dictionary<AttackDirection, MeleeAttack>();

		// Token: 0x040002D6 RID: 726
		[Nullable(new byte[]
		{
			2,
			1
		})]
		protected Dictionary<AttackDirection, MeleeAttack> DirectionalTwoHandedBlockBashes = new Dictionary<AttackDirection, MeleeAttack>();

		// Token: 0x040002D7 RID: 727
		[Nullable(new byte[]
		{
			2,
			1
		})]
		protected Dictionary<AttackDirection, MeleeAttack> DirectionalOffHandBlockBashes = new Dictionary<AttackDirection, MeleeAttack>();

		// Token: 0x040002D8 RID: 728
		[Nullable(2)]
		protected MeleeAttack OneHandedHandleAttack;

		// Token: 0x040002D9 RID: 729
		[Nullable(2)]
		protected MeleeAttack TwoHandedHandleAttack;

		// Token: 0x040002DA RID: 730
		[Nullable(2)]
		protected MeleeAttack OffHandHandleAttack;

		// Token: 0x040002DB RID: 731
		protected Dictionary<string, MeleeAttack> MainHandDualWieldHandleAttacks = new Dictionary<string, MeleeAttack>();

		// Token: 0x040002DC RID: 732
		protected Dictionary<string, MeleeAttack> OffHandDualWieldHandleAttacks = new Dictionary<string, MeleeAttack>();
	}
}
