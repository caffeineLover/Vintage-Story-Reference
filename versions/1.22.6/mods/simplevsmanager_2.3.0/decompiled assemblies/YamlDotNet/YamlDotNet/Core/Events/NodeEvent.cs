using System;

namespace YamlDotNet.Core.Events
{
	// Token: 0x0200010B RID: 267
	public abstract class NodeEvent : ParsingEvent
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x0001788F File Offset: 0x00015A8F
		public AnchorName Anchor { get; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x00017897 File Offset: 0x00015A97
		public TagName Tag { get; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000684 RID: 1668
		public abstract bool IsCanonical { get; }

		// Token: 0x06000685 RID: 1669 RVA: 0x0001789F File Offset: 0x00015A9F
		protected NodeEvent(AnchorName anchor, TagName tag, Mark start, Mark end) : base(start, end)
		{
			this.Anchor = anchor;
			this.Tag = tag;
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x000178B9 File Offset: 0x00015AB9
		protected NodeEvent(AnchorName anchor, TagName tag) : this(anchor, tag, Mark.Empty, Mark.Empty)
		{
		}
	}
}
