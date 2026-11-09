using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImGuiNET
{
	// Token: 0x0200006B RID: 107
	public struct ImGuiTextFilter
	{
		// Token: 0x040006EC RID: 1772
		[FixedBuffer(typeof(byte), 256)]
		public ImGuiTextFilter.<InputBuf>e__FixedBuffer InputBuf;

		// Token: 0x040006ED RID: 1773
		public ImVector Filters;

		// Token: 0x040006EE RID: 1774
		public int CountGrep;

		// Token: 0x0200009D RID: 157
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 256)]
		public struct <InputBuf>e__FixedBuffer
		{
			// Token: 0x040007FD RID: 2045
			public byte FixedElementField;
		}
	}
}
