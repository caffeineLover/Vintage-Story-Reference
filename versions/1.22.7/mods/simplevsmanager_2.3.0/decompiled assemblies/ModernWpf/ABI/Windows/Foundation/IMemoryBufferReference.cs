using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000D7 RID: 215
	[DynamicInterfaceCastableImplementation]
	[Guid("FBC4DD29-245B-11E4-AF98-689423260CF8")]
	internal interface IMemoryBufferReference : Windows.Foundation.IMemoryBufferReference, IDisposable
	{
		// Token: 0x060007CB RID: 1995 RVA: 0x00013EE8 File Offset: 0x000120E8
		unsafe static IMemoryBufferReference()
		{
			*(IInspectable.Vftbl*)((void*)IMemoryBufferReference.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IMemoryBufferReference.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Capacity_0);
			*(IntPtr*)((byte*)((void*)IMemoryBufferReference.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_add_Closed_1);
			*(IntPtr*)((byte*)((void*)IMemoryBufferReference.AbiToProjectionVftablePtr) + (IntPtr)8 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_remove_Closed_2);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00013F80 File Offset: 0x00012180
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Capacity_0(IntPtr thisPtr, uint* value)
		{
			*value = 0U;
			try
			{
				uint capacity = ComWrappersSupport.FindObject<Windows.Foundation.IMemoryBufferReference>(thisPtr).Capacity;
				*value = capacity;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00013FC4 File Offset: 0x000121C4
		private static ConditionalWeakTable<Windows.Foundation.IMemoryBufferReference, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>>> MakeClosedTable()
		{
			Interlocked.CompareExchange<ConditionalWeakTable<Windows.Foundation.IMemoryBufferReference, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>>>>(ref IMemoryBufferReference._Closed_tokenTables, new ConditionalWeakTable<Windows.Foundation.IMemoryBufferReference, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>>>(), null);
			return IMemoryBufferReference._Closed_tokenTables;
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x00013FDE File Offset: 0x000121DE
		ConditionalWeakTable<Windows.Foundation.IMemoryBufferReference, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>>> _Closed_TokenTables
		{
			get
			{
				return IMemoryBufferReference._Closed_tokenTables ?? IMemoryBufferReference.MakeClosedTable();
			}
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00013FF0 File Offset: 0x000121F0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_add_Closed_1(IntPtr thisPtr, IntPtr handler, EventRegistrationToken* cookie)
		{
			*cookie = default(EventRegistrationToken);
			int result;
			try
			{
				Windows.Foundation.IMemoryBufferReference memoryBufferReference = ComWrappersSupport.FindObject<Windows.Foundation.IMemoryBufferReference>(thisPtr);
				Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object> typedEventHandler = TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>.FromAbi(handler);
				*cookie = IMemoryBufferReference._Closed_TokenTables.GetOrCreateValue(memoryBufferReference).AddEventHandler(typedEventHandler);
				memoryBufferReference.Closed += typedEventHandler;
				result = 0;
			}
			catch (Exception ex)
			{
				result = ex.HResult;
			}
			return result;
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00014050 File Offset: 0x00012250
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private static int Do_Abi_remove_Closed_2(IntPtr thisPtr, EventRegistrationToken cookie)
		{
			int result;
			try
			{
				Windows.Foundation.IMemoryBufferReference memoryBufferReference = ComWrappersSupport.FindObject<Windows.Foundation.IMemoryBufferReference>(thisPtr);
				EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>> eventRegistrationTokenTable;
				Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object> value;
				if (memoryBufferReference != null && IMemoryBufferReference._Closed_TokenTables.TryGetValue(memoryBufferReference, ref eventRegistrationTokenTable) && eventRegistrationTokenTable.RemoveEventHandler(cookie, ref value))
				{
					memoryBufferReference.Closed -= value;
				}
				result = 0;
			}
			catch (Exception ex)
			{
				result = ex.HResult;
			}
			return result;
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x000140A8 File Offset: 0x000122A8
		uint Windows.Foundation.IMemoryBufferReference.Capacity
		{
			get
			{
				return IMemoryBufferReferenceMethods.get_Capacity(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IMemoryBufferReference).TypeHandle));
			}
		}

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060007D2 RID: 2002 RVA: 0x000140C9 File Offset: 0x000122C9
		// (remove) Token: 0x060007D3 RID: 2003 RVA: 0x000140FB File Offset: 0x000122FB
		event Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object> Windows.Foundation.IMemoryBufferReference.Closed
		{
			add
			{
				IMemoryBufferReferenceMethods.Get_Closed(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IMemoryBufferReference).TypeHandle), (IWinRTObject)this).Item1.Invoke(value);
			}
			remove
			{
				IMemoryBufferReferenceMethods.Get_Closed(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IMemoryBufferReference).TypeHandle), (IWinRTObject)this).Item2.Invoke(value);
			}
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0001412D File Offset: 0x0001232D
		void Dispose()
		{
			((IDisposable)((IWinRTObject)this)).Dispose();
		}

		// Token: 0x04000183 RID: 387
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IMemoryBufferReference), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 3);

		// Token: 0x04000184 RID: 388
		private static volatile ConditionalWeakTable<Windows.Foundation.IMemoryBufferReference, EventRegistrationTokenTable<Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>>> _Closed_tokenTables;
	}
}
