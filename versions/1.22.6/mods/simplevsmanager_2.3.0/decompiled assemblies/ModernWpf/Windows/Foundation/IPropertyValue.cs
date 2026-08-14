using System;
using System.Runtime.InteropServices;

namespace Windows.Foundation
{
	// Token: 0x0200002E RID: 46
	[Guid("4BD682DD-7554-40E9-9A9B-82654EDE7E62")]
	internal interface IPropertyValue
	{
		// Token: 0x060002C3 RID: 707
		byte GetUInt8();

		// Token: 0x060002C4 RID: 708
		short GetInt16();

		// Token: 0x060002C5 RID: 709
		ushort GetUInt16();

		// Token: 0x060002C6 RID: 710
		int GetInt32();

		// Token: 0x060002C7 RID: 711
		uint GetUInt32();

		// Token: 0x060002C8 RID: 712
		long GetInt64();

		// Token: 0x060002C9 RID: 713
		ulong GetUInt64();

		// Token: 0x060002CA RID: 714
		float GetSingle();

		// Token: 0x060002CB RID: 715
		double GetDouble();

		// Token: 0x060002CC RID: 716
		char GetChar16();

		// Token: 0x060002CD RID: 717
		bool GetBoolean();

		// Token: 0x060002CE RID: 718
		string GetString();

		// Token: 0x060002CF RID: 719
		Guid GetGuid();

		// Token: 0x060002D0 RID: 720
		DateTimeOffset GetDateTime();

		// Token: 0x060002D1 RID: 721
		TimeSpan GetTimeSpan();

		// Token: 0x060002D2 RID: 722
		Point GetPoint();

		// Token: 0x060002D3 RID: 723
		Size GetSize();

		// Token: 0x060002D4 RID: 724
		Rect GetRect();

		// Token: 0x060002D5 RID: 725
		void GetUInt8Array(out byte[] value);

		// Token: 0x060002D6 RID: 726
		void GetInt16Array(out short[] value);

		// Token: 0x060002D7 RID: 727
		void GetUInt16Array(out ushort[] value);

		// Token: 0x060002D8 RID: 728
		void GetInt32Array(out int[] value);

		// Token: 0x060002D9 RID: 729
		void GetUInt32Array(out uint[] value);

		// Token: 0x060002DA RID: 730
		void GetInt64Array(out long[] value);

		// Token: 0x060002DB RID: 731
		void GetUInt64Array(out ulong[] value);

		// Token: 0x060002DC RID: 732
		void GetSingleArray(out float[] value);

		// Token: 0x060002DD RID: 733
		void GetDoubleArray(out double[] value);

		// Token: 0x060002DE RID: 734
		void GetChar16Array(out char[] value);

		// Token: 0x060002DF RID: 735
		void GetBooleanArray(out bool[] value);

		// Token: 0x060002E0 RID: 736
		void GetStringArray(out string[] value);

		// Token: 0x060002E1 RID: 737
		void GetInspectableArray(out object[] value);

		// Token: 0x060002E2 RID: 738
		void GetGuidArray(out Guid[] value);

		// Token: 0x060002E3 RID: 739
		void GetDateTimeArray(out DateTimeOffset[] value);

		// Token: 0x060002E4 RID: 740
		void GetTimeSpanArray(out TimeSpan[] value);

		// Token: 0x060002E5 RID: 741
		void GetPointArray(out Point[] value);

		// Token: 0x060002E6 RID: 742
		void GetSizeArray(out Size[] value);

		// Token: 0x060002E7 RID: 743
		void GetRectArray(out Rect[] value);

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060002E8 RID: 744
		bool IsNumericScalar { get; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060002E9 RID: 745
		PropertyType Type { get; }
	}
}
