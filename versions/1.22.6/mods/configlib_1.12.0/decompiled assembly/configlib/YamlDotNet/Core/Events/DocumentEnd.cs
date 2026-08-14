using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001EE RID: 494
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DocumentEnd : ParsingEvent
	{
		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x00025BFA File Offset: 0x00023DFA
		public override int NestingIncrease
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000AFC RID: 2812 RVA: 0x00025BFD File Offset: 0x00023DFD
		internal override EventType Type
		{
			get
			{
				return EventType.DocumentEnd;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x00025C00 File Offset: 0x00023E00
		public bool IsImplicit { get; }

		// Token: 0x06000AFE RID: 2814 RVA: 0x00025C08 File Offset: 0x00023E08
		public DocumentEnd(bool isImplicit, Mark start, Mark end) : base(start, end)
		{
			this.IsImplicit = isImplicit;
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00025C1B File Offset: 0x00023E1B
		public DocumentEnd(bool isImplicit) : this(isImplicit, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x00025C30 File Offset: 0x00023E30
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(28, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Document end [isImplicit = ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.IsImplicit);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x00025C73 File Offset: 0x00023E73
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
