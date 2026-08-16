using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace Firearms
{
	// Token: 0x0200000C RID: 12
	[NullableContext(1)]
	[Nullable(0)]
	public class CrateTypeProperties
	{
		// Token: 0x04000036 RID: 54
		public int QuantitySlots;

		// Token: 0x04000037 RID: 55
		public CompositeShape Shape;

		// Token: 0x04000038 RID: 56
		public string RotatatableInterval;

		// Token: 0x04000039 RID: 57
		public EnumItemStorageFlags StorageType;

		// Token: 0x0400003A RID: 58
		public bool RetrieveOnly;
	}
}
