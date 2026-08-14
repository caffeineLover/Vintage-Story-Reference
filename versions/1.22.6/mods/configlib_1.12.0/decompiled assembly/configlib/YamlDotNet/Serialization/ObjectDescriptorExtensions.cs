using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200009C RID: 156
	public static class ObjectDescriptorExtensions
	{
		// Token: 0x060003CE RID: 974 RVA: 0x00010D6C File Offset: 0x0000EF6C
		[NullableContext(1)]
		public static object NonNullValue(this IObjectDescriptor objectDescriptor)
		{
			object value = objectDescriptor.Value;
			if (value == null)
			{
				throw new InvalidOperationException("Attempted to use a IObjectDescriptor of type '" + objectDescriptor.Type.FullName + "' whose Value is null at a point whete it is invalid to do so. This may indicate a bug in YamlDotNet.");
			}
			return value;
		}
	}
}
