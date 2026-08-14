using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace YamlDotNet
{
	// Token: 0x02000004 RID: 4
	internal static class PropertyInfoExtensions
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002073 File Offset: 0x00000273
		[NullableContext(1)]
		[return: Nullable(2)]
		public static object ReadValue(this PropertyInfo property, object target)
		{
			return property.GetValue(target, null);
		}
	}
}
