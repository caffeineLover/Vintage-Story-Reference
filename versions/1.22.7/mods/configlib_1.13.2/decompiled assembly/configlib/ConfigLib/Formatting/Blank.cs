using System;
using System.Runtime.CompilerServices;

namespace ConfigLib.Formatting
{
	// Token: 0x02000054 RID: 84
	internal sealed class Blank : IFormattingBlock, IConfigBlock
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000D761 File Offset: 0x0000B961
		public float SortingWeight
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0000206F File Offset: 0x0000026F
		public bool Collapsible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000206F File Offset: 0x0000026F
		public bool StopCollapsible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0000D768 File Offset: 0x0000B968
		[Nullable(1)]
		public string Yaml
		{
			[NullableContext(1)]
			get
			{
				return "";
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000206C File Offset: 0x0000026C
		[NullableContext(1)]
		public bool Draw(string id)
		{
			return true;
		}
	}
}
