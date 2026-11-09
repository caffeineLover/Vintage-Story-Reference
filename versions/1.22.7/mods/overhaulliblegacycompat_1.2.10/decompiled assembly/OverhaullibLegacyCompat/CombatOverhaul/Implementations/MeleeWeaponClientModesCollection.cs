using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000081 RID: 129
	[NullableContext(1)]
	[Nullable(0)]
	public class MeleeWeaponClientModesCollection
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x00029F41 File Offset: 0x00028141
		public string CurrentModeCode
		{
			get
			{
				return this.CurrentModeValue;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x00029F49 File Offset: 0x00028149
		public MeleeWeaponClient CurrentMode
		{
			get
			{
				return this.Clients[this.CurrentModeValue];
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x00029F5C File Offset: 0x0002815C
		// (set) Token: 0x060006A5 RID: 1701 RVA: 0x00029F64 File Offset: 0x00028164
		public Dictionary<string, MeleeWeaponClient> Clients { get; protected set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x00029F6D File Offset: 0x0002816D
		// (set) Token: 0x060006A7 RID: 1703 RVA: 0x00029F75 File Offset: 0x00028175
		public Dictionary<string, MeleeWeaponModeStats> Modes { get; protected set; } = new Dictionary<string, MeleeWeaponModeStats>();

		// Token: 0x060006A8 RID: 1704 RVA: 0x00029F80 File Offset: 0x00028180
		public MeleeWeaponClientModesCollection(ICoreClientAPI api, Item item)
		{
			MeleeWeaponClientModesCollection <>4__this = this;
			this.Api = api;
			if (item.Attributes.KeyExists("Modes"))
			{
				this.Modes = item.Attributes.AsObject<MeleeWeaponModeCollectionStats>(null).Modes;
			}
			else
			{
				this.Modes.Add("default", item.Attributes.AsObject<MeleeWeaponModeStats>(null));
			}
			this.CurrentModeValue = Enumerable.First<KeyValuePair<string, MeleeWeaponModeStats>>(this.Modes).Key;
			this.Clients = Enumerable.ToDictionary<KeyValuePair<string, MeleeWeaponModeStats>, string, MeleeWeaponClient>(this.Modes, (KeyValuePair<string, MeleeWeaponModeStats> entry) => entry.Key, (KeyValuePair<string, MeleeWeaponModeStats> entry) => new MeleeWeaponClient(api, item, entry.Value));
			EnumerableExtensions.Foreach<KeyValuePair<string, MeleeWeaponClient>>(Enumerable.Where<KeyValuePair<string, MeleeWeaponClient>>(this.Clients, (KeyValuePair<string, MeleeWeaponClient> entry) => entry.Key != <>4__this.CurrentModeCode), delegate(KeyValuePair<string, MeleeWeaponClient> entry)
			{
				entry.Value.Active = false;
			});
			if (this.Modes.Count > 1)
			{
				this.ModeSelector = new ModeSelector(api, Enumerable.Select<KeyValuePair<string, MeleeWeaponModeStats>, ModeConfig>(this.Modes, (KeyValuePair<string, MeleeWeaponModeStats> entry) => new ModeConfig(entry.Key, entry.Value.Name, entry.Value.Icon)));
			}
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0002A0F4 File Offset: 0x000282F4
		public virtual void ChangeMode(string mode, EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			int fakeState = 0;
			this.CurrentMode.OnDeselected(player, mainHand, ref fakeState);
			this.CurrentMode.Active = false;
			this.CurrentModeValue = mode;
			this.CurrentMode.Active = true;
			this.CurrentMode.OnSelected(slot, player, mainHand, ref fakeState);
			this.CurrentMode.PlayReadyAnimation(mainHand);
			MeleeWeaponClient.SetGlobalCooldown(this.Api, 0L);
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0002A15D File Offset: 0x0002835D
		public virtual int GetToolMode(EntityPlayer player, ItemSlot slot)
		{
			ModeSelector modeSelector = this.ModeSelector;
			if (modeSelector == null)
			{
				return 0;
			}
			return modeSelector.GetToolMode();
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0002A170 File Offset: 0x00028370
		[return: Nullable(new byte[]
		{
			2,
			1
		})]
		public virtual SkillItem[] GetToolModes(EntityPlayer player, ItemSlot slot)
		{
			ModeSelector modeSelector = this.ModeSelector;
			if (modeSelector == null)
			{
				return null;
			}
			return modeSelector.GetToolModes();
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0002A183 File Offset: 0x00028383
		public virtual void SetToolMode(EntityPlayer player, ItemSlot slot, int toolMode)
		{
			if (this.ModeSelector == null)
			{
				return;
			}
			this.ModeSelector.SetToolMode(toolMode);
			this.ChangeMode(this.ModeSelector.SelectedMode, player, slot, this.IsMainHand(player, slot));
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0002A1B5 File Offset: 0x000283B5
		protected bool IsMainHand(EntityPlayer player, ItemSlot slot)
		{
			return player.RightHandItemSlot == slot;
		}

		// Token: 0x040002E6 RID: 742
		protected string CurrentModeValue;

		// Token: 0x040002E7 RID: 743
		[Nullable(2)]
		protected ModeSelector ModeSelector;

		// Token: 0x040002E8 RID: 744
		protected ICoreClientAPI Api;
	}
}
