using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x02000268 RID: 616
	public class RatingItemFontInfo : RatingItemInfo
	{
		// Token: 0x17000375 RID: 885
		// (get) Token: 0x0600118C RID: 4492 RVA: 0x0003378A File Offset: 0x0003198A
		// (set) Token: 0x0600118D RID: 4493 RVA: 0x0003379C File Offset: 0x0003199C
		public string DisabledGlyph
		{
			get
			{
				return (string)base.GetValue(RatingItemFontInfo.DisabledGlyphProperty);
			}
			set
			{
				base.SetValue(RatingItemFontInfo.DisabledGlyphProperty, value);
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x0600118E RID: 4494 RVA: 0x000337AA File Offset: 0x000319AA
		// (set) Token: 0x0600118F RID: 4495 RVA: 0x000337BC File Offset: 0x000319BC
		public string Glyph
		{
			get
			{
				return (string)base.GetValue(RatingItemFontInfo.GlyphProperty);
			}
			set
			{
				base.SetValue(RatingItemFontInfo.GlyphProperty, value);
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06001190 RID: 4496 RVA: 0x000337CA File Offset: 0x000319CA
		// (set) Token: 0x06001191 RID: 4497 RVA: 0x000337DC File Offset: 0x000319DC
		public string PlaceholderGlyph
		{
			get
			{
				return (string)base.GetValue(RatingItemFontInfo.PlaceholderGlyphProperty);
			}
			set
			{
				base.SetValue(RatingItemFontInfo.PlaceholderGlyphProperty, value);
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06001192 RID: 4498 RVA: 0x000337EA File Offset: 0x000319EA
		// (set) Token: 0x06001193 RID: 4499 RVA: 0x000337FC File Offset: 0x000319FC
		public string PointerOverGlyph
		{
			get
			{
				return (string)base.GetValue(RatingItemFontInfo.PointerOverGlyphProperty);
			}
			set
			{
				base.SetValue(RatingItemFontInfo.PointerOverGlyphProperty, value);
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06001194 RID: 4500 RVA: 0x0003380A File Offset: 0x00031A0A
		// (set) Token: 0x06001195 RID: 4501 RVA: 0x0003381C File Offset: 0x00031A1C
		public string PointerOverPlaceholderGlyph
		{
			get
			{
				return (string)base.GetValue(RatingItemFontInfo.PointerOverPlaceholderGlyphProperty);
			}
			set
			{
				base.SetValue(RatingItemFontInfo.PointerOverPlaceholderGlyphProperty, value);
			}
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06001196 RID: 4502 RVA: 0x0003382A File Offset: 0x00031A2A
		// (set) Token: 0x06001197 RID: 4503 RVA: 0x0003383C File Offset: 0x00031A3C
		public string UnsetGlyph
		{
			get
			{
				return (string)base.GetValue(RatingItemFontInfo.UnsetGlyphProperty);
			}
			set
			{
				base.SetValue(RatingItemFontInfo.UnsetGlyphProperty, value);
			}
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x0003384A File Offset: 0x00031A4A
		protected override Freezable CreateInstanceCore()
		{
			return new RatingItemFontInfo();
		}

		// Token: 0x040004D9 RID: 1241
		public static readonly DependencyProperty DisabledGlyphProperty = DependencyProperty.Register("DisabledGlyph", typeof(string), typeof(RatingItemFontInfo), new PropertyMetadata(string.Empty));

		// Token: 0x040004DA RID: 1242
		public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register("Glyph", typeof(string), typeof(RatingItemFontInfo), new PropertyMetadata(string.Empty));

		// Token: 0x040004DB RID: 1243
		public static readonly DependencyProperty PlaceholderGlyphProperty = DependencyProperty.Register("PlaceholderGlyph", typeof(string), typeof(RatingItemFontInfo), new PropertyMetadata(string.Empty));

		// Token: 0x040004DC RID: 1244
		public static readonly DependencyProperty PointerOverGlyphProperty = DependencyProperty.Register("PointerOverGlyph", typeof(string), typeof(RatingItemFontInfo), new PropertyMetadata(string.Empty));

		// Token: 0x040004DD RID: 1245
		public static readonly DependencyProperty PointerOverPlaceholderGlyphProperty = DependencyProperty.Register("PointerOverPlaceholderGlyph", typeof(string), typeof(RatingItemFontInfo), new PropertyMetadata(string.Empty));

		// Token: 0x040004DE RID: 1246
		public static readonly DependencyProperty UnsetGlyphProperty = DependencyProperty.Register("UnsetGlyph", typeof(string), typeof(RatingItemFontInfo), new PropertyMetadata(string.Empty));
	}
}
