using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x02000102 RID: 258
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnchorAlias : ParsingEvent
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000648 RID: 1608 RVA: 0x000174F7 File Offset: 0x000156F7
		internal override EventType Type
		{
			get
			{
				return EventType.Alias;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x000174FA File Offset: 0x000156FA
		public AnchorName Value { get; }

		// Token: 0x0600064A RID: 1610 RVA: 0x00017502 File Offset: 0x00015702
		public AnchorAlias(AnchorName value, Mark start, Mark end) : base(start, end)
		{
			if (value.IsEmpty)
			{
				throw new YamlException(ref start, ref end, "Anchor value must not be empty.");
			}
			this.Value = value;
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x0001752D File Offset: 0x0001572D
		public AnchorAlias(AnchorName value) : this(value, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00017540 File Offset: 0x00015740
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(16, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Alias [value = ");
			defaultInterpolatedStringHandler.AppendFormatted<AnchorName>(this.Value);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00017583 File Offset: 0x00015783
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}
