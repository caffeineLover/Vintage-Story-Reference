using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000081 RID: 129
	public struct ImVector<T>
	{
		// Token: 0x06000917 RID: 2327 RVA: 0x000164F6 File Offset: 0x000146F6
		public ImVector(ImVector vector)
		{
			this.Size = vector.Size;
			this.Capacity = vector.Capacity;
			this.Data = vector.Data;
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0001651C File Offset: 0x0001471C
		public ImVector(int size, int capacity, IntPtr data)
		{
			this.Size = size;
			this.Capacity = capacity;
			this.Data = data;
		}

		// Token: 0x170001C8 RID: 456
		public unsafe T this[int index]
		{
			get
			{
				return Unsafe.AsRef<T>((void*)((byte*)((void*)this.Data) + index * Unsafe.SizeOf<T>()));
			}
		}

		// Token: 0x040007D7 RID: 2007
		public readonly int Size;

		// Token: 0x040007D8 RID: 2008
		public readonly int Capacity;

		// Token: 0x040007D9 RID: 2009
		public readonly IntPtr Data;
	}
}
