using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ModernWpf.Controls
{
	// Token: 0x02000263 RID: 611
	public class ContentPresenterEx : ContentPresenter
	{
		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06001162 RID: 4450 RVA: 0x0003323D File Offset: 0x0003143D
		// (set) Token: 0x06001163 RID: 4451 RVA: 0x0003324F File Offset: 0x0003144F
		[Localizability(14)]
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(ContentPresenterEx.FontFamilyProperty);
			}
			set
			{
				base.SetValue(ContentPresenterEx.FontFamilyProperty, value);
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06001164 RID: 4452 RVA: 0x0003325D File Offset: 0x0003145D
		// (set) Token: 0x06001165 RID: 4453 RVA: 0x0003326F File Offset: 0x0003146F
		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(ContentPresenterEx.FontStyleProperty);
			}
			set
			{
				base.SetValue(ContentPresenterEx.FontStyleProperty, value);
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06001166 RID: 4454 RVA: 0x00033282 File Offset: 0x00031482
		// (set) Token: 0x06001167 RID: 4455 RVA: 0x00033294 File Offset: 0x00031494
		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(ContentPresenterEx.FontWeightProperty);
			}
			set
			{
				base.SetValue(ContentPresenterEx.FontWeightProperty, value);
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06001168 RID: 4456 RVA: 0x000332A7 File Offset: 0x000314A7
		// (set) Token: 0x06001169 RID: 4457 RVA: 0x000332B9 File Offset: 0x000314B9
		public FontStretch FontStretch
		{
			get
			{
				return (FontStretch)base.GetValue(ContentPresenterEx.FontStretchProperty);
			}
			set
			{
				base.SetValue(ContentPresenterEx.FontStretchProperty, value);
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x0600116A RID: 4458 RVA: 0x000332CC File Offset: 0x000314CC
		// (set) Token: 0x0600116B RID: 4459 RVA: 0x000332DE File Offset: 0x000314DE
		[TypeConverter(typeof(FontSizeConverter))]
		[Localizability(0)]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(ContentPresenterEx.FontSizeProperty);
			}
			set
			{
				base.SetValue(ContentPresenterEx.FontSizeProperty, value);
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x0600116C RID: 4460 RVA: 0x000332F1 File Offset: 0x000314F1
		// (set) Token: 0x0600116D RID: 4461 RVA: 0x00033303 File Offset: 0x00031503
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(ContentPresenterEx.ForegroundProperty);
			}
			set
			{
				base.SetValue(ContentPresenterEx.ForegroundProperty, value);
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x0600116E RID: 4462 RVA: 0x00033311 File Offset: 0x00031511
		// (set) Token: 0x0600116F RID: 4463 RVA: 0x00033323 File Offset: 0x00031523
		[TypeConverter(typeof(LengthConverter))]
		public double LineHeight
		{
			get
			{
				return (double)base.GetValue(ContentPresenterEx.LineHeightProperty);
			}
			set
			{
				base.SetValue(ContentPresenterEx.LineHeightProperty, value);
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06001170 RID: 4464 RVA: 0x00033336 File Offset: 0x00031536
		// (set) Token: 0x06001171 RID: 4465 RVA: 0x00033348 File Offset: 0x00031548
		public LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return (LineStackingStrategy)base.GetValue(ContentPresenterEx.LineStackingStrategyProperty);
			}
			set
			{
				base.SetValue(ContentPresenterEx.LineStackingStrategyProperty, value);
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001172 RID: 4466 RVA: 0x0003335B File Offset: 0x0003155B
		// (set) Token: 0x06001173 RID: 4467 RVA: 0x0003336D File Offset: 0x0003156D
		public TextWrapping TextWrapping
		{
			get
			{
				return (TextWrapping)base.GetValue(ContentPresenterEx.TextWrappingProperty);
			}
			set
			{
				base.SetValue(ContentPresenterEx.TextWrappingProperty, value);
			}
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x00033380 File Offset: 0x00031580
		private static void OnTextWrappingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ContentPresenterEx contentPresenterEx = (ContentPresenterEx)d;
			if (contentPresenterEx.TextBlock != null)
			{
				contentPresenterEx.TextBlock.TextWrapping = (TextWrapping)e.NewValue;
				return;
			}
			if (contentPresenterEx.AccessText != null)
			{
				contentPresenterEx.AccessText.TextWrapping = (TextWrapping)e.NewValue;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001175 RID: 4469 RVA: 0x000333D3 File Offset: 0x000315D3
		// (set) Token: 0x06001176 RID: 4470 RVA: 0x000333DB File Offset: 0x000315DB
		private bool IsUsingDefaultTemplate { get; set; }

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001177 RID: 4471 RVA: 0x000333E4 File Offset: 0x000315E4
		// (set) Token: 0x06001178 RID: 4472 RVA: 0x000333EC File Offset: 0x000315EC
		private TextBlock TextBlock
		{
			get
			{
				return this._textBlock;
			}
			set
			{
				if (this._textBlock != null)
				{
					this._textBlock.ClearValue(TextBlock.TextWrappingProperty);
				}
				this._textBlock = value;
				if (this._textBlock != null)
				{
					this._textBlock.TextWrapping = this.TextWrapping;
				}
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001179 RID: 4473 RVA: 0x00033426 File Offset: 0x00031626
		// (set) Token: 0x0600117A RID: 4474 RVA: 0x0003342E File Offset: 0x0003162E
		private AccessText AccessText
		{
			get
			{
				return this._accessText;
			}
			set
			{
				if (this._accessText != null)
				{
					this._accessText.ClearValue(AccessText.TextWrappingProperty);
				}
				this._accessText = value;
				if (this._accessText != null)
				{
					this._accessText.TextWrapping = this.TextWrapping;
				}
			}
		}

		// Token: 0x0600117B RID: 4475 RVA: 0x00033468 File Offset: 0x00031668
		protected override DataTemplate ChooseTemplate()
		{
			object content = base.Content;
			DataTemplate dataTemplate = base.ContentTemplate;
			if (dataTemplate == null && base.ContentTemplateSelector != null)
			{
				dataTemplate = base.ContentTemplateSelector.SelectTemplate(content, this);
			}
			if (dataTemplate == null)
			{
				dataTemplate = base.ChooseTemplate();
				this.IsUsingDefaultTemplate = true;
			}
			else
			{
				this.IsUsingDefaultTemplate = false;
			}
			return dataTemplate;
		}

		// Token: 0x0600117C RID: 4476 RVA: 0x000334BC File Offset: 0x000316BC
		protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);
			if (visualAdded != null && this.IsUsingDefaultTemplate)
			{
				TextBlock textBlock = visualAdded as TextBlock;
				if (textBlock != null)
				{
					this.TextBlock = textBlock;
					return;
				}
				AccessText accessText = visualAdded as AccessText;
				if (accessText != null)
				{
					this.AccessText = accessText;
					return;
				}
			}
			else if (visualRemoved != null)
			{
				if (visualRemoved == this.TextBlock)
				{
					this.TextBlock = null;
					return;
				}
				if (visualRemoved == this.AccessText)
				{
					this.AccessText = null;
				}
			}
		}

		// Token: 0x040004C6 RID: 1222
		public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(ContentPresenterEx));

		// Token: 0x040004C7 RID: 1223
		public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(ContentPresenterEx));

		// Token: 0x040004C8 RID: 1224
		public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(ContentPresenterEx));

		// Token: 0x040004C9 RID: 1225
		public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(ContentPresenterEx));

		// Token: 0x040004CA RID: 1226
		public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(ContentPresenterEx));

		// Token: 0x040004CB RID: 1227
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(ContentPresenterEx));

		// Token: 0x040004CC RID: 1228
		public static readonly DependencyProperty LineHeightProperty = Block.LineHeightProperty.AddOwner(typeof(ContentPresenterEx));

		// Token: 0x040004CD RID: 1229
		public static readonly DependencyProperty LineStackingStrategyProperty = Block.LineStackingStrategyProperty.AddOwner(typeof(ContentPresenterEx));

		// Token: 0x040004CE RID: 1230
		public static readonly DependencyProperty TextWrappingProperty = TextBlock.TextWrappingProperty.AddOwner(typeof(ContentPresenterEx), new FrameworkPropertyMetadata(1, new PropertyChangedCallback(ContentPresenterEx.OnTextWrappingChanged)));

		// Token: 0x040004D0 RID: 1232
		private TextBlock _textBlock;

		// Token: 0x040004D1 RID: 1233
		private AccessText _accessText;
	}
}
