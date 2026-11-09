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
	// Token: 0x02000150 RID: 336
	[NullableContext(1)]
	[Nullable(0)]
	public class ToolBag : GearEquipableBag
	{
		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x0600106F RID: 4207 RVA: 0x000503B5 File Offset: 0x0004E5B5
		// (set) Token: 0x06001070 RID: 4208 RVA: 0x000503BD File Offset: 0x0004E5BD
		[Nullable(2)]
		public SlotConfig MainHandSlotConfig { [NullableContext(2)] get; [NullableContext(2)] protected set; }

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001071 RID: 4209 RVA: 0x000503C6 File Offset: 0x0004E5C6
		// (set) Token: 0x06001072 RID: 4210 RVA: 0x000503CE File Offset: 0x0004E5CE
		[Nullable(2)]
		public SlotConfig OffHandSlotConfig { [NullableContext(2)] get; [NullableContext(2)] protected set; }

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001073 RID: 4211 RVA: 0x000503D7 File Offset: 0x0004E5D7
		// (set) Token: 0x06001074 RID: 4212 RVA: 0x000503DF File Offset: 0x0004E5DF
		[Nullable(2)]
		public string TakeOutSlotColor { [NullableContext(2)] get; [NullableContext(2)] protected set; }

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06001075 RID: 4213 RVA: 0x000503E8 File Offset: 0x0004E5E8
		// (set) Token: 0x06001076 RID: 4214 RVA: 0x000503F0 File Offset: 0x0004E5F0
		public string HotkeyCode { get; protected set; } = "";

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001077 RID: 4215 RVA: 0x000503F9 File Offset: 0x0004E5F9
		// (set) Token: 0x06001078 RID: 4216 RVA: 0x00050401 File Offset: 0x0004E601
		public string HotkeyName { get; protected set; } = "";

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06001079 RID: 4217 RVA: 0x0005040A File Offset: 0x0004E60A
		// (set) Token: 0x0600107A RID: 4218 RVA: 0x00050412 File Offset: 0x0004E612
		public GlKeys HotKeyKey { get; protected set; } = 100;

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x0600107B RID: 4219 RVA: 0x0005041B File Offset: 0x0004E61B
		// (set) Token: 0x0600107C RID: 4220 RVA: 0x00050423 File Offset: 0x0004E623
		public List<SlotHotkeyConfig> HotkeyConfigs { get; protected set; } = new List<SlotHotkeyConfig>();

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x0600107D RID: 4221 RVA: 0x0005042C File Offset: 0x0004E62C
		// (set) Token: 0x0600107E RID: 4222 RVA: 0x00050434 File Offset: 0x0004E634
		public int RegularSlotsNumber { get; protected set; }

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x0005043D File Offset: 0x0004E63D
		// (set) Token: 0x06001080 RID: 4224 RVA: 0x00050445 File Offset: 0x0004E645
		public int ToolSlotNumber { get; protected set; }

		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001081 RID: 4225 RVA: 0x0005044E File Offset: 0x0004E64E
		// (set) Token: 0x06001082 RID: 4226 RVA: 0x00050456 File Offset: 0x0004E656
		public int TakeOutSlotNumber { get; protected set; }

		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x06001083 RID: 4227 RVA: 0x0005045F File Offset: 0x0004E65F
		// (set) Token: 0x06001084 RID: 4228 RVA: 0x00050467 File Offset: 0x0004E667
		[Nullable(2)]
		public string TakeOutSlotIcon { [NullableContext(2)] get; [NullableContext(2)] protected set; }

		// Token: 0x06001085 RID: 4229 RVA: 0x00050470 File Offset: 0x0004E670
		public ToolBag(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x000504B0 File Offset: 0x0004E6B0
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

		// Token: 0x06001087 RID: 4231 RVA: 0x00050738 File Offset: 0x0004E938
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

		// Token: 0x06001088 RID: 4232 RVA: 0x000508A4 File Offset: 0x0004EAA4
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

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001089 RID: 4233 RVA: 0x00051148 File Offset: 0x0004F348
		protected bool HasMainHandTakeOutSlot
		{
			get
			{
				return this.MainHandSlotConfig != null && this.TakeOutSlotNumber > 0;
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x0600108A RID: 4234 RVA: 0x0005115D File Offset: 0x0004F35D
		protected bool HasOffHandTakeOutSlot
		{
			get
			{
				return this.OffHandSlotConfig != null && this.TakeOutSlotNumber > ((this.MainHandSlotConfig != null) ? 1 : 0);
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x0600108B RID: 4235 RVA: 0x0005117A File Offset: 0x0004F37A
		protected int MainHandTakeOutSlotIndex
		{
			get
			{
				return this.RegularSlotsNumber + this.ToolSlotNumber;
			}
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x00051189 File Offset: 0x0004F389
		protected int OffHandTakeOutSlotIndex
		{
			get
			{
				return this.MainHandTakeOutSlotIndex + ((this.HasMainHandTakeOutSlot > false) ? 1 : 0);
			}
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x0005119C File Offset: 0x0004F39C
		protected bool IsSurplusTakeOutSlotIndex(int slotIndex)
		{
			int takeOutStartIndex = this.RegularSlotsNumber + this.ToolSlotNumber;
			return slotIndex >= takeOutStartIndex + this.TakeOutSlotNumber && slotIndex < takeOutStartIndex + this.ToolSlotNumber;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x000511D0 File Offset: 0x0004F3D0
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

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x0600108F RID: 4239 RVA: 0x00051235 File Offset: 0x0004F435
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

		// Token: 0x06001090 RID: 4240 RVA: 0x0005123D File Offset: 0x0004F43D
		protected virtual bool OnHotkeyPressed(KeyCombination keyCombination)
		{
			return this.OnHotkeyPressed(keyCombination, null);
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x00051248 File Offset: 0x0004F448
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

		// Token: 0x06001092 RID: 4242 RVA: 0x00051400 File Offset: 0x0004F600
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

		// Token: 0x06001093 RID: 4243 RVA: 0x0005149C File Offset: 0x0004F69C
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

		// Token: 0x06001094 RID: 4244 RVA: 0x00051570 File Offset: 0x0004F770
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

		// Token: 0x06001095 RID: 4245 RVA: 0x000515C8 File Offset: 0x0004F7C8
		protected static bool KeyCombinationsMatch([Nullable(2)] KeyCombination currentMapping, KeyCombination pressedCombination)
		{
			return currentMapping != null && (currentMapping.KeyCode == pressedCombination.KeyCode && currentMapping.SecondKeyCode.GetValueOrDefault() == pressedCombination.SecondKeyCode.GetValueOrDefault() && currentMapping.Ctrl == pressedCombination.Ctrl && currentMapping.Alt == pressedCombination.Alt) && currentMapping.Shift == pressedCombination.Shift;
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x0005162C File Offset: 0x0004F82C
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

		// Token: 0x06001097 RID: 4247 RVA: 0x000516A6 File Offset: 0x0004F8A6
		[NullableContext(2)]
		protected static bool HasHotkeyActionableStack(ItemSlot slot)
		{
			return ((slot != null) ? slot.Itemstack : null) != null && slot.Itemstack.StackSize > 0 && slot.Itemstack.Collectible != null;
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x000516D4 File Offset: 0x0004F8D4
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

		// Token: 0x06001099 RID: 4249 RVA: 0x0005175F File Offset: 0x0004F95F
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

		// Token: 0x0400079F RID: 1951
		[Nullable(new byte[]
		{
			2,
			1
		})]
		protected ActionConsumable<KeyCombination> PreviousHotkeyHandler;

		// Token: 0x040007A1 RID: 1953
		[Nullable(2)]
		protected ICoreClientAPI ClientApi;

		// Token: 0x040007A2 RID: 1954
		protected long HotkeyCooldownUntilMs;

		// Token: 0x040007A3 RID: 1955
		protected const long HotkeyCooldown = 120L;

		// Token: 0x020002A7 RID: 679
		[NullableContext(2)]
		[Nullable(0)]
		protected readonly struct HotkeyStoragePriority
		{
			// Token: 0x0600184B RID: 6219 RVA: 0x0006E20C File Offset: 0x0006C40C
			public HotkeyStoragePriority(bool heldItemCanBeStored, ItemSlotBagContentWithWildcardMatch targetSlot)
			{
				this.HeldItemCanBeStored = heldItemCanBeStored;
				this.TargetSlot = targetSlot;
			}

			// Token: 0x04000DA6 RID: 3494
			public readonly bool HeldItemCanBeStored;

			// Token: 0x04000DA7 RID: 3495
			public readonly ItemSlotBagContentWithWildcardMatch TargetSlot;
		}

		// Token: 0x020002A8 RID: 680
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000DA8 RID: 3496
			[Nullable(0)]
			public static Func<ItemSlotBagContentWithWildcardMatch, bool> <0>__HasHotkeyActionableStack;
		}
	}
}
