using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001F2 RID: 498
	internal static class PageSizeExtensions
	{
		// Token: 0x06000C06 RID: 3078 RVA: 0x0003B8AC File Offset: 0x00039AAC
		public static PageSize GetPageSize(this PdfRectangle rectangle)
		{
			PageSize result;
			if (!PageSizeExtensions.Lookup.TryGetValue(new PageSizeExtensions.WidthHeight(rectangle.Width, rectangle.Height), ref result) && !PageSizeExtensions.Lookup.TryGetValue(new PageSizeExtensions.WidthHeight(rectangle.Height, rectangle.Width), ref result))
			{
				return PageSize.Custom;
			}
			return result;
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0003B900 File Offset: 0x00039B00
		public static bool TryGetPdfRectangle(this PageSize size, out PdfRectangle rectangle)
		{
			rectangle = default(PdfRectangle);
			KeyValuePair<PageSizeExtensions.WidthHeight, PageSize> keyValuePair = Enumerable.FirstOrDefault<KeyValuePair<PageSizeExtensions.WidthHeight, PageSize>>(PageSizeExtensions.Lookup, (KeyValuePair<PageSizeExtensions.WidthHeight, PageSize> x) => x.Value == size);
			if (keyValuePair.Key.Width > 0.0)
			{
				rectangle = new PdfRectangle(0.0, 0.0, keyValuePair.Key.Width, keyValuePair.Key.Height);
			}
			return keyValuePair.Key.Width > 0.0;
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x0003B9A8 File Offset: 0x00039BA8
		// Note: this type is marked as 'beforefieldinit'.
		static PageSizeExtensions()
		{
			Dictionary<PageSizeExtensions.WidthHeight, PageSize> dictionary = new Dictionary<PageSizeExtensions.WidthHeight, PageSize>();
			dictionary.Add(new PageSizeExtensions.WidthHeight(2384.0, 3370.0), PageSize.A0);
			dictionary.Add(new PageSizeExtensions.WidthHeight(1684.0, 2384.0), PageSize.A1);
			dictionary.Add(new PageSizeExtensions.WidthHeight(1190.0, 1684.0), PageSize.A2);
			dictionary.Add(new PageSizeExtensions.WidthHeight(1191.0, 1684.0), PageSize.A2);
			dictionary.Add(new PageSizeExtensions.WidthHeight(842.0, 1190.0), PageSize.A3);
			dictionary.Add(new PageSizeExtensions.WidthHeight(842.0, 1191.0), PageSize.A3);
			dictionary.Add(new PageSizeExtensions.WidthHeight(595.0, 842.0), PageSize.A4);
			dictionary.Add(new PageSizeExtensions.WidthHeight(595.0, 841.0), PageSize.A4);
			dictionary.Add(new PageSizeExtensions.WidthHeight(420.0, 595.0), PageSize.A5);
			dictionary.Add(new PageSizeExtensions.WidthHeight(298.0, 420.0), PageSize.A6);
			dictionary.Add(new PageSizeExtensions.WidthHeight(210.0, 298.0), PageSize.A7);
			dictionary.Add(new PageSizeExtensions.WidthHeight(147.0, 210.0), PageSize.A8);
			dictionary.Add(new PageSizeExtensions.WidthHeight(105.0, 147.0), PageSize.A9);
			dictionary.Add(new PageSizeExtensions.WidthHeight(74.0, 105.0), PageSize.A10);
			dictionary.Add(new PageSizeExtensions.WidthHeight(612.0, 792.0), PageSize.Letter);
			dictionary.Add(new PageSizeExtensions.WidthHeight(612.0, 1008.0), PageSize.Legal);
			dictionary.Add(new PageSizeExtensions.WidthHeight(1224.0, 792.0), PageSize.Ledger);
			dictionary.Add(new PageSizeExtensions.WidthHeight(792.0, 1224.0), PageSize.Tabloid);
			dictionary.Add(new PageSizeExtensions.WidthHeight(540.0, 720.0), PageSize.Executive);
			dictionary.Add(new PageSizeExtensions.WidthHeight(522.0, 756.0), PageSize.Executive);
			PageSizeExtensions.Lookup = dictionary;
		}

		// Token: 0x0400065A RID: 1626
		private static readonly Dictionary<PageSizeExtensions.WidthHeight, PageSize> Lookup;

		// Token: 0x020002C7 RID: 711
		private struct WidthHeight
		{
			// Token: 0x17000423 RID: 1059
			// (get) Token: 0x06000F34 RID: 3892 RVA: 0x000442E1 File Offset: 0x000424E1
			public readonly double Width { get; }

			// Token: 0x17000424 RID: 1060
			// (get) Token: 0x06000F35 RID: 3893 RVA: 0x000442E9 File Offset: 0x000424E9
			public readonly double Height { get; }

			// Token: 0x06000F36 RID: 3894 RVA: 0x000442F1 File Offset: 0x000424F1
			public WidthHeight(double width, double height)
			{
				this.Width = width;
				this.Height = height;
			}

			// Token: 0x06000F37 RID: 3895 RVA: 0x00044304 File Offset: 0x00042504
			public override bool Equals(object obj)
			{
				if (obj is PageSizeExtensions.WidthHeight)
				{
					PageSizeExtensions.WidthHeight widthHeight = (PageSizeExtensions.WidthHeight)obj;
					if (Math.Round(this.Width) == Math.Round(widthHeight.Width))
					{
						return Math.Round(this.Height) == Math.Round(widthHeight.Height);
					}
				}
				return false;
			}

			// Token: 0x06000F38 RID: 3896 RVA: 0x00044354 File Offset: 0x00042554
			public override int GetHashCode()
			{
				return (859600377 * -1521134295 + Math.Round(this.Width).GetHashCode()) * -1521134295 + Math.Round(this.Height).GetHashCode();
			}
		}
	}
}
