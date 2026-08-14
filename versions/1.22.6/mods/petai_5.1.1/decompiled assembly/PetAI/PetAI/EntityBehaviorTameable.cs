using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x02000015 RID: 21
	public class EntityBehaviorTameable : EntityBehavior
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00003FB0 File Offset: 0x000021B0
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00003FDA File Offset: 0x000021DA
		public DomesticationLevel DomesticationLevel
		{
			get
			{
				DomesticationLevel level;
				if (Enum.TryParse<DomesticationLevel>(this.DomesticationStatus.GetString("domesticationLevel", null), ref level))
				{
					return level;
				}
				return DomesticationLevel.WILD;
			}
			set
			{
				this.DomesticationStatus.SetString("domesticationLevel", value.ToString());
				this.entity.WatchedAttributes.MarkPathDirty("domesticationstatus");
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600005D RID: 93 RVA: 0x0000400E File Offset: 0x0000220E
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00004021 File Offset: 0x00002221
		public string OwnerId
		{
			get
			{
				return this.DomesticationStatus.GetString("owner", null);
			}
			set
			{
				this.DomesticationStatus.SetString("owner", value);
				this.entity.WatchedAttributes.MarkPathDirty("domesticationstatus");
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600005F RID: 95 RVA: 0x0000404C File Offset: 0x0000224C
		public IPlayer CachedOwner
		{
			get
			{
				IPlayer cachedOwner = this._cachedOwner;
				if (((cachedOwner != null) ? cachedOwner.PlayerUID : null) == this.OwnerId)
				{
					return this._cachedOwner;
				}
				if (string.IsNullOrEmpty(this.OwnerId))
				{
					return null;
				}
				this._cachedOwner = this.entity.World.PlayerByUid(this.OwnerId);
				return this._cachedOwner;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000060 RID: 96 RVA: 0x000040B0 File Offset: 0x000022B0
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00004117 File Offset: 0x00002317
		public float DomesticationProgress
		{
			get
			{
				float result;
				switch (this.DomesticationLevel)
				{
				case DomesticationLevel.WILD:
					result = 0f;
					break;
				case DomesticationLevel.TAMING:
					result = this.DomesticationStatus.GetFloat("progress", 0f);
					break;
				case DomesticationLevel.DOMESTICATED:
					result = 1f;
					break;
				default:
					result = this.DomesticationStatus.GetFloat("progress", 0f);
					break;
				}
				return result;
			}
			set
			{
				this.DomesticationStatus.SetFloat("progress", value);
				this.entity.WatchedAttributes.MarkPathDirty("domesticationstatus");
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00004140 File Offset: 0x00002340
		// (set) Token: 0x06000063 RID: 99 RVA: 0x000041BB File Offset: 0x000023BB
		public float Obedience
		{
			get
			{
				float result;
				switch (this.DomesticationLevel)
				{
				case DomesticationLevel.WILD:
					result = 0f;
					break;
				case DomesticationLevel.TAMING:
					result = 0f;
					break;
				case DomesticationLevel.DOMESTICATED:
					result = Math.Min(Math.Max(this.DomesticationStatus.GetFloat("obedience", 0f), 0f), 1f);
					break;
				default:
					result = this.DomesticationStatus.GetFloat("obedience", 0f);
					break;
				}
				return result;
			}
			set
			{
				this.DomesticationStatus.SetFloat("obedience", value);
				this.entity.WatchedAttributes.MarkPathDirty("domesticationstatus");
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000064 RID: 100 RVA: 0x000041E3 File Offset: 0x000023E3
		// (set) Token: 0x06000065 RID: 101 RVA: 0x000041F6 File Offset: 0x000023F6
		public bool MultiplyAllowed
		{
			get
			{
				return this.DomesticationStatus.GetBool("multiplyAllowed", true);
			}
			set
			{
				this.DomesticationStatus.SetBool("multiplyAllowed", value);
				this.entity.WatchedAttributes.MarkPathDirty("domesticationstatus");
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000066 RID: 102 RVA: 0x0000421E File Offset: 0x0000241E
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00004245 File Offset: 0x00002445
		private double Cooldown
		{
			get
			{
				return this.DomesticationStatus.GetDouble("cooldown", this.entity.World.Calendar.TotalHours);
			}
			set
			{
				this.DomesticationStatus.SetDouble("cooldown", value);
				this.entity.WatchedAttributes.MarkPathDirty("domesticationstatus");
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000068 RID: 104 RVA: 0x0000426D File Offset: 0x0000246D
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00004294 File Offset: 0x00002494
		private double DisobedienceTime
		{
			get
			{
				return this.DomesticationStatus.GetDouble("disobedienceTime", this.entity.World.Calendar.TotalHours);
			}
			set
			{
				this.DomesticationStatus.SetDouble("disobedienceTime", value);
				this.entity.WatchedAttributes.MarkPathDirty("domesticationstatus");
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000042BC File Offset: 0x000024BC
		// (set) Token: 0x0600006B RID: 107 RVA: 0x0000430F File Offset: 0x0000250F
		public ITreeAttribute DomesticationStatus
		{
			get
			{
				if (this.entity.WatchedAttributes.GetTreeAttribute("domesticationstatus") == null)
				{
					this.entity.WatchedAttributes.SetAttribute("domesticationstatus", new TreeAttribute());
				}
				return this.entity.WatchedAttributes.GetTreeAttribute("domesticationstatus");
			}
			set
			{
				this.entity.WatchedAttributes.SetAttribute("domesticationstatus", value);
				this.entity.WatchedAttributes.MarkPathDirty("domesticationstatus");
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600006C RID: 108 RVA: 0x0000433C File Offset: 0x0000253C
		private int Generation
		{
			get
			{
				return this.entity.WatchedAttributes.GetInt("generation", 0);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00004354 File Offset: 0x00002554
		// (set) Token: 0x0600006E RID: 110 RVA: 0x0000435C File Offset: 0x0000255C
		public EnumNestSize Size { get; set; }

		// Token: 0x0600006F RID: 111 RVA: 0x00004365 File Offset: 0x00002565
		public EntityBehaviorTameable(Entity entity) : base(entity)
		{
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000437C File Offset: 0x0000257C
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			JsonObject jsonObject = attributes["treat"];
			foreach (JsonObject jsonObject2 in ((jsonObject != null) ? jsonObject.AsArray() : null) ?? Array.Empty<JsonObject>())
			{
				string name = jsonObject2["code"].AsString(null);
				string domain = jsonObject2["domain"].AsString("game");
				float progress = jsonObject2["progress"].AsFloat(1f);
				long cooldown = (long)jsonObject2["cooldown"].AsInt(1);
				this.treatList.Add(new TamingItem
				{
					Name = name,
					Domain = domain,
					Progress = progress,
					Cooldown = cooldown
				});
			}
			if (!string.IsNullOrEmpty(attributes["tameEntityCode"].AsString(null)))
			{
				this.tameEntityCode = AssetLocation.Create(attributes["tameEntityCode"].AsString(null), "game");
			}
			EnumNestSize nestSize = EnumNestSize.SMALL;
			JsonObject jsonObject3 = attributes["size"];
			string text;
			if (jsonObject3 == null)
			{
				text = null;
			}
			else
			{
				string text2 = jsonObject3.AsString(null);
				text = ((text2 != null) ? text2.ToUpper() : null);
			}
			Enum.TryParse<EnumNestSize>(text, ref nestSize);
			this.Size = nestSize;
			this.disobediencePerDay = attributes["disobediencePerDay"].AsFloat(0f);
			this.entity.Api.Event.EnqueueMainThreadTask(delegate()
			{
				this.listenerId = this.entity.World.RegisterGameTickListener(new Action<float>(this.Disobey), 60000, 0);
			}, "register disobedience tick listener");
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000044F2 File Offset: 0x000026F2
		public override void AfterInitialized(bool onFirstSpawn)
		{
			EntityBehaviorHealth behavior = this.entity.GetBehavior<EntityBehaviorHealth>();
			if (behavior == null)
			{
				return;
			}
			behavior.SetMaxHealthModifiers("petconfig", PetConfig.Current.Difficulty.petMaxHpModifier);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004520 File Offset: 0x00002720
		public override void OnInteract(EntityAgent byEntity, ItemSlot itemslot, Vec3d hitPosition, EnumInteractMode mode, ref EnumHandling handled)
		{
			base.OnInteract(byEntity, itemslot, hitPosition, mode, ref handled);
			EntityPlayer player = byEntity as EntityPlayer;
			if (player == null)
			{
				return;
			}
			if (this.CachedOwner != null && this.CachedOwner.PlayerUID != player.PlayerUID)
			{
				return;
			}
			if (mode != 1)
			{
				return;
			}
			if (!this.entity.Alive)
			{
				this.TryReviveWith(itemslot);
				return;
			}
			if (byEntity.Controls.Sneak)
			{
				return;
			}
			if (this.DomesticationLevel == DomesticationLevel.WILD)
			{
				bool flag;
				if (itemslot == null)
				{
					flag = (null != null);
				}
				else
				{
					ItemStack itemstack = itemslot.Itemstack;
					flag = (((itemstack != null) ? itemstack.Collectible : null) != null);
				}
				if (flag)
				{
					if (!this.FeedEntityIfPossible(itemslot, player))
					{
						goto IL_1C1;
					}
					this.DomesticationLevel = DomesticationLevel.TAMING;
					this.OwnerId = player.PlayerUID;
					IServerPlayer serverPlayer = player.Player as IServerPlayer;
					if (serverPlayer == null)
					{
						goto IL_1C1;
					}
					serverPlayer.SendMessage(GlobalConstants.GeneralChatGroup, Lang.Get("petai:message-startet-taming", new object[]
					{
						this.entity.GetName(),
						Math.Round((double)(this.DomesticationProgress * 100f), 2)
					}), 4, null);
					goto IL_1C1;
				}
			}
			if (this.DomesticationLevel == DomesticationLevel.TAMING)
			{
				bool flag2;
				if (itemslot == null)
				{
					flag2 = (null != null);
				}
				else
				{
					ItemStack itemstack2 = itemslot.Itemstack;
					flag2 = (((itemstack2 != null) ? itemstack2.Collectible : null) != null);
				}
				if (flag2)
				{
					if (this.FeedEntityIfPossible(itemslot, player))
					{
						IServerPlayer serverPlayer2 = player.Player as IServerPlayer;
						if (serverPlayer2 != null)
						{
							serverPlayer2.SendMessage(GlobalConstants.GeneralChatGroup, Lang.Get("petai:message-tended-to", new object[]
							{
								this.entity.GetName(),
								Math.Round((double)(this.DomesticationProgress * 100f), 2)
							}), 4, null);
						}
					}
					if (this.DomesticationProgress >= 1f)
					{
						this.DomesticationLevel = DomesticationLevel.DOMESTICATED;
						this.SpawnTameVariant(1f);
						goto IL_1C1;
					}
					goto IL_1C1;
				}
			}
			if (this.DomesticationLevel == DomesticationLevel.DOMESTICATED)
			{
				this.FeedEntityIfPossible(itemslot, player);
			}
			IL_1C1:
			string text;
			if (itemslot == null)
			{
				text = null;
			}
			else
			{
				ItemStack itemstack3 = itemslot.Itemstack;
				if (itemstack3 == null)
				{
					text = null;
				}
				else
				{
					CollectibleObject collectible = itemstack3.Collectible;
					if (collectible == null)
					{
						text = null;
					}
					else
					{
						AssetLocation code = collectible.Code;
						text = ((code != null) ? code.Path : null);
					}
				}
			}
			if (text == "magicbone")
			{
				this.DomesticationLevel = DomesticationLevel.DOMESTICATED;
				this.Obedience = 1f;
				EntityPlayer entityPlayer = byEntity as EntityPlayer;
				this.OwnerId = ((entityPlayer != null) ? entityPlayer.PlayerUID : null);
				this.SpawnTameVariant(1f);
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000475F File Offset: 0x0000295F
		public override string PropertyName()
		{
			return "tameable";
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004768 File Offset: 0x00002968
		private void SpawnTameVariant(float dt)
		{
			Entity tameEntity;
			if (this.tameEntityCode != null && this.entity.Api.Side == 1)
			{
				EntityProperties tameType = this.entity.World.GetEntityType(this.tameEntityCode);
				if (tameType == null)
				{
					this.entity.World.Logger.Error("Misconfigured entity. Entity with code '{0}' is configured (via Tameable behavior) to be tamed into '{1}', but no such entity type was registered.", new object[]
					{
						this.entity.Code,
						this.tameEntityCode
					});
					return;
				}
				Cuboidf collisionBox = tameType.SpawnCollisionBox;
				if (this.entity.World.CollisionTester.IsColliding(this.entity.World.BlockAccessor, collisionBox, this.entity.Pos.XYZ, false))
				{
					this.callbackId = this.entity.World.RegisterCallback(new Action<float>(this.SpawnTameVariant), 1000);
					return;
				}
				tameEntity = this.entity.World.ClassRegistry.CreateEntity(tameType);
				tameEntity.Pos.SetFrom(this.entity.Pos);
				tameEntity.Pos.SetFrom(tameEntity.Pos);
				this.entity.Die(6, null);
				this.entity.World.SpawnEntity(tameEntity);
				if (tameEntity.HasBehavior<EntityBehaviorTameable>())
				{
					tameEntity.GetBehavior<EntityBehaviorTameable>().DomesticationStatus = this.DomesticationStatus;
				}
				if (this.entity.WatchedAttributes.HasAttribute("grow"))
				{
					tameEntity.WatchedAttributes.SetAttribute("grow", this.entity.WatchedAttributes.GetAttribute("grow"));
				}
				EntityBehaviorNameTag behavior = tameEntity.GetBehavior<EntityBehaviorNameTag>();
				if (behavior != null)
				{
					EntityBehaviorNameTag behavior2 = this.entity.GetBehavior<EntityBehaviorNameTag>();
					behavior.SetName((behavior2 != null) ? behavior2.DisplayName : null);
				}
				tameEntity.WatchedAttributes.SetInt("textureIndex", this.entity.WatchedAttributes.GetInt("textureIndex", 0));
			}
			else
			{
				tameEntity = this.entity;
			}
			PetProfileMessage message = new PetProfileMessage
			{
				targetEntityUID = tameEntity.EntityId,
				oldEntityUID = this.entity.EntityId
			};
			ICoreServerAPI coreServerAPI = this.entity.Api as ICoreServerAPI;
			if (coreServerAPI == null)
			{
				return;
			}
			IServerNetworkChannel channel = coreServerAPI.Network.GetChannel("petainetwork");
			PetProfileMessage petProfileMessage = message;
			IServerPlayer[] array = new IServerPlayer[1];
			int num = 0;
			EntityBehaviorTameable behavior3 = this.entity.GetBehavior<EntityBehaviorTameable>();
			array[num] = (((behavior3 != null) ? behavior3.CachedOwner : null) as IServerPlayer);
			channel.SendPacket<PetProfileMessage>(petProfileMessage, array);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000049D4 File Offset: 0x00002BD4
		private bool IsValidTamingItem(TamingItem item, ItemSlot slot)
		{
			if (!item.Name.EndsWith("*"))
			{
				ItemStack itemstack = slot.Itemstack;
				string text;
				if (itemstack == null)
				{
					text = null;
				}
				else
				{
					CollectibleObject collectible = itemstack.Collectible;
					text = ((collectible != null) ? collectible.Code.Path : null);
				}
				return text == item.Name;
			}
			ItemStack itemstack2 = slot.Itemstack;
			if (itemstack2 == null)
			{
				return false;
			}
			CollectibleObject collectible2 = itemstack2.Collectible;
			bool? flag;
			if (collectible2 == null)
			{
				flag = default(bool?);
			}
			else
			{
				string path = collectible2.Code.Path;
				string name = item.Name;
				flag = new bool?(path.StartsWith(name.Substring(0, name.Length - 1)));
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004A74 File Offset: 0x00002C74
		private bool CheckTamingSuccess(TamingItem tamingItem, ItemSlot itemSlot, EntityPlayer player)
		{
			if (tamingItem == null)
			{
				return false;
			}
			if (this.Cooldown > this.entity.World.Calendar.TotalHours)
			{
				IServerPlayer serverPlayer = player.Player as IServerPlayer;
				if (serverPlayer != null)
				{
					serverPlayer.SendMessage(GlobalConstants.GeneralChatGroup, Lang.Get("petai:message-not-ready", new object[]
					{
						this.entity.GetName()
					}), 4, null);
				}
				return false;
			}
			int acceptedItems = 0;
			EntityAgent entityAgent = this.entity as EntityAgent;
			if (entityAgent != null && entityAgent.LeftHandItemSlot is ItemSlotMouth)
			{
				acceptedItems += itemSlot.TryPutInto(this.entity.World, (this.entity as EntityAgent).LeftHandItemSlot, 1);
			}
			else
			{
				acceptedItems = 1;
				itemSlot.TakeOut(1);
			}
			if (acceptedItems < 1)
			{
				return false;
			}
			if (this.DomesticationLevel == DomesticationLevel.DOMESTICATED)
			{
				this.Obedience += tamingItem.Progress * PetConfig.Current.Difficulty.obedienceMultiplier * (float)Math.Pow((double)(1f + PetConfig.Current.Difficulty.obedienceMultiplierIncreasePerGen), (double)this.Generation);
			}
			else
			{
				this.DomesticationProgress += tamingItem.Progress * PetConfig.Current.Difficulty.tamingMultiplier * (float)Math.Pow((double)(1f + PetConfig.Current.Difficulty.tamingMultiplierIncreasePerGen), (double)this.Generation);
			}
			this.Cooldown = this.entity.World.Calendar.TotalHours + (double)tamingItem.Cooldown;
			if (!this.entity.HasBehavior<EntityBehaviorMouthInventory>())
			{
				ITreeAttribute tree = this.entity.WatchedAttributes.GetOrAddTreeAttribute("hunger");
				tree.SetFloat("saturation", tree.GetFloat("saturation", 0f) + 1f);
			}
			this.entity.WatchedAttributes.MarkPathDirty("hunger");
			return true;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004C4E File Offset: 0x00002E4E
		public override void OnEntityDespawn(EntityDespawnData despawn)
		{
			this.entity.World.UnregisterCallback(this.callbackId);
			this.entity.World.UnregisterGameTickListener(this.listenerId);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004C7C File Offset: 0x00002E7C
		private bool FeedEntityIfPossible(ItemSlot foodsource, EntityPlayer player)
		{
			TamingItem tamingItem = this.treatList.Find((TamingItem item) => this.IsValidTamingItem(item, foodsource));
			return this.CheckTamingSuccess(tamingItem, foodsource, player);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00004CC4 File Offset: 0x00002EC4
		private void Disobey(float intervall)
		{
			double hoursPassed = Math.Min(24.0, this.entity.World.Calendar.TotalHours - this.DisobedienceTime);
			IPlayer cachedOwner = this.CachedOwner;
			if (((cachedOwner != null) ? cachedOwner.Entity : null) != null)
			{
				this.Obedience -= PetConfig.Current.Difficulty.disobedienceMultiplier * this.disobediencePerDay * (float)(hoursPassed / 24.0) * (float)Math.Pow((double)(1f - PetConfig.Current.Difficulty.disobedienceMultiplierDecreasePerGen), (double)this.Generation);
			}
			this.DisobedienceTime = this.entity.World.Calendar.TotalHours;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00004D80 File Offset: 0x00002F80
		public override WorldInteraction[] GetInteractionHelp(IClientWorldAccessor world, EntitySelection es, IClientPlayer player, ref EnumHandling handled)
		{
			ItemStack[] treats = this.treatList.ConvertAll<AssetLocation>((TamingItem treat) => new AssetLocation(treat.Domain + ":" + treat.Name)).ConvertAll<CollectibleObject>((AssetLocation treat) => world.GetItem(treat) ?? world.GetBlock(treat)).FindAll((CollectibleObject treat) => treat != null).ConvertAll<ItemStack>((CollectibleObject treat) => new ItemStack(treat, 1)).ToArray();
			if (this.entity.Alive && treats.Length != 0 && (string.IsNullOrEmpty(this.OwnerId) || player.PlayerUID == this.OwnerId))
			{
				return new WorldInteraction[]
				{
					new WorldInteraction
					{
						ActionLangCode = "petai:interact-feed",
						MouseButton = 2,
						Itemstacks = treats
					}
				};
			}
			if (!this.entity.Alive)
			{
				EntityBehaviorHarvestable behavior = this.entity.GetBehavior<EntityBehaviorHarvestable>();
				if (behavior == null || !behavior.IsHarvested)
				{
					string[] resurrectors = PetConfig.Current.Resurrectors;
					if (resurrectors != null && resurrectors.Length != 0)
					{
						WorldInteraction[] array = new WorldInteraction[1];
						int num = 0;
						WorldInteraction worldInteraction = new WorldInteraction();
						worldInteraction.ActionLangCode = "petai:interact-revive";
						worldInteraction.MouseButton = 2;
						worldInteraction.Itemstacks = Enumerable.ToList<string>(PetConfig.Current.Resurrectors).ConvertAll<AssetLocation>((string resurrector) => new AssetLocation(resurrector)).ConvertAll<CollectibleObject>((AssetLocation resurrector) => world.GetItem(resurrector) ?? world.GetBlock(resurrector)).FindAll((CollectibleObject resurrector) => resurrector != null).ConvertAll<ItemStack>((CollectibleObject resurrector) => new ItemStack(resurrector, 1)).ToArray();
						array[num] = worldInteraction;
						return array;
					}
				}
			}
			return base.GetInteractionHelp(world, es, player, ref handled);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00004F90 File Offset: 0x00003190
		public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage)
		{
			EntityPlayer player = (damageSource.CauseEntity ?? damageSource.SourceEntity) as EntityPlayer;
			if ((player != null && ((player.PlayerUID == this.OwnerId && !PetConfig.Current.PetDamageableByOwner) || (player.PlayerUID != this.OwnerId && !PetConfig.Current.PvpOn && this.DomesticationLevel != DomesticationLevel.WILD))) || (damageSource.Source == 2 && PetConfig.Current.FalldamageOff))
			{
				damage = 0f;
				damageSource.CauseEntity = null;
				damageSource.SourceEntity = null;
			}
			EntityBehaviorHealth behaviorHealth = this.entity.GetBehavior<EntityBehaviorHealth>();
			if (behaviorHealth != null && behaviorHealth.Health < 0f)
			{
				behaviorHealth.Health = GameMath.Clamp(behaviorHealth.Health, 0f, behaviorHealth.MaxHealth);
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00005060 File Offset: 0x00003260
		public override void OnEntityDeath(DamageSource damageSourceForDeath)
		{
			base.OnEntityDeath(damageSourceForDeath);
			if (!string.IsNullOrEmpty(this.OwnerId))
			{
				ICoreServerAPI sapi = this.entity.Api as ICoreServerAPI;
				if (sapi != null)
				{
					ICoreServerAPI coreServerAPI = sapi;
					IPlayer cachedOwner = this.CachedOwner;
					int generalChatGroup = GlobalConstants.GeneralChatGroup;
					string text = "petai:message-pet-dead";
					object[] array = new object[1];
					int num = 0;
					EntityBehaviorNameTag behavior = this.entity.GetBehavior<EntityBehaviorNameTag>();
					array[num] = ((behavior != null) ? behavior.DisplayName : null);
					coreServerAPI.SendMessage(cachedOwner, generalChatGroup, Lang.Get(text, array), 4, null);
					WaypointMapLayer waypointMap = this.entity.Api.ModLoader.GetModSystem<WorldMapManager>(true).MapLayers.Find((MapLayer ml) => ml is WaypointMapLayer) as WaypointMapLayer;
					if (waypointMap != null)
					{
						WaypointMapLayer waypointMapLayer = waypointMap;
						Waypoint waypoint = new Waypoint();
						waypoint.Color = 9044739;
						waypoint.Icon = "gravestone";
						waypoint.Pinned = true;
						waypoint.Position = this.entity.Pos.XYZ;
						waypoint.OwningPlayerUid = this.OwnerId;
						Waypoint waypoint2 = waypoint;
						string text2 = "petai:message-pet-dead";
						object[] array2 = new object[1];
						int num2 = 0;
						EntityBehaviorNameTag behavior2 = this.entity.GetBehavior<EntityBehaviorNameTag>();
						array2[num2] = ((behavior2 != null) ? behavior2.DisplayName : null);
						waypoint2.Title = Lang.Get(text2, array2);
						waypointMapLayer.AddWaypoint(waypoint, this.CachedOwner as IServerPlayer);
					}
				}
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000051AC File Offset: 0x000033AC
		public override void GetInfoText(StringBuilder infotext)
		{
			if (this.CachedOwner == null)
			{
				return;
			}
			string text = "petai:gui-pet-owner";
			object[] array = new object[1];
			int num = 0;
			IPlayer cachedOwner = this.CachedOwner;
			array[num] = ((cachedOwner != null) ? cachedOwner.PlayerName : null);
			infotext.AppendLine(Lang.Get(text, array)).AppendLine((this.DomesticationLevel == DomesticationLevel.DOMESTICATED) ? Lang.Get("petai:gui-pet-obedience", new object[]
			{
				Math.Round((double)(this.Obedience * 100f), 2)
			}) : Lang.Get("petai:gui-pet-domesticationProgress", new object[]
			{
				Math.Round((double)(this.DomesticationProgress * 100f), 2)
			})).AppendLine(Lang.Get("petai:gui-pet-nestsize", new object[]
			{
				Lang.Get("petai:gui-pet-nestsize-" + this.Size.ToString().ToLower(), Array.Empty<object>())
			}));
			if (this.entity.HasBehavior<EntityBehaviorHealth>())
			{
				EntityBehaviorHealth beh = this.entity.GetBehavior<EntityBehaviorHealth>();
				infotext.AppendLine(Lang.Get("Health: {0}/{1}", new object[]
				{
					beh.Health,
					beh.MaxHealth
				}));
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000052E8 File Offset: 0x000034E8
		private void TryReviveWith(ItemSlot itemslot)
		{
			if (Enumerable.Any<string>(PetConfig.Current.Resurrectors, delegate(string resurrector)
			{
				string text = Enumerable.Last<string>(resurrector.Split(":", 0));
				ItemSlot itemslot2 = itemslot;
				string text2;
				if (itemslot2 == null)
				{
					text2 = null;
				}
				else
				{
					ItemStack itemstack = itemslot2.Itemstack;
					if (itemstack == null)
					{
						text2 = null;
					}
					else
					{
						CollectibleObject collectible = itemstack.Collectible;
						if (collectible == null)
						{
							text2 = null;
						}
						else
						{
							AssetLocation code = collectible.Code;
							text2 = ((code != null) ? code.Path : null);
						}
					}
				}
				return text == text2;
			}))
			{
				EntityBehaviorHarvestable behavior = this.entity.GetBehavior<EntityBehaviorHarvestable>();
				if (behavior == null || !behavior.IsHarvested)
				{
					this.entity.Revive();
					if (this.entity.HasBehavior<EntityBehaviorMortallyWoundable>())
					{
						this.entity.GetBehavior<EntityBehaviorMortallyWoundable>().HealthState = 0;
					}
					IAnimationManager animManager = this.entity.AnimManager;
					if (animManager != null)
					{
						EnumerableExtensions.Foreach<string>(animManager.ActiveAnimationsByAnimCode.Keys, new Action<string>(this.entity.AnimManager.StopAnimation));
					}
					itemslot.TakeOut(1);
					itemslot.MarkDirty();
				}
			}
		}

		// Token: 0x0400003B RID: 59
		private IPlayer _cachedOwner;

		// Token: 0x0400003D RID: 61
		public List<TamingItem> treatList = new List<TamingItem>();

		// Token: 0x0400003E RID: 62
		private AssetLocation tameEntityCode;

		// Token: 0x0400003F RID: 63
		private float disobediencePerDay;

		// Token: 0x04000040 RID: 64
		private long callbackId;

		// Token: 0x04000041 RID: 65
		private long listenerId;
	}
}
