using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200001E RID: 30
	public static class ObjectDescriptorExtensions
	{
		// Token: 0x060000BB RID: 187 RVA: 0x000039DF File Offset: 0x00001BDF
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
