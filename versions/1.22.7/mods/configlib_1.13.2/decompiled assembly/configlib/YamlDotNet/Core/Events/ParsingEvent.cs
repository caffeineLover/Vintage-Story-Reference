using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001F2 RID: 498
	public abstract class ParsingEvent
	{
		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x0000206F File Offset: 0x0000026F
		public virtual int NestingIncrease
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000AF2 RID: 2802
		internal abstract EventType Type { get; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x00025575 File Offset: 0x00023775
		public Mark Start { get; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x0002557D File Offset: 0x0002377D
		public Mark End { get; }

		// Token: 0x06000AF5 RID: 2805
		[NullableContext(1)]
		public abstract void Accept(IParsingEventVisitor visitor);

		// Token: 0x06000AF6 RID: 2806 RVA: 0x00025585 File Offset: 0x00023785
		internal ParsingEvent(in Mark start, in Mark end)
		{
			this.Start = start;
			this.End = end;
		}
	}
}
