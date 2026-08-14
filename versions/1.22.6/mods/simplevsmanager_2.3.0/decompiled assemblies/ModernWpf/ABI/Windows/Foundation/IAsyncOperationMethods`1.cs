using System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000C7 RID: 199
	internal static class IAsyncOperationMethods<TResult>
	{
		// Token: 0x060007A1 RID: 1953 RVA: 0x00013418 File Offset: 0x00011618
		public static TResult GetResults(IObjectReference _genericObj)
		{
			ObjectReference<IAsyncOperation<TResult>.Vftbl> objectReference = (ObjectReference<IAsyncOperation<TResult>.Vftbl>)_genericObj;
			IntPtr thisPtr = objectReference.ThisPtr;
			object[] array = new object[2];
			array[0] = thisPtr;
			object[] array2 = array;
			TResult result;
			try
			{
				objectReference.Vftbl.GetResults_2.DynamicInvokeAbi(array2);
				result = Marshaler<TResult>.FromAbi.Invoke(array2[1]);
			}
			finally
			{
				Marshaler<TResult>.DisposeAbi.Invoke(array2[1]);
			}
			return result;
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00013484 File Offset: 0x00011684
		public static Windows.Foundation.AsyncOperationCompletedHandler<TResult> get_Completed(IObjectReference _genericObj)
		{
			ObjectReference<IAsyncOperation<TResult>.Vftbl> objectReference = (ObjectReference<IAsyncOperation<TResult>.Vftbl>)_genericObj;
			IntPtr thisPtr = objectReference.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			Windows.Foundation.AsyncOperationCompletedHandler<TResult> result;
			try
			{
				method get_Completed_ = objectReference.Vftbl.get_Completed_1;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, get_Completed_));
				result = AsyncOperationCompletedHandler<TResult>.FromAbi(intPtr);
			}
			finally
			{
				AsyncOperationCompletedHandler<TResult>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x000134EC File Offset: 0x000116EC
		public static void set_Completed(IObjectReference _genericObj, Windows.Foundation.AsyncOperationCompletedHandler<TResult> value)
		{
			ObjectReference<IAsyncOperation<TResult>.Vftbl> objectReference = (ObjectReference<IAsyncOperation<TResult>.Vftbl>)_genericObj;
			IntPtr thisPtr = objectReference.ThisPtr;
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			try
			{
				objectReferenceValue = AsyncOperationCompletedHandler<TResult>.CreateMarshaler2(value);
				method put_Completed_ = objectReference.Vftbl.put_Completed_0;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr), thisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), put_Completed_));
			}
			finally
			{
				MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
			}
		}
	}
}
