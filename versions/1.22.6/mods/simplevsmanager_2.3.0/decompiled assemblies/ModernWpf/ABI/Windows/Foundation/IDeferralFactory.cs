using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000CD RID: 205
	[Guid("65A1ECC5-3FB5-4832-8CA9-F061B281D13A")]
	internal interface IDeferralFactory : Windows.Foundation.IDeferralFactory
	{
		// Token: 0x060007B2 RID: 1970 RVA: 0x00013790 File Offset: 0x00011990
		unsafe static IDeferralFactory()
		{
			*(IInspectable.Vftbl*)((void*)IDeferralFactory.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IDeferralFactory.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_Create_0);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x000137F0 File Offset: 0x000119F0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_Create_0(IntPtr thisPtr, IntPtr handler, IntPtr* result)
		{
			*result = default(IntPtr);
			try
			{
				Windows.Foundation.Deferral obj = ComWrappersSupport.FindObject<Windows.Foundation.IDeferralFactory>(thisPtr).Create(DeferralCompletedHandler.FromAbi(handler));
				*result = Deferral.FromManaged(obj);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0400017D RID: 381
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IDeferralFactory), sizeof(IInspectable.Vftbl) + sizeof(IntPtr));
	}
}
