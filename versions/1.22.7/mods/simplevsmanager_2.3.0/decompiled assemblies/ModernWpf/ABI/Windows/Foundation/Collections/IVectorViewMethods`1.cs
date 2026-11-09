using System;
using ABI.System.Collections.Generic;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x020000F4 RID: 244
	internal static class IVectorViewMethods<T>
	{
		// Token: 0x060008C0 RID: 2240 RVA: 0x0001958C File Offset: 0x0001778C
		public static T GetAt(IObjectReference obj, uint index)
		{
			ObjectReference<IReadOnlyList<T>.Vftbl> objectReference = (ObjectReference<IReadOnlyList<T>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			object[] array = new object[3];
			array[0] = thisPtr;
			array[1] = index;
			object[] array2 = array;
			T result;
			try
			{
				objectReference.Vftbl.GetAt_0.DynamicInvokeAbi(array2);
				result = Marshaler<T>.FromAbi.Invoke(array2[2]);
			}
			finally
			{
				Marshaler<T>.DisposeAbi.Invoke(array2[2]);
			}
			return result;
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00019600 File Offset: 0x00017800
		public static bool IndexOf(IObjectReference obj, T value, out uint index)
		{
			ObjectReference<IReadOnlyList<T>.Vftbl> objectReference = (ObjectReference<IReadOnlyList<T>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			object obj2 = null;
			object[] array = new object[4];
			array[0] = thisPtr;
			object[] array2 = array;
			bool result;
			try
			{
				obj2 = Marshaler<T>.CreateMarshaler2.Invoke(value);
				array2[1] = Marshaler<T>.GetAbi.Invoke(obj2);
				objectReference.Vftbl.IndexOf_2.DynamicInvokeAbi(array2);
				index = (uint)array2[2];
				result = ((byte)array2[3] > 0);
			}
			finally
			{
				Marshaler<T>.DisposeMarshaler.Invoke(obj2);
			}
			return result;
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x00019690 File Offset: 0x00017890
		public static uint GetMany(IObjectReference obj, uint startIndex, ref T[] items)
		{
			ObjectReference<IReadOnlyList<T>.Vftbl> objectReference = (ObjectReference<IReadOnlyList<T>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			object obj2 = null;
			IntPtr intPtr = default(IntPtr);
			uint num = 0U;
			uint result;
			try
			{
				obj2 = Marshaler<T>.CreateMarshalerArray.Invoke(items);
				ValueTuple<int, IntPtr> valueTuple = Marshaler<T>.GetAbiArray.Invoke(obj2);
				int item = valueTuple.Item1;
				intPtr = valueTuple.Item2;
				method getMany_ = objectReference.Vftbl.GetMany_3;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32,System.Int32,System.IntPtr,System.UInt32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, startIndex, item, intPtr, ref num, getMany_));
				items = Marshaler<T>.FromAbiArray.Invoke(new ValueTuple<int, IntPtr>(item, intPtr));
				result = num;
			}
			finally
			{
				Marshaler<T>.DisposeMarshalerArray.Invoke(obj2);
			}
			return result;
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00019744 File Offset: 0x00017944
		public static uint get_Size(IObjectReference obj)
		{
			ObjectReference<IReadOnlyList<T>.Vftbl> objectReference = (ObjectReference<IReadOnlyList<T>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			uint result = 0U;
			method getSize_ = objectReference.Vftbl.GetSize_1;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getSize_));
			return result;
		}
	}
}
