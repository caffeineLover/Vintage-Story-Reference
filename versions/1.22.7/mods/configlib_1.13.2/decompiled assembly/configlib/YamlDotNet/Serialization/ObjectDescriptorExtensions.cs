using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000098 RID: 152
	public static class ObjectDescriptorExtensions
	{
		// Token: 0x06000392 RID: 914 RVA: 0x00010424 File Offset: 0x0000E624
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
