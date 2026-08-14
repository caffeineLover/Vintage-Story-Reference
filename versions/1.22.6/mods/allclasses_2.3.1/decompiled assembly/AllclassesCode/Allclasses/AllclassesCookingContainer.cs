using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace Allclasses
{
	// Token: 0x02000004 RID: 4
	public class AllclassesCookingContainer : BlockCookingContainer, IInFirepitRendererSupplier
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000020A9 File Offset: 0x000002A9
		[NullableContext(1)]
		public IInFirepitRenderer GetRendererWhenInFirepit(ItemStack stack, BlockEntityFirepit firepit, bool forOutputSlot)
		{
			return new AllclassesPotInFirepitRenderer(this.api as ICoreClientAPI, stack, firepit.Pos, forOutputSlot);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020C3 File Offset: 0x000002C3
		[NullableContext(1)]
		public EnumFirepitModel GetDesiredFirepitModel(ItemStack stack, BlockEntityFirepit firepit, bool forOutputSlot)
		{
			return 2;
		}
	}
}
