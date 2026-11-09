using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x020000FE RID: 254
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ReflectionUtility
	{
		// Token: 0x0600058F RID: 1423 RVA: 0x000142EC File Offset: 0x000124EC
		[return: Nullable(2)]
		public static Type GetImplementedGenericInterface(Type type, Type genericInterfaceType)
		{
			foreach (Type type2 in ReflectionUtility.GetImplementedInterfaces(type))
			{
				if (type2.IsGenericType() && type2.GetGenericTypeDefinition() == genericInterfaceType)
				{
					return type2;
				}
			}
			return null;
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00014350 File Offset: 0x00012550
		public static IEnumerable<Type> GetImplementedInterfaces(Type type)
		{
			if (type.IsInterface())
			{
				yield return type;
			}
			foreach (Type type2 in type.GetInterfaces())
			{
				yield return type2;
			}
			Type[] array = null;
			yield break;
		}
	}
}
