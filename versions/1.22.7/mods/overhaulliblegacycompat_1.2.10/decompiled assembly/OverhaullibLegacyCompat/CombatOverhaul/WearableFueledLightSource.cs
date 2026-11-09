using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace CombatOverhaul
{
	// Token: 0x02000009 RID: 9
	[NullableContext(1)]
	[Nullable(0)]
	public class WearableFueledLightSource : Item, IWearableLightSource, IFueledItem, ITogglableItem
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002FF8 File Offset: 0x000011F8
		public virtual string HotKeyCode
		{
			get
			{
				return "toggleWearableLight";
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002FFF File Offset: 0x000011FF
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00003007 File Offset: 0x00001207
		public WearableFueledLightSourceStats Stats { get; set; } = new WearableFueledLightSourceStats();

		// Token: 0x0600006B RID: 107 RVA: 0x00003010 File Offset: 0x00001210
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this.Stats = this.Attributes.AsObject<WearableFueledLightSourceStats>(null);
			if (this.MaxStackSize > 1)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(91, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Item '");
				defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(this.Code);
				defaultInterpolatedStringHandler.AppendLiteral("' has max stack size > 1 while WearableFueledLightSource supposed to have it set to 1");
				LoggerUtil.Error(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._clickToToggle = new WorldInteraction
			{
				MouseButton = 2,
				ActionLangCode = "combatoverhaul:interaction-toggle-light-source"
			};
			this._hotkeyToToggle = new WorldInteraction
			{
				ActionLangCode = Lang.Get("combatoverhaul:interaction-toggle-light-source-hotkey", Array.Empty<object>()),
				HotKeyCodes = new string[]
				{
					this.HotKeyCode
				},
				MouseButton = 255
			};
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000030DB File Offset: 0x000012DB
		public override int GetMergableQuantity(ItemStack sinkStack, ItemStack sourceStack, EnumMergePriority priority)
		{
			if (priority != 1)
			{
				return base.GetMergableQuantity(sinkStack, sourceStack, priority);
			}
			if (this.GetStackFuel(sourceStack) == 0f)
			{
				return base.GetMergableQuantity(sinkStack, sourceStack, priority);
			}
			return 1;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003104 File Offset: 0x00001304
		public override void TryMergeStacks(ItemStackMergeOperation op)
		{
			if (op.CurrentPriority == 1)
			{
				float stackFuel = this.GetStackFuel(op.SourceSlot.Itemstack);
				double fuelHours = this.GetFuelHours(op.ActingPlayer, op.SinkSlot);
				if (stackFuel > 0f && fuelHours + (double)(stackFuel * this.Stats.MaxFuelWasteFraction) < (double)this.Stats.FuelCapacityHours)
				{
					this.SetFuelHours(op.ActingPlayer, op.SinkSlot, (double)stackFuel + fuelHours);
					op.MovedQuantity = 1;
					op.SourceSlot.TakeOut(1);
					op.SinkSlot.MarkDirty();
					return;
				}
				if (this.api.Side == 2)
				{
					ICoreClientAPI coreClientAPI = this.api as ICoreClientAPI;
					if (coreClientAPI == null)
					{
						return;
					}
					coreClientAPI.TriggerIngameError(this, "maskfull", Lang.Get("ingameerror-mask-full", Array.Empty<object>()));
				}
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000031D8 File Offset: 0x000013D8
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
			if (this.Stats.NeedsFuel)
			{
				IClientWorldAccessor clientWorldAccessor = world as IClientWorldAccessor;
				double fuelHours = this.GetFuelHours((clientWorldAccessor != null) ? clientWorldAccessor.Player : null, inSlot);
				dsc.AppendLine(Lang.Get("Has fuel for {0:0.#} hours", new object[]
				{
					fuelHours
				}));
				if (fuelHours <= 0.0)
				{
					dsc.AppendLine(Lang.Get("Add fuel to refuel", Array.Empty<object>()));
				}
				dsc.AppendLine();
			}
			dsc.AppendLine();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003268 File Offset: 0x00001468
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
		{
			EntityPlayer entityPlayer = byEntity as EntityPlayer;
			IPlayer player = (entityPlayer != null) ? entityPlayer.Player : null;
			if (player != null)
			{
				this.Toggle(player, slot);
				handHandling = 4;
			}
			base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handHandling);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000032A6 File Offset: 0x000014A6
		public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
		{
			return Enumerable.ToArray<WorldInteraction>(ArrayExtensions.Append<WorldInteraction>(ArrayExtensions.Append<WorldInteraction>(base.GetHeldInteractionHelp(inSlot), this._clickToToggle), this._hotkeyToToggle));
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000032CC File Offset: 0x000014CC
		public virtual void AddFuelHours(IPlayer player, ItemSlot slot, double hours)
		{
			bool flag;
			if (slot == null)
			{
				flag = (null != null);
			}
			else
			{
				ItemStack itemstack = slot.Itemstack;
				flag = (((itemstack != null) ? itemstack.Attributes : null) != null);
			}
			if (!flag)
			{
				return;
			}
			if (hours < 0.0 && !this.TurnedOn(player, slot))
			{
				return;
			}
			slot.Itemstack.Attributes.SetDouble("fuelHours", Math.Max(0.0, hours + this.GetFuelHours(player, slot)));
			slot.OnItemSlotModified(null);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003340 File Offset: 0x00001540
		public virtual double GetFuelHours(IPlayer player, ItemSlot slot)
		{
			bool flag;
			if (slot == null)
			{
				flag = (null != null);
			}
			else
			{
				ItemStack itemstack = slot.Itemstack;
				flag = (((itemstack != null) ? itemstack.Attributes : null) != null);
			}
			if (!flag)
			{
				return 0.0;
			}
			return Math.Max(0.0, slot.Itemstack.Attributes.GetDecimal("fuelHours", 0.0));
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000339D File Offset: 0x0000159D
		public virtual bool ConsumeFuelWhenSleeping(IPlayer player, ItemSlot slot)
		{
			return this.Stats.ConsumeFuelWhileSleeping;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000033AA File Offset: 0x000015AA
		public virtual byte[] GetLightHsv(EntityPlayer player, ItemSlot slot)
		{
			if (!this.TurnedOn(player.Player, slot) || !this.Stats.NeedsFuel)
			{
				return this.Stats.TurnedOffLightHsv;
			}
			return this.Stats.LightHsv;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000033E0 File Offset: 0x000015E0
		public virtual float GetStackFuel(ItemStack stack)
		{
			foreach (KeyValuePair<string, float> keyValuePair in this.Stats.FuelItems)
			{
				string text;
				float num;
				keyValuePair.Deconstruct(ref text, ref num);
				string itemWildcard = text;
				float fuelAmount = num;
				string text2 = itemWildcard;
				Item item = stack.Item;
				if (WildcardUtil.Match(text2, ((item != null) ? item.Code.ToString() : null) ?? ""))
				{
					return fuelAmount * this.Stats.FuelEfficiency;
				}
				string text3 = itemWildcard;
				Block block = stack.Block;
				if (WildcardUtil.Match(text3, ((block != null) ? block.Code.ToString() : null) ?? ""))
				{
					return fuelAmount * this.Stats.FuelEfficiency;
				}
			}
			JsonObject itemAttributes = stack.ItemAttributes;
			return ((itemAttributes != null) ? itemAttributes[this.Stats.FuelAttribute].AsFloat(0f) : 0f) * this.Stats.FuelEfficiency;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000034F8 File Offset: 0x000016F8
		public virtual void SetFuelHours(IPlayer player, ItemSlot slot, double fuelHours)
		{
			bool flag;
			if (slot == null)
			{
				flag = (null != null);
			}
			else
			{
				ItemStack itemstack = slot.Itemstack;
				flag = (((itemstack != null) ? itemstack.Attributes : null) != null);
			}
			if (!flag)
			{
				return;
			}
			fuelHours = GameMath.Clamp(fuelHours, 0.0, (double)this.Stats.FuelCapacityHours);
			slot.Itemstack.Attributes.SetDouble("fuelHours", fuelHours);
			slot.MarkDirty();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000355C File Offset: 0x0000175C
		public virtual bool TurnedOn(IPlayer player, ItemSlot slot)
		{
			bool? flag;
			if (slot == null)
			{
				flag = default(bool?);
			}
			else
			{
				ItemStack itemstack = slot.Itemstack;
				if (itemstack == null)
				{
					flag = default(bool?);
				}
				else
				{
					ITreeAttribute attributes = itemstack.Attributes;
					flag = ((attributes != null) ? new bool?(attributes.GetBool(this.Stats.ToggleAttribute, false)) : default(bool?));
				}
			}
			bool? flag2 = flag;
			return flag2.GetValueOrDefault();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000035C0 File Offset: 0x000017C0
		public virtual void TurnOn(IPlayer player, ItemSlot slot)
		{
			if (this.GetFuelHours(player, slot) <= 0.0)
			{
				this.TurnOff(player, slot);
				return;
			}
			if (slot != null)
			{
				ItemStack itemstack = slot.Itemstack;
				if (itemstack != null)
				{
					ITreeAttribute attributes = itemstack.Attributes;
					if (attributes != null)
					{
						attributes.SetBool(this.Stats.ToggleAttribute, true);
					}
				}
			}
			if (slot != null)
			{
				slot.MarkDirty();
			}
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000361D File Offset: 0x0000181D
		public virtual void TurnOff(IPlayer player, ItemSlot slot)
		{
			if (slot != null)
			{
				ItemStack itemstack = slot.Itemstack;
				if (itemstack != null)
				{
					ITreeAttribute attributes = itemstack.Attributes;
					if (attributes != null)
					{
						attributes.SetBool(this.Stats.ToggleAttribute, false);
					}
				}
			}
			if (slot != null)
			{
				slot.MarkDirty();
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003653 File Offset: 0x00001853
		public virtual void Toggle(IPlayer player, ItemSlot slot)
		{
			if (this.TurnedOn(player, slot))
			{
				this.TurnOff(player, slot);
				return;
			}
			this.TurnOn(player, slot);
		}

		// Token: 0x0400001F RID: 31
		[Nullable(2)]
		private WorldInteraction _clickToToggle;

		// Token: 0x04000020 RID: 32
		[Nullable(2)]
		private WorldInteraction _hotkeyToToggle;
	}
}
