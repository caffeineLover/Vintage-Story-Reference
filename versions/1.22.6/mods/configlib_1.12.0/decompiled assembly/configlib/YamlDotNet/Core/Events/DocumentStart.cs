using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001EF RID: 495
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class DocumentStart : ParsingEvent
	{
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000B02 RID: 2818 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
		public override int NestingIncrease
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x0001C0BF File Offset: 0x0001A2BF
		internal override EventType Type
		{
			get
			{
				return EventType.DocumentStart;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000B04 RID: 2820 RVA: 0x00025C7C File Offset: 0x00023E7C
		public TagDirectiveCollection Tags { get; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x00025C84 File Offset: 0x00023E84
		public VersionDirective Version { get; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00025C8C File Offset: 0x00023E8C
		public bool IsImplicit { get; }

		// Token: 0x06000B07 RID: 2823 RVA: 0x00025C94 File Offset: 0x00023E94
		public DocumentStart(VersionDirective version, TagDirectiveCollection tags, bool isImplicit, Mark start, Mark end) : base(start, end)
		{
			this.Version = version;
			this.Tags = tags;
			this.IsImplicit = isImplicit;
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00025CB5 File Offset: 0x00023EB5
		public DocumentStart(VersionDirective version, TagDirectiveCollection tags, bool isImplicit) : this(version, tags, isImplicit, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00025CCA File Offset: 0x00023ECA
		public DocumentStart(in Mark start, in Mark end) : this(null, null, true, start, end)
		{
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00025CE1 File Offset: 0x00023EE1
		public DocumentStart() : this(null, null, true, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00025CF8 File Offset: 0x00023EF8
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

		// Token: 0x06000B0C RID: 2828 RVA: 0x00025D3B File Offset: 0x00023F3B
		[NullableContext(1)]
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
