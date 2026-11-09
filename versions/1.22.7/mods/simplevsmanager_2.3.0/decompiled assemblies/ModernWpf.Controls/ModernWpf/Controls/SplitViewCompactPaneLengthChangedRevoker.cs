using System;

namespace ModernWpf.Controls
{
	// Token: 0x020000DB RID: 219
	internal class SplitViewCompactPaneLengthChangedRevoker : EventRevoker<SplitView, DependencyPropertyChangedCallback>
	{
		// Token: 0x06000ACB RID: 2763 RVA: 0x00024F3C File Offset: 0x0002313C
		public SplitViewCompactPaneLengthChangedRevoker(SplitView source, DependencyPropertyChangedCallback handler) : base(source, handler)
		{
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x00024F46 File Offset: 0x00023146
		protected override void AddHandler(SplitView source, DependencyPropertyChangedCallback handler)
		{
			source.CompactPaneLengthChanged += handler;
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x00024F4F File Offset: 0x0002314F
		protected override void RemoveHandler(SplitView source, DependencyPropertyChangedCallback handler)
		{
			source.CompactPaneLengthChanged -= handler;
		}
	}
}
