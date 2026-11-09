using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Metadata;
using WinRT;

namespace ABI.Windows.Foundation.Metadata
{
	// Token: 0x020000EF RID: 239
	[Guid("997439FE-F681-4A11-B416-C13A47E8BA36")]
	internal interface IApiInformationStatics : Windows.Foundation.Metadata.IApiInformationStatics
	{
		// Token: 0x060008A7 RID: 2215 RVA: 0x00018B0C File Offset: 0x00016D0C
		unsafe static IApiInformationStatics()
		{
			*(IInspectable.Vftbl*)((void*)IApiInformationStatics.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IApiInformationStatics.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_IsTypePresent_0);
			*(IntPtr*)((byte*)((void*)IApiInformationStatics.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_IsMethodPresent_1);
			*(IntPtr*)((byte*)((void*)IApiInformationStatics.AbiToProjectionVftablePtr) + (IntPtr)8 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_IsMethodPresent_2);
			*(IntPtr*)((byte*)((void*)IApiInformationStatics.AbiToProjectionVftablePtr) + (IntPtr)9 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_IsEventPresent_3);
			*(IntPtr*)((byte*)((void*)IApiInformationStatics.AbiToProjectionVftablePtr) + (IntPtr)10 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_IsPropertyPresent_4);
			*(IntPtr*)((byte*)((void*)IApiInformationStatics.AbiToProjectionVftablePtr) + (IntPtr)11 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_IsReadOnlyPropertyPresent_5);
			*(IntPtr*)((byte*)((void*)IApiInformationStatics.AbiToProjectionVftablePtr) + (IntPtr)12 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_IsWriteablePropertyPresent_6);
			*(IntPtr*)((byte*)((void*)IApiInformationStatics.AbiToProjectionVftablePtr) + (IntPtr)13 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_IsEnumNamedValuePresent_7);
			*(IntPtr*)((byte*)((void*)IApiInformationStatics.AbiToProjectionVftablePtr) + (IntPtr)14 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_IsApiContractPresent_8);
			*(IntPtr*)((byte*)((void*)IApiInformationStatics.AbiToProjectionVftablePtr) + (IntPtr)15 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_IsApiContractPresent_9);
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00018C68 File Offset: 0x00016E68
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_IsTypePresent_0(IntPtr thisPtr, IntPtr typeName, byte* result)
		{
			*result = 0;
			try
			{
				*result = (ComWrappersSupport.FindObject<Windows.Foundation.Metadata.IApiInformationStatics>(thisPtr).IsTypePresent(MarshalString.FromAbi(typeName)) ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00018CBC File Offset: 0x00016EBC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_IsMethodPresent_1(IntPtr thisPtr, IntPtr typeName, IntPtr methodName, byte* result)
		{
			*result = 0;
			try
			{
				*result = (ComWrappersSupport.FindObject<Windows.Foundation.Metadata.IApiInformationStatics>(thisPtr).IsMethodPresent(MarshalString.FromAbi(typeName), MarshalString.FromAbi(methodName)) ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x00018D14 File Offset: 0x00016F14
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_IsMethodPresent_2(IntPtr thisPtr, IntPtr typeName, IntPtr methodName, uint inputParameterCount, byte* result)
		{
			*result = 0;
			try
			{
				*result = (ComWrappersSupport.FindObject<Windows.Foundation.Metadata.IApiInformationStatics>(thisPtr).IsMethodPresent(MarshalString.FromAbi(typeName), MarshalString.FromAbi(methodName), inputParameterCount) ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x00018D70 File Offset: 0x00016F70
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_IsEventPresent_3(IntPtr thisPtr, IntPtr typeName, IntPtr eventName, byte* result)
		{
			*result = 0;
			try
			{
				*result = (ComWrappersSupport.FindObject<Windows.Foundation.Metadata.IApiInformationStatics>(thisPtr).IsEventPresent(MarshalString.FromAbi(typeName), MarshalString.FromAbi(eventName)) ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00018DC8 File Offset: 0x00016FC8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_IsPropertyPresent_4(IntPtr thisPtr, IntPtr typeName, IntPtr propertyName, byte* result)
		{
			*result = 0;
			try
			{
				*result = (ComWrappersSupport.FindObject<Windows.Foundation.Metadata.IApiInformationStatics>(thisPtr).IsPropertyPresent(MarshalString.FromAbi(typeName), MarshalString.FromAbi(propertyName)) ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00018E20 File Offset: 0x00017020
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_IsReadOnlyPropertyPresent_5(IntPtr thisPtr, IntPtr typeName, IntPtr propertyName, byte* result)
		{
			*result = 0;
			try
			{
				*result = (ComWrappersSupport.FindObject<Windows.Foundation.Metadata.IApiInformationStatics>(thisPtr).IsReadOnlyPropertyPresent(MarshalString.FromAbi(typeName), MarshalString.FromAbi(propertyName)) ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x00018E78 File Offset: 0x00017078
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_IsWriteablePropertyPresent_6(IntPtr thisPtr, IntPtr typeName, IntPtr propertyName, byte* result)
		{
			*result = 0;
			try
			{
				*result = (ComWrappersSupport.FindObject<Windows.Foundation.Metadata.IApiInformationStatics>(thisPtr).IsWriteablePropertyPresent(MarshalString.FromAbi(typeName), MarshalString.FromAbi(propertyName)) ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x00018ED0 File Offset: 0x000170D0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_IsEnumNamedValuePresent_7(IntPtr thisPtr, IntPtr enumTypeName, IntPtr valueName, byte* result)
		{
			*result = 0;
			try
			{
				*result = (ComWrappersSupport.FindObject<Windows.Foundation.Metadata.IApiInformationStatics>(thisPtr).IsEnumNamedValuePresent(MarshalString.FromAbi(enumTypeName), MarshalString.FromAbi(valueName)) ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00018F28 File Offset: 0x00017128
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_IsApiContractPresent_8(IntPtr thisPtr, IntPtr contractName, ushort majorVersion, byte* result)
		{
			*result = 0;
			try
			{
				*result = (ComWrappersSupport.FindObject<Windows.Foundation.Metadata.IApiInformationStatics>(thisPtr).IsApiContractPresent(MarshalString.FromAbi(contractName), majorVersion) ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00018F7C File Offset: 0x0001717C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_IsApiContractPresent_9(IntPtr thisPtr, IntPtr contractName, ushort majorVersion, ushort minorVersion, byte* result)
		{
			*result = 0;
			try
			{
				*result = (ComWrappersSupport.FindObject<Windows.Foundation.Metadata.IApiInformationStatics>(thisPtr).IsApiContractPresent(MarshalString.FromAbi(contractName), majorVersion, minorVersion) ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x04000193 RID: 403
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IApiInformationStatics), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 10);
	}
}
