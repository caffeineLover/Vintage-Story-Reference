using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000EE RID: 238
	public static class ImpaleAccess
	{
		// Token: 0x06000C26 RID: 3110 RVA: 0x0003D65A File Offset: 0x0003B85A
		[NullableContext(2)]
		public static bool IsDevImpaleSpear(ItemSlot slot)
		{
			string text;
			if (slot == null)
			{
				text = null;
			}
			else
			{
				ItemStack itemstack = slot.Itemstack;
				if (itemstack == null)
				{
					text = null;
				}
				else
				{
					CollectibleObject collectible = itemstack.Collectible;
					if (collectible == null)
					{
						text = null;
					}
					else
					{
						AssetLocation code = collectible.Code;
						text = ((code != null) ? code.ToString() : null);
					}
				}
			}
			return text == "game:spear-devimpale";
		}

		// Token: 0x040005D5 RID: 1493
		[Nullable(1)]
		public const string DevImpaleSpearCode = "game:spear-devimpale";
	}
}
