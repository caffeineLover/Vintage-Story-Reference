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
	// Token: 0x020000BA RID: 186
	[EditorBrowsable(1)]
	[Guid("E85DF41D-6AA7-46E3-A8E2-F009D840C627")]
	internal static class AsyncOperationWithProgressCompletedHandler<TResult, TProgress>
	{
		// Token: 0x06000740 RID: 1856 RVA: 0x000122C0 File Offset: 0x000104C0
		unsafe static AsyncOperationWithProgressCompletedHandler()
		{
			AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.PIID = GuidGenerator.CreateIID(typeof(Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress>));
			AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.Abi_Invoke_Type = Expression.GetDelegateType(new Type[]
			{
				typeof(void*),
				typeof(IntPtr),
				typeof(AsyncStatus),
				typeof(int)
			});
			AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.<AbiInvokeDelegate>k__BackingField = Delegate.CreateDelegate(AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.Abi_Invoke_Type, typeof(AsyncOperationWithProgressCompletedHandler<TResult, TProgress>).GetMethod("Do_Abi_Invoke", 40));
			AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.AbiToProjectionVftable = new IDelegateVftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				Invoke = Marshal.GetFunctionPointerForDelegate(AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.AbiInvokeDelegate)
			};
			IntPtr intPtr = ComWrappersSupport.AllocateVtableMemory(typeof(AsyncOperationWithProgressCompletedHandler<TResult, TProgress>), Marshal.SizeOf<IDelegateVftbl>());
			Marshal.StructureToPtr<IDelegateVftbl>(AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.AbiToProjectionVftable, intPtr, false);
			AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.AbiToProjectionVftablePtr = intPtr;
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x0001239C File Offset: 0x0001059C
		public static Delegate AbiInvokeDelegate
		{
			[CompilerGenerated]
			get
			{
				return AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.<AbiInvokeDelegate>k__BackingField;
			}
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x000123A3 File Offset: 0x000105A3
		public static IObjectReference CreateMarshaler(Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress> managedDelegate)
		{
			if (managedDelegate != null)
			{
				return MarshalDelegate.CreateMarshaler(managedDelegate, GuidGenerator.GetIID(typeof(AsyncOperationWithProgressCompletedHandler<TResult, TProgress>)), true);
			}
			return null;
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x000123C0 File Offset: 0x000105C0
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress> managedDelegate)
		{
			return MarshalDelegate.CreateMarshaler2(managedDelegate, GuidGenerator.GetIID(typeof(AsyncOperationWithProgressCompletedHandler<TResult, TProgress>)), true);
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x000123D8 File Offset: 0x000105D8
		public static IntPtr GetAbi(IObjectReference value)
		{
			return MarshalInterfaceHelper<Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress>>.GetAbi(value);
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x000123E0 File Offset: 0x000105E0
		public static Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress> FromAbi(IntPtr nativeDelegate)
		{
			return MarshalDelegate.FromAbi<Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress>>(nativeDelegate);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x000123E8 File Offset: 0x000105E8
		public static Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress> CreateRcw(IntPtr ptr)
		{
			return new Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress>(new AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.NativeDelegateWrapper(ComWrappersSupport.GetObjectReferenceForInterface<IDelegateVftbl>(ptr, GuidGenerator.GetIID(typeof(AsyncOperationWithProgressCompletedHandler<TResult, TProgress>)))).Invoke);
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x00012410 File Offset: 0x00010610
		public static IntPtr FromManaged(Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress> managedDelegate)
		{
			return AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.CreateMarshaler2(managedDelegate).Detach();
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0001242B File Offset: 0x0001062B
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress>>.DisposeMarshaler(value);
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x00012433 File Offset: 0x00010633
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress>>.DisposeAbi(abi);
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0001243C File Offset: 0x0001063C
		private unsafe static int Do_Abi_Invoke(void* thisPtr, IntPtr asyncInfo, AsyncStatus asyncStatus)
		{
			try
			{
				ComWrappersSupport.FindObject<Windows.Foundation.AsyncOperationWithProgressCompletedHandler<TResult, TProgress>>(new IntPtr(thisPtr)).Invoke(MarshalInterface<Windows.Foundation.IAsyncOperationWithProgress<TResult, TProgress>>.FromAbi(asyncInfo), asyncStatus);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x04000170 RID: 368
		public static Guid PIID;

		// Token: 0x04000171 RID: 369
		private static readonly Type Abi_Invoke_Type;

		// Token: 0x04000172 RID: 370
		private static readonly IDelegateVftbl AbiToProjectionVftable;

		// Token: 0x04000173 RID: 371
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x020002FC RID: 764
		[ObjectReferenceWrapper("_nativeDelegate")]
		private sealed class NativeDelegateWrapper : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x060016A8 RID: 5800 RVA: 0x000402BC File Offset: 0x0003E4BC
			public NativeDelegateWrapper(ObjectReference<IDelegateVftbl> nativeDelegate)
			{
				this._nativeDelegate = nativeDelegate;
			}

			// Token: 0x1700044C RID: 1100
			// (get) Token: 0x060016A9 RID: 5801 RVA: 0x000402CB File Offset: 0x0003E4CB
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._nativeDelegate;
				}
			}

			// Token: 0x1700044D RID: 1101
			// (get) Token: 0x060016AA RID: 5802 RVA: 0x000402D3 File Offset: 0x0003E4D3
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060016AB RID: 5803 RVA: 0x000402D6 File Offset: 0x0003E4D6
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x1700044E RID: 1102
			// (get) Token: 0x060016AC RID: 5804 RVA: 0x000402F2 File Offset: 0x0003E4F2
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x060016AD RID: 5805 RVA: 0x00040306 File Offset: 0x0003E506
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x1700044F RID: 1103
			// (get) Token: 0x060016AE RID: 5806 RVA: 0x00040322 File Offset: 0x0003E522
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x060016AF RID: 5807 RVA: 0x00040338 File Offset: 0x0003E538
			public void Invoke(Windows.Foundation.IAsyncOperationWithProgress<TResult, TProgress> asyncInfo, AsyncStatus asyncStatus)
			{
				IntPtr thisPtr = this._nativeDelegate.ThisPtr;
				Delegate delegateForFunctionPointer = Marshal.GetDelegateForFunctionPointer(this._nativeDelegate.Vftbl.Invoke, AsyncOperationWithProgressCompletedHandler<TResult, TProgress>.Abi_Invoke_Type);
				ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
				object[] array = new object[]
				{
					thisPtr,
					default(object),
					asyncStatus
				};
				try
				{
					objectReferenceValue = MarshalInterface<Windows.Foundation.IAsyncOperationWithProgress<TResult, TProgress>>.CreateMarshaler2(asyncInfo, default(Guid));
					array[1] = MarshalInspectable<object>.GetAbi(objectReferenceValue);
					delegateForFunctionPointer.DynamicInvokeAbi(array);
				}
				finally
				{
					MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
				}
			}

			// Token: 0x0400079E RID: 1950
			private readonly ObjectReference<IDelegateVftbl> _nativeDelegate;

			// Token: 0x0400079F RID: 1951
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x040007A0 RID: 1952
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}
	}
}
