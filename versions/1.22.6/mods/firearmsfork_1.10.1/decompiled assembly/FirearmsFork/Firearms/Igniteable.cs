using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace Firearms
{
	// Token: 0x02000009 RID: 9
	[NullableContext(1)]
	[Nullable(0)]
	public class Igniteable : CollectibleBehavior
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00002E46 File Offset: 0x00001046
		public Igniteable(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002E5A File Offset: 0x0000105A
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00002E62 File Offset: 0x00001062
		public IgniteableStats Stats { get; set; } = new IgniteableStats();

		// Token: 0x0600005C RID: 92 RVA: 0x00002E6B File Offset: 0x0000106B
		public override void Initialize(JsonObject properties)
		{
			base.Initialize(properties);
			this.Stats = properties.AsObject<IgniteableStats>(null);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002E84 File Offset: 0x00001084
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling, ref EnumHandling handling)
		{
			if (byEntity.Api.Side == 1)
			{
				EntityPlayer player = byEntity as EntityPlayer;
				if (player != null)
				{
					try
					{
						this.ReplaceStack(slot, player);
					}
					catch
					{
						return;
					}
				}
				handling = 1;
				handHandling = 1;
				return;
			}
			if (((blockSel != null) ? blockSel.Block : null) == null)
			{
				return;
			}
			string code = blockSel.Block.Code.ToShortString();
			bool satisfy = false;
			string[] igniteFrom = this.Stats.IgniteFrom;
			for (int i = 0; i < igniteFrom.Length; i++)
			{
				if (WildcardUtil.Match(igniteFrom[i], code))
				{
					satisfy = true;
					break;
				}
			}
			if (!satisfy)
			{
				return;
			}
			handling = 1;
			handHandling = 1;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002F2C File Offset: 0x0000112C
		private void ReplaceStack(ItemSlot slot, EntityPlayer player)
		{
			JsonItemStack stack = this.Stats.TransitionStack.Clone();
			if (!stack.Resolve(player.Api.World, "Igniteable"))
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
