using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001E8 RID: 488
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnchorAlias : ParsingEvent
	{
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x000251A7 File Offset: 0x000233A7
		internal override EventType Type
		{
			get
			{
				return EventType.Alias;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x000251AA File Offset: 0x000233AA
		public AnchorName Value { get; }

		// Token: 0x06000AB4 RID: 2740 RVA: 0x000251B2 File Offset: 0x000233B2
		public AnchorAlias(AnchorName value, Mark start, Mark end) : base(start, end)
		{
			if (value.IsEmpty)
			{
				throw new YamlException(ref start, ref end, "Anchor value must not be empty.");
			}
			this.Value = value;
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x000251DD File Offset: 0x000233DD
		public AnchorAlias(AnchorName value) : this(value, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x000251F0 File Offset: 0x000233F0
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(16, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Alias [value = ");
			defaultInterpolatedStringHandler.AppendFormatted<AnchorName>(this.Value);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000AB7 RID: 2743 RVA: 0x00025233 File Offset: 0x00023433
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
