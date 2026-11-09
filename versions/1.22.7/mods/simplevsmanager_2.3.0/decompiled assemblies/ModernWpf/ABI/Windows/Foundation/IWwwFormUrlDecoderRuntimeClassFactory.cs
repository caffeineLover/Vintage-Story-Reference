using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000E9 RID: 233
	[Guid("5B8C6B3D-24AE-41B5-A1BF-F0C3D544845B")]
	internal interface IWwwFormUrlDecoderRuntimeClassFactory : Windows.Foundation.IWwwFormUrlDecoderRuntimeClassFactory
	{
		// Token: 0x06000869 RID: 2153 RVA: 0x00018118 File Offset: 0x00016318
		unsafe static IWwwFormUrlDecoderRuntimeClassFactory()
		{
			*(IInspectable.Vftbl*)((void*)IWwwFormUrlDecoderRuntimeClassFactory.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IWwwFormUrlDecoderRuntimeClassFactory.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateWwwFormUrlDecoder_0);
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00018178 File Offset: 0x00016378
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateWwwFormUrlDecoder_0(IntPtr thisPtr, IntPtr query, IntPtr* instance)
		{
			*instance = default(IntPtr);
			try
			{
				Windows.Foundation.WwwFormUrlDecoder obj = ComWrappersSupport.FindObject<Windows.Foundation.IWwwFormUrlDecoderRuntimeClassFactory>(thisPtr).CreateWwwFormUrlDecoder(MarshalString.FromAbi(query));
				*instance = WwwFormUrlDecoder.FromManaged(obj);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0400018D RID: 397
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IWwwFormUrlDecoderRuntimeClassFactory), sizeof(IInspectable.Vftbl) + sizeof(IntPtr));
	}
}
