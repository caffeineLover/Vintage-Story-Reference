using System;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001F5 RID: 501
	public abstract class NodeEvent : ParsingEvent
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000B28 RID: 2856 RVA: 0x00025E7F File Offset: 0x0002407F
		public AnchorName Anchor { get; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000B29 RID: 2857 RVA: 0x00025E87 File Offset: 0x00024087
		public TagName Tag { get; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000B2A RID: 2858
		public abstract bool IsCanonical { get; }

		// Token: 0x06000B2B RID: 2859 RVA: 0x00025E8F File Offset: 0x0002408F
		protected NodeEvent(AnchorName anchor, TagName tag, Mark start, Mark end) : base(start, end)
		{
			this.Anchor = anchor;
			this.Tag = tag;
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00025EA9 File Offset: 0x000240A9
		protected NodeEvent(AnchorName anchor, TagName tag) : this(anchor, tag, Mark.Empty, Mark.Empty)
		{
		}
	}
}
