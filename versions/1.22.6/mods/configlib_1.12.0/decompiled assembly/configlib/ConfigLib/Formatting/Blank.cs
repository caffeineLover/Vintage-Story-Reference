using System;
using System.Runtime.CompilerServices;

namespace ConfigLib.Formatting
{
	// Token: 0x02000052 RID: 82
	internal sealed class Blank : IFormattingBlock, IConfigBlock
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000D445 File Offset: 0x0000B645
		public float SortingWeight
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000D44C File Offset: 0x0000B64C
		public bool Collapsible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000D44C File Offset: 0x0000B64C
		public bool StopCollapsible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000D44F File Offset: 0x0000B64F
		[Nullable(1)]
		public string Yaml
		{
			[NullableContext(1)]
			get
			{
				return "";
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
		[NullableContext(1)]
		public bool Draw(string id)
		{
			return true;
		}
	}
}
