using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000D5 RID: 213
	[Guid("FBC4DD2B-245B-11E4-AF98-689423260CF8")]
	internal interface IMemoryBufferFactory : Windows.Foundation.IMemoryBufferFactory
	{
		// Token: 0x060007C5 RID: 1989 RVA: 0x00013D9C File Offset: 0x00011F9C
		unsafe static IMemoryBufferFactory()
		{
			*(IInspectable.Vftbl*)((void*)IMemoryBufferFactory.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IMemoryBufferFactory.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_Create_0);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00013DFC File Offset: 0x00011FFC
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_Create_0(IntPtr thisPtr, uint capacity, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				Windows.Foundation.MemoryBuffer obj = ComWrappersSupport.FindObject<Windows.Foundation.IMemoryBufferFactory>(thisPtr).Create(capacity);
				*value = MemoryBuffer.FromManaged(obj);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x04000181 RID: 385
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IMemoryBufferFactory), sizeof(IInspectable.Vftbl) + sizeof(IntPtr));
	}
}
