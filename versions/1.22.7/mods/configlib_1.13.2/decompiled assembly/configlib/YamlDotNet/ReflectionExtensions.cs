using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace YamlDotNet
{
	// Token: 0x02000071 RID: 113
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ReflectionExtensions
	{
		// Token: 0x060002BF RID: 703 RVA: 0x0000EE5B File Offset: 0x0000D05B
		[return: Nullable(2)]
		public static Type BaseType(this Type type)
		{
			return type.BaseType;
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000EE63 File Offset: 0x0000D063
		public static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000EE6B File Offset: 0x0000D06B
		public static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000EE73 File Offset: 0x0000D073
		public static bool IsGenericTypeDefinition(this Type type)
		{
			return type.IsGenericTypeDefinition;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000EE7B File Offset: 0x0000D07B
		public static bool IsInterface(this Type type)
		{
			return type.IsInterface;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000EE83 File Offset: 0x0000D083
		public static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000EE8B File Offset: 0x0000D08B
		public static bool IsDbNull(this object value)
		{
			return value is DBNull;
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000EE98 File Offset: 0x0000D098
		public static bool HasDefaultConstructor(this Type type, bool allowPrivateConstructors)
		{
			BindingFlags bindingFlags = 20;
			if (allowPrivateConstructors)
			{
				bindingFlags |= 32;
			}
			return type.IsValueType || type.GetConstructor(bindingFlags, null, Type.EmptyTypes, null) != null;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000EECE File Offset: 0x0000D0CE
		public static TypeCode GetTypeCode(this Type type)
		{
			return Type.GetTypeCode(type);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000EED6 File Offset: 0x0000D0D6
		[return: Nullable(2)]
		public static PropertyInfo GetPublicProperty(this Type type, string name)
		{
			return type.GetProperty(name);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000EEDF File Offset: 0x0000D0DF
		[return: Nullable(2)]
		public static FieldInfo GetPublicStaticField(this Type type, string name)
		{
			return type.GetField(name, 24);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000EEEC File Offset: 0x0000D0EC
		public static IEnumerable<PropertyInfo> GetProperties(this Type type, bool includeNonPublic)
		{
			BindingFlags bindingFlags = 20;
			if (includeNonPublic)
			{
				bindingFlags |= 32;
			}
			if (!type.IsInterface)
			{
				return type.GetProperties(bindingFlags);
			}
			return Enumerable.SelectMany<Type, PropertyInfo>(Enumerable.Concat<Type>(new Type[]
			{
				type
			}, type.GetInterfaces()), (Type i) => i.GetProperties(bindingFlags));
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000EF56 File Offset: 0x0000D156
		public static IEnumerable<PropertyInfo> GetPublicProperties(this Type type)
		{
			return type.GetProperties(false);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000EF5F File Offset: 0x0000D15F
		public static IEnumerable<FieldInfo> GetPublicFields(this Type type)
		{
			return type.GetFields(20);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000EF69 File Offset: 0x0000D169
		public static IEnumerable<MethodInfo> GetPublicStaticMethods(this Type type)
		{
			return type.GetMethods(24);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000EF74 File Offset: 0x0000D174
		public static MethodInfo GetPrivateStaticMethod(this Type type, string name)
		{
			MethodInfo method = type.GetMethod(name, 40);
			if (method == null)
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
			return method;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000EFDE File Offset: 0x0000D1DE
		[return: Nullable(2)]
		public static MethodInfo GetPublicStaticMethod(this Type type, string name, params Type[] parameterTypes)
		{
			return type.GetMethod(name, 24, null, parameterTypes, null);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000EFEC File Offset: 0x0000D1EC
		[return: Nullable(2)]
		public static MethodInfo GetPublicInstanceMethod(this Type type, string name)
		{
			return type.GetMethod(name, 20);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000EFF8 File Offset: 0x0000D1F8
		public static Exception Unwrap(this TargetInvocationException ex)
		{
			Exception innerException = ex.InnerException;
			if (innerException == null)
			{
				return ex;
			}
			if (ReflectionExtensions.RemoteStackTraceField != null)
			{
				ReflectionExtensions.RemoteStackTraceField.SetValue(innerException, innerException.StackTrace + "\r\n");
			}
			return innerException;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000F03A File Offset: 0x0000D23A
		public static bool IsInstanceOf(this Type type, object o)
		{
			return type.IsInstanceOfType(o);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000F043 File Offset: 0x0000D243
		public static Attribute[] GetAllCustomAttributes<[Nullable(2)] TAttribute>(this PropertyInfo property)
		{
			return Attribute.GetCustomAttributes(property, typeof(TAttribute), true);
		}

		// Token: 0x0400015C RID: 348
		[Nullable(2)]
		private static readonly FieldInfo RemoteStackTraceField = typeof(Exception).GetField("_remoteStackTraceString", 36);
	}
}
