using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Actions;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Annotations
{
	// Token: 0x020001F9 RID: 505
	public class Annotation
	{
		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x0003D3DB File Offset: 0x0003B5DB
		[NotNull]
		public DictionaryToken AnnotationDictionary { get; }

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x0003D3E3 File Offset: 0x0003B5E3
		public AnnotationType Type { get; }

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x0003D3EB File Offset: 0x0003B5EB
		public PdfRectangle Rectangle { get; }

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x0003D3F3 File Offset: 0x0003B5F3
		[CanBeNull]
		public string Content { get; }

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000C3F RID: 3135 RVA: 0x0003D3FB File Offset: 0x0003B5FB
		[CanBeNull]
		public string Name { get; }

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x0003D403 File Offset: 0x0003B603
		[CanBeNull]
		public string ModifiedDate { get; }

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000C41 RID: 3137 RVA: 0x0003D40B File Offset: 0x0003B60B
		public AnnotationFlags Flags { get; }

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0003D413 File Offset: 0x0003B613
		public AnnotationBorder Border { get; }

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000C43 RID: 3139 RVA: 0x0003D41B File Offset: 0x0003B61B
		public IReadOnlyList<QuadPointsQuadrilateral> QuadPoints { get; }

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x0003D423 File Offset: 0x0003B623
		public PdfAction Action { get; }

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x0003D42B File Offset: 0x0003B62B
		public bool HasNormalAppearance
		{
			get
			{
				return this.normalAppearanceStream != null;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x0003D436 File Offset: 0x0003B636
		public bool HasRollOverAppearance
		{
			get
			{
				return this.rollOverAppearanceStream != null;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000C47 RID: 3143 RVA: 0x0003D441 File Offset: 0x0003B641
		public bool HasDownAppearance
		{
			get
			{
				return this.downAppearanceStream != null;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000C48 RID: 3144 RVA: 0x0003D44C File Offset: 0x0003B64C
		public Annotation InReplyTo { get; }

		// Token: 0x06000C49 RID: 3145 RVA: 0x0003D454 File Offset: 0x0003B654
		public Annotation(DictionaryToken annotationDictionary, AnnotationType type, PdfRectangle rectangle, string content, string name, string modifiedDate, AnnotationFlags flags, AnnotationBorder border, IReadOnlyList<QuadPointsQuadrilateral> quadPoints, PdfAction action, AppearanceStream normalAppearanceStream, AppearanceStream rollOverAppearanceStream, AppearanceStream downAppearanceStream, string appearanceState, Annotation inReplyTo)
		{
			if (annotationDictionary == null)
			{
				throw new ArgumentNullException("annotationDictionary");
			}
			this.AnnotationDictionary = annotationDictionary;
			this.Type = type;
			this.Rectangle = rectangle;
			this.Content = content;
			this.Name = name;
			this.ModifiedDate = modifiedDate;
			this.Flags = flags;
			this.Border = border;
			this.QuadPoints = (quadPoints ?? EmptyArray<QuadPointsQuadrilateral>.Instance);
			this.Action = action;
			this.normalAppearanceStream = normalAppearanceStream;
			this.rollOverAppearanceStream = rollOverAppearanceStream;
			this.downAppearanceStream = downAppearanceStream;
			this.appearanceState = appearanceState;
			this.InReplyTo = inReplyTo;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x0003D4F4 File Offset: 0x0003B6F4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(3, 2);
			defaultInterpolatedStringHandler.AppendFormatted<AnnotationType>(this.Type);
			defaultInterpolatedStringHandler.AppendLiteral(" - ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Content);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400067F RID: 1663
		internal readonly AppearanceStream normalAppearanceStream;

		// Token: 0x04000680 RID: 1664
		internal readonly AppearanceStream rollOverAppearanceStream;

		// Token: 0x04000681 RID: 1665
		internal readonly AppearanceStream downAppearanceStream;

		// Token: 0x04000682 RID: 1666
		internal readonly string appearanceState;
	}
}
