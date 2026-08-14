using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001EC RID: 492
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnchorAlias : ParsingEvent
	{
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x00025AEF File Offset: 0x00023CEF
		internal override EventType Type
		{
			get
			{
				return EventType.Alias;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x00025AF2 File Offset: 0x00023CF2
		public AnchorName Value { get; }

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00025AFA File Offset: 0x00023CFA
		public AnchorAlias(AnchorName value, Mark start, Mark end) : base(start, end)
		{
			if (value.IsEmpty)
			{
				throw new YamlException(ref start, ref end, "Anchor value must not be empty.");
			}
			this.Value = value;
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x00025B25 File Offset: 0x00023D25
		public AnchorAlias(AnchorName value) : this(value, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x00025B38 File Offset: 0x00023D38
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(16, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Alias [value = ");
			defaultInterpolatedStringHandler.AppendFormatted<AnchorName>(this.Value);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x00025B7B File Offset: 0x00023D7B
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
