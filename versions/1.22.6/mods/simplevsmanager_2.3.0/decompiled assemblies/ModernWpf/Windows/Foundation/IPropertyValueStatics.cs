using System;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000047 RID: 71
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[Guid("629BDBC8-D932-4FF4-96B9-8D96C5C1E858")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal interface IPropertyValueStatics
	{
		// Token: 0x06000345 RID: 837
		object CreateEmpty();

		// Token: 0x06000346 RID: 838
		object CreateUInt8(byte value);

		// Token: 0x06000347 RID: 839
		object CreateInt16(short value);

		// Token: 0x06000348 RID: 840
		object CreateUInt16(ushort value);

		// Token: 0x06000349 RID: 841
		object CreateInt32(int value);

		// Token: 0x0600034A RID: 842
		object CreateUInt32(uint value);

		// Token: 0x0600034B RID: 843
		object CreateInt64(long value);

		// Token: 0x0600034C RID: 844
		object CreateUInt64(ulong value);

		// Token: 0x0600034D RID: 845
		object CreateSingle(float value);

		// Token: 0x0600034E RID: 846
		object CreateDouble(double value);

		// Token: 0x0600034F RID: 847
		object CreateChar16(char value);

		// Token: 0x06000350 RID: 848
		object CreateBoolean(bool value);

		// Token: 0x06000351 RID: 849
		object CreateString(string value);

		// Token: 0x06000352 RID: 850
		object CreateInspectable(object value);

		// Token: 0x06000353 RID: 851
		object CreateGuid(Guid value);

		// Token: 0x06000354 RID: 852
		object CreateDateTime(DateTimeOffset value);

		// Token: 0x06000355 RID: 853
		object CreateTimeSpan(TimeSpan value);

		// Token: 0x06000356 RID: 854
		object CreatePoint(Point value);

		// Token: 0x06000357 RID: 855
		object CreateSize(Size value);

		// Token: 0x06000358 RID: 856
		object CreateRect(Rect value);

		// Token: 0x06000359 RID: 857
		object CreateUInt8Array(byte[] value);

		// Token: 0x0600035A RID: 858
		object CreateInt16Array(short[] value);

		// Token: 0x0600035B RID: 859
		object CreateUInt16Array(ushort[] value);

		// Token: 0x0600035C RID: 860
		object CreateInt32Array(int[] value);

		// Token: 0x0600035D RID: 861
		object CreateUInt32Array(uint[] value);

		// Token: 0x0600035E RID: 862
		object CreateInt64Array(long[] value);

		// Token: 0x0600035F RID: 863
		object CreateUInt64Array(ulong[] value);

		// Token: 0x06000360 RID: 864
		object CreateSingleArray(float[] value);

		// Token: 0x06000361 RID: 865
		object CreateDoubleArray(double[] value);

		// Token: 0x06000362 RID: 866
		object CreateChar16Array(char[] value);

		// Token: 0x06000363 RID: 867
		object CreateBooleanArray(bool[] value);

		// Token: 0x06000364 RID: 868
		object CreateStringArray(string[] value);

		// Token: 0x06000365 RID: 869
		object CreateInspectableArray(object[] value);

		// Token: 0x06000366 RID: 870
		object CreateGuidArray(Guid[] value);

		// Token: 0x06000367 RID: 871
		object CreateDateTimeArray(DateTimeOffset[] value);

		// Token: 0x06000368 RID: 872
		object CreateTimeSpanArray(TimeSpan[] value);

		// Token: 0x06000369 RID: 873
		object CreatePointArray(Point[] value);

		// Token: 0x0600036A RID: 874
		object CreateSizeArray(Size[] value);

		// Token: 0x0600036B RID: 875
		object CreateRectArray(Rect[] value);
	}
}
