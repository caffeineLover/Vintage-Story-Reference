using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000DD RID: 221
	[Guid("C1D432BA-C824-4452-A7FD-512BC3BBE9A1")]
	internal interface IUriEscapeStatics : Windows.Foundation.IUriEscapeStatics
	{
		// Token: 0x0600082A RID: 2090 RVA: 0x000169C4 File Offset: 0x00014BC4
		unsafe static IUriEscapeStatics()
		{
			*(IInspectable.Vftbl*)((void*)IUriEscapeStatics.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IUriEscapeStatics.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_UnescapeComponent_0);
			*(IntPtr*)((byte*)((void*)IUriEscapeStatics.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_EscapeComponent_1);
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00016A40 File Offset: 0x00014C40
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_UnescapeComponent_0(IntPtr thisPtr, IntPtr toUnescape, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string value2 = ComWrappersSupport.FindObject<Windows.Foundation.IUriEscapeStatics>(thisPtr).UnescapeComponent(MarshalString.FromAbi(toUnescape));
				*value = MarshalString.FromManaged(value2);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00016A94 File Offset: 0x00014C94
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_EscapeComponent_1(IntPtr thisPtr, IntPtr toEscape, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string value2 = ComWrappersSupport.FindObject<Windows.Foundation.IUriEscapeStatics>(thisPtr).EscapeComponent(MarshalString.FromAbi(toEscape));
				*value = MarshalString.FromManaged(value2);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x04000187 RID: 391
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IUriEscapeStatics), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 2);
	}
}
