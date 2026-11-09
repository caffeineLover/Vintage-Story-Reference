using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Utils;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000097 RID: 151
	public class TextureAttributedItem : Item, IHandBookPageCodeProvider
	{
		// Token: 0x06000836 RID: 2102 RVA: 0x0002FC48 File Offset: 0x0002DE48
		[NullableContext(1)]
		public virtual string HandbookPageCodeForStack(IWorldAccessor world, ItemStack stack)
		{
			return TextureAttributeHandbook.PageCodeForStack(stack, TextureAttributeHandbook.GetTextureAttributes(this));
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x0002FC56 File Offset: 0x0002DE56
		[NullableContext(1)]
		public override bool Satisfies(ItemStack thisStack, ItemStack otherStack)
		{
			ICoreAPI api = this.api;
			return TextureAttributeHandbook.SatisfiesIgnoringAttributes((api != null) ? api.World : null, thisStack, otherStack, TextureAttributeHandbook.GetTextureAttributes(this)) || base.Satisfies(thisStack, otherStack);
		}
	}
}
