using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ABI.System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000AC RID: 172
	internal static class ManagedIPropertyValueImpl
	{
		// Token: 0x06000698 RID: 1688 RVA: 0x0000ECD8 File Offset: 0x0000CED8
		unsafe static ManagedIPropertyValueImpl()
		{
			ManagedIPropertyValueImpl.AbiToProjectionVftable = new IPropertyValue.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_get_Type_0 = ldftn(Do_Abi_get_Type_0),
				_get_IsNumericScalar_1 = ldftn(Do_Abi_get_IsNumericScalar_1),
				_GetUInt8_2 = ldftn(Do_Abi_GetUInt8_2),
				_GetInt16_3 = ldftn(Do_Abi_GetInt16_3),
				_GetUInt16_4 = ldftn(Do_Abi_GetUInt16_4),
				_GetInt32_5 = ldftn(Do_Abi_GetInt32_5),
				_GetUInt32_6 = ldftn(Do_Abi_GetUInt32_6),
				_GetInt64_7 = ldftn(Do_Abi_GetInt64_7),
				_GetUInt64_8 = ldftn(Do_Abi_GetUInt64_8),
				_GetSingle_9 = ldftn(Do_Abi_GetSingle_9),
				_GetDouble_10 = ldftn(Do_Abi_GetDouble_10),
				_GetChar16_11 = ldftn(Do_Abi_GetChar16_11),
				_GetBoolean_12 = ldftn(Do_Abi_GetBoolean_12),
				_GetString_13 = ldftn(Do_Abi_GetString_13),
				_GetGuid_14 = ldftn(Do_Abi_GetGuid_14),
				_GetDateTime_15 = ldftn(Do_Abi_GetDateTime_15),
				_GetTimeSpan_16 = ldftn(Do_Abi_GetTimeSpan_16),
				_GetPoint_17 = ldftn(Do_Abi_GetPoint_17),
				_GetSize_18 = ldftn(Do_Abi_GetSize_18),
				_GetRect_19 = ldftn(Do_Abi_GetRect_19),
				_GetUInt8Array_20 = ldftn(Do_Abi_GetUInt8Array_20),
				_GetInt16Array_21 = ldftn(Do_Abi_GetInt16Array_21),
				_GetUInt16Array_22 = ldftn(Do_Abi_GetUInt16Array_22),
				_GetInt32Array_23 = ldftn(Do_Abi_GetInt32Array_23),
				_GetUInt32Array_24 = ldftn(Do_Abi_GetUInt32Array_24),
				_GetInt64Array_25 = ldftn(Do_Abi_GetInt64Array_25),
				_GetUInt64Array_26 = ldftn(Do_Abi_GetUInt64Array_26),
				_GetSingleArray_27 = ldftn(Do_Abi_GetSingleArray_27),
				_GetDoubleArray_28 = ldftn(Do_Abi_GetDoubleArray_28),
				_GetChar16Array_29 = ldftn(Do_Abi_GetChar16Array_29),
				_GetBooleanArray_30 = ldftn(Do_Abi_GetBooleanArray_30),
				_GetStringArray_31 = ldftn(Do_Abi_GetStringArray_31),
				_GetInspectableArray_32 = ldftn(Do_Abi_GetInspectableArray_32),
				_GetGuidArray_33 = ldftn(Do_Abi_GetGuidArray_33),
				_GetDateTimeArray_34 = ldftn(Do_Abi_GetDateTimeArray_34),
				_GetTimeSpanArray_35 = ldftn(Do_Abi_GetTimeSpanArray_35),
				_GetPointArray_36 = ldftn(Do_Abi_GetPointArray_36),
				_GetSizeArray_37 = ldftn(Do_Abi_GetSizeArray_37),
				_GetRectArray_38 = ldftn(Do_Abi_GetRectArray_38)
			};
			IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(ManagedIPropertyValueImpl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 39));
			Marshal.StructureToPtr<IPropertyValue.Vftbl>(ManagedIPropertyValueImpl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
			ManagedIPropertyValueImpl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0000EF70 File Offset: 0x0000D170
		private static Dictionary<Type, PropertyType> NumericScalarTypes
		{
			get
			{
				if (ManagedIPropertyValueImpl.s_numericScalarTypes == null)
				{
					Dictionary<Type, PropertyType> dictionary = new Dictionary<Type, PropertyType>();
					dictionary.Add(typeof(byte), PropertyType.UInt8);
					dictionary.Add(typeof(short), PropertyType.Int16);
					dictionary.Add(typeof(ushort), PropertyType.UInt16);
					dictionary.Add(typeof(int), PropertyType.Int32);
					dictionary.Add(typeof(uint), PropertyType.UInt32);
					dictionary.Add(typeof(long), PropertyType.Int64);
					dictionary.Add(typeof(ulong), PropertyType.UInt64);
					dictionary.Add(typeof(float), PropertyType.Single);
					dictionary.Add(typeof(double), PropertyType.Double);
					ManagedIPropertyValueImpl.s_numericScalarTypes = dictionary;
				}
				return ManagedIPropertyValueImpl.s_numericScalarTypes;
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0000F038 File Offset: 0x0000D238
		private static T UnboxValue<T>(object value) where T : struct
		{
			Type type = value.GetType();
			if (type.FullName == typeof(T).FullName && Marshal.SizeOf(type) == Marshal.SizeOf<T>())
			{
				return Unsafe.As<ManagedIPropertyValueImpl.Boxed<T>>(value).Value;
			}
			throw new InvalidCastException("", -2147316576);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0000F090 File Offset: 0x0000D290
		private static T[] UnboxArray<T>(object value) where T : struct
		{
			Array array = value as Array;
			if (array == null)
			{
				throw new InvalidCastException();
			}
			T[] array2 = new T[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				try
				{
					array2[i] = ManagedIPropertyValueImpl.UnboxValue<T>(array.GetValue(i));
				}
				catch (InvalidCastException ex)
				{
					throw new InvalidCastException("", ex.HResult);
				}
			}
			return array2;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0000F104 File Offset: 0x0000D304
		private static bool IsCoercable(object value)
		{
			PropertyType propertyType;
			return value.GetType() == typeof(string) || value.GetType() == typeof(Guid) || value.GetType() != typeof(object) || ManagedIPropertyValueImpl.NumericScalarTypes.TryGetValue(value.GetType(), ref propertyType);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0000F16C File Offset: 0x0000D36C
		private static T CoerceValue<T>(object value)
		{
			if (value is T)
			{
				return (T)((object)value);
			}
			Windows.Foundation.IPropertyValue propertyValue = value as Windows.Foundation.IPropertyValue;
			if (propertyValue != null)
			{
				if (typeof(T) == typeof(byte))
				{
					return (T)((object)propertyValue.GetUInt8());
				}
				if (typeof(T) == typeof(short))
				{
					return (T)((object)propertyValue.GetInt16());
				}
				if (typeof(T) == typeof(ushort))
				{
					return (T)((object)propertyValue.GetUInt16());
				}
				if (typeof(T) == typeof(int))
				{
					return (T)((object)propertyValue.GetInt32());
				}
				if (typeof(T) == typeof(uint))
				{
					return (T)((object)propertyValue.GetUInt32());
				}
				if (typeof(T) == typeof(long))
				{
					return (T)((object)propertyValue.GetInt64());
				}
				if (typeof(T) == typeof(ulong))
				{
					return (T)((object)propertyValue.GetUInt64());
				}
				if (typeof(T) == typeof(float))
				{
					return (T)((object)propertyValue.GetSingle());
				}
				if (typeof(T) == typeof(double))
				{
					return (T)((object)propertyValue.GetDouble());
				}
			}
			if (!ManagedIPropertyValueImpl.IsCoercable(value))
			{
				throw new InvalidCastException();
			}
			try
			{
				string text = value as string;
				if (text != null && typeof(T) == typeof(Guid))
				{
					return (T)((object)Guid.Parse(text));
				}
				if (value is Guid)
				{
					Guid guid = (Guid)value;
					if (typeof(T) == typeof(string))
					{
						return (T)((object)guid.ToString("D", CultureInfo.InvariantCulture));
					}
				}
				PropertyType propertyType;
				if (ManagedIPropertyValueImpl.NumericScalarTypes.TryGetValue(typeof(T), ref propertyType))
				{
					return (T)((object)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture));
				}
			}
			catch (FormatException)
			{
				throw new InvalidCastException("", -2147316576);
			}
			catch (InvalidCastException)
			{
				throw new InvalidCastException("", -2147316576);
			}
			catch (OverflowException)
			{
				throw new InvalidCastException("", -2147352566);
			}
			throw new InvalidCastException();
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x0000F44C File Offset: 0x0000D64C
		private static T[] CoerceArray<T>(object value)
		{
			T[] array = value as T[];
			if (array != null)
			{
				return array;
			}
			Array array2 = value as Array;
			if (array2 == null)
			{
				throw new InvalidCastException();
			}
			T[] array3 = new T[array2.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				try
				{
					array3[i] = ManagedIPropertyValueImpl.CoerceValue<T>(array2.GetValue(i));
				}
				catch (InvalidCastException ex)
				{
					throw new InvalidCastException("", ex.HResult);
				}
			}
			return array3;
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0000F4D0 File Offset: 0x0000D6D0
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetUInt8_2(IntPtr thisPtr, byte* value)
		{
			try
			{
				*value = ManagedIPropertyValueImpl.CoerceValue<byte>(ComWrappersSupport.FindObject<object>(thisPtr));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0000F510 File Offset: 0x0000D710
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetInt16_3(IntPtr thisPtr, short* value)
		{
			try
			{
				*value = ManagedIPropertyValueImpl.CoerceValue<short>(ComWrappersSupport.FindObject<object>(thisPtr));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0000F550 File Offset: 0x0000D750
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetUInt16_4(IntPtr thisPtr, ushort* value)
		{
			try
			{
				*value = ManagedIPropertyValueImpl.CoerceValue<ushort>(ComWrappersSupport.FindObject<object>(thisPtr));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0000F590 File Offset: 0x0000D790
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetInt32_5(IntPtr thisPtr, int* value)
		{
			try
			{
				*value = ManagedIPropertyValueImpl.CoerceValue<int>(ComWrappersSupport.FindObject<object>(thisPtr));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0000F5D0 File Offset: 0x0000D7D0
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetUInt32_6(IntPtr thisPtr, uint* value)
		{
			try
			{
				*value = ManagedIPropertyValueImpl.CoerceValue<uint>(ComWrappersSupport.FindObject<object>(thisPtr));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0000F610 File Offset: 0x0000D810
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetInt64_7(IntPtr thisPtr, long* value)
		{
			try
			{
				*value = ManagedIPropertyValueImpl.CoerceValue<long>(ComWrappersSupport.FindObject<object>(thisPtr));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0000F650 File Offset: 0x0000D850
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetUInt64_8(IntPtr thisPtr, ulong* value)
		{
			try
			{
				*value = ManagedIPropertyValueImpl.CoerceValue<ulong>(ComWrappersSupport.FindObject<object>(thisPtr));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0000F690 File Offset: 0x0000D890
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetSingle_9(IntPtr thisPtr, float* value)
		{
			try
			{
				*value = ManagedIPropertyValueImpl.CoerceValue<float>(ComWrappersSupport.FindObject<object>(thisPtr));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0000F6D0 File Offset: 0x0000D8D0
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetDouble_10(IntPtr thisPtr, double* value)
		{
			try
			{
				*value = ManagedIPropertyValueImpl.CoerceValue<double>(ComWrappersSupport.FindObject<object>(thisPtr));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0000F710 File Offset: 0x0000D910
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetChar16_11(IntPtr thisPtr, ushort* value)
		{
			try
			{
				*value = (ushort)ManagedIPropertyValueImpl.CoerceValue<char>(ComWrappersSupport.FindObject<object>(thisPtr));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0000F750 File Offset: 0x0000D950
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetBoolean_12(IntPtr thisPtr, byte* value)
		{
			try
			{
				*value = (ManagedIPropertyValueImpl.CoerceValue<bool>(ComWrappersSupport.FindObject<object>(thisPtr)) ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0000F794 File Offset: 0x0000D994
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetString_13(IntPtr thisPtr, IntPtr* value)
		{
			try
			{
				*value = MarshalString.FromManaged(ManagedIPropertyValueImpl.CoerceValue<string>(ComWrappersSupport.FindObject<object>(thisPtr)));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0000F7D8 File Offset: 0x0000D9D8
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetGuid_14(IntPtr thisPtr, Guid* value)
		{
			try
			{
				*value = ManagedIPropertyValueImpl.CoerceValue<Guid>(ComWrappersSupport.FindObject<object>(thisPtr));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0000F81C File Offset: 0x0000DA1C
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetDateTime_15(IntPtr thisPtr, DateTimeOffset* value)
		{
			try
			{
				*value = DateTimeOffset.FromManaged(ManagedIPropertyValueImpl.CoerceValue<DateTimeOffset>(ComWrappersSupport.FindObject<object>(thisPtr)));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0000F864 File Offset: 0x0000DA64
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetTimeSpan_16(IntPtr thisPtr, TimeSpan* value)
		{
			try
			{
				*value = TimeSpan.FromManaged(ManagedIPropertyValueImpl.CoerceValue<TimeSpan>(ComWrappersSupport.FindObject<object>(thisPtr)));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0000F8AC File Offset: 0x0000DAAC
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetPoint_17(IntPtr thisPtr, Windows.Foundation.Point* value)
		{
			try
			{
				*value = ManagedIPropertyValueImpl.UnboxValue<Windows.Foundation.Point>(ComWrappersSupport.FindObject<object>(thisPtr));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0000F8F0 File Offset: 0x0000DAF0
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetSize_18(IntPtr thisPtr, Windows.Foundation.Size* value)
		{
			try
			{
				*value = ManagedIPropertyValueImpl.UnboxValue<Windows.Foundation.Size>(ComWrappersSupport.FindObject<object>(thisPtr));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0000F934 File Offset: 0x0000DB34
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetRect_19(IntPtr thisPtr, Windows.Foundation.Rect* value)
		{
			try
			{
				*value = ManagedIPropertyValueImpl.UnboxValue<Windows.Foundation.Rect>(ComWrappersSupport.FindObject<object>(thisPtr));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0000F978 File Offset: 0x0000DB78
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetUInt8Array_20(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				byte[] array = ManagedIPropertyValueImpl.CoerceArray<byte>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalBlittable<byte>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0000F9D0 File Offset: 0x0000DBD0
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetInt16Array_21(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				short[] array = ManagedIPropertyValueImpl.CoerceArray<short>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalBlittable<short>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0000FA28 File Offset: 0x0000DC28
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetUInt16Array_22(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				ushort[] array = ManagedIPropertyValueImpl.CoerceArray<ushort>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalBlittable<ushort>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0000FA80 File Offset: 0x0000DC80
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetInt32Array_23(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				int[] array = ManagedIPropertyValueImpl.CoerceArray<int>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalBlittable<int>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0000FAD8 File Offset: 0x0000DCD8
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetUInt32Array_24(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				uint[] array = ManagedIPropertyValueImpl.CoerceArray<uint>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalBlittable<uint>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0000FB30 File Offset: 0x0000DD30
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetInt64Array_25(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				long[] array = ManagedIPropertyValueImpl.CoerceArray<long>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalBlittable<long>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0000FB88 File Offset: 0x0000DD88
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetUInt64Array_26(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				ulong[] array = ManagedIPropertyValueImpl.CoerceArray<ulong>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalBlittable<ulong>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0000FBE0 File Offset: 0x0000DDE0
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetSingleArray_27(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				float[] array = ManagedIPropertyValueImpl.CoerceArray<float>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalBlittable<float>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0000FC38 File Offset: 0x0000DE38
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetDoubleArray_28(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				double[] array = ManagedIPropertyValueImpl.CoerceArray<double>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalBlittable<double>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0000FC90 File Offset: 0x0000DE90
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetChar16Array_29(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				char[] array = ManagedIPropertyValueImpl.CoerceArray<char>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalNonBlittable<char>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x0000FCE8 File Offset: 0x0000DEE8
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetBooleanArray_30(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				bool[] array = ManagedIPropertyValueImpl.CoerceArray<bool>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalNonBlittable<bool>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x0000FD40 File Offset: 0x0000DF40
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetStringArray_31(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				string[] array = ManagedIPropertyValueImpl.CoerceArray<string>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalString.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x0000FD98 File Offset: 0x0000DF98
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetInspectableArray_32(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				object[] array = ManagedIPropertyValueImpl.CoerceArray<object>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalInspectable<object>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0000FDF0 File Offset: 0x0000DFF0
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetGuidArray_33(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				Guid[] array = ManagedIPropertyValueImpl.CoerceArray<Guid>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalBlittable<Guid>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x0000FE48 File Offset: 0x0000E048
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetDateTimeArray_34(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				DateTimeOffset[] array = ManagedIPropertyValueImpl.CoerceArray<DateTimeOffset>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalNonBlittable<DateTimeOffset>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0000FEA0 File Offset: 0x0000E0A0
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetTimeSpanArray_35(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				TimeSpan[] array = ManagedIPropertyValueImpl.CoerceArray<TimeSpan>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalNonBlittable<TimeSpan>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0000FEF8 File Offset: 0x0000E0F8
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetPointArray_36(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				Windows.Foundation.Point[] array = ManagedIPropertyValueImpl.UnboxArray<Windows.Foundation.Point>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalBlittable<Windows.Foundation.Point>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0000FF50 File Offset: 0x0000E150
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetSizeArray_37(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				Windows.Foundation.Size[] array = ManagedIPropertyValueImpl.UnboxArray<Windows.Foundation.Size>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalBlittable<Windows.Foundation.Size>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0000FFA8 File Offset: 0x0000E1A8
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetRectArray_38(IntPtr thisPtr, int* __valueSize, IntPtr* value)
		{
			try
			{
				Windows.Foundation.Rect[] array = ManagedIPropertyValueImpl.UnboxArray<Windows.Foundation.Rect>(ComWrappersSupport.FindObject<object>(thisPtr));
				ref IntPtr ptr = ref *value;
				ValueTuple<int, IntPtr> valueTuple = MarshalBlittable<Windows.Foundation.Rect>.FromManagedArray(array);
				*__valueSize = valueTuple.Item1;
				ptr = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00010000 File Offset: 0x0000E200
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_get_IsNumericScalar_1(IntPtr thisPtr, byte* value)
		{
			try
			{
				PropertyType propertyType;
				*value = (ManagedIPropertyValueImpl.NumericScalarTypes.TryGetValue(ComWrappersSupport.FindObject<object>(thisPtr).GetType(), ref propertyType) ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00010050 File Offset: 0x0000E250
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_get_Type_0(IntPtr thisPtr, PropertyType* value)
		{
			try
			{
				*value = ManagedIPropertyValueImpl.GetPropertyTypeOfObject(ComWrappersSupport.FindObject<object>(thisPtr));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00010090 File Offset: 0x0000E290
		private static PropertyType GetPropertyTypeOfObject(object obj)
		{
			Type type = obj.GetType();
			bool isArray = type.IsArray;
			if (isArray)
			{
				type = type.GetElementType();
			}
			PropertyType propertyType;
			if (!ManagedIPropertyValueImpl.NumericScalarTypes.TryGetValue(type, ref propertyType))
			{
				if (type == typeof(string))
				{
					propertyType = PropertyType.String;
				}
				else if (type == typeof(char))
				{
					propertyType = PropertyType.Char16;
				}
				else if (type == typeof(bool))
				{
					propertyType = PropertyType.Boolean;
				}
				else if (type == typeof(DateTimeOffset))
				{
					propertyType = PropertyType.DateTime;
				}
				else if (type == typeof(TimeSpan))
				{
					propertyType = PropertyType.TimeSpan;
				}
				else if (type == typeof(Guid))
				{
					propertyType = PropertyType.Guid;
				}
				else if (string.CompareOrdinal(type.FullName, "Windows.Foundation.Point") == 0)
				{
					propertyType = PropertyType.Point;
				}
				else if (string.CompareOrdinal(type.FullName, "Windows.Foundation.Rect") == 0)
				{
					propertyType = PropertyType.Rect;
				}
				else if (string.CompareOrdinal(type.FullName, "Windows.Foundation.Size") == 0)
				{
					propertyType = PropertyType.Size;
				}
				else if (type == typeof(object))
				{
					propertyType = PropertyType.Inspectable;
				}
				else if (typeof(Delegate).IsAssignableFrom(type))
				{
					propertyType = PropertyType.OtherType;
				}
				else if (!type.IsValueType && type != typeof(Type) && isArray)
				{
					propertyType = PropertyType.Inspectable;
				}
				else
				{
					propertyType = PropertyType.OtherType;
				}
			}
			if (isArray)
			{
				propertyType += 1024U;
			}
			return propertyType;
		}

		// Token: 0x04000136 RID: 310
		private const int TYPE_E_TYPEMISMATCH = -2147316576;

		// Token: 0x04000137 RID: 311
		private const int DISP_E_OVERFLOW = -2147352566;

		// Token: 0x04000138 RID: 312
		private static IPropertyValue.Vftbl AbiToProjectionVftable;

		// Token: 0x04000139 RID: 313
		public static IntPtr AbiToProjectionVftablePtr;

		// Token: 0x0400013A RID: 314
		internal static readonly Guid IID = new Guid(1272349405, 30036, 16617, 154, 155, 130, 101, 78, 222, 126, 98);

		// Token: 0x0400013B RID: 315
		private static volatile Dictionary<Type, PropertyType> s_numericScalarTypes;

		// Token: 0x020002CC RID: 716
		private sealed class Boxed<T> where T : struct
		{
			// Token: 0x04000763 RID: 1891
			public T Value;
		}
	}
}
