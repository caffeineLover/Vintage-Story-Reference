using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ModernWpf.Media.Animation
{
	// Token: 0x02000247 RID: 583
	internal class NavigationAnimation
	{
		// Token: 0x060010FF RID: 4351 RVA: 0x000320F6 File Offset: 0x000302F6
		static NavigationAnimation()
		{
			NavigationAnimation._defaultBitmapCache.Freeze();
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0003210C File Offset: 0x0003030C
		public NavigationAnimation(FrameworkElement element, Storyboard storyboard)
		{
			this._element = element;
			this._storyboard = storyboard;
			this._storyboard.CurrentStateInvalidated += new EventHandler(this.OnCurrentStateInvalidated);
			this._storyboard.Completed += new EventHandler(this.OnCompleted);
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06001101 RID: 4353 RVA: 0x00032164 File Offset: 0x00030364
		// (remove) Token: 0x06001102 RID: 4354 RVA: 0x0003219C File Offset: 0x0003039C
		public event EventHandler Completed;

		// Token: 0x06001103 RID: 4355 RVA: 0x000321D1 File Offset: 0x000303D1
		public void Begin()
		{
			if (!(this._element.CacheMode is BitmapCache))
			{
				this._element.SetCurrentValue(UIElement.CacheModeProperty, this.GetBitmapCache());
			}
			this._storyboard.Begin(this._element, true);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00032210 File Offset: 0x00030410
		public void Stop()
		{
			if (this._currentState != 2)
			{
				this._storyboard.Stop(this._element);
			}
			this._element.InvalidateProperty(UIElement.CacheModeProperty);
			this._element.InvalidateProperty(UIElement.RenderTransformProperty);
			this._element.InvalidateProperty(UIElement.RenderTransformOriginProperty);
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00032268 File Offset: 0x00030468
		private void OnCurrentStateInvalidated(object sender, EventArgs e)
		{
			Clock clock = sender as Clock;
			if (clock != null)
			{
				this._currentState = clock.CurrentState;
			}
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x0003228B File Offset: 0x0003048B
		private void OnCompleted(object sender, EventArgs e)
		{
			EventHandler completed = this.Completed;
			if (completed == null)
			{
				return;
			}
			completed.Invoke(this, EventArgs.Empty);
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x000322A4 File Offset: 0x000304A4
		private BitmapCache GetBitmapCache()
		{
			return new BitmapCache(VisualTreeHelper.GetDpi(this._element).PixelsPerDip);
		}

		// Token: 0x0400049A RID: 1178
		private static readonly BitmapCache _defaultBitmapCache = new BitmapCache();

		// Token: 0x0400049B RID: 1179
		private readonly FrameworkElement _element;

		// Token: 0x0400049C RID: 1180
		private readonly Storyboard _storyboard;

		// Token: 0x0400049D RID: 1181
		private ClockState _currentState = 2;
	}
}
