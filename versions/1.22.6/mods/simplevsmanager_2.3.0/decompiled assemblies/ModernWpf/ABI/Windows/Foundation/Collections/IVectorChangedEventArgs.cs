using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x02000101 RID: 257
	[DynamicInterfaceCastableImplementation]
	[Guid("575933DF-34FE-4480-AF15-07691F3D5D9B")]
	internal interface IVectorChangedEventArgs : Windows.Foundation.Collections.IVectorChangedEventArgs
	{
		// Token: 0x0600090A RID: 2314 RVA: 0x00019E6C File Offset: 0x0001806C
		unsafe static IVectorChangedEventArgs()
		{
			*(IInspectable.Vftbl*)((void*)IVectorChangedEventArgs.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IVectorChangedEventArgs.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_CollectionChange_0);
			*(IntPtr*)((byte*)((void*)IVectorChangedEventArgs.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Index_1);
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00019EE8 File Offset: 0x000180E8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_CollectionChange_0(IntPtr thisPtr, CollectionChange* value)
		{
			*value = CollectionChange.Reset;
			try
			{
				CollectionChange collectionChange = ComWrappersSupport.FindObject<Windows.Foundation.Collections.IVectorChangedEventArgs>(thisPtr).CollectionChange;
				*value = collectionChange;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00019F2C File Offset: 0x0001812C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Index_1(IntPtr thisPtr, uint* value)
		{
			*value = 0U;
			try
			{
				uint index = ComWrappersSupport.FindObject<Windows.Foundation.Collections.IVectorChangedEventArgs>(thisPtr).Index;
				*value = index;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x0600090D RID: 2317 RVA: 0x00019F70 File Offset: 0x00018170
		CollectionChange Windows.Foundation.Collections.IVectorChangedEventArgs.CollectionChange
		{
			get
			{
				return IVectorChangedEventArgsMethods.get_CollectionChange(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.Collections.IVectorChangedEventArgs).TypeHandle));
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x00019F91 File Offset: 0x00018191
		uint Windows.Foundation.Collections.IVectorChangedEventArgs.Index
		{
			get
			{
				return IVectorChangedEventArgsMethods.get_Index(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.Collections.IVectorChangedEventArgs).TypeHandle));
			}
		}

		// Token: 0x0400019B RID: 411
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IVectorChangedEventArgs), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 2);
	}
}
