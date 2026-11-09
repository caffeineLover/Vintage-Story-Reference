using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000080 RID: 128
	public struct ImVector
	{
		// Token: 0x06000914 RID: 2324 RVA: 0x000164AB File Offset: 0x000146AB
		public ImVector(int size, int capacity, IntPtr data)
		{
			this.Size = size;
			this.Capacity = capacity;
			this.Data = data;
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x000164C2 File Offset: 0x000146C2
		public unsafe ref T Ref<T>(int index)
		{
			return Unsafe.AsRef<T>((void*)((byte*)((void*)this.Data) + index * Unsafe.SizeOf<T>()));
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x000164DC File Offset: 0x000146DC
		public unsafe IntPtr Address<T>(int index)
		{
			return (IntPtr)((void*)((byte*)((void*)this.Data) + index * Unsafe.SizeOf<T>()));
		}

		// Token: 0x040007D4 RID: 2004
		public readonly int Size;

		// Token: 0x040007D5 RID: 2005
		public readonly int Capacity;

		// Token: 0x040007D6 RID: 2006
		public readonly IntPtr Data;
	}
}
