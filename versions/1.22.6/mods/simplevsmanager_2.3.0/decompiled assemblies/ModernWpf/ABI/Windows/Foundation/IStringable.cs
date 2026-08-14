using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000DB RID: 219
	[DynamicInterfaceCastableImplementation]
	[Guid("96369F54-8EB6-48F0-ABCE-C1B211E627C3")]
	internal interface IStringable : Windows.Foundation.IStringable
	{
		// Token: 0x06000825 RID: 2085 RVA: 0x000167D8 File Offset: 0x000149D8
		unsafe static IStringable()
		{
			*(IInspectable.Vftbl*)((void*)IStringable.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IStringable.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_ToString_0);
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00016838 File Offset: 0x00014A38
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_ToString_0(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string value2 = ComWrappersSupport.FindObject<Windows.Foundation.IStringable>(thisPtr).ToString();
				*value = MarshalString.FromManaged(value2);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00016888 File Offset: 0x00014A88
		string ToString()
		{
			return IStringableMethods.ToString(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IStringable).TypeHandle));
		}

		// Token: 0x04000186 RID: 390
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IStringable), sizeof(IInspectable.Vftbl) + sizeof(IntPtr));
	}
}
