using System;
using System.Runtime.InteropServices;

namespace ImGuiNET
{
	// Token: 0x02000086 RID: 134
	[StructLayout(2)]
	public struct UnionValue
	{
		// Token: 0x040007E2 RID: 2018
		[FieldOffset(0)]
		public int ValueI32;

		// Token: 0x040007E3 RID: 2019
		[FieldOffset(0)]
		public float ValueF32;

		// Token: 0x040007E4 RID: 2020
		[FieldOffset(0)]
		public IntPtr ValuePtr;
	}
}
