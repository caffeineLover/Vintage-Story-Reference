using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using WinRT;
using WinRT.Interop;

namespace ABI.System.Collections.Specialized
{
	// Token: 0x02000153 RID: 339
	[EditorBrowsable(1)]
	[Guid("8B0909DC-2005-5D93-BF8A-725F017BAA8D")]
	internal static class NotifyCollectionChangedEventHandler
	{
		// Token: 0x06000ACE RID: 2766 RVA: 0x0001E268 File Offset: 0x0001C468
		static NotifyCollectionChangedEventHandler()
		{
			NotifyCollectionChangedEventHandler.AbiToProjectionVftable = new IDelegateVftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				Invoke = (IntPtr)ldftn(Do_Abi_Invoke)
			};
			IntPtr intPtr = ComWrappersSupport.AllocateVtableMemory(typeof(NotifyCollectionChangedEventHandler), Marshal.SizeOf<IDelegateVftbl>());
			Marshal.StructureToPtr<IDelegateVftbl>(NotifyCollectionChangedEventHandler.AbiToProjectionVftable, intPtr, false);
			NotifyCollectionChangedEventHandler.AbiToProjectionVftablePtr = intPtr;
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000ACF RID: 2767 RVA: 0x0001E2FC File Offset: 0x0001C4FC
		public static Delegate AbiInvokeDelegate { get; }

		// Token: 0x06000AD0 RID: 2768 RVA: 0x0001E303 File Offset: 0x0001C503
		public static IObjectReference CreateMarshaler(NotifyCollectionChangedEventHandler managedDelegate)
		{
			if (managedDelegate != null)
			{
				return MarshalDelegate.CreateMarshaler(managedDelegate, NotifyCollectionChangedEventHandler.IID, true);
			}
			return null;
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x0001E316 File Offset: 0x0001C516
		public static ObjectReferenceValue CreateMarshaler2(NotifyCollectionChangedEventHandler managedDelegate)
		{
			return MarshalDelegate.CreateMarshaler2(managedDelegate, NotifyCollectionChangedEventHandler.IID, true);
		}

		// Token: 0x06000AD2 RID: 2770 RVA: 0x0001E324 File Offset: 0x0001C524
		public static IntPtr GetAbi(IObjectReference value)
		{
			return MarshalInterfaceHelper<NotifyCollectionChangedEventHandler>.GetAbi(value);
		}

		// Token: 0x06000AD3 RID: 2771 RVA: 0x0001E32C File Offset: 0x0001C52C
		public static NotifyCollectionChangedEventHandler FromAbi(IntPtr nativeDelegate)
		{
			return MarshalDelegate.FromAbi<NotifyCollectionChangedEventHandler>(nativeDelegate);
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x0001E334 File Offset: 0x0001C534
		public static NotifyCollectionChangedEventHandler CreateRcw(IntPtr ptr)
		{
			return new NotifyCollectionChangedEventHandler(new NotifyCollectionChangedEventHandler.NativeDelegateWrapper(ComWrappersSupport.GetObjectReferenceForInterface<IDelegateVftbl>(ptr, NotifyCollectionChangedEventHandler.IID)).Invoke);
		}

		// Token: 0x06000AD5 RID: 2773 RVA: 0x0001E354 File Offset: 0x0001C554
		public static IntPtr FromManaged(NotifyCollectionChangedEventHandler managedDelegate)
		{
			return NotifyCollectionChangedEventHandler.CreateMarshaler2(managedDelegate).Detach();
		}

		// Token: 0x06000AD6 RID: 2774 RVA: 0x0001E36F File Offset: 0x0001C56F
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<NotifyCollectionChangedEventHandler>.DisposeMarshaler(value);
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x0001E377 File Offset: 0x0001C577
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<NotifyCollectionChangedEventHandler>.DisposeAbi(abi);
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x0001E380 File Offset: 0x0001C580
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private static int Do_Abi_Invoke(IntPtr thisPtr, IntPtr sender, IntPtr e)
		{
			try
			{
				ComWrappersSupport.FindObject<NotifyCollectionChangedEventHandler>(thisPtr).Invoke(MarshalInspectable<object>.FromAbi(sender), NotifyCollectionChangedEventArgs.FromAbi(e));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x040001E9 RID: 489
		private static readonly IDelegateVftbl AbiToProjectionVftable;

		// Token: 0x040001EA RID: 490
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x040001EC RID: 492
		private static readonly Guid IID = new Guid(2332625372U, 8197, 23955, 191, 138, 114, 95, 1, 123, 170, 141);

		// Token: 0x0200037B RID: 891
		[ObjectReferenceWrapper("_nativeDelegate")]
		private sealed class NativeDelegateWrapper : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x06001909 RID: 6409 RVA: 0x00045076 File Offset: 0x00043276
			public NativeDelegateWrapper(ObjectReference<IDelegateVftbl> nativeDelegate)
			{
				this._nativeDelegate = nativeDelegate;
			}

			// Token: 0x170004BD RID: 1213
			// (get) Token: 0x0600190A RID: 6410 RVA: 0x00045085 File Offset: 0x00043285
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._nativeDelegate;
				}
			}

			// Token: 0x170004BE RID: 1214
			// (get) Token: 0x0600190B RID: 6411 RVA: 0x0004508D File Offset: 0x0004328D
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return true;
				}
			}

			// Token: 0x0600190C RID: 6412 RVA: 0x00045090 File Offset: 0x00043290
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x170004BF RID: 1215
			// (get) Token: 0x0600190D RID: 6413 RVA: 0x000450AC File Offset: 0x000432AC
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x0600190E RID: 6414 RVA: 0x000450C0 File Offset: 0x000432C0
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x170004C0 RID: 1216
			// (get) Token: 0x0600190F RID: 6415 RVA: 0x000450DC File Offset: 0x000432DC
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x06001910 RID: 6416 RVA: 0x000450F0 File Offset: 0x000432F0
			public unsafe void Invoke(object sender, NotifyCollectionChangedEventArgs e)
			{
				IntPtr thisPtr = this._nativeDelegate.ThisPtr;
				method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr) = (void*)this._nativeDelegate.Vftbl.Invoke;
				ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
				ObjectReferenceValue objectReferenceValue2 = default(ObjectReferenceValue);
				try
				{
					objectReferenceValue = MarshalInspectable<object>.CreateMarshaler2(sender, true);
					objectReferenceValue2 = NotifyCollectionChangedEventArgs.CreateMarshaler2(e);
					method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr)2 = system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr);
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr), thisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), MarshalInspectable<object>.GetAbi(objectReferenceValue2), system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr)2));
				}
				finally
				{
					MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
					MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue2);
				}
			}

			// Token: 0x040008CB RID: 2251
			private readonly ObjectReference<IDelegateVftbl> _nativeDelegate;

			// Token: 0x040008CC RID: 2252
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x040008CD RID: 2253
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}
	}
}
