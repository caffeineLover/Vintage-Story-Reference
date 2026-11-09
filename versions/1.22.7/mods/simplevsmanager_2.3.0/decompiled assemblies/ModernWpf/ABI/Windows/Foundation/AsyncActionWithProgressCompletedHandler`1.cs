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
	// Token: 0x020000B7 RID: 183
	[EditorBrowsable(1)]
	[Guid("9C029F91-CC84-44FD-AC26-0A6C4E555281")]
	internal static class AsyncActionWithProgressCompletedHandler<TProgress>
	{
		// Token: 0x0600071F RID: 1823 RVA: 0x00011D54 File Offset: 0x0000FF54
		unsafe static AsyncActionWithProgressCompletedHandler()
		{
			AsyncActionWithProgressCompletedHandler<TProgress>.PIID = GuidGenerator.CreateIID(typeof(Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress>));
			AsyncActionWithProgressCompletedHandler<TProgress>.Abi_Invoke_Type = Expression.GetDelegateType(new Type[]
			{
				typeof(void*),
				typeof(IntPtr),
				typeof(AsyncStatus),
				typeof(int)
			});
			AsyncActionWithProgressCompletedHandler<TProgress>.<AbiInvokeDelegate>k__BackingField = Delegate.CreateDelegate(AsyncActionWithProgressCompletedHandler<TProgress>.Abi_Invoke_Type, typeof(AsyncActionWithProgressCompletedHandler<TProgress>).GetMethod("Do_Abi_Invoke", 40));
			AsyncActionWithProgressCompletedHandler<TProgress>.AbiToProjectionVftable = new IDelegateVftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				Invoke = Marshal.GetFunctionPointerForDelegate(AsyncActionWithProgressCompletedHandler<TProgress>.AbiInvokeDelegate)
			};
			IntPtr intPtr = ComWrappersSupport.AllocateVtableMemory(typeof(AsyncActionWithProgressCompletedHandler<TProgress>), Marshal.SizeOf<IDelegateVftbl>());
			Marshal.StructureToPtr<IDelegateVftbl>(AsyncActionWithProgressCompletedHandler<TProgress>.AbiToProjectionVftable, intPtr, false);
			AsyncActionWithProgressCompletedHandler<TProgress>.AbiToProjectionVftablePtr = intPtr;
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x00011E30 File Offset: 0x00010030
		public static Delegate AbiInvokeDelegate
		{
			[CompilerGenerated]
			get
			{
				return AsyncActionWithProgressCompletedHandler<TProgress>.<AbiInvokeDelegate>k__BackingField;
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00011E37 File Offset: 0x00010037
		public static IObjectReference CreateMarshaler(Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress> managedDelegate)
		{
			if (managedDelegate != null)
			{
				return MarshalDelegate.CreateMarshaler(managedDelegate, GuidGenerator.GetIID(typeof(AsyncActionWithProgressCompletedHandler<TProgress>)), true);
			}
			return null;
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x00011E54 File Offset: 0x00010054
		public static ObjectReferenceValue CreateMarshaler2(Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress> managedDelegate)
		{
			return MarshalDelegate.CreateMarshaler2(managedDelegate, GuidGenerator.GetIID(typeof(AsyncActionWithProgressCompletedHandler<TProgress>)), true);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x00011E6C File Offset: 0x0001006C
		public static IntPtr GetAbi(IObjectReference value)
		{
			return MarshalInterfaceHelper<Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress>>.GetAbi(value);
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00011E74 File Offset: 0x00010074
		public static Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress> FromAbi(IntPtr nativeDelegate)
		{
			return MarshalDelegate.FromAbi<Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress>>(nativeDelegate);
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00011E7C File Offset: 0x0001007C
		public static Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress> CreateRcw(IntPtr ptr)
		{
			return new Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress>(new AsyncActionWithProgressCompletedHandler<TProgress>.NativeDelegateWrapper(ComWrappersSupport.GetObjectReferenceForInterface<IDelegateVftbl>(ptr, GuidGenerator.GetIID(typeof(AsyncActionWithProgressCompletedHandler<TProgress>)))).Invoke);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00011EA4 File Offset: 0x000100A4
		public static IntPtr FromManaged(Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress> managedDelegate)
		{
			return AsyncActionWithProgressCompletedHandler<TProgress>.CreateMarshaler2(managedDelegate).Detach();
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00011EBF File Offset: 0x000100BF
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress>>.DisposeMarshaler(value);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00011EC7 File Offset: 0x000100C7
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress>>.DisposeAbi(abi);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00011ED0 File Offset: 0x000100D0
		private unsafe static int Do_Abi_Invoke(void* thisPtr, IntPtr asyncInfo, AsyncStatus asyncStatus)
		{
			try
			{
				ComWrappersSupport.FindObject<Windows.Foundation.AsyncActionWithProgressCompletedHandler<TProgress>>(new IntPtr(thisPtr)).Invoke(MarshalInterface<Windows.Foundation.IAsyncActionWithProgress<TProgress>>.FromAbi(asyncInfo), asyncStatus);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x04000161 RID: 353
		public static Guid PIID;

		// Token: 0x04000162 RID: 354
		private static readonly Type Abi_Invoke_Type;

		// Token: 0x04000163 RID: 355
		private static readonly IDelegateVftbl AbiToProjectionVftable;

		// Token: 0x04000164 RID: 356
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x020002F9 RID: 761
		[ObjectReferenceWrapper("_nativeDelegate")]
		private sealed class NativeDelegateWrapper : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x06001690 RID: 5776 RVA: 0x0003FF68 File Offset: 0x0003E168
			public NativeDelegateWrapper(ObjectReference<IDelegateVftbl> nativeDelegate)
			{
				this._nativeDelegate = nativeDelegate;
			}

			// Token: 0x17000440 RID: 1088
			// (get) Token: 0x06001691 RID: 5777 RVA: 0x0003FF77 File Offset: 0x0003E177
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._nativeDelegate;
				}
			}

			// Token: 0x17000441 RID: 1089
			// (get) Token: 0x06001692 RID: 5778 RVA: 0x0003FF7F File Offset: 0x0003E17F
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06001693 RID: 5779 RVA: 0x0003FF82 File Offset: 0x0003E182
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x17000442 RID: 1090
			// (get) Token: 0x06001694 RID: 5780 RVA: 0x0003FF9E File Offset: 0x0003E19E
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x06001695 RID: 5781 RVA: 0x0003FFB2 File Offset: 0x0003E1B2
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x17000443 RID: 1091
			// (get) Token: 0x06001696 RID: 5782 RVA: 0x0003FFCE File Offset: 0x0003E1CE
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x06001697 RID: 5783 RVA: 0x0003FFE4 File Offset: 0x0003E1E4
			public void Invoke(Windows.Foundation.IAsyncActionWithProgress<TProgress> asyncInfo, AsyncStatus asyncStatus)
			{
				IntPtr thisPtr = this._nativeDelegate.ThisPtr;
				Delegate delegateForFunctionPointer = Marshal.GetDelegateForFunctionPointer(this._nativeDelegate.Vftbl.Invoke, AsyncActionWithProgressCompletedHandler<TProgress>.Abi_Invoke_Type);
				ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
				object[] array = new object[]
				{
					thisPtr,
					default(object),
					asyncStatus
				};
				try
				{
					objectReferenceValue = MarshalInterface<Windows.Foundation.IAsyncActionWithProgress<TProgress>>.CreateMarshaler2(asyncInfo, default(Guid));
					array[1] = MarshalInspectable<object>.GetAbi(objectReferenceValue);
					delegateForFunctionPointer.DynamicInvokeAbi(array);
				}
				finally
				{
					MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
				}
			}

			// Token: 0x04000795 RID: 1941
			private readonly ObjectReference<IDelegateVftbl> _nativeDelegate;

			// Token: 0x04000796 RID: 1942
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x04000797 RID: 1943
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}
	}
}
