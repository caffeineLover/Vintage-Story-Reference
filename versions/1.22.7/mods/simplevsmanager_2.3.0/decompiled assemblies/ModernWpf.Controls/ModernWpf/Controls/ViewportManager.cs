using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000AC RID: 172
	internal abstract class ViewportManager
	{
		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000867 RID: 2151
		public abstract UIElement SuggestedAnchor { get; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000868 RID: 2152
		// (set) Token: 0x06000869 RID: 2153
		public abstract double HorizontalCacheLength { get; set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600086A RID: 2154
		// (set) Token: 0x0600086B RID: 2155
		public abstract double VerticalCacheLength { get; set; }

		// Token: 0x0600086C RID: 2156
		public abstract Rect GetLayoutVisibleWindow();

		// Token: 0x0600086D RID: 2157
		public abstract Rect GetLayoutRealizationWindow();

		// Token: 0x0600086E RID: 2158
		public abstract void SetLayoutExtent(Rect extent);

		// Token: 0x0600086F RID: 2159
		public abstract Point GetOrigin();

		// Token: 0x06000870 RID: 2160
		public abstract void OnLayoutChanged(bool isVirtualizing);

		// Token: 0x06000871 RID: 2161
		public abstract void OnElementPrepared(UIElement element);

		// Token: 0x06000872 RID: 2162
		public abstract void OnElementCleared(UIElement element);

		// Token: 0x06000873 RID: 2163
		public abstract void OnOwnerMeasuring();

		// Token: 0x06000874 RID: 2164
		public abstract void OnOwnerArranged();

		// Token: 0x06000875 RID: 2165
		public abstract void OnMakeAnchor(UIElement anchor, bool isAnchorOutsideRealizedRange);

		// Token: 0x06000876 RID: 2166
		public abstract void ResetScrollers();

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000877 RID: 2167
		public abstract UIElement MadeAnchor { get; }
	}
}
