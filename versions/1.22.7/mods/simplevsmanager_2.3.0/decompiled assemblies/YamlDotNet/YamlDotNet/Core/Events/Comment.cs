using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x02000103 RID: 259
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Comment : ParsingEvent
	{
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600064E RID: 1614 RVA: 0x0001758C File Offset: 0x0001578C
		public string Value { get; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00017594 File Offset: 0x00015794
		public bool IsInline { get; }

		// Token: 0x06000650 RID: 1616 RVA: 0x0001759C File Offset: 0x0001579C
		public Comment(string value, bool isInline) : this(value, isInline, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x000175B0 File Offset: 0x000157B0
		public Comment(string value, bool isInline, Mark start, Mark end) : base(start, end)
		{
			this.Value = value;
			this.IsInline = isInline;
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000652 RID: 1618 RVA: 0x000175CA File Offset: 0x000157CA
		internal override EventType Type
		{
			get
			{
				return EventType.Comment;
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x000175CE File Offset: 0x000157CE
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x000175D7 File Offset: 0x000157D7
		public override string ToString()
		{
			return (this.IsInline ? "Inline" : "Block") + " Comment [" + this.Value + "]";
		}
	}
}
