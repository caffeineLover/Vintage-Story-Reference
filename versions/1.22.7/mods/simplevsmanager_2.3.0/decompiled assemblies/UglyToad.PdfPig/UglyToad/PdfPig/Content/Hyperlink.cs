using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Annotations;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001E3 RID: 483
	public class Hyperlink
	{
		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x00039B86 File Offset: 0x00037D86
		public PdfRectangle Bounds { get; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x00039B8E File Offset: 0x00037D8E
		public string Text { get; }

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000B78 RID: 2936 RVA: 0x00039B96 File Offset: 0x00037D96
		public IReadOnlyList<Letter> Letters { get; }

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000B79 RID: 2937 RVA: 0x00039B9E File Offset: 0x00037D9E
		// (set) Token: 0x06000B7A RID: 2938 RVA: 0x00039BA6 File Offset: 0x00037DA6
		public string Uri { get; set; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x00039BAF File Offset: 0x00037DAF
		public Annotation Annotation { get; }

		// Token: 0x06000B7C RID: 2940 RVA: 0x00039BB8 File Offset: 0x00037DB8
		public Hyperlink(PdfRectangle bounds, IReadOnlyList<Letter> letters, string text, string uri, Annotation annotation)
		{
			this.Bounds = bounds;
			this.Text = (text ?? string.Empty);
			if (letters == null)
			{
				throw new ArgumentNullException("letters");
			}
			this.Letters = letters;
			this.Uri = (uri ?? string.Empty);
			if (annotation == null)
			{
				throw new ArgumentNullException("annotation");
			}
			this.Annotation = annotation;
		}

		// Token: 0x06000B7D RID: 2941 RVA: 0x00039C20 File Offset: 0x00037E20
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(9, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Link: ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Text);
			defaultInterpolatedStringHandler.AppendLiteral(" (");
			defaultInterpolatedStringHandler.AppendFormatted(this.Uri);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
