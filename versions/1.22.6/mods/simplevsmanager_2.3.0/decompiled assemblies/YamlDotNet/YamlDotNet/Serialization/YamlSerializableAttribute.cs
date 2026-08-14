using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000046 RID: 70
	[AttributeUsage(28, Inherited = false, AllowMultiple = true)]
	public sealed class YamlSerializableAttribute : Attribute
	{
		// Token: 0x0600020A RID: 522 RVA: 0x00006BE0 File Offset: 0x00004DE0
		public YamlSerializableAttribute()
		{
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00006BE8 File Offset: 0x00004DE8
		[NullableContext(1)]
		public YamlSerializableAttribute(Type serializableType)
		{
		}
	}
}
