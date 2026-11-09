using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Util;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000099 RID: 153
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AmmoSelector
	{
		// Token: 0x06000840 RID: 2112 RVA: 0x0002FF54 File Offset: 0x0002E154
		public AmmoSelector(ICoreClientAPI api, string ammoWildcard)
		{
			this._api = api;
			this._ammoWildcard = ammoWildcard;
			this.SelectedAmmo = ammoWildcard;
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000841 RID: 2113 RVA: 0x0002FFAA File Offset: 0x0002E1AA
		// (set) Token: 0x06000842 RID: 2114 RVA: 0x0002FFB2 File Offset: 0x0002E1B2
		public string SelectedAmmo { get; private set; }

		// Token: 0x06000843 RID: 2115 RVA: 0x0002FFBB File Offset: 0x0002E1BB
		public int GetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection)
		{
			if (this._ammoSlots.Count == 0)
			{
				this.UpdateAmmoSlots(byPlayer);
			}
			return this.GetSelectedModeIndex();
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0002FFD7 File Offset: 0x0002E1D7
		public SkillItem[] GetToolModes(ItemSlot slot, IClientPlayer forPlayer, BlockSelection blockSel)
		{
			this.UpdateAmmoSlots(forPlayer);
			return this.GetOrGenerateModes();
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0002FFE8 File Offset: 0x0002E1E8
		public void SetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection, int toolMode)
		{
			if (toolMode == 0 || this._ammoSlots.Count < toolMode)
			{
				this.SelectedAmmo = this._ammoWildcard;
				return;
			}
			this.SelectedAmmo = this._ammoSlots[toolMode - 1].Itemstack.Collectible.Code.ToString();
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00030040 File Offset: 0x0002E240
		private int GetSelectedModeIndex()
		{
			if (this.SelectedAmmo == this._ammoWildcard)
			{
				return 0;
			}
			for (int index = 0; index < this._ammoSlots.Count; index++)
			{
				if (WildcardUtil.Match(this._ammoWildcard, this._ammoSlots[index].Itemstack.Item.Code.ToString()))
				{
					return index + 1;
				}
			}
			return 0;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x000300AA File Offset: 0x0002E2AA
		private void UpdateAmmoSlots(IPlayer player)
		{
			this._ammoSlots.Clear();
			player.Entity.WalkInventory(delegate(ItemSlot slot)
			{
				bool flag;
				if (slot == null)
				{
					flag = (null != null);
				}
				else
				{
					ItemStack itemstack = slot.Itemstack;
					flag = (((itemstack != null) ? itemstack.Item : null) != null);
				}
				if (!flag)
				{
					return true;
				}
				if (slot.Itemstack.Item.GetCollectibleBehavior<ProjectileBehavior>(true) != null && WildcardUtil.Match(this._ammoWildcard, slot.Itemstack.Item.Code.ToString()))
				{
					this.AddAmmoStackToList(slot.Itemstack.Clone());
				}
				return true;
			});
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x000300D0 File Offset: 0x0002E2D0
		private void AddAmmoStackToList(ItemStack stack)
		{
			IEnumerable<ItemSlot> ammoSlots = this._ammoSlots;
			Func<ItemSlot, bool> <>9__0;
			Func<ItemSlot, bool> func;
			if ((func = <>9__0) == null)
			{
				func = (<>9__0 = ((ItemSlot slot) => slot.Itemstack.Collectible.Code.ToString() == stack.Collectible.Code.ToString()));
			}
			using (IEnumerator<ItemSlot> enumerator = Enumerable.Where<ItemSlot>(ammoSlots, func).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					enumerator.Current.Itemstack.StackSize += stack.StackSize;
					return;
				}
			}
			this._ammoSlots.Add(new DummySlot(stack));
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0003017C File Offset: 0x0002E37C
		private SkillItem[] GetOrGenerateModes()
		{
			TimeSpan currentTime = TimeSpan.FromMilliseconds(this._api.World.ElapsedMilliseconds);
			if (currentTime - this._lastGenerationTime < this._generationTimeout)
			{
				return this._modesCache;
			}
			this._lastGenerationTime = currentTime;
			this._modesCache = this.GenerateToolModes();
			return this._modesCache;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x000301D8 File Offset: 0x0002E3D8
		private SkillItem[] GenerateToolModes()
		{
			IEnumerable<SkillItem> modesFromSlots = ToolModesUtils.GetModesFromSlots(this._api, this._ammoSlots, (ItemSlot slot) => slot.Itemstack.Collectible.GetHeldItemName(slot.Itemstack));
			SkillItem mode = new SkillItem
			{
				Code = new AssetLocation("none-0"),
				Name = Lang.Get("combatoverhaul:toolmode-noselection", Array.Empty<object>())
			};
			return Enumerable.ToArray<SkillItem>(Enumerable.Prepend<SkillItem>(modesFromSlots, mode));
		}

		// Token: 0x040003C1 RID: 961
		private readonly ICoreClientAPI _api;

		// Token: 0x040003C2 RID: 962
		private readonly string _ammoWildcard;

		// Token: 0x040003C3 RID: 963
		private readonly List<ItemSlot> _ammoSlots = new List<ItemSlot>();

		// Token: 0x040003C4 RID: 964
		private readonly TimeSpan _generationTimeout = TimeSpan.FromSeconds(1L);

		// Token: 0x040003C5 RID: 965
		private TimeSpan _lastGenerationTime = TimeSpan.Zero;

		// Token: 0x040003C6 RID: 966
		private SkillItem[] _modesCache = Array.Empty<SkillItem>();
	}
}
