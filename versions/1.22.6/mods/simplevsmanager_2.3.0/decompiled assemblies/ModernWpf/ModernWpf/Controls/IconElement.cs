using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace ModernWpf.Controls
{
	// Token: 0x02000271 RID: 625
	[TypeConverter(typeof(IconElementConverter))]
	public abstract class IconElement : FrameworkElement
	{
		// Token: 0x060011E1 RID: 4577 RVA: 0x00034672 File Offset: 0x00032872
		private protected IconElement()
		{
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x00034681 File Offset: 0x00032881
		private static void OnForegroundPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((IconElement)sender).OnForegroundPropertyChanged(args);
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x00034690 File Offset: 0x00032890
		private void OnForegroundPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			BaseValueSource baseValueSource = DependencyPropertyHelper.GetValueSource(this, args.Property).BaseValueSource;
			this._isForegroundDefaultOrInherited = (baseValueSource <= 2);
			this.UpdateShouldInheritForegroundFromVisualParent();
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x000346C6 File Offset: 0x000328C6
		// (set) Token: 0x060011E5 RID: 4581 RVA: 0x000346D8 File Offset: 0x000328D8
		[Bindable(true)]
		[Category("Appearance")]
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(IconElement.ForegroundProperty);
			}
			set
			{
				base.SetValue(IconElement.ForegroundProperty, value);
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060011E6 RID: 4582 RVA: 0x000346E6 File Offset: 0x000328E6
		// (set) Token: 0x060011E7 RID: 4583 RVA: 0x000346F8 File Offset: 0x000328F8
		private protected Brush VisualParentForeground
		{
			get
			{
				return (Brush)base.GetValue(IconElement.VisualParentForegroundProperty);
			}
			set
			{
				base.SetValue(IconElement.VisualParentForegroundProperty, value);
			}
		}

		// Token: 0x060011E8 RID: 4584 RVA: 0x00034706 File Offset: 0x00032906
		private static void OnVisualParentForegroundPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
		{
			((IconElement)sender).OnVisualParentForegroundPropertyChanged(args);
		}

		// Token: 0x060011E9 RID: 4585 RVA: 0x00034714 File Offset: 0x00032914
		private protected virtual void OnVisualParentForegroundPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060011EA RID: 4586 RVA: 0x00034716 File Offset: 0x00032916
		// (set) Token: 0x060011EB RID: 4587 RVA: 0x00034720 File Offset: 0x00032920
		private protected bool ShouldInheritForegroundFromVisualParent
		{
			get
			{
				return this._shouldInheritForegroundFromVisualParent;
			}
			private set
			{
				if (this._shouldInheritForegroundFromVisualParent != value)
				{
					this._shouldInheritForegroundFromVisualParent = value;
					if (this._shouldInheritForegroundFromVisualParent)
					{
						base.SetBinding(IconElement.VisualParentForegroundProperty, new Binding
						{
							Path = new PropertyPath(TextElement.ForegroundProperty),
							Source = base.VisualParent
						});
					}
					else
					{
						base.ClearValue(IconElement.VisualParentForegroundProperty);
					}
					this.OnShouldInheritForegroundFromVisualParentChanged();
				}
			}
		}

		// Token: 0x060011EC RID: 4588 RVA: 0x00034785 File Offset: 0x00032985
		private protected virtual void OnShouldInheritForegroundFromVisualParentChanged()
		{
		}

		// Token: 0x060011ED RID: 4589 RVA: 0x00034787 File Offset: 0x00032987
		private void UpdateShouldInheritForegroundFromVisualParent()
		{
			this.ShouldInheritForegroundFromVisualParent = (this._isForegroundDefaultOrInherited && base.Parent != null && base.VisualParent != null && base.Parent != base.VisualParent);
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060011EE RID: 4590 RVA: 0x000347BB File Offset: 0x000329BB
		private protected UIElementCollection Children
		{
			get
			{
				this.EnsureLayoutRoot();
				return this._layoutRoot.Children;
			}
		}

		// Token: 0x060011EF RID: 4591
		private protected abstract void InitializeChildren();

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x060011F0 RID: 4592 RVA: 0x000347CE File Offset: 0x000329CE
		protected override int VisualChildrenCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060011F1 RID: 4593 RVA: 0x000347D1 File Offset: 0x000329D1
		protected override Visual GetVisualChild(int index)
		{
			if (index == 0)
			{
				this.EnsureLayoutRoot();
				return this._layoutRoot;
			}
			throw new ArgumentOutOfRangeException("index");
		}

		// Token: 0x060011F2 RID: 4594 RVA: 0x000347ED File Offset: 0x000329ED
		protected override Size MeasureOverride(Size availableSize)
		{
			this.EnsureLayoutRoot();
			this._layoutRoot.Measure(availableSize);
			return this._layoutRoot.DesiredSize;
		}

		// Token: 0x060011F3 RID: 4595 RVA: 0x0003480C File Offset: 0x00032A0C
		protected override Size ArrangeOverride(Size finalSize)
		{
			this.EnsureLayoutRoot();
			this._layoutRoot.Arrange(new Rect(default(Point), finalSize));
			return finalSize;
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x0003483A File Offset: 0x00032A3A
		protected override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			this.UpdateShouldInheritForegroundFromVisualParent();
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x00034849 File Offset: 0x00032A49
		private void EnsureLayoutRoot()
		{
			if (this._layoutRoot != null)
			{
				return;
			}
			this._layoutRoot = new Grid
			{
				Background = Brushes.Transparent,
				SnapsToDevicePixels = true
			};
			this.InitializeChildren();
			base.AddVisualChild(this._layoutRoot);
		}

		// Token: 0x040004FC RID: 1276
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(IconElement), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, 32, new PropertyChangedCallback(IconElement.OnForegroundPropertyChanged)));

		// Token: 0x040004FD RID: 1277
		private static readonly DependencyProperty VisualParentForegroundProperty = DependencyProperty.Register("VisualParentForeground", typeof(Brush), typeof(IconElement), new PropertyMetadata(null, new PropertyChangedCallback(IconElement.OnVisualParentForegroundPropertyChanged)));

		// Token: 0x040004FE RID: 1278
		private Grid _layoutRoot;

		// Token: 0x040004FF RID: 1279
		private bool _isForegroundDefaultOrInherited = true;

		// Token: 0x04000500 RID: 1280
		private bool _shouldInheritForegroundFromVisualParent;
	}
}
