using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020001A6 RID: 422
	internal class SingleInterfaceOptimizedObject : IWinRTObject, IDynamicInterfaceCastable
	{
		// Token: 0x06000DA9 RID: 3497 RVA: 0x00029308 File Offset: 0x00027508
		public SingleInterfaceOptimizedObject(Type type, IObjectReference objRef) : this(type, objRef, true)
		{
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x00029314 File Offset: 0x00027514
		internal SingleInterfaceOptimizedObject(Type type, IObjectReference objRef, bool requireQI)
		{
			this._type = type;
			if (!requireQI)
			{
				this._obj = objRef;
				return;
			}
			Type type2 = type.FindHelperType();
			Type type3 = type2.FindVftblType();
			if (type3 == null)
			{
				this._obj = objRef.As<IUnknownVftbl>(GuidGenerator.GetIID(type2));
				return;
			}
			this._obj = (IObjectReference)typeof(IObjectReference).GetMethod("As", Type.EmptyTypes).MakeGenericMethod(new Type[]
			{
				type3
			}).Invoke(objRef, null);
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000DAB RID: 3499 RVA: 0x00029397 File Offset: 0x00027597
		IObjectReference IWinRTObject.NativeObject
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x0002939F File Offset: 0x0002759F
		bool IWinRTObject.HasUnwrappableNativeObject
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x000293A2 File Offset: 0x000275A2
		private ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> MakeQueryInterfaceCache()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>>(ref this._queryInterfaceCache, new ConcurrentDictionary<RuntimeTypeHandle, IObjectReference>(), null);
			return this._queryInterfaceCache;
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x000293BE File Offset: 0x000275BE
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> IWinRTObject.QueryInterfaceCache
		{
			get
			{
				return this._queryInterfaceCache ?? this.MakeQueryInterfaceCache();
			}
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x000293D2 File Offset: 0x000275D2
		private ConcurrentDictionary<RuntimeTypeHandle, object> MakeAdditionalTypeData()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<RuntimeTypeHandle, object>>(ref this._additionalTypeData, new ConcurrentDictionary<RuntimeTypeHandle, object>(), null);
			return this._additionalTypeData;
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x000293EE File Offset: 0x000275EE
		ConcurrentDictionary<RuntimeTypeHandle, object> IWinRTObject.AdditionalTypeData
		{
			get
			{
				return this._additionalTypeData ?? this.MakeAdditionalTypeData();
			}
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x00029402 File Offset: 0x00027602
		bool IDynamicInterfaceCastable.IsInterfaceImplemented(RuntimeTypeHandle interfaceType, bool throwIfNotImplemented)
		{
			return this._type.Equals(Type.GetTypeFromHandle(interfaceType)) || ((IWinRTObject)this).IsInterfaceImplementedFallback(interfaceType, throwIfNotImplemented);
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x00029421 File Offset: 0x00027621
		IObjectReference IWinRTObject.GetObjectReferenceForType(RuntimeTypeHandle interfaceType)
		{
			if (this._type.Equals(Type.GetTypeFromHandle(interfaceType)))
			{
				return this._obj;
			}
			return ((IWinRTObject)this).GetObjectReferenceForTypeFallback(interfaceType);
		}

		// Token: 0x04000297 RID: 663
		private Type _type;

		// Token: 0x04000298 RID: 664
		private IObjectReference _obj;

		// Token: 0x04000299 RID: 665
		private volatile ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> _queryInterfaceCache;

		// Token: 0x0400029A RID: 666
		private volatile ConcurrentDictionary<RuntimeTypeHandle, object> _additionalTypeData;
	}
}
