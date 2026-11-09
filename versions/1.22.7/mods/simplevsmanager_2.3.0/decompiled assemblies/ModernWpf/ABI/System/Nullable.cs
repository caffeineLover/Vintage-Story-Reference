using System;

namespace ABI.System
{
	// Token: 0x02000120 RID: 288
	internal sealed class Nullable
	{
		// Token: 0x060009F1 RID: 2545 RVA: 0x0001BCA8 File Offset: 0x00019EA8
		public Nullable(object boxedObject)
		{
			this.Value = boxedObject;
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060009F2 RID: 2546 RVA: 0x0001BCB7 File Offset: 0x00019EB7
		public object Value { get; }
	}
}
