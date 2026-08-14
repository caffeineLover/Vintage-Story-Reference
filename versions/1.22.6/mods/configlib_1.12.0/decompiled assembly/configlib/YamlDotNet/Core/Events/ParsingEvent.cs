using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001F6 RID: 502
	public abstract class ParsingEvent
	{
		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000B2D RID: 2861 RVA: 0x0000D44C File Offset: 0x0000B64C
		public virtual int NestingIncrease
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000B2E RID: 2862
		internal abstract EventType Type { get; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000B2F RID: 2863 RVA: 0x00025EBD File Offset: 0x000240BD
		public Mark Start { get; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x00025EC5 File Offset: 0x000240C5
		public Mark End { get; }

		// Token: 0x06000B31 RID: 2865
		[NullableContext(1)]
		public abstract void Accept(IParsingEventVisitor visitor);

		// Token: 0x06000B32 RID: 2866 RVA: 0x00025ECD File Offset: 0x000240CD
		internal ParsingEvent(in Mark start, in Mark end)
		{
			this.Start = start;
			this.End = end;
		}
	}
}
