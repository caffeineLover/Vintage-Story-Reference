using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using WinRT;
using WinRT.Interop;

namespace ABI.System.ComponentModel
{
	// Token: 0x0200014A RID: 330
	[EditorBrowsable(1)]
	[Guid("E3DE52F6-1E32-5DA6-BB2D-B5B6096C962D")]
	internal static class PropertyChangedEventHandler
	{
		// Token: 0x06000A8F RID: 2703 RVA: 0x0001D798 File Offset: 0x0001B998
		static PropertyChangedEventHandler()
		{
			PropertyChangedEventHandler.AbiToProjectionVftable = new IDelegateVftbl
			{
				IUnknownVftbl = IUnknownVftbl.AbiToProjectionVftbl,
				Invoke = (IntPtr)ldftn(Do_Abi_Invoke)
			};
			IntPtr intPtr = ComWrappersSupport.AllocateVtableMemory(typeof(PropertyChangedEventHandler), Marshal.SizeOf<IDelegateVftbl>());
			Marshal.StructureToPtr<IDelegateVftbl>(PropertyChangedEventHandler.AbiToProjectionVftable, intPtr, false);
			PropertyChangedEventHandler.AbiToProjectionVftablePtr = intPtr;
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x0001D82D File Offset: 0x0001BA2D
		public static Delegate AbiInvokeDelegate { get; }

		// Token: 0x06000A91 RID: 2705 RVA: 0x0001D834 File Offset: 0x0001BA34
		public static IObjectReference CreateMarshaler(PropertyChangedEventHandler managedDelegate)
		{
			if (managedDelegate != null)
			{
				return MarshalDelegate.CreateMarshaler(managedDelegate, PropertyChangedEventHandler.IID, true);
			}
			return null;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0001D847 File Offset: 0x0001BA47
		public static ObjectReferenceValue CreateMarshaler2(PropertyChangedEventHandler managedDelegate)
		{
			return MarshalDelegate.CreateMarshaler2(managedDelegate, PropertyChangedEventHandler.IID, true);
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0001D855 File Offset: 0x0001BA55
		public static IntPtr GetAbi(IObjectReference value)
		{
			return MarshalInterfaceHelper<PropertyChangedEventHandler>.GetAbi(value);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0001D85D File Offset: 0x0001BA5D
		public static PropertyChangedEventHandler FromAbi(IntPtr nativeDelegate)
		{
			return MarshalDelegate.FromAbi<PropertyChangedEventHandler>(nativeDelegate);
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0001D865 File Offset: 0x0001BA65
		public static PropertyChangedEventHandler CreateRcw(IntPtr ptr)
		{
			return new PropertyChangedEventHandler(new PropertyChangedEventHandler.NativeDelegateWrapper(ComWrappersSupport.GetObjectReferenceForInterface<IDelegateVftbl>(ptr, PropertyChangedEventHandler.IID)).Invoke);
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0001D884 File Offset: 0x0001BA84
		public static IntPtr FromManaged(PropertyChangedEventHandler managedDelegate)
		{
			return PropertyChangedEventHandler.CreateMarshaler2(managedDelegate).Detach();
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0001D89F File Offset: 0x0001BA9F
		public static void DisposeMarshaler(IObjectReference value)
		{
			MarshalInterfaceHelper<PropertyChangedEventHandler>.DisposeMarshaler(value);
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0001D8A7 File Offset: 0x0001BAA7
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInterfaceHelper<PropertyChangedEventHandler>.DisposeAbi(abi);
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0001D8B0 File Offset: 0x0001BAB0
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private static int Do_Abi_Invoke(IntPtr thisPtr, IntPtr sender, IntPtr e)
		{
			try
			{
				ComWrappersSupport.FindObject<PropertyChangedEventHandler>(thisPtr).Invoke(MarshalInspectable<object>.FromAbi(sender), PropertyChangedEventArgs.FromAbi(e));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x040001E5 RID: 485
		private static readonly IDelegateVftbl AbiToProjectionVftable;

		// Token: 0x040001E6 RID: 486
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x040001E8 RID: 488
		private static readonly Guid IID = new Guid(3822998262U, 7730, 23974, 187, 45, 181, 182, 9, 108, 150, 45);

		// Token: 0x02000362 RID: 866
		[ObjectReferenceWrapper("_nativeDelegate")]
		private sealed class NativeDelegateWrapper : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x06001870 RID: 6256 RVA: 0x000440E2 File Offset: 0x000422E2
			public NativeDelegateWrapper(ObjectReference<IDelegateVftbl> nativeDelegate)
			{
				this._nativeDelegate = nativeDelegate;
			}

			// Token: 0x170004A4 RID: 1188
			// (get) Token: 0x06001871 RID: 6257 RVA: 0x000440F1 File Offset: 0x000422F1
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._nativeDelegate;
				}
			}

			// Token: 0x170004A5 RID: 1189
			// (get) Token: 0x06001872 RID: 6258 RVA: 0x000440F9 File Offset: 0x000422F9
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06001873 RID: 6259 RVA: 0x000440FC File Offset: 0x000422FC
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x170004A6 RID: 1190
			// (get) Token: 0x06001874 RID: 6260 RVA: 0x00044118 File Offset: 0x00042318
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x06001875 RID: 6261 RVA: 0x0004412C File Offset: 0x0004232C
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x170004A7 RID: 1191
			// (get) Token: 0x06001876 RID: 6262 RVA: 0x00044148 File Offset: 0x00042348
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x06001877 RID: 6263 RVA: 0x0004415C File Offset: 0x0004235C
			public unsafe void Invoke(object sender, PropertyChangedEventArgs e)
			{
				IntPtr thisPtr = this._nativeDelegate.ThisPtr;
				method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr) = (void*)this._nativeDelegate.Vftbl.Invoke;
				ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
				ObjectReferenceValue objectReferenceValue2 = default(ObjectReferenceValue);
				try
				{
					objectReferenceValue = MarshalInspectable<object>.CreateMarshaler2(sender, true);
					objectReferenceValue2 = PropertyChangedEventArgs.CreateMarshaler2(e);
					method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr)2 = system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr);
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr), thisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), MarshalInspectable<object>.GetAbi(objectReferenceValue2), system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr)2));
				}
				finally
				{
					MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
					MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue2);
				}
			}

			// Token: 0x040008A6 RID: 2214
			private readonly ObjectReference<IDelegateVftbl> _nativeDelegate;

			// Token: 0x040008A7 RID: 2215
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x040008A8 RID: 2216
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}
	}
}
