using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.Common;

namespace CombatOverhaul.Armor
{
	// Token: 0x02000144 RID: 324
	[NullableContext(1)]
	[Nullable(0)]
	public class ToolBag : GearEquipableBag
	{
		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000FFB RID: 4091 RVA: 0x0004EAAD File Offset: 0x0004CCAD
		// (set) Token: 0x06000FFC RID: 4092 RVA: 0x0004EAB5 File Offset: 0x0004CCB5
		[Nullable(2)]
		public SlotConfig MainHandSlotConfig { [NullableContext(2)] get; [NullableContext(2)] protected set; }

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000FFD RID: 4093 RVA: 0x0004EABE File Offset: 0x0004CCBE
		// (set) Token: 0x06000FFE RID: 4094 RVA: 0x0004EAC6 File Offset: 0x0004CCC6
		[Nullable(2)]
		public SlotConfig OffHandSlotConfig { [NullableContext(2)] get; [NullableContext(2)] protected set; }

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000FFF RID: 4095 RVA: 0x0004EACF File Offset: 0x0004CCCF
		// (set) Token: 0x06001000 RID: 4096 RVA: 0x0004EAD7 File Offset: 0x0004CCD7
		[Nullable(2)]
		public string TakeOutSlotColor { [NullableContext(2)] get; [NullableContext(2)] protected set; }

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001001 RID: 4097 RVA: 0x0004EAE0 File Offset: 0x0004CCE0
		// (set) Token: 0x06001002 RID: 4098 RVA: 0x0004EAE8 File Offset: 0x0004CCE8
		public string HotkeyCode { get; protected set; } = "";

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001003 RID: 4099 RVA: 0x0004EAF1 File Offset: 0x0004CCF1
		// (set) Token: 0x06001004 RID: 4100 RVA: 0x0004EAF9 File Offset: 0x0004CCF9
		public string HotkeyName { get; protected set; } = "";

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001005 RID: 4101 RVA: 0x0004EB02 File Offset: 0x0004CD02
		// (set) Token: 0x06001006 RID: 4102 RVA: 0x0004EB0A File Offset: 0x0004CD0A
		public GlKeys HotKeyKey { get; protected set; } = 100;

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001007 RID: 4103 RVA: 0x0004EB13 File Offset: 0x0004CD13
		// (set) Token: 0x06001008 RID: 4104 RVA: 0x0004EB1B File Offset: 0x0004CD1B
		public List<SlotHotkeyConfig> HotkeyConfigs { get; protected set; } = new List<SlotHotkeyConfig>();

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001009 RID: 4105 RVA: 0x0004EB24 File Offset: 0x0004CD24
		// (set) Token: 0x0600100A RID: 4106 RVA: 0x0004EB2C File Offset: 0x0004CD2C
		public int RegularSlotsNumber { get; protected set; }

		// Token: 0x170003F6 RID: 1014
		// (get) Token: 0x0600100B RID: 4107 RVA: 0x0004EB35 File Offset: 0x0004CD35
		// (set) Token: 0x0600100C RID: 4108 RVA: 0x0004EB3D File Offset: 0x0004CD3D
		public int ToolSlotNumber { get; protected set; }

		// Token: 0x170003F7 RID: 1015
		// (get) Token: 0x0600100D RID: 4109 RVA: 0x0004EB46 File Offset: 0x0004CD46
		// (set) Token: 0x0600100E RID: 4110 RVA: 0x0004EB4E File Offset: 0x0004CD4E
		public int TakeOutSlotNumber { get; protected set; }

		// Token: 0x170003F8 RID: 1016
		// (get) Token: 0x0600100F RID: 4111 RVA: 0x0004EB57 File Offset: 0x0004CD57
		// (set) Token: 0x06001010 RID: 4112 RVA: 0x0004EB5F File Offset: 0x0004CD5F
		[Nullable(2)]
		public string TakeOutSlotIcon { [NullableContext(2)] get; [NullableContext(2)] protected set; }

