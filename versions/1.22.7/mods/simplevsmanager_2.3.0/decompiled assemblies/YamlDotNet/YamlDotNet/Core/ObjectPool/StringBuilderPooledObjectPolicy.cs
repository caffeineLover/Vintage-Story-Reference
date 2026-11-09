using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace YamlDotNet.Core.ObjectPool
{
	// Token: 0x02000100 RID: 256
	[NullableContext(1)]
	[Nullable(0)]
	internal class StringBuilderPooledObjectPolicy : IPooledObjectPolicy<StringBuilder>
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x00017457 File Offset: 0x00015657
		// (set) Token: 0x06000640 RID: 1600 RVA: 0x0001745F File Offset: 0x0001565F
		public int InitialCapacity { get; set; } = 100;

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x00017468 File Offset: 0x00015668
		// (set) Token: 0x06000642 RID: 1602 RVA: 0x00017470 File Offset: 0x00015670
		public int MaximumRetainedCapacity { get; set; } = 4096;

		// Token: 0x06000643 RID: 1603 RVA: 0x00017479 File Offset: 0x00015679
		public StringBuilder Create()
		{
			return new StringBuilder(this.InitialCapacity);
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x00017486 File Offset: 0x00015686
		public bool Return(StringBuilder obj)
		{
			if (obj.Capacity > this.MaximumRetainedCapacity)
			{
				return false;
			}
			obj.Clear();
			return true;
		}
	}
}
