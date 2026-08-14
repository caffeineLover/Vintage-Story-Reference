using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Qpdf
{
	// Token: 0x02000035 RID: 53
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SimpleJsonPropertyNameAttribute : Attribute
	{
		// Token: 0x06000142 RID: 322 RVA: 0x000046EC File Offset: 0x000028EC
		public SimpleJsonPropertyNameAttribute(string name)
		{
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000143 RID: 323 RVA: 0x000046FB File Offset: 0x000028FB
		public string Name { get; } = name;
	}
}
