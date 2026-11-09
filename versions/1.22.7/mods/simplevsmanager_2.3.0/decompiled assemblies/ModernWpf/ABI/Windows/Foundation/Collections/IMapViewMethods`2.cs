using System;
using ABI.System.Collections.Generic;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x020000F3 RID: 243
	internal static class IMapViewMethods<K, V>
	{
		// Token: 0x060008BC RID: 2236 RVA: 0x000193B8 File Offset: 0x000175B8
		public static V Lookup(IObjectReference obj, K key)
		{
			ObjectReference<IReadOnlyDictionary<K, V>.Vftbl> objectReference = (ObjectReference<IReadOnlyDictionary<K, V>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			object obj2 = null;
			object[] array = new object[3];
			array[0] = thisPtr;
			object[] array2 = array;
			V result;
			try
			{
				obj2 = Marshaler<K>.CreateMarshaler2.Invoke(key);
				array2[1] = Marshaler<K>.GetAbi.Invoke(obj2);
				objectReference.Vftbl.Lookup_0.DynamicInvokeAbi(array2);
				result = Marshaler<V>.FromAbi.Invoke(array2[2]);
			}
			finally
			{
				Marshaler<K>.DisposeMarshaler.Invoke(obj2);
				Marshaler<V>.DisposeAbi.Invoke(array2[2]);
			}
			return result;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x0001944C File Offset: 0x0001764C
		public static bool HasKey(IObjectReference obj, K key)
		{
			ObjectReference<IReadOnlyDictionary<K, V>.Vftbl> objectReference = (ObjectReference<IReadOnlyDictionary<K, V>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			object obj2 = null;
			object[] array = new object[3];
			array[0] = thisPtr;
			object[] array2 = array;
			bool result;
			try
			{
				obj2 = Marshaler<K>.CreateMarshaler2.Invoke(key);
				array2[1] = Marshaler<K>.GetAbi.Invoke(obj2);
				objectReference.Vftbl.HasKey_2.DynamicInvokeAbi(array2);
				result = ((byte)array2[2] > 0);
			}
			finally
			{
				Marshaler<K>.DisposeMarshaler.Invoke(obj2);
			}
			return result;
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x000194D4 File Offset: 0x000176D4
		public static void Split(IObjectReference obj, out IMapView<K, V> first, out IMapView<K, V> second)
		{
			ObjectReference<IReadOnlyDictionary<K, V>.Vftbl> objectReference = (ObjectReference<IReadOnlyDictionary<K, V>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			try
			{
				method split_ = objectReference.Vftbl.Split_3;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, ref intPtr2, split_));
				first = MarshalInterface<IMapView<K, V>>.FromAbi(intPtr);
				second = MarshalInterface<IMapView<K, V>>.FromAbi(intPtr2);
			}
			finally
			{
				MarshalInterface<IMapView<K, V>>.DisposeAbi(intPtr);
				MarshalInterface<IMapView<K, V>>.DisposeAbi(intPtr2);
			}
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00019550 File Offset: 0x00017750
		public static uint get_Size(IObjectReference obj)
		{
			ObjectReference<IReadOnlyDictionary<K, V>.Vftbl> objectReference = (ObjectReference<IReadOnlyDictionary<K, V>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			uint result = 0U;
			method getSize_ = objectReference.Vftbl.GetSize_1;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getSize_));
			return result;
		}
	}
}
