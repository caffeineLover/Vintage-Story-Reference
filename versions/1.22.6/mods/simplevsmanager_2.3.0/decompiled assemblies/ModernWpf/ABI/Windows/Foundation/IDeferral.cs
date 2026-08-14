using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000CB RID: 203
	[Guid("D6269732-3B7F-46A7-B40B-4FDCA2A2C693")]
	internal interface IDeferral : Windows.Foundation.IDeferral, IDisposable
	{
		// Token: 0x060007AE RID: 1966 RVA: 0x00013664 File Offset: 0x00011864
		unsafe static IDeferral()
		{
			*(IInspectable.Vftbl*)((void*)IDeferral.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IDeferral.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_Complete_0);
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x000136C4 File Offset: 0x000118C4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private static int Do_Abi_Complete_0(IntPtr thisPtr)
		{
			try
			{
				ComWrappersSupport.FindObject<Windows.Foundation.IDeferral>(thisPtr).Complete();
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00013700 File Offset: 0x00011900
		void Dispose()
		{
			((IDisposable)((IWinRTObject)this)).Dispose();
		}

		// Token: 0x0400017C RID: 380
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IDeferral), sizeof(IInspectable.Vftbl) + sizeof(IntPtr));
	}
}
