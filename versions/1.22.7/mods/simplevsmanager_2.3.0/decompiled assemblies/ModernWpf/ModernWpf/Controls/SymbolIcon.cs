using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModernWpf.Controls
{
	// Token: 0x02000275 RID: 629
	public sealed class SymbolIcon : IconElement
	{
		// Token: 0x06001207 RID: 4615 RVA: 0x00034A8C File Offset: 0x00032C8C
		public SymbolIcon()
		{
		}

		// Token: 0x06001208 RID: 4616 RVA: 0x00034A94 File Offset: 0x00032C94
		public SymbolIcon(Symbol symbol)
		{
			this.Symbol = symbol;
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001209 RID: 4617 RVA: 0x00034AA3 File Offset: 0x00032CA3
		// (set) Token: 0x0600120A RID: 4618 RVA: 0x00034AB5 File Offset: 0x00032CB5
		public Symbol Symbol
		{
			get
			{
				return (Symbol)base.GetValue(SymbolIcon.SymbolProperty);
			}
			set
			{
				base.SetValue(SymbolIcon.SymbolProperty, value);
			}
		}

		// Token: 0x0600120B RID: 4619 RVA: 0x00034AC8 File Offset: 0x00032CC8
		private static void OnSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((SymbolIcon)d).OnSymbolChanged(e);
		}

		// Token: 0x0600120C RID: 4620 RVA: 0x00034AD6 File Offset: 0x00032CD6
		private void OnSymbolChanged(DependencyPropertyChangedEventArgs e)
		{
			if (this._textBlock != null)
			{
				this._textBlock.Text = SymbolIcon.ConvertToString((Symbol)e.NewValue);
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x0600120D RID: 4621 RVA: 0x00034AFC File Offset: 0x00032CFC
		// (set) Token: 0x0600120E RID: 4622 RVA: 0x00034B0E File Offset: 0x00032D0E
		internal double FontSize
		{
			get
			{
				return (double)base.GetValue(SymbolIcon.FontSizeProperty);
			}
			set
			{
				base.SetValue(SymbolIcon.FontSizeProperty, value);
			}
		}

		// Token: 0x0600120F RID: 4623 RVA: 0x00034B21 File Offset: 0x00032D21
		private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((SymbolIcon)d).OnFontSizeChanged(e);
		}

		// Token: 0x06001210 RID: 4624 RVA: 0x00034B2F File Offset: 0x00032D2F
		private void OnFontSizeChanged(DependencyPropertyChangedEventArgs e)
		{
			if (this._textBlock != null)
			{
				this._textBlock.FontSize = (double)e.NewValue;
			}
		}

		// Token: 0x06001211 RID: 4625 RVA: 0x00034B50 File Offset: 0x00032D50
		private protected override void InitializeChildren()
		{
			this._textBlock = new TextBlock
			{
				Style = null,
				HorizontalAlignment = 3,
				VerticalAlignment = 1,
				TextAlignment = 2,
				FontSize = this.FontSize,
				FontStyle = FontStyles.Normal,
				FontWeight = FontWeights.Normal,
				Text = SymbolIcon.ConvertToString(this.Symbol)
			};
			this._textBlock.SetResourceReference(TextBlock.FontFamilyProperty, "SymbolThemeFontFamily");
			if (base.ShouldInheritForegroundFromVisualParent)
			{
				this._textBlock.Foreground = base.VisualParentForeground;
			}
			base.Children.Add(this._textBlock);
		}

		// Token: 0x06001212 RID: 4626 RVA: 0x00034BF7 File Offset: 0x00032DF7
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

		// Token: 0x06001213 RID: 4627 RVA: 0x00034C2B File Offset: 0x00032E2B
		private protected override void OnVisualParentForegroundPropertyChanged(DependencyPropertyChangedEventArgs args)
		{
			if (base.ShouldInheritForegroundFromVisualParent && this._textBlock != null)
			{
				this._textBlock.Foreground = (Brush)args.NewValue;
			}
		}

		// Token: 0x06001214 RID: 4628 RVA: 0x00034C54 File Offset: 0x00032E54
		private static string ConvertToString(Symbol symbol)
		{
			return char.ConvertFromUtf32((int)symbol).ToString();
		}

		// Token: 0x040005C9 RID: 1481
		public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register("Symbol", typeof(Symbol), typeof(SymbolIcon), new PropertyMetadata(Symbol.Emoji, new PropertyChangedCallback(SymbolIcon.OnSymbolChanged)));

		// Token: 0x040005CA RID: 1482
		internal static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register("FontSize", typeof(double), typeof(SymbolIcon), new PropertyMetadata(20.0, new PropertyChangedCallback(SymbolIcon.OnFontSizeChanged)));

		// Token: 0x040005CB RID: 1483
		private TextBlock _textBlock;
	}
}
