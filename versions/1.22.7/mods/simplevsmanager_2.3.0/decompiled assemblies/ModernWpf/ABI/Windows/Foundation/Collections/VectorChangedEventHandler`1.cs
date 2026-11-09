using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Windows.Foundation.Collections;
using WinRT;
using WinRT.Interop;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x02000106 RID: 262
	[EditorBrowsable(1)]
	[Guid("0C051752-9FBF-4C70-AA0C-0E4C82D9A761")]
	internal static class VectorChangedEventHandler<T>
	{
		// Token: 0x06000941 RID: 2369 RVA: 0x0001A464 File Offset: 0x00018664
		unsafe static VectorChangedEventHandler()
		{
			VectorChangedEventHandler<T>.PIID = GuidGenerator.CreateIID(typeof(Windows.Foundation.Collections.VectorChangedEventHandler<T>));
			VectorChangedEventHandler<T>.Abi_Invoke_Type = Expression.GetDelegateType(new Type[]
			{
				typeof(void*),
				typeof(IntPtr),
				typeof(IntPtr),
				typeof(int)
			});
			VectorChangedEventHandler<T>.<AbiInvokeDelegate>k__BackingField = Delegate.CreateDelegate(VectorChangedEventHandler<T>.Abi_Invoke_Type, typeof(VectorChangedEventHandler<T>).GetMethod("Do_Abi_Invoke", 40));
			VectorChangedEventHandler<T>.AbiToProjectionVftable = new IDelegateVftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				Invoke = Marshal.GetFunctionPointerForDelegate(VectorChangedEventHandler<T>.AbiInvokeDelegate)
			};
			IntPtr intPtr = ComWrappersSupport.AllocateVtableMemory(typeof(VectorChangedEventHandler<T>), Marshal.SizeOf<IDelegateVftbl>());
			Marshal.StructureToPtr<IDelegateVftbl>(VectorChangedEventHandler<T>.AbiToProjectionVftable, intPtr, false);
			VectorChangedEventHandler<T>.AbiToProjectionVftablePtr = intPtr;
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000942 RID: 2370 RVA: 0x0001A540 File Offset: 0x00018740
		public static Delegate AbiInvokeDelegate
		{
			[CompilerGenerated]
			get
			{
				return VectorChangedEventHandler<T>.<AbiInvokeDelegate>k__BackingField;
			}
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0001A547 File Offset: 0x00018747
		public static IObjectReference CreateMarshaler(Windows.Foundation.Collections.VectorChangedEventHandler<T> managedDelegate)
		{
			if (managedDelegate != null)
			{
				return MarshalDelegate.CreateMarshaler(managedDelegate, GuidGenerator.GetIID(typeof(VectorChangedEventHandler<T>)), true);
			}
			return null;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x0001A564 File Offset: 0x00018764
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.Collections.VectorChangedEventHandler<T> managedDelegate)
		{
			return MarshalDelegate.CreateMarshaler2(managedDelegate, GuidGenerator.GetIID(typeof(VectorChangedEventHandler<T>)), true);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x0001A57C File Offset: 0x0001877C
		public static IntPtr GetAbi(IObjectReference value)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Collections.VectorChangedEventHandler<T>>.GetAbi(value);
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x0001A584 File Offset: 0x00018784
		public static Windows.Foundation.Collections.VectorChangedEventHandler<T> FromAbi(IntPtr nativeDelegate)
		{
			return MarshalDelegate.FromAbi<Windows.Foundation.Collections.VectorChangedEventHandler<T>>(nativeDelegate);
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0001A58C File Offset: 0x0001878C
		public static Windows.Foundation.Collections.VectorChangedEventHandler<T> CreateRcw(IntPtr ptr)
		{
			return new Windows.Foundation.Collections.VectorChangedEventHandler<T>(new VectorChangedEventHandler<T>.NativeDelegateWrapper(ComWrappersSupport.GetObjectReferenceForInterface<IDelegateVftbl>(ptr, GuidGenerator.GetIID(typeof(VectorChangedEventHandler<T>)))).Invoke);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x0001A5B4 File Offset: 0x000187B4
		public static IntPtr FromManaged(Windows.Foundation.Collections.VectorChangedEventHandler<T> managedDelegate)
		{
			return VectorChangedEventHandler<T>.CreateMarshaler2(managedDelegate).Detach();
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x0001A5CF File Offset: 0x000187CF
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<Windows.Foundation.Collections.VectorChangedEventHandler<T>>.DisposeMarshaler(value);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0001A5D7 File Offset: 0x000187D7
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<Windows.Foundation.Collections.VectorChangedEventHandler<T>>.DisposeAbi(abi);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0001A5E0 File Offset: 0x000187E0
		private unsafe static int Do_Abi_Invoke(void* thisPtr, IntPtr sender, IntPtr @event)
		{
			try
			{
				ComWrappersSupport.FindObject<Windows.Foundation.Collections.VectorChangedEventHandler<T>>(new IntPtr(thisPtr)).Invoke(MarshalInterface<Windows.Foundation.Collections.IObservableVector<T>>.FromAbi(sender), MarshalInterface<Windows.Foundation.Collections.IVectorChangedEventArgs>.FromAbi(@event));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x040001A1 RID: 417
		public static Guid PIID;

		// Token: 0x040001A2 RID: 418
		private static readonly Type Abi_Invoke_Type;

		// Token: 0x040001A3 RID: 419
		private static readonly IDelegateVftbl AbiToProjectionVftable;

		// Token: 0x040001A4 RID: 420
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x02000320 RID: 800
		[ObjectReferenceWrapper("_nativeDelegate")]
		private sealed class NativeDelegateWrapper : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x0600176D RID: 5997 RVA: 0x00041C7C File Offset: 0x0003FE7C
			public NativeDelegateWrapper(ObjectReference<IDelegateVftbl> nativeDelegate)
			{
				this._nativeDelegate = nativeDelegate;
			}

			// Token: 0x1700046E RID: 1134
			// (get) Token: 0x0600176E RID: 5998 RVA: 0x00041C8B File Offset: 0x0003FE8B
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._nativeDelegate;
				}
			}

			// Token: 0x1700046F RID: 1135
			// (get) Token: 0x0600176F RID: 5999 RVA: 0x00041C93 File Offset: 0x0003FE93
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06001770 RID: 6000 RVA: 0x00041C96 File Offset: 0x0003FE96
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x17000470 RID: 1136
			// (get) Token: 0x06001771 RID: 6001 RVA: 0x00041CB2 File Offset: 0x0003FEB2
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x06001772 RID: 6002 RVA: 0x00041CC6 File Offset: 0x0003FEC6
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x17000471 RID: 1137
			// (get) Token: 0x06001773 RID: 6003 RVA: 0x00041CE2 File Offset: 0x0003FEE2
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x06001774 RID: 6004 RVA: 0x00041CF8 File Offset: 0x0003FEF8
			public void Invoke(Windows.Foundation.Collections.IObservableVector<T> sender, Windows.Foundation.Collections.IVectorChangedEventArgs @event)
			{
				IntPtr thisPtr = this._nativeDelegate.ThisPtr;
				Delegate delegateForFunctionPointer = Marshal.GetDelegateForFunctionPointer(this._nativeDelegate.Vftbl.Invoke, VectorChangedEventHandler<T>.Abi_Invoke_Type);
				ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
				ObjectReferenceValue objectReferenceValue2 = default(ObjectReferenceValue);
				object[] array = new object[3];
				array[0] = thisPtr;
				object[] array2 = array;
				try
				{
					objectReferenceValue = MarshalInterface<Windows.Foundation.Collections.IObservableVector<T>>.CreateMarshaler2(sender, default(Guid));
					array2[1] = MarshalInspectable<object>.GetAbi(objectReferenceValue);
					objectReferenceValue2 = MarshalInterface<Windows.Foundation.Collections.IVectorChangedEventArgs>.CreateMarshaler2(@event, default(Guid));
					array2[2] = MarshalInspectable<object>.GetAbi(objectReferenceValue2);
					delegateForFunctionPointer.DynamicInvokeAbi(array2);
				}
				finally
				{
					MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
					MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue2);
				}
			}

			// Token: 0x040007F8 RID: 2040
			private readonly ObjectReference<IDelegateVftbl> _nativeDelegate;

			// Token: 0x040007F9 RID: 2041
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x040007FA RID: 2042
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}
	}
}
