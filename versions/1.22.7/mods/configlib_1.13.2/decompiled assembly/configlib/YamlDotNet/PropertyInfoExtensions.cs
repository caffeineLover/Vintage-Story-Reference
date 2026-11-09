using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace YamlDotNet
{
	// Token: 0x02000073 RID: 115
	internal static class PropertyInfoExtensions
	{
		// Token: 0x060002D7 RID: 727 RVA: 0x0000F081 File Offset: 0x0000D281
		[NullableContext(1)]
		[return: Nullable(2)]
		public static object ReadValue(this PropertyInfo property, object target)
		{
			return property.GetValue(target, null);
		}
	}
}
