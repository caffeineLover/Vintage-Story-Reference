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
	// Token: 0x02000093 RID: 147
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[ProjectedRuntimeClass("_default")]
	[ObjectReferenceWrapper("_inner")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class ValueSet : IPropertySet, IObservableMap<string, object>, IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable, ICustomQueryInterface, IWinRTObject, IDynamicInterfaceCastable, IEquatable<ValueSet>
	{
		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x0000707E File Offset: 0x0000527E
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

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060004DC RID: 1244 RVA: 0x0000709F File Offset: 0x0000529F
		private IObjectReference _objRef_global__Windows_Foundation_Collections_IPropertySet
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x000070A8 File Offset: 0x000052A8
		private IObjectReference Make___objRef_global__Windows_Foundation_Collections_IObservableMap_string__object_()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.___objRef_global__Windows_Foundation_Collections_IObservableMap_string__object_, (IObjectReference)typeof(IObjectReference).GetMethod("As", Type.EmptyTypes).MakeGenericMethod(new Type[]
			{
				typeof(IObservableMap<string, object>).GetHelperType().FindVftblType()
			}).Invoke(((IWinRTObject)this).NativeObject, null), null);
			return this.___objRef_global__Windows_Foundation_Collections_IObservableMap_string__object_;
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x00007116 File Offset: 0x00005316
		private IObjectReference _objRef_global__Windows_Foundation_Collections_IObservableMap_string__object_
		{
			get
			{
				return this.___objRef_global__Windows_Foundation_Collections_IObservableMap_string__object_ ?? this.Make___objRef_global__Windows_Foundation_Collections_IObservableMap_string__object_();
			}
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0000712C File Offset: 0x0000532C
		private IObjectReference Make___objRef_global__System_Collections_Generic_IDictionary_string__object_()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.___objRef_global__System_Collections_Generic_IDictionary_string__object_, (IObjectReference)typeof(IObjectReference).GetMethod("As", Type.EmptyTypes).MakeGenericMethod(new Type[]
			{
				typeof(IDictionary<string, object>).GetHelperType().FindVftblType()
			}).Invoke(((IWinRTObject)this).NativeObject, null), null);
			return this.___objRef_global__System_Collections_Generic_IDictionary_string__object_;
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x0000719A File Offset: 0x0000539A
		private IObjectReference _objRef_global__System_Collections_Generic_IDictionary_string__object_
		{
			get
			{
				return this.___objRef_global__System_Collections_Generic_IDictionary_string__object_ ?? this.Make___objRef_global__System_Collections_Generic_IDictionary_string__object_();
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x000071B0 File Offset: 0x000053B0
		private IObjectReference Make___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__, (IObjectReference)typeof(IObjectReference).GetMethod("As", Type.EmptyTypes).MakeGenericMethod(new Type[]
			{
				typeof(IEnumerable<KeyValuePair<string, object>>).GetHelperType().FindVftblType()
			}).Invoke(((IWinRTObject)this).NativeObject, null), null);
			return this.___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__;
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x0000721E File Offset: 0x0000541E
		private IObjectReference _objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__
		{
			get
			{
				return this.___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__ ?? this.Make___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__();
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x00007232 File Offset: 0x00005432
		private IPropertySet _default
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00007235 File Offset: 0x00005435
		public ValueSet() : this(ActivationFactory<ValueSet>.ActivateInstance<IUnknownVftbl>())
		{
			ComWrappersSupport.RegisterObjectForInterface(this, this.ThisPtr);
			ComWrappersHelper.Init(this._inner, false);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0000725A File Offset: 0x0000545A
		public static ValueSet FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			return MarshalInspectable<ValueSet>.FromAbi(thisPtr);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00007271 File Offset: 0x00005471
		internal ValueSet(IObjectReference objRef)
		{
			this._inner = objRef.As(GuidGenerator.GetIID(typeof(IPropertySet).GetHelperType()));
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x000072A4 File Offset: 0x000054A4
		public static bool operator ==(ValueSet x, ValueSet y)
		{
			return ((x != null) ? x.ThisPtr : IntPtr.Zero) == ((y != null) ? y.ThisPtr : IntPtr.Zero);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x000072CB File Offset: 0x000054CB
		public static bool operator !=(ValueSet x, ValueSet y)
		{
			return !(x == y);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x000072D7 File Offset: 0x000054D7
		public bool Equals(ValueSet other)
		{
			return this == other;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x000072E0 File Offset: 0x000054E0
		public override bool Equals(object obj)
		{
			ValueSet valueSet = obj as ValueSet;
			return valueSet != null && this == valueSet;
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00007300 File Offset: 0x00005500
		public override int GetHashCode()
		{
			return this.ThisPtr.GetHashCode();
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x0000731B File Offset: 0x0000551B
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0000731E File Offset: 0x0000551E
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00007326 File Offset: 0x00005526
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x00007342 File Offset: 0x00005542
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00007356 File Offset: 0x00005556
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x00007372 File Offset: 0x00005572
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060004F2 RID: 1266 RVA: 0x00007386 File Offset: 0x00005586
		// (remove) Token: 0x060004F3 RID: 1267 RVA: 0x0000739F File Offset: 0x0000559F
		public event MapChangedEventHandler<string, object> MapChanged
		{
			add
			{
				IObservableMapMethods<string, object>.Get_MapChanged(this._objRef_global__Windows_Foundation_Collections_IObservableMap_string__object_, this).Item1.Invoke(value);
			}
			remove
			{
				IObservableMapMethods<string, object>.Get_MapChanged(this._objRef_global__Windows_Foundation_Collections_IObservableMap_string__object_, this).Item2.Invoke(value);
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060004F4 RID: 1268 RVA: 0x000073B8 File Offset: 0x000055B8
		// (remove) Token: 0x060004F5 RID: 1269 RVA: 0x000073C1 File Offset: 0x000055C1
		event MapChangedEventHandler<string, object> IObservableMap<string, object>.MapChanged
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

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x000073CA File Offset: 0x000055CA
		public ICollection<string> Keys
		{
			get
			{
				return IDictionaryMethods<string, object>.get_Keys(this._objRef_global__System_Collections_Generic_IDictionary_string__object_);
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x000073D7 File Offset: 0x000055D7
		public ICollection<object> Values
		{
			get
			{
				return IDictionaryMethods<string, object>.get_Values(this._objRef_global__System_Collections_Generic_IDictionary_string__object_);
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x000073E4 File Offset: 0x000055E4
		public int Count
		{
			get
			{
				return IDictionaryMethods<string, object>.get_Count(this._objRef_global__System_Collections_Generic_IDictionary_string__object_);
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x000073F1 File Offset: 0x000055F1
		public bool IsReadOnly
		{
			get
			{
				return IDictionaryMethods<string, object>.get_IsReadOnly(this._objRef_global__System_Collections_Generic_IDictionary_string__object_);
			}
		}

		// Token: 0x17000220 RID: 544
		public object this[string key]
		{
			get
			{
				return IDictionaryMethods<string, object>.Indexer_Get(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, this._lookupCache, key);
			}
			set
			{
				IDictionaryMethods<string, object>.Indexer_Set(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, key, value);
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00007421 File Offset: 0x00005621
		public void Add(string key, object value)
		{
			IDictionaryMethods<string, object>.Add(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, key, value);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00007430 File Offset: 0x00005630
		public bool ContainsKey(string key)
		{
			return IDictionaryMethods<string, object>.ContainsKey(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, key);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0000743E File Offset: 0x0000563E
		public bool Remove(string key)
		{
			return IDictionaryMethods<string, object>.Remove(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, key);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0000744C File Offset: 0x0000564C
		public bool TryGetValue(string key, out object value)
		{
			return IDictionaryMethods<string, object>.TryGetValue(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, this._lookupCache, key, ref value);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00007461 File Offset: 0x00005661
		public void Add(KeyValuePair<string, object> item)
		{
			IDictionaryMethods<string, object>.Add(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, item);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0000746F File Offset: 0x0000566F
		public void Clear()
		{
			IDictionaryMethods<string, object>.Clear(this._objRef_global__System_Collections_Generic_IDictionary_string__object_);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0000747C File Offset: 0x0000567C
		public bool Contains(KeyValuePair<string, object> item)
		{
			return IDictionaryMethods<string, object>.Contains(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, this._lookupCache, item);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00007490 File Offset: 0x00005690
		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			IDictionaryMethods<string, object>.CopyTo(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, this._objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__, array, arrayIndex);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x000074A5 File Offset: 0x000056A5
		bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
		{
			return IDictionaryMethods<string, object>.Remove(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, item);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x000074B3 File Offset: 0x000056B3
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return IEnumerableMethods<KeyValuePair<string, object>>.GetEnumerator(this._objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000074C0 File Offset: 0x000056C0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x000074C8 File Offset: 0x000056C8
		private bool IsOverridableInterface(Guid iid)
		{
			return false;
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x000074CC File Offset: 0x000056CC
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

		// Token: 0x0400011F RID: 287
		private IObjectReference _inner;

		// Token: 0x04000120 RID: 288
		private volatile IObjectReference ___objRef_global__Windows_Foundation_Collections_IObservableMap_string__object_;

		// Token: 0x04000121 RID: 289
		private volatile IObjectReference ___objRef_global__System_Collections_Generic_IDictionary_string__object_;

		// Token: 0x04000122 RID: 290
		private volatile IObjectReference ___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__;

		// Token: 0x04000123 RID: 291
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x04000124 RID: 292
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;

		// Token: 0x04000125 RID: 293
		private Dictionary<string, ValueTuple<IntPtr, object>> _lookupCache = new Dictionary<string, ValueTuple<IntPtr, object>>();

		// Token: 0x020002C4 RID: 708
		private struct InterfaceTag<I>
		{
		}
	}
}
