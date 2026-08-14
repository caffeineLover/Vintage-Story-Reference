using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.Utilities
{
	// Token: 0x02000102 RID: 258
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ReflectionUtility
	{
		// Token: 0x060005CB RID: 1483 RVA: 0x00014C34 File Offset: 0x00012E34
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

		// Token: 0x060005CC RID: 1484 RVA: 0x00014C98 File Offset: 0x00012E98
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
