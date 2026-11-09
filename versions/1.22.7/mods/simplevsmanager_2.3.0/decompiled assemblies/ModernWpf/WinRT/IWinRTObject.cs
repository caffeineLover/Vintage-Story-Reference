using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ABI.System.Collections;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x02000194 RID: 404
	internal interface IWinRTObject : IDynamicInterfaceCastable
	{
		// Token: 0x06000CBE RID: 3262 RVA: 0x00024AAB File Offset: 0x00022CAB
		bool IsInterfaceImplemented(RuntimeTypeHandle interfaceType, bool throwIfNotImplemented)
		{
			return this.IsInterfaceImplementedFallback(interfaceType, throwIfNotImplemented);
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x00024AB8 File Offset: 0x00022CB8
		bool IsInterfaceImplementedFallback(RuntimeTypeHandle interfaceType, bool throwIfNotImplemented)
		{
			if (this.QueryInterfaceCache.ContainsKey(interfaceType))
			{
				return true;
			}
			Type type = Type.GetTypeFromHandle(interfaceType);
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IReadOnlyCollection))
			{
				Type type9 = type.GetGenericArguments()[0];
				if (type9.IsGenericType && type9.GetGenericTypeDefinition() == typeof(KeyValuePair))
				{
					Type type2 = typeof(IReadOnlyDictionary).MakeGenericType(type9.GetGenericArguments());
					if (this.IsInterfaceImplemented(type2.TypeHandle, false))
					{
						IObjectReference objectReference;
						if (this.QueryInterfaceCache.TryGetValue(type2.TypeHandle, ref objectReference) && !this.QueryInterfaceCache.TryAdd(interfaceType, objectReference))
						{
							objectReference.Dispose();
						}
						return true;
					}
				}
				Type type3 = typeof(IReadOnlyList).MakeGenericType(new Type[]
				{
					type9
				});
				if (this.IsInterfaceImplemented(type3.TypeHandle, throwIfNotImplemented))
				{
					IObjectReference objectReference2;
					if (this.QueryInterfaceCache.TryGetValue(type3.TypeHandle, ref objectReference2) && !this.QueryInterfaceCache.TryAdd(interfaceType, objectReference2))
					{
						objectReference2.Dispose();
					}
					return true;
				}
				return false;
			}
			else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection))
			{
				Type type4 = type.GetGenericArguments()[0];
				if (type4.IsGenericType && type4.GetGenericTypeDefinition() == typeof(KeyValuePair))
				{
					Type type5 = typeof(IDictionary).MakeGenericType(type4.GetGenericArguments());
					if (this.IsInterfaceImplemented(type5.TypeHandle, false))
					{
						IObjectReference objectReference3;
						if (this.QueryInterfaceCache.TryGetValue(type5.TypeHandle, ref objectReference3) && !this.QueryInterfaceCache.TryAdd(interfaceType, objectReference3))
						{
							objectReference3.Dispose();
						}
						return true;
					}
				}
				Type type6 = typeof(IList).MakeGenericType(new Type[]
				{
					type4
				});
				if (this.IsInterfaceImplemented(type6.TypeHandle, throwIfNotImplemented))
				{
					IObjectReference objectReference4;
					if (this.QueryInterfaceCache.TryGetValue(type6.TypeHandle, ref objectReference4) && !this.QueryInterfaceCache.TryAdd(interfaceType, objectReference4))
					{
						objectReference4.Dispose();
					}
					return true;
				}
				return false;
			}
			else
			{
				if (type == typeof(IEnumerable))
				{
					Type typeFromHandle = typeof(IEnumerable<object>);
					if (this.IsInterfaceImplemented(typeFromHandle.TypeHandle, false))
					{
						IObjectReference objectReference5;
						if (this.QueryInterfaceCache.TryGetValue(typeFromHandle.TypeHandle, ref objectReference5) && !this.QueryInterfaceCache.TryAdd(interfaceType, objectReference5))
						{
							objectReference5.Dispose();
						}
						return true;
					}
				}
				Type type7 = type.FindHelperType();
				if (type7 == null || !type7.IsInterface)
				{
					return false;
				}
				ObjectReference<IUnknownVftbl> objectReference6;
				int num = this.NativeObject.TryAs<IUnknownVftbl>(GuidGenerator.GetIID(type7), out objectReference6);
				if (num < 0)
				{
					if (throwIfNotImplemented)
					{
						ExceptionHelpers.ThrowExceptionForHR(num);
					}
					return false;
				}
				if (typeof(IEnumerable).IsAssignableFrom(type))
				{
					RuntimeTypeHandle typeHandle = typeof(IEnumerable).TypeHandle;
					this.AdditionalTypeData.GetOrAdd(typeHandle, (RuntimeTypeHandle _) => new IEnumerable.AdaptiveFromAbiHelper(type, this));
				}
				Type type8 = type7.FindVftblType();
				bool result;
				using (objectReference6)
				{
					if (type8 == null)
					{
						ObjectReference<IUnknownVftbl> objectReference8 = objectReference6.As<IUnknownVftbl>(GuidGenerator.GetIID(type7));
						if (!this.QueryInterfaceCache.TryAdd(interfaceType, objectReference8))
						{
							objectReference8.Dispose();
						}
						result = true;
					}
					else
					{
						IObjectReference objectReference9 = (IObjectReference)typeof(IObjectReference).GetMethod("As", Type.EmptyTypes).MakeGenericMethod(new Type[]
						{
							type8
						}).Invoke(objectReference6, null);
						if (!this.QueryInterfaceCache.TryAdd(interfaceType, objectReference9))
						{
							objectReference9.Dispose();
						}
						result = true;
					}
				}
				return result;
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00024EA0 File Offset: 0x000230A0
		RuntimeTypeHandle GetInterfaceImplementation(RuntimeTypeHandle interfaceType)
		{
			Type helperType = Type.GetTypeFromHandle(interfaceType).GetHelperType();
			if (helperType.IsInterface)
			{
				return helperType.TypeHandle;
			}
			return default(RuntimeTypeHandle);
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000CC1 RID: 3265
		IObjectReference NativeObject { get; }

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000CC2 RID: 3266
		bool HasUnwrappableNativeObject { get; }

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000CC3 RID: 3267
		ConcurrentDictionary<RuntimeTypeHandle, IObjectReference> QueryInterfaceCache { get; }

		// Token: 0x06000CC4 RID: 3268 RVA: 0x00024ED1 File Offset: 0x000230D1
		IObjectReference GetObjectReferenceForType(RuntimeTypeHandle type)
		{
			return this.GetObjectReferenceForTypeFallback(type);
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x00024EDA File Offset: 0x000230DA
		IObjectReference GetObjectReferenceForTypeFallback(RuntimeTypeHandle type)
		{
			if (this.IsInterfaceImplemented(type, true))
			{
				return this.QueryInterfaceCache[type];
			}
			string text = "Interface ";
			Type typeFromHandle = Type.GetTypeFromHandle(type);
			throw new Exception(text + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + " is not implemented.");
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000CC6 RID: 3270
		ConcurrentDictionary<RuntimeTypeHandle, object> AdditionalTypeData { get; }

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00024F1C File Offset: 0x0002311C
		object GetOrCreateTypeHelperData(RuntimeTypeHandle type, Func<object> helperDataFactory)
		{
			return this.AdditionalTypeData.GetOrAdd(type2, (RuntimeTypeHandle type) => helperDataFactory.Invoke());
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00024F50 File Offset: 0x00023150
		void Resurrect()
		{
			if (this.NativeObject.Resurrect())
			{
				foreach (KeyValuePair<RuntimeTypeHandle, IObjectReference> keyValuePair in this.QueryInterfaceCache)
				{
					keyValuePair.Value.Resurrect();
				}
				object obj;
				if (this.AdditionalTypeData.TryGetValue(typeof(AgileReference).TypeHandle, ref obj))
				{
					this.AdditionalTypeData.TryUpdate(typeof(AgileReference).TypeHandle, new AgileReference(this.NativeObject), obj);
				}
			}
		}
	}
}
