using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ABI.System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000D9 RID: 217
	[Guid("629BDBC8-D932-4FF4-96B9-8D96C5C1E858")]
	internal interface IPropertyValueStatics : Windows.Foundation.IPropertyValueStatics
	{
		// Token: 0x060007FC RID: 2044 RVA: 0x0001557C File Offset: 0x0001377C
		unsafe static IPropertyValueStatics()
		{
			*(IInspectable.Vftbl*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateEmpty_0);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateUInt8_1);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)8 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateInt16_2);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)9 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateUInt16_3);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)10 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateInt32_4);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)11 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateUInt32_5);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)12 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateInt64_6);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)13 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateUInt64_7);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)14 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateSingle_8);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)15 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateDouble_9);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)16 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateChar16_10);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)17 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateBoolean_11);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)18 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateString_12);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)19 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateInspectable_13);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)20 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateGuid_14);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)21 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateDateTime_15);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)22 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateTimeSpan_16);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)23 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreatePoint_17);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)24 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateSize_18);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)25 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateRect_19);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)26 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateUInt8Array_20);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)27 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateInt16Array_21);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)28 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateUInt16Array_22);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)29 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateInt32Array_23);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)30 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateUInt32Array_24);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)31 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateInt64Array_25);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)32 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateUInt64Array_26);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)33 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateSingleArray_27);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)34 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateDoubleArray_28);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)35 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateChar16Array_29);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)36 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateBooleanArray_30);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)37 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateStringArray_31);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)38 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateInspectableArray_32);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)39 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateGuidArray_33);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)40 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateDateTimeArray_34);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)41 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateTimeSpanArray_35);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)42 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreatePointArray_36);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)43 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateSizeArray_37);
			*(IntPtr*)((byte*)((void*)IPropertyValueStatics.AbiToProjectionVftablePtr) + (IntPtr)44 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateRectArray_38);
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00015A04 File Offset: 0x00013C04
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateEmpty_0(IntPtr thisPtr, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateEmpty();
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00015A54 File Offset: 0x00013C54
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateUInt8_1(IntPtr thisPtr, byte value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateUInt8(value);
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00015AA4 File Offset: 0x00013CA4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateInt16_2(IntPtr thisPtr, short value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateInt16(value);
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00015AF4 File Offset: 0x00013CF4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateUInt16_3(IntPtr thisPtr, ushort value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateUInt16(value);
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00015B44 File Offset: 0x00013D44
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateInt32_4(IntPtr thisPtr, int value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateInt32(value);
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x00015B94 File Offset: 0x00013D94
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateUInt32_5(IntPtr thisPtr, uint value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateUInt32(value);
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00015BE4 File Offset: 0x00013DE4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateInt64_6(IntPtr thisPtr, long value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateInt64(value);
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00015C34 File Offset: 0x00013E34
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateUInt64_7(IntPtr thisPtr, ulong value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateUInt64(value);
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00015C84 File Offset: 0x00013E84
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateSingle_8(IntPtr thisPtr, float value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateSingle(value);
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00015CD4 File Offset: 0x00013ED4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateDouble_9(IntPtr thisPtr, double value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateDouble(value);
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00015D24 File Offset: 0x00013F24
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateChar16_10(IntPtr thisPtr, ushort value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateChar16((char)value);
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00015D74 File Offset: 0x00013F74
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateBoolean_11(IntPtr thisPtr, byte value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateBoolean(value > 0);
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00015DC8 File Offset: 0x00013FC8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateString_12(IntPtr thisPtr, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateString(MarshalString.FromAbi(value));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00015E1C File Offset: 0x0001401C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateInspectable_13(IntPtr thisPtr, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateInspectable(MarshalInspectable<object>.FromAbi(value));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00015E70 File Offset: 0x00014070
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateGuid_14(IntPtr thisPtr, Guid value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateGuid(value);
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00015EC0 File Offset: 0x000140C0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateDateTime_15(IntPtr thisPtr, DateTimeOffset value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateDateTime(DateTimeOffset.FromAbi(value));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00015F14 File Offset: 0x00014114
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateTimeSpan_16(IntPtr thisPtr, TimeSpan value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateTimeSpan(TimeSpan.FromAbi(value));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00015F68 File Offset: 0x00014168
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreatePoint_17(IntPtr thisPtr, Windows.Foundation.Point value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreatePoint(value);
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00015FB8 File Offset: 0x000141B8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateSize_18(IntPtr thisPtr, Windows.Foundation.Size value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateSize(value);
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00016008 File Offset: 0x00014208
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateRect_19(IntPtr thisPtr, Windows.Foundation.Rect value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateRect(value);
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00016058 File Offset: 0x00014258
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateUInt8Array_20(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateUInt8Array(MarshalBlittable<byte>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x000160B8 File Offset: 0x000142B8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateInt16Array_21(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateInt16Array(MarshalBlittable<short>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00016118 File Offset: 0x00014318
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateUInt16Array_22(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateUInt16Array(MarshalBlittable<ushort>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x00016178 File Offset: 0x00014378
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateInt32Array_23(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateInt32Array(MarshalBlittable<int>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x000161D8 File Offset: 0x000143D8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateUInt32Array_24(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateUInt32Array(MarshalBlittable<uint>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00016238 File Offset: 0x00014438
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateInt64Array_25(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateInt64Array(MarshalBlittable<long>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00016298 File Offset: 0x00014498
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateUInt64Array_26(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateUInt64Array(MarshalBlittable<ulong>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x000162F8 File Offset: 0x000144F8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateSingleArray_27(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateSingleArray(MarshalBlittable<float>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x00016358 File Offset: 0x00014558
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateDoubleArray_28(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateDoubleArray(MarshalBlittable<double>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x000163B8 File Offset: 0x000145B8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateChar16Array_29(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateChar16Array(MarshalNonBlittable<char>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00016418 File Offset: 0x00014618
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateBooleanArray_30(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateBooleanArray(MarshalNonBlittable<bool>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00016478 File Offset: 0x00014678
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateStringArray_31(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateStringArray(MarshalString.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x000164D8 File Offset: 0x000146D8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateInspectableArray_32(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateInspectableArray(MarshalInspectable<object>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00016538 File Offset: 0x00014738
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateGuidArray_33(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateGuidArray(MarshalBlittable<Guid>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00016598 File Offset: 0x00014798
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateDateTimeArray_34(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateDateTimeArray(MarshalNonBlittable<DateTimeOffset>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x000165F8 File Offset: 0x000147F8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateTimeSpanArray_35(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateTimeSpanArray(MarshalNonBlittable<TimeSpan>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00016658 File Offset: 0x00014858
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreatePointArray_36(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreatePointArray(MarshalBlittable<Windows.Foundation.Point>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x000166B8 File Offset: 0x000148B8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateSizeArray_37(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateSizeArray(MarshalBlittable<Windows.Foundation.Size>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00016718 File Offset: 0x00014918
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateRectArray_38(IntPtr thisPtr, int __valueSize, IntPtr value, IntPtr* propertyValue)
		{
			*propertyValue = default(IntPtr);
			try
			{
				object obj = ComWrappersSupport.FindObject<Windows.Foundation.IPropertyValueStatics>(thisPtr).CreateRectArray(MarshalBlittable<Windows.Foundation.Rect>.FromAbiArray(new ValueTuple<int, IntPtr>(__valueSize, value)));
				*propertyValue = MarshalInspectable<object>.FromManaged(obj, true);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x04000185 RID: 389
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IPropertyValueStatics), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 39);
	}
}
