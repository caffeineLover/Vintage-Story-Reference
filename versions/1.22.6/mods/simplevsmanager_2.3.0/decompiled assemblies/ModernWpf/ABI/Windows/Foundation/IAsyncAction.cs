using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000BE RID: 190
	[DynamicInterfaceCastableImplementation]
	[Guid("5A648006-843A-4DA9-865B-9D26E5DFAD7B")]
	internal interface IAsyncAction : Windows.Foundation.IAsyncAction, Windows.Foundation.IAsyncInfo
	{
		// Token: 0x06000765 RID: 1893 RVA: 0x000127BC File Offset: 0x000109BC
		unsafe static IAsyncAction()
		{
			*(IInspectable.Vftbl*)((void*)IAsyncAction.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IAsyncAction.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_put_Completed_0);
			*(IntPtr*)((byte*)((void*)IAsyncAction.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Completed_1);
			*(IntPtr*)((byte*)((void*)IAsyncAction.AbiToProjectionVftablePtr) + (IntPtr)8 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_GetResults_2);
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00012854 File Offset: 0x00010A54
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private static int Do_Abi_GetResults_2(IntPtr thisPtr)
		{
			try
			{
				ComWrappersSupport.FindObject<Windows.Foundation.IAsyncAction>(thisPtr).GetResults();
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00012890 File Offset: 0x00010A90
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private static int Do_Abi_put_Completed_0(IntPtr thisPtr, IntPtr handler)
		{
			try
			{
				ComWrappersSupport.FindObject<Windows.Foundation.IAsyncAction>(thisPtr).Completed = AsyncActionCompletedHandler.FromAbi(handler);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x000128D4 File Offset: 0x00010AD4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Completed_1(IntPtr thisPtr, IntPtr* handler)
		{
			*handler = default(IntPtr);
			try
			{
				Windows.Foundation.AsyncActionCompletedHandler completed = ComWrappersSupport.FindObject<Windows.Foundation.IAsyncAction>(thisPtr).Completed;
				*handler = AsyncActionCompletedHandler.FromManaged(completed);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x00012924 File Offset: 0x00010B24
		void GetResults()
		{
			IAsyncActionMethods.GetResults(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncAction).TypeHandle));
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x00012945 File Offset: 0x00010B45
		// (set) Token: 0x0600076B RID: 1899 RVA: 0x00012966 File Offset: 0x00010B66
		Windows.Foundation.AsyncActionCompletedHandler Windows.Foundation.IAsyncAction.Completed
		{
			get
			{
				return IAsyncActionMethods.get_Completed(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncAction).TypeHandle));
			}
			set
			{
				IAsyncActionMethods.set_Completed(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncAction).TypeHandle), value);
			}
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00012988 File Offset: 0x00010B88
		void Cancel()
		{
			((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).Cancel();
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0001299A File Offset: 0x00010B9A
		void Close()
		{
			((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).Close();
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x000129AC File Offset: 0x00010BAC
		Exception Windows.Foundation.IAsyncInfo.ErrorCode
		{
			get
			{
				return ((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).ErrorCode;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x000129BE File Offset: 0x00010BBE
		uint Windows.Foundation.IAsyncInfo.Id
		{
			get
			{
				return ((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).Id;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x000129D0 File Offset: 0x00010BD0
		AsyncStatus Windows.Foundation.IAsyncInfo.Status
		{
			get
			{
				return ((Windows.Foundation.IAsyncInfo)((IWinRTObject)this)).Status;
			}
		}

		// Token: 0x04000177 RID: 375
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IAsyncAction), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 3);
	}
}
