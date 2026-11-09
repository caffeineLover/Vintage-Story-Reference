using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000082 RID: 130
	public struct ImPtrVector<T>
	{
		// Token: 0x0600091A RID: 2330 RVA: 0x0001654D File Offset: 0x0001474D
		public ImPtrVector(ImVector vector, int stride)
		{
			this = new ImPtrVector<T>(vector.Size, vector.Capacity, vector.Data, stride);
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x00016568 File Offset: 0x00014768
		public ImPtrVector(int size, int capacity, IntPtr data, int stride)
		{
			this.Size = size;
			this.Capacity = capacity;
			this.Data = data;
			this._stride = stride;
		}

		// Token: 0x170001C9 RID: 457
		public unsafe T this[int index]
		{
			get
			{
				byte* ptr = (byte*)((void*)this.Data) + index * this._stride;
				return Unsafe.Read<T>((void*)(&ptr));
			}
		}

		// Token: 0x040007DA RID: 2010
		public readonly int Size;

		// Token: 0x040007DB RID: 2011
		public readonly int Capacity;

		// Token: 0x040007DC RID: 2012
		public readonly IntPtr Data;

		// Token: 0x040007DD RID: 2013
		private readonly int _stride;
	}
}
