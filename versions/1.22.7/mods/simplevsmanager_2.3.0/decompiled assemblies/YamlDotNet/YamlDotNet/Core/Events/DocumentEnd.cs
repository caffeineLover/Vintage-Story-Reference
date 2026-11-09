using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x02000104 RID: 260
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DocumentEnd : ParsingEvent
	{
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00017602 File Offset: 0x00015802
		public override int NestingIncrease
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x00017605 File Offset: 0x00015805
		internal override EventType Type
		{
			get
			{
				return EventType.DocumentEnd;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x00017608 File Offset: 0x00015808
		public bool IsImplicit { get; }

		// Token: 0x06000658 RID: 1624 RVA: 0x00017610 File Offset: 0x00015810
		public DocumentEnd(bool isImplicit, Mark start, Mark end) : base(start, end)
		{
			this.IsImplicit = isImplicit;
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00017623 File Offset: 0x00015823
		public DocumentEnd(bool isImplicit) : this(isImplicit, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00017638 File Offset: 0x00015838
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(28, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Document end [isImplicit = ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.IsImplicit);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x0001767B File Offset: 0x0001587B
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
