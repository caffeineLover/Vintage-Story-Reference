using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace Allclasses
{
	// Token: 0x02000003 RID: 3
	public class AllclassesCookedContainer : BlockCookedContainer, IInFirepitRendererSupplier
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002084 File Offset: 0x00000284
		[NullableContext(1)]
		public IInFirepitRenderer GetRendererWhenInFirepit(ItemStack stack, BlockEntityFirepit firepit, bool forOutputSlot)
		{
			return new AllclassesPotInFirepitRenderer(this.api as ICoreClientAPI, stack, firepit.Pos, forOutputSlot);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000209E File Offset: 0x0000029E
		[NullableContext(1)]
		public EnumFirepitModel GetDesiredFirepitModel(ItemStack stack, BlockEntityFirepit firepit, bool forOutputSlot)
		{
			return 2;
		}
	}
}
