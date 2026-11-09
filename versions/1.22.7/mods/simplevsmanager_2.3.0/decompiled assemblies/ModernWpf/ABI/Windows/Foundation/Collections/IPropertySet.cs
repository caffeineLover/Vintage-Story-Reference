using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Foundation.Collections;
using WinRT;

namespace ABI.Windows.Foundation.Collections
{
	// Token: 0x020000FF RID: 255
	[DynamicInterfaceCastableImplementation]
	[Guid("8A43ED9F-F4E6-4421-ACF9-1DAB2986820C")]
	internal interface IPropertySet : Windows.Foundation.Collections.IPropertySet, Windows.Foundation.Collections.IObservableMap<string, object>, IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
	{
		// Token: 0x060008F4 RID: 2292 RVA: 0x00019C6E File Offset: 0x00017E6E
		unsafe static IPropertySet()
		{
			*(IInspectable.Vftbl*)((void*)IPropertySet.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060008F5 RID: 2293 RVA: 0x00019CA0 File Offset: 0x00017EA0
		ICollection<string> IDictionary<string, object>.Keys
		{
			get
			{
				return ((IDictionary<string, object>)((IWinRTObject)this)).Keys;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060008F6 RID: 2294 RVA: 0x00019CB2 File Offset: 0x00017EB2
		ICollection<object> IDictionary<string, object>.Values
		{
			get
			{
				return ((IDictionary<string, object>)((IWinRTObject)this)).Values;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060008F7 RID: 2295 RVA: 0x00019CC4 File Offset: 0x00017EC4
		int ICollection<KeyValuePair<string, object>>.Count
		{
			get
			{
				return ((IDictionary<string, object>)((IWinRTObject)this)).Count;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060008F8 RID: 2296 RVA: 0x00019CD6 File Offset: 0x00017ED6
		bool ICollection<KeyValuePair<string, object>>.IsReadOnly
		{
			get
			{
				return ((IDictionary<string, object>)((IWinRTObject)this)).IsReadOnly;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060008F9 RID: 2297 RVA: 0x00019CE8 File Offset: 0x00017EE8
		// (set) Token: 0x060008FA RID: 2298 RVA: 0x00019CFB File Offset: 0x00017EFB
		object IDictionary<string, object>.Item
		{
			get
			{
				return ((IDictionary<string, object>)((IWinRTObject)this))[key];
			}
			set
			{
				((IDictionary<string, object>)((IWinRTObject)this))[key] = value;
			}
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00019D0F File Offset: 0x00017F0F
		void Add(string key, object value)
		{
			((IDictionary<string, object>)((IWinRTObject)this)).Add(key, value);
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00019D23 File Offset: 0x00017F23
		bool ContainsKey(string key)
		{
			return ((IDictionary<string, object>)((IWinRTObject)this)).ContainsKey(key);
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00019D36 File Offset: 0x00017F36
		bool Remove(string key)
		{
			return ((IDictionary<string, object>)((IWinRTObject)this)).Remove(key);
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00019D49 File Offset: 0x00017F49
		bool TryGetValue(string key, out object value)
		{
			return ((IDictionary<string, object>)((IWinRTObject)this)).TryGetValue(key, ref value);
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x00019D5D File Offset: 0x00017F5D
		void Add(KeyValuePair<string, object> item)
		{
			((IDictionary<string, object>)((IWinRTObject)this)).Add(item);
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00019D70 File Offset: 0x00017F70
		void Clear()
		{
			((IDictionary<string, object>)((IWinRTObject)this)).Clear();
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00019D82 File Offset: 0x00017F82
		bool Contains(KeyValuePair<string, object> item)
		{
			return ((IDictionary<string, object>)((IWinRTObject)this)).Contains(item);
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00019D95 File Offset: 0x00017F95
		void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			((IDictionary<string, object>)((IWinRTObject)this)).CopyTo(array, arrayIndex);
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00019DA9 File Offset: 0x00017FA9
		bool Remove(KeyValuePair<string, object> item)
		{
			return ((IDictionary<string, object>)((IWinRTObject)this)).Remove(item);
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x00019DBC File Offset: 0x00017FBC
		IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return ((IDictionary<string, object>)((IWinRTObject)this)).GetEnumerator();
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x00019DCE File Offset: 0x00017FCE
		IEnumerator GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06000906 RID: 2310 RVA: 0x00019DD6 File Offset: 0x00017FD6
		// (remove) Token: 0x06000907 RID: 2311 RVA: 0x00019DE9 File Offset: 0x00017FE9
		event Windows.Foundation.Collections.MapChangedEventHandler<string, object> Windows.Foundation.Collections.IObservableMap<string, object>.MapChanged
		{
			add
			{
				((Windows.Foundation.Collections.IObservableMap<string, object>)((IWinRTObject)this)).MapChanged += value;
			}
			remove
			{
				((Windows.Foundation.Collections.IObservableMap<string, object>)((IWinRTObject)this)).MapChanged -= value;
			}
		}

		// Token: 0x0400019A RID: 410
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IPropertySet), sizeof(IInspectable.Vftbl) + 0);
	}
}
