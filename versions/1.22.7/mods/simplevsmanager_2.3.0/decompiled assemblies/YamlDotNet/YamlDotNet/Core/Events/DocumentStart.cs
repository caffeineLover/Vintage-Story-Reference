using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core.Events
{
	// Token: 0x02000105 RID: 261
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class DocumentStart : ParsingEvent
	{
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x00017684 File Offset: 0x00015884
		public override int NestingIncrease
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x00017687 File Offset: 0x00015887
		internal override EventType Type
		{
			get
			{
				return EventType.DocumentStart;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x0001768A File Offset: 0x0001588A
		public TagDirectiveCollection Tags { get; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x00017692 File Offset: 0x00015892
		public VersionDirective Version { get; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x0001769A File Offset: 0x0001589A
		public bool IsImplicit { get; }

		// Token: 0x06000661 RID: 1633 RVA: 0x000176A2 File Offset: 0x000158A2
		public DocumentStart(VersionDirective version, TagDirectiveCollection tags, bool isImplicit, Mark start, Mark end) : base(start, end)
		{
			this.Version = version;
			this.Tags = tags;
			this.IsImplicit = isImplicit;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x000176C3 File Offset: 0x000158C3
		public DocumentStart(VersionDirective version, TagDirectiveCollection tags, bool isImplicit) : this(version, tags, isImplicit, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x000176D8 File Offset: 0x000158D8
		public DocumentStart(in Mark start, in Mark end) : this(null, null, true, start, end)
		{
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x000176EF File Offset: 0x000158EF
		public DocumentStart() : this(null, null, true, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00017704 File Offset: 0x00015904
		[NullableContext(1)]
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(30, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Document start [isImplicit = ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.IsImplicit);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00017747 File Offset: 0x00015947
		[NullableContext(1)]
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
