using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CombatOverhaul.Armor;
using CombatOverhaul.Colliders;
using CombatOverhaul.Compatibility;
using CombatOverhaul.Integration;
using CombatOverhaul.Utils;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace CombatOverhaul.DamageSystems
{
	// Token: 0x02000127 RID: 295
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PlayerDamageModelBehavior : EntityBehavior
	{
		// Token: 0x06000E25 RID: 3621 RVA: 0x00043790 File Offset: 0x00041990
		public unsafe PlayerDamageModelBehavior(Entity entity)
		{
			Dictionary<string, PlayerBodyPart> dictionary = new Dictionary<string, PlayerBodyPart>();
			dictionary.Add("LowerTorso", PlayerBodyPart.Torso);
			dictionary.Add("UpperTorso", PlayerBodyPart.Torso);
			dictionary.Add("Head", PlayerBodyPart.Head);
			dictionary.Add("Neck", PlayerBodyPart.Neck);
			dictionary.Add("UpperArmR", PlayerBodyPart.RightArm);
			dictionary.Add("UpperArmL", PlayerBodyPart.LeftArm);
			dictionary.Add("LowerArmR", PlayerBodyPart.RightHand);
			dictionary.Add("LowerArmL", PlayerBodyPart.LeftHand);
			dictionary.Add("UpperFootL", PlayerBodyPart.LeftLeg);
			dictionary.Add("UpperFootR", PlayerBodyPart.RightLeg);
			dictionary.Add("LowerFootL", PlayerBodyPart.LeftFoot);
			dictionary.Add("LowerFootR", PlayerBodyPart.RightFoot);
			this.CollidersToBodyParts = dictionary;
			Dictionary<PlayerBodyPart, DamageZone> dictionary2 = new Dictionary<PlayerBodyPart, DamageZone>();
			dictionary2.Add(PlayerBodyPart.None, DamageZone.None);
			dictionary2.Add(PlayerBodyPart.Head, DamageZone.Head);
			dictionary2.Add(PlayerBodyPart.Face, DamageZone.Face);
			dictionary2.Add(PlayerBodyPart.Neck, DamageZone.Neck);
			dictionary2.Add(PlayerBodyPart.Torso, DamageZone.Torso);
			dictionary2.Add(PlayerBodyPart.LeftArm, DamageZone.Arms);
			dictionary2.Add(PlayerBodyPart.RightArm, DamageZone.Arms);
			dictionary2.Add(PlayerBodyPart.LeftHand, DamageZone.Hands);
			dictionary2.Add(PlayerBodyPart.RightHand, DamageZone.Hands);
			dictionary2.Add(PlayerBodyPart.LeftLeg, DamageZone.Legs);
			dictionary2.Add(PlayerBodyPart.RightLeg, DamageZone.Legs);
			dictionary2.Add(PlayerBodyPart.LeftFoot, DamageZone.Feet);
			dictionary2.Add(PlayerBodyPart.RightFoot, DamageZone.Feet);
			this.BodyPartsToZones = dictionary2;
			Dictionary<DamageZone, string> dictionary3 = new Dictionary<DamageZone, string>();
			dictionary3.Add(DamageZone.Head, "playerHeadDamageFactor");
			dictionary3.Add(DamageZone.Face, "playerFaceDamageFactor");
			dictionary3.Add(DamageZone.Neck, "playerNeckDamageFactor");
			dictionary3.Add(DamageZone.Torso, "playerTorsoDamageFactor");
			dictionary3.Add(DamageZone.Arms, "playerArmsDamageFactor");
			dictionary3.Add(DamageZone.Hands, "playerHandsDamageFactor");
			dictionary3.Add(DamageZone.Legs, "playerLegsDamageFactor");
			dictionary3.Add(DamageZone.Feet, "playerFeetDamageFactor");
			this.MultiplierStats = dictionary3;
			Dictionary<DamageZone, string> dictionary4 = new Dictionary<DamageZone, string>();
			dictionary4.Add(DamageZone.Head, "playerHeadDamageTierReduction");
			dictionary4.Add(DamageZone.Face, "playerFaceDamageTierReduction");
			dictionary4.Add(DamageZone.Neck, "playerNeckDamageTierReduction");
			dictionary4.Add(DamageZone.Torso, "playerTorsoDamageTierReduction");
			dictionary4.Add(DamageZone.Arms, "playerArmsDamageTierReduction");
			dictionary4.Add(DamageZone.Hands, "playerHandsDamageTierReduction");
			dictionary4.Add(DamageZone.Legs, "playerLegsDamageTierReduction");
			dictionary4.Add(DamageZone.Feet, "playerFeetDamageTierReduction");
			this.DamageTierReductionStats = dictionary4;
			Dictionary<EnumDamageType, string> dictionary5 = new Dictionary<EnumDamageType, string>();
			dictionary5.Add(3, "playerSlashingDamageFactor");
			dictionary5.Add(4, "playerPiercingDamageFactor");
			dictionary5.Add(2, "playerBluntDamageFactor");
			this.DamageTypeMultiplierStats = dictionary5;
			Dictionary<EnumDamageType, string> dictionary6 = new Dictionary<EnumDamageType, string>();
			dictionary6.Add(3, "playerSlashingDamageTierReduction");
			dictionary6.Add(4, "playerPiercingDamageTierReduction");
			dictionary6.Add(2, "playerBluntDamageTierReduction");
			this.DamageTypeTierReductionStats = dictionary6;
			int num = 3;
			List<EnumDamageType> list = new List<EnumDamageType>(num);
			CollectionsMarshal.SetCount<EnumDamageType>(list, num);
			Span<EnumDamageType> span = CollectionsMarshal.AsSpan<EnumDamageType>(list);
			*span[0] = 4;
			*span[1] = 3;
			*span[2] = 2;
			this.DamageTypesToProcess = list;
			this._healthAfterSecondChance = 1f;
			this._defaultConfig = new PlayerDamageModelConfig();
			base..ctor(entity);
			this._system = entity.Api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			this._settings = this._system.Settings;
			EntityPlayer entityPlayer = entity as EntityPlayer;
			if (entityPlayer == null)
			{
				throw new ArgumentNullException("entity", "'PlayerDamageModelBehavior' should be attached to player");
			}
			this._player = entityPlayer;
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x06000E26 RID: 3622 RVA: 0x00043AD8 File Offset: 0x00041CD8
		// (remove) Token: 0x06000E27 RID: 3623 RVA: 0x00043B10 File Offset: 0x00041D10
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event OnPlayerReceiveDamageDelegate OnReceiveDamage;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000E28 RID: 3624 RVA: 0x00043B48 File Offset: 0x00041D48
		// (remove) Token: 0x06000E29 RID: 3625 RVA: 0x00043B80 File Offset: 0x00041D80
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public event Action<DamageBlockEventArgs> OnDamageBlocked;

		// Token: 0x06000E2A RID: 3626 RVA: 0x00043BB5 File Offset: 0x00041DB5
		public override string PropertyName()
		{
			return "PlayerDamageModel";
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000E2B RID: 3627 RVA: 0x00043BBC File Offset: 0x00041DBC
		// (set) Token: 0x06000E2C RID: 3628 RVA: 0x00043BC4 File Offset: 0x00041DC4
		public PlayerDamageModel DamageModel { get; private set; } = new PlayerDamageModel(Array.Empty<DamageZoneStatsJson>());

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000E2D RID: 3629 RVA: 0x00043BCD File Offset: 0x00041DCD
		// (set) Token: 0x06000E2E RID: 3630 RVA: 0x00043BD5 File Offset: 0x00041DD5
		public Dictionary<string, PlayerBodyPart> CollidersToBodyParts { get; private set; }

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000E2F RID: 3631 RVA: 0x00043BDE File Offset: 0x00041DDE
		// (set) Token: 0x06000E30 RID: 3632 RVA: 0x00043BE6 File Offset: 0x00041DE6
		public Dictionary<PlayerBodyPart, DamageZone> BodyPartsToZones { get; private set; }

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000E31 RID: 3633 RVA: 0x00043BEF File Offset: 0x00041DEF
		// (set) Token: 0x06000E32 RID: 3634 RVA: 0x00043BF7 File Offset: 0x00041DF7
		public Dictionary<DamageZone, string> MultiplierStats { get; private set; }

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000E33 RID: 3635 RVA: 0x00043C00 File Offset: 0x00041E00
		// (set) Token: 0x06000E34 RID: 3636 RVA: 0x00043C08 File Offset: 0x00041E08
		public Dictionary<DamageZone, string> DamageTierReductionStats { get; private set; }

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06000E35 RID: 3637 RVA: 0x00043C11 File Offset: 0x00041E11
		// (set) Token: 0x06000E36 RID: 3638 RVA: 0x00043C19 File Offset: 0x00041E19
		public Dictionary<EnumDamageType, string> DamageTypeMultiplierStats { get; private set; }

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06000E37 RID: 3639 RVA: 0x00043C22 File Offset: 0x00041E22
		// (set) Token: 0x06000E38 RID: 3640 RVA: 0x00043C2A File Offset: 0x00041E2A
		public Dictionary<EnumDamageType, string> DamageTypeTierReductionStats { get; private set; }

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06000E39 RID: 3641 RVA: 0x00043C33 File Offset: 0x00041E33
		// (set) Token: 0x06000E3A RID: 3642 RVA: 0x00043C3B File Offset: 0x00041E3B
		public List<EnumDamageType> DamageTypesToProcess { get; private set; }

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06000E3B RID: 3643 RVA: 0x00043C44 File Offset: 0x00041E44
		// (set) Token: 0x06000E3C RID: 3644 RVA: 0x00043C4C File Offset: 0x00041E4C
		public TimeSpan SecondChanceDefaultCooldown { get; set; }

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06000E3D RID: 3645 RVA: 0x00043C55 File Offset: 0x00041E55
		public TimeSpan SecondChanceCooldown
		{
			get
			{
				return this.SecondChanceDefaultCooldown * (double)this.entity.Stats.GetBlended("secondChanceCooldown");
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000E3E RID: 3646 RVA: 0x00043C78 File Offset: 0x00041E78
		// (set) Token: 0x06000E3F RID: 3647 RVA: 0x00043C80 File Offset: 0x00041E80
		public TimeSpan SecondChanceDefaultGracePeriod { get; set; }

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000E40 RID: 3648 RVA: 0x00043C89 File Offset: 0x00041E89
		public TimeSpan SecondChanceGracePeriod
		{
			get
			{
				return this.SecondChanceDefaultGracePeriod * (double)this.entity.Stats.GetBlended("secondChanceGracePeriod");
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000E41 RID: 3649 RVA: 0x00043CAC File Offset: 0x00041EAC
		// (set) Token: 0x06000E42 RID: 3650 RVA: 0x00043CB4 File Offset: 0x00041EB4
		public bool SecondChanceAvailable { get; set; }

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000E43 RID: 3651 RVA: 0x00043CBD File Offset: 0x00041EBD
		// (set) Token: 0x06000E44 RID: 3652 RVA: 0x00043CC5 File Offset: 0x00041EC5
		[Nullable(2)]
		public DamageBlockStats CurrentDamageBlock { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x00043CCE File Offset: 0x00041ECE
		// (set) Token: 0x06000E46 RID: 3654 RVA: 0x00043CD6 File Offset: 0x00041ED6
		[Nullable(2)]
		internal DamageBlockEventArgs LastDamageBlock { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x06000E47 RID: 3655 RVA: 0x00043CDF File Offset: 0x00041EDF
		[return: Nullable(2)]
		internal DamageBlockEventArgs GetLastDamageBlock(DamageSource damageSource)
		{
			DamageBlockEventArgs lastDamageBlock = this.LastDamageBlock;
			if (((lastDamageBlock != null) ? lastDamageBlock.DamageSource : null) != damageSource)
			{
				return null;
			}
			return this.LastDamageBlock;
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x00043CFE File Offset: 0x00041EFE
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			this._serverSide = (this.entity.Api.Side == 1);
			this._defaultConfig = attributes.AsObject<PlayerDamageModelConfig>(null);
			this.ApplyConfig(this._defaultConfig);
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x00043D34 File Offset: 0x00041F34
		public override void AfterInitialized(bool onFirstSpawn)
		{
			this._colliders = this.entity.GetBehavior<CollidersEntityBehavior>();
			if (this._serverSide)
			{
				this._subscribedHealthBehavior = this.entity.GetBehavior<EntityBehaviorHealth>();
				if (this._subscribedHealthBehavior != null)
				{
					this._subscribedHealthBehavior.onDamaged += new OnDamagedDelegate(this.OnReceiveDamageHandler);
				}
			}
			if (this.entity.Api.ModLoader.IsModEnabled("playermodellib"))
			{
				this.SubscribeOnModelChange();
			}
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x00043DAC File Offset: 0x00041FAC
		public override void OnEntityDespawn(EntityDespawnData despawn)
		{
			if (this._subscribedHealthBehavior != null)
			{
				this._subscribedHealthBehavior.onDamaged -= new OnDamagedDelegate(this.OnReceiveDamageHandler);
				this._subscribedHealthBehavior = null;
			}
			PlayerModelSkinSubscription subscribedSkinBehavior = this._subscribedSkinBehavior;
			if (subscribedSkinBehavior != null)
			{
				subscribedSkinBehavior.Dispose();
			}
			this._subscribedSkinBehavior = null;
			base.OnEntityDespawn(despawn);
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x00043E00 File Offset: 0x00042000
		public override void OnGameTick(float deltaTime)
		{
			if (!this._serverSide)
			{
				return;
			}
			if (this._settings.DebugBlockAnglesParticles && this.CurrentDamageBlock != null)
			{
				ICoreServerAPI serverApi = this.entity.Api as ICoreServerAPI;
				if (serverApi != null)
				{
					DebugParticleSpawner.SpawnDebugBlockParticles(serverApi, this.entity, this.CurrentDamageBlock);
				}
			}
			long currentTime = this.entity.World.ElapsedMilliseconds;
			if (this._nextTemperatureDamageCheck < currentTime)
			{
				this.ApplyDamageFromHotClothes();
				this._nextTemperatureDamageCheck = currentTime + 1000L;
			}
			float secondChanceCooldown = this.entity.WatchedAttributes.GetFloat("secondChanceCooldown", 0f);
			if (secondChanceCooldown > 0f)
			{
				secondChanceCooldown = Math.Clamp(secondChanceCooldown - deltaTime, 0f, secondChanceCooldown);
				this.entity.WatchedAttributes.SetFloat("secondChanceCooldown", secondChanceCooldown);
			}
			float secondChanceGracePeriod = this.entity.WatchedAttributes.GetFloat("secondChanceGracePeriod", 0f);
			if (secondChanceGracePeriod > 0f)
			{
				secondChanceGracePeriod = Math.Clamp(secondChanceGracePeriod - deltaTime, 0f, secondChanceGracePeriod);
				this.entity.WatchedAttributes.SetFloat("secondChanceGracePeriod", secondChanceGracePeriod);
			}
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00043F10 File Offset: 0x00042110
		public static float GetDamageReductionFactor(IPlayer player, DamageZone zone, EnumDamageType damageType, int damageTier, DamageReceivedCalculationType calculationType)
		{
			ArmorInventory inventory = player.InventoryManager.GetOwnInventory("character") as ArmorInventory;
			if (inventory == null)
			{
				return 0f;
			}
			if (zone == DamageZone.None)
			{
				return 0f;
			}
			float damage = 1f;
			PlayerDamageModelBehavior damageModelBehavior = player.Entity.GetBehavior<PlayerDamageModelBehavior>();
			if (calculationType == DamageReceivedCalculationType.WithBodyParts && damageModelBehavior != null)
			{
				float zoneMultiplier = Enumerable.Average(Enumerable.Select<PlayerBodyPart, float>(Enumerable.Select<KeyValuePair<PlayerBodyPart, DamageZone>, PlayerBodyPart>(Enumerable.Where<KeyValuePair<PlayerBodyPart, DamageZone>>(damageModelBehavior.BodyPartsToZones, (KeyValuePair<PlayerBodyPart, DamageZone> entry) => entry.Value == zone), (KeyValuePair<PlayerBodyPart, DamageZone> entry) => entry.Key), new Func<PlayerBodyPart, float>(damageModelBehavior.DamageModel.GetMultiplier)));
				damage *= zoneMultiplier * damageModelBehavior.GetDamageMultiplier(zone, damageType);
			}
			if (calculationType == DamageReceivedCalculationType.HitChance && damageModelBehavior != null)
			{
				DirectionOffset direction = new DirectionOffset(Angle.FromDegrees(-1f), Angle.FromDegrees(1f));
				float totalWeight = Enumerable.Sum(Enumerable.Select<DamageZoneStats, float>(Enumerable.Where<DamageZoneStats>(damageModelBehavior.DamageModel.DamageZones, (DamageZoneStats value) => value.Directions.Check(direction)), (DamageZoneStats value) => value.Coverage));
				if (totalWeight <= 0f)
				{
					return 0f;
				}
				IEnumerable<PlayerBodyPart> bodyParts = Enumerable.Select<KeyValuePair<PlayerBodyPart, DamageZone>, PlayerBodyPart>(Enumerable.Where<KeyValuePair<PlayerBodyPart, DamageZone>>(damageModelBehavior.BodyPartsToZones, (KeyValuePair<PlayerBodyPart, DamageZone> entry) => entry.Value == zone), (KeyValuePair<PlayerBodyPart, DamageZone> entry) => entry.Key);
				float zoneWieght = Enumerable.Sum(Enumerable.Select<DamageZoneStats, float>(Enumerable.Where<DamageZoneStats>(Enumerable.Where<DamageZoneStats>(damageModelBehavior.DamageModel.DamageZones, (DamageZoneStats value) => Enumerable.Contains<PlayerBodyPart>(bodyParts, value.ZoneType)), (DamageZoneStats value) => value.Directions.Check(direction)), (DamageZoneStats value) => value.Coverage));
				damage *= zoneWieght / totalWeight;
				return damage;
			}
			else
			{
				if (calculationType == DamageReceivedCalculationType.HitChance)
				{
					return 0f;
				}
				if (calculationType == DamageReceivedCalculationType.Average && damageModelBehavior != null)
				{
					DirectionOffset direction = new DirectionOffset(Angle.Zero, Angle.FromDegrees(1f));
					float totalWeight2 = Enumerable.Sum(Enumerable.Select<DamageZoneStats, float>(Enumerable.Where<DamageZoneStats>(damageModelBehavior.DamageModel.DamageZones, (DamageZoneStats value) => value.Directions.Check(direction)), (DamageZoneStats value) => value.Coverage * value.DamageMultiplier * damageModelBehavior.GetDamageMultiplier(damageModelBehavior.BodyPartsToZones[value.ZoneType], damageType)));
					if (totalWeight2 <= 0f)
					{
						return damage;
					}
					IEnumerable<PlayerBodyPart> bodyParts = Enumerable.Select<KeyValuePair<PlayerBodyPart, DamageZone>, PlayerBodyPart>(Enumerable.Where<KeyValuePair<PlayerBodyPart, DamageZone>>(damageModelBehavior.BodyPartsToZones, (KeyValuePair<PlayerBodyPart, DamageZone> entry) => entry.Value == zone), (KeyValuePair<PlayerBodyPart, DamageZone> entry) => entry.Key);
					float zoneWieght2 = Enumerable.Sum(Enumerable.Select<DamageZoneStats, float>(Enumerable.Where<DamageZoneStats>(Enumerable.Where<DamageZoneStats>(damageModelBehavior.DamageModel.DamageZones, (DamageZoneStats value) => Enumerable.Contains<PlayerBodyPart>(bodyParts, value.ZoneType)), (DamageZoneStats value) => value.Directions.Check(direction)), (DamageZoneStats value) => value.Coverage * value.DamageMultiplier * damageModelBehavior.GetDamageMultiplier(damageModelBehavior.BodyPartsToZones[value.ZoneType], damageType)));
					damage *= zoneWieght2 / totalWeight2;
				}
				IEnumerable<ArmorSlot> slots = inventory.GetNotEmptyZoneSlots(zone);
				if (!Enumerable.Any<ArmorSlot>(slots))
				{
					return damage;
				}
				DamageResistData damageResistData = DamageResistData.Combine(Enumerable.Select<ArmorSlot, DamageResistData>(Enumerable.Where<ArmorSlot>(Enumerable.Where<ArmorSlot>(slots, delegate(ArmorSlot slot)
				{
					object obj;
					if (slot == null)
					{
						obj = null;
					}
					else
					{
						ItemStack itemstack = slot.Itemstack;
						obj = ((itemstack != null) ? itemstack.Item : null);
					}
					return obj != null;
				}), delegate(ArmorSlot slot)
				{
					int? num;
					int num2;
					if (slot != null)
					{
						ItemStack itemstack = slot.Itemstack;
						num = ((itemstack != null) ? new int?(itemstack.Item.GetRemainingDurability(slot.Itemstack)) : default(int?));
						num2 = 0;
						if (num.GetValueOrDefault() > num2 & num != null)
						{
							return true;
						}
					}
					if (slot == null)
					{
						return false;
					}
					ItemStack itemstack2 = slot.Itemstack;
					num = ((itemstack2 != null) ? new int?(itemstack2.Item.GetMaxDurability(slot.Itemstack)) : default(int?));
					num2 = 0;
					return num.GetValueOrDefault() == num2 & num != null;
				}), (ArmorSlot slot) => slot.GetResists(zone)));
				if (damageModelBehavior != null)
				{
					damageTier = Math.Max(0, damageTier - damageModelBehavior.GetDamageTierReduction(zone, damageType));
				}
				int durabilityDamage = 0;
				damageResistData.ApplyPlayerResist(new DamageData(damageType, damageTier, 0), ref damage, out durabilityDamage);
				return damage;
			}
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x00044324 File Offset: 0x00042524
		private float OnReceiveDamageHandler(float damage, DamageSource damageSource)
		{
			this.LastDamageBlock = null;
			if (!this.DamageTypesToProcess.Contains(damageSource.Type))
			{
				return damage;
			}
			ValueTuple<PlayerBodyPart, float> valueTuple = this.DetermineHitZone(damageSource);
			PlayerBodyPart detailedDamageZone = valueTuple.Item1;
			float multiplier = valueTuple.Item2;
			DamageZone damageZone = this.BodyPartsToZones[detailedDamageZone];
			multiplier *= this.GetDamageMultiplier(damageZone, damageSource.Type);
			damageSource.DamageTier = Math.Max(0, damageSource.DamageTier - this.GetDamageTierReduction(damageZone, damageSource.Type));
			string blockDamageLogMessage;
			this.ApplyBlock(damageSource, detailedDamageZone, ref damage, out blockDamageLogMessage);
			this.PrintToDamageLog(blockDamageLogMessage);
			string armorDamageLogMessage;
			EnumDamageType damageType;
			this.ApplyArmorResists(damageSource, damageZone, ref damage, out armorDamageLogMessage, out damageType);
			this.PrintToDamageLog(armorDamageLogMessage);
			damage *= multiplier;
			if (this._settings.SecondChanceAvailable && this.SecondChanceAvailable)
			{
				this.ApplySecondChance(ref damage);
			}
			OnPlayerReceiveDamageDelegate onReceiveDamage = this.OnReceiveDamage;
			if (onReceiveDamage != null)
			{
				onReceiveDamage(ref damage, damageSource, detailedDamageZone);
			}
			if (damage != 0f)
			{
				string text = "combatoverhaul:damagelog-received-damage";
				object[] array = new object[3];
				int num = 0;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<float>(damage, "F1");
				array[num] = defaultInterpolatedStringHandler.ToStringAndClear();
				int num2 = 1;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(36, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("combatoverhaul:detailed-damage-zone-");
				defaultInterpolatedStringHandler2.AppendFormatted<PlayerBodyPart>(detailedDamageZone);
				array[num2] = Lang.Get(defaultInterpolatedStringHandler2.ToStringAndClear(), Array.Empty<object>());
				int num3 = 2;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
				defaultInterpolatedStringHandler3..ctor(27, 1);
				defaultInterpolatedStringHandler3.AppendLiteral("combatoverhaul:damage-type-");
				defaultInterpolatedStringHandler3.AppendFormatted<EnumDamageType>(damageType);
				array[num3] = Lang.Get(defaultInterpolatedStringHandler3.ToStringAndClear(), Array.Empty<object>());
				string damageLogMessage = Lang.Get(text, array);
				this.PrintToDamageLog(damageLogMessage);
			}
			return damage;
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x000444AC File Offset: 0x000426AC
		private void PrintToDamageLog(string message)
		{
			if (this._settings.PrintPlayerHits && message != "")
			{
				EntityPlayer entityPlayer = this.entity as EntityPlayer;
				IServerPlayer serverPlayer = ((entityPlayer != null) ? entityPlayer.Player : null) as IServerPlayer;
				if (serverPlayer == null)
				{
					return;
				}
				serverPlayer.SendMessage(GlobalConstants.DamageLogChatGroup, message, 4, null);
			}
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00044504 File Offset: 0x00042704
		public float GetDamageMultiplier(DamageZone zone, EnumDamageType damageType)
		{
			float fullBodyMultiplier = this._player.Stats.GetBlended("playerDamageFactor");
			float damageTypeMultiplier = this.GetBlendedOrDefault<EnumDamageType>(this.DamageTypeMultiplierStats, damageType, 1f);
			float bodyZoneMultiplier = this.GetBlendedOrDefault<DamageZone>(this.MultiplierStats, zone, 1f);
			return Math.Max(0f, fullBodyMultiplier * damageTypeMultiplier * bodyZoneMultiplier);
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0004455C File Offset: 0x0004275C
		public int GetDamageTierReduction(DamageZone zone, EnumDamageType damageType)
		{
			float statDelta = this.GetStatDelta("playerDamageTierReduction");
			float damageTypeReduction = this.GetStatDeltaOrDefault<EnumDamageType>(this.DamageTypeTierReductionStats, damageType);
			float bodyZoneReduction = this.GetStatDeltaOrDefault<DamageZone>(this.DamageTierReductionStats, zone);
			return (int)(statDelta + damageTypeReduction + bodyZoneReduction);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x00044598 File Offset: 0x00042798
		private float GetBlendedOrDefault<TKey>(Dictionary<TKey, string> statsByKey, TKey key, float defaultValue)
		{
			string stat;
			if (!statsByKey.TryGetValue(key, ref stat))
			{
				return defaultValue;
			}
			return this._player.Stats.GetBlended(stat);
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x000445C4 File Offset: 0x000427C4
		private float GetStatDeltaOrDefault<TKey>(Dictionary<TKey, string> statsByKey, TKey key)
		{
			string stat;
			if (!statsByKey.TryGetValue(key, ref stat))
			{
				return 0f;
			}
			return this.GetStatDelta(stat);
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x000445E9 File Offset: 0x000427E9
		private float GetStatDelta(string stat)
		{
			return this._player.Stats.GetBlended(stat) - 1f;
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x00044604 File Offset: 0x00042804
		[NullableContext(0)]
		[return: TupleElementNames(new string[]
		{
			"zone",
			"multiplier"
		})]
		private ValueTuple<PlayerBodyPart, float> DetermineHitZone([Nullable(1)] DamageSource damageSource)
		{
			PlayerBodyPart damageZone;
			float multiplier;
			if (this._colliders != null)
			{
				ILocationalDamage locationalDamageSource = damageSource as ILocationalDamage;
				if (locationalDamageSource != null && locationalDamageSource.Collider != "" && this.CollidersToBodyParts.TryGetValue(locationalDamageSource.Collider, ref damageZone))
				{
					multiplier = this.DamageModel.GetMultiplier(damageZone);
					goto IL_10B;
				}
			}
			IDirectionalDamage directionalDamage = damageSource as IDirectionalDamage;
			if (directionalDamage != null)
			{
				ValueTuple<PlayerBodyPart, float> zone = this.DamageModel.GetZone(new DirectionOffset?(directionalDamage.Direction), directionalDamage.Target, directionalDamage.WeightMultiplier);
				damageZone = zone.Item1;
				multiplier = zone.Item2;
			}
			else if (damageSource.SourceEntity != null && damageSource.SourceEntity.EntityId != this.entity.EntityId)
			{
				DirectionOffset direction = DirectionOffset.GetDirection(this.entity, damageSource.SourceEntity);
				ValueTuple<PlayerBodyPart, float> zone2 = this.DamageModel.GetZone(new DirectionOffset?(direction), PlayerBodyPart.None, 1f);
				damageZone = zone2.Item1;
				multiplier = zone2.Item2;
			}
			else
			{
				ValueTuple<PlayerBodyPart, float> zone3 = this.DamageModel.GetZone(default(DirectionOffset?), PlayerBodyPart.None, 1f);
				damageZone = zone3.Item1;
				multiplier = zone3.Item2;
			}
			IL_10B:
			return new ValueTuple<PlayerBodyPart, float>(damageZone, multiplier);
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00044724 File Offset: 0x00042924
		private void ApplyBlock(DamageSource damageSource, PlayerBodyPart zone, ref float damage, out string damageLogMessage)
		{
			damageLogMessage = "";
			if (this.CurrentDamageBlock == null)
			{
				return;
			}
			if (this._serverSide)
			{
				IServerPlayer serverPlayer = this._player.Player as IServerPlayer;
				if (serverPlayer != null && !ServerDamageBlockProfiles.IsStillAuthorized(serverPlayer, this.CurrentDamageBlock))
				{
					this.CurrentDamageBlock = null;
					return;
				}
			}
			if (!this.CurrentDamageBlock.CanBlockProjectiles && this.IsCausedByProjectile(damageSource))
			{
				damageLogMessage = Lang.Get("combatoverhaul:damagelog-missed-block-projectile", Array.Empty<object>());
				return;
			}
			if ((zone & this.CurrentDamageBlock.ZoneType) == PlayerBodyPart.None)
			{
				string text = "combatoverhaul:damagelog-missed-block-zone";
				object[] array = new object[1];
				int num = 0;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(36, 1);
				defaultInterpolatedStringHandler.AppendLiteral("combatoverhaul:detailed-damage-zone-");
				defaultInterpolatedStringHandler.AppendFormatted<PlayerBodyPart>(zone);
				array[num] = Lang.Get(defaultInterpolatedStringHandler.ToStringAndClear(), Array.Empty<object>());
				damageLogMessage = Lang.Get(text, array);
				return;
			}
			DirectionOffset direction;
			if (this.GetAttackDirection(damageSource, out direction) && !this.CurrentDamageBlock.Directions.Check(direction))
			{
				damageLogMessage = Lang.Get("combatoverhaul:damagelog-missed-block-direction", new object[]
				{
					direction
				});
				return;
			}
			int damageTier = damageSource.DamageTier;
			int blockTier = 0;
			float initialDamage = damage;
			EnumDamageType damageType = damageSource.Type;
			if (this.CurrentDamageBlock.BlockTier != null)
			{
				if (!this.CurrentDamageBlock.BlockTier.ContainsKey(damageType))
				{
					string text2 = "combatoverhaul:damagelog-missed-block-damageType";
					object[] array2 = new object[1];
					int num2 = 0;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
					defaultInterpolatedStringHandler2..ctor(12, 1);
					defaultInterpolatedStringHandler2.AppendLiteral("damage-type-");
					defaultInterpolatedStringHandler2.AppendFormatted<EnumDamageType>(damageType);
					array2[num2] = Lang.Get(defaultInterpolatedStringHandler2.ToStringAndClear(), Array.Empty<object>());
					damageLogMessage = Lang.Get(text2, array2);
					return;
				}
				blockTier = this.CurrentDamageBlock.BlockTier[damageType];
				if (blockTier < damageTier)
				{
					this.ApplyBlockResists((float)blockTier, (float)damageTier, ref damage);
					damageSource.DamageTier = Math.Clamp(damageTier - blockTier, 0, damageTier);
					string text3 = "combatoverhaul:damagelog-partial-block";
					object[] array3 = new object[2];
					int num3 = 0;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
					defaultInterpolatedStringHandler3..ctor(36, 1);
					defaultInterpolatedStringHandler3.AppendLiteral("combatoverhaul:detailed-damage-zone-");
					defaultInterpolatedStringHandler3.AppendFormatted<PlayerBodyPart>(zone);
					array3[num3] = Lang.Get(defaultInterpolatedStringHandler3.ToStringAndClear(), Array.Empty<object>());
					int num4 = 1;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4;
					defaultInterpolatedStringHandler4..ctor(0, 1);
					defaultInterpolatedStringHandler4.AppendFormatted<int>(damageSource.DamageTier, "F0");
					array3[num4] = defaultInterpolatedStringHandler4.ToStringAndClear();
					damageLogMessage = Lang.Get(text3, array3);
				}
				else
				{
					string text4 = "combatoverhaul:damagelog-success-block";
					object[] array4 = new object[2];
					int num5 = 0;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5;
					defaultInterpolatedStringHandler5..ctor(36, 1);
					defaultInterpolatedStringHandler5.AppendLiteral("combatoverhaul:detailed-damage-zone-");
					defaultInterpolatedStringHandler5.AppendFormatted<PlayerBodyPart>(zone);
					array4[num5] = Lang.Get(defaultInterpolatedStringHandler5.ToStringAndClear(), Array.Empty<object>());
					int num6 = 1;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler6;
					defaultInterpolatedStringHandler6..ctor(0, 1);
					defaultInterpolatedStringHandler6.AppendFormatted<float>(damage, "F1");
					array4[num6] = defaultInterpolatedStringHandler6.ToStringAndClear();
					damageLogMessage = Lang.Get(text4, array4);
					damage = 0f;
				}
			}
			else
			{
				damage = 0f;
			}
			if (this.CurrentDamageBlock.StaggerTime > TimeSpan.Zero)
			{
				Entity sourceEntity = damageSource.SourceEntity;
				if (sourceEntity != null)
				{
					StaggerBehavior behavior = sourceEntity.GetBehavior<StaggerBehavior>();
					if (behavior != null)
					{
						behavior.TriggerStagger(this.CurrentDamageBlock.StaggerTime, this.CurrentDamageBlock.StaggerTier);
					}
				}
			}
			this.CurrentDamageBlock.Callback.Invoke(initialDamage - damage, damageTier, blockTier);
			DamageBlockEventArgs args = new DamageBlockEventArgs(this._player, damageSource, zone, this.CurrentDamageBlock.Kind, this.CurrentDamageBlock.MainHand, initialDamage, damage, damageTier, blockTier);
			this.LastDamageBlock = args;
			Action<DamageBlockEventArgs> onDamageBlocked = this.OnDamageBlocked;
			if (onDamageBlocked != null)
			{
				onDamageBlocked.Invoke(args);
			}
			MeleeBlockSystemServer serverBlockSystem = this._system.ServerBlockSystem;
			if (serverBlockSystem != null)
			{
				serverBlockSystem.EmitDamageBlocked(args);
			}
			if (this.CurrentDamageBlock.Sound != null)
			{
				this.entity.Api.World.PlaySoundAt(new AssetLocation(this.CurrentDamageBlock.Sound), this.entity, null, false, 16f, 1f);
			}
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00044AC8 File Offset: 0x00042CC8
		private void ApplyArmorResists(DamageSource damageSource, DamageZone zone, ref float damage, out string damageLogMessage, out EnumDamageType damageType)
		{
			damageLogMessage = "";
			damageType = damageSource.Type;
			EntityPlayer entityPlayer = this.entity as EntityPlayer;
			ArmorInventory inventory = ((entityPlayer != null) ? entityPlayer.Player.InventoryManager.GetOwnInventory("character") : null) as ArmorInventory;
			if (inventory == null)
			{
				return;
			}
			if (zone == DamageZone.None)
			{
				return;
			}
			List<ArmorSlot> slots = Enumerable.ToList<ArmorSlot>(inventory.GetNotEmptyZoneSlots(zone));
			if (slots.Count == 0)
			{
				return;
			}
			IEnumerable<ValueTuple<ArmorSlot, DamageResistData>> protectedSlots = Enumerable.Select<ArmorSlot, ValueTuple<ArmorSlot, DamageResistData>>(Enumerable.Where<ArmorSlot>(Enumerable.Where<ArmorSlot>(slots, delegate(ArmorSlot slot)
			{
				object obj;
				if (slot == null)
				{
					obj = null;
				}
				else
				{
					ItemStack itemstack = slot.Itemstack;
					obj = ((itemstack != null) ? itemstack.Item : null);
				}
				return obj != null;
			}), delegate(ArmorSlot slot)
			{
				int? num4;
				int num5;
				if (slot != null)
				{
					ItemStack itemstack = slot.Itemstack;
					num4 = ((itemstack != null) ? new int?(itemstack.Item.GetRemainingDurability(slot.Itemstack)) : default(int?));
					num5 = 0;
					if (num4.GetValueOrDefault() > num5 & num4 != null)
					{
						return true;
					}
				}
				if (slot == null)
				{
					return false;
				}
				ItemStack itemstack2 = slot.Itemstack;
				num4 = ((itemstack2 != null) ? new int?(itemstack2.Item.GetMaxDurability(slot.Itemstack)) : default(int?));
				num5 = 0;
				return num4.GetValueOrDefault() == num5 & num4 != null;
			}), (ArmorSlot slot) => new ValueTuple<ArmorSlot, DamageResistData>(slot, slot.GetResists(zone)));
			DirectionOffset direction;
			if (this.GetAttackDirection(damageSource, out direction))
			{
				protectedSlots = Enumerable.Where<ValueTuple<ArmorSlot, DamageResistData>>(protectedSlots, ([TupleElementNames(new string[]
				{
					"Slot",
					"Resists"
				})] ValueTuple<ArmorSlot, DamageResistData> slot) => slot.Item2.CheckDirection(direction));
			}
			List<ValueTuple<ArmorSlot, DamageResistData>> protectedSlotList = Enumerable.ToList<ValueTuple<ArmorSlot, DamageResistData>>(protectedSlots);
			if (protectedSlotList.Count == 0)
			{
				return;
			}
			DamageResistData damageResistData = DamageResistData.Combine(Enumerable.Select<ValueTuple<ArmorSlot, DamageResistData>, DamageResistData>(protectedSlotList, ([TupleElementNames(new string[]
			{
				"Slot",
				"Resists"
			})] ValueTuple<ArmorSlot, DamageResistData> slot) => slot.Item2));
			float previousDamage = damage;
			int durabilityDamage = 0;
			int apTier = 0;
			IArmorPiercing apSource = damageSource as IArmorPiercing;
			if (apSource != null)
			{
				apTier = apSource.ArmorPiercingTier;
			}
			damageResistData.ApplyPlayerResist(new DamageData(damageSource.Type, damageSource.DamageTier, apTier), ref damage, out durabilityDamage);
			durabilityDamage = Math.Max(durabilityDamage, 1);
			int totalDurabilityDamage;
			this.DamageArmor(protectedSlotList, damageType, durabilityDamage, out totalDurabilityDamage);
			if (previousDamage - damage > 0f)
			{
				string text = "combatoverhaul:damagelog-armor-damage-negation";
				object[] array = new object[5];
				int num = 0;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<float>(previousDamage - damage, "F1");
				array[num] = defaultInterpolatedStringHandler.ToStringAndClear();
				int num2 = 1;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(27, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("combatoverhaul:damage-zone-");
				defaultInterpolatedStringHandler2.AppendFormatted<DamageZone>(zone);
				array[num2] = Lang.Get(defaultInterpolatedStringHandler2.ToStringAndClear(), Array.Empty<object>());
				array[2] = totalDurabilityDamage;
				int num3 = 3;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3;
				defaultInterpolatedStringHandler3..ctor(27, 1);
				defaultInterpolatedStringHandler3.AppendLiteral("combatoverhaul:damage-type-");
				defaultInterpolatedStringHandler3.AppendFormatted<EnumDamageType>(damageType);
				array[num3] = Lang.Get(defaultInterpolatedStringHandler3.ToStringAndClear(), Array.Empty<object>());
				array[4] = damageSource.DamageTier;
				damageLogMessage = Lang.Get(text, array);
			}
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00044D20 File Offset: 0x00042F20
		private void ApplyBlockResists(float blockTier, float damageTier, ref float damage)
		{
			float multiplier = damageTier > blockTier;
			damage *= multiplier;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00044D3C File Offset: 0x00042F3C
		private void ApplySecondChance(ref float damage)
		{
			EntityBehaviorHealth behavior = this.entity.GetBehavior<EntityBehaviorHealth>();
			float currentHealth = (behavior != null) ? behavior.Health : 0f;
			if (currentHealth > damage)
			{
				return;
			}
			float secondChanceCooldown = this.entity.WatchedAttributes.GetFloat("secondChanceCooldown", 0f);
			float secondChanceGracePeriod = this.entity.WatchedAttributes.GetFloat("secondChanceGracePeriod", 0f);
			if (secondChanceGracePeriod > 0f)
			{
				damage *= 0f;
				this.PrintToDamageLog(Lang.Get("combatoverhaul:damagelog-second-chance-grace-period", new object[]
				{
					(int)secondChanceGracePeriod
				}));
				this.SpawnGracePeriodPArticles();
				return;
			}
			if (secondChanceCooldown > 0f)
			{
				this.PrintToDamageLog(Lang.Get("combatoverhaul:damagelog-second-chance-cooldown", new object[]
				{
					(int)secondChanceCooldown
				}));
				return;
			}
			this.entity.WatchedAttributes.SetFloat("secondChanceCooldown", (float)this.SecondChanceCooldown.TotalSeconds);
			this.entity.WatchedAttributes.SetFloat("secondChanceGracePeriod", (float)this.SecondChanceGracePeriod.TotalSeconds);
			damage = currentHealth - this._healthAfterSecondChance;
			this.SpawnSecondChanceParticles();
			this.PrintToDamageLog(Lang.Get("combatoverhaul:damagelog-second-chance", Array.Empty<object>()));
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x00044E70 File Offset: 0x00043070
		private void SpawnSecondChanceParticles()
		{
			if (!this._settings.SecondChanceParticles)
			{
				return;
			}
			CombatOverhaulAnimationsSystem modSystem = this.entity.Api.ModLoader.GetModSystem<CombatOverhaulAnimationsSystem>(true);
			object obj = (modSystem != null) ? modSystem.ParticleEffectsManager : null;
			Vec3f position = (this.entity.Pos.XYZ + this.entity.LocalEyePos * 0.5).ToVec3f();
			object obj2 = obj;
			if (obj2 == null)
			{
				return;
			}
			obj2.Spawn("combatoverhaul:second-chance", new Vector3(position.X, position.Y, position.Z), default(Vector3), 1f);
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x00044F18 File Offset: 0x00043118
		private void SpawnGracePeriodPArticles()
		{
			if (!this._settings.SecondChanceParticles)
			{
				return;
			}
			CombatOverhaulAnimationsSystem modSystem = this.entity.Api.ModLoader.GetModSystem<CombatOverhaulAnimationsSystem>(true);
			object obj = (modSystem != null) ? modSystem.ParticleEffectsManager : null;
			Vec3f position = (this.entity.Pos.XYZ + this.entity.LocalEyePos * 0.5).ToVec3f();
			object obj2 = obj;
			if (obj2 == null)
			{
				return;
			}
			obj2.Spawn("combatoverhaul:grace-period", new Vector3(position.X, position.Y, position.Z), default(Vector3), 1f);
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x00044FC0 File Offset: 0x000431C0
		private bool IsCausedByProjectile(DamageSource damageSource)
		{
			Entity sourceEntity = damageSource.SourceEntity;
			Entity causeEntity = damageSource.CauseEntity;
			return sourceEntity != null && causeEntity != null && sourceEntity != causeEntity;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x00044FEC File Offset: 0x000431EC
		private void ApplyConfig(PlayerDamageModelConfig config)
		{
			this.DamageModel = new PlayerDamageModel(config.DamageModel.Zones);
			this.CollidersToBodyParts = config.BodyParts;
			this.SecondChanceDefaultCooldown = TimeSpan.FromSeconds((double)config.SecondChanceCooldownSec);
			this.SecondChanceAvailable = config.SecondChanceAvailable;
			this.SecondChanceDefaultGracePeriod = TimeSpan.FromSeconds((double)config.SecondChanceGracePeriodSec);
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0004504B File Offset: 0x0004324B
		private void SubscribeOnModelChange()
		{
			this._subscribedSkinBehavior = PlayerModelSkinSubscription.Subscribe(this.entity, new Action<string>(this.ReloadConfigForCustomModel), false);
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x0004506C File Offset: 0x0004326C
		private void ReloadConfigForCustomModel(string modelCode)
		{
			ICoreAPI api = this.entity.Api;
			PlayerModelLibCompatibilitySystem system = (api != null) ? api.ModLoader.GetModSystem<PlayerModelLibCompatibilitySystem>(true) : null;
			if (system == null)
			{
				return;
			}
			PlayerModelConfig customModelConfig;
			if (!system.CustomModelConfigs.TryGetValue(modelCode, ref customModelConfig) || customModelConfig.DamageModel == null)
			{
				this.ApplyConfig(this._defaultConfig);
				return;
			}
			this.DamageModel = new PlayerDamageModel(customModelConfig.DamageModel.DamageModel.Zones);
			this.CollidersToBodyParts = customModelConfig.DamageModel.BodyParts;
			this.SecondChanceDefaultCooldown = TimeSpan.FromSeconds((double)customModelConfig.DamageModel.SecondChanceCooldownSec);
			this.SecondChanceAvailable = customModelConfig.DamageModel.SecondChanceAvailable;
			this.SecondChanceDefaultGracePeriod = TimeSpan.FromSeconds((double)customModelConfig.DamageModel.SecondChanceGracePeriodSec);
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x0004512C File Offset: 0x0004332C
		private void DamageArmor([TupleElementNames(new string[]
		{
			"Slot",
			"Resists"
		})] [Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] IEnumerable<ValueTuple<ArmorSlot, DamageResistData>> slots, EnumDamageType damageType, int durabilityDamage, out int totalDurabilityDamage)
		{
			List<ValueTuple<ArmorSlot, DamageResistData>> slotList = Enumerable.ToList<ValueTuple<ArmorSlot, DamageResistData>>(slots);
			float totalProtection = Enumerable.Sum(Enumerable.Select<ValueTuple<ArmorSlot, DamageResistData>, float>(slotList, ([TupleElementNames(new string[]
			{
				"Slot",
				"Resists"
			})] ValueTuple<ArmorSlot, DamageResistData> slot) => PlayerDamageModelBehavior.GetProtection(slot.Item2, damageType)));
			if (totalProtection <= 3E-45f)
			{
				durabilityDamage /= slotList.Count;
				durabilityDamage = Math.Max(durabilityDamage, 1);
				foreach (ValueTuple<ArmorSlot, DamageResistData> valueTuple in slotList)
				{
					ArmorSlot slot3 = valueTuple.Item1;
					if (slot3.Itemstack.Item.GetMaxDurability(slot3.Itemstack) > 0)
					{
						slot3.Itemstack.Item.DamageItem(this.entity.Api.World, this.entity, slot3, durabilityDamage, true);
						slot3.MarkDirty();
					}
				}
				totalDurabilityDamage = durabilityDamage * slotList.Count;
				return;
			}
			totalDurabilityDamage = 0;
			foreach (ValueTuple<ArmorSlot, DamageResistData> valueTuple2 in slotList)
			{
				ArmorSlot slot2 = valueTuple2.Item1;
				DamageResistData resists = valueTuple2.Item2;
				if (slot2.Itemstack.Item.GetMaxDurability(slot2.Itemstack) > 0)
				{
					float protection = PlayerDamageModelBehavior.GetProtection(resists, damageType);
					if (protection > 0f)
					{
						int durabilityDamagePerSlot = (int)Math.Ceiling((double)((float)durabilityDamage * protection / totalProtection));
						totalDurabilityDamage += durabilityDamagePerSlot;
						slot2.Itemstack.Item.DamageItem(this.entity.Api.World, this.entity, slot2, durabilityDamagePerSlot, true);
						slot2.MarkDirty();
					}
				}
			}
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x000452EC File Offset: 0x000434EC
		private static float GetProtection(DamageResistData resists, EnumDamageType damageType)
		{
			float protection;
			if (!resists.Resists.TryGetValue(damageType, ref protection))
			{
				return 0f;
			}
			return protection;
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x00045310 File Offset: 0x00043510
		private void ApplyDamageFromHotClothes()
		{
			EntityPlayer entityPlayer = this.entity as EntityPlayer;
			ArmorInventory inventory = ((entityPlayer != null) ? entityPlayer.Player.InventoryManager.GetOwnInventory("character") : null) as ArmorInventory;
			if (inventory == null)
			{
				return;
			}
			float damage = 0f;
			foreach (ItemSlot slot2 in Enumerable.Where<ItemSlot>(inventory, delegate(ItemSlot slot)
			{
				ItemStack itemstack = slot.Itemstack;
				return ((itemstack != null) ? itemstack.Item : null) != null;
			}))
			{
				float temperature = slot2.Itemstack.Item.GetTemperature(this.entity.Api.World, slot2.Itemstack);
				if (temperature > 100f)
				{
					float temperatureFactor = (temperature - 100f) / 900f;
					float newDamage = 0.1f + 15.9f * temperatureFactor;
					if (newDamage > damage)
					{
						damage = newDamage;
					}
					if (newDamage > 6f)
					{
						this.entity.Api.World.SpawnItemEntity(slot2.TakeOutWhole(), this.entity.Pos.AsBlockPos, null);
					}
				}
			}
			if (damage > 0f)
			{
				DamageSource heatDamageSource = new DamageSource
				{
					CauseEntity = this.entity,
					SourceEntity = this.entity,
					IgnoreInvFrames = true,
					DamageTier = 0,
					Type = 12,
					KnockbackStrength = 0f,
					Source = 7
				};
				this.entity.ReceiveDamage(heatDamageSource, damage);
			}
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x000454A0 File Offset: 0x000436A0
		private bool GetAttackDirection(DamageSource damageSource, out DirectionOffset direction)
		{
			IDirectionalDamage directionalDamage = damageSource as IDirectionalDamage;
			if (directionalDamage != null)
			{
				direction = directionalDamage.Direction;
				return true;
			}
			if (damageSource.SourceEntity != null)
			{
				direction = DirectionOffset.GetDirectionWithRespectToCamera(this.entity, damageSource.SourceEntity);
				return true;
			}
			direction = default(DirectionOffset);
			return false;
		}

		// Token: 0x040006E8 RID: 1768
		public const string FullBodyDamageFactorStat = "playerDamageFactor";

		// Token: 0x040006E9 RID: 1769
		public const string FullBodyDamageTierReductionStat = "playerDamageTierReduction";

		// Token: 0x040006F9 RID: 1785
		private readonly EntityPlayer _player;

		// Token: 0x040006FA RID: 1786
		private readonly CombatOverhaulSystem _system;

		// Token: 0x040006FB RID: 1787
		private readonly Settings _settings;

		// Token: 0x040006FC RID: 1788
		[Nullable(2)]
		private CollidersEntityBehavior _colliders;

		// Token: 0x040006FD RID: 1789
		[Nullable(2)]
		private PlayerModelSkinSubscription _subscribedSkinBehavior;

		// Token: 0x040006FE RID: 1790
		[Nullable(2)]
		private EntityBehaviorHealth _subscribedHealthBehavior;

		// Token: 0x040006FF RID: 1791
		private readonly float _healthAfterSecondChance;

		// Token: 0x04000700 RID: 1792
		private PlayerDamageModelConfig _defaultConfig;

		// Token: 0x04000701 RID: 1793
		private bool _serverSide;

		// Token: 0x04000702 RID: 1794
		private const float _minTemperatureToDamage = 100f;

		// Token: 0x04000703 RID: 1795
		private const float _minTemperatureDamage = 0.1f;

		// Token: 0x04000704 RID: 1796
		private const float _maxTemperatureToDamage = 1000f;

		// Token: 0x04000705 RID: 1797
		private const float _maxTemperatureDamage = 16f;

		// Token: 0x04000706 RID: 1798
		private long _nextTemperatureDamageCheck;

		// Token: 0x04000707 RID: 1799
		private const long _temperatureDamageCheckCooldownMs = 1000L;
	}
}
