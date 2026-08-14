using System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000BF RID: 191
	internal static class IAsyncActionWithProgressMethods<TProgress>
	{
		// Token: 0x06000771 RID: 1905 RVA: 0x000129E4 File Offset: 0x00010BE4
		public static void GetResults(IObjectReference _genericObj)
		{
			ObjectReference<IAsyncActionWithProgress<TProgress>.Vftbl> objectReference = (ObjectReference<IAsyncActionWithProgress<TProgress>.Vftbl>)_genericObj;
			IntPtr thisPtr = objectReference.ThisPtr;
			method getResults_ = objectReference.Vftbl.GetResults_4;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr), thisPtr, getResults_));
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x00012A18 File Offset: 0x00010C18
		public static Windows.Foundation.AsyncActionProgressHandler<TProgress> get_Progress(IObjectReference _genericObj)
		{
			ObjectReference<IAsyncActionWithProgress<TProgress>.Vftbl> objectReference = (ObjectReference<IAsyncActionWithProgress<TProgress>.Vftbl>)_genericObj;
			IntPtr thisPtr = objectReference.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			Windows.Foundation.AsyncActionProgressHandler<TProgress> result;
			try
			{
				method get_Progress_ = objectReference.Vftbl.get_Progress_1;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, get_Progress_));
				result = AsyncActionProgressHandler<TProgress>.FromAbi(intPtr);
			}
			finally
			{
				AsyncActionProgressHandler<TProgress>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00012A80 File Offset: 0x00010C80
		public static void set_Progress(IObjectReference _genericObj, Windows.Foundation.AsyncActionProgressHandler<TProgress> value)
		{
			ObjectReference<IAsyncActionWithProgress<TProgress>.Vftbl> objectReference = (ObjectReference<IAsyncActionWithProgress<TProgress>.Vftbl>)_genericObj;
			IntPtr thisPtr = objectReference.ThisPtr;
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			try
			{
				objectReferenceValue = AsyncActionProgressHandler<TProgress>.CreateMarshaler2(value);
				method put_Progress_ = objectReference.Vftbl.put_Progress_0;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr), thisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), put_Progress_));
			}
			finally
			{
				MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
			}
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00012AE8 File Offset: 0x00010CE8
		public static Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress> get_Completed(IObjectReference _genericObj)
		{
			ObjectReference<IAsyncActionWithProgress<TProgress>.Vftbl> objectReference = (ObjectReference<IAsyncActionWithProgress<TProgress>.Vftbl>)_genericObj;
			IntPtr thisPtr = objectReference.ThisPtr;
			IntPtr intPtr = default(IntPtr);
			Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress> result;
			try
			{
				method get_Completed_ = objectReference.Vftbl.get_Completed_3;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref intPtr, get_Completed_));
				result = AsyncActionWithProgressCompletedHandler<TProgress>.FromAbi(intPtr);
			}
			finally
			{
				AsyncActionWithProgressCompletedHandler<TProgress>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00012B50 File Offset: 0x00010D50
		public static void set_Completed(IObjectReference _genericObj, Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress> value)
		{
			ObjectReference<IAsyncActionWithProgress<TProgress>.Vftbl> objectReference = (ObjectReference<IAsyncActionWithProgress<TProgress>.Vftbl>)_genericObj;
			IntPtr thisPtr = objectReference.ThisPtr;
			ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
			try
			{
				objectReferenceValue = AsyncActionWithProgressCompletedHandler<TProgress>.CreateMarshaler2(value);
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
