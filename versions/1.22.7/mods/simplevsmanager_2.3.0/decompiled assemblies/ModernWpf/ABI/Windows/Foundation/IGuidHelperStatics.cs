using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000D1 RID: 209
	[Guid("59C7966B-AE52-5283-AD7F-A1B9E9678ADD")]
	internal interface IGuidHelperStatics : Windows.Foundation.IGuidHelperStatics
	{
		// Token: 0x060007BB RID: 1979 RVA: 0x00013A68 File Offset: 0x00011C68
		unsafe static IGuidHelperStatics()
		{
			*(IInspectable.Vftbl*)((void*)IGuidHelperStatics.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IGuidHelperStatics.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateNewGuid_0);
			*(IntPtr*)((byte*)((void*)IGuidHelperStatics.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Empty_1);
			*(IntPtr*)((byte*)((void*)IGuidHelperStatics.AbiToProjectionVftablePtr) + (IntPtr)8 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_Equals_2);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x00013B00 File Offset: 0x00011D00
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateNewGuid_0(IntPtr thisPtr, Guid* result)
		{
			Guid guid = default(Guid);
			*result = default(Guid);
			try
			{
				guid = ComWrappersSupport.FindObject<Windows.Foundation.IGuidHelperStatics>(thisPtr).CreateNewGuid();
				*result = guid;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x00013B54 File Offset: 0x00011D54
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_Equals_2(IntPtr thisPtr, Guid* target, Guid* value, byte* result)
		{
			*result = 0;
			try
			{
				*result = (ComWrappersSupport.FindObject<Windows.Foundation.IGuidHelperStatics>(thisPtr).Equals(target, value) ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x00013BA4 File Offset: 0x00011DA4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Empty_1(IntPtr thisPtr, Guid* value)
		{
			Guid guid = default(Guid);
			*value = default(Guid);
			try
			{
				guid = ComWrappersSupport.FindObject<Windows.Foundation.IGuidHelperStatics>(thisPtr).Empty;
				*value = guid;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0400017F RID: 383
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IGuidHelperStatics), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 3);
	}
}
