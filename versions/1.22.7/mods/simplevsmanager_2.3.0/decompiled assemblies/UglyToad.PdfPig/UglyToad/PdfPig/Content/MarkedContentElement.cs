using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Graphics;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001E9 RID: 489
	public class MarkedContentElement
	{
		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x0003A0CD File Offset: 0x000382CD
		public int MarkedContentIdentifier { get; }

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x0003A0D5 File Offset: 0x000382D5
		public int Index { get; }

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000BBD RID: 3005 RVA: 0x0003A0DD File Offset: 0x000382DD
		public string Tag { get; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x0003A0E5 File Offset: 0x000382E5
		public DictionaryToken Properties { get; }

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x0003A0ED File Offset: 0x000382ED
		public bool IsArtifact { get; }

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x0003A0F5 File Offset: 0x000382F5
		public IReadOnlyList<MarkedContentElement> Children { get; }

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x0003A0FD File Offset: 0x000382FD
		public IReadOnlyList<Letter> Letters { get; }

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x0003A105 File Offset: 0x00038305
		public IReadOnlyList<PdfPath> Paths { get; }

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x0003A10D File Offset: 0x0003830D
		public IReadOnlyList<IPdfImage> Images { get; }

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x0003A115 File Offset: 0x00038315
		public string Language { get; }

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x0003A11D File Offset: 0x0003831D
		public string ActualText { get; }

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x0003A125 File Offset: 0x00038325
		public string AlternateDescription { get; }

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000BC7 RID: 3015 RVA: 0x0003A12D File Offset: 0x0003832D
		public string ExpandedForm { get; }

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0003A138 File Offset: 0x00038338
		public MarkedContentElement(int markedContentIdentifier, NameToken tag, DictionaryToken properties, string language, string actualText, string alternateDescription, string expandedForm, bool isArtifact, IReadOnlyList<MarkedContentElement> children, IReadOnlyList<Letter> letters, IReadOnlyList<PdfPath> paths, IReadOnlyList<IPdfImage> images, int index)
		{
			this.MarkedContentIdentifier = markedContentIdentifier;
			this.Tag = tag;
			this.Language = language;
			this.ActualText = actualText;
			this.AlternateDescription = alternateDescription;
			this.ExpandedForm = expandedForm;
			this.Properties = (properties ?? new DictionaryToken(new Dictionary<NameToken, IToken>()));
			this.IsArtifact = isArtifact;
			if (children == null)
			{
				throw new ArgumentNullException("children");
			}
			this.Children = children;
			if (letters == null)
			{
				throw new ArgumentNullException("letters");
			}
			this.Letters = letters;
			if (paths == null)
			{
				throw new ArgumentNullException("paths");
			}
			this.Paths = paths;
			if (images == null)
			{
				throw new ArgumentNullException("images");
			}
			this.Images = images;
			this.Index = index;
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0003A200 File Offset: 0x00038400
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(40, 5);
			defaultInterpolatedStringHandler.AppendLiteral("Id=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Index);
			defaultInterpolatedStringHandler.AppendLiteral(", MCID=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.MarkedContentIdentifier);
			defaultInterpolatedStringHandler.AppendLiteral(", Tag=");
			defaultInterpolatedStringHandler.AppendFormatted(this.Tag);
			defaultInterpolatedStringHandler.AppendLiteral(", Properties=");
			defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(this.Properties);
			defaultInterpolatedStringHandler.AppendLiteral(", Children=");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Children.Count);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
