using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Windows.Foundation.Collections
{
	// Token: 0x02000086 RID: 134
	[Guid("913337E9-11A1-4345-A3A2-4E7F956E222D")]
	internal interface IVector<T> : IIterable<T>
	{
		// Token: 0x0600045E RID: 1118
		T GetAt(uint index);

		// Token: 0x0600045F RID: 1119
		IReadOnlyList<T> GetView();

		// Token: 0x06000460 RID: 1120
		bool IndexOf(T value, out uint index);

		// Token: 0x06000461 RID: 1121
		void SetAt(uint index, T value);

		// Token: 0x06000462 RID: 1122
		void InsertAt(uint index, T value);

		// Token: 0x06000463 RID: 1123
		void RemoveAt(uint index);

		// Token: 0x06000464 RID: 1124
		void Append(T value);

		// Token: 0x06000465 RID: 1125
		void RemoveAtEnd();

		// Token: 0x06000466 RID: 1126
		void _Clear();

		// Token: 0x06000467 RID: 1127
		uint GetMany(uint startIndex, ref T[] items);

		// Token: 0x06000468 RID: 1128
		void ReplaceAll(T[] items);

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000469 RID: 1129
		uint Size { get; }
	}
}
