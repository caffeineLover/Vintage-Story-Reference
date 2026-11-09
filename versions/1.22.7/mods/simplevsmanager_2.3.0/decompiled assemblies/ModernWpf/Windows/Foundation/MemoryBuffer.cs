using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using ABI.Windows.Foundation;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000050 RID: 80
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[ProjectedRuntimeClass("_default")]
	[ObjectReferenceWrapper("_inner")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal sealed class MemoryBuffer : IMemoryBuffer, IDisposable, ICustomQueryInterface, IWinRTObject, IDynamicInterfaceCastable, IEquatable<MemoryBuffer>
	{
		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00005933 File Offset: 0x00003B33
		private IntPtr ThisPtr
		{
			get
			{
				if (this._inner != null)
				{
					return this._inner.ThisPtr;
				}
				return ((IWinRTObject)this).NativeObject.ThisPtr;
			}
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00005954 File Offset: 0x00003B54
		private IDisposable Make__lazy_global__System_IDisposable()
		{
			Interlocked.CompareExchange<IDisposable>(ref this._lazy_global__System_IDisposable, (IDisposable)new SingleInterfaceOptimizedObject(typeof(IDisposable), this._inner ?? ((IWinRTObject)this).NativeObject), null);
			return this._lazy_global__System_IDisposable;
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0000598F File Offset: 0x00003B8F
		private IObjectReference _objRef_global__Windows_Foundation_IMemoryBuffer
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00005997 File Offset: 0x00003B97
		private IMemoryBuffer _default
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000599C File Offset: 0x00003B9C
		public MemoryBuffer(uint capacity)
		{
			IntPtr intPtr = MemoryBuffer._IMemoryBufferFactory.Instance.Create(capacity);
			try
			{
				this._inner = ComWrappersSupport.GetObjectReferenceForInterface(intPtr);
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			ComWrappersSupport.RegisterObjectForInterface(this, this.ThisPtr);
			ComWrappersHelper.Init(this._inner, false);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x000059F8 File Offset: 0x00003BF8
		public static MemoryBuffer FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			return MarshalInspectable<MemoryBuffer>.FromAbi(thisPtr);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00005A0F File Offset: 0x00003C0F
		internal MemoryBuffer(IObjectReference objRef)
		{
			this._inner = objRef.As(GuidGenerator.GetIID(typeof(IMemoryBuffer).GetHelperType()));
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00005A37 File Offset: 0x00003C37
		public static bool operator ==(MemoryBuffer x, MemoryBuffer y)
		{
			return ((x != null) ? x.ThisPtr : IntPtr.Zero) == ((y != null) ? y.ThisPtr : IntPtr.Zero);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00005A5E File Offset: 0x00003C5E
		public static bool operator !=(MemoryBuffer x, MemoryBuffer y)
		{
			return !(x == y);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00005A6A File Offset: 0x00003C6A
		public bool Equals(MemoryBuffer other)
		{
			return this == other;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00005A74 File Offset: 0x00003C74
		public override bool Equals(object obj)
		{
			MemoryBuffer memoryBuffer = obj as MemoryBuffer;
			return memoryBuffer != null && this == memoryBuffer;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00005A94 File Offset: 0x00003C94
		public override int GetHashCode()
		{
			return this.ThisPtr.GetHashCode();
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000394 RID: 916 RVA: 0x00005AAF File Offset: 0x00003CAF
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000395 RID: 917 RVA: 0x00005AB2 File Offset: 0x00003CB2
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00005ABA File Offset: 0x00003CBA
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000397 RID: 919 RVA: 0x00005AD6 File Offset: 0x00003CD6
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00005AEA File Offset: 0x00003CEA
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00005B06 File Offset: 0x00003D06
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00005B1A File Offset: 0x00003D1A
		public IMemoryBufferReference CreateReference()
		{
			return IMemoryBufferMethods.CreateReference(this._objRef_global__Windows_Foundation_IMemoryBuffer);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00005B27 File Offset: 0x00003D27
		IMemoryBufferReference IMemoryBuffer.CreateReference()
		{
			return this.CreateReference();
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00005B2F File Offset: 0x00003D2F
		private IDisposable AsInternal(MemoryBuffer.InterfaceTag<IDisposable> _)
		{
			return this._lazy_global__System_IDisposable ?? this.Make__lazy_global__System_IDisposable();
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00005B44 File Offset: 0x00003D44
		public void Dispose()
		{
			this.AsInternal(default(MemoryBuffer.InterfaceTag<IDisposable>)).Dispose();
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00005B65 File Offset: 0x00003D65
		private bool IsOverridableInterface(Guid iid)
		{
			return false;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00005B68 File Offset: 0x00003D68
		CustomQueryInterfaceResult ICustomQueryInterface.GetInterface(ref Guid iid, out IntPtr ppv)
		{
			ppv = IntPtr.Zero;
			if (this.IsOverridableInterface(iid) || InterfaceIIDs.IInspectable_IID == iid)
			{
				return 1;
			}
			if (((IWinRTObject)this).NativeObject.TryAs(iid, out ppv) >= 0)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x040000C0 RID: 192
		private IObjectReference _inner;

		// Token: 0x040000C1 RID: 193
		private volatile IDisposable _lazy_global__System_IDisposable;

		// Token: 0x040000C2 RID: 194
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x040000C3 RID: 195
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;

		// Token: 0x020002BD RID: 701
		internal sealed class _IMemoryBufferFactory : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x17000404 RID: 1028
			// (get) Token: 0x06001563 RID: 5475 RVA: 0x0003F66C File Offset: 0x0003D86C
			private IntPtr ThisPtr
			{
				get
				{
					return this._obj.ThisPtr;
				}
			}

			// Token: 0x06001564 RID: 5476 RVA: 0x0003F679 File Offset: 0x0003D879
			public _IMemoryBufferFactory()
			{
				this._obj = ActivationFactory<MemoryBuffer>.As(GuidGenerator.GetIID(typeof(IMemoryBufferFactory).GetHelperType()));
			}

			// Token: 0x17000405 RID: 1029
			// (get) Token: 0x06001565 RID: 5477 RVA: 0x0003F6A0 File Offset: 0x0003D8A0
			internal static MemoryBuffer._IMemoryBufferFactory Instance
			{
				get
				{
					return MemoryBuffer._IMemoryBufferFactory._instance;
				}
			}

			// Token: 0x17000406 RID: 1030
			// (get) Token: 0x06001566 RID: 5478 RVA: 0x0003F6A7 File Offset: 0x0003D8A7
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._obj;
				}
			}

			// Token: 0x17000407 RID: 1031
			// (get) Token: 0x06001567 RID: 5479 RVA: 0x0003F6AF File Offset: 0x0003D8AF
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06001568 RID: 5480 RVA: 0x0003F6B2 File Offset: 0x0003D8B2
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x17000408 RID: 1032
			// (get) Token: 0x06001569 RID: 5481 RVA: 0x0003F6CE File Offset: 0x0003D8CE
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x0600156A RID: 5482 RVA: 0x0003F6E2 File Offset: 0x0003D8E2
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x17000409 RID: 1033
			// (get) Token: 0x0600156B RID: 5483 RVA: 0x0003F6FE File Offset: 0x0003D8FE
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x0600156C RID: 5484 RVA: 0x0003F714 File Offset: 0x0003D914
			public unsafe IntPtr Create(uint capacity)
			{
				IntPtr result = default(IntPtr);
				method system.Int32_u0020(System.IntPtr,System.UInt32,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)this.ThisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), this.ThisPtr, capacity, ref result, system.Int32_u0020(System.IntPtr,System.UInt32,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
				return result;
			}

			// Token: 0x0400074E RID: 1870
			private IObjectReference _obj;

			// Token: 0x0400074F RID: 1871
			private static MemoryBuffer._IMemoryBufferFactory _instance = new MemoryBuffer._IMemoryBufferFactory();

			// Token: 0x04000750 RID: 1872
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x04000751 RID: 1873
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}

		// Token: 0x020002BE RID: 702
		private struct InterfaceTag<I>
		{
		}
	}
}
