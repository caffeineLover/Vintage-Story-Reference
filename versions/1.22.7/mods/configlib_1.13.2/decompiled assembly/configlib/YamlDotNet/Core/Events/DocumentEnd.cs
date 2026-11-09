using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001EA RID: 490
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DocumentEnd : ParsingEvent
	{
		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x000252B2 File Offset: 0x000234B2
		public override int NestingIncrease
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x000252B5 File Offset: 0x000234B5
		internal override EventType Type
		{
			get
			{
				return EventType.DocumentEnd;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x000252B8 File Offset: 0x000234B8
		public bool IsImplicit { get; }

		// Token: 0x06000AC2 RID: 2754 RVA: 0x000252C0 File Offset: 0x000234C0
		public DocumentEnd(bool isImplicit, Mark start, Mark end) : base(start, end)
		{
			this.IsImplicit = isImplicit;
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x000252D3 File Offset: 0x000234D3
		public DocumentEnd(bool isImplicit) : this(isImplicit, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x000252E8 File Offset: 0x000234E8
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(28, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Document end [isImplicit = ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.IsImplicit);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0002532B File Offset: 0x0002352B
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
