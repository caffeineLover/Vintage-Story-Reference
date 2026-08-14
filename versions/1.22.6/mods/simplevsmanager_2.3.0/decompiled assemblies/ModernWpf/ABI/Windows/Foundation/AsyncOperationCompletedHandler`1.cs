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
	// Token: 0x020000B8 RID: 184
	[EditorBrowsable(1)]
	[Guid("FCDCF02C-E5D8-4478-915A-4D90B74B83A5")]
	internal static class AsyncOperationCompletedHandler<TResult>
	{
		// Token: 0x0600072A RID: 1834 RVA: 0x00011F18 File Offset: 0x00010118
		unsafe static AsyncOperationCompletedHandler()
		{
			AsyncOperationCompletedHandler<TResult>.PIID = GuidGenerator.CreateIID(typeof(Windows.Foundation.AsyncOperationCompletedHandler<TResult>));
			AsyncOperationCompletedHandler<TResult>.Abi_Invoke_Type = Expression.GetDelegateType(new Type[]
			{
				typeof(void*),
				typeof(IntPtr),
				typeof(AsyncStatus),
				typeof(int)
			});
			AsyncOperationCompletedHandler<TResult>.<AbiInvokeDelegate>k__BackingField = Delegate.CreateDelegate(AsyncOperationCompletedHandler<TResult>.Abi_Invoke_Type, typeof(AsyncOperationCompletedHandler<TResult>).GetMethod("Do_Abi_Invoke", 40));
			AsyncOperationCompletedHandler<TResult>.AbiToProjectionVftable = new IDelegateVftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				Invoke = Marshal.GetFunctionPointerForDelegate(AsyncOperationCompletedHandler<TResult>.AbiInvokeDelegate)
			};
			IntPtr intPtr = ComWrappersSupport.AllocateVtableMemory(typeof(AsyncOperationCompletedHandler<TResult>), Marshal.SizeOf<IDelegateVftbl>());
			Marshal.StructureToPtr<IDelegateVftbl>(AsyncOperationCompletedHandler<TResult>.AbiToProjectionVftable, intPtr, false);
			AsyncOperationCompletedHandler<TResult>.AbiToProjectionVftablePtr = intPtr;
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x0600072B RID: 1835 RVA: 0x00011FF4 File Offset: 0x000101F4
		public static Delegate AbiInvokeDelegate
		{
			[CompilerGenerated]
			get
			{
				return AsyncOperationCompletedHandler<TResult>.<AbiInvokeDelegate>k__BackingField;
			}
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00011FFB File Offset: 0x000101FB
		public static IObjectReference CreateMarshaler(Windows.Foundation.AsyncOperationCompletedHandler<TResult> managedDelegate)
		{
			if (managedDelegate != null)
			{
				return MarshalDelegate.CreateMarshaler(managedDelegate, GuidGenerator.GetIID(typeof(AsyncOperationCompletedHandler<TResult>)), true);
			}
			return null;
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00012018 File Offset: 0x00010218
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.AsyncOperationCompletedHandler<TResult> managedDelegate)
		{
			return MarshalDelegate.CreateMarshaler2(managedDelegate, GuidGenerator.GetIID(typeof(AsyncOperationCompletedHandler<TResult>)), true);
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00012030 File Offset: 0x00010230
		public static IntPtr GetAbi(IObjectReference value)
		{
			return MarshalInterfaceHelper<Windows.Foundation.AsyncOperationCompletedHandler<TResult>>.GetAbi(value);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00012038 File Offset: 0x00010238
		public static Windows.Foundation.AsyncOperationCompletedHandler<TResult> FromAbi(IntPtr nativeDelegate)
		{
			return MarshalDelegate.FromAbi<Windows.Foundation.AsyncOperationCompletedHandler<TResult>>(nativeDelegate);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00012040 File Offset: 0x00010240
		public static Windows.Foundation.AsyncOperationCompletedHandler<TResult> CreateRcw(IntPtr ptr)
		{
			return new Windows.Foundation.AsyncOperationCompletedHandler<TResult>(new AsyncOperationCompletedHandler<TResult>.NativeDelegateWrapper(ComWrappersSupport.GetObjectReferenceForInterface<IDelegateVftbl>(ptr, GuidGenerator.GetIID(typeof(AsyncOperationCompletedHandler<TResult>)))).Invoke);
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00012068 File Offset: 0x00010268
		public static IntPtr FromManaged(Windows.Foundation.AsyncOperationCompletedHandler<TResult> managedDelegate)
		{
			return AsyncOperationCompletedHandler<TResult>.CreateMarshaler2(managedDelegate).Detach();
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00012083 File Offset: 0x00010283
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<Windows.Foundation.AsyncOperationCompletedHandler<TResult>>.DisposeMarshaler(value);
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0001208B File Offset: 0x0001028B
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<Windows.Foundation.AsyncOperationCompletedHandler<TResult>>.DisposeAbi(abi);
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00012094 File Offset: 0x00010294
		private unsafe static int Do_Abi_Invoke(void* thisPtr, IntPtr asyncInfo, AsyncStatus asyncStatus)
		{
			try
			{
				ComWrappersSupport.FindObject<Windows.Foundation.AsyncOperationCompletedHandler<TResult>>(new IntPtr(thisPtr)).Invoke(MarshalInterface<Windows.Foundation.IAsyncOperation<TResult>>.FromAbi(asyncInfo), asyncStatus);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x04000166 RID: 358
		public static Guid PIID;

		// Token: 0x04000167 RID: 359
		private static readonly Type Abi_Invoke_Type;

		// Token: 0x04000168 RID: 360
		private static readonly IDelegateVftbl AbiToProjectionVftable;

		// Token: 0x04000169 RID: 361
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x020002FA RID: 762
		[ObjectReferenceWrapper("_nativeDelegate")]
		private sealed class NativeDelegateWrapper : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x06001698 RID: 5784 RVA: 0x00040078 File Offset: 0x0003E278
			public NativeDelegateWrapper(ObjectReference<IDelegateVftbl> nativeDelegate)
			{
				this._nativeDelegate = nativeDelegate;
			}

			// Token: 0x17000444 RID: 1092
			// (get) Token: 0x06001699 RID: 5785 RVA: 0x00040087 File Offset: 0x0003E287
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._nativeDelegate;
				}
			}

			// Token: 0x17000445 RID: 1093
			// (get) Token: 0x0600169A RID: 5786 RVA: 0x0004008F File Offset: 0x0003E28F
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return true;
				}
			}

			// Token: 0x0600169B RID: 5787 RVA: 0x00040092 File Offset: 0x0003E292
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x17000446 RID: 1094
			// (get) Token: 0x0600169C RID: 5788 RVA: 0x000400AE File Offset: 0x0003E2AE
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x0600169D RID: 5789 RVA: 0x000400C2 File Offset: 0x0003E2C2
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x17000447 RID: 1095
			// (get) Token: 0x0600169E RID: 5790 RVA: 0x000400DE File Offset: 0x0003E2DE
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x0600169F RID: 5791 RVA: 0x000400F4 File Offset: 0x0003E2F4
			public void Invoke(Windows.Foundation.IAsyncOperation<TResult> asyncInfo, AsyncStatus asyncStatus)
			{
				IntPtr thisPtr = this._nativeDelegate.ThisPtr;
				Delegate delegateForFunctionPointer = Marshal.GetDelegateForFunctionPointer(this._nativeDelegate.Vftbl.Invoke, AsyncOperationCompletedHandler<TResult>.Abi_Invoke_Type);
				ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
				object[] array = new object[]
				{
					thisPtr,
					default(object),
					asyncStatus
				};
				try
				{
					objectReferenceValue = MarshalInterface<Windows.Foundation.IAsyncOperation<TResult>>.CreateMarshaler2(asyncInfo, default(Guid));
					array[1] = MarshalInspectable<object>.GetAbi(objectReferenceValue);
					delegateForFunctionPointer.DynamicInvokeAbi(array);
				}
				finally
				{
					MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
				}
			}

			// Token: 0x04000798 RID: 1944
			private readonly ObjectReference<IDelegateVftbl> _nativeDelegate;

			// Token: 0x04000799 RID: 1945
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x0400079A RID: 1946
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}
	}
}
