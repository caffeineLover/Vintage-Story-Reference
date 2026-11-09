using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Armor;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.Vanity
{
	// Token: 0x020000B0 RID: 176
	[NullableContext(1)]
	[Nullable(0)]
	public class VanityInventory : ArmorInventory
	{
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x00032A2F File Offset: 0x00030C2F
		// (set) Token: 0x06000959 RID: 2393 RVA: 0x00032A37 File Offset: 0x00030C37
		public override bool ModifyColors { get; set; }

		// Token: 0x0600095A RID: 2394 RVA: 0x00032A40 File Offset: 0x00030C40
		public VanityInventory(string inventoryID, ICoreAPI api) : base(inventoryID, api)
		{
			this.RecolorSlots();
			this.Synchronize();
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x00032A56 File Offset: 0x00030C56
		public override void OnItemSlotModified(ItemSlot slot)
		{
			base.OnItemSlotModified(slot);
			this.Synchronize();
			IPlayer player = base.Player;
			if (player == null)
			{
				return;
			}
			EntityPlayer entity = player.Entity;
			if (entity == null)
			{
				return;
			}
			entity.MarkShapeModified();
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x00032A80 File Offset: 0x00030C80
		public override void FromTreeAttributes(ITreeAttribute tree)
		{
			base.FromTreeAttributes(tree);
			foreach (ItemSlot itemSlot in this)
			{
				itemSlot.HexBackgroundColor = null;
			}
			if (tree.HasAttribute("hiddenSlots"))
			{
				foreach (byte slotId in tree.GetBytes("hiddenSlots", null))
				{
					ISlotContentCanHide slot = this[(int)slotId] as ISlotContentCanHide;
					if (slot != null)
					{
						slot.Hide = true;
						this[(int)slotId].HexBackgroundColor = "#776666";
					}
				}
			}
			this.RecolorSlots();
			this.Synchronize();
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00032B30 File Offset: 0x00030D30
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			byte[] hiddenSlots = Enumerable.ToArray<byte>(Enumerable.Select<ItemSlot, byte>(Enumerable.Where<ItemSlot>(this, delegate(ItemSlot slot)
			{
				ISlotContentCanHide slotContentCanHide = slot as ISlotContentCanHide;
				return slotContentCanHide != null && slotContentCanHide.Hide;
			}), (ItemSlot slot) => (byte)this.GetSlotId(slot)));
			tree.SetBytes("hiddenSlots", hiddenSlots);
			this.Synchronize();
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x00032B94 File Offset: 0x00030D94
		public void RecolorSlots()
		{
			foreach (ISlotContentCanHide slot in Enumerable.OfType<ISlotContentCanHide>(this))
			{
				if (slot.Hide)
				{
					(slot as ItemSlot).HexBackgroundColor = "#776666";
				}
				else
				{
					(slot as ItemSlot).HexBackgroundColor = null;
				}
			}
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x00032C00 File Offset: 0x00030E00
		public void Synchronize()
		{
			if (base.Player == null)
			{
				return;
			}
			byte[] hiddenSlots = Enumerable.ToArray<byte>(Enumerable.Select<ItemSlot, byte>(Enumerable.Where<ItemSlot>(this, delegate(ItemSlot slot)
			{
				ISlotContentCanHide slotContentCanHide = slot as ISlotContentCanHide;
				return slotContentCanHide != null && slotContentCanHide.Hide;
			}), (ItemSlot slot) => (byte)this.GetSlotId(slot)));
			CombatOverhaulSystem modSystem = this.Api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			if (modSystem == null)
			{
				return;
			}
			VanitySystemServer serverVanitySystem = modSystem.ServerVanitySystem;
			if (serverVanitySystem == null)
			{
				return;
			}
			serverVanitySystem.SendUpdate(base.Player, hiddenSlots);
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00032C80 File Offset: 0x00030E80
		protected override ItemSlot NewSlot(int slotId)
		{
			if (slotId < ArmorInventory._clothesSlotsCount)
			{
				ClothesVanitySlot slot = new ClothesVanitySlot(slotId, this);
				this._clothesSlotsIcons.TryGetValue(slotId, ref slot.BackgroundIcon);
				return slot;
			}
			if (slotId < ArmorInventory._vanillaSlots)
			{
				if (ArmorInventory._disableVanillaArmorSlots)
				{
					return new ArmorVanitySlot(this, ArmorType.Empty)
					{
						DrawUnavailable = true
					};
				}
				ClothesVanitySlot slot2 = new ClothesVanitySlot(slotId, this);
				this._clothesSlotsIcons.TryGetValue(slotId, ref slot2.BackgroundIcon);
				return slot2;
			}
			else
			{
				if (slotId < ArmorInventory._armorSlotsLastIndex)
				{
					ArmorType armorType = ArmorInventory.ArmorTypeFromIndex(slotId);
					ArmorVanitySlot slot3 = new ArmorVanitySlot(this, armorType);
					this._slotsByType[armorType] = slot3;
					this._armorSlotsIcons.TryGetValue(armorType, ref slot3.BackgroundIcon);
					return slot3;
				}
				if (slotId < ArmorInventory._gearSlotsLastIndex)
				{
					string slotType = ArmorInventory.GearSlotTypes[slotId - ArmorInventory._armorSlotsLastIndex];
					GearVanitySlot slot4 = new GearVanitySlot(slotType, this);
					this._gearSlots[slotType] = slot4;
					this._gearSlotsIcons.TryGetValue(slotType, ref slot4.BackgroundIcon);
					return slot4;
				}
				return new ItemSlot(this);
			}
		}
	}
}
