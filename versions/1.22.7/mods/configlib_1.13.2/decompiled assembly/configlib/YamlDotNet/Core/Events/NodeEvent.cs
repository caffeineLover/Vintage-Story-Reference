using System;

namespace YamlDotNet.Core.Events
{
	// Token: 0x020001F1 RID: 497
	public abstract class NodeEvent : ParsingEvent
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x00025537 File Offset: 0x00023737
		public AnchorName Anchor { get; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x0002553F File Offset: 0x0002373F
		public TagName Tag { get; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000AEE RID: 2798
		public abstract bool IsCanonical { get; }

		// Token: 0x06000AEF RID: 2799 RVA: 0x00025547 File Offset: 0x00023747
		protected NodeEvent(AnchorName anchor, TagName tag, Mark start, Mark end) : base(start, end)
		{
			this.Anchor = anchor;
			this.Tag = tag;
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00025561 File Offset: 0x00023761
		protected NodeEvent(AnchorName anchor, TagName tag) : this(anchor, tag, Mark.Empty, Mark.Empty)
		{
		}
	}
}
