using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace CombatOverhaul.Armor
{
	// Token: 0x0200014D RID: 333
	[NullableContext(1)]
	[Nullable(0)]
	public class GoesIntoSlotsInfo : CollectibleBehavior
	{
		// Token: 0x06001045 RID: 4165 RVA: 0x0004F8A5 File Offset: 0x0004DAA5
		public GoesIntoSlotsInfo(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0004F8B0 File Offset: 0x0004DAB0
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
			ItemStack itemstack = inSlot.Itemstack;
			if (((itemstack != null) ? itemstack.Collectible : null) == null)
			{
				return;
			}
			string stackDressType = inSlot.Itemstack.Collectible.Attributes["clothescategory"].AsString(null) ?? inSlot.Itemstack.Collectible.Attributes["attachableToEntity"]["categoryCode"].AsString(null);
			string[] stackDressTypes = inSlot.Itemstack.Collectible.Attributes["clothescategories"].AsObject<string[]>(null) ?? inSlot.Itemstack.Collectible.Attributes["attachableToEntity"]["categoryCodes"].AsObject<string[]>(null);
			List<string> slotTypes = new List<string>();
			if (stackDressTypes != null)
			{
				slotTypes.AddRange(stackDressTypes);
			}
			if (stackDressType != null)
			{
				slotTypes.Add(stackDressType);
			}
			if (!Enumerable.Any<string>(slotTypes))
			{
				return;
			}
			string slotTypeNames = Enumerable.Aggregate<string>(Enumerable.Select<string, string>(slotTypes, (string slot) => Lang.Get("combatoverhaul:slot-" + slot, Array.Empty<object>())), (string f, string s) => f + ", " + s);
			string slotTypePrefix = Lang.Get("combatoverhaul:slot-types", Array.Empty<object>());
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
			appendInterpolatedStringHandler..ctor(2, 2, dsc);
			appendInterpolatedStringHandler.AppendFormatted(slotTypePrefix);
			appendInterpolatedStringHandler.AppendLiteral(": ");
			appendInterpolatedStringHandler.AppendFormatted(slotTypeNames);
			dsc.AppendLine(ref appendInterpolatedStringHandler);
		}
	}
}
