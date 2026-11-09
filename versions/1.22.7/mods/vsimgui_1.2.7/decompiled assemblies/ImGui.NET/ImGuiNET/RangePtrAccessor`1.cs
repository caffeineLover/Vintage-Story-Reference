using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000088 RID: 136
	public struct RangePtrAccessor<T> where T : struct
	{
		// Token: 0x0600092A RID: 2346 RVA: 0x0001668C File Offset: 0x0001488C
		public RangePtrAccessor(IntPtr data, int count)
		{
			this = new RangePtrAccessor<T>(data.ToPointer(), count);
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0001669C File Offset: 0x0001489C
		public unsafe RangePtrAccessor(void* data, int count)
		{
			this.Data = data;
			this.Count = count;
		}

		// Token: 0x170001CC RID: 460
		public unsafe T this[int index]
		{
			get
			{
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException();
				}
				return Unsafe.Read<T>((void*)((byte*)this.Data + sizeof(void*) * index));
			}
		}

		// Token: 0x040007E8 RID: 2024
		public unsafe readonly void* Data;

		// Token: 0x040007E9 RID: 2025
		public readonly int Count;
	}
}
