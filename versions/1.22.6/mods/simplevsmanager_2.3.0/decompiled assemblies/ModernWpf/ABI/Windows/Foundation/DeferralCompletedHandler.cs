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
	// Token: 0x020000BC RID: 188
	[EditorBrowsable(1)]
	[Guid("ED32A372-F3C8-4FAA-9CFB-470148DA3888")]
	internal static class DeferralCompletedHandler
	{
		// Token: 0x06000758 RID: 1880 RVA: 0x00012590 File Offset: 0x00010790
		static DeferralCompletedHandler()
		{
			IntPtr intPtr = ComWrappersSupport.AllocateVtableMemory(typeof(DeferralCompletedHandler), Marshal.SizeOf<IDelegateVftbl>());
			Marshal.StructureToPtr<IDelegateVftbl>(DeferralCompletedHandler.AbiToProjectionVftable, intPtr, false);
			DeferralCompletedHandler.AbiToProjectionVftablePtr = intPtr;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x000125F0 File Offset: 0x000107F0
		public static IObjectReference CreateMarshaler(Windows.Foundation.DeferralCompletedHandler managedDelegate)
		{
			if (managedDelegate != null)
			{
				return MarshalDelegate.CreateMarshaler(managedDelegate, GuidGenerator.GetIID(typeof(DeferralCompletedHandler)), true);
			}
			return null;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x0001260D File Offset: 0x0001080D
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.DeferralCompletedHandler managedDelegate)
		{
			return MarshalDelegate.CreateMarshaler2(managedDelegate, GuidGenerator.GetIID(typeof(DeferralCompletedHandler)), true);
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00012625 File Offset: 0x00010825
		public static IntPtr GetAbi(IObjectReference value)
		{
			return MarshalInterfaceHelper<Windows.Foundation.DeferralCompletedHandler>.GetAbi(value);
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0001262D File Offset: 0x0001082D
		public static Windows.Foundation.DeferralCompletedHandler FromAbi(IntPtr nativeDelegate)
		{
			return MarshalDelegate.FromAbi<Windows.Foundation.DeferralCompletedHandler>(nativeDelegate);
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00012635 File Offset: 0x00010835
		public static Windows.Foundation.DeferralCompletedHandler CreateRcw(IntPtr ptr)
		{
			return new Windows.Foundation.DeferralCompletedHandler(new DeferralCompletedHandler.NativeDelegateWrapper(ComWrappersSupport.GetObjectReferenceForInterface<IDelegateVftbl>(ptr, GuidGenerator.GetIID(typeof(DeferralCompletedHandler)))).Invoke);
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0001265C File Offset: 0x0001085C
		public static IntPtr FromManaged(Windows.Foundation.DeferralCompletedHandler managedDelegate)
		{
			return DeferralCompletedHandler.CreateMarshaler2(managedDelegate).Detach();
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00012677 File Offset: 0x00010877
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<Windows.Foundation.DeferralCompletedHandler>.DisposeMarshaler(value);
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0001267F File Offset: 0x0001087F
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<Windows.Foundation.DeferralCompletedHandler>.DisposeAbi(abi);
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00012688 File Offset: 0x00010888
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private static int Do_Abi_Invoke(IntPtr thisPtr)
		{
			try
			{
				ComWrappersSupport.FindObject<Windows.Foundation.DeferralCompletedHandler>(thisPtr)();
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x04000175 RID: 373
		private static readonly IDelegateVftbl AbiToProjectionVftable = new IDelegateVftbl
		{
			IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
			Invoke = (IntPtr)ldftn(Do_Abi_Invoke)
		};

		// Token: 0x04000176 RID: 374
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x020002FE RID: 766
		[ObjectReferenceWrapper("_nativeDelegate")]
		private sealed class NativeDelegateWrapper : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x060016B4 RID: 5812 RVA: 0x000403F0 File Offset: 0x0003E5F0
			public NativeDelegateWrapper(ObjectReference<IDelegateVftbl> nativeDelegate)
			{
				this._nativeDelegate = nativeDelegate;
			}

			// Token: 0x17000450 RID: 1104
			// (get) Token: 0x060016B5 RID: 5813 RVA: 0x000403FF File Offset: 0x0003E5FF
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._nativeDelegate;
				}
			}

			// Token: 0x17000451 RID: 1105
			// (get) Token: 0x060016B6 RID: 5814 RVA: 0x00040407 File Offset: 0x0003E607
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060016B7 RID: 5815 RVA: 0x0004040A File Offset: 0x0003E60A
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x17000452 RID: 1106
			// (get) Token: 0x060016B8 RID: 5816 RVA: 0x00040426 File Offset: 0x0003E626
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x060016B9 RID: 5817 RVA: 0x0004043A File Offset: 0x0003E63A
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x17000453 RID: 1107
			// (get) Token: 0x060016BA RID: 5818 RVA: 0x00040456 File Offset: 0x0003E656
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x060016BB RID: 5819 RVA: 0x0004046C File Offset: 0x0003E66C
			public unsafe void Invoke()
			{
				IntPtr thisPtr = this._nativeDelegate.ThisPtr;
				method system.Int32_u0020(System.IntPtr) = (void*)this._nativeDelegate.Vftbl.Invoke;
				method system.Int32_u0020(System.IntPtr)2 = system.Int32_u0020(System.IntPtr);
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr), thisPtr, system.Int32_u0020(System.IntPtr)2));
			}

			// Token: 0x040007A4 RID: 1956
			private readonly ObjectReference<IDelegateVftbl> _nativeDelegate;

			// Token: 0x040007A5 RID: 1957
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x040007A6 RID: 1958
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}
	}
}
