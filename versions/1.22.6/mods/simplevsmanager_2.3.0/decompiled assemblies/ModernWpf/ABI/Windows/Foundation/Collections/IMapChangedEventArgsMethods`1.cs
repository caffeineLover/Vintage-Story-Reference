using System;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x020000F5 RID: 245
	internal static class IMapChangedEventArgsMethods<K>
	{
		// Token: 0x060008C4 RID: 2244 RVA: 0x00019780 File Offset: 0x00017980
		public static CollectionChange get_CollectionChange(IObjectReference _genericObj)
		{
			ObjectReference<IMapChangedEventArgs<K>.Vftbl> objectReference = (ObjectReference<IMapChangedEventArgs<K>.Vftbl>)_genericObj;
			IntPtr thisPtr = objectReference.ThisPtr;
			CollectionChange result = CollectionChange.Reset;
			method get_CollectionChange_ = objectReference.Vftbl.get_CollectionChange_0;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.Foundation.Collections.CollectionChange& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, get_CollectionChange_));
			return result;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x000197BC File Offset: 0x000179BC
		public static K get_Key(IObjectReference _genericObj)
		{
			ObjectReference<IMapChangedEventArgs<K>.Vftbl> objectReference = (ObjectReference<IMapChangedEventArgs<K>.Vftbl>)_genericObj;
			IntPtr thisPtr = objectReference.ThisPtr;
			object[] array = new object[2];
			array[0] = thisPtr;
			object[] array2 = array;
			K result;
			try
			{
				objectReference.Vftbl.get_Key_1.DynamicInvokeAbi(array2);
				result = Marshaler<K>.FromAbi.Invoke(array2[1]);
			}
			finally
			{
				Marshaler<K>.DisposeAbi.Invoke(array2[1]);
			}
			return result;
		}
	}
}
