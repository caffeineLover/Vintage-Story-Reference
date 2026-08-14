using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001ED RID: 493
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Comment : ParsingEvent
	{
		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x00025B84 File Offset: 0x00023D84
		public string Value { get; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x00025B8C File Offset: 0x00023D8C
		public bool IsInline { get; }

		// Token: 0x06000AF6 RID: 2806 RVA: 0x00025B94 File Offset: 0x00023D94
		public Comment(string value, bool isInline) : this(value, isInline, Mark.Empty, Mark.Empty)
		{
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x00025BA8 File Offset: 0x00023DA8
		public Comment(string value, bool isInline, Mark start, Mark end) : base(start, end)
		{
			this.Value = value;
			this.IsInline = isInline;
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x00025BC2 File Offset: 0x00023DC2
		internal override EventType Type
		{
			get
			{
				return EventType.Comment;
			}
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00025BC6 File Offset: 0x00023DC6
		public override void Accept(IParsingEventVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00025BCF File Offset: 0x00023DCF
		public override string ToString()
		{
			return (this.IsInline ? "Inline" : "Block") + " Comment [" + this.Value + "]";
		}
	}
}
