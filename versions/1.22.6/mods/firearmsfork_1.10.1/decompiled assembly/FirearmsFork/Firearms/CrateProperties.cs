using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.GameContent;

namespace Firearms
{
	// Token: 0x0200000D RID: 13
	[NullableContext(1)]
	[Nullable(0)]
	public class CrateProperties
	{
		// Token: 0x17000021 RID: 33
		public CrateTypeProperties this[string type]
		{
			get
			{
				CrateTypeProperties props;
				if (!this.Properties.TryGetValue(type, ref props))
				{
					return this.Properties["*"];
				}
				return props;
			}
		}

		// Token: 0x0400003B RID: 59
		public Dictionary<string, CrateTypeProperties> Properties;

		// Token: 0x0400003C RID: 60
		public string[] Types;

		// Token: 0x0400003D RID: 61
		public Dictionary<string, LabelProps> Labels;

		// Token: 0x0400003E RID: 62
		public string DefaultType = "wood-aged";

		// Token: 0x0400003F RID: 63
		public string VariantByGroup;

		// Token: 0x04000040 RID: 64
		public string VariantByGroupInventory;

		// Token: 0x04000041 RID: 65
		public string InventoryClassName = "crate";
	}
}
