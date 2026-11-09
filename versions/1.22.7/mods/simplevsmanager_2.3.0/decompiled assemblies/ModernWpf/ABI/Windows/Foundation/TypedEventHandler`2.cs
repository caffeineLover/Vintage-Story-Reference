using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Windows.Foundation;
using WinRT;
using WinRT.Interop;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000EB RID: 235
	[EditorBrowsable(1)]
	[Guid("9DE1C534-6AE1-11E0-84E1-18A905BCC53F")]
	internal static class TypedEventHandler<TSender, TResult>
	{
		// Token: 0x06000878 RID: 2168 RVA: 0x000182C4 File Offset: 0x000164C4
		unsafe static TypedEventHandler()
		{
			TypedEventHandler<TSender, TResult>.PIID = GuidGenerator.CreateIID(typeof(Windows.Foundation.TypedEventHandler<TSender, TResult>));
			TypedEventHandler<TSender, TResult>.Abi_Invoke_Type = Expression.GetDelegateType(new Type[]
			{
				typeof(void*),
				Marshaler<TSender>.AbiType,
				Marshaler<TResult>.AbiType,
				typeof(int)
			});
			TypedEventHandler<TSender, TResult>.<AbiInvokeDelegate>k__BackingField = Delegate.CreateDelegate(TypedEventHandler<TSender, TResult>.Abi_Invoke_Type, typeof(TypedEventHandler<TSender, TResult>).GetMethod("Do_Abi_Invoke", 40).MakeGenericMethod(new Type[]
			{
				Marshaler<TSender>.AbiType,
				Marshaler<TResult>.AbiType
			}));
			TypedEventHandler<TSender, TResult>.AbiToProjectionVftable = new IDelegateVftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				Invoke = Marshal.GetFunctionPointerForDelegate(TypedEventHandler<TSender, TResult>.AbiInvokeDelegate)
			};
			IntPtr intPtr = ComWrappersSupport.AllocateVtableMemory(typeof(TypedEventHandler<TSender, TResult>), Marshal.SizeOf<IDelegateVftbl>());
			Marshal.StructureToPtr<IDelegateVftbl>(TypedEventHandler<TSender, TResult>.AbiToProjectionVftable, intPtr, false);
			TypedEventHandler<TSender, TResult>.AbiToProjectionVftablePtr = intPtr;
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x000183B1 File Offset: 0x000165B1
		public static Delegate AbiInvokeDelegate
		{
			[CompilerGenerated]
			get
			{
				return TypedEventHandler<TSender, TResult>.<AbiInvokeDelegate>k__BackingField;
			}
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x000183B8 File Offset: 0x000165B8
		public static IObjectReference CreateMarshaler(Windows.Foundation.TypedEventHandler<TSender, TResult> managedDelegate)
		{
			if (managedDelegate != null)
			{
				return MarshalDelegate.CreateMarshaler(managedDelegate, GuidGenerator.GetIID(typeof(TypedEventHandler<TSender, TResult>)), true);
			}
			return null;
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x000183D5 File Offset: 0x000165D5
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.TypedEventHandler<TSender, TResult> managedDelegate)
		{
			return MarshalDelegate.CreateMarshaler2(managedDelegate, GuidGenerator.GetIID(typeof(TypedEventHandler<TSender, TResult>)), true);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x000183ED File Offset: 0x000165ED
		public static IntPtr GetAbi(IObjectReference value)
		{
			return MarshalInterfaceHelper<Windows.Foundation.TypedEventHandler<TSender, TResult>>.GetAbi(value);
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x000183F5 File Offset: 0x000165F5
		public static Windows.Foundation.TypedEventHandler<TSender, TResult> FromAbi(IntPtr nativeDelegate)
		{
			return MarshalDelegate.FromAbi<Windows.Foundation.TypedEventHandler<TSender, TResult>>(nativeDelegate);
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x000183FD File Offset: 0x000165FD
		public static Windows.Foundation.TypedEventHandler<TSender, TResult> CreateRcw(IntPtr ptr)
		{
			return new Windows.Foundation.TypedEventHandler<TSender, TResult>(new TypedEventHandler<TSender, TResult>.NativeDelegateWrapper(ComWrappersSupport.GetObjectReferenceForInterface<IDelegateVftbl>(ptr, GuidGenerator.GetIID(typeof(TypedEventHandler<TSender, TResult>)))).Invoke);
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00018424 File Offset: 0x00016624
		public static IntPtr FromManaged(Windows.Foundation.TypedEventHandler<TSender, TResult> managedDelegate)
		{
			return TypedEventHandler<TSender, TResult>.CreateMarshaler2(managedDelegate).Detach();
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0001843F File Offset: 0x0001663F
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<Windows.Foundation.TypedEventHandler<TSender, TResult>>.DisposeMarshaler(value);
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00018447 File Offset: 0x00016647
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<Windows.Foundation.TypedEventHandler<TSender, TResult>>.DisposeAbi(abi);
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00018450 File Offset: 0x00016650
		private unsafe static int Do_Abi_Invoke<TSenderAbi, TResultAbi>(void* thisPtr, TSenderAbi sender, TResultAbi args)
		{
			try
			{
				ComWrappersSupport.FindObject<Windows.Foundation.TypedEventHandler<TSender, TResult>>(new IntPtr(thisPtr)).Invoke(Marshaler<TSender>.FromAbi.Invoke(sender), Marshaler<TResult>.FromAbi.Invoke(args));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0400018E RID: 398
		public static Guid PIID;

		// Token: 0x0400018F RID: 399
		private static readonly Type Abi_Invoke_Type;

		// Token: 0x04000190 RID: 400
		private static readonly IDelegateVftbl AbiToProjectionVftable;

		// Token: 0x04000191 RID: 401
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x0200030F RID: 783
		[ObjectReferenceWrapper("_nativeDelegate")]
		private sealed class NativeDelegateWrapper : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x06001717 RID: 5911 RVA: 0x00041149 File Offset: 0x0003F349
			public NativeDelegateWrapper(ObjectReference<IDelegateVftbl> nativeDelegate)
			{
				this._nativeDelegate = nativeDelegate;
			}

			// Token: 0x1700045F RID: 1119
			// (get) Token: 0x06001718 RID: 5912 RVA: 0x00041158 File Offset: 0x0003F358
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._nativeDelegate;
				}
			}

			// Token: 0x17000460 RID: 1120
			// (get) Token: 0x06001719 RID: 5913 RVA: 0x00041160 File Offset: 0x0003F360
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return true;
				}
			}

			// Token: 0x0600171A RID: 5914 RVA: 0x00041163 File Offset: 0x0003F363
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x17000461 RID: 1121
			// (get) Token: 0x0600171B RID: 5915 RVA: 0x0004117F File Offset: 0x0003F37F
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x0600171C RID: 5916 RVA: 0x00041193 File Offset: 0x0003F393
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x17000462 RID: 1122
			// (get) Token: 0x0600171D RID: 5917 RVA: 0x000411AF File Offset: 0x0003F3AF
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x0600171E RID: 5918 RVA: 0x000411C4 File Offset: 0x0003F3C4
			public void Invoke(TSender sender, TResult args)
			{
				IntPtr thisPtr = this._nativeDelegate.ThisPtr;
				Delegate delegateForFunctionPointer = Marshal.GetDelegateForFunctionPointer(this._nativeDelegate.Vftbl.Invoke, TypedEventHandler<TSender, TResult>.Abi_Invoke_Type);
				object obj = null;
				object obj2 = null;
				object[] array = new object[3];
				array[0] = thisPtr;
				object[] array2 = array;
				try
				{
					obj = Marshaler<TSender>.CreateMarshaler.Invoke(sender);
					array2[1] = Marshaler<TSender>.GetAbi.Invoke(obj);
					obj2 = Marshaler<TResult>.CreateMarshaler.Invoke(args);
					array2[2] = Marshaler<TResult>.GetAbi.Invoke(obj2);
					delegateForFunctionPointer.DynamicInvokeAbi(array2);
				}
				finally
				{
					Marshaler<TSender>.DisposeMarshaler.Invoke(obj);
					Marshaler<TResult>.DisposeMarshaler.Invoke(obj2);
				}
			}

			// Token: 0x040007C9 RID: 1993
			private readonly ObjectReference<IDelegateVftbl> _nativeDelegate;

			// Token: 0x040007CA RID: 1994
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x040007CB RID: 1995
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}
	}
}
