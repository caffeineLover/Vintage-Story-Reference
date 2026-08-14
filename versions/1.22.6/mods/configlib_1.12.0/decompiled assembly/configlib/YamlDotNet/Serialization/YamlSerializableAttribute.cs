using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000F6 RID: 246
	[AttributeUsage(4, Inherited = false, AllowMultiple = true)]
	public sealed class YamlSerializableAttribute : Attribute
	{
		// Token: 0x060005AD RID: 1453 RVA: 0x0000F733 File Offset: 0x0000D933
		public YamlSerializableAttribute()
		{
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0000F733 File Offset: 0x0000D933
		[NullableContext(1)]
		public YamlSerializableAttribute(Type serializableType)
		{
		}
	}
}
