using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000079 RID: 121
	[NullableContext(1)]
	[Nullable(0)]
	public class HandbookAntlerMount : BlockAntlerMount
	{
		// Token: 0x060005B7 RID: 1463 RVA: 0x000218E8 File Offset: 0x0001FAE8
		public override List<ItemStack> GetHandBookStacks(ICoreClientAPI capi)
		{
			if (this.Code == null)
			{
				return null;
			}
			JsonObject attributes = this.Attributes;
			bool flag;
			if (attributes == null)
			{
				flag = false;
			}
			else
			{
				JsonObject jsonObject = attributes["handbook"];
				flag = ((jsonObject != null) ? new bool?(jsonObject["exclude"].AsBool(false)) : default(bool?)).GetValueOrDefault();
			}
			if (flag)
			{
				return null;
			}
			JsonObject attributes2 = this.Attributes;
			string[] array = ((attributes2 != null) ? attributes2["types"].AsArray<string>(null, "game") : null) ?? Array.Empty<string>();
			List<ItemStack> stacks = new List<ItemStack>();
			foreach (string type in array)
			{
				ItemStack stack = new ItemStack(this, 1);
				stack.Attributes.SetString("type", type);
				stack.Attributes.SetString("material", "oak");
				stack.Attributes.SetBool("combatOverhaulHandbookRepresentative", true);
				stack.TempAttributes.SetBool("combatOverhaulHandbookRepresentative", true);
				stacks.Add(stack);
			}
			if (stacks.Count <= 0)
			{
				return base.GetHandBookStacks(capi);
			}
			return stacks;
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00021A04 File Offset: 0x0001FC04
		public override bool Satisfies(ItemStack thisStack, ItemStack otherStack)
		{
			if (HandbookAntlerMount.IsHandbookRepresentative(otherStack) && ((thisStack != null) ? thisStack.Collectible : null) == this && otherStack.Collectible == this && thisStack.Class == otherStack.Class && thisStack.Id == otherStack.Id)
			{
				string @string = thisStack.Attributes.GetString("type", null);
				string handbookType = otherStack.Attributes.GetString("type", null);
				return @string == handbookType;
			}
			return base.Satisfies(thisStack, otherStack);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00021A7F File Offset: 0x0001FC7F
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			if (HandbookAntlerMount.IsHandbookRepresentative(inSlot.Itemstack))
			{
				return;
			}
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00021A9C File Offset: 0x0001FC9C
		[NullableContext(2)]
		private static bool IsHandbookRepresentative(ItemStack itemStack)
		{
			if (itemStack != null)
			{
				ITreeAttribute tempAttributes = itemStack.TempAttributes;
				if (((tempAttributes != null) ? new bool?(tempAttributes.GetBool("combatOverhaulHandbookRepresentative", false)) : default(bool?)).GetValueOrDefault())
				{
					return true;
				}
			}
			if (itemStack == null)
			{
				return false;
			}
			ITreeAttribute attributes = itemStack.Attributes;
			return ((attributes != null) ? new bool?(attributes.GetBool("combatOverhaulHandbookRepresentative", false)) : default(bool?)).GetValueOrDefault();
		}

		// Token: 0x0400027B RID: 635
		private const string HandbookRepresentativeAttribute = "combatOverhaulHandbookRepresentative";
	}
}
