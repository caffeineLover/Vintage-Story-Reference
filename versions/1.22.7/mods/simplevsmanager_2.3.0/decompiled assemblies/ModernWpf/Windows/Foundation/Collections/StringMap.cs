using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using ABI.System.Collections.Generic;
using ABI.Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using WinRT;
using WinRT.Interop;

namespace Windows.Foundation.Collections
{
	// Token: 0x02000092 RID: 146
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[ProjectedRuntimeClass("_default")]
	[ObjectReferenceWrapper("_inner")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class StringMap : IDictionary<string, string>, ICollection<KeyValuePair<string, string>>, IEnumerable<KeyValuePair<string, string>>, IEnumerable, IObservableMap<string, string>, ICustomQueryInterface, IWinRTObject, IDynamicInterfaceCastable, IEquatable<StringMap>
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00006BEA File Offset: 0x00004DEA
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

		// Token: 0x060004AF RID: 1199 RVA: 0x00006C0C File Offset: 0x00004E0C
		private IObjectReference Make___objRef_global__System_Collections_Generic_IDictionary_string__string_()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.___objRef_global__System_Collections_Generic_IDictionary_string__string_, (IObjectReference)typeof(IObjectReference).GetMethod("As", Type.EmptyTypes).MakeGenericMethod(new Type[]
			{
				typeof(IDictionary<string, string>).GetHelperType().FindVftblType()
			}).Invoke(((IWinRTObject)this).NativeObject, null), null);
			return this.___objRef_global__System_Collections_Generic_IDictionary_string__string_;
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00006C7A File Offset: 0x00004E7A
		private IObjectReference _objRef_global__System_Collections_Generic_IDictionary_string__string_
		{
			get
			{
				return this.___objRef_global__System_Collections_Generic_IDictionary_string__string_ ?? this.Make___objRef_global__System_Collections_Generic_IDictionary_string__string_();
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00006C90 File Offset: 0x00004E90
		private IObjectReference Make___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__string__()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__string__, (IObjectReference)typeof(IObjectReference).GetMethod("As", Type.EmptyTypes).MakeGenericMethod(new Type[]
			{
				typeof(IEnumerable<KeyValuePair<string, string>>).GetHelperType().FindVftblType()
			}).Invoke(((IWinRTObject)this).NativeObject, null), null);
			return this.___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__string__;
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00006CFE File Offset: 0x00004EFE
		private IObjectReference _objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__string__
		{
			get
			{
				return this.___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__string__ ?? this.Make___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__string__();
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00006D14 File Offset: 0x00004F14
		private IObjectReference Make___objRef_global__Windows_Foundation_Collections_IObservableMap_string__string_()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.___objRef_global__Windows_Foundation_Collections_IObservableMap_string__string_, (IObjectReference)typeof(IObjectReference).GetMethod("As", Type.EmptyTypes).MakeGenericMethod(new Type[]
			{
				typeof(IObservableMap<string, string>).GetHelperType().FindVftblType()
			}).Invoke(((IWinRTObject)this).NativeObject, null), null);
			return this.___objRef_global__Windows_Foundation_Collections_IObservableMap_string__string_;
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00006D82 File Offset: 0x00004F82
		private IObjectReference _objRef_global__Windows_Foundation_Collections_IObservableMap_string__string_
		{
			get
			{
				return this.___objRef_global__Windows_Foundation_Collections_IObservableMap_string__string_ ?? this.Make___objRef_global__Windows_Foundation_Collections_IObservableMap_string__string_();
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00006D96 File Offset: 0x00004F96
		private IDictionary<string, string> _default
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00006D99 File Offset: 0x00004F99
		public StringMap() : this(ActivationFactory<StringMap>.ActivateInstance<IUnknownVftbl>())
		{
			ComWrappersSupport.RegisterObjectForInterface(this, this.ThisPtr);
			ComWrappersHelper.Init(this._inner, false);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00006DBE File Offset: 0x00004FBE
		public static StringMap FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			return MarshalInspectable<StringMap>.FromAbi(thisPtr);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00006DD5 File Offset: 0x00004FD5
		internal StringMap(IObjectReference objRef)
		{
			this._inner = objRef.As(GuidGenerator.GetIID(typeof(IDictionary<string, string>).GetHelperType()));
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00006E08 File Offset: 0x00005008
		public static bool operator ==(StringMap x, StringMap y)
		{
			return ((x != null) ? x.ThisPtr : IntPtr.Zero) == ((y != null) ? y.ThisPtr : IntPtr.Zero);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00006E2F File Offset: 0x0000502F
		public static bool operator !=(StringMap x, StringMap y)
		{
			return !(x == y);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00006E3B File Offset: 0x0000503B
		public bool Equals(StringMap other)
		{
			return this == other;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00006E44 File Offset: 0x00005044
		public override bool Equals(object obj)
		{
			StringMap stringMap = obj as StringMap;
			return stringMap != null && this == stringMap;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00006E64 File Offset: 0x00005064
		public override int GetHashCode()
		{
			return this.ThisPtr.GetHashCode();
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00006E7F File Offset: 0x0000507F
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x00006E82 File Offset: 0x00005082
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00006E8A File Offset: 0x0000508A
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x00006EA6 File Offset: 0x000050A6
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00006EBA File Offset: 0x000050BA
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x00006ED6 File Offset: 0x000050D6
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x00006EEA File Offset: 0x000050EA
		public ICollection<string> Keys
		{
			get
			{
				return IDictionaryMethods<string, string>.get_Keys(this._objRef_global__System_Collections_Generic_IDictionary_string__string_);
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00006EF7 File Offset: 0x000050F7
		public ICollection<string> Values
		{
			get
			{
				return IDictionaryMethods<string, string>.get_Values(this._objRef_global__System_Collections_Generic_IDictionary_string__string_);
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00006F04 File Offset: 0x00005104
		public int Count
		{
			get
			{
				return IDictionaryMethods<string, string>.get_Count(this._objRef_global__System_Collections_Generic_IDictionary_string__string_);
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00006F11 File Offset: 0x00005111
		public bool IsReadOnly
		{
			get
			{
				return IDictionaryMethods<string, string>.get_IsReadOnly(this._objRef_global__System_Collections_Generic_IDictionary_string__string_);
			}
		}

		// Token: 0x17000211 RID: 529
		public string this[string key]
		{
			get
			{
				return IDictionaryMethods<string, string>.Indexer_Get(this._objRef_global__System_Collections_Generic_IDictionary_string__string_, this._lookupCache, key);
			}
			set
			{
				IDictionaryMethods<string, string>.Indexer_Set(this._objRef_global__System_Collections_Generic_IDictionary_string__string_, key, value);
			}
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00006F41 File Offset: 0x00005141
		public void Add(string key, string value)
		{
			IDictionaryMethods<string, string>.Add(this._objRef_global__System_Collections_Generic_IDictionary_string__string_, key, value);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00006F50 File Offset: 0x00005150
		public bool ContainsKey(string key)
		{
			return IDictionaryMethods<string, string>.ContainsKey(this._objRef_global__System_Collections_Generic_IDictionary_string__string_, key);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00006F5E File Offset: 0x0000515E
		public bool Remove(string key)
		{
			return IDictionaryMethods<string, string>.Remove(this._objRef_global__System_Collections_Generic_IDictionary_string__string_, key);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00006F6C File Offset: 0x0000516C
		public bool TryGetValue(string key, out string value)
		{
			return IDictionaryMethods<string, string>.TryGetValue(this._objRef_global__System_Collections_Generic_IDictionary_string__string_, this._lookupCache, key, ref value);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00006F81 File Offset: 0x00005181
		public void Add(KeyValuePair<string, string> item)
		{
			IDictionaryMethods<string, string>.Add(this._objRef_global__System_Collections_Generic_IDictionary_string__string_, item);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00006F8F File Offset: 0x0000518F
		public void Clear()
		{
			IDictionaryMethods<string, string>.Clear(this._objRef_global__System_Collections_Generic_IDictionary_string__string_);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00006F9C File Offset: 0x0000519C
		public bool Contains(KeyValuePair<string, string> item)
		{
			return IDictionaryMethods<string, string>.Contains(this._objRef_global__System_Collections_Generic_IDictionary_string__string_, this._lookupCache, item);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00006FB0 File Offset: 0x000051B0
		public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
		{
			IDictionaryMethods<string, string>.CopyTo(this._objRef_global__System_Collections_Generic_IDictionary_string__string_, this._objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__string__, array, arrayIndex);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00006FC5 File Offset: 0x000051C5
		bool ICollection<KeyValuePair<string, string>>.Remove(KeyValuePair<string, string> item)
		{
			return IDictionaryMethods<string, string>.Remove(this._objRef_global__System_Collections_Generic_IDictionary_string__string_, item);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00006FD3 File Offset: 0x000051D3
		public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
		{
			return IEnumerableMethods<KeyValuePair<string, string>>.GetEnumerator(this._objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__string__);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00006FE0 File Offset: 0x000051E0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060004D5 RID: 1237 RVA: 0x00006FE8 File Offset: 0x000051E8
		// (remove) Token: 0x060004D6 RID: 1238 RVA: 0x00007001 File Offset: 0x00005201
		public event MapChangedEventHandler<string, string> MapChanged
		{
			add
			{
				IObservableMapMethods<string, string>.Get_MapChanged(this._objRef_global__Windows_Foundation_Collections_IObservableMap_string__string_, this).Item1.Invoke(value);
			}
			remove
			{
				IObservableMapMethods<string, string>.Get_MapChanged(this._objRef_global__Windows_Foundation_Collections_IObservableMap_string__string_, this).Item2.Invoke(value);
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060004D7 RID: 1239 RVA: 0x0000701A File Offset: 0x0000521A
		// (remove) Token: 0x060004D8 RID: 1240 RVA: 0x00007023 File Offset: 0x00005223
		event MapChangedEventHandler<string, string> IObservableMap<string, string>.MapChanged
		{
			add
			{
				this.MapChanged += value;
			}
			remove
			{
				this.MapChanged -= value;
			}
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0000702C File Offset: 0x0000522C
		private bool IsOverridableInterface(Guid iid)
		{
			return false;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00007030 File Offset: 0x00005230
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

		// Token: 0x04000118 RID: 280
		private IObjectReference _inner;

		// Token: 0x04000119 RID: 281
		private volatile IObjectReference ___objRef_global__System_Collections_Generic_IDictionary_string__string_;

		// Token: 0x0400011A RID: 282
		private volatile IObjectReference ___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__string__;

		// Token: 0x0400011B RID: 283
		private volatile IObjectReference ___objRef_global__Windows_Foundation_Collections_IObservableMap_string__string_;

		// Token: 0x0400011C RID: 284
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x0400011D RID: 285
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;

		// Token: 0x0400011E RID: 286
		private Dictionary<string, ValueTuple<IntPtr, string>> _lookupCache = new Dictionary<string, ValueTuple<IntPtr, string>>();

		// Token: 0x020002C3 RID: 707
		private struct InterfaceTag<I>
		{
		}
	}
}
