using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000D3 RID: 211
	[DynamicInterfaceCastableImplementation]
	[Guid("FBC4DD2A-245B-11E4-AF98-689423260CF8")]
	internal interface IMemoryBuffer : Windows.Foundation.IMemoryBuffer, IDisposable
	{
		// Token: 0x060007C0 RID: 1984 RVA: 0x00013C58 File Offset: 0x00011E58
		unsafe static IMemoryBuffer()
		{
			*(IInspectable.Vftbl*)((void*)IMemoryBuffer.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IMemoryBuffer.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_CreateReference_0);
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00013CB8 File Offset: 0x00011EB8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_CreateReference_0(IntPtr thisPtr, IntPtr* reference)
		{
			*reference = default(IntPtr);
			try
			{
				Windows.Foundation.IMemoryBufferReference memoryBufferReference = ComWrappersSupport.FindObject<Windows.Foundation.IMemoryBuffer>(thisPtr).CreateReference();
				*reference = MarshalInterface<Windows.Foundation.IMemoryBufferReference>.FromManaged(memoryBufferReference);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00013D08 File Offset: 0x00011F08
		Windows.Foundation.IMemoryBufferReference CreateReference()
		{
			return IMemoryBufferMethods.CreateReference(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IMemoryBuffer).TypeHandle));
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x00013D29 File Offset: 0x00011F29
		void Dispose()
		{
			((IDisposable)((IWinRTObject)this)).Dispose();
		}

		// Token: 0x04000180 RID: 384
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IMemoryBuffer), sizeof(IInspectable.Vftbl) + sizeof(IntPtr));
	}
}
