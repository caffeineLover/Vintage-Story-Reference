using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace WinRT
{
	// Token: 0x020001A7 RID: 423
	internal static class TypeExtensions
	{
		// Token: 0x06000DB3 RID: 3507 RVA: 0x00029444 File Offset: 0x00027644
		public static Type FindHelperType(this Type type)
		{
			return TypeExtensions.HelperTypeCache.GetOrAdd(type2, delegate(Type type)
			{
				if (typeof(Exception).IsAssignableFrom(type))
				{
					type = typeof(Exception);
				}
				Type type3 = Projections.FindCustomHelperTypeMapping(type, false);
				if (type3 != null)
				{
					return type3;
				}
				string text = type.FullName;
				string text2 = "ABI.Impl.";
				if (text.StartsWith(text2, 4))
				{
					text = text.Substring(text2.Length);
				}
				string text3 = "ABI." + text;
				return type.Assembly.GetType(text3) ?? Type.GetType(text3);
			});
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x00029470 File Offset: 0x00027670
		public static Type GetHelperType(this Type type)
		{
			Type type2 = type.FindHelperType();
			if (type2 != null)
			{
				return type2;
			}
			throw new InvalidOperationException("Target type is not a projected type: " + type.FullName + ".");
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x000294A3 File Offset: 0x000276A3
		public static Type GetGuidType(this Type type)
		{
			if (!type.IsDelegate())
			{
				return type;
			}
			return type.GetHelperType();
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x000294B8 File Offset: 0x000276B8
		public static Type FindVftblType(this Type helperType)
		{
			Type type = helperType.GetNestedType("Vftbl");
			if (type == null)
			{
				return null;
			}
			if (helperType.IsGenericType && type != null)
			{
				type = type.MakeGenericType(helperType.GetGenericArguments());
			}
			return type;
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x000294EF File Offset: 0x000276EF
		internal static IntPtr GetAbiToProjectionVftblPtr(this Type helperType)
		{
			return (IntPtr)(helperType.FindVftblType() ?? helperType).GetField("AbiToProjectionVftablePtr", 24).GetValue(null);
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00029513 File Offset: 0x00027713
		public static Type GetAbiType(this Type type)
		{
			return type.GetHelperType().GetMethod("GetAbi", 56).ReturnType;
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0002952C File Offset: 0x0002772C
		public static Type GetMarshalerType(this Type type)
		{
			return type.GetHelperType().GetMethod("CreateMarshaler", 56).ReturnType;
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x00029548 File Offset: 0x00027748
		internal static Type GetMarshaler2Type(this Type type)
		{
			Type helperType = type.GetHelperType();
			return (helperType.GetMethod("CreateMarshaler2", 56) ?? helperType.GetMethod("CreateMarshaler", 56)).ReturnType;
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x0002957F File Offset: 0x0002777F
		internal static Type GetMarshalerArrayType(this Type type)
		{
			MethodInfo method = type.GetHelperType().GetMethod("CreateMarshalerArray", 56);
			if (method == null)
			{
				return null;
			}
			return method.ReturnType;
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x0002959E File Offset: 0x0002779E
		public static bool IsDelegate(this Type type)
		{
			return typeof(Delegate).IsAssignableFrom(type);
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x000295B0 File Offset: 0x000277B0
		internal static bool IsTypeOfType(this Type type)
		{
			return typeof(Type).IsAssignableFrom(type);
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x000295C2 File Offset: 0x000277C2
		public static Type GetRuntimeClassCCWType(this Type type)
		{
			if (!type.IsClass || type.IsArray)
			{
				return null;
			}
			return type.GetAuthoringMetadataType();
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x000295DC File Offset: 0x000277DC
		internal static Type GetAuthoringMetadataType(this Type type)
		{
			return TypeExtensions.AuthoringMetadataTypeCache.GetOrAdd(type2, delegate(Type type)
			{
				string text = "ABI.Impl." + type.FullName;
				return type.Assembly.GetType(text, false) ?? Type.GetType(text, false);
			});
		}

		// Token: 0x0400029B RID: 667
		private static readonly ConcurrentDictionary<Type, Type> HelperTypeCache = new ConcurrentDictionary<Type, Type>();

		// Token: 0x0400029C RID: 668
		private static readonly ConcurrentDictionary<Type, Type> AuthoringMetadataTypeCache = new ConcurrentDictionary<Type, Type>();
	}
}
