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
	// Token: 0x02000037 RID: 55
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[ProjectedRuntimeClass("_default")]
	[ObjectReferenceWrapper("_inner")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class Deferral : IDisposable, ICustomQueryInterface, IWinRTObject, IDynamicInterfaceCastable, IEquatable<Deferral>
	{
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000303 RID: 771 RVA: 0x0000562C File Offset: 0x0000382C
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

		// Token: 0x06000304 RID: 772 RVA: 0x0000564D File Offset: 0x0000384D
		private IDisposable Make__lazy_global__System_IDisposable()
		{
			Interlocked.CompareExchange<IDisposable>(ref this._lazy_global__System_IDisposable, (IDisposable)new SingleInterfaceOptimizedObject(typeof(IDisposable), this._inner ?? ((IWinRTObject)this).NativeObject), null);
			return this._lazy_global__System_IDisposable;
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00005688 File Offset: 0x00003888
		private IObjectReference _objRef_global__Windows_Foundation_IDeferral
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000306 RID: 774 RVA: 0x00005690 File Offset: 0x00003890
		private IDeferral _default
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00005694 File Offset: 0x00003894
		public Deferral(DeferralCompletedHandler handler)
		{
			IntPtr intPtr = Deferral._IDeferralFactory.Instance.Create(handler);
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

		// Token: 0x06000308 RID: 776 RVA: 0x000056F0 File Offset: 0x000038F0
		public static Deferral FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			return MarshalInspectable<Deferral>.FromAbi(thisPtr);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00005707 File Offset: 0x00003907
		internal Deferral(IObjectReference objRef)
		{
			this._inner = objRef.As(GuidGenerator.GetIID(typeof(IDeferral).GetHelperType()));
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000572F File Offset: 0x0000392F
		public static bool operator ==(Deferral x, Deferral y)
		{
			return ((x != null) ? x.ThisPtr : IntPtr.Zero) == ((y != null) ? y.ThisPtr : IntPtr.Zero);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00005756 File Offset: 0x00003956
		public static bool operator !=(Deferral x, Deferral y)
		{
			return !(x == y);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00005762 File Offset: 0x00003962
		public bool Equals(Deferral other)
		{
			return this == other;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000576C File Offset: 0x0000396C
		public override bool Equals(object obj)
		{
			Deferral deferral = obj as Deferral;
			return deferral != null && this == deferral;
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000578C File Offset: 0x0000398C
		public override int GetHashCode()
		{
			return this.ThisPtr.GetHashCode();
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x0600030F RID: 783 RVA: 0x000057A7 File Offset: 0x000039A7
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000310 RID: 784 RVA: 0x000057AA File Offset: 0x000039AA
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x000057B2 File Offset: 0x000039B2
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000312 RID: 786 RVA: 0x000057CE File Offset: 0x000039CE
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x000057E2 File Offset: 0x000039E2
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000314 RID: 788 RVA: 0x000057FE File Offset: 0x000039FE
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x06000315 RID: 789 RVA: 0x00005812 File Offset: 0x00003A12
		public void Complete()
		{
			IDeferralMethods.Complete(this._objRef_global__Windows_Foundation_IDeferral);
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000581F File Offset: 0x00003A1F
		private IDisposable AsInternal(Deferral.InterfaceTag<IDisposable> _)
		{
			return this._lazy_global__System_IDisposable ?? this.Make__lazy_global__System_IDisposable();
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00005834 File Offset: 0x00003A34
		public void Dispose()
		{
			this.AsInternal(default(Deferral.InterfaceTag<IDisposable>)).Dispose();
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00005855 File Offset: 0x00003A55
		private bool IsOverridableInterface(Guid iid)
		{
			return false;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00005858 File Offset: 0x00003A58
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

		// Token: 0x040000B9 RID: 185
		private IObjectReference _inner;

		// Token: 0x040000BA RID: 186
		private volatile IDisposable _lazy_global__System_IDisposable;

		// Token: 0x040000BB RID: 187
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x040000BC RID: 188
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;

		// Token: 0x020002BB RID: 699
		internal sealed class _IDeferralFactory : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x170003FE RID: 1022
			// (get) Token: 0x06001558 RID: 5464 RVA: 0x0003F545 File Offset: 0x0003D745
			private IntPtr ThisPtr
			{
				get
				{
					return this._obj.ThisPtr;
				}
			}

			// Token: 0x06001559 RID: 5465 RVA: 0x0003F552 File Offset: 0x0003D752
			public _IDeferralFactory()
			{
				this._obj = ActivationFactory<Deferral>.As(GuidGenerator.GetIID(typeof(IDeferralFactory).GetHelperType()));
			}

			// Token: 0x170003FF RID: 1023
			// (get) Token: 0x0600155A RID: 5466 RVA: 0x0003F579 File Offset: 0x0003D779
			internal static Deferral._IDeferralFactory Instance
			{
				get
				{
					return Deferral._IDeferralFactory._instance;
				}
			}

			// Token: 0x17000400 RID: 1024
			// (get) Token: 0x0600155B RID: 5467 RVA: 0x0003F580 File Offset: 0x0003D780
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._obj;
				}
			}

			// Token: 0x17000401 RID: 1025
			// (get) Token: 0x0600155C RID: 5468 RVA: 0x0003F588 File Offset: 0x0003D788
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return false;
				}
			}

			// Token: 0x0600155D RID: 5469 RVA: 0x0003F58B File Offset: 0x0003D78B
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x17000402 RID: 1026
			// (get) Token: 0x0600155E RID: 5470 RVA: 0x0003F5A7 File Offset: 0x0003D7A7
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x0600155F RID: 5471 RVA: 0x0003F5BB File Offset: 0x0003D7BB
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x17000403 RID: 1027
			// (get) Token: 0x06001560 RID: 5472 RVA: 0x0003F5D7 File Offset: 0x0003D7D7
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x06001561 RID: 5473 RVA: 0x0003F5EC File Offset: 0x0003D7EC
			public unsafe IntPtr Create(DeferralCompletedHandler handler)
			{
				ObjectReferenceValue objectReferenceValue = default(ObjectReferenceValue);
				IntPtr intPtr = default(IntPtr);
				IntPtr result;
				try
				{
					objectReferenceValue = ABI.Windows.Foundation.DeferralCompletedHandler.CreateMarshaler2(handler);
					method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)this.ThisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), this.ThisPtr, MarshalInspectable<object>.GetAbi(objectReferenceValue), ref intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
					result = intPtr;
				}
				finally
				{
					MarshalInspectable<object>.DisposeMarshaler(objectReferenceValue);
				}
				return result;
			}

			// Token: 0x0400074A RID: 1866
			private IObjectReference _obj;

			// Token: 0x0400074B RID: 1867
			private static Deferral._IDeferralFactory _instance = new Deferral._IDeferralFactory();

			// Token: 0x0400074C RID: 1868
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x0400074D RID: 1869
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}

		// Token: 0x020002BC RID: 700
		private struct InterfaceTag<I>
		{
		}
	}
}
