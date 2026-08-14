using System;
using System.Windows;
using System.Windows.Media;

namespace ModernWpf.Controls
{
	// Token: 0x02000277 RID: 631
	public class FontIconSource : IconSource
	{
		// Token: 0x1700039A RID: 922
		// (get) Token: 0x0600121D RID: 4637 RVA: 0x00034D9C File Offset: 0x00032F9C
		// (set) Token: 0x0600121E RID: 4638 RVA: 0x00034DAE File Offset: 0x00032FAE
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(FontIconSource.FontFamilyProperty);
			}
			set
			{
				base.SetValue(FontIconSource.FontFamilyProperty, value);
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x0600121F RID: 4639 RVA: 0x00034DBC File Offset: 0x00032FBC
		// (set) Token: 0x06001220 RID: 4640 RVA: 0x00034DCE File Offset: 0x00032FCE
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(FontIconSource.FontSizeProperty);
			}
			set
			{
				base.SetValue(FontIconSource.FontSizeProperty, value);
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06001221 RID: 4641 RVA: 0x00034DE1 File Offset: 0x00032FE1
		// (set) Token: 0x06001222 RID: 4642 RVA: 0x00034DF3 File Offset: 0x00032FF3
		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(FontIconSource.FontStyleProperty);
			}
			set
			{
				base.SetValue(FontIconSource.FontStyleProperty, value);
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x00034E06 File Offset: 0x00033006
		// (set) Token: 0x06001224 RID: 4644 RVA: 0x00034E18 File Offset: 0x00033018
		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(FontIconSource.FontWeightProperty);
			}
			set
			{
				base.SetValue(FontIconSource.FontWeightProperty, value);
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001225 RID: 4645 RVA: 0x00034E2B File Offset: 0x0003302B
		// (set) Token: 0x06001226 RID: 4646 RVA: 0x00034E3D File Offset: 0x0003303D
		public string Glyph
		{
			get
			{
				return (string)base.GetValue(FontIconSource.GlyphProperty);
			}
			set
			{
				base.SetValue(FontIconSource.GlyphProperty, value);
			}
		}

		// Token: 0x040005CE RID: 1486
		private const string c_fontIconSourceDefaultFontFamily = "Segoe MDL2 Assets";

		// Token: 0x040005CF RID: 1487
		public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(FontIconSource), new PropertyMetadata(new FontFamily("Segoe MDL2 Assets")));

		// Token: 0x040005D0 RID: 1488
		public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register("FontSize", typeof(double), typeof(FontIconSource), new PropertyMetadata(20.0));

		// Token: 0x040005D1 RID: 1489
		public static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register("FontStyle", typeof(FontStyle), typeof(FontIconSource), new PropertyMetadata(FontStyles.Normal));

		// Token: 0x040005D2 RID: 1490
		public static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register("FontWeight", typeof(FontWeight), typeof(FontIconSource), new PropertyMetadata(FontWeights.Normal));

		// Token: 0x040005D3 RID: 1491
		public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph", typeof(string), typeof(FontIconSource), new PropertyMetadata(string.Empty));
	}
}
