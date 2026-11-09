using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.AdobeFontMetrics
{
	// Token: 0x020000AC RID: 172
	public class AdobeFontMetrics
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x0001DAE0 File Offset: 0x0001BCE0
		public decimal AfmVersion { get; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x0001DAE8 File Offset: 0x0001BCE8
		public IReadOnlyList<string> Comments { get; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0001DAF0 File Offset: 0x0001BCF0
		public AdobeFontMetricsWritingDirections MetricSets { get; }

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x0001DAF8 File Offset: 0x0001BCF8
		public string FontName { get; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x0001DB00 File Offset: 0x0001BD00
		public string FullName { get; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x0001DB08 File Offset: 0x0001BD08
		public string FamilyName { get; }

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x0001DB10 File Offset: 0x0001BD10
		public string Weight { get; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0001DB18 File Offset: 0x0001BD18
		public PdfRectangle BoundingBox { get; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x0001DB20 File Offset: 0x0001BD20
		public string Version { get; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x0001DB28 File Offset: 0x0001BD28
		public string Notice { get; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0001DB30 File Offset: 0x0001BD30
		public string EncodingScheme { get; }

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x0001DB38 File Offset: 0x0001BD38
		public int MappingScheme { get; }

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x0001DB40 File Offset: 0x0001BD40
		public int EscapeCharacter { get; }

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x0001DB48 File Offset: 0x0001BD48
		public string CharacterSet { get; }

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x0001DB50 File Offset: 0x0001BD50
		public int Characters { get; }

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x0001DB58 File Offset: 0x0001BD58
		public bool IsBaseFont { get; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x0001DB60 File Offset: 0x0001BD60
		public AdobeFontMetricsVector VVector { get; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x0001DB68 File Offset: 0x0001BD68
		public bool IsFixedV { get; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x0001DB70 File Offset: 0x0001BD70
		public decimal CapHeight { get; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x0001DB78 File Offset: 0x0001BD78
		public decimal XHeight { get; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000429 RID: 1065 RVA: 0x0001DB80 File Offset: 0x0001BD80
		public decimal Ascender { get; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x0001DB88 File Offset: 0x0001BD88
		public decimal Descender { get; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x0001DB90 File Offset: 0x0001BD90
		public decimal UnderlinePosition { get; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x0001DB98 File Offset: 0x0001BD98
		public decimal UnderlineThickness { get; }

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x0001DBA0 File Offset: 0x0001BDA0
		public decimal ItalicAngle { get; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x0001DBA8 File Offset: 0x0001BDA8
		public AdobeFontMetricsCharacterSize CharacterWidth { get; }

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x0001DBB0 File Offset: 0x0001BDB0
		public decimal HorizontalStemWidth { get; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x0001DBB8 File Offset: 0x0001BDB8
		public decimal VerticalStemWidth { get; }

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x0001DBC0 File Offset: 0x0001BDC0
		public IReadOnlyDictionary<string, AdobeFontMetricsIndividualCharacterMetric> CharacterMetrics { get; }

		// Token: 0x06000432 RID: 1074 RVA: 0x0001DBC8 File Offset: 0x0001BDC8
		public AdobeFontMetrics(decimal afmVersion, IReadOnlyList<string> comments, int metricSets, string fontName, string fullName, string familyName, string weight, PdfRectangle boundingBox, string version, string notice, string encodingScheme, int mappingScheme, int escapeCharacter, string characterSet, int characters, bool isBaseFont, AdobeFontMetricsVector vVector, bool isFixedV, decimal capHeight, decimal xHeight, decimal ascender, decimal descender, decimal underlinePosition, decimal underlineThickness, decimal italicAngle, AdobeFontMetricsCharacterSize characterWidth, decimal horizontalStemWidth, decimal verticalStemWidth, IReadOnlyDictionary<string, AdobeFontMetricsIndividualCharacterMetric> characterMetrics)
		{
			this.AfmVersion = afmVersion;
			this.Comments = comments;
			this.MetricSets = (byte)metricSets;
			this.FontName = fontName;
			this.FullName = fullName;
			this.FamilyName = familyName;
			this.Weight = weight;
			this.BoundingBox = boundingBox;
			this.Version = version;
			this.Notice = notice;
			this.EncodingScheme = encodingScheme;
			this.MappingScheme = mappingScheme;
			this.EscapeCharacter = escapeCharacter;
			this.CharacterSet = characterSet;
			this.Characters = characters;
			this.IsBaseFont = isBaseFont;
			this.VVector = vVector;
			this.IsFixedV = isFixedV;
			this.CapHeight = capHeight;
			this.XHeight = xHeight;
			this.Ascender = ascender;
			this.Descender = descender;
			this.UnderlinePosition = underlinePosition;
			this.UnderlineThickness = underlineThickness;
			this.ItalicAngle = italicAngle;
			this.CharacterWidth = characterWidth;
			this.HorizontalStemWidth = horizontalStemWidth;
			this.VerticalStemWidth = verticalStemWidth;
			this.CharacterMetrics = characterMetrics;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0001DCC4 File Offset: 0x0001BEC4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(27, 2);
			defaultInterpolatedStringHandler.AppendLiteral("AFM Font ");
			defaultInterpolatedStringHandler.AppendFormatted(this.FontName ?? this.FullName);
			defaultInterpolatedStringHandler.AppendLiteral(" with ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Characters);
			defaultInterpolatedStringHandler.AppendLiteral(" characters.");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
