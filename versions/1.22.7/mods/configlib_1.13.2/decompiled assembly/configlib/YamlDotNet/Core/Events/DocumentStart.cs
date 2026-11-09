using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001EB RID: 491
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class DocumentStart : ParsingEvent
	{
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0000206C File Offset: 0x0000026C
		public override int NestingIncrease
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000AC7 RID: 2759 RVA: 0x0001B777 File Offset: 0x00019977
		internal override EventType Type
		{
			get
			{
				return EventType.DocumentStart;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000AC8 RID: 2760 RVA: 0x00025334 File Offset: 0x00023534
		public TagDirectiveCollection Tags { get; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000AC9 RID: 2761 RVA: 0x0002533C File Offset: 0x0002353C
		public VersionDirective Version { get; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000ACA RID: 2762 RVA: 0x00025344 File Offset: 0x00023544
		public bool IsImplicit { get; }

		// Token: 0x06000ACB RID: 2763 RVA: 0x0002534C File Offset: 0x0002354C
		public DocumentStart(VersionDirective version, TagDirectiveCollection tags, bool isImplicit, Mark start, Mark end) : base(start, end)
		{
			this.Version = version;
			this.Tags = tags;
			this.IsImplicit = isImplicit;
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x0002536D File Offset: 0x0002356D
		public DocumentStart(VersionDirective version, TagDirectiveCollection tags, bool isImplicit) : this(version, tags, isImplicit, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x00025382 File Offset: 0x00023582
		public DocumentStart(in Mark start, in Mark end) : this(null, null, true, start, end)
		{
		}

		// Token: 0x06000ACE RID: 2766 RVA: 0x00025399 File Offset: 0x00023599
		public DocumentStart() : this(null, null, true, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x000253B0 File Offset: 0x000235B0
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

		// Token: 0x06000AD0 RID: 2768 RVA: 0x000253F3 File Offset: 0x000235F3
		[NullableContext(1)]
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
