using System;
using ABI.System;
using Windows.Foundation;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000AE RID: 174
	internal static class IPropertyValue_Delegates
	{
		// Token: 0x020002CE RID: 718
		// (Invoke) Token: 0x060015DD RID: 5597
		public unsafe delegate int get_Type_0(IntPtr thisPtr, PropertyType* value);

		// Token: 0x020002CF RID: 719
		// (Invoke) Token: 0x060015E1 RID: 5601
		public unsafe delegate int get_IsNumericScalar_1(IntPtr thisPtr, byte* value);

		// Token: 0x020002D0 RID: 720
		// (Invoke) Token: 0x060015E5 RID: 5605
		public unsafe delegate int GetUInt8_2(IntPtr thisPtr, byte* value);

		// Token: 0x020002D1 RID: 721
		// (Invoke) Token: 0x060015E9 RID: 5609
		public unsafe delegate int GetInt16_3(IntPtr thisPtr, short* value);

		// Token: 0x020002D2 RID: 722
		// (Invoke) Token: 0x060015ED RID: 5613
		public unsafe delegate int GetUInt16_4(IntPtr thisPtr, ushort* value);

		// Token: 0x020002D3 RID: 723
		// (Invoke) Token: 0x060015F1 RID: 5617
		public unsafe delegate int GetInt32_5(IntPtr thisPtr, int* value);

		// Token: 0x020002D4 RID: 724
		// (Invoke) Token: 0x060015F5 RID: 5621
		public unsafe delegate int GetUInt32_6(IntPtr thisPtr, uint* value);

		// Token: 0x020002D5 RID: 725
		// (Invoke) Token: 0x060015F9 RID: 5625
		public unsafe delegate int GetInt64_7(IntPtr thisPtr, long* value);

		// Token: 0x020002D6 RID: 726
		// (Invoke) Token: 0x060015FD RID: 5629
		public unsafe delegate int GetUInt64_8(IntPtr thisPtr, ulong* value);

		// Token: 0x020002D7 RID: 727
		// (Invoke) Token: 0x06001601 RID: 5633
		public unsafe delegate int GetSingle_9(IntPtr thisPtr, float* value);

		// Token: 0x020002D8 RID: 728
		// (Invoke) Token: 0x06001605 RID: 5637
		public unsafe delegate int GetDouble_10(IntPtr thisPtr, double* value);

		// Token: 0x020002D9 RID: 729
		// (Invoke) Token: 0x06001609 RID: 5641
		public unsafe delegate int GetChar16_11(IntPtr thisPtr, ushort* value);

		// Token: 0x020002DA RID: 730
		// (Invoke) Token: 0x0600160D RID: 5645
		public unsafe delegate int GetBoolean_12(IntPtr thisPtr, byte* value);

		// Token: 0x020002DB RID: 731
		// (Invoke) Token: 0x06001611 RID: 5649
		public unsafe delegate int GetString_13(IntPtr thisPtr, IntPtr* value);

		// Token: 0x020002DC RID: 732
		// (Invoke) Token: 0x06001615 RID: 5653
		public unsafe delegate int GetGuid_14(IntPtr thisPtr, Guid* value);

		// Token: 0x020002DD RID: 733
		// (Invoke) Token: 0x06001619 RID: 5657
		public unsafe delegate int GetDateTime_15(IntPtr thisPtr, DateTimeOffset* value);

		// Token: 0x020002DE RID: 734
		// (Invoke) Token: 0x0600161D RID: 5661
		public unsafe delegate int GetTimeSpan_16(IntPtr thisPtr, TimeSpan* value);

		// Token: 0x020002DF RID: 735
		// (Invoke) Token: 0x06001621 RID: 5665
		public unsafe delegate int GetPoint_17(IntPtr thisPtr, Windows.Foundation.Point* value);

		// Token: 0x020002E0 RID: 736
		// (Invoke) Token: 0x06001625 RID: 5669
		public unsafe delegate int GetSize_18(IntPtr thisPtr, Windows.Foundation.Size* value);

		// Token: 0x020002E1 RID: 737
		// (Invoke) Token: 0x06001629 RID: 5673
		public unsafe delegate int GetRect_19(IntPtr thisPtr, Windows.Foundation.Rect* value);

		// Token: 0x020002E2 RID: 738
		// (Invoke) Token: 0x0600162D RID: 5677
		public unsafe delegate int GetUInt8Array_20(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002E3 RID: 739
		// (Invoke) Token: 0x06001631 RID: 5681
		public unsafe delegate int GetInt16Array_21(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002E4 RID: 740
		// (Invoke) Token: 0x06001635 RID: 5685
		public unsafe delegate int GetUInt16Array_22(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002E5 RID: 741
		// (Invoke) Token: 0x06001639 RID: 5689
		public unsafe delegate int GetInt32Array_23(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002E6 RID: 742
		// (Invoke) Token: 0x0600163D RID: 5693
		public unsafe delegate int GetUInt32Array_24(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002E7 RID: 743
		// (Invoke) Token: 0x06001641 RID: 5697
		public unsafe delegate int GetInt64Array_25(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002E8 RID: 744
		// (Invoke) Token: 0x06001645 RID: 5701
		public unsafe delegate int GetUInt64Array_26(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002E9 RID: 745
		// (Invoke) Token: 0x06001649 RID: 5705
		public unsafe delegate int GetSingleArray_27(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002EA RID: 746
		// (Invoke) Token: 0x0600164D RID: 5709
		public unsafe delegate int GetDoubleArray_28(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002EB RID: 747
		// (Invoke) Token: 0x06001651 RID: 5713
		public unsafe delegate int GetChar16Array_29(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002EC RID: 748
		// (Invoke) Token: 0x06001655 RID: 5717
		public unsafe delegate int GetBooleanArray_30(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002ED RID: 749
		// (Invoke) Token: 0x06001659 RID: 5721
		public unsafe delegate int GetStringArray_31(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002EE RID: 750
		// (Invoke) Token: 0x0600165D RID: 5725
		public unsafe delegate int GetInspectableArray_32(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002EF RID: 751
		// (Invoke) Token: 0x06001661 RID: 5729
		public unsafe delegate int GetGuidArray_33(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002F0 RID: 752
		// (Invoke) Token: 0x06001665 RID: 5733
		public unsafe delegate int GetDateTimeArray_34(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002F1 RID: 753
		// (Invoke) Token: 0x06001669 RID: 5737
		public unsafe delegate int GetTimeSpanArray_35(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002F2 RID: 754
		// (Invoke) Token: 0x0600166D RID: 5741
		public unsafe delegate int GetPointArray_36(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002F3 RID: 755
		// (Invoke) Token: 0x06001671 RID: 5745
		public unsafe delegate int GetSizeArray_37(IntPtr thisPtr, int* __valueSize, IntPtr* value);

		// Token: 0x020002F4 RID: 756
		// (Invoke) Token: 0x06001675 RID: 5749
		public unsafe delegate int GetRectArray_38(IntPtr thisPtr, int* __valueSize, IntPtr* value);
	}
}
