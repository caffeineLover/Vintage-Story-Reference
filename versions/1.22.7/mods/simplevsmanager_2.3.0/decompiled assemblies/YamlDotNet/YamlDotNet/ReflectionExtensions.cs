using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace YamlDotNet
{
	// Token: 0x02000005 RID: 5
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ReflectionExtensions
	{
		// Token: 0x06000004 RID: 4 RVA: 0x0000207D File Offset: 0x0000027D
		[return: Nullable(2)]
		public static Type BaseType(this Type type)
		{
			return IntrospectionExtensions.GetTypeInfo(type).BaseType;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000208A File Offset: 0x0000028A
		public static bool IsValueType(this Type type)
		{
			return IntrospectionExtensions.GetTypeInfo(type).IsValueType;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002097 File Offset: 0x00000297
		public static bool IsGenericType(this Type type)
		{
			return IntrospectionExtensions.GetTypeInfo(type).IsGenericType;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020A4 File Offset: 0x000002A4
		public static bool IsGenericTypeDefinition(this Type type)
		{
			return IntrospectionExtensions.GetTypeInfo(type).IsGenericTypeDefinition;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000020B4 File Offset: 0x000002B4
		[return: Nullable(2)]
		public static Type GetImplementationOfOpenGenericInterface(this Type type, Type openGenericType)
		{
			if (!openGenericType.IsGenericType || !openGenericType.IsInterface)
			{
				throw new ArgumentException("The type must be a generic type definition and an interface", "openGenericType");
			}
			if (ReflectionExtensions.<GetImplementationOfOpenGenericInterface>g__IsGenericDefinitionOfType|4_1(type, openGenericType))
			{
				return type;
			}
			return Enumerable.FirstOrDefault<Type>(type.FindInterfaces((Type t, [Nullable(2)] object context) => ReflectionExtensions.<GetImplementationOfOpenGenericInterface>g__IsGenericDefinitionOfType|4_1(t, context), openGenericType));
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002117 File Offset: 0x00000317
		public static bool IsInterface(this Type type)
		{
			return IntrospectionExtensions.GetTypeInfo(type).IsInterface;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002124 File Offset: 0x00000324
		public static bool IsEnum(this Type type)
		{
			return IntrospectionExtensions.GetTypeInfo(type).IsEnum;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002134 File Offset: 0x00000334
		public static bool IsRequired(this MemberInfo member)
		{
			return Enumerable.Any<RequiredMemberAttribute>(CustomAttributeExtensions.GetCustomAttributes<RequiredMemberAttribute>(member));
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002150 File Offset: 0x00000350
		public static bool HasDefaultConstructor(this Type type, bool allowPrivateConstructors)
		{
			BindingFlags bindingFlags = 20;
			if (allowPrivateConstructors)
			{
				bindingFlags |= 32;
			}
			return type.IsValueType || type.GetConstructor(bindingFlags, null, Type.EmptyTypes, null) != null;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002186 File Offset: 0x00000386
		public static bool IsAssignableFrom(this Type type, Type source)
		{
			return type.IsAssignableFrom(IntrospectionExtensions.GetTypeInfo(source));
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002194 File Offset: 0x00000394
		public static bool IsAssignableFrom(this Type type, TypeInfo source)
		{
			return IntrospectionExtensions.GetTypeInfo(type).IsAssignableFrom(source);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000021A4 File Offset: 0x000003A4
		public static TypeCode GetTypeCode(this Type type)
		{
			bool flag = type.IsEnum();
			if (flag)
			{
				type = Enum.GetUnderlyingType(type);
			}
			if (type == typeof(bool))
			{
				return 3;
			}
			if (type == typeof(char))
			{
				return 4;
			}
			if (type == typeof(sbyte))
			{
				return 5;
			}
			if (type == typeof(byte))
			{
				return 6;
			}
			if (type == typeof(short))
			{
				return 7;
			}
			if (type == typeof(ushort))
			{
				return 8;
			}
			if (type == typeof(int))
			{
				return 9;
			}
			if (type == typeof(uint))
			{
				return 10;
			}
			if (type == typeof(long))
			{
				return 11;
			}
			if (type == typeof(ulong))
			{
				return 12;
			}
			if (type == typeof(float))
			{
				return 13;
			}
			if (type == typeof(double))
			{
				return 14;
			}
			if (type == typeof(decimal))
			{
				return 15;
			}
			if (type == typeof(DateTime))
			{
				return 16;
			}
			if (type == typeof(string))
			{
				return 18;
			}
			return 1;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000022F9 File Offset: 0x000004F9
		public static bool IsDbNull(this object value)
		{
			string text;
			if (value == null)
			{
				text = null;
			}
			else
			{
				Type type = value.GetType();
				text = ((type != null) ? type.FullName : null);
			}
			return text == "System.DBNull";
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000231D File Offset: 0x0000051D
		public static Type[] GetGenericArguments(this Type type)
		{
			return IntrospectionExtensions.GetTypeInfo(type).GenericTypeArguments;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000232C File Offset: 0x0000052C
		[return: Nullable(2)]
		public static PropertyInfo GetPublicProperty(this Type type, string name)
		{
			return Enumerable.FirstOrDefault<PropertyInfo>(type.GetProperties(28), (PropertyInfo p) => p.Name == name);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000235F File Offset: 0x0000055F
		[return: Nullable(2)]
		public static FieldInfo GetPublicStaticField(this Type type, string name)
		{
			return RuntimeReflectionExtensions.GetRuntimeField(type, name);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002368 File Offset: 0x00000568
		public static IEnumerable<PropertyInfo> GetProperties(this Type type, bool includeNonPublic)
		{
			Func<PropertyInfo, bool> predicate = includeNonPublic ? ReflectionExtensions.IsInstance : ReflectionExtensions.IsInstancePublic;
			if (!type.IsInterface())
			{
				return Enumerable.Where<PropertyInfo>(RuntimeReflectionExtensions.GetRuntimeProperties(type), predicate);
			}
			return Enumerable.SelectMany<Type, PropertyInfo>(Enumerable.Concat<Type>(new Type[]
			{
				type
			}, type.GetInterfaces()), (Type i) => Enumerable.Where<PropertyInfo>(RuntimeReflectionExtensions.GetRuntimeProperties(i), predicate));
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000023D0 File Offset: 0x000005D0
		public static IEnumerable<PropertyInfo> GetPublicProperties(this Type type)
		{
			return type.GetProperties(false);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000023D9 File Offset: 0x000005D9
		public static IEnumerable<FieldInfo> GetPublicFields(this Type type)
		{
			return Enumerable.Where<FieldInfo>(RuntimeReflectionExtensions.GetRuntimeFields(type), (FieldInfo f) => !f.IsStatic && f.IsPublic);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002405 File Offset: 0x00000605
		public static IEnumerable<MethodInfo> GetPublicStaticMethods(this Type type)
		{
			return Enumerable.Where<MethodInfo>(RuntimeReflectionExtensions.GetRuntimeMethods(type), (MethodInfo m) => m.IsPublic && m.IsStatic);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002434 File Offset: 0x00000634
		public static MethodInfo GetPrivateStaticMethod(this Type type, string name)
		{
			MethodInfo methodInfo = Enumerable.FirstOrDefault<MethodInfo>(RuntimeReflectionExtensions.GetRuntimeMethods(type), (MethodInfo m) => !m.IsPublic && m.IsStatic && m.Name.Equals(name));
			if (methodInfo == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(41, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Expected to find a method named '");
				defaultInterpolatedStringHandler.AppendFormatted(name);
				defaultInterpolatedStringHandler.AppendLiteral("' in '");
				defaultInterpolatedStringHandler.AppendFormatted(type.FullName);
				defaultInterpolatedStringHandler.AppendLiteral("'.");
				throw new MissingMethodException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return methodInfo;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000024C0 File Offset: 0x000006C0
		[return: Nullable(2)]
		public static MethodInfo GetPublicStaticMethod(this Type type, string name, params Type[] parameterTypes)
		{
			return Enumerable.FirstOrDefault<MethodInfo>(RuntimeReflectionExtensions.GetRuntimeMethods(type), delegate(MethodInfo m)
			{
				if (!m.IsPublic || !m.IsStatic || !m.Name.Equals(name))
				{
					return false;
				}
				ParameterInfo[] parameters = m.GetParameters();
				if (parameters.Length == parameterTypes.Length)
				{
					return Enumerable.All<bool>(Enumerable.Zip<ParameterInfo, Type, bool>(parameters, parameterTypes, (ParameterInfo pi, Type pt) => pi.ParameterType == pt), (bool r) => r);
				}
				return false;
			});
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000024F8 File Offset: 0x000006F8
		[return: Nullable(2)]
		public static MethodInfo GetPublicInstanceMethod(this Type type, string name)
		{
			return Enumerable.FirstOrDefault<MethodInfo>(RuntimeReflectionExtensions.GetRuntimeMethods(type), (MethodInfo m) => m.IsPublic && !m.IsStatic && m.Name.Equals(name));
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000252C File Offset: 0x0000072C
		[return: Nullable(2)]
		public static MethodInfo GetGetMethod(this PropertyInfo property, bool nonPublic)
		{
			MethodInfo methodInfo = property.GetMethod;
			if (!nonPublic && !methodInfo.IsPublic)
			{
				methodInfo = null;
			}
			return methodInfo;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000254E File Offset: 0x0000074E
		[return: Nullable(2)]
		public static MethodInfo GetSetMethod(this PropertyInfo property)
		{
			return property.SetMethod;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002556 File Offset: 0x00000756
		public static IEnumerable<Type> GetInterfaces(this Type type)
		{
			return IntrospectionExtensions.GetTypeInfo(type).ImplementedInterfaces;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002563 File Offset: 0x00000763
		public static bool IsInstanceOf(this Type type, object o)
		{
			return o.GetType() == type || IntrospectionExtensions.GetTypeInfo(o.GetType()).IsSubclassOf(type);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002586 File Offset: 0x00000786
		public static Attribute[] GetAllCustomAttributes<[Nullable(2)] TAttribute>(this PropertyInfo member)
		{
			return Attribute.GetCustomAttributes(member, typeof(TAttribute), true);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000259C File Offset: 0x0000079C
		public static bool AcceptsNull(this MemberInfo member)
		{
			object[] customAttributes = member.DeclaringType.GetCustomAttributes(typeof(NullableContextAttribute), true);
			NullableContextAttribute nullableContextAttribute = Enumerable.FirstOrDefault<NullableContextAttribute>(Enumerable.OfType<NullableContextAttribute>(customAttributes));
			byte b = (nullableContextAttribute != null) ? nullableContextAttribute.Flag : 0;
			object[] customAttributes2 = member.GetCustomAttributes(typeof(NullableAttribute), true);
			NullableAttribute nullableAttribute = Enumerable.FirstOrDefault<NullableAttribute>(Enumerable.OfType<NullableAttribute>(customAttributes2));
			Nullable<bool> flag2;
			if (nullableAttribute == null)
			{
				flag2 = default(bool?);
			}
			else
			{
				flag2 = new bool?(Enumerable.Any<byte>(nullableAttribute.NullableFlags, (byte flag) => flag == 2));
			}
			return flag2 ?? (b == 2);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000267D File Offset: 0x0000087D
		[CompilerGenerated]
		internal static bool <GetImplementationOfOpenGenericInterface>g__IsGenericDefinitionOfType|4_1(Type t, [Nullable(2)] object context)
		{
			return t.IsGenericType && t.GetGenericTypeDefinition() == (Type)context;
		}

		// Token: 0x04000002 RID: 2
		private static readonly Func<PropertyInfo, bool> IsInstance = (PropertyInfo property) => !(property.GetMethod ?? property.SetMethod).IsStatic;

		// Token: 0x04000003 RID: 3
		private static readonly Func<PropertyInfo, bool> IsInstancePublic = (PropertyInfo property) => ReflectionExtensions.IsInstance.Invoke(property) && (property.GetMethod ?? property.SetMethod).IsPublic;
	}
}
