using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000E3 RID: 227
	[Guid("758D9661-221C-480F-A339-50656673F46F")]
	internal interface IUriRuntimeClassWithAbsoluteCanonicalUri : Windows.Foundation.IUriRuntimeClassWithAbsoluteCanonicalUri
	{
		// Token: 0x06000857 RID: 2135 RVA: 0x00017BD0 File Offset: 0x00015DD0
		unsafe static IUriRuntimeClassWithAbsoluteCanonicalUri()
		{
			*(IInspectable.Vftbl*)((void*)IUriRuntimeClassWithAbsoluteCanonicalUri.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IUriRuntimeClassWithAbsoluteCanonicalUri.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_AbsoluteCanonicalUri_0);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClassWithAbsoluteCanonicalUri.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DisplayIri_1);
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00017C4C File Offset: 0x00015E4C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_AbsoluteCanonicalUri_0(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string absoluteCanonicalUri = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClassWithAbsoluteCanonicalUri>(thisPtr).AbsoluteCanonicalUri;
				*value = MarshalString.FromManaged(absoluteCanonicalUri);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x00017C9C File Offset: 0x00015E9C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DisplayIri_1(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string displayIri = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClassWithAbsoluteCanonicalUri>(thisPtr).DisplayIri;
				*value = MarshalString.FromManaged(displayIri);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0400018A RID: 394
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IUriRuntimeClassWithAbsoluteCanonicalUri), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 2);
	}
}
