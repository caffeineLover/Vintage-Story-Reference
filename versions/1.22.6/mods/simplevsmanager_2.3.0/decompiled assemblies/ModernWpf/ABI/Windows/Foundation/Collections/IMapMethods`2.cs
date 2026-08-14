using System;
using System.Collections.Generic;
using ABI.System.Collections.Generic;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x020000F0 RID: 240
	internal static class IMapMethods<K, V>
	{
		// Token: 0x060008B2 RID: 2226 RVA: 0x00018FD4 File Offset: 0x000171D4
		public static V Lookup(IObjectReference obj, Dictionary<K, ValueTuple<IntPtr, V>> __lookupCache, K key)
		{
			ObjectReference<IDictionary<K, V>.Vftbl> objectReference = (ObjectReference<IDictionary<K, V>.Vftbl>)obj;
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
				ValueTuple<IntPtr, V> valueTuple;
				if (__lookupCache != null && __lookupCache.TryGetValue(key, ref valueTuple) && valueTuple.Item1 == (IntPtr)array2[2])
				{
					result = valueTuple.Item2;
				}
				else
				{
					V v = Marshaler<V>.FromAbi.Invoke(array2[2]);
					if (__lookupCache != null)
					{
						__lookupCache[key] = new ValueTuple<IntPtr, V>((IntPtr)array2[2], v);
					}
					result = v;
				}
			}
			finally
			{
				Marshaler<K>.DisposeMarshaler.Invoke(obj2);
				Marshaler<V>.DisposeAbi.Invoke(array2[2]);
			}
			return result;
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x000190B4 File Offset: 0x000172B4
		public static bool HasKey(IObjectReference obj, K key)
		{
			ObjectReference<IDictionary<K, V>.Vftbl> objectReference = (ObjectReference<IDictionary<K, V>.Vftbl>)obj;
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

		// Token: 0x060008B4 RID: 2228 RVA: 0x0001913C File Offset: 0x0001733C
		public static IReadOnlyDictionary<K, V> GetView(IObjectReference obj)
		{
			ObjectReference<IDictionary<K, V>.Vftbl> objectReference = (ObjectReference<IDictionary<K, V>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			IReadOnlyDictionary<K, V> result;
			try
			{
				method getView_ = objectReference.Vftbl.GetView_3;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, getView_));
				result = MarshalInterface<IReadOnlyDictionary<K, V>>.FromAbi(intPtr);
			}
			finally
			{
				MarshalInterface<IMapView<K, V>>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x000191A4 File Offset: 0x000173A4
		public static bool Insert(IObjectReference obj, K key, V value)
		{
			ObjectReference<IDictionary<K, V>.Vftbl> objectReference = (ObjectReference<IDictionary<K, V>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			object obj2 = null;
			object obj3 = null;
			object[] array = new object[4];
			array[0] = thisPtr;
			object[] array2 = array;
			bool result;
			try
			{
				obj2 = Marshaler<K>.CreateMarshaler2.Invoke(key);
				array2[1] = Marshaler<K>.GetAbi.Invoke(obj2);
				obj3 = Marshaler<V>.CreateMarshaler2.Invoke(value);
				array2[2] = Marshaler<V>.GetAbi.Invoke(obj3);
				objectReference.Vftbl.Insert_4.DynamicInvokeAbi(array2);
				result = ((byte)array2[3] > 0);
			}
			finally
			{
				Marshaler<K>.DisposeMarshaler.Invoke(obj2);
				Marshaler<V>.DisposeMarshaler.Invoke(obj3);
			}
			return result;
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x00019258 File Offset: 0x00017458
		public static void Remove(IObjectReference obj, K key)
		{
			ObjectReference<IDictionary<K, V>.Vftbl> objectReference = (ObjectReference<IDictionary<K, V>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			object obj2 = null;
			object[] array = new object[2];
			array[0] = thisPtr;
			object[] array2 = array;
			try
			{
				obj2 = Marshaler<K>.CreateMarshaler2.Invoke(key);
				array2[1] = Marshaler<K>.GetAbi.Invoke(obj2);
				objectReference.Vftbl.Remove_5.DynamicInvokeAbi(array2);
			}
			finally
			{
				Marshaler<K>.DisposeMarshaler.Invoke(obj2);
			}
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x000192D0 File Offset: 0x000174D0
		public static void Clear(IObjectReference obj)
		{
			IMapMethods<K, V>._ClearHelper(obj);
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x000192D8 File Offset: 0x000174D8
		private static void _ClearHelper(IObjectReference obj)
		{
			ObjectReference<IDictionary<K, V>.Vftbl> objectReference = (ObjectReference<IDictionary<K, V>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			method clear_ = objectReference.Vftbl.Clear_6;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr), thisPtr, clear_));
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0001930C File Offset: 0x0001750C
		public static uint get_Size(IObjectReference obj)
		{
			ObjectReference<IDictionary<K, V>.Vftbl> objectReference = (ObjectReference<IDictionary<K, V>.Vftbl>)obj;
			IntPtr thisPtr = objectReference.ThisPtr;
			uint result = 0U;
			method getSize_ = objectReference.Vftbl.GetSize_1;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, getSize_));
			return result;
		}
	}
}
