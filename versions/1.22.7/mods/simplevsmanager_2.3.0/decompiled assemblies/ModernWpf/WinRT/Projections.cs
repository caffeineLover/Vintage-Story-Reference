using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using ABI.System;
using ABI.System.Collections;
using ABI.System.Collections.Generic;
using ABI.System.Collections.Specialized;
using ABI.System.ComponentModel;
using ABI.System.Numerics;
using ABI.System.Windows.Input;
using ABI.WinRT;
using Microsoft.UI.Xaml.Interop;
using Windows.Foundation.Collections;

namespace WinRT
{
	// Token: 0x020001A5 RID: 421
	internal static class Projections
	{
		// Token: 0x06000D98 RID: 3480 RVA: 0x0002848C File Offset: 0x0002668C
		static Projections()
		{
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(bool), typeof(Boolean), "Boolean", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(char), typeof(Char), "Char", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(EventRegistrationToken), typeof(ABI.WinRT.EventRegistrationToken), "Windows.Foundation.EventRegistrationToken", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(Nullable), typeof(Nullable<>), "Windows.Foundation.IReference`1", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(int?), typeof(Nullable_int), "Windows.Foundation.IReference`1<Int32>", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(byte?), typeof(Nullable_byte), "Windows.Foundation.IReference`1<UInt8>", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(sbyte?), typeof(Nullable_sbyte), "Windows.Foundation.IReference`1<Int8>", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(short?), typeof(Nullable_short), "Windows.Foundation.IReference`1<Int16>", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(ushort?), typeof(Nullable_ushort), "Windows.Foundation.IReference`1<UInt16>", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(uint?), typeof(Nullable_uint), "Windows.Foundation.IReference`1<UInt32>", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(long?), typeof(Nullable_long), "Windows.Foundation.IReference`1<Int64>", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(ulong?), typeof(Nullable_ulong), "Windows.Foundation.IReference`1<UInt64>", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(float?), typeof(Nullable_float), "Windows.Foundation.IReference`1<Single>", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(double?), typeof(Nullable_double), "Windows.Foundation.IReference`1<Double>", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(char?), typeof(Nullable_char), "Windows.Foundation.IReference`1<Char16>", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(bool?), typeof(Nullable_bool), "Windows.Foundation.IReference`1<Boolean>", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(Guid?), typeof(Nullable_guid), "Windows.Foundation.IReference`1<Guid>", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(DateTimeOffset?), typeof(Nullable_DateTimeOffset), "Windows.Foundation.IReference`1<Windows.Foundation.DateTime>", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(TimeSpan?), typeof(Nullable_TimeSpan), "Windows.Foundation.IReference`1<TimeSpan>", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(DateTimeOffset), typeof(DateTimeOffset), "Windows.Foundation.DateTime", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(Exception), typeof(Exception), "Windows.Foundation.HResult", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(TimeSpan), typeof(TimeSpan), "Windows.Foundation.TimeSpan", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(Uri), typeof(Uri), "Windows.Foundation.Uri", true);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(DataErrorsChangedEventArgs), typeof(DataErrorsChangedEventArgs), "Microsoft.UI.Xaml.Data.DataErrorsChangedEventArgs", true);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(PropertyChangedEventArgs), typeof(PropertyChangedEventArgs), "Microsoft.UI.Xaml.Data.PropertyChangedEventArgs", true);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(PropertyChangedEventHandler), typeof(PropertyChangedEventHandler), "Microsoft.UI.Xaml.Data.PropertyChangedEventHandler", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(INotifyDataErrorInfo), typeof(INotifyDataErrorInfo), "Microsoft.UI.Xaml.Data.INotifyDataErrorInfo", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(INotifyPropertyChanged), typeof(INotifyPropertyChanged), "Microsoft.UI.Xaml.Data.INotifyPropertyChanged", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(ICommand), typeof(ICommand), "Microsoft.UI.Xaml.Interop.ICommand", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(IServiceProvider), typeof(IServiceProvider), "Microsoft.UI.Xaml.IXamlServiceProvider", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(EventHandler), typeof(EventHandler<>), "Windows.Foundation.EventHandler`1", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(KeyValuePair), typeof(KeyValuePair<, >), "Windows.Foundation.Collections.IKeyValuePair`2", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(IEnumerable), typeof(IEnumerable<>), "Windows.Foundation.Collections.IIterable`1", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(IEnumerator), typeof(IEnumerator<>), "Windows.Foundation.Collections.IIterator`1", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(IList), typeof(IList<>), "Windows.Foundation.Collections.IVector`1", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(IReadOnlyList), typeof(IReadOnlyList<>), "Windows.Foundation.Collections.IVectorView`1", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(IDictionary), typeof(IDictionary<, >), "Windows.Foundation.Collections.IMap`2", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(IReadOnlyDictionary), typeof(IReadOnlyDictionary<, >), "Windows.Foundation.Collections.IMapView`2", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(IDisposable), typeof(IDisposable), "Windows.Foundation.IClosable", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(IEnumerable), typeof(IEnumerable), "Microsoft.UI.Xaml.Interop.IBindableIterable", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(IList), typeof(IList), "Microsoft.UI.Xaml.Interop.IBindableVector", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(INotifyCollectionChanged), typeof(INotifyCollectionChanged), "Microsoft.UI.Xaml.Interop.INotifyCollectionChanged", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(NotifyCollectionChangedAction), typeof(NotifyCollectionChangedAction), "Microsoft.UI.Xaml.Interop.NotifyCollectionChangedAction", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(NotifyCollectionChangedEventArgs), typeof(NotifyCollectionChangedEventArgs), "Microsoft.UI.Xaml.Interop.NotifyCollectionChangedEventArgs", true);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(NotifyCollectionChangedEventHandler), typeof(NotifyCollectionChangedEventHandler), "Microsoft.UI.Xaml.Interop.NotifyCollectionChangedEventHandler", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(Matrix3x2), typeof(Matrix3x2), "Windows.Foundation.Numerics.Matrix3x2", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(Matrix4x4), typeof(Matrix4x4), "Windows.Foundation.Numerics.Matrix4x4", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(Plane), typeof(Plane), "Windows.Foundation.Numerics.Plane", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(Quaternion), typeof(Quaternion), "Windows.Foundation.Numerics.Quaternion", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(Vector2), typeof(Vector2), "Windows.Foundation.Numerics.Vector2", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(Vector3), typeof(Vector3), "Windows.Foundation.Numerics.Vector3", false);
			Projections.RegisterCustomAbiTypeMappingNoLock(typeof(Vector4), typeof(Vector4), "Windows.Foundation.Numerics.Vector4", false);
			Projections.CustomTypeToHelperTypeMappings.Add(typeof(IMap<, >), typeof(IDictionary<, >));
			Projections.CustomTypeToHelperTypeMappings.Add(typeof(IVector<>), typeof(IList<>));
			Projections.CustomTypeToHelperTypeMappings.Add(typeof(IMapView<, >), typeof(IReadOnlyDictionary<, >));
			Projections.CustomTypeToHelperTypeMappings.Add(typeof(IVectorView<>), typeof(IReadOnlyList<>));
			Projections.CustomTypeToHelperTypeMappings.Add(typeof(IBindableVector), typeof(IList));
			Projections.CustomTypeToAbiTypeNameMappings.Add(typeof(Type), "Windows.UI.Xaml.Interop.TypeName");
		}

		// Token: 0x06000D99 RID: 3481 RVA: 0x00028BF8 File Offset: 0x00026DF8
		public static void RegisterCustomAbiTypeMapping(Type publicType, Type abiType, string winrtTypeName, bool isRuntimeClass = false)
		{
			Projections.rwlock.EnterWriteLock();
			try
			{
				Projections.RegisterCustomAbiTypeMappingNoLock(publicType, abiType, winrtTypeName, isRuntimeClass);
			}
			finally
			{
				Projections.rwlock.ExitWriteLock();
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00028C38 File Offset: 0x00026E38
		private static void RegisterCustomAbiTypeMappingNoLock(Type publicType, Type abiType, string winrtTypeName, bool isRuntimeClass = false)
		{
			Projections.CustomTypeToHelperTypeMappings.Add(publicType, abiType);
			Projections.CustomAbiTypeToTypeMappings.Add(abiType, publicType);
			Projections.CustomTypeToAbiTypeNameMappings.Add(publicType, winrtTypeName);
			Projections.CustomAbiTypeNameToTypeMappings.Add(winrtTypeName, publicType);
			if (isRuntimeClass)
			{
				Projections.ProjectedRuntimeClassNames.Add(winrtTypeName);
				Projections.ProjectedCustomTypeRuntimeClasses.Add(publicType);
			}
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x00028C90 File Offset: 0x00026E90
		public static Type FindCustomHelperTypeMapping(Type publicType, bool filterToRuntimeClass = false)
		{
			Projections.rwlock.EnterReadLock();
			Type result;
			try
			{
				if (filterToRuntimeClass && !Projections.ProjectedCustomTypeRuntimeClasses.Contains(publicType))
				{
					result = null;
				}
				else if (publicType.IsGenericType)
				{
					Type type;
					if (Projections.CustomTypeToHelperTypeMappings.TryGetValue(publicType, ref type))
					{
						result = type;
					}
					else
					{
						Type type2;
						result = (Projections.CustomTypeToHelperTypeMappings.TryGetValue(publicType.GetGenericTypeDefinition(), ref type2) ? type2.MakeGenericType(publicType.GetGenericArguments()) : null);
					}
				}
				else
				{
					Type type3;
					result = (Projections.CustomTypeToHelperTypeMappings.TryGetValue(publicType, ref type3) ? type3 : null);
				}
			}
			finally
			{
				Projections.rwlock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x00028D30 File Offset: 0x00026F30
		public static Type FindCustomPublicTypeForAbiType(Type abiType)
		{
			Projections.rwlock.EnterReadLock();
			Type result;
			try
			{
				if (abiType.IsGenericType)
				{
					Type type;
					if (Projections.CustomAbiTypeToTypeMappings.TryGetValue(abiType, ref type))
					{
						result = type;
					}
					else
					{
						Type type2;
						result = (Projections.CustomAbiTypeToTypeMappings.TryGetValue(abiType.GetGenericTypeDefinition(), ref type2) ? type2.MakeGenericType(abiType.GetGenericArguments()) : null);
					}
				}
				else
				{
					Type type3;
					result = (Projections.CustomAbiTypeToTypeMappings.TryGetValue(abiType, ref type3) ? type3 : null);
				}
			}
			finally
			{
				Projections.rwlock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00028DBC File Offset: 0x00026FBC
		public static Type FindCustomTypeForAbiTypeName(string abiTypeName)
		{
			Projections.rwlock.EnterReadLock();
			Type result;
			try
			{
				Type type;
				result = (Projections.CustomAbiTypeNameToTypeMappings.TryGetValue(abiTypeName, ref type) ? type : null);
			}
			finally
			{
				Projections.rwlock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x06000D9E RID: 3486 RVA: 0x00028E08 File Offset: 0x00027008
		public static string FindCustomAbiTypeNameForType(Type type)
		{
			Projections.rwlock.EnterReadLock();
			string result;
			try
			{
				string text;
				result = (Projections.CustomTypeToAbiTypeNameMappings.TryGetValue(type, ref text) ? text : null);
			}
			finally
			{
				Projections.rwlock.ExitReadLock();
			}
			return result;
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x00028E54 File Offset: 0x00027054
		public static bool IsTypeWindowsRuntimeType(Type type)
		{
			return Projections.IsTypeWindowsRuntimeTypeCache.GetOrAdd(type2, delegate(Type type)
			{
				Type type3 = type;
				if (type3.IsArray)
				{
					type3 = type3.GetElementType();
				}
				return Projections.IsTypeWindowsRuntimeTypeNoArray(type3);
			});
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x00028E80 File Offset: 0x00027080
		private static bool IsTypeWindowsRuntimeTypeNoArray(Type type)
		{
			type = (type.GetAuthoringMetadataType() ?? type);
			if (!type.IsConstructedGenericType)
			{
				return Projections.CustomTypeToAbiTypeNameMappings.ContainsKey(type) || type.IsPrimitive || type == typeof(string) || type == typeof(Guid) || type == typeof(object) || CustomAttributeExtensions.IsDefined(type, typeof(WindowsRuntimeTypeAttribute));
			}
			if (Projections.IsTypeWindowsRuntimeTypeNoArray(type.GetGenericTypeDefinition()))
			{
				Type[] genericArguments = type.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					if (!Projections.IsTypeWindowsRuntimeTypeNoArray(genericArguments[i]))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x00028F34 File Offset: 0x00027134
		public static bool TryGetCompatibleWindowsRuntimeTypeForVariantType(Type type, out Type compatibleType)
		{
			compatibleType = null;
			if (!type.IsConstructedGenericType)
			{
				throw new ArgumentException("type");
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			if (!Projections.IsTypeWindowsRuntimeTypeNoArray(genericTypeDefinition))
			{
				return false;
			}
			Type[] genericArguments = genericTypeDefinition.GetGenericArguments();
			Type[] genericArguments2 = type.GetGenericArguments();
			Type[] array = new Type[genericArguments2.Length];
			for (int i = 0; i < genericArguments2.Length; i++)
			{
				if (!Projections.IsTypeWindowsRuntimeTypeNoArray(genericArguments2[i]))
				{
					if ((genericArguments[i].GenericParameterAttributes & 3) != 1 || genericArguments2[i].IsValueType)
					{
						return false;
					}
					array[i] = typeof(object);
				}
				else
				{
					array[i] = genericArguments2[i];
				}
			}
			compatibleType = genericTypeDefinition.MakeGenericType(array);
			return true;
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x00028FDC File Offset: 0x000271DC
		private static HashSet<Type> GetCompatibleTypes(Type type)
		{
			HashSet<Type> hashSet = new HashSet<Type>();
			foreach (Type type2 in type.GetInterfaces())
			{
				if (Projections.IsTypeWindowsRuntimeTypeNoArray(type2))
				{
					hashSet.Add(type2);
				}
				IEnumerable<Type> enumerable;
				if (type2.IsConstructedGenericType && Projections.TryGetCompatibleWindowsRuntimeTypesForVariantType(type2, out enumerable))
				{
					hashSet.UnionWith(enumerable);
				}
			}
			Type baseType = type.BaseType;
			while (baseType != null)
			{
				if (Projections.IsTypeWindowsRuntimeTypeNoArray(baseType))
				{
					hashSet.Add(baseType);
				}
				baseType = baseType.BaseType;
			}
			return hashSet;
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x00029064 File Offset: 0x00027264
		internal static IEnumerable<Type> GetAllPossibleTypeCombinations(IEnumerable<IEnumerable<Type>> compatibleTypesPerGeneric, Type definition)
		{
			Projections.<>c__DisplayClass19_0 CS$<>8__locals1;
			CS$<>8__locals1.definition = definition;
			List<Type> list = new List<Type>();
			IEnumerable<Type>[] array = Enumerable.ToArray<IEnumerable<Type>>(compatibleTypesPerGeneric);
			if (array.Length != 0)
			{
				Projections.<GetAllPossibleTypeCombinations>g__GetAllPossibleTypeCombinationsCore|19_0(list, new Stack<Type>(), array, array.Length - 1, ref CS$<>8__locals1);
			}
			return list;
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x000290A0 File Offset: 0x000272A0
		internal static bool TryGetCompatibleWindowsRuntimeTypesForVariantType(Type type, out IEnumerable<Type> compatibleTypes)
		{
			compatibleTypes = null;
			if (!type.IsConstructedGenericType)
			{
				throw new ArgumentException("type");
			}
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			if (!Projections.IsTypeWindowsRuntimeTypeNoArray(genericTypeDefinition))
			{
				return false;
			}
			Type[] genericArguments = genericTypeDefinition.GetGenericArguments();
			Type[] genericArguments2 = type.GetGenericArguments();
			List<List<Type>> list = new List<List<Type>>();
			for (int i = 0; i < genericArguments2.Length; i++)
			{
				List<Type> list2 = new List<Type>();
				bool flag = (genericArguments[i].GenericParameterAttributes & 3) == 1 && !genericArguments2[i].IsValueType;
				if (Projections.IsTypeWindowsRuntimeTypeNoArray(genericArguments2[i]))
				{
					list2.Add(genericArguments2[i]);
				}
				else if (!flag)
				{
					return false;
				}
				if (flag)
				{
					list2.AddRange(Projections.GetCompatibleTypes(genericArguments2[i]));
				}
				list.Add(list2);
			}
			compatibleTypes = Projections.GetAllPossibleTypeCombinations(list, genericTypeDefinition);
			return true;
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x00029164 File Offset: 0x00027364
		internal static bool TryGetDefaultInterfaceTypeForRuntimeClassType(Type runtimeClass, out Type defaultInterface)
		{
			defaultInterface = Projections.DefaultInterfaceTypeCache.GetOrAdd(runtimeClass2, delegate(Type runtimeClass)
			{
				runtimeClass = (runtimeClass.GetRuntimeClassCCWType() ?? runtimeClass);
				ProjectedRuntimeClassAttribute customAttribute = CustomAttributeExtensions.GetCustomAttribute<ProjectedRuntimeClassAttribute>(runtimeClass);
				if (customAttribute == null)
				{
					return null;
				}
				if (customAttribute.DefaultInterfaceProperty != null)
				{
					return runtimeClass.GetProperty(customAttribute.DefaultInterfaceProperty, 54).PropertyType;
				}
				return customAttribute.DefaultInterface;
			});
			return defaultInterface != null;
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0002919C File Offset: 0x0002739C
		internal static Type GetDefaultInterfaceTypeForRuntimeClassType(Type runtimeClass)
		{
			Type result;
			if (!Projections.TryGetDefaultInterfaceTypeForRuntimeClassType(runtimeClass, out result))
			{
				throw new ArgumentException("The provided type '" + runtimeClass.FullName + "' is not a WinRT projected runtime class.", "runtimeClass");
			}
			return result;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x000291D4 File Offset: 0x000273D4
		internal static bool TryGetMarshalerTypeForProjectedRuntimeClass<T>(IObjectReference objectReference, out Type type)
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle == typeof(object))
			{
				ObjectReference<IInspectable.Vftbl> objectReference2;
				if (objectReference.TryAs<IInspectable.Vftbl>(InterfaceIIDs.IInspectable_IID, out objectReference2) == 0)
				{
					Projections.rwlock.EnterReadLock();
					try
					{
						string runtimeClassName = objectReference2.GetRuntimeClassName(true);
						if (runtimeClassName != null && Projections.ProjectedRuntimeClassNames.Contains(runtimeClassName))
						{
							type = Projections.CustomTypeToHelperTypeMappings[Projections.CustomAbiTypeNameToTypeMappings[runtimeClassName]];
							return true;
						}
						goto IL_93;
					}
					finally
					{
						objectReference2.Dispose();
						Projections.rwlock.ExitReadLock();
					}
					goto IL_81;
				}
				IL_93:
				type = null;
				return false;
			}
			IL_81:
			type = Projections.FindCustomHelperTypeMapping(typeFromHandle, true);
			return type != null;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0002928C File Offset: 0x0002748C
		[CompilerGenerated]
		internal static void <GetAllPossibleTypeCombinations>g__GetAllPossibleTypeCombinationsCore|19_0(List<Type> accum, Stack<Type> stack, IEnumerable<Type>[] compatibleTypes, int index, ref Projections.<>c__DisplayClass19_0 A_4)
		{
			foreach (Type type in compatibleTypes[index])
			{
				stack.Push(type);
				if (index == 0)
				{
					accum.Add(A_4.definition.MakeGenericType(stack.ToArray()));
				}
				else
				{
					Projections.<GetAllPossibleTypeCombinations>g__GetAllPossibleTypeCombinationsCore|19_0(accum, stack, compatibleTypes, index - 1, ref A_4);
				}
				stack.Pop();
			}
		}

		// Token: 0x0400028E RID: 654
		private static readonly ReaderWriterLockSlim rwlock = new ReaderWriterLockSlim();

		// Token: 0x0400028F RID: 655
		private static readonly Dictionary<Type, Type> CustomTypeToHelperTypeMappings = new Dictionary<Type, Type>();

		// Token: 0x04000290 RID: 656
		private static readonly Dictionary<Type, Type> CustomAbiTypeToTypeMappings = new Dictionary<Type, Type>();

		// Token: 0x04000291 RID: 657
		private static readonly Dictionary<string, Type> CustomAbiTypeNameToTypeMappings = new Dictionary<string, Type>(StringComparer.Ordinal);

		// Token: 0x04000292 RID: 658
		private static readonly Dictionary<Type, string> CustomTypeToAbiTypeNameMappings = new Dictionary<Type, string>();

		// Token: 0x04000293 RID: 659
		private static readonly HashSet<string> ProjectedRuntimeClassNames = new HashSet<string>(StringComparer.Ordinal);

		// Token: 0x04000294 RID: 660
		private static readonly HashSet<Type> ProjectedCustomTypeRuntimeClasses = new HashSet<Type>();

		// Token: 0x04000295 RID: 661
		private static readonly ConcurrentDictionary<Type, bool> IsTypeWindowsRuntimeTypeCache = new ConcurrentDictionary<Type, bool>();

		// Token: 0x04000296 RID: 662
		private static readonly ConcurrentDictionary<Type, Type> DefaultInterfaceTypeCache = new ConcurrentDictionary<Type, Type>();
	}
}
