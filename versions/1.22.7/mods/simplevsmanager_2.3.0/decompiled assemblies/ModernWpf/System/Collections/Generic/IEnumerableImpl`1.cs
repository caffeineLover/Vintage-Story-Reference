using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using ABI.System.Collections.Generic;
using WinRT;

namespace System.Collections.Generic
{
	// Token: 0x02000013 RID: 19
	internal sealed class IEnumerableImpl<T> : IEnumerable<T>, IEnumerable, IWinRTObject, IDynamicInterfaceCastable
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00003515 File Offset: 0x00001715
		internal IEnumerableImpl(IObjectReference _inner)
		{
			this._inner = _inner;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003524 File Offset: 0x00001724
		public static IEnumerableImpl<T> CreateRcw(IInspectable obj)
		{
			return new IEnumerableImpl<T>(obj.ObjRef);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003531 File Offset: 0x00001731
		private IObjectReference Make_IEnumerableObjRef()
		{
			Interlocked.CompareExchange<IObjectReference>(ref this.__iEnumerableObjRef, this._inner.As<IEnumerable<T>.Vftbl>(), null);
			return this.__iEnumerableObjRef;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00003553 File Offset: 0x00001753
		private IObjectReference iEnumerableObjRef
		{
			get
			{
				return this.__iEnumerableObjRef ?? this.Make_IEnumerableObjRef();
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00003567 File Offset: 0x00001767
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._inner;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000094 RID: 148 RVA: 0x0000356F File Offset: 0x0000176F
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003572 File Offset: 0x00001772
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000096 RID: 150 RVA: 0x0000358E File Offset: 0x0000178E
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000035A2 File Offset: 0x000017A2
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000035BE File Offset: 0x000017BE
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000035D2 File Offset: 0x000017D2
		public IEnumerator<T> GetEnumerator()
		{
			return IEnumerableMethods<T>.GetEnumerator(this.iEnumerableObjRef);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000035DF File Offset: 0x000017DF
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000026 RID: 38
		private IObjectReference _inner;

		// Token: 0x04000027 RID: 39
		private volatile IObjectReference __iEnumerableObjRef;

		// Token: 0x04000028 RID: 40
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x04000029 RID: 41
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
	}
}
