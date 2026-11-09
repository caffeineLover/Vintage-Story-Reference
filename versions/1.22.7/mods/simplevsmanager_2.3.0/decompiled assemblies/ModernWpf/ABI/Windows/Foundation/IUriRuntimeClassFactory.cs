using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ABI.System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000E1 RID: 225
	[Guid("44A9796F-723E-4FDF-A218-033E75B0C084")]
	internal interface IUriRuntimeClassFactory : Windows.Foundation.IUriRuntimeClassFactory
	{
		// Token: 0x06000852 RID: 2130 RVA: 0x000179E4 File Offset: 0x00015BE4
		unsafe static IUriRuntimeClassFactory()
		{
			*(IInspectable.Vftbl*)((void*)IUriRuntimeClassFactory.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IUriRuntimeClassFactory.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateUri_0);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClassFactory.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateWithRelativeUri_1);
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00017A60 File Offset: 0x00015C60
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateUri_0(IntPtr thisPtr, IntPtr uri, IntPtr* instance)
		{
			*instance = default(IntPtr);
			try
			{
				Uri value = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClassFactory>(thisPtr).CreateUri(MarshalString.FromAbi(uri));
				*instance = Uri.FromManaged(value);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00017AB4 File Offset: 0x00015CB4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateWithRelativeUri_1(IntPtr thisPtr, IntPtr baseUri, IntPtr relativeUri, IntPtr* instance)
		{
			*instance = default(IntPtr);
			try
			{
				Uri value = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClassFactory>(thisPtr).CreateWithRelativeUri(MarshalString.FromAbi(baseUri), MarshalString.FromAbi(relativeUri));
				*instance = Uri.FromManaged(value);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x04000189 RID: 393
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IUriRuntimeClassFactory), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 2);
	}
}
