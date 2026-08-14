using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x02000116 RID: 278
	[DynamicInterfaceCastableImplementation]
	[Guid("00000037-0000-0000-C000-000000000046")]
	internal interface IWeakReference : WinRT.Interop.IWeakReference
	{
		// Token: 0x060009AF RID: 2479 RVA: 0x0001B3B8 File Offset: 0x000195B8
		unsafe static IWeakReference()
		{
			*(IUnknownVftbl*)((void*)IWeakReference.AbiToProjectionVftablePtr) = IUnknownVftbl.AbiToProjectionVftbl;
			*(IntPtr*)((byte*)((void*)IWeakReference.AbiToProjectionVftablePtr) + (IntPtr)3 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_Resolve);
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0001B430 File Offset: 0x00019630
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_Resolve(IntPtr thisPtr, Guid* riid, IntPtr* objectReference)
		{
			*objectReference = default(IntPtr);
			try
			{
				WinRT.Interop.IWeakReference weakReference = ComWrappersSupport.FindObject<WinRT.Interop.IWeakReference>(thisPtr);
				ManagedWeakReference managedWeakReference = weakReference as ManagedWeakReference;
				if (managedWeakReference != null)
				{
					*objectReference = managedWeakReference.ResolveForABI(*riid);
				}
				else
				{
					using (IObjectReference objectReference2 = weakReference.Resolve(*riid))
					{
						*objectReference = ((objectReference2 != null) ? objectReference2.GetRef() : IntPtr.Zero);
					}
				}
			}
			catch (Exception ex)
			{
				return ex.HResult;
			}
			return 0;
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0001B4BC File Offset: 0x000196BC
		unsafe IObjectReference Resolve(Guid riid)
		{
			IntPtr thisPtr = ((IWinRTObject)this).GetObjectReferenceForType(typeof(WinRT.Interop.IWeakReference).TypeHandle).ThisPtr;
			method system.Int32_u0020(System.IntPtr,System.Guid*,System.IntPtr*) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)3 * (IntPtr)sizeof(method));
			IntPtr intPtr;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Guid*,System.IntPtr*), thisPtr, &riid, &intPtr, system.Int32_u0020(System.IntPtr,System.Guid*,System.IntPtr*)));
			IObjectReference objectReferenceForInterface;
			try
			{
				objectReferenceForInterface = ComWrappersSupport.GetObjectReferenceForInterface(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return objectReferenceForInterface;
		}

		// Token: 0x040001B7 RID: 439
		internal static readonly Guid IID = new Guid(55, 0, 0, 192, 0, 0, 0, 0, 0, 0, 70);

		// Token: 0x040001B8 RID: 440
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IWeakReference), sizeof(IUnknownVftbl) + sizeof(IntPtr));
	}
}
