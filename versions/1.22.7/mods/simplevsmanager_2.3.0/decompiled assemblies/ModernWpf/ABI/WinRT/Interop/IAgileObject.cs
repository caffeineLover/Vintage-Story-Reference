using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x0200010F RID: 271
	[DynamicInterfaceCastableImplementation]
	[Guid("94ea2b94-e9cc-49e0-c0ff-ee64ca8f5b90")]
	internal interface IAgileObject : WinRT.Interop.IAgileObject
	{
		// Token: 0x06000984 RID: 2436 RVA: 0x0001AE32 File Offset: 0x00019032
		unsafe static IAgileObject()
		{
			*(IUnknownVftbl*)((void*)IAgileObject.AbiToProjectionVftablePtr) = IUnknownVftbl.AbiToProjectionVftbl;
		}

		// Token: 0x040001AC RID: 428
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IAgileObject), sizeof(IUnknownVftbl));
	}
}
