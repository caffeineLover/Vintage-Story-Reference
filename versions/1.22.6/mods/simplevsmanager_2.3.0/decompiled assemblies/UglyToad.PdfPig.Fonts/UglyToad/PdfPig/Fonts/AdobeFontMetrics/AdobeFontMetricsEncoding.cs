using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Fonts.Encodings;

namespace UglyToad.PdfPig.Fonts.AdobeFontMetrics
{
	// Token: 0x020000AF RID: 175
	public class AdobeFontMetricsEncoding : Encoding
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x0001E0A3 File Offset: 0x0001C2A3
		public override string EncodingName { get; } = "AFM";

		// Token: 0x06000475 RID: 1141 RVA: 0x0001E0AC File Offset: 0x0001C2AC
		public AdobeFontMetricsEncoding(AdobeFontMetrics metrics)
		{
			if (metrics == null)
			{
				throw new ArgumentNullException("metrics");
			}
			foreach (KeyValuePair<string, AdobeFontMetricsIndividualCharacterMetric> keyValuePair in metrics.CharacterMetrics)
			{
				base.Add(keyValuePair.Value.CharacterCode, keyValuePair.Key);
			}
		}
	}
}
