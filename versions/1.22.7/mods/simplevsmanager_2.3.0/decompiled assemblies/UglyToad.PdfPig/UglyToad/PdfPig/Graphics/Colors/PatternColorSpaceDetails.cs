using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000171 RID: 369
	public sealed class PatternColorSpaceDetails : ColorSpaceDetails
	{
		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x00027CF3 File Offset: 0x00025EF3
		public IReadOnlyDictionary<NameToken, PatternColor> Patterns { get; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x00027CFB File Offset: 0x00025EFB
		public override int NumberOfColorComponents
		{
			get
			{
				throw new InvalidOperationException("PatternColorSpaceDetails");
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x00027D07 File Offset: 0x00025F07
		internal override int BaseNumberOfColorComponents
		{
			get
			{
				return this.UnderlyingColourSpace.NumberOfColorComponents;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x00027D14 File Offset: 0x00025F14
		[CanBeNull]
		public ColorSpaceDetails UnderlyingColourSpace { get; }

		// Token: 0x06000894 RID: 2196 RVA: 0x00027D1C File Offset: 0x00025F1C
		public PatternColorSpaceDetails(IReadOnlyDictionary<NameToken, PatternColor> patterns, ColorSpaceDetails underlyingColourSpace) : base(ColorSpace.Pattern)
		{
			if (patterns == null)
			{
				throw new ArgumentNullException("patterns");
			}
			this.Patterns = patterns;
			this.UnderlyingColourSpace = underlyingColourSpace;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x00027D42 File Offset: 0x00025F42
		public PatternColor GetColor(NameToken name)
		{
			return this.Patterns[name];
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00027D50 File Offset: 0x00025F50
		internal override double[] Process(params double[] values)
		{
			throw new InvalidOperationException("PatternColorSpaceDetails");
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00027D5C File Offset: 0x00025F5C
		public override IColor GetColor(params double[] values)
		{
			throw new InvalidOperationException("PatternColorSpaceDetails");
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00027D68 File Offset: 0x00025F68
		public override IColor GetInitializeColor()
		{
			return null;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00027D6B File Offset: 0x00025F6B
		internal override IReadOnlyList<byte> Transform(IReadOnlyList<byte> decoded)
		{
			throw new InvalidOperationException("PatternColorSpaceDetails");
		}
	}
}
