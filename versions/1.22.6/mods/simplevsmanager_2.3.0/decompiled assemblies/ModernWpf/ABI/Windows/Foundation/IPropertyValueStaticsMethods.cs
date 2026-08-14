using System;
using ABI.System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000D8 RID: 216
	internal static class IPropertyValueStaticsMethods
	{
		// Token: 0x060007D5 RID: 2005 RVA: 0x00014140 File Offset: 0x00012340
		public unsafe static object CreateEmpty(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x000141A0 File Offset: 0x000123A0
		public unsafe static object CreateUInt8(IObjectReference _obj, byte value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.Byte,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)7 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Byte,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, value, ref intPtr, system.Int32_u0020(System.IntPtr,System.Byte,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00014200 File Offset: 0x00012400
		public unsafe static object CreateInt16(IObjectReference _obj, short value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.Int16,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)8 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int16,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, value, ref intPtr, system.Int32_u0020(System.IntPtr,System.Int16,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00014260 File Offset: 0x00012460
		public unsafe static object CreateUInt16(IObjectReference _obj, ushort value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.UInt16,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)9 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt16,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, value, ref intPtr, system.Int32_u0020(System.IntPtr,System.UInt16,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x000142C0 File Offset: 0x000124C0
		public unsafe static object CreateInt32(IObjectReference _obj, int value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)10 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, value, ref intPtr, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00014320 File Offset: 0x00012520
		public unsafe static object CreateUInt32(IObjectReference _obj, uint value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.UInt32,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)11 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, value, ref intPtr, system.Int32_u0020(System.IntPtr,System.UInt32,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00014380 File Offset: 0x00012580
		public unsafe static object CreateInt64(IObjectReference _obj, long value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.Int64,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)12 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int64,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, value, ref intPtr, system.Int32_u0020(System.IntPtr,System.Int64,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x000143E0 File Offset: 0x000125E0
		public unsafe static object CreateUInt64(IObjectReference _obj, ulong value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.UInt64,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)13 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt64,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, value, ref intPtr, system.Int32_u0020(System.IntPtr,System.UInt64,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00014440 File Offset: 0x00012640
		public unsafe static object CreateSingle(IObjectReference _obj, float value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.Single,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)14 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Single,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, value, ref intPtr, system.Int32_u0020(System.IntPtr,System.Single,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x000144A0 File Offset: 0x000126A0
		public unsafe static object CreateDouble(IObjectReference _obj, double value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.Double,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)15 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Double,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, value, ref intPtr, system.Int32_u0020(System.IntPtr,System.Double,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00014500 File Offset: 0x00012700
		public unsafe static object CreateChar16(IObjectReference _obj, char value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.UInt16,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)16 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt16,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, value, ref intPtr, system.Int32_u0020(System.IntPtr,System.UInt16,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00014560 File Offset: 0x00012760
		public unsafe static object CreateBoolean(IObjectReference _obj, bool value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.Byte,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)17 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Byte,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, value ? 1 : 0, ref intPtr, system.Int32_u0020(System.IntPtr,System.Byte,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x000145C8 File Offset: 0x000127C8
		public unsafe static object CreateString(IObjectReference _obj, string value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				MarshalString.Pinnable pinnable = new MarshalString.Pinnable(value);
				try
				{
					fixed (char* ptr = pinnable.GetPinnableReference())
					{
						char* ptr2 = ptr;
						method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)18 * (IntPtr)sizeof(method));
						ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalString.GetAbi(ref pinnable), ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
						result = MarshalInspectable<object>.FromAbi(intPtr);
					}
				}
				finally
				{
					char* ptr = null;
				}
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00014658 File Offset: 0x00012858
		public unsafe static object CreateInspectable(IObjectReference _obj, object value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				objectReferenceValue = MarshalInspectable<object>.CreateMarshaler2(value, true);
				method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)19 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x000146D8 File Offset: 0x000128D8
		public unsafe static object CreateGuid(IObjectReference _obj, Guid value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method system.Int32_u0020(System.IntPtr,System.Guid,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)20 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Guid,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, value, ref intPtr, system.Int32_u0020(System.IntPtr,System.Guid,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00014738 File Offset: 0x00012938
		public unsafe static object CreateDateTime(IObjectReference _obj, DateTimeOffset value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			DateTimeOffset.Marshaler m = default(DateTimeOffset.Marshaler);
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				m = DateTimeOffset.CreateMarshaler(value);
				method system.Int32_u0020(System.IntPtr,ABI.System.DateTimeOffset,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)21 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,ABI.System.DateTimeOffset,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, DateTimeOffset.GetAbi(m), ref intPtr, system.Int32_u0020(System.IntPtr,ABI.System.DateTimeOffset,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				DateTimeOffset.DisposeMarshaler(m);
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x000147B4 File Offset: 0x000129B4
		public unsafe static object CreateTimeSpan(IObjectReference _obj, TimeSpan value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			TimeSpan.Marshaler m = default(TimeSpan.Marshaler);
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				m = TimeSpan.CreateMarshaler(value);
				method system.Int32_u0020(System.IntPtr,ABI.System.TimeSpan,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)22 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,ABI.System.TimeSpan,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, TimeSpan.GetAbi(m), ref intPtr, system.Int32_u0020(System.IntPtr,ABI.System.TimeSpan,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				TimeSpan.DisposeMarshaler(m);
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00014830 File Offset: 0x00012A30
		public unsafe static object CreatePoint(IObjectReference _obj, Windows.Foundation.Point value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method system.Int32_u0020(System.IntPtr,Windows.Foundation.Point,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)23 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.Foundation.Point,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, value, ref intPtr, system.Int32_u0020(System.IntPtr,Windows.Foundation.Point,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00014890 File Offset: 0x00012A90
		public unsafe static object CreateSize(IObjectReference _obj, Windows.Foundation.Size value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method system.Int32_u0020(System.IntPtr,Windows.Foundation.Size,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)24 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.Foundation.Size,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, value, ref intPtr, system.Int32_u0020(System.IntPtr,Windows.Foundation.Size,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x000148F0 File Offset: 0x00012AF0
		public unsafe static object CreateRect(IObjectReference _obj, Windows.Foundation.Rect value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				method system.Int32_u0020(System.IntPtr,Windows.Foundation.Rect,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)25 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.Foundation.Rect,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, value, ref intPtr, system.Int32_u0020(System.IntPtr,Windows.Foundation.Rect,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x00014950 File Offset: 0x00012B50
		public unsafe static object CreateUInt8Array(IObjectReference _obj, byte[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalBlittable<byte>.MarshalerArray marshalerArray = default(MarshalBlittable<byte>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalBlittable<byte>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalBlittable<byte>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)26 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalBlittable<byte>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x000149F4 File Offset: 0x00012BF4
		public unsafe static object CreateInt16Array(IObjectReference _obj, short[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalBlittable<short>.MarshalerArray marshalerArray = default(MarshalBlittable<short>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalBlittable<short>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalBlittable<short>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)27 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalBlittable<short>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x00014A98 File Offset: 0x00012C98
		public unsafe static object CreateUInt16Array(IObjectReference _obj, ushort[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalBlittable<ushort>.MarshalerArray marshalerArray = default(MarshalBlittable<ushort>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalBlittable<ushort>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalBlittable<ushort>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)28 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalBlittable<ushort>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x00014B3C File Offset: 0x00012D3C
		public unsafe static object CreateInt32Array(IObjectReference _obj, int[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalBlittable<int>.MarshalerArray marshalerArray = default(MarshalBlittable<int>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalBlittable<int>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalBlittable<int>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)29 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalBlittable<int>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007ED RID: 2029 RVA: 0x00014BE0 File Offset: 0x00012DE0
		public unsafe static object CreateUInt32Array(IObjectReference _obj, uint[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalBlittable<uint>.MarshalerArray marshalerArray = default(MarshalBlittable<uint>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalBlittable<uint>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalBlittable<uint>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)30 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalBlittable<uint>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x00014C84 File Offset: 0x00012E84
		public unsafe static object CreateInt64Array(IObjectReference _obj, long[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalBlittable<long>.MarshalerArray marshalerArray = default(MarshalBlittable<long>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalBlittable<long>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalBlittable<long>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)31 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalBlittable<long>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x00014D28 File Offset: 0x00012F28
		public unsafe static object CreateUInt64Array(IObjectReference _obj, ulong[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalBlittable<ulong>.MarshalerArray marshalerArray = default(MarshalBlittable<ulong>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalBlittable<ulong>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalBlittable<ulong>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)32 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalBlittable<ulong>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00014DCC File Offset: 0x00012FCC
		public unsafe static object CreateSingleArray(IObjectReference _obj, float[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalBlittable<float>.MarshalerArray marshalerArray = default(MarshalBlittable<float>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalBlittable<float>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalBlittable<float>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)33 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalBlittable<float>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00014E70 File Offset: 0x00013070
		public unsafe static object CreateDoubleArray(IObjectReference _obj, double[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalBlittable<double>.MarshalerArray marshalerArray = default(MarshalBlittable<double>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalBlittable<double>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalBlittable<double>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)34 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalBlittable<double>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00014F14 File Offset: 0x00013114
		public unsafe static object CreateChar16Array(IObjectReference _obj, char[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalNonBlittable<char>.MarshalerArray marshalerArray = default(MarshalNonBlittable<char>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalNonBlittable<char>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalNonBlittable<char>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)35 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalNonBlittable<char>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x00014FB8 File Offset: 0x000131B8
		public unsafe static object CreateBooleanArray(IObjectReference _obj, bool[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalNonBlittable<bool>.MarshalerArray marshalerArray = default(MarshalNonBlittable<bool>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalNonBlittable<bool>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalNonBlittable<bool>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)36 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalNonBlittable<bool>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0001505C File Offset: 0x0001325C
		public unsafe static object CreateStringArray(IObjectReference _obj, string[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalString.MarshalerArray marshalerArray = default(MarshalString.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalString.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalString.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)37 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalString.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00015100 File Offset: 0x00013300
		public unsafe static object CreateInspectableArray(IObjectReference _obj, object[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalInterfaceHelper<object>.MarshalerArray marshalerArray = default(MarshalInterfaceHelper<object>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalInspectable<object>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalInspectable<object>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)38 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalInspectable<object>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x000151A4 File Offset: 0x000133A4
		public unsafe static object CreateGuidArray(IObjectReference _obj, Guid[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalBlittable<Guid>.MarshalerArray marshalerArray = default(MarshalBlittable<Guid>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalBlittable<Guid>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalBlittable<Guid>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)39 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalBlittable<Guid>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00015248 File Offset: 0x00013448
		public unsafe static object CreateDateTimeArray(IObjectReference _obj, DateTimeOffset[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalNonBlittable<DateTimeOffset>.MarshalerArray marshalerArray = default(MarshalNonBlittable<DateTimeOffset>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalNonBlittable<DateTimeOffset>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalNonBlittable<DateTimeOffset>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)40 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalNonBlittable<DateTimeOffset>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x000152EC File Offset: 0x000134EC
		public unsafe static object CreateTimeSpanArray(IObjectReference _obj, TimeSpan[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalNonBlittable<TimeSpan>.MarshalerArray marshalerArray = default(MarshalNonBlittable<TimeSpan>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalNonBlittable<TimeSpan>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalNonBlittable<TimeSpan>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)41 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalNonBlittable<TimeSpan>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00015390 File Offset: 0x00013590
		public unsafe static object CreatePointArray(IObjectReference _obj, Windows.Foundation.Point[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalBlittable<Windows.Foundation.Point>.MarshalerArray marshalerArray = default(MarshalBlittable<Windows.Foundation.Point>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalBlittable<Windows.Foundation.Point>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalBlittable<Windows.Foundation.Point>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)42 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalBlittable<Windows.Foundation.Point>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x00015434 File Offset: 0x00013634
		public unsafe static object CreateSizeArray(IObjectReference _obj, Windows.Foundation.Size[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalBlittable<Windows.Foundation.Size>.MarshalerArray marshalerArray = default(MarshalBlittable<Windows.Foundation.Size>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalBlittable<Windows.Foundation.Size>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalBlittable<Windows.Foundation.Size>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)43 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalBlittable<Windows.Foundation.Size>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x000154D8 File Offset: 0x000136D8
		public unsafe static object CreateRectArray(IObjectReference _obj, Windows.Foundation.Rect[] value)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			MarshalBlittable<Windows.Foundation.Rect>.MarshalerArray marshalerArray = default(MarshalBlittable<Windows.Foundation.Rect>.MarshalerArray);
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			object result;
			try
			{
				marshalerArray = MarshalBlittable<Windows.Foundation.Rect>.CreateMarshalerArray(value);
				ValueTuple<int, IntPtr> abiArray = MarshalBlittable<Windows.Foundation.Rect>.GetAbiArray(marshalerArray);
				int item = abiArray.Item1;
				intPtr = abiArray.Item2;
				method system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)44 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, item, intPtr, ref intPtr2, system.Int32_u0020(System.IntPtr,System.Int32,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				result = MarshalInspectable<object>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalBlittable<Windows.Foundation.Rect>.DisposeMarshalerArray(marshalerArray);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}
	}
}
