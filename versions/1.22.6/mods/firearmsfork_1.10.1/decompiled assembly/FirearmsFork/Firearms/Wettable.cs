using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Inputs;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Firearms
{
	// Token: 0x02000013 RID: 19
	[NullableContext(1)]
	[Nullable(0)]
	public class Wettable : CollectibleBehavior, IOnInInventory
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x0000473B File Offset: 0x0000293B
		public Wettable(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000474F File Offset: 0x0000294F
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00004757 File Offset: 0x00002957
		public WettableStats Stats { get; set; } = new WettableStats();

		// Token: 0x060000A9 RID: 169 RVA: 0x00004760 File Offset: 0x00002960
		public override void Initialize(JsonObject properties)
		{
			base.Initialize(properties);
			this.Stats = properties.AsObject<WettableStats>(null);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004778 File Offset: 0x00002978
		public void OnInInventory(EntityPlayer player, ItemSlot slot)
		{
			if (!player.Swimming)
			{
				return;
			}
			JsonItemStack stack = this.Stats.TransitionStack.Clone();
			if (!stack.Resolve(player.Api.World, "Wettable"))
			{
				return;
			}
			stack.ResolvedItemstack.Attributes.MergeTree(slot.Itemstack.Attributes);
			stack.ResolvedItemstack.StackSize = slot.Itemstack.StackSize;
			ItemSlot itemSlot = new DummySlot(stack.ResolvedItemstack);
			slot.TakeOutWhole();
			if (itemSlot.TryPutInto(player.Api.World, slot, stack.ResolvedItemstack.StackSize) <= 0)
			{
				return;
			}
			if (this.Stats.Sound != "")
			{
				AssetLocation sound = new AssetLocation(this.Stats.Sound);
				player.Api.World.PlaySoundAt(sound, player.Player, player.Player, false, 16f, 1f);
			}
			slot.MarkDirty();
		}
	}
}
