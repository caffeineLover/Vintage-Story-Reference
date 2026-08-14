using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace YamlDotNet
{
	// Token: 0x02000077 RID: 119
	internal static class PropertyInfoExtensions
	{
		// Token: 0x06000313 RID: 787 RVA: 0x0000F9C9 File Offset: 0x0000DBC9
		[NullableContext(1)]
		[return: Nullable(2)]
		public static object ReadValue(this PropertyInfo property, object target)
		{
			return property.GetValue(target, null);
		}
	}
}
