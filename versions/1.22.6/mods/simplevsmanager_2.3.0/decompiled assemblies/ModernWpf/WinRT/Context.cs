using System;
using System.Runtime.InteropServices;
using ABI.WinRT.Interop;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x0200018C RID: 396
	internal static class Context
	{
		// Token: 0x06000C8B RID: 3211
		[DllImport("api-ms-win-core-com-l1-1-0.dll")]
		private unsafe static extern int CoGetContextToken(IntPtr* contextToken);

		// Token: 0x06000C8C RID: 3212
		[DllImport("api-ms-win-core-com-l1-1-0.dll")]
		private static extern int CoGetObjectContext(ref Guid riid, out IntPtr ppv);

		// Token: 0x06000C8D RID: 3213 RVA: 0x00023A8C File Offset: 0x00021C8C
		public static IntPtr GetContextCallback()
		{
			Guid iid = ABI.WinRT.Interop.IContextCallback.IID;
			IntPtr result;
			Marshal.ThrowExceptionForHR(Context.CoGetObjectContext(ref iid, out result));
			return result;
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x00023AB0 File Offset: 0x00021CB0
		public unsafe static IntPtr GetContextToken()
		{
			IntPtr result;
			Marshal.ThrowExceptionForHR(Context.CoGetContextToken(&result));
			return result;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00023ACC File Offset: 0x00021CCC
		public unsafe static void CallInContext(IntPtr contextCallbackPtr, IntPtr contextToken, Action callback, Action onFailCallback)
		{
			if (contextCallbackPtr == IntPtr.Zero || Context.GetContextToken() == contextToken)
			{
				callback.Invoke();
				return;
			}
			ComCallData comCallData = default(ComCallData);
			ABI.WinRT.Interop.IContextCallback contextCallback = new ABI.WinRT.Interop.IContextCallback(ObjectReference<ABI.WinRT.Interop.IContextCallback.Vftbl>.FromAbi(contextCallbackPtr));
			try
			{
				contextCallback.ContextCallback(delegate(ComCallData* _)
				{
					callback.Invoke();
					return 0;
				}, &comCallData, Context.IID_ICallbackWithNoReentrancyToApplicationSTA, 5);
			}
			catch (Exception)
			{
				if (onFailCallback != null)
				{
					onFailCallback.Invoke();
				}
			}
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00023B5C File Offset: 0x00021D5C
		public static void DisposeContextCallback(IntPtr contextCallbackPtr)
		{
			MarshalInspectable<object>.DisposeAbi(contextCallbackPtr);
		}

		// Token: 0x0400021F RID: 543
		private static readonly Guid IID_ICallbackWithNoReentrancyToApplicationSTA = new Guid(170497908U, 15950, 64578, 29, 157, 114, 206, 225, 5, 202, 87);
	}
}
