using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000042 RID: 66
	[NullableContext(1)]
	[Nullable(0)]
	[AttributeUsage(384)]
	public sealed class YamlConverterAttribute : Attribute
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000692A File Offset: 0x00004B2A
		public Type ConverterType { get; }

		// Token: 0x060001E8 RID: 488 RVA: 0x00006932 File Offset: 0x00004B32
		public YamlConverterAttribute(Type converterType)
		{
			this.ConverterType = converterType;
		}
	}
}
