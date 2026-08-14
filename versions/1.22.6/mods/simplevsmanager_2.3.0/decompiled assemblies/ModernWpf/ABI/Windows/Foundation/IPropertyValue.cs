using System;
using System.Runtime.InteropServices;
using ABI.System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000AD RID: 173
	[DynamicInterfaceCastableImplementation]
	[Guid("4BD682DD-7554-40E9-9A9B-82654EDE7E62")]
	internal interface IPropertyValue : Windows.Foundation.IPropertyValue
	{
		// Token: 0x060006C7 RID: 1735 RVA: 0x0001020C File Offset: 0x0000E40C
		byte GetUInt8()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			byte result = 0;
			method getUInt8_ = objectReference.Vftbl.GetUInt8_2;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getUInt8_));
			return result;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00010260 File Offset: 0x0000E460
		short GetInt16()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			short result = 0;
			method getInt16_ = objectReference.Vftbl.GetInt16_3;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int16& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getInt16_));
			return result;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x000102B4 File Offset: 0x0000E4B4
		ushort GetUInt16()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			ushort result = 0;
			method getUInt16_ = objectReference.Vftbl.GetUInt16_4;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt16& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getUInt16_));
			return result;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00010308 File Offset: 0x0000E508
		int GetInt32()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int result = 0;
			method getInt32_ = objectReference.Vftbl.GetInt32_5;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getInt32_));
			return result;
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0001035C File Offset: 0x0000E55C
		uint GetUInt32()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			uint result = 0U;
			method getUInt32_ = objectReference.Vftbl.GetUInt32_6;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getUInt32_));
			return result;
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x000103B0 File Offset: 0x0000E5B0
		long GetInt64()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			long result = 0L;
			method getInt64_ = objectReference.Vftbl.GetInt64_7;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int64& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getInt64_));
			return result;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00010404 File Offset: 0x0000E604
		ulong GetUInt64()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			ulong result = 0UL;
			method getUInt64_ = objectReference.Vftbl.GetUInt64_8;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt64& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getUInt64_));
			return result;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00010458 File Offset: 0x0000E658
		float GetSingle()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			float result = 0f;
			method getSingle_ = objectReference.Vftbl.GetSingle_9;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Single& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getSingle_));
			return result;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x000104B0 File Offset: 0x0000E6B0
		double GetDouble()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			double result = 0.0;
			method getDouble_ = objectReference.Vftbl.GetDouble_10;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Double& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getDouble_));
			return result;
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001050C File Offset: 0x0000E70C
		char GetChar16()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			ushort result = 0;
			method getChar16_ = objectReference.Vftbl.GetChar16_11;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt16& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getChar16_));
			return (char)result;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x00010560 File Offset: 0x0000E760
		bool GetBoolean()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			byte b = 0;
			method getBoolean_ = objectReference.Vftbl.GetBoolean_12;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref b, getBoolean_));
			return b > 0;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x000105B8 File Offset: 0x0000E7B8
		string GetString()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method getString_ = objectReference.Vftbl.GetString_13;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, getString_));
				result = MarshalString.FromAbi(intPtr);
			}
			finally
			{
				MarshalString.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x00010638 File Offset: 0x0000E838
		Guid GetGuid()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			Guid result = default(Guid);
			method getGuid_ = objectReference.Vftbl.GetGuid_14;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Guid& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getGuid_));
			return result;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00010690 File Offset: 0x0000E890
		DateTimeOffset GetDateTime()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			DateTimeOffset dateTimeOffset = default(DateTimeOffset);
			DateTimeOffset result;
			try
			{
				method getDateTime_ = objectReference.Vftbl.GetDateTime_15;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,ABI.System.DateTimeOffset& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref dateTimeOffset, getDateTime_));
				result = DateTimeOffset.FromAbi(dateTimeOffset);
			}
			finally
			{
				DateTimeOffset.DisposeAbi(dateTimeOffset);
			}
			return result;
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00010710 File Offset: 0x0000E910
		TimeSpan GetTimeSpan()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			TimeSpan timeSpan = default(TimeSpan);
			TimeSpan result;
			try
			{
				method getTimeSpan_ = objectReference.Vftbl.GetTimeSpan_16;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,ABI.System.TimeSpan& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref timeSpan, getTimeSpan_));
				result = TimeSpan.FromAbi(timeSpan);
			}
			finally
			{
				TimeSpan.DisposeAbi(timeSpan);
			}
			return result;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x00010790 File Offset: 0x0000E990
		Windows.Foundation.Point GetPoint()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			Windows.Foundation.Point result = default(Windows.Foundation.Point);
			method getPoint_ = objectReference.Vftbl.GetPoint_17;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.Foundation.Point& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getPoint_));
			return result;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x000107E8 File Offset: 0x0000E9E8
		Windows.Foundation.Size GetSize()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			Windows.Foundation.Size result = default(Windows.Foundation.Size);
			method getSize_ = objectReference.Vftbl.GetSize_18;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.Foundation.Size& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getSize_));
			return result;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x00010840 File Offset: 0x0000EA40
		Windows.Foundation.Rect GetRect()
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			Windows.Foundation.Rect result = default(Windows.Foundation.Rect);
			method getRect_ = objectReference.Vftbl.GetRect_19;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.Foundation.Rect& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getRect_));
			return result;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00010898 File Offset: 0x0000EA98
		void GetUInt8Array(out byte[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getUInt8Array_ = objectReference.Vftbl.GetUInt8Array_20;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getUInt8Array_));
				value = MarshalBlittable<byte>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalBlittable<byte>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x00010930 File Offset: 0x0000EB30
		void GetInt16Array(out short[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getInt16Array_ = objectReference.Vftbl.GetInt16Array_21;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getInt16Array_));
				value = MarshalBlittable<short>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalBlittable<short>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x000109C8 File Offset: 0x0000EBC8
		void GetUInt16Array(out ushort[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getUInt16Array_ = objectReference.Vftbl.GetUInt16Array_22;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getUInt16Array_));
				value = MarshalBlittable<ushort>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalBlittable<ushort>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x00010A60 File Offset: 0x0000EC60
		void GetInt32Array(out int[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getInt32Array_ = objectReference.Vftbl.GetInt32Array_23;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getInt32Array_));
				value = MarshalBlittable<int>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalBlittable<int>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x00010AF8 File Offset: 0x0000ECF8
		void GetUInt32Array(out uint[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getUInt32Array_ = objectReference.Vftbl.GetUInt32Array_24;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getUInt32Array_));
				value = MarshalBlittable<uint>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalBlittable<uint>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x00010B90 File Offset: 0x0000ED90
		void GetInt64Array(out long[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getInt64Array_ = objectReference.Vftbl.GetInt64Array_25;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getInt64Array_));
				value = MarshalBlittable<long>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalBlittable<long>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x00010C28 File Offset: 0x0000EE28
		void GetUInt64Array(out ulong[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getUInt64Array_ = objectReference.Vftbl.GetUInt64Array_26;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getUInt64Array_));
				value = MarshalBlittable<ulong>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalBlittable<ulong>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x00010CC0 File Offset: 0x0000EEC0
		void GetSingleArray(out float[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getSingleArray_ = objectReference.Vftbl.GetSingleArray_27;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getSingleArray_));
				value = MarshalBlittable<float>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalBlittable<float>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x00010D58 File Offset: 0x0000EF58
		void GetDoubleArray(out double[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getDoubleArray_ = objectReference.Vftbl.GetDoubleArray_28;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getDoubleArray_));
				value = MarshalBlittable<double>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalBlittable<double>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x00010DF0 File Offset: 0x0000EFF0
		void GetChar16Array(out char[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getChar16Array_ = objectReference.Vftbl.GetChar16Array_29;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getChar16Array_));
				value = MarshalNonBlittable<char>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalNonBlittable<char>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x00010E88 File Offset: 0x0000F088
		void GetBooleanArray(out bool[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getBooleanArray_ = objectReference.Vftbl.GetBooleanArray_30;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getBooleanArray_));
				value = MarshalNonBlittable<bool>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalNonBlittable<bool>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x00010F20 File Offset: 0x0000F120
		void GetStringArray(out string[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getStringArray_ = objectReference.Vftbl.GetStringArray_31;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getStringArray_));
				value = MarshalString.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalString.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x00010FB8 File Offset: 0x0000F1B8
		void GetInspectableArray(out object[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getInspectableArray_ = objectReference.Vftbl.GetInspectableArray_32;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getInspectableArray_));
				value = MarshalInspectable<object>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00011050 File Offset: 0x0000F250
		void GetGuidArray(out Guid[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getGuidArray_ = objectReference.Vftbl.GetGuidArray_33;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getGuidArray_));
				value = MarshalBlittable<Guid>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalBlittable<Guid>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x000110E8 File Offset: 0x0000F2E8
		void GetDateTimeArray(out DateTimeOffset[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getDateTimeArray_ = objectReference.Vftbl.GetDateTimeArray_34;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getDateTimeArray_));
				value = MarshalNonBlittable<DateTimeOffset>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalNonBlittable<DateTimeOffset>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00011180 File Offset: 0x0000F380
		void GetTimeSpanArray(out TimeSpan[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getTimeSpanArray_ = objectReference.Vftbl.GetTimeSpanArray_35;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getTimeSpanArray_));
				value = MarshalNonBlittable<TimeSpan>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalNonBlittable<TimeSpan>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00011218 File Offset: 0x0000F418
		void GetPointArray(out Windows.Foundation.Point[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getPointArray_ = objectReference.Vftbl.GetPointArray_36;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getPointArray_));
				value = MarshalBlittable<Windows.Foundation.Point>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalBlittable<Windows.Foundation.Point>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x000112B0 File Offset: 0x0000F4B0
		void GetSizeArray(out Windows.Foundation.Size[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getSizeArray_ = objectReference.Vftbl.GetSizeArray_37;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getSizeArray_));
				value = MarshalBlittable<Windows.Foundation.Size>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalBlittable<Windows.Foundation.Size>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x00011348 File Offset: 0x0000F548
		void GetRectArray(out Windows.Foundation.Rect[] value)
		{
			ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
			IntPtr thisPtr = objectReference.ThisPtr;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			try
			{
				method getRectArray_ = objectReference.Vftbl.GetRectArray_38;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref num, ref intPtr, getRectArray_));
				value = MarshalBlittable<Windows.Foundation.Rect>.FromAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				MarshalBlittable<Windows.Foundation.Rect>.DisposeAbiArray(new ValueTuple<int, IntPtr>(num, intPtr));
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x000113E0 File Offset: 0x0000F5E0
		bool Windows.Foundation.IPropertyValue.IsNumericScalar
		{
			get
			{
				ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
				IntPtr thisPtr = objectReference.ThisPtr;
				byte b = 0;
				method get_IsNumericScalar_ = objectReference.Vftbl.get_IsNumericScalar_1;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref b, get_IsNumericScalar_));
				return b > 0;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x00011438 File Offset: 0x0000F638
		PropertyType Windows.Foundation.IPropertyValue.Type
		{
			get
			{
				ObjectReference<IPropertyValue.Vftbl> objectReference = (ObjectReference<IPropertyValue.Vftbl>)((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IPropertyValue).TypeHandle);
				IntPtr thisPtr = objectReference.ThisPtr;
				PropertyType result = PropertyType.Empty;
				method get_Type_ = objectReference.Vftbl.get_Type_0;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.Foundation.PropertyType& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, get_Type_));
				return result;
			}
		}

		// Token: 0x020002CD RID: 717
		[Guid("4BD682DD-7554-40E9-9A9B-82654EDE7E62")]
		public struct Vftbl
		{
			// Token: 0x17000410 RID: 1040
			// (get) Token: 0x0600158E RID: 5518 RVA: 0x0003FA31 File Offset: 0x0003DC31
			// (set) Token: 0x0600158F RID: 5519 RVA: 0x0003FA39 File Offset: 0x0003DC39
			public method get_Type_0
			{
				get
				{
					return this._get_Type_0;
				}
				set
				{
					this._get_Type_0 = value;
				}
			}

			// Token: 0x17000411 RID: 1041
			// (get) Token: 0x06001590 RID: 5520 RVA: 0x0003FA42 File Offset: 0x0003DC42
			// (set) Token: 0x06001591 RID: 5521 RVA: 0x0003FA4A File Offset: 0x0003DC4A
			public method get_IsNumericScalar_1
			{
				get
				{
					return this._get_IsNumericScalar_1;
				}
				set
				{
					this._get_IsNumericScalar_1 = value;
				}
			}

			// Token: 0x17000412 RID: 1042
			// (get) Token: 0x06001592 RID: 5522 RVA: 0x0003FA53 File Offset: 0x0003DC53
			// (set) Token: 0x06001593 RID: 5523 RVA: 0x0003FA5B File Offset: 0x0003DC5B
			public method GetUInt8_2
			{
				get
				{
					return this._GetUInt8_2;
				}
				set
				{
					this._GetUInt8_2 = value;
				}
			}

			// Token: 0x17000413 RID: 1043
			// (get) Token: 0x06001594 RID: 5524 RVA: 0x0003FA64 File Offset: 0x0003DC64
			// (set) Token: 0x06001595 RID: 5525 RVA: 0x0003FA6C File Offset: 0x0003DC6C
			public method GetInt16_3
			{
				get
				{
					return this._GetInt16_3;
				}
				set
				{
					this._GetInt16_3 = value;
				}
			}

			// Token: 0x17000414 RID: 1044
			// (get) Token: 0x06001596 RID: 5526 RVA: 0x0003FA75 File Offset: 0x0003DC75
			// (set) Token: 0x06001597 RID: 5527 RVA: 0x0003FA7D File Offset: 0x0003DC7D
			public method GetUInt16_4
			{
				get
				{
					return this._GetUInt16_4;
				}
				set
				{
					this._GetUInt16_4 = value;
				}
			}

			// Token: 0x17000415 RID: 1045
			// (get) Token: 0x06001598 RID: 5528 RVA: 0x0003FA86 File Offset: 0x0003DC86
			// (set) Token: 0x06001599 RID: 5529 RVA: 0x0003FA8E File Offset: 0x0003DC8E
			public method GetInt32_5
			{
				get
				{
					return this._GetInt32_5;
				}
				set
				{
					this._GetInt32_5 = value;
				}
			}

			// Token: 0x17000416 RID: 1046
			// (get) Token: 0x0600159A RID: 5530 RVA: 0x0003FA97 File Offset: 0x0003DC97
			// (set) Token: 0x0600159B RID: 5531 RVA: 0x0003FA9F File Offset: 0x0003DC9F
			public method GetUInt32_6
			{
				get
				{
					return this._GetUInt32_6;
				}
				set
				{
					this._GetUInt32_6 = value;
				}
			}

			// Token: 0x17000417 RID: 1047
			// (get) Token: 0x0600159C RID: 5532 RVA: 0x0003FAA8 File Offset: 0x0003DCA8
			// (set) Token: 0x0600159D RID: 5533 RVA: 0x0003FAB0 File Offset: 0x0003DCB0
			public method GetInt64_7
			{
				get
				{
					return this._GetInt64_7;
				}
				set
				{
					this._GetInt64_7 = value;
				}
			}

			// Token: 0x17000418 RID: 1048
			// (get) Token: 0x0600159E RID: 5534 RVA: 0x0003FAB9 File Offset: 0x0003DCB9
			// (set) Token: 0x0600159F RID: 5535 RVA: 0x0003FAC1 File Offset: 0x0003DCC1
			public method GetUInt64_8
			{
				get
				{
					return this._GetUInt64_8;
				}
				set
				{
					this._GetUInt64_8 = value;
				}
			}

			// Token: 0x17000419 RID: 1049
			// (get) Token: 0x060015A0 RID: 5536 RVA: 0x0003FACA File Offset: 0x0003DCCA
			// (set) Token: 0x060015A1 RID: 5537 RVA: 0x0003FAD2 File Offset: 0x0003DCD2
			public method GetSingle_9
			{
				get
				{
					return this._GetSingle_9;
				}
				set
				{
					this._GetSingle_9 = value;
				}
			}

			// Token: 0x1700041A RID: 1050
			// (get) Token: 0x060015A2 RID: 5538 RVA: 0x0003FADB File Offset: 0x0003DCDB
			// (set) Token: 0x060015A3 RID: 5539 RVA: 0x0003FAE3 File Offset: 0x0003DCE3
			public method GetDouble_10
			{
				get
				{
					return this._GetDouble_10;
				}
				set
				{
					this._GetDouble_10 = value;
				}
			}

			// Token: 0x1700041B RID: 1051
			// (get) Token: 0x060015A4 RID: 5540 RVA: 0x0003FAEC File Offset: 0x0003DCEC
			// (set) Token: 0x060015A5 RID: 5541 RVA: 0x0003FAF4 File Offset: 0x0003DCF4
			public method GetChar16_11
			{
				get
				{
					return this._GetChar16_11;
				}
				set
				{
					this._GetChar16_11 = value;
				}
			}

			// Token: 0x1700041C RID: 1052
			// (get) Token: 0x060015A6 RID: 5542 RVA: 0x0003FAFD File Offset: 0x0003DCFD
			// (set) Token: 0x060015A7 RID: 5543 RVA: 0x0003FB05 File Offset: 0x0003DD05
			public method GetBoolean_12
			{
				get
				{
					return this._GetBoolean_12;
				}
				set
				{
					this._GetBoolean_12 = value;
				}
			}

			// Token: 0x1700041D RID: 1053
			// (get) Token: 0x060015A8 RID: 5544 RVA: 0x0003FB0E File Offset: 0x0003DD0E
			// (set) Token: 0x060015A9 RID: 5545 RVA: 0x0003FB16 File Offset: 0x0003DD16
			public method GetString_13
			{
				get
				{
					return this._GetString_13;
				}
				set
				{
					this._GetString_13 = value;
				}
			}

			// Token: 0x1700041E RID: 1054
			// (get) Token: 0x060015AA RID: 5546 RVA: 0x0003FB1F File Offset: 0x0003DD1F
			// (set) Token: 0x060015AB RID: 5547 RVA: 0x0003FB27 File Offset: 0x0003DD27
			public method GetGuid_14
			{
				get
				{
					return this._GetGuid_14;
				}
				set
				{
					this._GetGuid_14 = value;
				}
			}

			// Token: 0x1700041F RID: 1055
			// (get) Token: 0x060015AC RID: 5548 RVA: 0x0003FB30 File Offset: 0x0003DD30
			// (set) Token: 0x060015AD RID: 5549 RVA: 0x0003FB38 File Offset: 0x0003DD38
			public method GetDateTime_15
			{
				get
				{
					return this._GetDateTime_15;
				}
				set
				{
					this._GetDateTime_15 = value;
				}
			}

			// Token: 0x17000420 RID: 1056
			// (get) Token: 0x060015AE RID: 5550 RVA: 0x0003FB41 File Offset: 0x0003DD41
			// (set) Token: 0x060015AF RID: 5551 RVA: 0x0003FB49 File Offset: 0x0003DD49
			public method GetTimeSpan_16
			{
				get
				{
					return this._GetTimeSpan_16;
				}
				set
				{
					this._GetTimeSpan_16 = value;
				}
			}

			// Token: 0x17000421 RID: 1057
			// (get) Token: 0x060015B0 RID: 5552 RVA: 0x0003FB52 File Offset: 0x0003DD52
			// (set) Token: 0x060015B1 RID: 5553 RVA: 0x0003FB5A File Offset: 0x0003DD5A
			public method GetPoint_17
			{
				get
				{
					return this._GetPoint_17;
				}
				set
				{
					this._GetPoint_17 = value;
				}
			}

			// Token: 0x17000422 RID: 1058
			// (get) Token: 0x060015B2 RID: 5554 RVA: 0x0003FB63 File Offset: 0x0003DD63
			// (set) Token: 0x060015B3 RID: 5555 RVA: 0x0003FB6B File Offset: 0x0003DD6B
			public method GetSize_18
			{
				get
				{
					return this._GetSize_18;
				}
				set
				{
					this._GetSize_18 = value;
				}
			}

			// Token: 0x17000423 RID: 1059
			// (get) Token: 0x060015B4 RID: 5556 RVA: 0x0003FB74 File Offset: 0x0003DD74
			// (set) Token: 0x060015B5 RID: 5557 RVA: 0x0003FB7C File Offset: 0x0003DD7C
			public method GetRect_19
			{
				get
				{
					return this._GetRect_19;
				}
				set
				{
					this._GetRect_19 = value;
				}
			}

			// Token: 0x17000424 RID: 1060
			// (get) Token: 0x060015B6 RID: 5558 RVA: 0x0003FB85 File Offset: 0x0003DD85
			// (set) Token: 0x060015B7 RID: 5559 RVA: 0x0003FB8D File Offset: 0x0003DD8D
			public method GetUInt8Array_20
			{
				get
				{
					return this._GetUInt8Array_20;
				}
				set
				{
					this._GetUInt8Array_20 = value;
				}
			}

			// Token: 0x17000425 RID: 1061
			// (get) Token: 0x060015B8 RID: 5560 RVA: 0x0003FB96 File Offset: 0x0003DD96
			// (set) Token: 0x060015B9 RID: 5561 RVA: 0x0003FB9E File Offset: 0x0003DD9E
			public method GetInt16Array_21
			{
				get
				{
					return this._GetInt16Array_21;
				}
				set
				{
					this._GetInt16Array_21 = value;
				}
			}

			// Token: 0x17000426 RID: 1062
			// (get) Token: 0x060015BA RID: 5562 RVA: 0x0003FBA7 File Offset: 0x0003DDA7
			// (set) Token: 0x060015BB RID: 5563 RVA: 0x0003FBAF File Offset: 0x0003DDAF
			public method GetUInt16Array_22
			{
				get
				{
					return this._GetUInt16Array_22;
				}
				set
				{
					this._GetUInt16Array_22 = value;
				}
			}

			// Token: 0x17000427 RID: 1063
			// (get) Token: 0x060015BC RID: 5564 RVA: 0x0003FBB8 File Offset: 0x0003DDB8
			// (set) Token: 0x060015BD RID: 5565 RVA: 0x0003FBC0 File Offset: 0x0003DDC0
			public method GetInt32Array_23
			{
				get
				{
					return this._GetInt32Array_23;
				}
				set
				{
					this._GetInt32Array_23 = value;
				}
			}

			// Token: 0x17000428 RID: 1064
			// (get) Token: 0x060015BE RID: 5566 RVA: 0x0003FBC9 File Offset: 0x0003DDC9
			// (set) Token: 0x060015BF RID: 5567 RVA: 0x0003FBD1 File Offset: 0x0003DDD1
			public method GetUInt32Array_24
			{
				get
				{
					return this._GetUInt32Array_24;
				}
				set
				{
					this._GetUInt32Array_24 = value;
				}
			}

			// Token: 0x17000429 RID: 1065
			// (get) Token: 0x060015C0 RID: 5568 RVA: 0x0003FBDA File Offset: 0x0003DDDA
			// (set) Token: 0x060015C1 RID: 5569 RVA: 0x0003FBE2 File Offset: 0x0003DDE2
			public method GetInt64Array_25
			{
				get
				{
					return this._GetInt64Array_25;
				}
				set
				{
					this._GetInt64Array_25 = value;
				}
			}

			// Token: 0x1700042A RID: 1066
			// (get) Token: 0x060015C2 RID: 5570 RVA: 0x0003FBEB File Offset: 0x0003DDEB
			// (set) Token: 0x060015C3 RID: 5571 RVA: 0x0003FBF3 File Offset: 0x0003DDF3
			public method GetUInt64Array_26
			{
				get
				{
					return this._GetUInt64Array_26;
				}
				set
				{
					this._GetUInt64Array_26 = value;
				}
			}

			// Token: 0x1700042B RID: 1067
			// (get) Token: 0x060015C4 RID: 5572 RVA: 0x0003FBFC File Offset: 0x0003DDFC
			// (set) Token: 0x060015C5 RID: 5573 RVA: 0x0003FC04 File Offset: 0x0003DE04
			public method GetSingleArray_27
			{
				get
				{
					return this._GetSingleArray_27;
				}
				set
				{
					this._GetSingleArray_27 = value;
				}
			}

			// Token: 0x1700042C RID: 1068
			// (get) Token: 0x060015C6 RID: 5574 RVA: 0x0003FC0D File Offset: 0x0003DE0D
			// (set) Token: 0x060015C7 RID: 5575 RVA: 0x0003FC15 File Offset: 0x0003DE15
			public method GetDoubleArray_28
			{
				get
				{
					return this._GetDoubleArray_28;
				}
				set
				{
					this._GetDoubleArray_28 = value;
				}
			}

			// Token: 0x1700042D RID: 1069
			// (get) Token: 0x060015C8 RID: 5576 RVA: 0x0003FC1E File Offset: 0x0003DE1E
			// (set) Token: 0x060015C9 RID: 5577 RVA: 0x0003FC26 File Offset: 0x0003DE26
			public method GetChar16Array_29
			{
				get
				{
					return this._GetChar16Array_29;
				}
				set
				{
					this._GetChar16Array_29 = value;
				}
			}

			// Token: 0x1700042E RID: 1070
			// (get) Token: 0x060015CA RID: 5578 RVA: 0x0003FC2F File Offset: 0x0003DE2F
			// (set) Token: 0x060015CB RID: 5579 RVA: 0x0003FC37 File Offset: 0x0003DE37
			public method GetBooleanArray_30
			{
				get
				{
					return this._GetBooleanArray_30;
				}
				set
				{
					this._GetBooleanArray_30 = value;
				}
			}

			// Token: 0x1700042F RID: 1071
			// (get) Token: 0x060015CC RID: 5580 RVA: 0x0003FC40 File Offset: 0x0003DE40
			// (set) Token: 0x060015CD RID: 5581 RVA: 0x0003FC48 File Offset: 0x0003DE48
			public method GetStringArray_31
			{
				get
				{
					return this._GetStringArray_31;
				}
				set
				{
					this._GetStringArray_31 = value;
				}
			}

			// Token: 0x17000430 RID: 1072
			// (get) Token: 0x060015CE RID: 5582 RVA: 0x0003FC51 File Offset: 0x0003DE51
			// (set) Token: 0x060015CF RID: 5583 RVA: 0x0003FC59 File Offset: 0x0003DE59
			public method GetInspectableArray_32
			{
				get
				{
					return this._GetInspectableArray_32;
				}
				set
				{
					this._GetInspectableArray_32 = value;
				}
			}

			// Token: 0x17000431 RID: 1073
			// (get) Token: 0x060015D0 RID: 5584 RVA: 0x0003FC62 File Offset: 0x0003DE62
			// (set) Token: 0x060015D1 RID: 5585 RVA: 0x0003FC6A File Offset: 0x0003DE6A
			public method GetGuidArray_33
			{
				get
				{
					return this._GetGuidArray_33;
				}
				set
				{
					this._GetGuidArray_33 = value;
				}
			}

			// Token: 0x17000432 RID: 1074
			// (get) Token: 0x060015D2 RID: 5586 RVA: 0x0003FC73 File Offset: 0x0003DE73
			// (set) Token: 0x060015D3 RID: 5587 RVA: 0x0003FC7B File Offset: 0x0003DE7B
			public method GetDateTimeArray_34
			{
				get
				{
					return this._GetDateTimeArray_34;
				}
				set
				{
					this._GetDateTimeArray_34 = value;
				}
			}

			// Token: 0x17000433 RID: 1075
			// (get) Token: 0x060015D4 RID: 5588 RVA: 0x0003FC84 File Offset: 0x0003DE84
			// (set) Token: 0x060015D5 RID: 5589 RVA: 0x0003FC8C File Offset: 0x0003DE8C
			public method GetTimeSpanArray_35
			{
				get
				{
					return this._GetTimeSpanArray_35;
				}
				set
				{
					this._GetTimeSpanArray_35 = value;
				}
			}

			// Token: 0x17000434 RID: 1076
			// (get) Token: 0x060015D6 RID: 5590 RVA: 0x0003FC95 File Offset: 0x0003DE95
			// (set) Token: 0x060015D7 RID: 5591 RVA: 0x0003FC9D File Offset: 0x0003DE9D
			public method GetPointArray_36
			{
				get
				{
					return this._GetPointArray_36;
				}
				set
				{
					this._GetPointArray_36 = value;
				}
			}

			// Token: 0x17000435 RID: 1077
			// (get) Token: 0x060015D8 RID: 5592 RVA: 0x0003FCA6 File Offset: 0x0003DEA6
			// (set) Token: 0x060015D9 RID: 5593 RVA: 0x0003FCAE File Offset: 0x0003DEAE
			public method GetSizeArray_37
			{
				get
				{
					return this._GetSizeArray_37;
				}
				set
				{
					this._GetSizeArray_37 = value;
				}
			}

			// Token: 0x17000436 RID: 1078
			// (get) Token: 0x060015DA RID: 5594 RVA: 0x0003FCB7 File Offset: 0x0003DEB7
			// (set) Token: 0x060015DB RID: 5595 RVA: 0x0003FCBF File Offset: 0x0003DEBF
			public method GetRectArray_38
			{
				get
				{
					return this._GetRectArray_38;
				}
				set
				{
					this._GetRectArray_38 = value;
				}
			}

			// Token: 0x04000764 RID: 1892
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000765 RID: 1893
			internal unsafe void* _get_Type_0;

			// Token: 0x04000766 RID: 1894
			public unsafe void* _get_IsNumericScalar_1;

			// Token: 0x04000767 RID: 1895
			internal unsafe void* _GetUInt8_2;

			// Token: 0x04000768 RID: 1896
			internal unsafe void* _GetInt16_3;

			// Token: 0x04000769 RID: 1897
			internal unsafe void* _GetUInt16_4;

			// Token: 0x0400076A RID: 1898
			internal unsafe void* _GetInt32_5;

			// Token: 0x0400076B RID: 1899
			internal unsafe void* _GetUInt32_6;

			// Token: 0x0400076C RID: 1900
			internal unsafe void* _GetInt64_7;

			// Token: 0x0400076D RID: 1901
			internal unsafe void* _GetUInt64_8;

			// Token: 0x0400076E RID: 1902
			internal unsafe void* _GetSingle_9;

			// Token: 0x0400076F RID: 1903
			internal unsafe void* _GetDouble_10;

			// Token: 0x04000770 RID: 1904
			internal unsafe void* _GetChar16_11;

			// Token: 0x04000771 RID: 1905
			internal unsafe void* _GetBoolean_12;

			// Token: 0x04000772 RID: 1906
			internal unsafe void* _GetString_13;

			// Token: 0x04000773 RID: 1907
			internal unsafe void* _GetGuid_14;

			// Token: 0x04000774 RID: 1908
			internal unsafe void* _GetDateTime_15;

			// Token: 0x04000775 RID: 1909
			internal unsafe void* _GetTimeSpan_16;

			// Token: 0x04000776 RID: 1910
			internal unsafe void* _GetPoint_17;

			// Token: 0x04000777 RID: 1911
			internal unsafe void* _GetSize_18;

			// Token: 0x04000778 RID: 1912
			internal unsafe void* _GetRect_19;

			// Token: 0x04000779 RID: 1913
			internal unsafe void* _GetUInt8Array_20;

			// Token: 0x0400077A RID: 1914
			internal unsafe void* _GetInt16Array_21;

			// Token: 0x0400077B RID: 1915
			internal unsafe void* _GetUInt16Array_22;

			// Token: 0x0400077C RID: 1916
			internal unsafe void* _GetInt32Array_23;

			// Token: 0x0400077D RID: 1917
			internal unsafe void* _GetUInt32Array_24;

			// Token: 0x0400077E RID: 1918
			internal unsafe void* _GetInt64Array_25;

			// Token: 0x0400077F RID: 1919
			internal unsafe void* _GetUInt64Array_26;

			// Token: 0x04000780 RID: 1920
			internal unsafe void* _GetSingleArray_27;

			// Token: 0x04000781 RID: 1921
			internal unsafe void* _GetDoubleArray_28;

			// Token: 0x04000782 RID: 1922
			internal unsafe void* _GetChar16Array_29;

			// Token: 0x04000783 RID: 1923
			internal unsafe void* _GetBooleanArray_30;

			// Token: 0x04000784 RID: 1924
			internal unsafe void* _GetStringArray_31;

			// Token: 0x04000785 RID: 1925
			internal unsafe void* _GetInspectableArray_32;

			// Token: 0x04000786 RID: 1926
			internal unsafe void* _GetGuidArray_33;

			// Token: 0x04000787 RID: 1927
			internal unsafe void* _GetDateTimeArray_34;

			// Token: 0x04000788 RID: 1928
			internal unsafe void* _GetTimeSpanArray_35;

			// Token: 0x04000789 RID: 1929
			internal unsafe void* _GetPointArray_36;

			// Token: 0x0400078A RID: 1930
			internal unsafe void* _GetSizeArray_37;

			// Token: 0x0400078B RID: 1931
			internal unsafe void* _GetRectArray_38;
		}
	}
}
