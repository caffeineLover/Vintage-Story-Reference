using System;
using System.Runtime.Versioning;
using System.Threading;
using ABI.Windows.Foundation;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000051 RID: 81
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal static class PropertyValue
	{
		// Token: 0x060003A0 RID: 928 RVA: 0x00005BB6 File Offset: 0x00003DB6
		public static I As<I>()
		{
			return PropertyValue._factory.AsInterface<I>();
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00005BC2 File Offset: 0x00003DC2
		private static IObjectReference Make___objRef_global__Windows_Foundation_IPropertyValueStatics()
		{
			Interlocked.CompareExchange<IObjectReference>(ref PropertyValue.___objRef_global__Windows_Foundation_IPropertyValueStatics, PropertyValue._factory._As(GuidGenerator.GetIID(typeof(IPropertyValueStatics).GetHelperType())), null);
			return PropertyValue.___objRef_global__Windows_Foundation_IPropertyValueStatics;
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00005BF5 File Offset: 0x00003DF5
		private static IObjectReference _objRef_global__Windows_Foundation_IPropertyValueStatics
		{
			get
			{
				return PropertyValue.___objRef_global__Windows_Foundation_IPropertyValueStatics ?? PropertyValue.Make___objRef_global__Windows_Foundation_IPropertyValueStatics();
			}
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00005C07 File Offset: 0x00003E07
		public static object CreateEmpty()
		{
			return IPropertyValueStaticsMethods.CreateEmpty(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00005C13 File Offset: 0x00003E13
		public static object CreateUInt8(byte value)
		{
			return IPropertyValueStaticsMethods.CreateUInt8(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00005C20 File Offset: 0x00003E20
		public static object CreateInt16(short value)
		{
			return IPropertyValueStaticsMethods.CreateInt16(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00005C2D File Offset: 0x00003E2D
		public static object CreateUInt16(ushort value)
		{
			return IPropertyValueStaticsMethods.CreateUInt16(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00005C3A File Offset: 0x00003E3A
		public static object CreateInt32(int value)
		{
			return IPropertyValueStaticsMethods.CreateInt32(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00005C47 File Offset: 0x00003E47
		public static object CreateUInt32(uint value)
		{
			return IPropertyValueStaticsMethods.CreateUInt32(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00005C54 File Offset: 0x00003E54
		public static object CreateInt64(long value)
		{
			return IPropertyValueStaticsMethods.CreateInt64(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00005C61 File Offset: 0x00003E61
		public static object CreateUInt64(ulong value)
		{
			return IPropertyValueStaticsMethods.CreateUInt64(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00005C6E File Offset: 0x00003E6E
		public static object CreateSingle(float value)
		{
			return IPropertyValueStaticsMethods.CreateSingle(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00005C7B File Offset: 0x00003E7B
		public static object CreateDouble(double value)
		{
			return IPropertyValueStaticsMethods.CreateDouble(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00005C88 File Offset: 0x00003E88
		public static object CreateChar16(char value)
		{
			return IPropertyValueStaticsMethods.CreateChar16(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00005C95 File Offset: 0x00003E95
		public static object CreateBoolean(bool value)
		{
			return IPropertyValueStaticsMethods.CreateBoolean(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00005CA2 File Offset: 0x00003EA2
		public static object CreateString(string value)
		{
			return IPropertyValueStaticsMethods.CreateString(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00005CAF File Offset: 0x00003EAF
		public static object CreateInspectable(object value)
		{
			return IPropertyValueStaticsMethods.CreateInspectable(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00005CBC File Offset: 0x00003EBC
		public static object CreateGuid(Guid value)
		{
			return IPropertyValueStaticsMethods.CreateGuid(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00005CC9 File Offset: 0x00003EC9
		public static object CreateDateTime(DateTimeOffset value)
		{
			return IPropertyValueStaticsMethods.CreateDateTime(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00005CD6 File Offset: 0x00003ED6
		public static object CreateTimeSpan(TimeSpan value)
		{
			return IPropertyValueStaticsMethods.CreateTimeSpan(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00005CE3 File Offset: 0x00003EE3
		public static object CreatePoint(Point value)
		{
			return IPropertyValueStaticsMethods.CreatePoint(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00005CF0 File Offset: 0x00003EF0
		public static object CreateSize(Size value)
		{
			return IPropertyValueStaticsMethods.CreateSize(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00005CFD File Offset: 0x00003EFD
		public static object CreateRect(Rect value)
		{
			return IPropertyValueStaticsMethods.CreateRect(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00005D0A File Offset: 0x00003F0A
		public static object CreateUInt8Array(byte[] value)
		{
			return IPropertyValueStaticsMethods.CreateUInt8Array(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00005D17 File Offset: 0x00003F17
		public static object CreateInt16Array(short[] value)
		{
			return IPropertyValueStaticsMethods.CreateInt16Array(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00005D24 File Offset: 0x00003F24
		public static object CreateUInt16Array(ushort[] value)
		{
			return IPropertyValueStaticsMethods.CreateUInt16Array(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00005D31 File Offset: 0x00003F31
		public static object CreateInt32Array(int[] value)
		{
			return IPropertyValueStaticsMethods.CreateInt32Array(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00005D3E File Offset: 0x00003F3E
		public static object CreateUInt32Array(uint[] value)
		{
			return IPropertyValueStaticsMethods.CreateUInt32Array(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00005D4B File Offset: 0x00003F4B
		public static object CreateInt64Array(long[] value)
		{
			return IPropertyValueStaticsMethods.CreateInt64Array(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00005D58 File Offset: 0x00003F58
		public static object CreateUInt64Array(ulong[] value)
		{
			return IPropertyValueStaticsMethods.CreateUInt64Array(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00005D65 File Offset: 0x00003F65
		public static object CreateSingleArray(float[] value)
		{
			return IPropertyValueStaticsMethods.CreateSingleArray(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00005D72 File Offset: 0x00003F72
		public static object CreateDoubleArray(double[] value)
		{
			return IPropertyValueStaticsMethods.CreateDoubleArray(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00005D7F File Offset: 0x00003F7F
		public static object CreateChar16Array(char[] value)
		{
			return IPropertyValueStaticsMethods.CreateChar16Array(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00005D8C File Offset: 0x00003F8C
		public static object CreateBooleanArray(bool[] value)
		{
			return IPropertyValueStaticsMethods.CreateBooleanArray(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00005D99 File Offset: 0x00003F99
		public static object CreateStringArray(string[] value)
		{
			return IPropertyValueStaticsMethods.CreateStringArray(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00005DA6 File Offset: 0x00003FA6
		public static object CreateInspectableArray(object[] value)
		{
			return IPropertyValueStaticsMethods.CreateInspectableArray(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00005DB3 File Offset: 0x00003FB3
		public static object CreateGuidArray(Guid[] value)
		{
			return IPropertyValueStaticsMethods.CreateGuidArray(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00005DC0 File Offset: 0x00003FC0
		public static object CreateDateTimeArray(DateTimeOffset[] value)
		{
			return IPropertyValueStaticsMethods.CreateDateTimeArray(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00005DCD File Offset: 0x00003FCD
		public static object CreateTimeSpanArray(TimeSpan[] value)
		{
			return IPropertyValueStaticsMethods.CreateTimeSpanArray(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00005DDA File Offset: 0x00003FDA
		public static object CreatePointArray(Point[] value)
		{
			return IPropertyValueStaticsMethods.CreatePointArray(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00005DE7 File Offset: 0x00003FE7
		public static object CreateSizeArray(Size[] value)
		{
			return IPropertyValueStaticsMethods.CreateSizeArray(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00005DF4 File Offset: 0x00003FF4
		public static object CreateRectArray(Rect[] value)
		{
			return IPropertyValueStaticsMethods.CreateRectArray(PropertyValue._objRef_global__Windows_Foundation_IPropertyValueStatics, value);
		}

		// Token: 0x040000C4 RID: 196
		internal static BaseActivationFactory _factory = new BaseActivationFactory("Windows.Foundation", "Windows.Foundation.PropertyValue");

		// Token: 0x040000C5 RID: 197
		private static volatile IObjectReference ___objRef_global__Windows_Foundation_IPropertyValueStatics;
	}
}
