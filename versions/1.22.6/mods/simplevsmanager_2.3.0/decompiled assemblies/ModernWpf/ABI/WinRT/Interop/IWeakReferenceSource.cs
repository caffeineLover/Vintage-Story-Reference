using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x02000115 RID: 277
	[DynamicInterfaceCastableImplementation]
	[Guid("00000038-0000-0000-C000-000000000046")]
	internal interface IWeakReferenceSource : WinRT.Interop.IWeakReferenceSource
	{
		// Token: 0x060009AC RID: 2476 RVA: 0x0001B2E4 File Offset: 0x000194E4
		unsafe static IWeakReferenceSource()
		{
			*(IUnknownVftbl*)((void*)IWeakReferenceSource.AbiToProjectionVftablePtr) = IUnknownVftbl.AbiToProjectionVftbl;
			*(IntPtr*)((byte*)((void*)IWeakReferenceSource.AbiToProjectionVftablePtr) + (IntPtr)3 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_GetWeakReference);
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0001B34C File Offset: 0x0001954C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_GetWeakReference(IntPtr thisPtr, IntPtr* weakReference)
		{
			*weakReference = default(IntPtr);
			try
			{
				*weakReference = ComWrappersSupport.CreateCCWForObjectForABI(new ManagedWeakReference(ComWrappersSupport.FindObject<object>(thisPtr)), IWeakReference.IID);
			}
			catch (Exception ex)
			{
				return ex.HResult;
			}
			return 0;
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0001B394 File Offset: 0x00019594
		WinRT.Interop.IWeakReference GetWeakReference()
		{
			return IWeakReferenceSourceMethods.GetWeakReference(((IWinRTObject)this).GetObjectReferenceForType(typeof(WinRT.Interop.IWeakReferenceSource).TypeHandle));
		}

		// Token: 0x040001B5 RID: 437
		internal static readonly Guid IID = InterfaceIIDs.IWeakReferenceSource_IID;

		// Token: 0x040001B6 RID: 438
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IWeakReferenceSource), sizeof(IUnknownVftbl) + sizeof(IntPtr));
	}
}
