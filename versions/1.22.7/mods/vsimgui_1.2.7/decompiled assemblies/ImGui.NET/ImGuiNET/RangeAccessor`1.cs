using System;
using System.Runtime.CompilerServices;

namespace ImGuiNET
{
	// Token: 0x02000087 RID: 135
	public struct RangeAccessor<T> where T : struct
	{
		// Token: 0x06000926 RID: 2342 RVA: 0x00016638 File Offset: 0x00014838
		public RangeAccessor(IntPtr data, int count)
		{
			this = new RangeAccessor<T>(data.ToPointer(), count);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x00016648 File Offset: 0x00014848
		public unsafe RangeAccessor(void* data, int count)
		{
			this.Data = data;
			this.Count = count;
		}

		// Token: 0x170001CB RID: 459
		public unsafe T this[int index]
		{
			get
			{
				if (index < 0 || index >= this.Count)
				{
					throw new IndexOutOfRangeException();
				}
				return Unsafe.AsRef<T>((void*)((byte*)this.Data + RangeAccessor<T>.s_sizeOfT * index));
			}
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x00016680 File Offset: 0x00014880
		// Note: this type is marked as 'beforefieldinit'.
		static RangeAccessor()
		{
			RangeAccessor<T>.s_sizeOfT = Unsafe.SizeOf<T>();
		}

		// Token: 0x040007E5 RID: 2021
		private static readonly int s_sizeOfT;

		// Token: 0x040007E6 RID: 2022
		public unsafe readonly void* Data;

		// Token: 0x040007E7 RID: 2023
		public readonly int Count;
	}
}
