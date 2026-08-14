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
	// Token: 0x020000B6 RID: 182
	[EditorBrowsable(1)]
	[Guid("6D844858-0CFF-4590-AE89-95A5A5C8B4B8")]
	internal static class AsyncActionProgressHandler<TProgress>
	{
		// Token: 0x06000714 RID: 1812 RVA: 0x00011B70 File Offset: 0x0000FD70
		unsafe static AsyncActionProgressHandler()
		{
			AsyncActionProgressHandler<TProgress>.PIID = GuidGenerator.CreateIID(typeof(Windows.Foundation.AsyncActionProgressHandler<TProgress>));
			AsyncActionProgressHandler<TProgress>.Abi_Invoke_Type = Expression.GetDelegateType(new Type[]
			{
				typeof(void*),
				typeof(IntPtr),
				Marshaler<TProgress>.AbiType,
				typeof(int)
			});
			AsyncActionProgressHandler<TProgress>.<AbiInvokeDelegate>k__BackingField = Delegate.CreateDelegate(AsyncActionProgressHandler<TProgress>.Abi_Invoke_Type, typeof(AsyncActionProgressHandler<TProgress>).GetMethod("Do_Abi_Invoke", 40).MakeGenericMethod(new Type[]
			{
				Marshaler<TProgress>.AbiType
			}));
			AsyncActionProgressHandler<TProgress>.AbiToProjectionVftable = new IDelegateVftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				Invoke = Marshal.GetFunctionPointerForDelegate(AsyncActionProgressHandler<TProgress>.AbiInvokeDelegate)
			};
			IntPtr intPtr = ComWrappersSupport.AllocateVtableMemory(typeof(AsyncActionProgressHandler<TProgress>), Marshal.SizeOf<IDelegateVftbl>());
			Marshal.StructureToPtr<IDelegateVftbl>(AsyncActionProgressHandler<TProgress>.AbiToProjectionVftable, intPtr, false);
			AsyncActionProgressHandler<TProgress>.AbiToProjectionVftablePtr = intPtr;
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x00011C5A File Offset: 0x0000FE5A
		public static Delegate AbiInvokeDelegate
		{
			[CompilerGenerated]
			get
			{
				return AsyncActionProgressHandler<TProgress>.<AbiInvokeDelegate>k__BackingField;
			}
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00011C61 File Offset: 0x0000FE61
		public static IObjectReference CreateMarshaler(Windows.Foundation.AsyncActionProgressHandler<TProgress> managedDelegate)
		{
			if (managedDelegate != null)
			{
				return MarshalDelegate.CreateMarshaler(managedDelegate, GuidGenerator.GetIID(typeof(AsyncActionProgressHandler<TProgress>)), true);
			}
			return null;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00011C7E File Offset: 0x0000FE7E
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.AsyncActionProgressHandler<TProgress> managedDelegate)
		{
			return MarshalDelegate.CreateMarshaler2(managedDelegate, GuidGenerator.GetIID(typeof(AsyncActionProgressHandler<TProgress>)), true);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00011C96 File Offset: 0x0000FE96
		public static IntPtr GetAbi(IObjectReference value)
		{
			return MarshalInterfaceHelper<Windows.Foundation.AsyncActionProgressHandler<TProgress>>.GetAbi(value);
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00011C9E File Offset: 0x0000FE9E
		public static Windows.Foundation.AsyncActionProgressHandler<TProgress> FromAbi(IntPtr nativeDelegate)
		{
			return MarshalDelegate.FromAbi<Windows.Foundation.AsyncActionProgressHandler<TProgress>>(nativeDelegate);
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00011CA6 File Offset: 0x0000FEA6
		public static Windows.Foundation.AsyncActionProgressHandler<TProgress> CreateRcw(IntPtr ptr)
		{
			return new Windows.Foundation.AsyncActionProgressHandler<TProgress>(new AsyncActionProgressHandler<TProgress>.NativeDelegateWrapper(ComWrappersSupport.GetObjectReferenceForInterface<IDelegateVftbl>(ptr, GuidGenerator.GetIID(typeof(AsyncActionProgressHandler<TProgress>)))).Invoke);
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00011CD0 File Offset: 0x0000FED0
		public static IntPtr FromManaged(Windows.Foundation.AsyncActionProgressHandler<TProgress> managedDelegate)
		{
			return AsyncActionProgressHandler<TProgress>.CreateMarshaler2(managedDelegate).Detach();
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00011CEB File Offset: 0x0000FEEB
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<Windows.Foundation.AsyncActionProgressHandler<TProgress>>.DisposeMarshaler(value);
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00011CF3 File Offset: 0x0000FEF3
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<Windows.Foundation.AsyncActionProgressHandler<TProgress>>.DisposeAbi(abi);
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x00011CFC File Offset: 0x0000FEFC
		private unsafe static int Do_Abi_Invoke<TProgressAbi>(void* thisPtr, IntPtr asyncInfo, TProgressAbi progressInfo)
		{
			try
			{
				ComWrappersSupport.FindObject<Windows.Foundation.AsyncActionProgressHandler<TProgress>>(new IntPtr(thisPtr)).Invoke(MarshalInterface<Windows.Foundation.IAsyncActionWithProgress<TProgress>>.FromAbi(asyncInfo), Marshaler<TProgress>.FromAbi.Invoke(progressInfo));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0400015C RID: 348
		public static Guid PIID;

		// Token: 0x0400015D RID: 349
		private static readonly Type Abi_Invoke_Type;

		// Token: 0x0400015E RID: 350
		private static readonly IDelegateVftbl AbiToProjectionVftable;

		// Token: 0x0400015F RID: 351
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x020002F8 RID: 760
		[ObjectReferenceWrapper("_nativeDelegate")]
		private sealed class NativeDelegateWrapper : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x06001688 RID: 5768 RVA: 0x0003FE34 File Offset: 0x0003E034
			public NativeDelegateWrapper(ObjectReference<IDelegateVftbl> nativeDelegate)
			{
				this._nativeDelegate = nativeDelegate;
			}

			// Token: 0x1700043C RID: 1084
			// (get) Token: 0x06001689 RID: 5769 RVA: 0x0003FE43 File Offset: 0x0003E043
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._nativeDelegate;
				}
			}

			// Token: 0x1700043D RID: 1085
			// (get) Token: 0x0600168A RID: 5770 RVA: 0x0003FE4B File Offset: 0x0003E04B
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return true;
				}
			}

			// Token: 0x0600168B RID: 5771 RVA: 0x0003FE4E File Offset: 0x0003E04E
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x1700043E RID: 1086
			// (get) Token: 0x0600168C RID: 5772 RVA: 0x0003FE6A File Offset: 0x0003E06A
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x0600168D RID: 5773 RVA: 0x0003FE7E File Offset: 0x0003E07E
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x1700043F RID: 1087
			// (get) Token: 0x0600168E RID: 5774 RVA: 0x0003FE9A File Offset: 0x0003E09A
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x0600168F RID: 5775 RVA: 0x0003FEB0 File Offset: 0x0003E0B0
			public void Invoke(Windows.Foundation.IAsyncActionWithProgress<TProgress> asyncInfo, TProgress progressInfo)
			{
				IntPtr thisPtr = this._nativeDelegate.ThisPtr;
				Delegate delegateForFunctionPointer = Marshal.GetDelegateForFunctionPointer(this._nativeDelegate.Vftbl.Invoke, AsyncActionProgressHandler<TProgress>.Abi_Invoke_Type);
				ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
				object obj = null;
				object[] array = new object[3];
				array[0] = thisPtr;
				object[] array2 = array;
				try
				{
					objectReferenceValue = MarshalInterface<Windows.Foundation.IAsyncActionWithProgress<TProgress>>.CreateMarshaler2(asyncInfo, default(Guid));
					array2[1] = MarshalInspectable<object>.GetAbi(objectReferenceValue);
					obj = Marshaler<TProgress>.CreateMarshaler.Invoke(progressInfo);
					array2[2] = Marshaler<TProgress>.GetAbi.Invoke(obj);
					delegateForFunctionPointer.DynamicInvokeAbi(array2);
				}
				finally
				{
					MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
					Marshaler<TProgress>.DisposeMarshaler.Invoke(obj);
				}
			}

			// Token: 0x04000792 RID: 1938
			private readonly ObjectReference<IDelegateVftbl> _nativeDelegate;

			// Token: 0x04000793 RID: 1939
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x04000794 RID: 1940
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}
	}
}