		// Token: 0x06001011 RID: 4113 RVA: 0x0004EB68 File Offset: 0x0004CD68
		public ToolBag(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0004EBA8 File Offset: 0x0004CDA8
		public override void Initialize(JsonObject properties)
		{
			base.Initialize(properties);
			this.TakeOutSlotColor = properties["takeOutColor"].AsString(null);
			if (properties.KeyExists("hotkey"))
			{
				SlotHotkeyConfig hotkeyConfig = properties["hotkey"].AsObject<SlotHotkeyConfig>(null);
				this.HotkeyCode = hotkeyConfig.HotkeyCode;
				this.HotkeyName = hotkeyConfig.HotkeyName;
				this.HotKeyKey = hotkeyConfig.HotkeyKey;
				this.HotkeyConfigs.Add(hotkeyConfig);
			}
			else
			{
				this.HotkeyCode = properties["hotkeyCode"].AsString("");
				this.HotkeyName = properties["hotkeyName"].AsString("");
				this.HotKeyKey = Enum.Parse<GlKeys>(properties["hotkeyKey"].AsString("R"));
				if (this.HotkeyCode != "")
				{
					this.HotkeyConfigs.Add(new SlotHotkeyConfig
					{
						HotkeyCode = this.HotkeyCode,
						HotkeyName = this.HotkeyName,
						HotkeyKey = this.HotKeyKey
					});
				}
			}
			if (properties.KeyExists("hotkeys"))
			{
				SlotHotkeyConfig[] hotkeyConfigs = properties["hotkeys"].AsObject<SlotHotkeyConfig[]>(null) ?? Array.Empty<SlotHotkeyConfig>();
				this.HotkeyConfigs.AddRange(Enumerable.Where<SlotHotkeyConfig>(hotkeyConfigs, (SlotHotkeyConfig config) => config.HotkeyCode != ""));
				SlotHotkeyConfig firstConfig = Enumerable.FirstOrDefault<SlotHotkeyConfig>(this.HotkeyConfigs);
				if (firstConfig != null)
				{
					this.HotkeyCode = firstConfig.HotkeyCode;
					this.HotkeyName = firstConfig.HotkeyName;
					this.HotKeyKey = firstConfig.HotkeyKey;
				}
			}
			this.TakeOutSlotIcon = properties["takeOutSlotIcon"].AsString(null);
			JsonObject jsonObject = properties["toolSlot"];
			SlotConfigJson mainHandSlotConfigJson = (jsonObject != null) ? jsonObject.AsObject<SlotConfigJson>(null) : null;
			JsonObject jsonObject2 = properties["offhandToolSlot"];
			SlotConfigJson offHandSlotConfigJson = (jsonObject2 != null) ? jsonObject2.AsObject<SlotConfigJson>(null) : null;
			if (mainHandSlotConfigJson != null)
			{
				this.MainHandSlotConfig = mainHandSlotConfigJson.ToConfig();
			}
			if (offHandSlotConfigJson != null)
			{
				this.OffHandSlotConfig = offHandSlotConfigJson.ToConfig();
			}
			this.RegularSlotsNumber = base.SlotsNumber;
			if (mainHandSlotConfigJson != null)
			{
				this.ToolSlotNumber++;
			}
			if (offHandSlotConfigJson != null)
			{
				this.ToolSlotNumber++;
			}
			this.TakeOutSlotNumber = Math.Clamp(properties["takeOutSlotsNumber"].AsInt(this.ToolSlotNumber), 0, this.ToolSlotNumber);
			base.SlotsNumber += this.ToolSlotNumber + this.TakeOutSlotNumber;
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x0004EE30 File Offset: 0x0004D030
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this.Api = api;
			SlotConfig mainHandSlotConfig = this.MainHandSlotConfig;
			if (mainHandSlotConfig != null)
			{
				mainHandSlotConfig.Resolve(api);
			}
			SlotConfig offHandSlotConfig = this.OffHandSlotConfig;
			if (offHandSlotConfig != null)
			{
				offHandSlotConfig.Resolve(api);
			}
			ICoreClientAPI clientApi = api as ICoreClientAPI;
			if (clientApi == null)
			{
				return;
			}
			this.ClientApi = clientApi;
			using (List<SlotHotkeyConfig>.Enumerator enumerator = this.HotkeyConfigs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SlotHotkeyConfig hotkeyConfig = enumerator.Current;
					if (!(hotkeyConfig.HotkeyCode == ""))
					{
						HotKey hotkey;
						if (!clientApi.Input.HotKeys.TryGetValue(hotkeyConfig.HotkeyCode, ref hotkey))
						{
							clientApi.Input.RegisterHotKey(hotkeyConfig.HotkeyCode, hotkeyConfig.HotkeyName, hotkeyConfig.HotkeyKey, 4, false, false, false);
							hotkey = clientApi.Input.HotKeys[hotkeyConfig.HotkeyCode];
						}
						this.PreviousHotkeyHandlers[hotkeyConfig.HotkeyCode] = hotkey.Handler;
						clientApi.Input.SetHotKeyHandler(hotkeyConfig.HotkeyCode, (KeyCombination keyCombination) => this.OnHotkeyPressed(keyCombination, hotkeyConfig));
					}
				}
			}
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x0004EF9C File Offset: 0x0004D19C
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public override List<ItemSlotBagContent> GetOrCreateSlots(ItemStack bagstack, InventoryBase parentinv, int bagIndex, IWorldAccessor world)
		{
			List<ItemSlotBagContent> bagContents = new List<ItemSlotBagContent>();
			EnumItemStorageFlags flags = 189;
			ITreeAttribute stackBackPackTree = bagstack.Attributes.GetTreeAttribute("backpack");
			if (stackBackPackTree == null)
			{
				stackBackPackTree = new TreeAttribute();
				ITreeAttribute slotsTree = new TreeAttribute();
				int slotIndex = 0;
				if (this.MainHandSlotConfig != null)
				{
					ItemSlotBagContentWithWildcardMatch toolSlot = new ItemSlotBagContentWithWildcardMatch(parentinv, bagIndex, slotIndex, flags, bagstack, this.MainHandSlotConfig.SlotColor)
					{
						Config = this.MainHandSlotConfig
					};
					toolSlot.MainHand = true;
					bagContents.Add(toolSlot);
					slotsTree["slot-" + slotIndex.ToString()] = new ItemstackAttribute(null);
					if (this.MainHandSlotConfig.SlotsIcon != null)
					{
						toolSlot.BackgroundIcon = this.MainHandSlotConfig.SlotsIcon;
					}
					slotIndex++;
				}
				if (this.OffHandSlotConfig != null)
				{
					ItemSlotBagContentWithWildcardMatch toolSlot2 = new ItemSlotBagContentWithWildcardMatch(parentinv, bagIndex, slotIndex, flags, bagstack, this.OffHandSlotConfig.SlotColor)
					{
						Config = this.OffHandSlotConfig
					};
					toolSlot2.MainHand = false;
					bagContents.Add(toolSlot2);
					slotsTree["slot-" + slotIndex.ToString()] = new ItemstackAttribute(null);
					if (this.OffHandSlotConfig.SlotsIcon != null)
					{
						toolSlot2.BackgroundIcon = this.OffHandSlotConfig.SlotsIcon;
					}
					slotIndex++;
				}
				while (slotIndex < base.DefaultSlotConfig.SlotsNumber + this.ToolSlotNumber)
				{
					ItemSlotBagContentWithWildcardMatch slot = new ItemSlotBagContentWithWildcardMatch(parentinv, bagIndex, slotIndex, flags, bagstack, base.DefaultSlotConfig.SlotColor)
					{
						Config = base.DefaultSlotConfig
					};
					bagContents.Add(slot);
					slotsTree["slot-" + slotIndex.ToString()] = new ItemstackAttribute(null);
					if (base.DefaultSlotConfig.SlotsIcon != null)
					{
						slot.BackgroundIcon = base.DefaultSlotConfig.SlotsIcon;
					}
					slotIndex++;
				}
				foreach (SlotConfig config in base.SlotConfigs)
				{
					int lastIndex = slotIndex + config.SlotsNumber;
					while (slotIndex < lastIndex)
					{
						ItemSlotBagContentWithWildcardMatch slot2 = new ItemSlotBagContentWithWildcardMatch(parentinv, bagIndex, slotIndex, flags, bagstack, config.SlotColor)
						{
							Config = config
						};
						bagContents.Add(slot2);
						slotsTree["slot-" + slotIndex.ToString()] = new ItemstackAttribute(null);
						if (config.SlotsIcon != null)
						{
							slot2.BackgroundIcon = config.SlotsIcon;
						}
						slotIndex++;
					}
				}
				if (this.HasMainHandTakeOutSlot)
				{
					ItemSlotTakeOutOnly takeOutSLot = new ItemSlotTakeOutOnly(parentinv, bagIndex, slotIndex, flags, bagstack, this.TakeOutSlotColor);
					takeOutSLot.MainHand = true;
					bagContents.Add(takeOutSLot);
					slotsTree["slot-" + slotIndex.ToString()] = new ItemstackAttribute(null);
					if (this.TakeOutSlotIcon != null)
					{
						takeOutSLot.BackgroundIcon = this.TakeOutSlotIcon;
					}
					slotIndex++;
				}
				if (this.HasOffHandTakeOutSlot)
				{
					ItemSlotTakeOutOnly takeOutSLot2 = new ItemSlotTakeOutOnly(parentinv, bagIndex, slotIndex, flags, bagstack, this.TakeOutSlotColor);
					takeOutSLot2.MainHand = false;
					bagContents.Add(takeOutSLot2);
					slotsTree["slot-" + slotIndex.ToString()] = new ItemstackAttribute(null);
					if (this.TakeOutSlotIcon != null)
					{
						takeOutSLot2.BackgroundIcon = this.TakeOutSlotIcon;
					}
				}
				stackBackPackTree["slots"] = slotsTree;
				bagstack.Attributes["backpack"] = stackBackPackTree;
			}
			else
			{
				foreach (KeyValuePair<string, IAttribute> val in stackBackPackTree.GetTreeAttribute("slots").SortedCopy(false))
				{
					int slotIndex2 = int.Parse(val.Key.Split("-", 0)[1]);
					if (slotIndex2 == 0)
					{
						if (this.MainHandSlotConfig != null)
						{
							ItemSlotBagContentWithWildcardMatch slot3 = new ItemSlotBagContentWithWildcardMatch(parentinv, bagIndex, slotIndex2, flags, bagstack, this.MainHandSlotConfig.SlotColor)
							{
								Config = this.MainHandSlotConfig
							};
							slot3.MainHand = true;
							IAttribute value = val.Value;
							if (((value != null) ? value.GetValue() : null) != null)
							{
								ItemstackAttribute attr = (ItemstackAttribute)val.Value;
								slot3.Itemstack = attr.value;
								slot3.Itemstack.ResolveBlockOrItem(world);
							}
							if (this.MainHandSlotConfig.SlotsIcon != null)
							{
								slot3.BackgroundIcon = this.MainHandSlotConfig.SlotsIcon;
							}
							while (bagContents.Count <= slotIndex2)
							{
								bagContents.Add(null);
							}
							bagContents[slotIndex2] = slot3;
						}
						else if (this.OffHandSlotConfig != null)
						{
							ItemSlotBagContentWithWildcardMatch slot4 = new ItemSlotBagContentWithWildcardMatch(parentinv, bagIndex, slotIndex2, flags, bagstack, this.OffHandSlotConfig.SlotColor)
							{
								Config = this.OffHandSlotConfig
							};
							slot4.MainHand = false;
							IAttribute value2 = val.Value;
							if (((value2 != null) ? value2.GetValue() : null) != null)
							{
								ItemstackAttribute attr2 = (ItemstackAttribute)val.Value;
								slot4.Itemstack = attr2.value;
								slot4.Itemstack.ResolveBlockOrItem(world);
							}
							if (this.OffHandSlotConfig.SlotsIcon != null)
							{
								slot4.BackgroundIcon = this.OffHandSlotConfig.SlotsIcon;
							}
							while (bagContents.Count <= slotIndex2)
							{
								bagContents.Add(null);
							}
							bagContents[slotIndex2] = slot4;
						}
						else
						{
							SlotConfig config2 = this.GetSlotConfig(slotIndex2 - this.ToolSlotNumber);
							ItemSlotBagContentWithWildcardMatch slot5 = new ItemSlotBagContentWithWildcardMatch(parentinv, bagIndex, slotIndex2, flags, bagstack, config2.SlotColor)
							{
								Config = config2
							};
							if (config2.SlotsIcon != null)
							{
								slot5.BackgroundIcon = config2.SlotsIcon;
							}
							IAttribute value3 = val.Value;
							if (((value3 != null) ? value3.GetValue() : null) != null)
							{
								ItemstackAttribute attr3 = (ItemstackAttribute)val.Value;
								slot5.Itemstack = attr3.value;
								slot5.Itemstack.ResolveBlockOrItem(world);
							}
							while (bagContents.Count <= slotIndex2)
							{
								bagContents.Add(null);
							}
							bagContents[slotIndex2] = slot5;
						}
					}
					else if (slotIndex2 == 1 && this.ToolSlotNumber == 2 && this.OffHandSlotConfig != null)
					{
						ItemSlotBagContentWithWildcardMatch slot6 = new ItemSlotBagContentWithWildcardMatch(parentinv, bagIndex, slotIndex2, flags, bagstack, this.OffHandSlotConfig.SlotColor)
						{
							Config = this.OffHandSlotConfig
						};
						slot6.MainHand = false;
						IAttribute value4 = val.Value;
						if (((value4 != null) ? value4.GetValue() : null) != null)
						{
							ItemstackAttribute attr4 = (ItemstackAttribute)val.Value;
							slot6.Itemstack = attr4.value;
							slot6.Itemstack.ResolveBlockOrItem(world);
						}
						if (this.OffHandSlotConfig.SlotsIcon != null)
						{
							slot6.BackgroundIcon = this.OffHandSlotConfig.SlotsIcon;
						}
						while (bagContents.Count <= slotIndex2)
						{
							bagContents.Add(null);
						}
						bagContents[slotIndex2] = slot6;
					}
					else if (slotIndex2 == this.MainHandTakeOutSlotIndex && this.HasMainHandTakeOutSlot)
					{
						ItemSlotTakeOutOnly takeOutSLot3 = new ItemSlotTakeOutOnly(parentinv, bagIndex, slotIndex2, flags, bagstack, this.TakeOutSlotColor);
						takeOutSLot3.MainHand = true;
						IAttribute value5 = val.Value;
						if (((value5 != null) ? value5.GetValue() : null) != null)
						{
							ItemstackAttribute attr5 = (ItemstackAttribute)val.Value;
							takeOutSLot3.Itemstack = attr5.value;
							takeOutSLot3.Itemstack.ResolveBlockOrItem(world);
						}
						if (this.TakeOutSlotIcon != null)
						{
							takeOutSLot3.BackgroundIcon = this.TakeOutSlotIcon;
						}
						while (bagContents.Count <= slotIndex2)
						{
							bagContents.Add(null);
						}
						bagContents[slotIndex2] = takeOutSLot3;
					}
					else if (slotIndex2 == this.OffHandTakeOutSlotIndex && this.HasOffHandTakeOutSlot)
					{
						ItemSlotTakeOutOnly takeOutSLot4 = new ItemSlotTakeOutOnly(parentinv, bagIndex, slotIndex2, flags, bagstack, this.TakeOutSlotColor);
						takeOutSLot4.MainHand = false;
						IAttribute value6 = val.Value;
						if (((value6 != null) ? value6.GetValue() : null) != null)
						{
							ItemstackAttribute attr6 = (ItemstackAttribute)val.Value;
							takeOutSLot4.Itemstack = attr6.value;
							takeOutSLot4.Itemstack.ResolveBlockOrItem(world);
						}
						if (this.TakeOutSlotIcon != null)
						{
							takeOutSLot4.BackgroundIcon = this.TakeOutSlotIcon;
						}
						while (bagContents.Count <= slotIndex2)
						{
							bagContents.Add(null);
						}
						bagContents[slotIndex2] = takeOutSLot4;
					}
					else if (!this.IsSurplusTakeOutSlotIndex(slotIndex2))
					{
						SlotConfig config3 = this.GetSlotConfig(slotIndex2 - this.ToolSlotNumber);
						ItemSlotBagContentWithWildcardMatch slot7 = new ItemSlotBagContentWithWildcardMatch(parentinv, bagIndex, slotIndex2, flags, bagstack, config3.SlotColor)
						{
							Config = config3
						};
						if (config3.SlotsIcon != null)
						{
							slot7.BackgroundIcon = config3.SlotsIcon;
						}
						IAttribute value7 = val.Value;
						if (((value7 != null) ? value7.GetValue() : null) != null)
						{
							ItemstackAttribute attr7 = (ItemstackAttribute)val.Value;
							slot7.Itemstack = attr7.value;
							slot7.Itemstack.ResolveBlockOrItem(world);
						}
						while (bagContents.Count <= slotIndex2)
						{
							bagContents.Add(null);
						}
						bagContents[slotIndex2] = slot7;
					}
				}
			}
			return bagContents;
		}

		// Token: 0x170003F9 RID: 1017
		// (get) Token: 0x06001015 RID: 4117 RVA: 0x0004F840 File Offset: 0x0004DA40
		protected bool HasMainHandTakeOutSlot
		{
			get
			{
				return this.MainHandSlotConfig != null && this.TakeOutSlotNumber > 0;
			}
		}

		// Token: 0x170003FA RID: 1018
		// (get) Token: 0x06001016 RID: 4118 RVA: 0x0004F855 File Offset: 0x0004DA55
		protected bool HasOffHandTakeOutSlot
		{
			get
			{
				return this.OffHandSlotConfig != null && this.TakeOutSlotNumber > ((this.MainHandSlotConfig != null) ? 1 : 0);
			}
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x06001017 RID: 4119 RVA: 0x0004F872 File Offset: 0x0004DA72
		protected int MainHandTakeOutSlotIndex
		{
			get
			{
				return this.RegularSlotsNumber + this.ToolSlotNumber;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x06001018 RID: 4120 RVA: 0x0004F881 File Offset: 0x0004DA81
		protected int OffHandTakeOutSlotIndex
		{
			get
			{
				return this.MainHandTakeOutSlotIndex + ((this.HasMainHandTakeOutSlot > false) ? 1 : 0);
			}
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0004F894 File Offset: 0x0004DA94
		protected bool IsSurplusTakeOutSlotIndex(int slotIndex)
		{
			int takeOutStartIndex = this.RegularSlotsNumber + this.ToolSlotNumber;
			return slotIndex >= takeOutStartIndex + this.TakeOutSlotNumber && slotIndex < takeOutStartIndex + this.ToolSlotNumber;
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0004F8C8 File Offset: 0x0004DAC8
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
			if (this.HotkeyName != "")
			{
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
				appendInterpolatedStringHandler..ctor(15, 1, dsc);
				appendInterpolatedStringHandler.AppendLiteral("Uses hotkey: '");
				appendInterpolatedStringHandler.AppendFormatted(this.HotkeyName);
				appendInterpolatedStringHandler.AppendLiteral("'");
				dsc.AppendLine(ref appendInterpolatedStringHandler);
			}
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x0600101B RID: 4123 RVA: 0x0004F92D File Offset: 0x0004DB2D
		[Nullable(new byte[]
		{
			1,
			1,
			2,
			1
		})]
		protected Dictionary<string, ActionConsumable<KeyCombination>> PreviousHotkeyHandlers { [return: Nullable(new byte[]
		{
			1,
			1,
			2,
			1
		})] get; } = new Dictionary<string, ActionConsumable<KeyCombination>>();

		// Token: 0x0600101C RID: 4124 RVA: 0x0004F935 File Offset: 0x0004DB35
		protected virtual bool OnHotkeyPressed(KeyCombination keyCombination)
		{
			return this.OnHotkeyPressed(keyCombination, null);
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0004F940 File Offset: 0x0004DB40
		protected virtual bool OnHotkeyPressed(KeyCombination keyCombination, [Nullable(2)] SlotHotkeyConfig hotkeyConfig)
		{
			InventoryPlayerBackpacks inventory = this.GetBackpackInventory();
			bool handled = false;
			if (inventory != null && this.Api != null && this.HotkeyCooldownUntilMs < this.Api.World.ElapsedMilliseconds)
			{
				ICoreClientAPI clientApi = this.ClientApi;
				ToolBagSystemClient toolBagSystemClient;
				if (clientApi == null)
				{
					toolBagSystemClient = null;
				}
				else
				{
					IModLoader modLoader = clientApi.ModLoader;
					if (modLoader == null)
					{
						toolBagSystemClient = null;
					}
					else
					{
						CombatOverhaulSystem modSystem = modLoader.GetModSystem<CombatOverhaulSystem>(true);
						toolBagSystemClient = ((modSystem != null) ? modSystem.ClientToolBagSystem : null);
					}
				}
				ToolBagSystemClient system = toolBagSystemClient;
				if (system != null)
				{
					ItemSlotBagContentWithWildcardMatch selectedSlot = null;
					ToolBag.HotkeyStoragePriority storagePriority = this.GetSameHotkeyStoragePriority(inventory, keyCombination);
					if (storagePriority.TargetSlot != null)
					{
						selectedSlot = storagePriority.TargetSlot;
					}
					else if (storagePriority.HeldItemCanBeStored)
					{
						handled = true;
					}
					else
					{
						List<ItemSlotBagContentWithWildcardMatch> slots = Enumerable.ToList<ItemSlotBagContentWithWildcardMatch>(Enumerable.Where<ItemSlotBagContentWithWildcardMatch>(Enumerable.Where<ItemSlotBagContentWithWildcardMatch>(Enumerable.OfType<ItemSlotBagContentWithWildcardMatch>(inventory), (ItemSlotBagContentWithWildcardMatch slot) => slot.Config.HandleHotkey), (ItemSlotBagContentWithWildcardMatch slot) => this.SlotUsesKeyCombination(slot, keyCombination)));
						ItemSlotBagContentWithWildcardMatch itemSlotBagContentWithWildcardMatch;
						if ((itemSlotBagContentWithWildcardMatch = Enumerable.FirstOrDefault<ItemSlotBagContentWithWildcardMatch>(slots, (ItemSlotBagContentWithWildcardMatch slot) => !ToolBag.HasHotkeyActionableStack(this.GetHandSlot(slot)) && ToolBag.HasHotkeyActionableStack(slot))) == null)
						{
							IEnumerable<ItemSlotBagContentWithWildcardMatch> enumerable = slots;
							Func<ItemSlotBagContentWithWildcardMatch, bool> func;
							if ((func = ToolBag.<>O.<0>__HasHotkeyActionableStack) == null)
							{
								func = (ToolBag.<>O.<0>__HasHotkeyActionableStack = new Func<ItemSlotBagContentWithWildcardMatch, bool>(ToolBag.HasHotkeyActionableStack));
							}
							itemSlotBagContentWithWildcardMatch = Enumerable.FirstOrDefault<ItemSlotBagContentWithWildcardMatch>(enumerable, func);
						}
						selectedSlot = itemSlotBagContentWithWildcardMatch;
					}
					if (selectedSlot != null)
					{
						system.Send(selectedSlot.ToolBagId, selectedSlot.ToolBagIndex, selectedSlot.MainHand, selectedSlot.SlotIndex);
						handled = true;
					}
					if (handled)
					{
						this.HotkeyCooldownUntilMs = this.Api.World.ElapsedMilliseconds + 120L;
					}
				}
			}
			if (handled)
			{
				return true;
			}
			ActionConsumable<KeyCombination> previousHandler = this.PreviousHotkeyHandler;
			if (hotkeyConfig != null)
			{
				this.PreviousHotkeyHandlers.TryGetValue(hotkeyConfig.HotkeyCode, ref previousHandler);
			}
			return previousHandler != null && previousHandler.Invoke(keyCombination);
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0004FAF8 File Offset: 0x0004DCF8
		protected virtual ToolBag.HotkeyStoragePriority GetSameHotkeyStoragePriority(InventoryPlayerBackpacks inventory, KeyCombination keyCombination)
		{
			bool heldItemCanBeStored = false;
			foreach (ItemSlotBagContentWithWildcardMatch slot in Enumerable.OfType<ItemSlotBagContentWithWildcardMatch>(inventory))
			{
				if (slot.Config.HandleHotkey && this.SlotUsesKeyCombination(slot, keyCombination))
				{
					ItemSlot handSlot = this.GetHandSlot(slot);
					if (ToolBag.HasHotkeyActionableStack(handSlot) && handSlot != null && slot.CanHold(handSlot))
					{
						heldItemCanBeStored = true;
						if (!ToolBag.HasHotkeyActionableStack(slot))
						{
							return new ToolBag.HotkeyStoragePriority(true, slot);
						}
					}
				}
			}
			return new ToolBag.HotkeyStoragePriority(heldItemCanBeStored, null);
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x0004FB94 File Offset: 0x0004DD94
		protected virtual bool SlotUsesKeyCombination(ItemSlotBagContentWithWildcardMatch slot, KeyCombination keyCombination)
		{
			ItemStack sourceBag = slot.SourceBag;
			bool flag;
			if (sourceBag == null)
			{
				flag = (null != null);
			}
			else
			{
				CollectibleObject collectible = sourceBag.Collectible;
				flag = (((collectible != null) ? collectible.CollectibleBehaviors : null) != null);
			}
			if (!flag)
			{
				return false;
			}
			foreach (ToolBag behavior in Enumerable.OfType<ToolBag>(slot.SourceBag.Collectible.CollectibleBehaviors))
			{
				foreach (SlotHotkeyConfig config in behavior.HotkeyConfigs)
				{
					if (behavior.MatchesHotkeyConfig(slot.SourceBag, config) && this.HotkeyConfigMatchesCurrentMapping(config, keyCombination))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0004FC68 File Offset: 0x0004DE68
		protected virtual bool HotkeyConfigMatchesCurrentMapping(SlotHotkeyConfig hotkeyConfig, KeyCombination keyCombination)
		{
			if (!(hotkeyConfig.HotkeyCode == ""))
			{
				ICoreClientAPI clientApi = this.ClientApi;
				HotKey hotkey;
				if (clientApi != null && clientApi.Input.HotKeys.TryGetValue(hotkeyConfig.HotkeyCode, ref hotkey))
				{
					return ToolBag.KeyCombinationsMatch(hotkey.CurrentMapping, keyCombination);
				}
			}
			return false;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x0004FCC0 File Offset: 0x0004DEC0
		protected static bool KeyCombinationsMatch([Nullable(2)] KeyCombination currentMapping, KeyCombination pressedCombination)
		{
			return currentMapping != null && (currentMapping.KeyCode == pressedCombination.KeyCode && currentMapping.SecondKeyCode.GetValueOrDefault() == pressedCombination.SecondKeyCode.GetValueOrDefault() && currentMapping.Ctrl == pressedCombination.Ctrl && currentMapping.Alt == pressedCombination.Alt) && currentMapping.Shift == pressedCombination.Shift;
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x0004FD24 File Offset: 0x0004DF24
		[return: Nullable(2)]
		protected ItemSlot GetHandSlot(ItemSlotBagContentWithWildcardMatch slot)
		{
			if (!slot.MainHand)
			{
				ICoreClientAPI clientApi = this.ClientApi;
				if (clientApi == null)
				{
					return null;
				}
				IClientWorldAccessor world = clientApi.World;
				if (world == null)
				{
					return null;
				}
				IClientPlayer player = world.Player;
				if (player == null)
				{
					return null;
				}
				EntityPlayer entity = player.Entity;
				if (entity == null)
				{
					return null;
				}
				return entity.LeftHandItemSlot;
			}
			else
			{
				ICoreClientAPI clientApi2 = this.ClientApi;
				if (clientApi2 == null)
				{
					return null;
				}
				IClientWorldAccessor world2 = clientApi2.World;
				if (world2 == null)
				{
					return null;
				}
				IClientPlayer player2 = world2.Player;
				if (player2 == null)
				{
					return null;
				}
				EntityPlayer entity2 = player2.Entity;
				if (entity2 == null)
				{
					return null;
				}
				return entity2.RightHandItemSlot;
			}
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x0004FD9E File Offset: 0x0004DF9E
		[NullableContext(2)]
		protected static bool HasHotkeyActionableStack(ItemSlot slot)
		{
			return ((slot != null) ? slot.Itemstack : null) != null && slot.Itemstack.StackSize > 0 && slot.Itemstack.Collectible != null;
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x0004FDCC File Offset: 0x0004DFCC
		protected virtual bool MatchesHotkeyConfig(ItemStack sourceBag, SlotHotkeyConfig hotkeyConfig)
		{
			if (hotkeyConfig.RequiredTypeCode == "" || hotkeyConfig.RequiredTypeValue == "")
			{
				return true;
			}
			ITreeAttribute attributes = sourceBag.Attributes;
			ITreeAttribute types = ((attributes != null) ? attributes["types"] : null) as ITreeAttribute;
			if (types == null)
			{
				return true;
			}
			IAttribute attribute = types[hotkeyConfig.RequiredTypeCode];
			string text;
			if (attribute == null)
			{
				text = null;
			}
			else
			{
				object value = attribute.GetValue();
				text = ((value != null) ? value.ToString() : null);
			}
			string actualValue = text;
			return actualValue == null || actualValue == hotkeyConfig.RequiredTypeValue;
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x0004FE57 File Offset: 0x0004E057
		[NullableContext(2)]
		protected InventoryPlayerBackpacks GetBackpackInventory()
		{
			ICoreClientAPI clientApi = this.ClientApi;
			object obj;
			if (clientApi == null)
			{
				obj = null;
			}
			else
			{
				IClientWorldAccessor world = clientApi.World;
				if (world == null)
				{
					obj = null;
				}
				else
				{
					IClientPlayer player = world.Player;
					obj = ((player != null) ? player.InventoryManager.GetOwnInventory("backpack") : null);
				}
			}
			return obj as InventoryPlayerBackpacks;
		}

		// Token: 0x04000779 RID: 1913
		[Nullable(new byte[]
		{
			2,
			1
		})]
		protected ActionConsumable<KeyCombination> PreviousHotkeyHandler;

		// Token: 0x0400077B RID: 1915
		[Nullable(2)]
		protected ICoreClientAPI ClientApi;

		// Token: 0x0400077C RID: 1916
		protected long HotkeyCooldownUntilMs;

		// Token: 0x0400077D RID: 1917
		protected const long HotkeyCooldown = 120L;

		// Token: 0x02000294 RID: 660
		[NullableContext(2)]
		[Nullable(0)]
		protected readonly struct HotkeyStoragePriority
		{
			// Token: 0x060017A6 RID: 6054 RVA: 0x0006BC70 File Offset: 0x00069E70
			public HotkeyStoragePriority(bool heldItemCanBeStored, ItemSlotBagContentWithWildcardMatch targetSlot)
			{
				this.HeldItemCanBeStored = heldItemCanBeStored;
				this.TargetSlot = targetSlot;
			}

			// Token: 0x04000D5A RID: 3418
			public readonly bool HeldItemCanBeStored;

			// Token: 0x04000D5B RID: 3419
			public readonly ItemSlotBagContentWithWildcardMatch TargetSlot;
		}

		// Token: 0x02000295 RID: 661
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000D5C RID: 3420
			[Nullable(0)]
			public static Func<ItemSlotBagContentWithWildcardMatch, bool> <0>__HasHotkeyActionableStack;
		}
	}
}
