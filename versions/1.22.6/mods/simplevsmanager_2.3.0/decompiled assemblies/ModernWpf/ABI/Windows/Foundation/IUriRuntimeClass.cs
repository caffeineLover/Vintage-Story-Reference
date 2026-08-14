using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ABI.System;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000DF RID: 223
	[Guid("9E365E57-48B2-4160-956F-C7385120BBFC")]
	internal interface IUriRuntimeClass : Windows.Foundation.IUriRuntimeClass
	{
		// Token: 0x0600083E RID: 2110 RVA: 0x00017140 File Offset: 0x00015340
		unsafe static IUriRuntimeClass()
		{
			*(IInspectable.Vftbl*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_AbsoluteUri_0);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_DisplayUri_1);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)8 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Domain_2);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)9 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Extension_3);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)10 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Fragment_4);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)11 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Host_5);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)12 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Password_6);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)13 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Path_7);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)14 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Query_8);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)15 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_QueryParsed_9);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)16 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_RawUri_10);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)17 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_SchemeName_11);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)18 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_UserName_12);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)19 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Port_13);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)20 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Suspicious_14);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)21 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_Equals_15);
			*(IntPtr*)((byte*)((void*)IUriRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)22 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CombineUri_16);
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x00017360 File Offset: 0x00015560
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_Equals_15(IntPtr thisPtr, IntPtr pUri, byte* value)
		{
			*value = 0;
			try
			{
				*value = (ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).Equals(Uri.FromAbi(pUri)) ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x000173B4 File Offset: 0x000155B4
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CombineUri_16(IntPtr thisPtr, IntPtr relativeUri, IntPtr* instance)
		{
			*instance = default(IntPtr);
			try
			{
				Uri value = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).CombineUri(MarshalString.FromAbi(relativeUri));
				*instance = Uri.FromManaged(value);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00017408 File Offset: 0x00015608
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_AbsoluteUri_0(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string absoluteUri = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).AbsoluteUri;
				*value = MarshalString.FromManaged(absoluteUri);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x00017458 File Offset: 0x00015658
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_DisplayUri_1(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string displayUri = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).DisplayUri;
				*value = MarshalString.FromManaged(displayUri);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x000174A8 File Offset: 0x000156A8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Domain_2(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string domain = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).Domain;
				*value = MarshalString.FromManaged(domain);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x000174F8 File Offset: 0x000156F8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Extension_3(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string extension = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).Extension;
				*value = MarshalString.FromManaged(extension);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00017548 File Offset: 0x00015748
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Fragment_4(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string fragment = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).Fragment;
				*value = MarshalString.FromManaged(fragment);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00017598 File Offset: 0x00015798
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Host_5(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string host = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).Host;
				*value = MarshalString.FromManaged(host);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x000175E8 File Offset: 0x000157E8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Password_6(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string password = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).Password;
				*value = MarshalString.FromManaged(password);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00017638 File Offset: 0x00015838
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Path_7(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string path = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).Path;
				*value = MarshalString.FromManaged(path);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00017688 File Offset: 0x00015888
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Port_13(IntPtr thisPtr, int* value)
		{
			*value = 0;
			try
			{
				int port = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).Port;
				*value = port;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x000176CC File Offset: 0x000158CC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Query_8(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string query = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).Query;
				*value = MarshalString.FromManaged(query);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0001771C File Offset: 0x0001591C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_QueryParsed_9(IntPtr thisPtr, IntPtr* ppWwwFormUrlDecoder)
		{
			*ppWwwFormUrlDecoder = default(IntPtr);
			try
			{
				Windows.Foundation.WwwFormUrlDecoder queryParsed = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).QueryParsed;
				*ppWwwFormUrlDecoder = WwwFormUrlDecoder.FromManaged(queryParsed);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0001776C File Offset: 0x0001596C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_RawUri_10(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string rawUri = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).RawUri;
				*value = MarshalString.FromManaged(rawUri);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x000177BC File Offset: 0x000159BC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_SchemeName_11(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string schemeName = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).SchemeName;
				*value = MarshalString.FromManaged(schemeName);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0001780C File Offset: 0x00015A0C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Suspicious_14(IntPtr thisPtr, byte* value)
		{
			*value = 0;
			try
			{
				*value = (ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).Suspicious ? 1 : 0);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x00017858 File Offset: 0x00015A58
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_UserName_12(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string userName = ComWrappersSupport.FindObject<Windows.Foundation.IUriRuntimeClass>(thisPtr).UserName;
				*value = MarshalString.FromManaged(userName);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x04000188 RID: 392
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IUriRuntimeClass), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 17);
	}
}
