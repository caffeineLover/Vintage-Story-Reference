using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Windows.Foundation;
using WinRT;
using WinRT.Interop;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000B5 RID: 181
	[EditorBrowsable(1)]
	[Guid("A4ED5C81-76C9-40BD-8BE6-B1D90FB20AE7")]
	internal static class AsyncActionCompletedHandler
	{
		// Token: 0x0600070A RID: 1802 RVA: 0x00011A34 File Offset: 0x0000FC34
		static AsyncActionCompletedHandler()
		{
			IntPtr intPtr = ComWrappersSupport.AllocateVtableMemory(typeof(AsyncActionCompletedHandler), Marshal.SizeOf<IDelegateVftbl>());
			Marshal.StructureToPtr<IDelegateVftbl>(AsyncActionCompletedHandler.AbiToProjectionVftable, intPtr, false);
			AsyncActionCompletedHandler.AbiToProjectionVftablePtr = intPtr;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00011A94 File Offset: 0x0000FC94
		public static IObjectReference CreateMarshaler(Windows.Foundation.AsyncActionCompletedHandler managedDelegate)
		{
			if (managedDelegate != null)
			{
				return MarshalDelegate.CreateMarshaler(managedDelegate, GuidGenerator.GetIID(typeof(AsyncActionCompletedHandler)), true);
			}
			return null;
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00011AB1 File Offset: 0x0000FCB1
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.AsyncActionCompletedHandler managedDelegate)
		{
			return MarshalDelegate.CreateMarshaler2(managedDelegate, GuidGenerator.GetIID(typeof(AsyncActionCompletedHandler)), true);
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x00011AC9 File Offset: 0x0000FCC9
		public static IntPtr GetAbi(IObjectReference value)
		{
			return MarshalInterfaceHelper<Windows.Foundation.AsyncActionCompletedHandler>.GetAbi(value);
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x00011AD1 File Offset: 0x0000FCD1
		public static Windows.Foundation.AsyncActionCompletedHandler FromAbi(IntPtr nativeDelegate)
		{
			return MarshalDelegate.FromAbi<Windows.Foundation.AsyncActionCompletedHandler>(nativeDelegate);
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x00011AD9 File Offset: 0x0000FCD9
		public static Windows.Foundation.AsyncActionCompletedHandler CreateRcw(IntPtr ptr)
		{
			return new Windows.Foundation.AsyncActionCompletedHandler(new AsyncActionCompletedHandler.NativeDelegateWrapper(ComWrappersSupport.GetObjectReferenceForInterface<IDelegateVftbl>(ptr, GuidGenerator.GetIID(typeof(AsyncActionCompletedHandler)))).Invoke);
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x00011B00 File Offset: 0x0000FD00
		public static IntPtr FromManaged(Windows.Foundation.AsyncActionCompletedHandler managedDelegate)
		{
			return AsyncActionCompletedHandler.CreateMarshaler2(managedDelegate).Detach();
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00011B1B File Offset: 0x0000FD1B
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<Windows.Foundation.AsyncActionCompletedHandler>.DisposeMarshaler(value);
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00011B23 File Offset: 0x0000FD23
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<Windows.Foundation.AsyncActionCompletedHandler>.DisposeAbi(abi);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00011B2C File Offset: 0x0000FD2C
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private static int Do_Abi_Invoke(IntPtr thisPtr, IntPtr asyncInfo, AsyncStatus asyncStatus)
		{
			try
			{
				ComWrappersSupport.FindObject<Windows.Foundation.AsyncActionCompletedHandler>(thisPtr)(MarshalInterface<Windows.Foundation.IAsyncAction>.FromAbi(asyncInfo), asyncStatus);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0400015A RID: 346
		private static readonly IDelegateVftbl AbiToProjectionVftable = new IDelegateVftbl
		{
			IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
			Invoke = (IntPtr)ldftn(Do_Abi_Invoke)
		};

		// Token: 0x0400015B RID: 347
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x020002F7 RID: 759
		[ObjectReferenceWrapper("_nativeDelegate")]
		private sealed class NativeDelegateWrapper : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x06001680 RID: 5760 RVA: 0x0003FD41 File Offset: 0x0003DF41
			public NativeDelegateWrapper(ObjectReference<IDelegateVftbl> nativeDelegate)
			{
				this._nativeDelegate = nativeDelegate;
			}

			// Token: 0x17000438 RID: 1080
			// (get) Token: 0x06001681 RID: 5761 RVA: 0x0003FD50 File Offset: 0x0003DF50
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._nativeDelegate;
				}
			}

			// Token: 0x17000439 RID: 1081
			// (get) Token: 0x06001682 RID: 5762 RVA: 0x0003FD58 File Offset: 0x0003DF58
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06001683 RID: 5763 RVA: 0x0003FD5B File Offset: 0x0003DF5B
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x1700043A RID: 1082
			// (get) Token: 0x06001684 RID: 5764 RVA: 0x0003FD77 File Offset: 0x0003DF77
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x06001685 RID: 5765 RVA: 0x0003FD8B File Offset: 0x0003DF8B
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x1700043B RID: 1083
			// (get) Token: 0x06001686 RID: 5766 RVA: 0x0003FDA7 File Offset: 0x0003DFA7
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x06001687 RID: 5767 RVA: 0x0003FDBC File Offset: 0x0003DFBC
			public unsafe void Invoke(Windows.Foundation.IAsyncAction asyncInfo, AsyncStatus asyncStatus)
			{
				IntPtr thisPtr = this._nativeDelegate.ThisPtr;
				method system.Int32_u0020(System.IntPtr,System.IntPtr,Windows.Foundation.AsyncStatus) = (void*)this._nativeDelegate.Vftbl.Invoke;
				ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
				try
				{
					objectReferenceValue = MarshalInterface<Windows.Foundation.IAsyncAction>.CreateMarshaler2(asyncInfo, default(Guid));
					method system.Int32_u0020(System.IntPtr,System.IntPtr,Windows.Foundation.AsyncStatus)2 = system.Int32_u0020(System.IntPtr,System.IntPtr,Windows.Foundation.AsyncStatus);
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,Windows.Foundation.AsyncStatus), thisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), asyncStatus, system.Int32_u0020(System.IntPtr,System.IntPtr,Windows.Foundation.AsyncStatus)2));
				}
				finally
				{
					MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
				}
			}

			// Token: 0x0400078F RID: 1935
			private readonly ObjectReference<IDelegateVftbl> _nativeDelegate;

			// Token: 0x04000790 RID: 1936
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x04000791 RID: 1937
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}
	}
}
