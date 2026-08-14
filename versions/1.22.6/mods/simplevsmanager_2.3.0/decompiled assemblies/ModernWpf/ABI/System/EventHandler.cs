using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using WinRT;
using WinRT.Interop;

namespace ABI.System
{
	// Token: 0x02000118 RID: 280
	[Guid("c50898f6-c536-5f47-8583-8b2c2438a13b")]
	internal static class EventHandler
	{
		// Token: 0x060009BD RID: 2493 RVA: 0x0001B6FC File Offset: 0x000198FC
		static EventHandler()
		{
			EventHandler.AbiToProjectionVftable = new IDelegateVftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				Invoke = (IntPtr)ldftn(Do_Abi_Invoke)
			};
			IntPtr intPtr = ComWrappersSupport.AllocateVtableMemory(typeof(EventHandler), Marshal.SizeOf<IDelegateVftbl>());
			Marshal.StructureToPtr<IDelegateVftbl>(EventHandler.AbiToProjectionVftable, intPtr, false);
			EventHandler.AbiToProjectionVftablePtr = intPtr;
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0001B791 File Offset: 0x00019991
		public static IObjectReference CreateMarshaler(EventHandler managedDelegate)
		{
			if (managedDelegate != null)
			{
				return MarshalDelegate.CreateMarshaler(managedDelegate, EventHandler.IID, true);
			}
			return null;
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0001B7A4 File Offset: 0x000199A4
		public static ObjectReferenceValue CreateMarshaler2(EventHandler managedDelegate)
		{
			return MarshalDelegate.CreateMarshaler2(managedDelegate, EventHandler.IID, true);
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0001B7B2 File Offset: 0x000199B2
		public static IntPtr GetAbi(IObjectReference value)
		{
			if (value != null)
			{
				return MarshalInterfaceHelper<EventHandler<object>>.GetAbi(value);
			}
			return IntPtr.Zero;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0001B7C3 File Offset: 0x000199C3
		public static EventHandler FromAbi(IntPtr nativeDelegate)
		{
			return MarshalDelegate.FromAbi<EventHandler>(nativeDelegate);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0001B7CB File Offset: 0x000199CB
		public static EventHandler CreateRcw(IntPtr ptr)
		{
			return new EventHandler(new EventHandler.NativeDelegateWrapper(ComWrappersSupport.GetObjectReferenceForInterface<IDelegateVftbl>(ptr, EventHandler.IID)).Invoke);
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0001B7E8 File Offset: 0x000199E8
		public static IntPtr FromManaged(EventHandler managedDelegate)
		{
			return EventHandler.CreateMarshaler2(managedDelegate).Detach();
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0001B803 File Offset: 0x00019A03
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<EventHandler<object>>.DisposeMarshaler(value);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0001B80B File Offset: 0x00019A0B
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<EventHandler<object>>.DisposeAbi(abi);
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0001B814 File Offset: 0x00019A14
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private static int Do_Abi_Invoke(IntPtr thisPtr, IntPtr sender, IntPtr args)
		{
			try
			{
				ComWrappersSupport.FindObject<EventHandler>(thisPtr).Invoke(MarshalInspectable<object>.FromAbi(sender), (MarshalInspectable<object>.FromAbi(args) as EventArgs) ?? EventArgs.Empty);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x040001BE RID: 446
		private static readonly IDelegateVftbl AbiToProjectionVftable;

		// Token: 0x040001BF RID: 447
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x040001C0 RID: 448
		private static readonly Guid IID = new Guid(3305674998U, 50486, 24391, 133, 131, 139, 44, 36, 56, 161, 59);

		// Token: 0x02000331 RID: 817
		[ObjectReferenceWrapper("_nativeDelegate")]
		private sealed class NativeDelegateWrapper : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x060017BC RID: 6076 RVA: 0x000425FC File Offset: 0x000407FC
			public NativeDelegateWrapper(ObjectReference<IDelegateVftbl> nativeDelegate)
			{
				this._nativeDelegate = nativeDelegate;
			}

			// Token: 0x17000483 RID: 1155
			// (get) Token: 0x060017BD RID: 6077 RVA: 0x0004260B File Offset: 0x0004080B
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._nativeDelegate;
				}
			}

			// Token: 0x17000484 RID: 1156
			// (get) Token: 0x060017BE RID: 6078 RVA: 0x00042613 File Offset: 0x00040813
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return true;
				}
			}

			// Token: 0x060017BF RID: 6079 RVA: 0x00042616 File Offset: 0x00040816
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x17000485 RID: 1157
			// (get) Token: 0x060017C0 RID: 6080 RVA: 0x00042632 File Offset: 0x00040832
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x060017C1 RID: 6081 RVA: 0x00042646 File Offset: 0x00040846
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x17000486 RID: 1158
			// (get) Token: 0x060017C2 RID: 6082 RVA: 0x00042662 File Offset: 0x00040862
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x060017C3 RID: 6083 RVA: 0x00042678 File Offset: 0x00040878
			public unsafe void Invoke(object sender, EventArgs args)
			{
				IntPtr thisPtr = this._nativeDelegate.ThisPtr;
				method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr) = (void*)this._nativeDelegate.Vftbl.Invoke;
				ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
				ObjectReferenceValue objectReferenceValue2 = default(ObjectReferenceValue);
				try
				{
					objectReferenceValue = MarshalInspectable<object>.CreateMarshaler2(sender, true);
					objectReferenceValue2 = MarshalInspectable<EventArgs>.CreateMarshaler2(args, true);
					method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr)2 = system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr);
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr), thisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), MarshalInspectable<EventArgs>.GetAbi(objectReferenceValue2), system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr)2));
				}
				finally
				{
					MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
					MarshalInspectable<EventArgs>.DisposeMarshaler(objectReferenceValue2);
				}
			}

			// Token: 0x04000823 RID: 2083
			private readonly ObjectReference<IDelegateVftbl> _nativeDelegate;

			// Token: 0x04000824 RID: 2084
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x04000825 RID: 2085
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}
	}
}
