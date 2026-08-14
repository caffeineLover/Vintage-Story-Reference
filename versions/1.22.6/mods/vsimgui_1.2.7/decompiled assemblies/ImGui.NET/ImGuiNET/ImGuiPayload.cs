using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ImGuiNET
{
	// Token: 0x0200004C RID: 76
	public struct ImGuiPayload
	{
		// Token: 0x040005A7 RID: 1447
		public unsafe void* Data;

		// Token: 0x040005A8 RID: 1448
		public int DataSize;

		// Token: 0x040005A9 RID: 1449
		public uint SourceId;

		// Token: 0x040005AA RID: 1450
		public uint SourceParentId;

		// Token: 0x040005AB RID: 1451
		public int DataFrameCount;

		// Token: 0x040005AC RID: 1452
		[FixedBuffer(typeof(byte), 33)]
		public ImGuiPayload.<DataType>e__FixedBuffer DataType;

		// Token: 0x040005AD RID: 1453
		public byte Preview;

		// Token: 0x040005AE RID: 1454
		public byte Delivery;

		// Token: 0x0200009C RID: 156
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(0, Size = 33)]
		public struct <DataType>e__FixedBuffer
		{
			// Token: 0x040007FC RID: 2044
			public byte FixedElementField;
		}
	}
}
