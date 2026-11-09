using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000F2 RID: 242
	[AttributeUsage(4, Inherited = false, AllowMultiple = true)]
	public sealed class YamlSerializableAttribute : Attribute
	{
		// Token: 0x06000571 RID: 1393 RVA: 0x0000EDEB File Offset: 0x0000CFEB
		public YamlSerializableAttribute()
		{
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0000EDEB File Offset: 0x0000CFEB
		[NullableContext(1)]
		public YamlSerializableAttribute(Type serializableType)
		{
		}
	}
}
