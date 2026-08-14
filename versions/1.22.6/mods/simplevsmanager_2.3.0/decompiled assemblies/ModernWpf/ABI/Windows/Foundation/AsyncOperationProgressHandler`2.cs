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
	// Token: 0x020000B9 RID: 185
	[EditorBrowsable(1)]
	[Guid("55690902-0AAB-421A-8778-F8CE5026D758")]
	internal static class AsyncOperationProgressHandler<TResult, TProgress>
	{
		// Token: 0x06000735 RID: 1845 RVA: 0x000120DC File Offset: 0x000102DC
		unsafe static AsyncOperationProgressHandler()
		{
			AsyncOperationProgressHandler<TResult, TProgress>.PIID = GuidGenerator.CreateIID(typeof(Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress>));
			AsyncOperationProgressHandler<TResult, TProgress>.Abi_Invoke_Type = Expression.GetDelegateType(new Type[]
			{
				typeof(void*),
				typeof(IntPtr),
				Marshaler<TProgress>.AbiType,
				typeof(int)
			});
			AsyncOperationProgressHandler<TResult, TProgress>.<AbiInvokeDelegate>k__BackingField = Delegate.CreateDelegate(AsyncOperationProgressHandler<TResult, TProgress>.Abi_Invoke_Type, typeof(AsyncOperationProgressHandler<TResult, TProgress>).GetMethod("Do_Abi_Invoke", 40).MakeGenericMethod(new Type[]
			{
				Marshaler<TProgress>.AbiType
			}));
			AsyncOperationProgressHandler<TResult, TProgress>.AbiToProjectionVftable = new IDelegateVftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				Invoke = Marshal.GetFunctionPointerForDelegate(AsyncOperationProgressHandler<TResult, TProgress>.AbiInvokeDelegate)
			};
			IntPtr intPtr = ComWrappersSupport.AllocateVtableMemory(typeof(AsyncOperationProgressHandler<TResult, TProgress>), Marshal.SizeOf<IDelegateVftbl>());
			Marshal.StructureToPtr<IDelegateVftbl>(AsyncOperationProgressHandler<TResult, TProgress>.AbiToProjectionVftable, intPtr, false);
			AsyncOperationProgressHandler<TResult, TProgress>.AbiToProjectionVftablePtr = intPtr;
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x000121C6 File Offset: 0x000103C6
		public static Delegate AbiInvokeDelegate
		{
			[CompilerGenerated]
			get
			{
				return AsyncOperationProgressHandler<TResult, TProgress>.<AbiInvokeDelegate>k__BackingField;
			}
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x000121CD File Offset: 0x000103CD
		public static IObjectReference CreateMarshaler(Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress> managedDelegate)
		{
			if (managedDelegate != null)
			{
				return MarshalDelegate.CreateMarshaler(managedDelegate, GuidGenerator.GetIID(typeof(AsyncOperationProgressHandler<TResult, TProgress>)), true);
			}
			return null;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x000121EA File Offset: 0x000103EA
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress> managedDelegate)
		{
			return MarshalDelegate.CreateMarshaler2(managedDelegate, GuidGenerator.GetIID(typeof(AsyncOperationProgressHandler<TResult, TProgress>)), true);
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00012202 File Offset: 0x00010402
		public static IntPtr GetAbi(IObjectReference value)
		{
			return MarshalInterfaceHelper<Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress>>.GetAbi(value);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001220A File Offset: 0x0001040A
		public static Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress> FromAbi(IntPtr nativeDelegate)
		{
			return MarshalDelegate.FromAbi<Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress>>(nativeDelegate);
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00012212 File Offset: 0x00010412
		public static Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress> CreateRcw(IntPtr ptr)
		{
			return new Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress>(new AsyncOperationProgressHandler<TResult, TProgress>.NativeDelegateWrapper(ComWrappersSupport.GetObjectReferenceForInterface<IDelegateVftbl>(ptr, GuidGenerator.GetIID(typeof(AsyncOperationProgressHandler<TResult, TProgress>)))).Invoke);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0001223C File Offset: 0x0001043C
		public static IntPtr FromManaged(Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress> managedDelegate)
		{
			return AsyncOperationProgressHandler<TResult, TProgress>.CreateMarshaler2(managedDelegate).Detach();
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x00012257 File Offset: 0x00010457
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress>>.DisposeMarshaler(value);
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0001225F File Offset: 0x0001045F
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress>>.DisposeAbi(abi);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00012268 File Offset: 0x00010468
		private unsafe static int Do_Abi_Invoke<TProgressAbi>(void* thisPtr, IntPtr asyncInfo, TProgressAbi progressInfo)
		{
			try
			{
				ComWrappersSupport.FindObject<Windows.Foundation.AsyncOperationProgressHandler<TResult, TProgress>>(new IntPtr(thisPtr)).Invoke(MarshalInterface<Windows.Foundation.IAsyncOperationWithProgress<TResult, TProgress>>.FromAbi(asyncInfo), Marshaler<TProgress>.FromAbi.Invoke(progressInfo));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0400016B RID: 363
		public static Guid PIID;

		// Token: 0x0400016C RID: 364
		private static readonly Type Abi_Invoke_Type;

		// Token: 0x0400016D RID: 365
		private static readonly IDelegateVftbl AbiToProjectionVftable;

		// Token: 0x0400016E RID: 366
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x020002FB RID: 763
		[ObjectReferenceWrapper("_nativeDelegate")]
		private sealed class NativeDelegateWrapper : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x060016A0 RID: 5792 RVA: 0x00040188 File Offset: 0x0003E388
			public NativeDelegateWrapper(ObjectReference<IDelegateVftbl> nativeDelegate)
			{
				this._nativeDelegate = nativeDelegate;
			}

			// Token: 0x17000448 RID: 1096
			// (get) Token: 0x060016A1 RID: 5793 RVA: 0x00040197 File Offset: 0x0003E397
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._nativeDelegate;
				}
			}

			// Token: 0x17000449 RID: 1097
			// (get) Token: 0x060016A2 RID: 5794 RVA: 0x0004019F File Offset: 0x0003E39F
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060016A3 RID: 5795 RVA: 0x000401A2 File Offset: 0x0003E3A2
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x1700044A RID: 1098
			// (get) Token: 0x060016A4 RID: 5796 RVA: 0x000401BE File Offset: 0x0003E3BE
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x060016A5 RID: 5797 RVA: 0x000401D2 File Offset: 0x0003E3D2
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x1700044B RID: 1099
			// (get) Token: 0x060016A6 RID: 5798 RVA: 0x000401EE File Offset: 0x0003E3EE
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x060016A7 RID: 5799 RVA: 0x00040204 File Offset: 0x0003E404
			public void Invoke(Windows.Foundation.IAsyncOperationWithProgress<TResult, TProgress> asyncInfo, TProgress progressInfo)
			{
				IntPtr thisPtr = this._nativeDelegate.ThisPtr;
				Delegate delegateForFunctionPointer = Marshal.GetDelegateForFunctionPointer(this._nativeDelegate.Vftbl.Invoke, AsyncOperationProgressHandler<TResult, TProgress>.Abi_Invoke_Type);
				ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
				object obj = null;
				object[] array = new object[3];
				array[0] = thisPtr;
				object[] array2 = array;
				try
				{
					objectReferenceValue = MarshalInterface<Windows.Foundation.IAsyncOperationWithProgress<TResult, TProgress>>.CreateMarshaler2(asyncInfo, default(Guid));
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

			// Token: 0x0400079B RID: 1947
			private readonly ObjectReference<IDelegateVftbl> _nativeDelegate;

			// Token: 0x0400079C RID: 1948
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x0400079D RID: 1949
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}
	}
}
