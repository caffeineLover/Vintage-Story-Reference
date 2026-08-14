using System;
using System.Windows;
using System.Windows.Media;

namespace ModernWpf.Controls
{
	// Token: 0x02000279 RID: 633
	public class PathIconSource : IconSource
	{
		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x0600122D RID: 4653 RVA: 0x00034FA7 File Offset: 0x000331A7
		// (set) Token: 0x0600122E RID: 4654 RVA: 0x00034FB9 File Offset: 0x000331B9
		public Geometry Data
		{
			get
			{
				return (Geometry)base.GetValue(PathIconSource.DataProperty);
			}
			set
			{
				base.SetValue(PathIconSource.DataProperty, value);
			}
		}

		// Token: 0x040005D5 RID: 1493
		public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(Geometry), typeof(PathIconSource));
	}
}
