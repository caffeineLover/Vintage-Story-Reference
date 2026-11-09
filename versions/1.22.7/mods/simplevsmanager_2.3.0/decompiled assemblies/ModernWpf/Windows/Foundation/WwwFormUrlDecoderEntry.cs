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
	// Token: 0x02000055 RID: 85
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[ProjectedRuntimeClass("_default")]
	[ObjectReferenceWrapper("_inner")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(UniversalApiContract), 65536U)]
	internal sealed class WwwFormUrlDecoderEntry : IWwwFormUrlDecoderEntry, ICustomQueryInterface, IWinRTObject, IDynamicInterfaceCastable, IEquatable<WwwFormUrlDecoderEntry>
	{
		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x00006163 File Offset: 0x00004363
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

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x00006184 File Offset: 0x00004384
		private IObjectReference _objRef_global__Windows_Foundation_IWwwFormUrlDecoderEntry
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x0000618C File Offset: 0x0000438C
		private IWwwFormUrlDecoderEntry _default
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0000618F File Offset: 0x0000438F
		public static WwwFormUrlDecoderEntry FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			return MarshalInspectable<WwwFormUrlDecoderEntry>.FromAbi(thisPtr);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x000061A6 File Offset: 0x000043A6
		internal WwwFormUrlDecoderEntry(IObjectReference objRef)
		{
			this._inner = objRef.As(GuidGenerator.GetIID(typeof(IWwwFormUrlDecoderEntry).GetHelperType()));
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x000061CE File Offset: 0x000043CE
		public static bool operator ==(WwwFormUrlDecoderEntry x, WwwFormUrlDecoderEntry y)
		{
			return ((x != null) ? x.ThisPtr : IntPtr.Zero) == ((y != null) ? y.ThisPtr : IntPtr.Zero);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x000061F5 File Offset: 0x000043F5
		public static bool operator !=(WwwFormUrlDecoderEntry x, WwwFormUrlDecoderEntry y)
		{
			return !(x == y);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00006201 File Offset: 0x00004401
		public bool Equals(WwwFormUrlDecoderEntry other)
		{
			return this == other;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0000620C File Offset: 0x0000440C
		public override bool Equals(object obj)
		{
			WwwFormUrlDecoderEntry wwwFormUrlDecoderEntry = obj as WwwFormUrlDecoderEntry;
			return wwwFormUrlDecoderEntry != null && this == wwwFormUrlDecoderEntry;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0000622C File Offset: 0x0000442C
		public override int GetHashCode()
		{
			return this.ThisPtr.GetHashCode();
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00006247 File Offset: 0x00004447
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x0000624A File Offset: 0x0000444A
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00006252 File Offset: 0x00004452
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x0000626E File Offset: 0x0000446E
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00006282 File Offset: 0x00004482
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x0000629E File Offset: 0x0000449E
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x000062B2 File Offset: 0x000044B2
		string IWwwFormUrlDecoderEntry.Name
		{
			get
			{
				return IWwwFormUrlDecoderEntryMethods.get_Name(this._objRef_global__Windows_Foundation_IWwwFormUrlDecoderEntry);
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x000062BF File Offset: 0x000044BF
		string IWwwFormUrlDecoderEntry.Value
		{
			get
			{
				return IWwwFormUrlDecoderEntryMethods.get_Value(this._objRef_global__Windows_Foundation_IWwwFormUrlDecoderEntry);
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x000062CC File Offset: 0x000044CC
		public string Name
		{
			get
			{
				return IWwwFormUrlDecoderEntryMethods.get_Name(this._objRef_global__Windows_Foundation_IWwwFormUrlDecoderEntry);
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x000062D9 File Offset: 0x000044D9
		public string Value
		{
			get
			{
				return IWwwFormUrlDecoderEntryMethods.get_Value(this._objRef_global__Windows_Foundation_IWwwFormUrlDecoderEntry);
			}
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x000062E6 File Offset: 0x000044E6
		private bool IsOverridableInterface(Guid iid)
		{
			return false;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x000062EC File Offset: 0x000044EC
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

		// Token: 0x040000CC RID: 204
		private IObjectReference _inner;

		// Token: 0x040000CD RID: 205
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x040000CE RID: 206
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;

		// Token: 0x020002C1 RID: 705
		private struct InterfaceTag<I>
		{
		}
	}
}
