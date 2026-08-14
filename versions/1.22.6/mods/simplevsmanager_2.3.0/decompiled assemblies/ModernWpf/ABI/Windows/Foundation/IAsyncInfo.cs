using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ABI.System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000C3 RID: 195
	[DynamicInterfaceCastableImplementation]
	[Guid("00000036-0000-0000-C000-000000000046")]
	internal interface IAsyncInfo : Windows.Foundation.IAsyncInfo
	{
		// Token: 0x06000786 RID: 1926 RVA: 0x00012E18 File Offset: 0x00011018
		unsafe static IAsyncInfo()
		{
			*(IInspectable.Vftbl*)((void*)IAsyncInfo.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IAsyncInfo.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Id_0);
			*(IntPtr*)((byte*)((void*)IAsyncInfo.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Status_1);
			*(IntPtr*)((byte*)((void*)IAsyncInfo.AbiToProjectionVftablePtr) + (IntPtr)8 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_ErrorCode_2);
			*(IntPtr*)((byte*)((void*)IAsyncInfo.AbiToProjectionVftablePtr) + (IntPtr)9 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_Cancel_3);
			*(IntPtr*)((byte*)((void*)IAsyncInfo.AbiToProjectionVftablePtr) + (IntPtr)10 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_Close_4);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00012EE8 File Offset: 0x000110E8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private static int Do_Abi_Cancel_3(IntPtr thisPtr)
		{
			try
			{
				ComWrappersSupport.FindObject<Windows.Foundation.IAsyncInfo>(thisPtr).Cancel();
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00012F24 File Offset: 0x00011124
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private static int Do_Abi_Close_4(IntPtr thisPtr)
		{
			try
			{
				ComWrappersSupport.FindObject<Windows.Foundation.IAsyncInfo>(thisPtr).Close();
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00012F60 File Offset: 0x00011160
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_ErrorCode_2(IntPtr thisPtr, Exception* __return_value__)
		{
			*__return_value__ = default(Exception);
			try
			{
				Exception errorCode = ComWrappersSupport.FindObject<Windows.Foundation.IAsyncInfo>(thisPtr).ErrorCode;
				*__return_value__ = Exception.FromManaged(errorCode);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00012FB4 File Offset: 0x000111B4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Id_0(IntPtr thisPtr, uint* __return_value__)
		{
			*__return_value__ = 0U;
			try
			{
				uint id = ComWrappersSupport.FindObject<Windows.Foundation.IAsyncInfo>(thisPtr).Id;
				*__return_value__ = id;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00012FF8 File Offset: 0x000111F8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Status_1(IntPtr thisPtr, AsyncStatus* __return_value__)
		{
			*__return_value__ = AsyncStatus.Started;
			try
			{
				AsyncStatus status = ComWrappersSupport.FindObject<Windows.Foundation.IAsyncInfo>(thisPtr).Status;
				*__return_value__ = status;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x0001303C File Offset: 0x0001123C
		void Cancel()
		{
			IAsyncInfoMethods.Cancel(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncInfo).TypeHandle));
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x0001305D File Offset: 0x0001125D
		void Close()
		{
			IAsyncInfoMethods.Close(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncInfo).TypeHandle));
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x0001307E File Offset: 0x0001127E
		Exception Windows.Foundation.IAsyncInfo.ErrorCode
		{
			get
			{
				return IAsyncInfoMethods.get_ErrorCode(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncInfo).TypeHandle));
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x0001309F File Offset: 0x0001129F
		uint Windows.Foundation.IAsyncInfo.Id
		{
			get
			{
				return IAsyncInfoMethods.get_Id(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncInfo).TypeHandle));
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x000130C0 File Offset: 0x000112C0
		AsyncStatus Windows.Foundation.IAsyncInfo.Status
		{
			get
			{
				return IAsyncInfoMethods.get_Status(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IAsyncInfo).TypeHandle));
			}
		}

		// Token: 0x04000179 RID: 377
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IAsyncInfo), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 5);
	}
}
