using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Geometry;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.PdfFonts.CidFonts
{
	// Token: 0x020000C1 RID: 193
	internal class VerticalWritingMetrics
	{
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x00015F16 File Offset: 0x00014116
		public VerticalVectorComponents DefaultVerticalWritingMetrics { get; }

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00015F1E File Offset: 0x0001411E
		[NotNull]
		public IReadOnlyDictionary<int, double> IndividualVerticalWritingDisplacements { get; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x00015F26 File Offset: 0x00014126
		[NotNull]
		public IReadOnlyDictionary<int, PdfVector> IndividualVerticalWritingPositions { get; }

		// Token: 0x060003B9 RID: 953 RVA: 0x00015F2E File Offset: 0x0001412E
		public VerticalWritingMetrics(VerticalVectorComponents defaultVerticalWritingMetrics, [CanBeNull] IReadOnlyDictionary<int, double> individualVerticalWritingDisplacements, [CanBeNull] IReadOnlyDictionary<int, PdfVector> individualVerticalWritingPositions)
		{
			this.DefaultVerticalWritingMetrics = defaultVerticalWritingMetrics;
			this.IndividualVerticalWritingDisplacements = (individualVerticalWritingDisplacements ?? new Dictionary<int, double>(0));
			this.IndividualVerticalWritingPositions = (individualVerticalWritingPositions ?? new Dictionary<int, PdfVector>(0));
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00015F60 File Offset: 0x00014160
		public PdfVector GetPositionVector(int characterIdentifier, double glyphWidth)
		{
			PdfVector result;
			if (this.IndividualVerticalWritingPositions.TryGetValue(characterIdentifier, ref result))
			{
				return result;
			}
			return this.DefaultVerticalWritingMetrics.GetPositionVector(glyphWidth);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00015F90 File Offset: 0x00014190
		public PdfVector GetDisplacementVector(int characterIdentifier)
		{
			double y;
			if (this.IndividualVerticalWritingDisplacements.TryGetValue(characterIdentifier, ref y))
			{
				return new PdfVector(0.0, y);
			}
			return this.DefaultVerticalWritingMetrics.GetDisplacementVector();
		}
	}
}
