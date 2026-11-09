using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace Firearms
{
	// Token: 0x0200000B RID: 11
	[NullableContext(1)]
	[Nullable(0)]
	public class PowderBarrelEntity : BlockEntityContainer
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00003024 File Offset: 0x00001224
		public override InventoryBase Inventory
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000061 RID: 97 RVA: 0x0000302B File Offset: 0x0000122B
		public override string InventoryClassName
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}
