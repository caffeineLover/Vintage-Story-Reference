using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Animations;
using CombatOverhaul.Colliders;
using CombatOverhaul.Implementations;
using CombatOverhaul.Inputs;
using CombatOverhaul.MeleeSystems;
using CombatOverhaul.RangedSystems;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Util;

namespace Firearms
{
	// Token: 0x0200001B RID: 27
	[NullableContext(1)]
	[Nullable(0)]
	public class MusketClient : MuzzleloaderClient, IOnGameTick
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x00005268 File Offset: 0x00003468
		public MusketClient(ICoreClientAPI api, Item item) : base(api, item)
		{
			this.StatsMusket = item.Attributes.AsObject<MusketStats>(null);
			this.BayonetAttack = new MeleeAttack(api, this.StatsMusket.BayonetAttack);
			MusketClient.RegisterCollider(item.Code.ToString(), "bayonet-", this.BayonetAttack);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000052CC File Offset: 0x000034CC
		public virtual void RenderDebugCollider(ItemSlot inSlot, IClientPlayer byPlayer)
		{
			MeleeDamageType[] damageTypes = this.BayonetAttack.DamageTypes;
			for (int i = 0; i < damageTypes.Length; i++)
			{
				ICollider collider = damageTypes[i].RelativeCollider.Transform(byPlayer.Entity.Pos, byPlayer.Entity, inSlot, this.Api, true);
				if (collider != null)
				{
					collider.Render(this.Api, byPlayer.Entity, -1);
				}
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005334 File Offset: 0x00003534
		public void OnGameTick(ItemSlot slot, EntityPlayer player, ref int state, bool mainHand)
		{
			if (base.GetState<MusketState>(mainHand) == MusketState.Attack)
			{
				this.TryAttack(this.BayonetAttack, slot, player, mainHand);
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005354 File Offset: 0x00003554
		[HotkeyEventHandler("attach-bayonet", "maltiezfirearms:attach-bayonet", 84, 4, false, false, false)]
		protected virtual bool Bayonet(ItemSlot slot, EntityPlayer player, ref int state, KeyCombination keyCombination, bool mainHand, AttackDirection direction)
		{
			ItemStack itemstack = slot.Itemstack;
			if (!(((itemstack != null) ? itemstack.Item : null) is MusketItem))
			{
				return false;
			}
			this.BayonetInventory.Read(slot, "bayonet");
			if (this.BayonetInventory.Items.Count == 0)
			{
				string bayonetWildcard = this.StatsMusket.BayonetWildcard;
				ItemStack itemstack2 = player.LeftHandItemSlot.Itemstack;
				string text;
				if (itemstack2 == null)
				{
					text = null;
				}
				else
				{
					Item item = itemstack2.Item;
					if (item == null)
					{
						text = null;
					}
					else
					{
						AssetLocation code = item.Code;
						text = ((code != null) ? code.ToString() : null);
					}
				}
				if (!WildcardUtil.Match(bayonetWildcard, text ?? ""))
				{
					return false;
				}
				this.RangedWeaponSystem.Reload(slot, player.LeftHandItemSlot, 1, mainHand, new Action<bool>(this.ServerAttachBayonetCallback), MuzzleloaderClient.SerializeLoadingStage<MusketLoadingStage>(MusketLoadingStage.AttachBayonet));
				this.BayonetInventory.Clear();
			}
			else
			{
				this.RangedWeaponSystem.Reload(slot, player.LeftHandItemSlot, 1, mainHand, new Action<bool>(this.ServerAttachBayonetCallback), MuzzleloaderClient.SerializeLoadingStage<MusketLoadingStage>(MusketLoadingStage.DetachBayonet));
				this.BayonetInventory.Clear();
			}
			return true;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00005457 File Offset: 0x00003657
		protected virtual void ServerAttachBayonetCallback(bool result)
		{
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000545C File Offset: 0x0000365C
		[ActionEventHandler(9, 2)]
		protected virtual bool Attack(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (InteractionsTester.PlayerTriesToInteract(player, mainHand, eventData))
			{
				return false;
			}
			if (RangeWeaponClient.CheckState<MusketState>(state, new MusketState[]
			{
				MusketState.Loading,
				MusketState.Aim,
				MusketState.Priming,
				MusketState.Shoot
			}))
			{
				return false;
			}
			if (RangeWeaponClient.CheckState<MusketState>(state, new MusketState[]
			{
				MusketState.AttackWindup,
				MusketState.Attack,
				MusketState.Cooldown
			}))
			{
				return false;
			}
			this.BayonetInventory.Read(slot, "bayonet");
			if (!Enumerable.Any<ItemStack>(this.BayonetInventory.Items))
			{
				this.BayonetInventory.Clear();
				return false;
			}
			this.BayonetInventory.Clear();
			base.SetState<MusketState>(MusketState.AttackWindup, mainHand);
			this.BayonetAttack.Start(player.Player);
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				bool mainHand2 = mainHand;
				string bayonetAttackAnimation = this.StatsMusket.BayonetAttackAnimation;
				float animationSpeed = base.GetAnimationSpeed(player, "spearsProficiency", 0.5f, 2f);
				animationBehavior.Play(mainHand2, bayonetAttackAnimation, MusketClient.AnimationCategory(mainHand), animationSpeed, 1f, () => this.AttackAnimationCallback(slot, player, mainHand), delegate(string code)
				{
					this.AttackAnimationCallbackHandler(slot, player, code, mainHand);
				}, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				bool mainHand3 = mainHand;
				string bayonetAttackAnimation2 = this.StatsMusket.BayonetAttackAnimation;
				float animationSpeed = base.GetAnimationSpeed(player, "spearsProficiency", 0.5f, 2f);
				tpAnimationBehavior.Play(mainHand3, bayonetAttackAnimation2, MusketClient.AnimationCategory(mainHand), animationSpeed, 1f, true);
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.StopAllVanillaAnimations(mainHand);
			}
			if (this.TpAnimationBehavior == null)
			{
				FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
				if (animationBehavior3 != null)
				{
					animationBehavior3.PlayVanillaAnimation(this.StatsMusket.BayonetAttackTpAnimation, mainHand);
				}
			}
			return true;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000563C File Offset: 0x0000383C
		protected virtual void TryAttack(MeleeAttack attack, ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			bool hasBayonet = false;
			this.BayonetInventory.Read(slot, "bayonet");
			ItemStackMeleeWeaponStats stackStats;
			if (!Enumerable.Any<ItemStack>(this.BayonetInventory.Items))
			{
				stackStats..ctor();
			}
			else
			{
				stackStats = ItemStackMeleeWeaponStats.FromItemStack(this.BayonetInventory.Items[0]);
				hasBayonet = true;
			}
			this.BayonetInventory.Clear();
			IEnumerable<ValueTuple<Block, Vector3d>> terrainCollision;
			IEnumerable<ValueTuple<Entity, Vector3d>> entitiesCollision;
			bool attacked = attack.Attack(player.Player, slot, mainHand, ref terrainCollision, ref entitiesCollision, stackStats, 0);
			if (hasBayonet && attacked && this.StatsMusket.AnimationStaggerOnHitDurationMs > 0f)
			{
				FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
				if (animationBehavior != null)
				{
					animationBehavior.SetSpeedModifier(new AnimationSpeedModifierDelegate(this.AttackImpactFunction));
				}
				this.RangedWeaponSystem.Reload(slot, player.LeftHandItemSlot, 1, mainHand, new Action<bool>(this.ServerAttachBayonetCallback), MuzzleloaderClient.SerializeLoadingStage<MusketLoadingStage>(MusketLoadingStage.DamageBayonet));
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00005710 File Offset: 0x00003910
		protected virtual bool AttackImpactFunction(TimeSpan duration, ref TimeSpan delta)
		{
			TimeSpan totalDuration = TimeSpan.FromMilliseconds((double)this.StatsMusket.AnimationStaggerOnHitDurationMs);
			delta = TimeSpan.Zero;
			return duration < totalDuration;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005744 File Offset: 0x00003944
		protected virtual bool AttackAnimationCallback(ItemSlot slot, EntityPlayer player, bool mainHand)
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
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			int state = (playerBehavior != null) ? playerBehavior.GetState(mainHand) : 0;
			this.OnSelected(slot, player, mainHand, ref state);
			ActionsManagerPlayerBehavior playerBehavior2 = this.PlayerBehavior;
			if (playerBehavior2 != null)
			{
				playerBehavior2.SetState(state, mainHand);
			}
			return true;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000057A8 File Offset: 0x000039A8
		protected virtual void AttackAnimationCallbackHandler(ItemSlot slot, EntityPlayer player, string callbackCode, bool mainHand)
		{
			if (callbackCode == "start")
			{
				base.SetState<MusketState>(MusketState.Attack, mainHand);
				return;
			}
			if (callbackCode == "stop")
			{
				base.SetState<MusketState>(MusketState.Cooldown, mainHand);
				return;
			}
			if (!(callbackCode == "ready"))
			{
				return;
			}
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			int state = (playerBehavior != null) ? playerBehavior.GetState(mainHand) : 0;
			this.OnSelected(slot, player, mainHand, ref state);
			ActionsManagerPlayerBehavior playerBehavior2 = this.PlayerBehavior;
			if (playerBehavior2 == null)
			{
				return;
			}
			playerBehavior2.SetState(state, mainHand);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005827 File Offset: 0x00003A27
		protected static string AnimationCategory(bool mainHand = true)
		{
			if (!mainHand)
			{
				return "mainOffhand";
			}
			return "main";
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005837 File Offset: 0x00003A37
		protected static void RegisterCollider(string item, string type, MeleeAttack attack)
		{
		}

		// Token: 0x04000076 RID: 118
		protected readonly MusketStats StatsMusket;

		// Token: 0x04000077 RID: 119
		protected readonly ItemInventoryBuffer BayonetInventory = new ItemInventoryBuffer();

		// Token: 0x04000078 RID: 120
		protected const string BayonetInventoryId = "bayonet";

		// Token: 0x04000079 RID: 121
		protected MeleeAttack BayonetAttack;
	}
}
