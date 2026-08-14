using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x0200010C RID: 268
	public abstract class ParsingEvent
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x000178CD File Offset: 0x00015ACD
		public virtual int NestingIncrease
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000688 RID: 1672
		internal abstract EventType Type { get; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x000178D0 File Offset: 0x00015AD0
		public Mark Start { get; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x000178D8 File Offset: 0x00015AD8
		public Mark End { get; }

		// Token: 0x0600068B RID: 1675
		[NullableContext(1)]
		public abstract void Accept(IParsingEventVisitor visitor);

		// Token: 0x0600068C RID: 1676 RVA: 0x000178E0 File Offset: 0x00015AE0
		internal ParsingEvent(in Mark start, in Mark end)
		{
			this.Start = start;
			this.End = end;
		}
	}
}
