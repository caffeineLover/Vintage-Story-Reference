using System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000C4 RID: 196
	internal static class IAsyncOperationWithProgressMethods<TResult, TProgress>
	{
		// Token: 0x06000791 RID: 1937 RVA: 0x000130E4 File Offset: 0x000112E4
		public static TResult GetResults(IObjectReference _genericObj)
		{
			ObjectReference<IAsyncOperationWithProgress<TResult, TProgress>.Vftbl> objectReference = (ObjectReference<IAsyncOperationWithProgress<TResult, TProgress>.Vftbl>)_genericObj;
			IntPtr thisPtr = objectReference.ThisPtr;
			object[] array = new object[2];
			array[0] = thisPtr;
			object[] array2 = array;
			TResult result;
			try
			{
				objectReference.Vftbl.GetResults_4.DynamicInvokeAbi(array2);
				result = Marshaler<TResult>.FromAbi.Invoke(array2[1]);
			}
			finally
			{
				Marshaler<TResult>.DisposeAbi.Invoke(array2[1]);
			}
			return result;
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00013150 File Offset: 0x00011350
		public static Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress> get_Progress(IObjectReference _genericObj)
		{
			ObjectReference<IAsyncOperationWithProgress<TResult, TProgress>.Vftbl> objectReference = (ObjectReference<IAsyncOperationWithProgress<TResult, TProgress>.Vftbl>)_genericObj;
			IntPtr thisPtr = objectReference.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress> result;
			try
			{
				method get_Progress_ = objectReference.Vftbl.get_Progress_1;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, get_Progress_));
				result = AsyncOperationProgressHandler<TResult, TProgress>.FromAbi(intPtr);
			}
			finally
			{
				AsyncOperationProgressHandler<TResult, TProgress>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x000131B8 File Offset: 0x000113B8
		public static void set_Progress(IObjectReference _genericObj, Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress> value)
		{
			ObjectReference<IAsyncOperationWithProgress<TResult, TProgress>.Vftbl> objectReference = (ObjectReference<IAsyncOperationWithProgress<TResult, TProgress>.Vftbl>)_genericObj;
			IntPtr thisPtr = objectReference.ThisPtr;
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			try
			{
				objectReferenceValue = AsyncOperationProgressHandler<TResult, TProgress>.CreateMarshaler2(value);
				method put_Progress_ = objectReference.Vftbl.put_Progress_0;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr), thisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), put_Progress_));
			}
			finally
			{
				MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
			}
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00013220 File Offset: 0x00011420
		public static Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress> get_Completed(IObjectReference _genericObj)
		{
			ObjectReference<IAsyncOperationWithProgress<TResult, TProgress>.Vftbl> objectReference = (ObjectReference<IAsyncOperationWithProgress<TResult, TProgress>.Vftbl>)_genericObj;
			IntPtr thisPtr = objectReference.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress> result;
			try
			{
				method get_Completed_ = objectReference.Vftbl.get_Completed_3;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, get_Completed_));
				result = AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.FromAbi(intPtr);
			}
			finally
			{
				AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00013288 File Offset: 0x00011488
		public static void set_Completed(IObjectReference _genericObj, Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress> value)
		{
			ObjectReference<IAsyncOperationWithProgress<TResult, TProgress>.Vftbl> objectReference = (ObjectReference<IAsyncOperationWithProgress<TResult, TProgress>.Vftbl>)_genericObj;
			IntPtr thisPtr = objectReference.ThisPtr;
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			try
			{
				objectReferenceValue = AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.CreateMarshaler2(value);
				method put_Completed_ = objectReference.Vftbl.put_Completed_2;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr), thisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), put_Completed_));
			}
			finally
			{
				MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
			}
		}
	}
}
