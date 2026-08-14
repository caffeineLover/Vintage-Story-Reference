using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace YamlDotNet
{
	// Token: 0x02000075 RID: 117
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ReflectionExtensions
	{
		// Token: 0x060002FB RID: 763 RVA: 0x0000F7A3 File Offset: 0x0000D9A3
		[return: Nullable(2)]
		public static Type BaseType(this Type type)
		{
			return type.BaseType;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000F7AB File Offset: 0x0000D9AB
		public static bool IsValueType(this Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000F7B3 File Offset: 0x0000D9B3
		public static bool IsGenericType(this Type type)
		{
			return type.IsGenericType;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000F7BB File Offset: 0x0000D9BB
		public static bool IsGenericTypeDefinition(this Type type)
		{
			return type.IsGenericTypeDefinition;
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000F7C3 File Offset: 0x0000D9C3
		public static bool IsInterface(this Type type)
		{
			return type.IsInterface;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000F7CB File Offset: 0x0000D9CB
		public static bool IsEnum(this Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000F7D3 File Offset: 0x0000D9D3
		public static bool IsDbNull(this object value)
		{
			return value is DBNull;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000F7E0 File Offset: 0x0000D9E0
		public static bool HasDefaultConstructor(this Type type, bool allowPrivateConstructors)
		{
			BindingFlags bindingFlags = 20;
			if (allowPrivateConstructors)
			{
				bindingFlags |= 32;
			}
			return type.IsValueType || type.GetConstructor(bindingFlags, null, Type.EmptyTypes, null) != null;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000F816 File Offset: 0x0000DA16
		public static TypeCode GetTypeCode(this Type type)
		{
			return Type.GetTypeCode(type);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000F81E File Offset: 0x0000DA1E
		[return: Nullable(2)]
		public static PropertyInfo GetPublicProperty(this Type type, string name)
		{
			return type.GetProperty(name);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000F827 File Offset: 0x0000DA27
		[return: Nullable(2)]
		public static FieldInfo GetPublicStaticField(this Type type, string name)
		{
			return type.GetField(name, 24);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000F834 File Offset: 0x0000DA34
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

		// Token: 0x06000307 RID: 775 RVA: 0x0000F89E File Offset: 0x0000DA9E
		public static IEnumerable<PropertyInfo> GetPublicProperties(this Type type)
		{
			return type.GetProperties(false);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000F8A7 File Offset: 0x0000DAA7
		public static IEnumerable<FieldInfo> GetPublicFields(this Type type)
		{
			return type.GetFields(20);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000F8B1 File Offset: 0x0000DAB1
		public static IEnumerable<MethodInfo> GetPublicStaticMethods(this Type type)
		{
			return type.GetMethods(24);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000F8BC File Offset: 0x0000DABC
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

		// Token: 0x0600030B RID: 779 RVA: 0x0000F926 File Offset: 0x0000DB26
		[return: Nullable(2)]
		public static MethodInfo GetPublicStaticMethod(this Type type, string name, params Type[] parameterTypes)
		{
			return type.GetMethod(name, 24, null, parameterTypes, null);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000F934 File Offset: 0x0000DB34
		[return: Nullable(2)]
		public static MethodInfo GetPublicInstanceMethod(this Type type, string name)
		{
			return type.GetMethod(name, 20);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000F940 File Offset: 0x0000DB40
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

		// Token: 0x0600030E RID: 782 RVA: 0x0000F982 File Offset: 0x0000DB82
		public static bool IsInstanceOf(this Type type, object o)
		{
			return type.IsInstanceOfType(o);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000F98B File Offset: 0x0000DB8B
		public static Attribute[] GetAllCustomAttributes<[Nullable(2)] TAttribute>(this PropertyInfo property)
		{
			return Attribute.GetCustomAttributes(property, typeof(TAttribute), true);
		}

		// Token: 0x0400018A RID: 394
		[Nullable(2)]
		private static readonly FieldInfo RemoteStackTraceField = typeof(Exception).GetField("_remoteStackTraceString", 36);
	}
}
