using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using ABI.System;

namespace WinRT
{
	// Token: 0x020001A9 RID: 425
	internal static class TypeNameSupport
	{
		// Token: 0x06000DC1 RID: 3521 RVA: 0x0002961E File Offset: 0x0002781E
		public static void RegisterProjectionAssembly(Assembly assembly)
		{
			TypeNameSupport.projectionAssemblies.Add(assembly);
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0002962B File Offset: 0x0002782B
		public static Type FindTypeByNameCached(string runtimeClassName)
		{
			return TypeNameSupport.typeNameCache.GetOrAdd(runtimeClassName2, delegate(string runtimeClassName)
			{
				Type result = null;
				try
				{
					result = TypeNameSupport.FindTypeByName(MemoryExtensions.AsSpan(runtimeClassName)).Item1;
				}
				catch (Exception)
				{
				}
				return result;
			});
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x00029658 File Offset: 0x00027858
		[return: TupleElementNames(new string[]
		{
			"type",
			"remaining"
		})]
		public static ValueTuple<Type, int> FindTypeByName(ReadOnlySpan<char> runtimeClassName)
		{
			if (MemoryExtensions.StartsWith(runtimeClassName, MemoryExtensions.AsSpan("<>f__AnonymousType"), 4))
			{
				return new ValueTuple<Type, int>(typeof(ExpandoObject), 0);
			}
			if (MemoryExtensions.CompareTo(runtimeClassName, MemoryExtensions.AsSpan("Windows.Foundation.IReference`1<String>"), 4) == 0)
			{
				return new ValueTuple<Type, int>(typeof(Nullable_string), 0);
			}
			if (MemoryExtensions.CompareTo(runtimeClassName, MemoryExtensions.AsSpan("Windows.Foundation.IReference`1<Windows.UI.Xaml.Interop.TypeName>"), 4) == 0)
			{
				return new ValueTuple<Type, int>(typeof(Nullable_Type), 0);
			}
			ValueTuple<string, Type[], int> valueTuple = TypeNameSupport.ParseGenericTypeName(runtimeClassName);
			string item = valueTuple.Item1;
			Type[] item2 = valueTuple.Item2;
			int item3 = valueTuple.Item3;
			return new ValueTuple<Type, int>(TypeNameSupport.FindTypeByNameCore(item, item2), item3);
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x000296F8 File Offset: 0x000278F8
		private static Type FindTypeByNameCore(string runtimeClassName, Type[] genericTypes)
		{
			Type type = Projections.FindCustomTypeForAbiTypeName(runtimeClassName);
			if (type == null)
			{
				if (genericTypes == null)
				{
					Type type2 = TypeNameSupport.ResolvePrimitiveType(runtimeClassName);
					if (type2 != null)
					{
						return type2;
					}
				}
				foreach (Assembly assembly in TypeNameSupport.projectionAssemblies)
				{
					Type type3 = assembly.GetType(runtimeClassName);
					if (type3 != null)
					{
						type = type3;
						break;
					}
				}
			}
			if (type == null)
			{
				Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
				for (int i = 0; i < assemblies.Length; i++)
				{
					Type type4 = assemblies[i].GetType(runtimeClassName);
					if (type4 != null)
					{
						type = type4;
						break;
					}
				}
			}
			if (type != null)
			{
				if (genericTypes != null)
				{
					if (type == typeof(Nullable) && genericTypes[0].IsDelegate())
					{
						return typeof(Nullable_Delegate<>).MakeGenericType(genericTypes);
					}
					type = type.MakeGenericType(genericTypes);
				}
				return type;
			}
			throw new TypeLoadException("Unable to find a type named '" + runtimeClassName + "'");
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x000297F4 File Offset: 0x000279F4
		public static Type ResolvePrimitiveType(string primitiveTypeName)
		{
			uint num = <PrivateImplementationDetails>.ComputeStringHash(primitiveTypeName);
			if (num <= 1615808600U)
			{
				if (num <= 765439473U)
				{
					if (num <= 697196164U)
					{
						if (num != 679076413U)
						{
							if (num == 697196164U)
							{
								if (primitiveTypeName == "Int64")
								{
									return typeof(long);
								}
							}
						}
						else if (primitiveTypeName == "Char")
						{
							return typeof(char);
						}
					}
					else if (num != 701452950U)
					{
						if (num == 765439473U)
						{
							if (primitiveTypeName == "Int16")
							{
								return typeof(short);
							}
						}
					}
					else if (primitiveTypeName == "Char16")
					{
						return typeof(char);
					}
				}
				else if (num <= 1323747186U)
				{
					if (num != 1059919963U)
					{
						if (num == 1323747186U)
						{
							if (primitiveTypeName == "UInt16")
							{
								return typeof(ushort);
							}
						}
					}
					else if (primitiveTypeName == "UInt8")
					{
						return typeof(byte);
					}
				}
				else if (num != 1324880019U)
				{
					if (num == 1615808600U)
					{
						if (primitiveTypeName == "String")
						{
							return typeof(string);
						}
					}
				}
				else if (primitiveTypeName == "UInt64")
				{
					return typeof(ulong);
				}
			}
			else if (num <= 2898774828U)
			{
				if (num <= 2417551388U)
				{
					if (num != 2386971688U)
					{
						if (num == 2417551388U)
						{
							if (primitiveTypeName == "TimeSpan")
							{
								return typeof(TimeSpan);
							}
						}
					}
					else if (primitiveTypeName == "Double")
					{
						return typeof(double);
					}
				}
				else if (num != 2711245919U)
				{
					if (num == 2898774828U)
					{
						if (primitiveTypeName == "Guid")
						{
							return typeof(Guid);
						}
					}
				}
				else if (primitiveTypeName == "Int32")
				{
					return typeof(int);
				}
			}
			else if (num <= 3837675186U)
			{
				if (num != 3538687084U)
				{
					if (num == 3837675186U)
					{
						if (primitiveTypeName == "Int8")
						{
							return typeof(sbyte);
						}
					}
				}
				else if (primitiveTypeName == "UInt32")
				{
					return typeof(uint);
				}
			}
			else if (num != 3851314394U)
			{
				if (num != 3969205087U)
				{
					if (num == 4051133705U)
					{
						if (primitiveTypeName == "Single")
						{
							return typeof(float);
						}
					}
				}
				else if (primitiveTypeName == "Boolean")
				{
					return typeof(bool);
				}
			}
			else if (primitiveTypeName == "Object")
			{
				return typeof(object);
			}
			return null;
		}

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00029B80 File Offset: 0x00027D80
		[return: TupleElementNames(new string[]
		{
			"genericTypeName",
			"genericTypes",
			"remaining"
		})]
		private unsafe static ValueTuple<string, Type[], int> ParseGenericTypeName(ReadOnlySpan<char> partialTypeName)
		{
			int num = MemoryExtensions.IndexOfAny<char>(partialTypeName, ',', '>');
			int num2 = partialTypeName.Length;
			if (num != -1)
			{
				num2 = num;
			}
			ReadOnlySpan<char> readOnlySpan = partialTypeName.Slice(0, num2);
			if (!MemoryExtensions.Contains(readOnlySpan, MemoryExtensions.AsSpan("`"), 4))
			{
				return new ValueTuple<string, Type[], int>(readOnlySpan.ToString(), null, num2);
			}
			int num3 = MemoryExtensions.IndexOf<char>(partialTypeName, '<');
			ReadOnlySpan<char> readOnlySpan2 = partialTypeName.Slice(0, num3);
			ReadOnlySpan<char> runtimeClassName = partialTypeName.Slice(num3 + 1);
			int num4 = num3 + 1;
			List<Type> list = new List<Type>();
			for (;;)
			{
				ValueTuple<Type, int> valueTuple = TypeNameSupport.FindTypeByName(runtimeClassName);
				Type item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				num4 += item2;
				list.Add(item);
				runtimeClassName = runtimeClassName.Slice(item2);
				if (*runtimeClassName[0] != 44)
				{
					break;
				}
				num4 += 2;
				runtimeClassName = runtimeClassName.Slice(2);
			}
			if (*runtimeClassName[0] == 62)
			{
				int num5 = (runtimeClassName.Length > 1 && *runtimeClassName[1] == 32) ? 2 : 1;
				num4 += num5;
				runtimeClassName = runtimeClassName.Slice(num5);
				return new ValueTuple<string, Type[], int>(readOnlySpan2.ToString(), list.ToArray(), partialTypeName.Length - runtimeClassName.Length);
			}
			throw new InvalidOperationException("The provided type name is invalid.");
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x00029CC8 File Offset: 0x00027EC8
		public static string GetNameForType(Type type, TypeNameGenerationFlags flags)
		{
			if (type == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (TypeNameSupport.TryAppendTypeName(type, stringBuilder, flags))
			{
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x00029CF8 File Offset: 0x00027EF8
		private static bool TryAppendSimpleTypeName(Type type, StringBuilder builder, TypeNameGenerationFlags flags)
		{
			if (type.IsPrimitive || type == typeof(string) || type == typeof(Guid) || type == typeof(TimeSpan))
			{
				if ((flags & TypeNameGenerationFlags.GenerateBoxedName) != TypeNameGenerationFlags.None)
				{
					builder.Append("Windows.Foundation.IReference`1<");
					if (!TypeNameSupport.TryAppendSimpleTypeName(type, builder, flags & ~TypeNameGenerationFlags.GenerateBoxedName))
					{
						return false;
					}
					builder.Append('>');
					return true;
				}
				else if (type == typeof(byte))
				{
					builder.Append("UInt8");
				}
				else if (type == typeof(sbyte))
				{
					builder.Append("Int8");
				}
				else
				{
					builder.Append(type.Name);
				}
			}
			else if (type == typeof(object))
			{
				builder.Append("Object");
			}
			else
			{
				string text = Projections.FindCustomAbiTypeNameForType(type);
				if (text != null)
				{
					builder.Append(text);
				}
				else if (Projections.IsTypeWindowsRuntimeType(type))
				{
					builder.Append(type.FullName);
				}
				else
				{
					if ((flags & TypeNameGenerationFlags.NoCustomTypeName) != TypeNameGenerationFlags.None)
					{
						return TypeNameSupport.TryAppendWinRTInterfaceNameForType(type, builder, flags);
					}
					builder.Append(type.FullName);
				}
			}
			return true;
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x00029E28 File Offset: 0x00028028
		private static bool TryAppendWinRTInterfaceNameForType(Type type, StringBuilder builder, TypeNameGenerationFlags flags)
		{
			Stack<TypeNameSupport.VisitedType> value = TypeNameSupport.VisitedTypes.Value;
			if (!Enumerable.Any<TypeNameSupport.VisitedType>(value, (TypeNameSupport.VisitedType visited) => visited.Type == type))
			{
				value.Push(new TypeNameSupport.VisitedType
				{
					Type = type
				});
				Type type2 = null;
				foreach (Type type3 in type.GetInterfaces())
				{
					if (Projections.IsTypeWindowsRuntimeType(type3) && (type2 == null || type3.IsAssignableFrom(type2)))
					{
						type2 = type3;
					}
				}
				bool result = false;
				if (type2 != null)
				{
					result = TypeNameSupport.TryAppendTypeName(type2, builder, flags);
				}
				value.Pop();
				return result;
			}
			if (value.Peek().Covariant && !type.IsValueType)
			{
				builder.Append("Object");
				return true;
			}
			return false;
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x00029F08 File Offset: 0x00028108
		private static bool TryAppendTypeName(Type type, StringBuilder builder, TypeNameGenerationFlags flags)
		{
			if (type.IsSZArray)
			{
				builder.Append("Windows.Foundation.IReferenceArray`1<");
				if (TypeNameSupport.TryAppendTypeName(type.GetElementType(), builder, flags & ~TypeNameGenerationFlags.GenerateBoxedName))
				{
					builder.Append('>');
					return true;
				}
				return true;
			}
			else
			{
				if (!type.IsGenericType || type.IsGenericTypeDefinition)
				{
					return TypeNameSupport.TryAppendSimpleTypeName(type, builder, flags);
				}
				if ((flags & TypeNameGenerationFlags.NoCustomTypeName) != TypeNameGenerationFlags.None && !Projections.IsTypeWindowsRuntimeType(type))
				{
					return TypeNameSupport.TryAppendWinRTInterfaceNameForType(type, builder, flags);
				}
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (!TypeNameSupport.TryAppendSimpleTypeName(genericTypeDefinition, builder, flags))
				{
					return false;
				}
				builder.Append('<');
				bool flag = true;
				Type[] genericArguments = type.GetGenericArguments();
				Type[] genericArguments2 = genericTypeDefinition.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					Type type2 = genericArguments[i];
					if (type2.ContainsGenericParameters)
					{
						throw new ArgumentException("type");
					}
					if (!flag)
					{
						builder.Append(", ");
					}
					flag = false;
					if ((flags & TypeNameGenerationFlags.NoCustomTypeName) != TypeNameGenerationFlags.None)
					{
						TypeNameSupport.VisitedTypes.Value.Push(new TypeNameSupport.VisitedType
						{
							Type = type,
							Covariant = ((genericArguments2[i].GenericParameterAttributes & 3) == 1)
						});
					}
					bool flag2 = TypeNameSupport.TryAppendTypeName(type2, builder, flags & ~TypeNameGenerationFlags.GenerateBoxedName);
					if ((flags & TypeNameGenerationFlags.NoCustomTypeName) != TypeNameGenerationFlags.None)
					{
						TypeNameSupport.VisitedTypes.Value.Pop();
					}
					if (!flag2)
					{
						return false;
					}
				}
				builder.Append('>');
				return true;
			}
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0002A04C File Offset: 0x0002824C
		// Note: this type is marked as 'beforefieldinit'.
		static TypeNameSupport()
		{
			ConcurrentDictionary<string, Type> concurrentDictionary = new ConcurrentDictionary<string, Type>(StringComparer.Ordinal);
			concurrentDictionary["TrackerCollection<T>"] = null;
			TypeNameSupport.typeNameCache = concurrentDictionary;
			TypeNameSupport.VisitedTypes = new ThreadLocal<Stack<TypeNameSupport.VisitedType>>(() => new Stack<TypeNameSupport.VisitedType>());
		}

		// Token: 0x040002A1 RID: 673
		private static readonly List<Assembly> projectionAssemblies = new List<Assembly>();

		// Token: 0x040002A2 RID: 674
		private static readonly ConcurrentDictionary<string, Type> typeNameCache;

		// Token: 0x040002A3 RID: 675
		private static readonly ThreadLocal<Stack<TypeNameSupport.VisitedType>> VisitedTypes;

		// Token: 0x020003F8 RID: 1016
		private struct VisitedType
		{
			// Token: 0x170004FD RID: 1277
			// (get) Token: 0x06001B73 RID: 7027 RVA: 0x00049ECA File Offset: 0x000480CA
			// (set) Token: 0x06001B74 RID: 7028 RVA: 0x00049ED2 File Offset: 0x000480D2
			public Type Type { readonly get; set; }

			// Token: 0x170004FE RID: 1278
			// (get) Token: 0x06001B75 RID: 7029 RVA: 0x00049EDB File Offset: 0x000480DB
			// (set) Token: 0x06001B76 RID: 7030 RVA: 0x00049EE3 File Offset: 0x000480E3
			public bool Covariant { readonly get; set; }
		}
	}
}
