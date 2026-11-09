using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModernWpf.Controls
{
	// Token: 0x02000270 RID: 624
	public class FontIcon : IconElement
	{
		// Token: 0x1700038B RID: 907
		// (get) Token: 0x060011CE RID: 4558 RVA: 0x00034288 File Offset: 0x00032488
		// (set) Token: 0x060011CF RID: 4559 RVA: 0x0003429A File Offset: 0x0003249A
		[Bindable(true)]
		[Category("Appearance")]
		[Localizability(14)]
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(FontIcon.FontFamilyProperty);
			}
			set
			{
				base.SetValue(FontIcon.FontFamilyProperty, value);
			}
		}

		// Token: 0x060011D0 RID: 4560 RVA: 0x000342A8 File Offset: 0x000324A8
		private static void OnFontFamilyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FontIcon fontIcon = (FontIcon)d;
			if (fontIcon._textBlock != null)
			{
				fontIcon._textBlock.FontFamily = (FontFamily)e.NewValue;
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x060011D1 RID: 4561 RVA: 0x000342DB File Offset: 0x000324DB
		// (set) Token: 0x060011D2 RID: 4562 RVA: 0x000342ED File Offset: 0x000324ED
		[TypeConverter(typeof(FontSizeConverter))]
		[Bindable(true)]
		[Category("Appearance")]
		[Localizability(0)]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(FontIcon.FontSizeProperty);
			}
			set
			{
				base.SetValue(FontIcon.FontSizeProperty, value);
			}
		}

		// Token: 0x060011D3 RID: 4563 RVA: 0x00034300 File Offset: 0x00032500
		private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FontIcon fontIcon = (FontIcon)d;
			if (fontIcon._textBlock != null)
			{
				fontIcon._textBlock.FontSize = (double)e.NewValue;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x060011D4 RID: 4564 RVA: 0x00034333 File Offset: 0x00032533
		// (set) Token: 0x060011D5 RID: 4565 RVA: 0x00034345 File Offset: 0x00032545
		[Bindable(true)]
		[Category("Appearance")]
		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(FontIcon.FontStyleProperty);
			}
			set
			{
				base.SetValue(FontIcon.FontStyleProperty, value);
			}
		}

		// Token: 0x060011D6 RID: 4566 RVA: 0x00034358 File Offset: 0x00032558
		private static void OnFontStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FontIcon fontIcon = (FontIcon)d;
			if (fontIcon._textBlock != null)
			{
				fontIcon._textBlock.FontStyle = (FontStyle)e.NewValue;
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x060011D7 RID: 4567 RVA: 0x0003438B File Offset: 0x0003258B
		// (set) Token: 0x060011D8 RID: 4568 RVA: 0x0003439D File Offset: 0x0003259D
		[Bindable(true)]
		[Category("Appearance")]
		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(FontIcon.FontWeightProperty);
			}
			set
			{
				base.SetValue(FontIcon.FontWeightProperty, value);
			}
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x000343B0 File Offset: 0x000325B0
		private static void OnFontWeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FontIcon fontIcon = (FontIcon)d;
			if (fontIcon._textBlock != null)
			{
				fontIcon._textBlock.FontWeight = (FontWeight)e.NewValue;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060011DA RID: 4570 RVA: 0x000343E3 File Offset: 0x000325E3
		// (set) Token: 0x060011DB RID: 4571 RVA: 0x000343F5 File Offset: 0x000325F5
		public string Glyph
		{
			get
			{
				return (string)base.GetValue(FontIcon.GlyphProperty);
			}
			set
			{
				base.SetValue(FontIcon.GlyphProperty, value);
			}
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00034404 File Offset: 0x00032604
		private static void OnGlyphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			FontIcon fontIcon = (FontIcon)d;
			if (fontIcon._textBlock != null)
			{
				fontIcon._textBlock.Text = (string)e.NewValue;
			}
		}

		// Token: 0x060011DD RID: 4573 RVA: 0x00034438 File Offset: 0x00032638
		private protected override void InitializeChildren()
		{
			this._textBlock = new TextBlock
			{
				Style = null,
				HorizontalAlignment = 3,
				VerticalAlignment = 1,
				TextAlignment = 2,
				FontFamily = this.FontFamily,
				FontSize = this.FontSize,
				FontStyle = this.FontStyle,
				FontWeight = this.FontWeight,
				Text = this.Glyph
			};
			if (base.ShouldInheritForegroundFromVisualParent)
			{
				this._textBlock.Foreground = base.VisualParentForeground;
			}
			base.Children.Add(this._textBlock);
		}

		// Token: 0x060011DE RID: 4574 RVA: 0x000344D3 File Offset: 0x000326D3
		private protected override void OnShouldInheritForegroundFromVisualParentChanged()
		{
			if (this._textBlock != null)
			{
				if (base.ShouldInheritForegroundFromVisualParent)
				{
					this._textBlock.Foreground = base.VisualParentForeground;
					return;
				}
				this._textBlock.ClearValue(TextBlock.ForegroundProperty);
			}
		}

		// Token: 0x060011DF RID: 4575 RVA: 0x00034507 File Offset: 0x00032707
		private protected override void OnVisualParentForegroundPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			if (base.ShouldInheritForegroundFromVisualParent && this._textBlock != null)
			{
				this._textBlock.Foreground = (Brush)args.NewValue;
			}
		}

		// Token: 0x040004F6 RID: 1270
		public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(FontIcon), new FrameworkPropertyMetadata(new FontFamily("Segoe MDL2 Assets"), new PropertyChangedCallback(FontIcon.OnFontFamilyChanged)));

		// Token: 0x040004F7 RID: 1271
		public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register("FontSize", typeof(double), typeof(FontIcon), new FrameworkPropertyMetadata(20.0, new PropertyChangedCallback(FontIcon.OnFontSizeChanged)));

		// Token: 0x040004F8 RID: 1272
		public static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register("FontStyle", typeof(FontStyle), typeof(FontIcon), new FrameworkPropertyMetadata(FontStyles.Normal, new PropertyChangedCallback(FontIcon.OnFontStyleChanged)));

		// Token: 0x040004F9 RID: 1273
		public static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register("FontWeight", typeof(FontWeight), typeof(FontIcon), new FrameworkPropertyMetadata(FontWeights.Normal, new PropertyChangedCallback(FontIcon.OnFontWeightChanged)));

		// Token: 0x040004FA RID: 1274
		public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph", typeof(string), typeof(FontIcon), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(FontIcon.OnGlyphChanged)));

		// Token: 0x040004FB RID: 1275
		private TextBlock _textBlock;
	}
}
