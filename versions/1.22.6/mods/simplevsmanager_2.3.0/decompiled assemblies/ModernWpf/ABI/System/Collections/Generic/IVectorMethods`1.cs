using System;
using System.Collections.Generic;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.System.Collections.Generic
{
	// Token: 0x02000160 RID: 352
	internal static class IVectorMethods<T>
	{
		// Token: 0x06000B50 RID: 2896 RVA: 0x0001F4AC File Offset: 0x0001D6AC
		public static uint get_Size(IObjectReference obj)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)obj;
			uint result = 0U;
			method getSize_ = objectReference.Vftbl.GetSize_1;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32& modreq(System.Runtime.InteropServices.OutAttribute)), objectReference.ThisPtr, ref result, getSize_));
			return result;
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0001F4E8 File Offset: 0x0001D6E8
		public static T GetAt(IObjectReference obj, uint index)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)obj;
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

		// Token: 0x06000B52 RID: 2898 RVA: 0x0001F55C File Offset: 0x0001D75C
		public static IReadOnlyList<T> GetView(IObjectReference obj)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			IReadOnlyList<T> result;
			try
			{
				method getView_ = objectReference.Vftbl.GetView_2;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, getView_));
				result = MarshalInterface<IReadOnlyList<T>>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInterface<IVectorView<T>>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0001F5C4 File Offset: 0x0001D7C4
		public static bool IndexOf(IObjectReference obj, T value, out uint index)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)obj;
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
				objectReference.Vftbl.IndexOf_3.DynamicInvokeAbi(array2);
				index = (uint)array2[2];
				result = ((byte)array2[3] > 0);
			}
			finally
			{
				Marshaler<T>.DisposeMarshaler.Invoke(obj2);
			}
			return result;
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0001F654 File Offset: 0x0001D854
		public static void SetAt(IObjectReference obj, uint index, T value)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			object obj2 = null;
			object[] array = new object[3];
			array[0] = thisPtr;
			array[1] = index;
			object[] array2 = array;
			try
			{
				obj2 = Marshaler<T>.CreateMarshaler2.Invoke(value);
				array2[2] = Marshaler<T>.GetAbi.Invoke(obj2);
				objectReference.Vftbl.SetAt_4.DynamicInvokeAbi(array2);
			}
			finally
			{
				Marshaler<T>.DisposeMarshaler.Invoke(obj2);
			}
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0001F6D4 File Offset: 0x0001D8D4
		public static void InsertAt(IObjectReference obj, uint index, T value)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			object obj2 = null;
			object[] array = new object[3];
			array[0] = thisPtr;
			array[1] = index;
			object[] array2 = array;
			try
			{
				obj2 = Marshaler<T>.CreateMarshaler2.Invoke(value);
				array2[2] = Marshaler<T>.GetAbi.Invoke(obj2);
				objectReference.Vftbl.InsertAt_5.DynamicInvokeAbi(array2);
			}
			finally
			{
				Marshaler<T>.DisposeMarshaler.Invoke(obj2);
			}
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0001F754 File Offset: 0x0001D954
		public static void RemoveAt(IObjectReference obj, uint index)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			method removeAt_ = objectReference.Vftbl.RemoveAt_6;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32), thisPtr, index, removeAt_));
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x0001F78C File Offset: 0x0001D98C
		public static void Append(IObjectReference obj, T value)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			object obj2 = null;
			object[] array = new object[2];
			array[0] = thisPtr;
			object[] array2 = array;
			try
			{
				obj2 = Marshaler<T>.CreateMarshaler2.Invoke(value);
				array2[1] = Marshaler<T>.GetAbi.Invoke(obj2);
				objectReference.Vftbl.Append_7.DynamicInvokeAbi(array2);
			}
			finally
			{
				Marshaler<T>.DisposeMarshaler.Invoke(obj2);
			}
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0001F804 File Offset: 0x0001DA04
		public static void RemoveAtEnd(IObjectReference obj)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			method removeAtEnd_ = objectReference.Vftbl.RemoveAtEnd_8;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr), thisPtr, removeAtEnd_));
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0001F838 File Offset: 0x0001DA38
		public static void Clear(IObjectReference obj)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			method clear_ = objectReference.Vftbl.Clear_9;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr), thisPtr, clear_));
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0001F86C File Offset: 0x0001DA6C
		public static uint GetMany(IObjectReference obj, uint startIndex, ref T[] items)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)obj;
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
				method getMany_ = objectReference.Vftbl.GetMany_10;
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

		// Token: 0x06000B5B RID: 2907 RVA: 0x0001F920 File Offset: 0x0001DB20
		public static void ReplaceAll(IObjectReference obj, T[] items)
		{
			ObjectReference<IList<T>.Vftbl> objectReference = (ObjectReference<IList<T>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			object obj2 = null;
			IntPtr intPtr = default(IntPtr);
			try
			{
				obj2 = Marshaler<T>.CreateMarshalerArray.Invoke(items);
				ValueTuple<int, IntPtr> valueTuple = Marshaler<T>.GetAbiArray.Invoke(obj2);
				int item = valueTuple.Item1;
				intPtr = valueTuple.Item2;
				method replaceAll_ = objectReference.Vftbl.ReplaceAll_11;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32,System.IntPtr), thisPtr, item, intPtr, replaceAll_));
			}
			finally
			{
				Marshaler<T>.DisposeMarshalerArray.Invoke(obj2);
			}
		}
	}
}
