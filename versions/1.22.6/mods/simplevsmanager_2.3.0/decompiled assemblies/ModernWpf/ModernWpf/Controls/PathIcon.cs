using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ModernWpf.Controls
{
	// Token: 0x02000273 RID: 627
	public class PathIcon : IconElement
	{
		// Token: 0x060011FC RID: 4604 RVA: 0x00034950 File Offset: 0x00032B50
		static PathIcon()
		{
			IconElement.ForegroundProperty.OverrideMetadata(typeof(PathIcon), new FrameworkPropertyMetadata(new PropertyChangedCallback(PathIcon.OnForegroundChanged)));
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060011FE RID: 4606 RVA: 0x000349B4 File Offset: 0x00032BB4
		// (set) Token: 0x060011FF RID: 4607 RVA: 0x000349C6 File Offset: 0x00032BC6
		public Geometry Data
		{
			get
			{
				return (Geometry)base.GetValue(PathIcon.DataProperty);
			}
			set
			{
				base.SetValue(PathIcon.DataProperty, value);
			}
		}

		// Token: 0x06001200 RID: 4608 RVA: 0x000349D4 File Offset: 0x00032BD4
		private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((PathIcon)d).ApplyData();
		}

		// Token: 0x06001201 RID: 4609 RVA: 0x000349E1 File Offset: 0x00032BE1
		private protected override void InitializeChildren()
		{
			this._path = new Path
			{
				HorizontalAlignment = 3,
				VerticalAlignment = 3,
				Stretch = 2
			};
			this.ApplyForeground();
			this.ApplyData();
			base.Children.Add(this._path);
		}

		// Token: 0x06001202 RID: 4610 RVA: 0x00034A21 File Offset: 0x00032C21
		private protected override void OnShouldInheritForegroundFromVisualParentChanged()
		{
			this.ApplyForeground();
		}

		// Token: 0x06001203 RID: 4611 RVA: 0x00034A29 File Offset: 0x00032C29
		private protected override void OnVisualParentForegroundPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			if (base.ShouldInheritForegroundFromVisualParent)
			{
				this.ApplyForeground();
			}
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x00034A39 File Offset: 0x00032C39
		private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((PathIcon)d).ApplyForeground();
		}

		// Token: 0x06001205 RID: 4613 RVA: 0x00034A46 File Offset: 0x00032C46
		private void ApplyForeground()
		{
			if (this._path != null)
			{
				this._path.Fill = (base.ShouldInheritForegroundFromVisualParent ? base.VisualParentForeground : base.Foreground);
			}
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x00034A71 File Offset: 0x00032C71
		private void ApplyData()
		{
			if (this._path != null)
			{
				this._path.Data = this.Data;
			}
		}

		// Token: 0x04000501 RID: 1281
		public static readonly DependencyProperty DataProperty = Path.DataProperty.AddOwner(typeof(PathIcon), new FrameworkPropertyMetadata(new PropertyChangedCallback(PathIcon.OnDataChanged)));

		// Token: 0x04000502 RID: 1282
		private Path _path;
	}
}
