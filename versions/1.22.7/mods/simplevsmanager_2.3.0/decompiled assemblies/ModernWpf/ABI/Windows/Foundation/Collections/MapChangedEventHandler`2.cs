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
	// Token: 0x02000102 RID: 258
	[EditorBrowsable(1)]
	[Guid("179517F3-94EE-41F8-BDDC-768A895544F3")]
	internal static class MapChangedEventHandler<K, V>
	{
		// Token: 0x0600090F RID: 2319 RVA: 0x00019FB4 File Offset: 0x000181B4
		unsafe static MapChangedEventHandler()
		{
			MapChangedEventHandler<K, V>.PIID = GuidGenerator.CreateIID(typeof(Windows.Foundation.Collections.MapChangedEventHandler<K, V>));
			MapChangedEventHandler<K, V>.Abi_Invoke_Type = Expression.GetDelegateType(new Type[]
			{
				typeof(void*),
				typeof(IntPtr),
				typeof(IntPtr),
				typeof(int)
			});
			MapChangedEventHandler<K, V>.<AbiInvokeDelegate>k__BackingField = Delegate.CreateDelegate(MapChangedEventHandler<K, V>.Abi_Invoke_Type, typeof(MapChangedEventHandler<K, V>).GetMethod("Do_Abi_Invoke", 40));
			MapChangedEventHandler<K, V>.AbiToProjectionVftable = new IDelegateVftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				Invoke = Marshal.GetFunctionPointerForDelegate(MapChangedEventHandler<K, V>.AbiInvokeDelegate)
			};
			IntPtr intPtr = ComWrappersSupport.AllocateVtableMemory(typeof(MapChangedEventHandler<K, V>), Marshal.SizeOf<IDelegateVftbl>());
			Marshal.StructureToPtr<IDelegateVftbl>(MapChangedEventHandler<K, V>.AbiToProjectionVftable, intPtr, false);
			MapChangedEventHandler<K, V>.AbiToProjectionVftablePtr = intPtr;
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x0001A090 File Offset: 0x00018290
		public static Delegate AbiInvokeDelegate
		{
			[CompilerGenerated]
			get
			{
				return MapChangedEventHandler<K, V>.<AbiInvokeDelegate>k__BackingField;
			}
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0001A097 File Offset: 0x00018297
		public static IObjectReference CreateMarshaler(Windows.Foundation.Collections.MapChangedEventHandler<K, V> managedDelegate)
		{
			if (managedDelegate != null)
			{
				return MarshalDelegate.CreateMarshaler(managedDelegate, GuidGenerator.GetIID(typeof(MapChangedEventHandler<K, V>)), true);
			}
			return null;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0001A0B4 File Offset: 0x000182B4
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.Collections.MapChangedEventHandler<K, V> managedDelegate)
		{
			return MarshalDelegate.CreateMarshaler2(managedDelegate, GuidGenerator.GetIID(typeof(MapChangedEventHandler<K, V>)), true);
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x0001A0CC File Offset: 0x000182CC
		public static IntPtr GetAbi(IObjectReference value)
		{
			return MarshalInterfaceHelper<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>.GetAbi(value);
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x0001A0D4 File Offset: 0x000182D4
		public static Windows.Foundation.Collections.MapChangedEventHandler<K, V> FromAbi(IntPtr nativeDelegate)
		{
			return MarshalDelegate.FromAbi<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>(nativeDelegate);
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x0001A0DC File Offset: 0x000182DC
		public static Windows.Foundation.Collections.MapChangedEventHandler<K, V> CreateRcw(IntPtr ptr)
		{
			return new Windows.Foundation.Collections.MapChangedEventHandler<K, V>(new MapChangedEventHandler<K, V>.NativeDelegateWrapper(ComWrappersSupport.GetObjectReferenceForInterface<IDelegateVftbl>(ptr, GuidGenerator.GetIID(typeof(MapChangedEventHandler<K, V>)))).Invoke);
		}

		// Token: 0x06000916 RID: 2326 RVA: 0x0001A104 File Offset: 0x00018304
		public static IntPtr FromManaged(Windows.Foundation.Collections.MapChangedEventHandler<K, V> managedDelegate)
		{
			return MapChangedEventHandler<K, V>.CreateMarshaler2(managedDelegate).Detach();
		}

		// Token: 0x06000917 RID: 2327 RVA: 0x0001A11F File Offset: 0x0001831F
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>.DisposeMarshaler(value);
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x0001A127 File Offset: 0x00018327
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>.DisposeAbi(abi);
		}

		// Token: 0x06000919 RID: 2329 RVA: 0x0001A130 File Offset: 0x00018330
		private unsafe static int Do_Abi_Invoke(void* thisPtr, IntPtr sender, IntPtr @event)
		{
			try
			{
				ComWrappersSupport.FindObject<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>(new IntPtr(thisPtr)).Invoke(MarshalInterface<Windows.Foundation.Collections.IObservableMap<K, V>>.FromAbi(sender), MarshalInterface<Windows.Foundation.Collections.IMapChangedEventArgs<K>>.FromAbi(@event));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0400019C RID: 412
		public static Guid PIID;

		// Token: 0x0400019D RID: 413
		private static readonly Type Abi_Invoke_Type;

		// Token: 0x0400019E RID: 414
		private static readonly IDelegateVftbl AbiToProjectionVftable;

		// Token: 0x0400019F RID: 415
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x0200031C RID: 796
		[ObjectReferenceWrapper("_nativeDelegate")]
		private sealed class NativeDelegateWrapper : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x06001759 RID: 5977 RVA: 0x00041AD8 File Offset: 0x0003FCD8
			public NativeDelegateWrapper(ObjectReference<IDelegateVftbl> nativeDelegate)
			{
				this._nativeDelegate = nativeDelegate;
			}

			// Token: 0x1700046A RID: 1130
			// (get) Token: 0x0600175A RID: 5978 RVA: 0x00041AE7 File Offset: 0x0003FCE7
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._nativeDelegate;
				}
			}

			// Token: 0x1700046B RID: 1131
			// (get) Token: 0x0600175B RID: 5979 RVA: 0x00041AEF File Offset: 0x0003FCEF
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return true;
				}
			}

			// Token: 0x0600175C RID: 5980 RVA: 0x00041AF2 File Offset: 0x0003FCF2
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x1700046C RID: 1132
			// (get) Token: 0x0600175D RID: 5981 RVA: 0x00041B0E File Offset: 0x0003FD0E
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x0600175E RID: 5982 RVA: 0x00041B22 File Offset: 0x0003FD22
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x1700046D RID: 1133
			// (get) Token: 0x0600175F RID: 5983 RVA: 0x00041B3E File Offset: 0x0003FD3E
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x06001760 RID: 5984 RVA: 0x00041B54 File Offset: 0x0003FD54
			public void Invoke(Windows.Foundation.Collections.IObservableMap<K, V> sender, Windows.Foundation.Collections.IMapChangedEventArgs<K> @event)
			{
				IntPtr thisPtr = this._nativeDelegate.ThisPtr;
				Delegate delegateForFunctionPointer = Marshal.GetDelegateForFunctionPointer(this._nativeDelegate.Vftbl.Invoke, MapChangedEventHandler<K, V>.Abi_Invoke_Type);
				ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
				ObjectReferenceValue objectReferenceValue2 = default(ObjectReferenceValue);
				object[] array = new object[3];
				array[0] = thisPtr;
				object[] array2 = array;
				try
				{
					objectReferenceValue = MarshalInterface<Windows.Foundation.Collections.IObservableMap<K, V>>.CreateMarshaler2(sender, default(Guid));
					array2[1] = MarshalInspectable<object>.GetAbi(objectReferenceValue);
					objectReferenceValue2 = MarshalInterface<Windows.Foundation.Collections.IMapChangedEventArgs<K>>.CreateMarshaler2(@event, default(Guid));
					array2[2] = MarshalInspectable<object>.GetAbi(objectReferenceValue2);
					delegateForFunctionPointer.DynamicInvokeAbi(array2);
				}
				finally
				{
					MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
					MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue2);
				}
			}

			// Token: 0x040007EC RID: 2028
			private readonly ObjectReference<IDelegateVftbl> _nativeDelegate;

			// Token: 0x040007ED RID: 2029
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x040007EE RID: 2030
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}
	}
}
