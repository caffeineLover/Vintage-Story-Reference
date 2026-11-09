using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x0200010E RID: 270
	[DynamicInterfaceCastableImplementation]
	[Guid("C03F6A43-65A4-9818-987E-E0B810D2A6F2")]
	internal interface IAgileReference : WinRT.Interop.IAgileReference
	{
		// Token: 0x06000981 RID: 2433 RVA: 0x0001AD58 File Offset: 0x00018F58
		unsafe static IAgileReference()
		{
			*(IUnknownVftbl*)((void*)IAgileReference.AbiToProjectionVftablePtr) = IUnknownVftbl.AbiToProjectionVftbl;
			*(IntPtr*)((byte*)((void*)IAgileReference.AbiToProjectionVftablePtr) + (IntPtr)3 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_Resolve);
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0001ADB8 File Offset: 0x00018FB8
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_Resolve(IntPtr thisPtr, Guid* riid, IntPtr* objectReference)
		{
			*objectReference = default(IntPtr);
			try
			{
				IObjectReference objectReference2 = ComWrappersSupport.FindObject<WinRT.Interop.IAgileReference>(thisPtr).Resolve(*riid);
				*objectReference = ((objectReference2 != null) ? objectReference2.GetRef() : IntPtr.Zero);
			}
			catch (Exception ex)
			{
				return ex.HResult;
			}
			return 0;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0001AE10 File Offset: 0x00019010
		IObjectReference Resolve(Guid riid)
		{
			return IAgileReferenceMethods.Resolve(((IWinRTObject)this).GetObjectReferenceForType(typeof(WinRT.Interop.IAgileReference).TypeHandle), riid);
		}

		// Token: 0x040001AB RID: 427
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IAgileReference), sizeof(IUnknownVftbl) + sizeof(IntPtr));
	}
}
