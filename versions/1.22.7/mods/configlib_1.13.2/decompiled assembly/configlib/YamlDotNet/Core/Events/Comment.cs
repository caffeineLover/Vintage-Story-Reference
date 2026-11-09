using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001E9 RID: 489
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Comment : ParsingEvent
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x0002523C File Offset: 0x0002343C
		public string Value { get; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x00025244 File Offset: 0x00023444
		public bool IsInline { get; }

		// Token: 0x06000ABA RID: 2746 RVA: 0x0002524C File Offset: 0x0002344C
		public Comment(string value, bool isInline) : this(value, isInline, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x00025260 File Offset: 0x00023460
		public Comment(string value, bool isInline, Mark start, Mark end) : base(start, end)
		{
			this.Value = value;
			this.IsInline = isInline;
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x0002527A File Offset: 0x0002347A
		internal override EventType Type
		{
			get
			{
				return EventType.Comment;
			}
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x0002527E File Offset: 0x0002347E
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x00025287 File Offset: 0x00023487
		public override string ToString()
		{
			return (this.IsInline ? "Inline" : "Block") + " Comment [" + this.Value + "]";
		}
	}
}
