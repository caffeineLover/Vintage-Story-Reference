using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using ABI.System.Collections.Generic;
using ABI.Windows.Foundation;
using Windows.Foundation.Metadata;
using WinRT;

namespace Windows.Foundation
{
	// Token: 0x02000054 RID: 84
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[ProjectedRuntimeClass("_default")]
	[ObjectReferenceWrapper("_inner")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal sealed class WwwFormUrlDecoder : IReadOnlyList<IWwwFormUrlDecoderEntry>, IEnumerable<IWwwFormUrlDecoderEntry>, IEnumerable, IReadOnlyCollection<IWwwFormUrlDecoderEntry>, ICustomQueryInterface, IWinRTObject, IDynamicInterfaceCastable, IEquatable<WwwFormUrlDecoder>
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060003CF RID: 975 RVA: 0x00005E17 File Offset: 0x00004017
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

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x00005E38 File Offset: 0x00004038
		private IObjectReference _objRef_global__Windows_Foundation_IWwwFormUrlDecoderRuntimeClass
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00005E40 File Offset: 0x00004040
		private IObjectReference Make___objRef_global__System_Collections_Generic_IReadOnlyList_IWwwFormUrlDecoderEntry_()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.___objRef_global__System_Collections_Generic_IReadOnlyList_IWwwFormUrlDecoderEntry_, (IObjectReference)typeof(IObjectReference).GetMethod("As", Type.EmptyTypes).MakeGenericMethod(new Type[]
			{
				typeof(IReadOnlyList<IWwwFormUrlDecoderEntry>).GetHelperType().FindVftblType()
			}).Invoke(((IWinRTObject)this).NativeObject, null), null);
			return this.___objRef_global__System_Collections_Generic_IReadOnlyList_IWwwFormUrlDecoderEntry_;
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x00005EAE File Offset: 0x000040AE
		private IObjectReference _objRef_global__System_Collections_Generic_IReadOnlyList_IWwwFormUrlDecoderEntry_
		{
			get
			{
				return this.___objRef_global__System_Collections_Generic_IReadOnlyList_IWwwFormUrlDecoderEntry_ ?? this.Make___objRef_global__System_Collections_Generic_IReadOnlyList_IWwwFormUrlDecoderEntry_();
			}
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00005EC4 File Offset: 0x000040C4
		private IObjectReference Make___objRef_global__System_Collections_Generic_IEnumerable_IWwwFormUrlDecoderEntry_()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.___objRef_global__System_Collections_Generic_IEnumerable_IWwwFormUrlDecoderEntry_, (IObjectReference)typeof(IObjectReference).GetMethod("As", Type.EmptyTypes).MakeGenericMethod(new Type[]
			{
				typeof(IEnumerable<IWwwFormUrlDecoderEntry>).GetHelperType().FindVftblType()
			}).Invoke(((IWinRTObject)this).NativeObject, null), null);
			return this.___objRef_global__System_Collections_Generic_IEnumerable_IWwwFormUrlDecoderEntry_;
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x00005F32 File Offset: 0x00004132
		private IObjectReference _objRef_global__System_Collections_Generic_IEnumerable_IWwwFormUrlDecoderEntry_
		{
			get
			{
				return this.___objRef_global__System_Collections_Generic_IEnumerable_IWwwFormUrlDecoderEntry_ ?? this.Make___objRef_global__System_Collections_Generic_IEnumerable_IWwwFormUrlDecoderEntry_();
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x00005F46 File Offset: 0x00004146
		private IWwwFormUrlDecoderRuntimeClass _default
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00005F4C File Offset: 0x0000414C
		public WwwFormUrlDecoder(string query)
		{
			IntPtr intPtr = WwwFormUrlDecoder._IWwwFormUrlDecoderRuntimeClassFactory.Instance.CreateWwwFormUrlDecoder(query);
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

		// Token: 0x060003D7 RID: 983 RVA: 0x00005FA8 File Offset: 0x000041A8
		public static WwwFormUrlDecoder FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			return MarshalInspectable<WwwFormUrlDecoder>.FromAbi(thisPtr);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00005FBF File Offset: 0x000041BF
		internal WwwFormUrlDecoder(IObjectReference objRef)
		{
			this._inner = objRef.As(GuidGenerator.GetIID(typeof(IWwwFormUrlDecoderRuntimeClass).GetHelperType()));
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00005FE7 File Offset: 0x000041E7
		public static bool operator ==(WwwFormUrlDecoder x, WwwFormUrlDecoder y)
		{
			return ((x != null) ? x.ThisPtr : IntPtr.Zero) == ((y != null) ? y.ThisPtr : IntPtr.Zero);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000600E File Offset: 0x0000420E
		public static bool operator !=(WwwFormUrlDecoder x, WwwFormUrlDecoder y)
		{
			return !(x == y);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000601A File Offset: 0x0000421A
		public bool Equals(WwwFormUrlDecoder other)
		{
			return this == other;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00006024 File Offset: 0x00004224
		public override bool Equals(object obj)
		{
			WwwFormUrlDecoder wwwFormUrlDecoder = obj as WwwFormUrlDecoder;
			return wwwFormUrlDecoder != null && this == wwwFormUrlDecoder;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00006044 File Offset: 0x00004244
		public override int GetHashCode()
		{
			return this.ThisPtr.GetHashCode();
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0000605F File Offset: 0x0000425F
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060003DF RID: 991 RVA: 0x00006062 File Offset: 0x00004262
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000606A File Offset: 0x0000426A
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x00006086 File Offset: 0x00004286
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000609A File Offset: 0x0000429A
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x000060B6 File Offset: 0x000042B6
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000060CA File Offset: 0x000042CA
		public string GetFirstValueByName(string name)
		{
			return IWwwFormUrlDecoderRuntimeClassMethods.GetFirstValueByName(this._objRef_global__Windows_Foundation_IWwwFormUrlDecoderRuntimeClass, name);
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x000060D8 File Offset: 0x000042D8
		public int Count
		{
			get
			{
				return IReadOnlyListMethods<IWwwFormUrlDecoderEntry>.get_Count(this._objRef_global__System_Collections_Generic_IReadOnlyList_IWwwFormUrlDecoderEntry_);
			}
		}

		// Token: 0x170001DC RID: 476
		[IndexerName("ReadOnlyListItem")]
		public IWwwFormUrlDecoderEntry this[int index]
		{
			get
			{
				return IReadOnlyListMethods<IWwwFormUrlDecoderEntry>.Indexer_Get(this._objRef_global__System_Collections_Generic_IReadOnlyList_IWwwFormUrlDecoderEntry_, index);
			}
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000060F3 File Offset: 0x000042F3
		public IEnumerator<IWwwFormUrlDecoderEntry> GetEnumerator()
		{
			return IEnumerableMethods<IWwwFormUrlDecoderEntry>.GetEnumerator(this._objRef_global__System_Collections_Generic_IEnumerable_IWwwFormUrlDecoderEntry_);
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00006100 File Offset: 0x00004300
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00006108 File Offset: 0x00004308
		private bool IsOverridableInterface(Guid iid)
		{
			return false;
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000610C File Offset: 0x0000430C
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

		// Token: 0x060003EB RID: 1003 RVA: 0x0000615A File Offset: 0x0000435A
		IWwwFormUrlDecoderEntry IReadOnlyList<IWwwFormUrlDecoderEntry>.get_Item(int index)
		{
			return this[index];
		}

		// Token: 0x040000C7 RID: 199
		private IObjectReference _inner;

		// Token: 0x040000C8 RID: 200
		private volatile IObjectReference ___objRef_global__System_Collections_Generic_IReadOnlyList_IWwwFormUrlDecoderEntry_;

		// Token: 0x040000C9 RID: 201
		private volatile IObjectReference ___objRef_global__System_Collections_Generic_IEnumerable_IWwwFormUrlDecoderEntry_;

		// Token: 0x040000CA RID: 202
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x040000CB RID: 203
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;

		// Token: 0x020002BF RID: 703
		internal sealed class _IWwwFormUrlDecoderRuntimeClassFactory : IWinRTObject, IDynamicInterfaceCastable
		{
			// Token: 0x1700040A RID: 1034
			// (get) Token: 0x0600156E RID: 5486 RVA: 0x0003F762 File Offset: 0x0003D962
			private IntPtr ThisPtr
			{
				get
				{
					return this._obj.ThisPtr;
				}
			}

			// Token: 0x0600156F RID: 5487 RVA: 0x0003F76F File Offset: 0x0003D96F
			public _IWwwFormUrlDecoderRuntimeClassFactory()
			{
				this._obj = ActivationFactory<WwwFormUrlDecoder>.As(GuidGenerator.GetIID(typeof(IWwwFormUrlDecoderRuntimeClassFactory).GetHelperType()));
			}

			// Token: 0x1700040B RID: 1035
			// (get) Token: 0x06001570 RID: 5488 RVA: 0x0003F796 File Offset: 0x0003D996
			internal static WwwFormUrlDecoder._IWwwFormUrlDecoderRuntimeClassFactory Instance
			{
				get
				{
					return WwwFormUrlDecoder._IWwwFormUrlDecoderRuntimeClassFactory._instance;
				}
			}

			// Token: 0x1700040C RID: 1036
			// (get) Token: 0x06001571 RID: 5489 RVA: 0x0003F79D File Offset: 0x0003D99D
			IObjectReference IWinRTObject.NativeObject
			{
				get
				{
					return this._obj;
				}
			}

			// Token: 0x1700040D RID: 1037
			// (get) Token: 0x06001572 RID: 5490 RVA: 0x0003F7A5 File Offset: 0x0003D9A5
			bool IWinRTObject.HasUnwrappableNativeObject
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06001573 RID: 5491 RVA: 0x0003F7A8 File Offset: 0x0003D9A8
			private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
				return this._queryInterfaceCache;
			}

			// Token: 0x1700040E RID: 1038
			// (get) Token: 0x06001574 RID: 5492 RVA: 0x0003F7C4 File Offset: 0x0003D9C4
			ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
			{
				get
				{
					return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
				}
			}

			// Token: 0x06001575 RID: 5493 RVA: 0x0003F7D8 File Offset: 0x0003D9D8
			private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
			{
				Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
				return this._additionalTypeData;
			}

			// Token: 0x1700040F RID: 1039
			// (get) Token: 0x06001576 RID: 5494 RVA: 0x0003F7F4 File Offset: 0x0003D9F4
			ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
			{
				get
				{
					return this._additionalTypeData ?? this.MakeAdditionalTypeData();
				}
			}

			// Token: 0x06001577 RID: 5495 RVA: 0x0003F808 File Offset: 0x0003DA08
			public unsafe IntPtr CreateWwwFormUrlDecoder(string query)
			{
				IntPtr result = default(IntPtr);
				MarshalString.Pinnable pinnable = new MarshalString.Pinnable(query);
				fixed (char* pinnableReference = pinnable.GetPinnableReference())
				{
					char* ptr = pinnableReference;
					method system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)this.ThisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), this.ThisPtr, MarshalString.GetAbi(ref pinnable), ref result, system.Int32_u0020(System.IntPtr,System.IntPtr,System.IntPtr&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
					return result;
				}
			}

			// Token: 0x04000752 RID: 1874
			private IObjectReference _obj;

			// Token: 0x04000753 RID: 1875
			private static WwwFormUrlDecoder._IWwwFormUrlDecoderRuntimeClassFactory _instance = new WwwFormUrlDecoder._IWwwFormUrlDecoderRuntimeClassFactory();

			// Token: 0x04000754 RID: 1876
			private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

			// Token: 0x04000755 RID: 1877
			private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
		}

		// Token: 0x020002C0 RID: 704
		private struct InterfaceTag<I>
		{
		}
	}
}
