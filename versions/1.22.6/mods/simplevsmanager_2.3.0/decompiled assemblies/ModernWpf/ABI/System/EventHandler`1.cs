using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using WinRT;
using WinRT.Interop;

namespace ABI.System
{
	// Token: 0x02000117 RID: 279
	[Guid("9DE1C535-6AE1-11E0-84E1-18A905BCC53F")]
	[EditorBrowsable(1)]
	internal static class EventHandler<T>
	{
		// Token: 0x060009B2 RID: 2482 RVA: 0x0001B530 File Offset: 0x00019730
		unsafe static EventHandler()
		{
			EventHandler<T>.PIID = GuidGenerator.CreateIID(typeof(EventHandler<T>));
			EventHandler<T>.Abi_Invoke_Type = Expression.GetDelegateType(new Type[]
			{
				typeof(void*),
				typeof(IntPtr),
				Marshaler<T>.AbiType,
				typeof(int)
			});
			EventHandler<T>.<AbiInvokeDelegate>k__BackingField = Delegate.CreateDelegate(EventHandler<T>.Abi_Invoke_Type, typeof(EventHandler<T>).GetMethod("Do_Abi_Invoke", 40).MakeGenericMethod(new Type[]
			{
				Marshaler<T>.AbiType
			}));
			EventHandler<T>.AbiToProjectionVftable = new IDelegateVftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				Invoke = Marshal.GetFunctionPointerForDelegate(EventHandler<T>.AbiInvokeDelegate)
			};
			IntPtr intPtr = ComWrappersSupport.AllocateVtableMemory(typeof(EventHandler<T>), Marshal.SizeOf<IDelegateVftbl>());
			Marshal.StructureToPtr<IDelegateVftbl>(EventHandler<T>.AbiToProjectionVftable, intPtr, false);
			EventHandler<T>.AbiToProjectionVftablePtr = intPtr;
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x0001B61A File Offset: 0x0001981A
		public static Delegate AbiInvokeDelegate
		{
			[CompilerGenerated]
			get
			{
				return EventHandler<T>.<AbiInvokeDelegate>k__BackingField;
			}
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0001B621 File Offset: 0x00019821
		public static IObjectReference CreateMarshaler(EventHandler<T> managedDelegate)
		{
			if (managedDelegate != null)
			{
				return MarshalDelegate.CreateMarshaler(managedDelegate, EventHandler<T>.PIID, true);
			}
			return null;
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0001B634 File Offset: 0x00019834
		public static ObjectReferenceValue CreateMarshaler2(EventHandler<T> managedDelegate)
		{
			return MarshalDelegate.CreateMarshaler2(managedDelegate, EventHandler<T>.PIID, true);
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0001B642 File Offset: 0x00019842
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<EventHandler<T>>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0001B653 File Offset: 0x00019853
		public static EventHandler<T> FromAbi(IntPtr nativeDelegate)
		{
			return MarshalDelegate.FromAbi<EventHandler<T>>(nativeDelegate);
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0001B65B File Offset: 0x0001985B
		public static EventHandler<T> CreateRcw(IntPtr ptr)
		{
			return new EventHandler<T>(new EventHandler<T>.NativeDelegateWrapper(ComWrappersSupport.GetObjectReferenceForInterface<IDelegateVftbl>(ptr, EventHandler<T>.PIID)).Invoke);
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0001B678 File Offset: 0x00019878
		public static IntPtr FromManaged(EventHandler<T> managedDelegate)
		{
			return EventHandler<T>.CreateMarshaler2(managedDelegate).Detach();
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0001B693 File Offset: 0x00019893
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<EventHandler<T>>.DisposeMarshaler(value);
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0001B69B File Offset: 0x0001989B
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<EventHandler<T>>.DisposeAbi(abi);
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0001B6A4 File Offset: 0x000198A4
		private unsafe static int Do_Abi_Invoke<TAbi>(void* thisPtr, IntPtr sender, TAbi args)
		{
			try
			{
				ComWrappersSupport.FindObject<EventHandler<T>>(new IntPtr(thisPtr)).Invoke(MarshalInspectable<object>.FromAbi(sender), Marshaler<T>.FromAbi.Invoke(args));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x040001B9 RID: 441
		public static Guid PIID;

		// Token: 0x040001BA RID: 442
		private static readonly Type Abi_Invoke_Type;

		// Token: 0x040001BB RID: 443
		private static readonly IDelegateVftbl AbiToProjectionVftable;

		// Token: 0x040001BC RID: 444
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x02000330 RID: 816
		[ObjectReferenceWrapper("_nativeDelegate")]
		private sealed class NativeDelegateWrapper : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x060017B4 RID: 6068 RVA: 0x000424D0 File Offset: 0x000406D0
			public NativeDelegateWrapper(ObjectReference<IDelegateVftbl> nativeDelegate)
			{
				this._nativeDelegate = nativeDelegate;
			}

			// Token: 0x1700047F RID: 1151
			// (get) Token: 0x060017B5 RID: 6069 RVA: 0x000424DF File Offset: 0x000406DF
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._nativeDelegate;
				}
			}

			// Token: 0x17000480 RID: 1152
			// (get) Token: 0x060017B6 RID: 6070 RVA: 0x000424E7 File Offset: 0x000406E7
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060017B7 RID: 6071 RVA: 0x000424EA File Offset: 0x000406EA
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x17000481 RID: 1153
			// (get) Token: 0x060017B8 RID: 6072 RVA: 0x00042506 File Offset: 0x00040706
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x060017B9 RID: 6073 RVA: 0x0004251A File Offset: 0x0004071A
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x17000482 RID: 1154
			// (get) Token: 0x060017BA RID: 6074 RVA: 0x00042536 File Offset: 0x00040736
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x060017BB RID: 6075 RVA: 0x0004254C File Offset: 0x0004074C
			public void Invoke(object sender, T args)
			{
				IntPtr thisPtr = this._nativeDelegate.ThisPtr;
				Delegate delegateForFunctionPointer = Marshal.GetDelegateForFunctionPointer(this._nativeDelegate.Vftbl.Invoke, EventHandler<T>.Abi_Invoke_Type);
				ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
				object obj = null;
				object[] array = new object[3];
				array[0] = thisPtr;
				object[] array2 = array;
				try
				{
					objectReferenceValue = MarshalInspectable<object>.CreateMarshaler2(sender, true);
					array2[1] = MarshalInspectable<object>.GetAbi(objectReferenceValue);
					obj = Marshaler<T>.CreateMarshaler2.Invoke(args);
					array2[2] = Marshaler<T>.GetAbi.Invoke(obj);
					delegateForFunctionPointer.DynamicInvokeAbi(array2);
				}
				finally
				{
					MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
					Marshaler<T>.DisposeMarshaler.Invoke(obj);
				}
			}

			// Token: 0x04000820 RID: 2080
			private readonly ObjectReference<IDelegateVftbl> _nativeDelegate;

			// Token: 0x04000821 RID: 2081
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x04000822 RID: 2082
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}
	}
}
