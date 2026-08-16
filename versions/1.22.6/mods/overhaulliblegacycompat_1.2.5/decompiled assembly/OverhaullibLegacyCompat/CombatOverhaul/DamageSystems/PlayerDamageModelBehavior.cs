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
	// Token: 0x0200011C RID: 284
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class PlayerDamageModelBehavior : EntityBehavior
	{
		// Token: 0x06000DC9 RID: 3529 RVA: 0x0004276C File Offset: 0x0004096C
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
		// (add) Token: 0x06000DCA RID: 3530 RVA: 0x000429E4 File Offset: 0x00040BE4
		// (remove) Token: 0x06000DCB RID: 3531 RVA: 0x00042A1C File Offset: 0x00040C1C
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event OnPlayerReceiveDamageDelegate OnReceiveDamage;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x06000DCC RID: 3532 RVA: 0x00042A54 File Offset: 0x00040C54
		// (remove) Token: 0x06000DCD RID: 3533 RVA: 0x00042A8C File Offset: 0x00040C8C
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

		// Token: 0x06000DCE RID: 3534 RVA: 0x00042AC1 File Offset: 0x00040CC1
		public override string PropertyName()
		{
			return "PlayerDamageModel";
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000DCF RID: 3535 RVA: 0x00042AC8 File Offset: 0x00040CC8
		// (set) Token: 0x06000DD0 RID: 3536 RVA: 0x00042AD0 File Offset: 0x00040CD0
		public PlayerDamageModel DamageModel { get; private set; } = new PlayerDamageModel(Array.Empty<DamageZoneStatsJson>());

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000DD1 RID: 3537 RVA: 0x00042AD9 File Offset: 0x00040CD9
		// (set) Token: 0x06000DD2 RID: 3538 RVA: 0x00042AE1 File Offset: 0x00040CE1
		public Dictionary<string, PlayerBodyPart> CollidersToBodyParts { get; private set; }

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000DD3 RID: 3539 RVA: 0x00042AEA File Offset: 0x00040CEA
		// (set) Token: 0x06000DD4 RID: 3540 RVA: 0x00042AF2 File Offset: 0x00040CF2
		public Dictionary<PlayerBodyPart, DamageZone> BodyPartsToZones { get; private set; }

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000DD5 RID: 3541 RVA: 0x00042AFB File Offset: 0x00040CFB
		// (set) Token: 0x06000DD6 RID: 3542 RVA: 0x00042B03 File Offset: 0x00040D03
		public Dictionary<DamageZone, string> MultiplierStats { get; private set; }

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000DD7 RID: 3543 RVA: 0x00042B0C File Offset: 0x00040D0C
		// (set) Token: 0x06000DD8 RID: 3544 RVA: 0x00042B14 File Offset: 0x00040D14
		public List<EnumDamageType> DamageTypesToProcess { get; private set; }

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x00042B1D File Offset: 0x00040D1D
		// (set) Token: 0x06000DDA RID: 3546 RVA: 0x00042B25 File Offset: 0x00040D25
		public TimeSpan SecondChanceDefaultCooldown { get; set; }

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x00042B2E File Offset: 0x00040D2E
		public TimeSpan SecondChanceCooldown
		{
			get
			{
				return this.SecondChanceDefaultCooldown * (double)this.entity.Stats.GetBlended("secondChanceCooldown");
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x00042B51 File Offset: 0x00040D51
		// (set) Token: 0x06000DDD RID: 3549 RVA: 0x00042B59 File Offset: 0x00040D59
		public TimeSpan SecondChanceDefaultGracePeriod { get; set; }

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000DDE RID: 3550 RVA: 0x00042B62 File Offset: 0x00040D62
		public TimeSpan SecondChanceGracePeriod
		{
			get
			{
				return this.SecondChanceDefaultGracePeriod * (double)this.entity.Stats.GetBlended("secondChanceGracePeriod");
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000DDF RID: 3551 RVA: 0x00042B85 File Offset: 0x00040D85
		// (set) Token: 0x06000DE0 RID: 3552 RVA: 0x00042B8D File Offset: 0x00040D8D
		public bool SecondChanceAvailable { get; set; }

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000DE1 RID: 3553 RVA: 0x00042B96 File Offset: 0x00040D96
		// (set) Token: 0x06000DE2 RID: 3554 RVA: 0x00042B9E File Offset: 0x00040D9E
		[Nullable(2)]
		public DamageBlockStats CurrentDamageBlock { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000DE3 RID: 3555 RVA: 0x00042BA7 File Offset: 0x00040DA7
		// (set) Token: 0x06000DE4 RID: 3556 RVA: 0x00042BAF File Offset: 0x00040DAF
		[Nullable(2)]
		internal DamageBlockEventArgs LastDamageBlock { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x06000DE5 RID: 3557 RVA: 0x00042BB8 File Offset: 0x00040DB8
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

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00042BD7 File Offset: 0x00040DD7
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			this._serverSide = (this.entity.Api.Side == 1);
			this._defaultConfig = attributes.AsObject<PlayerDamageModelConfig>(null);
			this.ApplyConfig(this._defaultConfig);
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x00042C0C File Offset: 0x00040E0C
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

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00042C84 File Offset: 0x00040E84
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

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00042CD8 File Offset: 0x00040ED8
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

		// Token: 0x06000DEA RID: 3562 RVA: 0x00042DE8 File Offset: 0x00040FE8
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
				damage *= zoneMultiplier * damageModelBehavior.GetStatsMultiplier(zone);
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
					float totalWeight2 = Enumerable.Sum(Enumerable.Select<DamageZoneStats, float>(Enumerable.Where<DamageZoneStats>(damageModelBehavior.DamageModel.DamageZones, (DamageZoneStats value) => value.Directions.Check(direction)), (DamageZoneStats value) => value.Coverage * value.DamageMultiplier * damageModelBehavior.GetStatsMultiplier(damageModelBehavior.BodyPartsToZones[value.ZoneType])));
					if (totalWeight2 <= 0f)
					{
						return damage;
					}
					IEnumerable<PlayerBodyPart> bodyParts = Enumerable.Select<KeyValuePair<PlayerBodyPart, DamageZone>, PlayerBodyPart>(Enumerable.Where<KeyValuePair<PlayerBodyPart, DamageZone>>(damageModelBehavior.BodyPartsToZones, (KeyValuePair<PlayerBodyPart, DamageZone> entry) => entry.Value == zone), (KeyValuePair<PlayerBodyPart, DamageZone> entry) => entry.Key);
					float zoneWieght2 = Enumerable.Sum(Enumerable.Select<DamageZoneStats, float>(Enumerable.Where<DamageZoneStats>(Enumerable.Where<DamageZoneStats>(damageModelBehavior.DamageModel.DamageZones, (DamageZoneStats value) => Enumerable.Contains<PlayerBodyPart>(bodyParts, value.ZoneType)), (DamageZoneStats value) => value.Directions.Check(direction)), (DamageZoneStats value) => value.Coverage * value.DamageMultiplier * damageModelBehavior.GetStatsMultiplier(damageModelBehavior.BodyPartsToZones[value.ZoneType])));
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
				int durabilityDamage = 0;
				damageResistData.ApplyPlayerResist(new DamageData(damageType, damageTier, 0), ref damage, out durabilityDamage);
				return damage;
			}
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x000431C4 File Offset: 0x000413C4
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
			multiplier *= this.GetStatsMultiplier(damageZone);
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

		// Token: 0x06000DEC RID: 3564 RVA: 0x00043324 File Offset: 0x00041524
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

		// Token: 0x06000DED RID: 3565 RVA: 0x0004337C File Offset: 0x0004157C
		private float GetStatsMultiplier(DamageZone part)
		{
			string stat;
			if (this.MultiplierStats.TryGetValue(part, ref stat))
			{
				return this._player.Stats.GetBlended(stat);
			}
			return 1f;
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x000433B0 File Offset: 0x000415B0
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

		// Token: 0x06000DEF RID: 3567 RVA: 0x000434D0 File Offset: 0x000416D0
		private void ApplyBlock(DamageSource damageSource, PlayerBodyPart zone, ref float damage, out string damageLogMessage)
		{
			damageLogMessage = "";
			if (this.CurrentDamageBlock == null)
			{
				return;
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

		// Token: 0x06000DF0 RID: 3568 RVA: 0x00043840 File Offset: 0x00041A40
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

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00043A98 File Offset: 0x00041C98
		private void ApplyBlockResists(float blockTier, float damageTier, ref float damage)
		{
			float multiplier = damageTier > blockTier;
			damage *= multiplier;
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x00043AB4 File Offset: 0x00041CB4
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

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00043BE8 File Offset: 0x00041DE8
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

		// Token: 0x06000DF4 RID: 3572 RVA: 0x00043C90 File Offset: 0x00041E90
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

		// Token: 0x06000DF5 RID: 3573 RVA: 0x00043D38 File Offset: 0x00041F38
		private bool IsCausedByProjectile(DamageSource damageSource)
		{
			Entity sourceEntity = damageSource.SourceEntity;
			Entity causeEntity = damageSource.CauseEntity;
			return sourceEntity != null && causeEntity != null && sourceEntity != causeEntity;
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x00043D64 File Offset: 0x00041F64
		private void ApplyConfig(PlayerDamageModelConfig config)
		{
			this.DamageModel = new PlayerDamageModel(config.DamageModel.Zones);
			this.CollidersToBodyParts = config.BodyParts;
			this.SecondChanceDefaultCooldown = TimeSpan.FromSeconds((double)config.SecondChanceCooldownSec);
			this.SecondChanceAvailable = config.SecondChanceAvailable;
			this.SecondChanceDefaultGracePeriod = TimeSpan.FromSeconds((double)config.SecondChanceGracePeriodSec);
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x00043DC3 File Offset: 0x00041FC3
		private void SubscribeOnModelChange()
		{
			this._subscribedSkinBehavior = PlayerModelSkinSubscription.Subscribe(this.entity, new Action<string>(this.ReloadConfigForCustomModel), false);
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x00043DE4 File Offset: 0x00041FE4
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

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00043EA4 File Offset: 0x000420A4
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

		// Token: 0x06000DFA RID: 3578 RVA: 0x00044064 File Offset: 0x00042264
		private static float GetProtection(DamageResistData resists, EnumDamageType damageType)
		{
			float protection;
			if (!resists.Resists.TryGetValue(damageType, ref protection))
			{
				return 0f;
			}
			return protection;
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00044088 File Offset: 0x00042288
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

		// Token: 0x06000DFC RID: 3580 RVA: 0x00044218 File Offset: 0x00042418
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

		// Token: 0x040006D3 RID: 1747
		private readonly EntityPlayer _player;

		// Token: 0x040006D4 RID: 1748
		private readonly CombatOverhaulSystem _system;

		// Token: 0x040006D5 RID: 1749
		private readonly Settings _settings;

		// Token: 0x040006D6 RID: 1750
		[Nullable(2)]
		private CollidersEntityBehavior _colliders;

		// Token: 0x040006D7 RID: 1751
		[Nullable(2)]
		private PlayerModelSkinSubscription _subscribedSkinBehavior;

		// Token: 0x040006D8 RID: 1752
		[Nullable(2)]
		private EntityBehaviorHealth _subscribedHealthBehavior;

		// Token: 0x040006D9 RID: 1753
		private readonly float _healthAfterSecondChance;

		// Token: 0x040006DA RID: 1754
		private PlayerDamageModelConfig _defaultConfig;

		// Token: 0x040006DB RID: 1755
		private bool _serverSide;

		// Token: 0x040006DC RID: 1756
		private const float _minTemperatureToDamage = 100f;

		// Token: 0x040006DD RID: 1757
		private const float _minTemperatureDamage = 0.1f;

		// Token: 0x040006DE RID: 1758
		private const float _maxTemperatureToDamage = 1000f;

		// Token: 0x040006DF RID: 1759
		private const float _maxTemperatureDamage = 16f;

		// Token: 0x040006E0 RID: 1760
		private long _nextTemperatureDamageCheck;

		// Token: 0x040006E1 RID: 1761
		private const long _temperatureDamageCheckCooldownMs = 1000L;
	}
}
