using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ABI.Microsoft.UI.Xaml.Data;
using ABI.System;
using ABI.Windows.Foundation;
using ABI.WinRT.Interop;
using Windows.Foundation;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x02000189 RID: 393
	internal static class ComWrappersSupport
	{
		// Token: 0x06000C4B RID: 3147 RVA: 0x00021DBC File Offset: 0x0001FFBC
		public static TReturn MarshalDelegateInvoke<TDelegate, TReturn>(IntPtr thisPtr, Func<TDelegate, TReturn> invoke) where TDelegate : class, Delegate
		{
			TDelegate tdelegate = ComWrappersSupport.FindObject<TDelegate>(thisPtr);
			if (tdelegate != null)
			{
				return invoke.Invoke(tdelegate);
			}
			return default(TReturn);
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x00021DF0 File Offset: 0x0001FFF0
		public static void MarshalDelegateInvoke<T>(IntPtr thisPtr, Action<T> invoke) where T : class, Delegate
		{
			T t = ComWrappersSupport.FindObject<T>(thisPtr);
			if (t != null)
			{
				invoke.Invoke(t);
			}
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x00021E1C File Offset: 0x0002001C
		internal unsafe static bool IsFreeThreaded(IObjectReference objRef)
		{
			IntPtr intPtr;
			if (objRef.TryAs(WinRT.Interop.IAgileObject.IID, out intPtr) >= 0)
			{
				Marshal.Release(intPtr);
				return true;
			}
			IntPtr intPtr2;
			if (objRef.TryAs(ABI.WinRT.Interop.IMarshal.IID, out intPtr2) >= 0)
			{
				try
				{
					Guid iid = IUnknownVftbl.IID;
					method getUnmarshalClass_ = ((IntPtr*)((void*)intPtr2))->GetUnmarshalClass_0;
					Guid guid;
					Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Guid*,System.IntPtr,WinRT.Interop.MSHCTX,System.IntPtr,WinRT.Interop.MSHLFLAGS,System.Guid*), intPtr2, &iid, IntPtr.Zero, 3, IntPtr.Zero, 0, &guid, getUnmarshalClass_));
					if (guid == ABI.WinRT.Interop.IMarshal.IID_InProcFreeThreadedMarshaler.Value)
					{
						return true;
					}
				}
				finally
				{
					Marshal.Release(intPtr2);
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x00021EBC File Offset: 0x000200BC
		public static IObjectReference GetObjectReferenceForInterface(IntPtr externalComObject)
		{
			return ComWrappersSupport.GetObjectReferenceForInterface<IUnknownVftbl>(externalComObject);
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00021EC4 File Offset: 0x000200C4
		public static ObjectReference<T> GetObjectReferenceForInterface<T>(IntPtr externalComObject)
		{
			if (externalComObject == IntPtr.Zero)
			{
				return null;
			}
			ObjectReference<T> objectReference = ObjectReference<T>.FromAbi(externalComObject);
			if (ComWrappersSupport.IsFreeThreaded(objectReference))
			{
				return objectReference;
			}
			ObjectReference<T> result;
			using (objectReference)
			{
				result = new ObjectReferenceWithContext<T>(objectReference.GetRef(), Context.GetContextCallback(), Context.GetContextToken());
			}
			return result;
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00021F28 File Offset: 0x00020128
		public static ObjectReference<T> GetObjectReferenceForInterface<T>(IntPtr externalComObject, Guid iid)
		{
			if (externalComObject == IntPtr.Zero)
			{
				return null;
			}
			IntPtr intPtr;
			Marshal.ThrowExceptionForHR(Marshal.QueryInterface(externalComObject, ref iid, ref intPtr));
			ObjectReference<T> objectReference = ObjectReference<T>.Attach(ref intPtr);
			if (ComWrappersSupport.IsFreeThreaded(objectReference))
			{
				return objectReference;
			}
			ObjectReference<T> result;
			using (objectReference)
			{
				result = new ObjectReferenceWithContext<T>(objectReference.GetRef(), Context.GetContextCallback(), Context.GetContextToken(), iid);
			}
			return result;
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00021F9C File Offset: 0x0002019C
		public static void RegisterProjectionAssembly(Assembly assembly)
		{
			TypeNameSupport.RegisterProjectionAssembly(assembly);
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00021FA4 File Offset: 0x000201A4
		internal static object GetRuntimeClassCCWTypeIfAny(object obj)
		{
			Type type = obj2.GetType();
			Type ccwType = type.GetRuntimeClassCCWType();
			if (ccwType != null)
			{
				return ComWrappersSupport.CCWTable.GetValue(obj2, ([Nullable(1)] object obj) => ccwType.GetConstructor(564, null, new Type[]
				{
					type
				}, null).Invoke(new object[]
				{
					obj
				}));
			}
			return obj2;
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x00021FFC File Offset: 0x000201FC
		internal static List<ComWrappers.ComInterfaceEntry> GetInterfaceTableEntries(Type type)
		{
			List<ComWrappers.ComInterfaceEntry> list = new List<ComWrappers.ComInterfaceEntry>();
			bool flag = false;
			ComWrappers.ComInterfaceEntry comInterfaceEntry;
			if (type.IsDelegate())
			{
				Type type2 = type.FindHelperType();
				if (type2 != null)
				{
					List<ComWrappers.ComInterfaceEntry> list2 = list;
					comInterfaceEntry = default(ComWrappers.ComInterfaceEntry);
					comInterfaceEntry.IID = GuidGenerator.GetIID(type);
					comInterfaceEntry.Vtable = type2.GetAbiToProjectionVftblPtr();
					list2.Add(comInterfaceEntry);
				}
				if (ComWrappersSupport.ShouldProvideIReference(type))
				{
					list.Add(ComWrappersSupport.IPropertyValueEntry);
					list.Add(ComWrappersSupport.ProvideIReference(type));
				}
			}
			else
			{
				Type type3 = type.GetRuntimeClassCCWType() ?? type;
				foreach (Type type4 in type3.GetInterfaces())
				{
					if (Projections.IsTypeWindowsRuntimeType(type4))
					{
						Type type5 = type4.FindHelperType();
						Guid iid = GuidGenerator.GetIID(type5);
						List<ComWrappers.ComInterfaceEntry> list3 = list;
						comInterfaceEntry = default(ComWrappers.ComInterfaceEntry);
						comInterfaceEntry.IID = iid;
						comInterfaceEntry.Vtable = type5.GetAbiToProjectionVftblPtr();
						list3.Add(comInterfaceEntry);
						if (!flag && iid == ABI.WinRT.Interop.IMarshal.IID)
						{
							flag = true;
						}
					}
					IEnumerable<Type> enumerable;
					if (type4.IsConstructedGenericType && Projections.TryGetCompatibleWindowsRuntimeTypesForVariantType(type4, out enumerable))
					{
						foreach (Type type6 in enumerable)
						{
							Type type7 = type6.FindHelperType();
							List<ComWrappers.ComInterfaceEntry> list4 = list;
							comInterfaceEntry = default(ComWrappers.ComInterfaceEntry);
							comInterfaceEntry.IID = GuidGenerator.GetIID(type7);
							comInterfaceEntry.Vtable = type7.GetAbiToProjectionVftblPtr();
							list4.Add(comInterfaceEntry);
						}
					}
				}
				if (type3.IsGenericType && type3.GetGenericTypeDefinition() == typeof(KeyValuePair))
				{
					Type type8 = type3.FindHelperType();
					List<ComWrappers.ComInterfaceEntry> list5 = list;
					comInterfaceEntry = default(ComWrappers.ComInterfaceEntry);
					comInterfaceEntry.IID = GuidGenerator.GetIID(type8);
					comInterfaceEntry.Vtable = type8.GetAbiToProjectionVftblPtr();
					list5.Add(comInterfaceEntry);
				}
				else if (ComWrappersSupport.ShouldProvideIReference(type))
				{
					list.Add(ComWrappersSupport.IPropertyValueEntry);
					list.Add(ComWrappersSupport.ProvideIReference(type));
				}
				else if (ComWrappersSupport.ShouldProvideIReferenceArray(type))
				{
					list.Add(ComWrappersSupport.IPropertyValueEntry);
					list.Add(ComWrappersSupport.ProvideIReferenceArray(type));
				}
			}
			List<ComWrappers.ComInterfaceEntry> list6 = list;
			comInterfaceEntry = default(ComWrappers.ComInterfaceEntry);
			comInterfaceEntry.IID = ManagedIStringableVftbl.IID;
			comInterfaceEntry.Vtable = ManagedIStringableVftbl.AbiToProjectionVftablePtr;
			list6.Add(comInterfaceEntry);
			List<ComWrappers.ComInterfaceEntry> list7 = list;
			comInterfaceEntry = default(ComWrappers.ComInterfaceEntry);
			comInterfaceEntry.IID = ManagedCustomPropertyProviderVftbl.IID;
			comInterfaceEntry.Vtable = ManagedCustomPropertyProviderVftbl.AbiToProjectionVftablePtr;
			list7.Add(comInterfaceEntry);
			List<ComWrappers.ComInterfaceEntry> list8 = list;
			comInterfaceEntry = default(ComWrappers.ComInterfaceEntry);
			comInterfaceEntry.IID = ABI.WinRT.Interop.IWeakReferenceSource.IID;
			comInterfaceEntry.Vtable = ABI.WinRT.Interop.IWeakReferenceSource.AbiToProjectionVftablePtr;
			list8.Add(comInterfaceEntry);
			if (!flag)
			{
				List<ComWrappers.ComInterfaceEntry> list9 = list;
				comInterfaceEntry = default(ComWrappers.ComInterfaceEntry);
				comInterfaceEntry.IID = ABI.WinRT.Interop.IMarshal.IID;
				comInterfaceEntry.Vtable = ABI.WinRT.Interop.IMarshal.Vftbl.AbiToProjectionVftablePtr;
				list9.Add(comInterfaceEntry);
			}
			List<ComWrappers.ComInterfaceEntry> list10 = list;
			comInterfaceEntry = default(ComWrappers.ComInterfaceEntry);
			comInterfaceEntry.IID = WinRT.Interop.IAgileObject.IID;
			comInterfaceEntry.Vtable = IUnknownVftbl.AbiToProjectionVftblPtr;
			list10.Add(comInterfaceEntry);
			List<ComWrappers.ComInterfaceEntry> list11 = list;
			comInterfaceEntry = default(ComWrappers.ComInterfaceEntry);
			comInterfaceEntry.IID = InterfaceIIDs.IInspectable_IID;
			comInterfaceEntry.Vtable = IInspectable.Vftbl.AbiToProjectionVftablePtr;
			list11.Add(comInterfaceEntry);
			List<ComWrappers.ComInterfaceEntry> list12 = list;
			comInterfaceEntry = default(ComWrappers.ComInterfaceEntry);
			comInterfaceEntry.IID = IUnknownVftbl.IID;
			comInterfaceEntry.Vtable = IUnknownVftbl.AbiToProjectionVftblPtr;
			list12.Add(comInterfaceEntry);
			return list;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x00022328 File Offset: 0x00020528
		[return: TupleElementNames(new string[]
		{
			"inspectableInfo",
			"interfaceTableEntries"
		})]
		internal static ValueTuple<ComWrappersSupport.InspectableInfo, List<ComWrappers.ComInterfaceEntry>> PregenerateNativeTypeInformation(Type type)
		{
			List<ComWrappers.ComInterfaceEntry> interfaceTableEntries = ComWrappersSupport.GetInterfaceTableEntries(type);
			Guid[] array = new Guid[interfaceTableEntries.Count];
			for (int i = 0; i < interfaceTableEntries.Count; i++)
			{
				array[i] = interfaceTableEntries[i].IID;
			}
			if (type.FullName.StartsWith("ABI.", 4))
			{
				Type type2;
				if ((type2 = Projections.FindCustomPublicTypeForAbiType(type)) == null)
				{
					type2 = (type.Assembly.GetType(type.FullName.Substring("ABI.".Length)) ?? type);
				}
				type = type2;
			}
			return new ValueTuple<ComWrappersSupport.InspectableInfo, List<ComWrappers.ComInterfaceEntry>>(new ComWrappersSupport.InspectableInfo(type, array), interfaceTableEntries);
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x000223BD File Offset: 0x000205BD
		private static bool IsNullableT(Type implementationType)
		{
			return implementationType.IsGenericType && implementationType.GetGenericTypeDefinition() == typeof(Nullable);
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x000223DE File Offset: 0x000205DE
		private static bool IsAbiNullableDelegate(Type implementationType)
		{
			return implementationType.IsGenericType && implementationType.GetGenericTypeDefinition() == typeof(Nullable_Delegate<>);
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x000223FF File Offset: 0x000205FF
		private static bool IsIReferenceArray(Type implementationType)
		{
			return implementationType.IsGenericType && implementationType.GetGenericTypeDefinition() == typeof(Windows.Foundation.IReferenceArray<>);
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x00022420 File Offset: 0x00020620
		private static Func<IInspectable, object> CreateKeyValuePairFactory(Type type)
		{
			return (Func<IInspectable, object>)type.GetHelperType().GetMethod("CreateRcw", 24).CreateDelegate(typeof(Func<IInspectable, object>));
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x00022448 File Offset: 0x00020648
		internal static Func<IntPtr, object> CreateDelegateFactory(Type type)
		{
			return ComWrappersSupport.DelegateFactoryCache.GetOrAdd(type2, delegate(Type type)
			{
				Func<IntPtr, object> createRcwFunc = (Func<IntPtr, object>)type.GetHelperType().GetMethod("CreateRcw", 24).CreateDelegate(typeof(Func<IntPtr, object>));
				Guid iid = GuidGenerator.GetIID(type);
				return delegate(IntPtr externalComObject)
				{
					IntPtr intPtr;
					Marshal.ThrowExceptionForHR(Marshal.QueryInterface(externalComObject, ref iid, ref intPtr));
					object result;
					try
					{
						result = createRcwFunc.Invoke(intPtr);
					}
					finally
					{
						Marshal.Release(intPtr);
					}
					return result;
				};
			});
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x00022474 File Offset: 0x00020674
		private static Func<IInspectable, object> CreateNullableTFactory(Type implementationType)
		{
			MethodInfo getValueMethod = implementationType.GetHelperType().GetMethod("GetValue", 40);
			return delegate(IInspectable obj)
			{
				MethodBase getValueMethod = getValueMethod;
				object obj2 = null;
				object[] array = new IInspectable[]
				{
					obj
				};
				return getValueMethod.Invoke(obj2, array);
			};
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x0002249E File Offset: 0x0002069E
		private static Func<IInspectable, object> CreateAbiNullableTFactory(Type implementationType)
		{
			MethodInfo getValueMethod = implementationType.GetMethod("GetValue", 40);
			return delegate(IInspectable obj)
			{
				MethodBase getValueMethod = getValueMethod;
				object obj2 = null;
				object[] array = new IInspectable[]
				{
					obj
				};
				return getValueMethod.Invoke(obj2, array);
			};
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x000224C3 File Offset: 0x000206C3
		private static Func<IInspectable, object> CreateArrayFactory(Type implementationType)
		{
			return (Func<IInspectable, object>)implementationType.GetHelperType().GetMethod("GetValue", 40).CreateDelegate(typeof(Func<IInspectable, object>));
		}

		// Token: 0x06000C5D RID: 3165 RVA: 0x000224EB File Offset: 0x000206EB
		private static Func<IInspectable, object> CreateReferenceCachingFactory(Func<IInspectable, object> internalFactory)
		{
			return delegate(IInspectable inspectable)
			{
				object obj = internalFactory.Invoke(inspectable);
				ComWrappersSupport._boxedValueReferenceCache.Add(obj, inspectable);
				return obj;
			};
		}

		// Token: 0x06000C5E RID: 3166 RVA: 0x00022504 File Offset: 0x00020704
		private static Func<IInspectable, object> CreateCustomTypeMappingFactory(Type customTypeHelperType)
		{
			MethodInfo method = customTypeHelperType.GetMethod("FromAbi", 56);
			if (method == null)
			{
				throw new MissingMethodException();
			}
			Func<IntPtr, object> fromAbiMethodFunc = (Func<IntPtr, object>)method.CreateDelegate(typeof(Func<IntPtr, object>));
			return (IInspectable obj) => fromAbiMethodFunc.Invoke(obj.ThisPtr);
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x00022554 File Offset: 0x00020754
		internal static Func<IInspectable, object> CreateTypedRcwFactory(Type implementationType, string runtimeClassName = null)
		{
			if (implementationType == null || implementationType == typeof(object))
			{
				return (IInspectable obj) => obj;
			}
			if (implementationType == typeof(Nullable_string))
			{
				return ComWrappersSupport.CreateReferenceCachingFactory((IInspectable obj) => Nullable_string.GetValue(obj));
			}
			if (implementationType == typeof(Nullable_Type))
			{
				return ComWrappersSupport.CreateReferenceCachingFactory((IInspectable obj) => Nullable_Type.GetValue(obj));
			}
			Type type = Projections.FindCustomHelperTypeMapping(implementationType, true);
			if (type != null)
			{
				return ComWrappersSupport.CreateReferenceCachingFactory(ComWrappersSupport.CreateCustomTypeMappingFactory(type));
			}
			if (implementationType.IsGenericType && implementationType.GetGenericTypeDefinition() == typeof(KeyValuePair))
			{
				return ComWrappersSupport.CreateReferenceCachingFactory(ComWrappersSupport.CreateKeyValuePairFactory(implementationType));
			}
			if (implementationType.IsValueType)
			{
				if (ComWrappersSupport.IsNullableT(implementationType))
				{
					return ComWrappersSupport.CreateReferenceCachingFactory(ComWrappersSupport.CreateNullableTFactory(implementationType));
				}
				return ComWrappersSupport.CreateReferenceCachingFactory(ComWrappersSupport.CreateNullableTFactory(typeof(Nullable).MakeGenericType(new Type[]
				{
					implementationType
				})));
			}
			else
			{
				if (ComWrappersSupport.IsAbiNullableDelegate(implementationType))
				{
					return ComWrappersSupport.CreateReferenceCachingFactory(ComWrappersSupport.CreateAbiNullableTFactory(implementationType));
				}
				if (ComWrappersSupport.IsIReferenceArray(implementationType))
				{
					return ComWrappersSupport.CreateReferenceCachingFactory(ComWrappersSupport.CreateArrayFactory(implementationType));
				}
				return ComWrappersSupport.CreateFactoryForImplementationType(runtimeClassName, implementationType);
			}
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x000226C4 File Offset: 0x000208C4
		internal static Type GetRuntimeClassForTypeCreation(IInspectable inspectable, Type staticallyDeterminedType)
		{
			string runtimeClassName = inspectable.GetRuntimeClassName(true);
			Type type = null;
			if (!string.IsNullOrEmpty(runtimeClassName))
			{
				type = TypeNameSupport.FindTypeByNameCached(runtimeClassName);
			}
			if (staticallyDeterminedType != null && staticallyDeterminedType != typeof(object) && (!(type != null) || (!(staticallyDeterminedType == type) && !staticallyDeterminedType.IsAssignableFrom(type) && (!staticallyDeterminedType.IsGenericType || !Enumerable.Any<Type>(type.GetInterfaces(), (Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == staticallyDeterminedType.GetGenericTypeDefinition())))))
			{
				return staticallyDeterminedType;
			}
			return type;
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0002276F File Offset: 0x0002096F
		private static bool IsIReferenceType(Type type)
		{
			return ComWrappersSupport.IsIReferenceTypeCache.GetOrAdd(type2, delegate(Type type)
			{
				if (type == typeof(string) || type.IsTypeOfType())
				{
					return true;
				}
				if (type.IsDelegate())
				{
					return ComWrappersSupport.<IsIReferenceType>g__IsIReferenceTypeHelper|27_0(type);
				}
				return type.IsValueType && (type.IsPrimitive || ComWrappersSupport.<IsIReferenceType>g__IsIReferenceTypeHelper|27_0(type));
			});
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x0002279B File Offset: 0x0002099B
		private static bool ShouldProvideIReference(Type type)
		{
			return ComWrappersSupport.IsIReferenceType(type);
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x000227A4 File Offset: 0x000209A4
		private static ComWrappers.ComInterfaceEntry IPropertyValueEntry
		{
			get
			{
				ComWrappers.ComInterfaceEntry result = default(ComWrappers.ComInterfaceEntry);
				result.IID = ManagedIPropertyValueImpl.IID;
				result.Vtable = ManagedIPropertyValueImpl.AbiToProjectionVftablePtr;
				return result;
			}
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x000227D4 File Offset: 0x000209D4
		private static ComWrappers.ComInterfaceEntry ProvideIReference(Type type)
		{
			ComWrappers.ComInterfaceEntry result;
			if (type == typeof(int))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_int.IID;
				result.Vtable = Nullable_int.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type == typeof(string))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_string.IID;
				result.Vtable = Nullable_string.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type == typeof(byte))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_byte.IID;
				result.Vtable = Nullable_byte.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type == typeof(short))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_short.IID;
				result.Vtable = Nullable_short.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type == typeof(ushort))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_ushort.IID;
				result.Vtable = Nullable_ushort.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type == typeof(uint))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_uint.IID;
				result.Vtable = Nullable_uint.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type == typeof(long))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_long.IID;
				result.Vtable = Nullable_long.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type == typeof(ulong))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_ulong.IID;
				result.Vtable = Nullable_ulong.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type == typeof(float))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_float.IID;
				result.Vtable = Nullable_float.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type == typeof(double))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_double.IID;
				result.Vtable = Nullable_double.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type == typeof(char))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_char.IID;
				result.Vtable = Nullable_char.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type == typeof(bool))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_bool.IID;
				result.Vtable = Nullable_bool.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type == typeof(Guid))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_guid.IID;
				result.Vtable = Nullable_guid.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type == typeof(DateTimeOffset))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_DateTimeOffset.IID;
				result.Vtable = Nullable_DateTimeOffset.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type == typeof(TimeSpan))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_TimeSpan.IID;
				result.Vtable = Nullable_TimeSpan.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type == typeof(object))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_Object.IID;
				result.Vtable = Nullable_Object.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type.IsTypeOfType())
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_Type.IID;
				result.Vtable = Nullable_Type.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type == typeof(sbyte))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = Nullable_sbyte.IID;
				result.Vtable = Nullable_sbyte.Vftbl.AbiToProjectionVftablePtr;
				return result;
			}
			if (type.IsDelegate())
			{
				Type type2 = typeof(Nullable_Delegate<>).MakeGenericType(new Type[]
				{
					type
				});
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(type2);
				result.Vtable = type2.GetAbiToProjectionVftblPtr();
				return result;
			}
			result = default(ComWrappers.ComInterfaceEntry);
			result.IID = GuidGenerator.GetIID(typeof(Nullable<>).MakeGenericType(new Type[]
			{
				type
			}));
			result.Vtable = typeof(BoxedValueIReferenceImpl<>).MakeGenericType(new Type[]
			{
				type
			}).GetAbiToProjectionVftblPtr();
			return result;
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00022C18 File Offset: 0x00020E18
		private static bool ShouldProvideIReferenceArray(Type type)
		{
			return type.IsArray && type == type.GetElementType().MakeArrayType() && !type.GetElementType().IsArray;
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x00022C48 File Offset: 0x00020E48
		private static ComWrappers.ComInterfaceEntry ProvideIReferenceArray(Type arrayType)
		{
			Type elementType = arrayType.GetElementType();
			ComWrappers.ComInterfaceEntry result;
			if (elementType == typeof(int))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<int>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<int>.AbiToProjectionVftablePtr;
				return result;
			}
			if (elementType == typeof(string))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<string>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<string>.AbiToProjectionVftablePtr;
				return result;
			}
			if (elementType == typeof(byte))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<byte>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<byte>.AbiToProjectionVftablePtr;
				return result;
			}
			if (elementType == typeof(short))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<short>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<short>.AbiToProjectionVftablePtr;
				return result;
			}
			if (elementType == typeof(ushort))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<ushort>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<ushort>.AbiToProjectionVftablePtr;
				return result;
			}
			if (elementType == typeof(uint))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<uint>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<uint>.AbiToProjectionVftablePtr;
				return result;
			}
			if (elementType == typeof(long))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<long>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<long>.AbiToProjectionVftablePtr;
				return result;
			}
			if (elementType == typeof(ulong))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<ulong>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<ulong>.AbiToProjectionVftablePtr;
				return result;
			}
			if (elementType == typeof(float))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<float>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<float>.AbiToProjectionVftablePtr;
				return result;
			}
			if (elementType == typeof(double))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<double>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<double>.AbiToProjectionVftablePtr;
				return result;
			}
			if (elementType == typeof(char))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<char>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<char>.AbiToProjectionVftablePtr;
				return result;
			}
			if (elementType == typeof(bool))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<bool>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<bool>.AbiToProjectionVftablePtr;
				return result;
			}
			if (elementType == typeof(Guid))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<Guid>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<Guid>.AbiToProjectionVftablePtr;
				return result;
			}
			if (elementType == typeof(DateTimeOffset))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<DateTimeOffset>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<DateTimeOffset>.AbiToProjectionVftablePtr;
				return result;
			}
			if (elementType == typeof(TimeSpan))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<TimeSpan>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<TimeSpan>.AbiToProjectionVftablePtr;
				return result;
			}
			if (elementType == typeof(object))
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<object>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<object>.AbiToProjectionVftablePtr;
				return result;
			}
			if (elementType.IsTypeOfType())
			{
				result = default(ComWrappers.ComInterfaceEntry);
				result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<Type>));
				result.Vtable = BoxedArrayIReferenceArrayImpl<Type>.AbiToProjectionVftablePtr;
				return result;
			}
			result = default(ComWrappers.ComInterfaceEntry);
			result.IID = GuidGenerator.GetIID(typeof(ABI.Windows.Foundation.IReferenceArray<>).MakeGenericType(new Type[]
			{
				elementType
			}));
			result.Vtable = typeof(BoxedArrayIReferenceArrayImpl<>).MakeGenericType(new Type[]
			{
				elementType
			}).GetAbiToProjectionVftblPtr();
			return result;
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x000230C4 File Offset: 0x000212C4
		internal static ObjectReference<T> CreateCCWForObject<T>(object obj, Guid iid)
		{
			IntPtr intPtr = ComWrappersSupport.CreateCCWForObjectForABI(obj, iid);
			return ObjectReference<T>.Attach(ref intPtr);
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x000230E0 File Offset: 0x000212E0
		internal static ObjectReferenceValue CreateCCWForObjectForMarshaling(object obj, Guid iid)
		{
			return new ObjectReferenceValue(ComWrappersSupport.CreateCCWForObjectForABI(obj, iid));
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x000230EE File Offset: 0x000212EE
		private static DefaultComWrappers DefaultComWrappersInstance
		{
			get
			{
				if (ComWrappersSupport._instance == null)
				{
					ComWrappersSupport._instance = new DefaultComWrappers();
				}
				return ComWrappersSupport._instance;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x00023108 File Offset: 0x00021308
		// (set) Token: 0x06000C6B RID: 3179 RVA: 0x00023160 File Offset: 0x00021360
		private static ComWrappers ComWrappers
		{
			get
			{
				if (ComWrappersSupport._comWrappers == null)
				{
					object comWrappersLock = ComWrappersSupport._comWrappersLock;
					lock (comWrappersLock)
					{
						if (ComWrappersSupport._comWrappers == null)
						{
							ComWrappersSupport._comWrappers = ComWrappersSupport.DefaultComWrappersInstance;
						}
					}
				}
				return ComWrappersSupport._comWrappers;
			}
			set
			{
				object comWrappersLock = ComWrappersSupport._comWrappersLock;
				lock (comWrappersLock)
				{
					if (value != null || ComWrappersSupport._comWrappers != ComWrappersSupport.DefaultComWrappersInstance)
					{
						ComWrappersSupport._comWrappers = (value ?? ComWrappersSupport.DefaultComWrappersInstance);
					}
				}
			}
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x000231BC File Offset: 0x000213BC
		internal static ComWrappersSupport.InspectableInfo GetInspectableInfo(IntPtr pThis)
		{
			object obj = ComWrappersSupport.FindObject<object>(pThis);
			return ComWrappersSupport.InspectableInfoTable.GetValue(obj.GetType(), ([Nullable(1)] Type o) => ComWrappersSupport.PregenerateNativeTypeInformation(o).Item1);
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x000231FF File Offset: 0x000213FF
		public static T CreateRcwForComObject<T>(IntPtr ptr)
		{
			return ComWrappersSupport.CreateRcwForComObject<T>(ptr, true);
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x00023208 File Offset: 0x00021408
		private static T CreateRcwForComObject<T>(IntPtr ptr, bool tryUseCache)
		{
			if (ptr == IntPtr.Zero)
			{
				return default(T);
			}
			ComWrappersSupport.CreateRCWType = typeof(T);
			CreateObjectFlags createObjectFlags = tryUseCache ? 1 : 3;
			object orCreateObjectForComInstance = ComWrappersSupport.ComWrappers.GetOrCreateObjectForComInstance(ptr, createObjectFlags);
			ComWrappersSupport.CreateRCWType = null;
			IWinRTObject winRTObject = orCreateObjectForComInstance as IWinRTObject;
			if (winRTObject != null)
			{
				winRTObject.Resurrect();
			}
			object obj = orCreateObjectForComInstance;
			Nullable nullable = obj as Nullable;
			T result;
			if (nullable == null)
			{
				if (obj is T)
				{
					T t = (T)((object)obj);
					result = t;
				}
				else
				{
					if (!tryUseCache)
					{
						throw new ArgumentException(string.Format("Unable to create a wrapper object. The WinRT object {0} has type {1} which cannot be assigned to type {2}", ptr, orCreateObjectForComInstance.GetType(), typeof(T)));
					}
					result = ComWrappersSupport.CreateRcwForComObject<T>(ptr, false);
				}
			}
			else
			{
				result = (T)((object)nullable.Value);
			}
			return result;
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x000232D4 File Offset: 0x000214D4
		public static bool TryUnwrapObject(object o, out IObjectReference objRef)
		{
			Delegate @delegate = o as Delegate;
			if (@delegate != null)
			{
				return ComWrappersSupport.TryUnwrapObject(@delegate.Target, out objRef);
			}
			IWinRTObject winRTObject = o as IWinRTObject;
			if (winRTObject != null && winRTObject.HasUnwrappableNativeObject)
			{
				objRef = winRTObject.NativeObject;
				return true;
			}
			objRef = null;
			return false;
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00023318 File Offset: 0x00021518
		public static void RegisterObjectForInterface(object obj, IntPtr thisPtr, CreateObjectFlags createObjectFlags)
		{
			ComWrappersSupport.ComWrappers.GetOrRegisterObjectForComInstance(thisPtr, createObjectFlags, obj);
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00023328 File Offset: 0x00021528
		public static void RegisterObjectForInterface(object obj, IntPtr thisPtr)
		{
			ComWrappersSupport.TryRegisterObjectForInterface(obj, thisPtr);
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x00023334 File Offset: 0x00021534
		public static object TryRegisterObjectForInterface(object obj, IntPtr thisPtr)
		{
			object orRegisterObjectForComInstance = ComWrappersSupport.ComWrappers.GetOrRegisterObjectForComInstance(thisPtr, 1, obj);
			Delegate @delegate = orRegisterObjectForComInstance as Delegate;
			IWinRTObject winRTObject = ((@delegate != null) ? @delegate.Target : orRegisterObjectForComInstance) as IWinRTObject;
			if (winRTObject != null)
			{
				winRTObject.Resurrect();
			}
			return orRegisterObjectForComInstance;
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00023374 File Offset: 0x00021574
		public static IObjectReference CreateCCWForObject(object obj)
		{
			IntPtr orCreateComInterfaceForObject = ComWrappersSupport.ComWrappers.GetOrCreateComInterfaceForObject(obj, 2);
			return ObjectReference<IUnknownVftbl>.Attach(ref orCreateComInterfaceForObject);
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x00023398 File Offset: 0x00021598
		internal static IntPtr CreateCCWForObjectForABI(object obj, Guid iid)
		{
			IntPtr orCreateComInterfaceForObject = ComWrappersSupport.ComWrappers.GetOrCreateComInterfaceForObject(obj, 2);
			IntPtr result;
			try
			{
				IntPtr intPtr;
				Marshal.ThrowExceptionForHR(Marshal.QueryInterface(orCreateComInterfaceForObject, ref iid, ref intPtr));
				result = intPtr;
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(orCreateComInterfaceForObject);
			}
			return result;
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x000233E0 File Offset: 0x000215E0
		public unsafe static T FindObject<T>(IntPtr ptr) where T : class
		{
			return ComWrappers.ComInterfaceDispatch.GetInstance<T>((ComWrappers.ComInterfaceDispatch*)((void*)ptr));
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x000233ED File Offset: 0x000215ED
		private static T FindDelegate<T>(IntPtr thisPtr) where T : class, Delegate
		{
			return ComWrappersSupport.FindObject<T>(thisPtr);
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x000233F5 File Offset: 0x000215F5
		public static IUnknownVftbl IUnknownVftbl
		{
			get
			{
				return DefaultComWrappers.IUnknownVftbl;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x000233FC File Offset: 0x000215FC
		public static IntPtr IUnknownVftblPtr
		{
			get
			{
				return DefaultComWrappers.IUnknownVftblPtr;
			}
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00023403 File Offset: 0x00021603
		public static IntPtr AllocateVtableMemory(Type vtableType, int size)
		{
			return RuntimeHelpers.AllocateTypeAssociatedMemory(vtableType, size);
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0002340C File Offset: 0x0002160C
		public static void InitializeComWrappers(ComWrappers wrappers = null)
		{
			ComWrappersSupport.ComWrappers = wrappers;
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00023414 File Offset: 0x00021614
		internal static Func<IInspectable, object> GetTypedRcwFactory(Type implementationType)
		{
			return ComWrappersSupport.TypedObjectFactoryCacheForType.GetOrAdd(implementationType, (Type classType) => ComWrappersSupport.CreateTypedRcwFactory(classType, null));
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x00023440 File Offset: 0x00021640
		private static Func<IInspectable, object> CreateFactoryForImplementationType(string runtimeClassName, Type implementationType)
		{
			if (implementationType.IsGenericType)
			{
				Type genericTypeDefinition = implementationType.GetGenericTypeDefinition();
				Type type = null;
				if (genericTypeDefinition == typeof(IList))
				{
					type = typeof(IListImpl<>);
				}
				else if (genericTypeDefinition == typeof(IDictionary))
				{
					type = typeof(IDictionaryImpl<, >);
				}
				else if (genericTypeDefinition == typeof(IReadOnlyDictionary))
				{
					type = typeof(IReadOnlyDictionaryImpl<, >);
				}
				else if (genericTypeDefinition == typeof(IReadOnlyList))
				{
					type = typeof(IReadOnlyListImpl<>);
				}
				else if (genericTypeDefinition == typeof(IEnumerable))
				{
					type = typeof(IEnumerableImpl<>);
				}
				if (type != null)
				{
					return (Func<IInspectable, object>)type.MakeGenericType(implementationType.GetGenericArguments()).GetMethod("CreateRcw", 24, null, new Type[]
					{
						typeof(IInspectable)
					}, null).CreateDelegate(typeof(Func<IInspectable, object>));
				}
			}
			if (implementationType.IsInterface)
			{
				return (IInspectable obj) => obj;
			}
			ConstructorInfo constructor = implementationType.GetConstructor(548, null, new Type[]
			{
				typeof(IObjectReference)
			}, null);
			return delegate(IInspectable obj)
			{
				ConstructorInfo constructor = constructor;
				object[] array = new IObjectReference[]
				{
					obj.ObjRef
				};
				return constructor.Invoke(array);
			};
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x000235FC File Offset: 0x000217FC
		[CompilerGenerated]
		internal static bool <IsIReferenceType>g__IsIReferenceTypeHelper|27_0(Type type)
		{
			if (CustomAttributeExtensions.IsDefined(type, typeof(WindowsRuntimeTypeAttribute)) || Projections.IsTypeWindowsRuntimeType(type))
			{
				return true;
			}
			type = type.GetAuthoringMetadataType();
			return type != null && (CustomAttributeExtensions.IsDefined(type, typeof(WindowsRuntimeTypeAttribute)) || Projections.IsTypeWindowsRuntimeType(type));
		}

		// Token: 0x04000213 RID: 531
		private static readonly ConcurrentDictionary<Type, Func<IInspectable, object>> TypedObjectFactoryCacheForType = new ConcurrentDictionary<Type, Func<IInspectable, object>>();

		// Token: 0x04000214 RID: 532
		private static readonly ConditionalWeakTable<object, object> CCWTable = new ConditionalWeakTable<object, object>();

		// Token: 0x04000215 RID: 533
		private static readonly ConcurrentDictionary<Type, Func<IntPtr, object>> DelegateFactoryCache = new ConcurrentDictionary<Type, Func<IntPtr, object>>();

		// Token: 0x04000216 RID: 534
		private static readonly ConditionalWeakTable<object, IInspectable> _boxedValueReferenceCache = new ConditionalWeakTable<object, IInspectable>();

		// Token: 0x04000217 RID: 535
		private static readonly ConcurrentDictionary<Type, bool> IsIReferenceTypeCache = new ConcurrentDictionary<Type, bool>();

		// Token: 0x04000218 RID: 536
		private static DefaultComWrappers _instance;

		// Token: 0x04000219 RID: 537
		internal static readonly ConditionalWeakTable<Type, ComWrappersSupport.InspectableInfo> InspectableInfoTable = new ConditionalWeakTable<Type, ComWrappersSupport.InspectableInfo>();

		// Token: 0x0400021A RID: 538
		[ThreadStatic]
		internal static Type CreateRCWType;

		// Token: 0x0400021B RID: 539
		private static ComWrappers _comWrappers;

		// Token: 0x0400021C RID: 540
		private static object _comWrappersLock = new object();

		// Token: 0x020003C0 RID: 960
		internal sealed class InspectableInfo
		{
			// Token: 0x170004F5 RID: 1269
			// (get) Token: 0x06001A8E RID: 6798 RVA: 0x00048C78 File Offset: 0x00046E78
			public Guid[] IIDs { get; }

			// Token: 0x170004F6 RID: 1270
			// (get) Token: 0x06001A8F RID: 6799 RVA: 0x00048C80 File Offset: 0x00046E80
			public string RuntimeClassName
			{
				get
				{
					return this.runtimeClassName.Value;
				}
			}

			// Token: 0x06001A90 RID: 6800 RVA: 0x00048C90 File Offset: 0x00046E90
			internal InspectableInfo(Type type, Guid[] iids)
			{
				this.runtimeClassName = new Lazy<string>(() => TypeNameSupport.GetNameForType(type, TypeNameGenerationFlags.GenerateBoxedName | TypeNameGenerationFlags.NoCustomTypeName));
				this.IIDs = iids;
			}

			// Token: 0x04000970 RID: 2416
			private readonly Lazy<string> runtimeClassName;
		}
	}
}
