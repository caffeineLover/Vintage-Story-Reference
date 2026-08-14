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
	// Token: 0x02000091 RID: 145
	[WindowsRuntimeType("Windows.Foundation.FoundationContract")]
	[ProjectedRuntimeClass("_default")]
	[ObjectReferenceWrapper("_inner")]
	[SupportedOSPlatform("Windows10.0.10240.0")]
	[ContractVersion(typeof(FoundationContract), 65536U)]
	internal sealed class PropertySet : IPropertySet, IObservableMap<string, object>, IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable, ICustomQueryInterface, IWinRTObject, IDynamicInterfaceCastable, IEquatable<PropertySet>
	{
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x0000674F File Offset: 0x0000494F
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

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x00006770 File Offset: 0x00004970
		private IObjectReference _objRef_global__Windows_Foundation_Collections_IPropertySet
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00006778 File Offset: 0x00004978
		private IObjectReference Make___objRef_global__Windows_Foundation_Collections_IObservableMap_string__object_()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.___objRef_global__Windows_Foundation_Collections_IObservableMap_string__object_, (IObjectReference)typeof(IObjectReference).GetMethod("As", Type.EmptyTypes).MakeGenericMethod(new Type[]
			{
				typeof(IObservableMap<string, object>).GetHelperType().FindVftblType()
			}).Invoke(((IWinRTObject)this).NativeObject, null), null);
			return this.___objRef_global__Windows_Foundation_Collections_IObservableMap_string__object_;
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000483 RID: 1155 RVA: 0x000067E6 File Offset: 0x000049E6
		private IObjectReference _objRef_global__Windows_Foundation_Collections_IObservableMap_string__object_
		{
			get
			{
				return this.___objRef_global__Windows_Foundation_Collections_IObservableMap_string__object_ ?? this.Make___objRef_global__Windows_Foundation_Collections_IObservableMap_string__object_();
			}
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x000067FC File Offset: 0x000049FC
		private IObjectReference Make___objRef_global__System_Collections_Generic_IDictionary_string__object_()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.___objRef_global__System_Collections_Generic_IDictionary_string__object_, (IObjectReference)typeof(IObjectReference).GetMethod("As", Type.EmptyTypes).MakeGenericMethod(new Type[]
			{
				typeof(IDictionary<string, object>).GetHelperType().FindVftblType()
			}).Invoke(((IWinRTObject)this).NativeObject, null), null);
			return this.___objRef_global__System_Collections_Generic_IDictionary_string__object_;
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x0000686A File Offset: 0x00004A6A
		private IObjectReference _objRef_global__System_Collections_Generic_IDictionary_string__object_
		{
			get
			{
				return this.___objRef_global__System_Collections_Generic_IDictionary_string__object_ ?? this.Make___objRef_global__System_Collections_Generic_IDictionary_string__object_();
			}
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00006880 File Offset: 0x00004A80
		private IObjectReference Make___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__, (IObjectReference)typeof(IObjectReference).GetMethod("As", Type.EmptyTypes).MakeGenericMethod(new Type[]
			{
				typeof(IEnumerable<KeyValuePair<string, object>>).GetHelperType().FindVftblType()
			}).Invoke(((IWinRTObject)this).NativeObject, null), null);
			return this.___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__;
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000487 RID: 1159 RVA: 0x000068EE File Offset: 0x00004AEE
		private IObjectReference _objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__
		{
			get
			{
				return this.___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__ ?? this.Make___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__();
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x00006902 File Offset: 0x00004B02
		private IPropertySet _default
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00006905 File Offset: 0x00004B05
		public PropertySet() : this(ActivationFactory<PropertySet>.ActivateInstance<IUnknownVftbl>())
		{
			ComWrappersSupport.RegisterObjectForInterface(this, this.ThisPtr);
			ComWrappersHelper.Init(this._inner, false);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0000692A File Offset: 0x00004B2A
		public static PropertySet FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			return MarshalInspectable<PropertySet>.FromAbi(thisPtr);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00006941 File Offset: 0x00004B41
		internal PropertySet(IObjectReference objRef)
		{
			this._inner = objRef.As(GuidGenerator.GetIID(typeof(IPropertySet).GetHelperType()));
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x00006974 File Offset: 0x00004B74
		public static bool operator ==(PropertySet x, PropertySet y)
		{
			return ((x != null) ? x.ThisPtr : IntPtr.Zero) == ((y != null) ? y.ThisPtr : IntPtr.Zero);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x0000699B File Offset: 0x00004B9B
		public static bool operator !=(PropertySet x, PropertySet y)
		{
			return !(x == y);
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x000069A7 File Offset: 0x00004BA7
		public bool Equals(PropertySet other)
		{
			return this == other;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x000069B0 File Offset: 0x00004BB0
		public override bool Equals(object obj)
		{
			PropertySet propertySet = obj as PropertySet;
			return propertySet != null && this == propertySet;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x000069D0 File Offset: 0x00004BD0
		public override int GetHashCode()
		{
			return this.ThisPtr.GetHashCode();
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000491 RID: 1169 RVA: 0x000069EB File Offset: 0x00004BEB
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x000069EE File Offset: 0x00004BEE
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x000069F6 File Offset: 0x00004BF6
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x00006A12 File Offset: 0x00004C12
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x00006A26 File Offset: 0x00004C26
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x00006A42 File Offset: 0x00004C42
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000497 RID: 1175 RVA: 0x00006A56 File Offset: 0x00004C56
		// (remove) Token: 0x06000498 RID: 1176 RVA: 0x00006A6F File Offset: 0x00004C6F
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

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000499 RID: 1177 RVA: 0x00006A88 File Offset: 0x00004C88
		// (remove) Token: 0x0600049A RID: 1178 RVA: 0x00006A91 File Offset: 0x00004C91
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

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x00006A9A File Offset: 0x00004C9A
		public ICollection<string> Keys
		{
			get
			{
				return IDictionaryMethods<string, object>.get_Keys(this._objRef_global__System_Collections_Generic_IDictionary_string__object_);
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x00006AA7 File Offset: 0x00004CA7
		public ICollection<object> Values
		{
			get
			{
				return IDictionaryMethods<string, object>.get_Values(this._objRef_global__System_Collections_Generic_IDictionary_string__object_);
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x00006AB4 File Offset: 0x00004CB4
		public int Count
		{
			get
			{
				return IDictionaryMethods<string, object>.get_Count(this._objRef_global__System_Collections_Generic_IDictionary_string__object_);
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x00006AC1 File Offset: 0x00004CC1
		public bool IsReadOnly
		{
			get
			{
				return IDictionaryMethods<string, object>.get_IsReadOnly(this._objRef_global__System_Collections_Generic_IDictionary_string__object_);
			}
		}

		// Token: 0x17000203 RID: 515
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

		// Token: 0x060004A1 RID: 1185 RVA: 0x00006AF1 File Offset: 0x00004CF1
		public void Add(string key, object value)
		{
			IDictionaryMethods<string, object>.Add(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, key, value);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00006B00 File Offset: 0x00004D00
		public bool ContainsKey(string key)
		{
			return IDictionaryMethods<string, object>.ContainsKey(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, key);
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00006B0E File Offset: 0x00004D0E
		public bool Remove(string key)
		{
			return IDictionaryMethods<string, object>.Remove(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, key);
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00006B1C File Offset: 0x00004D1C
		public bool TryGetValue(string key, out object value)
		{
			return IDictionaryMethods<string, object>.TryGetValue(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, this._lookupCache, key, ref value);
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00006B31 File Offset: 0x00004D31
		public void Add(KeyValuePair<string, object> item)
		{
			IDictionaryMethods<string, object>.Add(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, item);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00006B3F File Offset: 0x00004D3F
		public void Clear()
		{
			IDictionaryMethods<string, object>.Clear(this._objRef_global__System_Collections_Generic_IDictionary_string__object_);
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00006B4C File Offset: 0x00004D4C
		public bool Contains(KeyValuePair<string, object> item)
		{
			return IDictionaryMethods<string, object>.Contains(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, this._lookupCache, item);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00006B60 File Offset: 0x00004D60
		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			IDictionaryMethods<string, object>.CopyTo(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, this._objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__, array, arrayIndex);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00006B75 File Offset: 0x00004D75
		bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
		{
			return IDictionaryMethods<string, object>.Remove(this._objRef_global__System_Collections_Generic_IDictionary_string__object_, item);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00006B83 File Offset: 0x00004D83
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return IEnumerableMethods<KeyValuePair<string, object>>.GetEnumerator(this._objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00006B90 File Offset: 0x00004D90
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00006B98 File Offset: 0x00004D98
		private bool IsOverridableInterface(Guid iid)
		{
			return false;
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00006B9C File Offset: 0x00004D9C
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

		// Token: 0x04000111 RID: 273
		private IObjectReference _inner;

		// Token: 0x04000112 RID: 274
		private volatile IObjectReference ___objRef_global__Windows_Foundation_Collections_IObservableMap_string__object_;

		// Token: 0x04000113 RID: 275
		private volatile IObjectReference ___objRef_global__System_Collections_Generic_IDictionary_string__object_;

		// Token: 0x04000114 RID: 276
		private volatile IObjectReference ___objRef_global__System_Collections_Generic_IEnumerable_global__System_Collections_Generic_KeyValuePair_string__object__;

		// Token: 0x04000115 RID: 277
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x04000116 RID: 278
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;

		// Token: 0x04000117 RID: 279
		private Dictionary<string, ValueTuple<IntPtr, object>> _lookupCache = new Dictionary<string, ValueTuple<IntPtr, object>>();

		// Token: 0x020002C2 RID: 706
		private struct InterfaceTag<I>
		{
		}
	}
}
