using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Helpers
{
	// Token: 0x02000072 RID: 114
	[NullableContext(1)]
	[Nullable(0)]
	public static class FontFeatures
	{
		// Token: 0x060002C5 RID: 709 RVA: 0x00007328 File Offset: 0x00005528
		public static string CharacterVariant(int value)
		{
			if (value < 1 || value > 99)
			{
				throw new ArgumentOutOfRangeException("value", "Character Variant value must be between 1 and 99.");
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 1);
			defaultInterpolatedStringHandler.AppendLiteral("cv");
			defaultInterpolatedStringHandler.AppendFormatted<int>(value, "00");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00007378 File Offset: 0x00005578
		public static string StylisticSet(int value)
		{
			if (value < 1 || value > 20)
			{
				throw new ArgumentOutOfRangeException("value", "Character Variant value must be between 1 and 20.");
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 1);
			defaultInterpolatedStringHandler.AppendLiteral("ss");
			defaultInterpolatedStringHandler.AppendFormatted<int>(value, "00");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400019E RID: 414
		public const string AccessAllAlternates = "aalt";

		// Token: 0x0400019F RID: 415
		public const string AboveBaseForms = "abvf";

		// Token: 0x040001A0 RID: 416
		public const string AboveBaseMarkPositioning = "abvm";

		// Token: 0x040001A1 RID: 417
		public const string AboveBaseSubstitutions = "abvs";

		// Token: 0x040001A2 RID: 418
		public const string AlternativeFractions = "afrc";

		// Token: 0x040001A3 RID: 419
		public const string Akhand = "akhn";

		// Token: 0x040001A4 RID: 420
		public const string KerningForAlternateProportionalWidths = "apkn";

		// Token: 0x040001A5 RID: 421
		public const string BelowBaseForms = "blwf";

		// Token: 0x040001A6 RID: 422
		public const string BelowBaseMarkPositioning = "blwm";

		// Token: 0x040001A7 RID: 423
		public const string BelowBaseSubstitutions = "blws";

		// Token: 0x040001A8 RID: 424
		public const string ContextualAlternates = "calt";

		// Token: 0x040001A9 RID: 425
		public const string CaseSensitiveForms = "case";

		// Token: 0x040001AA RID: 426
		public const string GlyphCompositionDecomposition = "ccmp";

		// Token: 0x040001AB RID: 427
		public const string ConjunctFormAfterRo = "cfar";

		// Token: 0x040001AC RID: 428
		public const string ContextualHalfWidthSpacing = "chws";

		// Token: 0x040001AD RID: 429
		public const string ConjunctForms = "cjct";

		// Token: 0x040001AE RID: 430
		public const string ContextualLigatures = "clig";

		// Token: 0x040001AF RID: 431
		public const string CenteredCjkPunctuation = "cpct";

		// Token: 0x040001B0 RID: 432
		public const string CapitalSpacing = "cpsp";

		// Token: 0x040001B1 RID: 433
		public const string ContextualSwash = "cswh";

		// Token: 0x040001B2 RID: 434
		public const string CursivePositioning = "curs";

		// Token: 0x040001B3 RID: 435
		public const string PetiteCapitalsFromCapitals = "c2pc";

		// Token: 0x040001B4 RID: 436
		public const string SmallCapitalsFromCapitals = "c2sc";

		// Token: 0x040001B5 RID: 437
		public const string Distances = "dist";

		// Token: 0x040001B6 RID: 438
		public const string DiscretionaryLigatures = "dlig";

		// Token: 0x040001B7 RID: 439
		public const string Denominators = "dnom";

		// Token: 0x040001B8 RID: 440
		public const string DotlessForms = "dtls";

		// Token: 0x040001B9 RID: 441
		public const string ExpertForms = "expt";

		// Token: 0x040001BA RID: 442
		public const string FinalGlyphOnLineAlternates = "falt";

		// Token: 0x040001BB RID: 443
		public const string TerminalForms2 = "fin2";

		// Token: 0x040001BC RID: 444
		public const string TerminalForms3 = "fin3";

		// Token: 0x040001BD RID: 445
		public const string TerminalForms = "fina";

		// Token: 0x040001BE RID: 446
		public const string FlattenedAccentForms = "flac";

		// Token: 0x040001BF RID: 447
		public const string Fractions = "frac";

		// Token: 0x040001C0 RID: 448
		public const string FullWidths = "fwid";

		// Token: 0x040001C1 RID: 449
		public const string HalfForms = "half";

		// Token: 0x040001C2 RID: 450
		public const string HalantForms = "haln";

		// Token: 0x040001C3 RID: 451
		public const string AlternateHalfWidths = "halt";

		// Token: 0x040001C4 RID: 452
		public const string HistoricalForms = "hist";

		// Token: 0x040001C5 RID: 453
		public const string HorizontalKanaAlternates = "hkna";

		// Token: 0x040001C6 RID: 454
		public const string HistoricalLigatures = "hlig";

		// Token: 0x040001C7 RID: 455
		public const string Hangul = "hngl";

		// Token: 0x040001C8 RID: 456
		public const string HalfWidths = "hwid";

		// Token: 0x040001C9 RID: 457
		public const string InitialForms = "init";

		// Token: 0x040001CA RID: 458
		public const string IsolatedForms = "isol";

		// Token: 0x040001CB RID: 459
		public const string Italics = "ital";

		// Token: 0x040001CC RID: 460
		public const string JustificationAlternates = "jalt";

		// Token: 0x040001CD RID: 461
		public const string JIS78Forms = "jp78";

		// Token: 0x040001CE RID: 462
		public const string JIS83Forms = "jp83";

		// Token: 0x040001CF RID: 463
		public const string JIS90Forms = "jp90";

		// Token: 0x040001D0 RID: 464
		public const string JIS2004Forms = "jp04";

		// Token: 0x040001D1 RID: 465
		public const string Kerning = "kern";

		// Token: 0x040001D2 RID: 466
		public const string LeftBounds = "lfbd";

		// Token: 0x040001D3 RID: 467
		public const string StandardLigatures = "liga";

		// Token: 0x040001D4 RID: 468
		public const string LeadingJamoForms = "ljmo";

		// Token: 0x040001D5 RID: 469
		public const string LiningFigures = "lnum";

		// Token: 0x040001D6 RID: 470
		public const string LocalizedForms = "locl";

		// Token: 0x040001D7 RID: 471
		public const string LeftToRightAlternates = "ltra";

		// Token: 0x040001D8 RID: 472
		public const string LeftToRightMirroredForms = "ltrm";

		// Token: 0x040001D9 RID: 473
		public const string MarkPositioning = "mark";

		// Token: 0x040001DA RID: 474
		public const string MedialForms2 = "med2";

		// Token: 0x040001DB RID: 475
		public const string MedialForms = "medi";

		// Token: 0x040001DC RID: 476
		public const string MathematicalGreek = "mgrk";

		// Token: 0x040001DD RID: 477
		public const string MarkToMarkPositioning = "mkmk";

		// Token: 0x040001DE RID: 478
		public const string MarkPositioningViaSubstitution = "mset";

		// Token: 0x040001DF RID: 479
		public const string AlternateAnnotationForms = "nalt";

		// Token: 0x040001E0 RID: 480
		public const string NlcKanjiForms = "nlck";

		// Token: 0x040001E1 RID: 481
		public const string NuktaForms = "nukt";

		// Token: 0x040001E2 RID: 482
		public const string Numerators = "numr";

		// Token: 0x040001E3 RID: 483
		public const string OldstyleFigures = "onum";

		// Token: 0x040001E4 RID: 484
		public const string OpticalBounds = "opbd";

		// Token: 0x040001E5 RID: 485
		public const string Ordinals = "ordn";

		// Token: 0x040001E6 RID: 486
		public const string Ornaments = "ornm";

		// Token: 0x040001E7 RID: 487
		public const string ProportionalAlternateWidths = "palt";

		// Token: 0x040001E8 RID: 488
		public const string PetiteCapitals = "pcap";

		// Token: 0x040001E9 RID: 489
		public const string ProportionalKana = "pkna";

		// Token: 0x040001EA RID: 490
		public const string ProportionalFigures = "pnum";

		// Token: 0x040001EB RID: 491
		public const string PreBaseForms = "pref";

		// Token: 0x040001EC RID: 492
		public const string PreBaseSubstitutions = "pres";

		// Token: 0x040001ED RID: 493
		public const string PostBaseForms = "pstf";

		// Token: 0x040001EE RID: 494
		public const string PostBaseSubstitutions = "psts";

		// Token: 0x040001EF RID: 495
		public const string ProportionalWidths = "pwid";

		// Token: 0x040001F0 RID: 496
		public const string QuarterWidths = "qwid";

		// Token: 0x040001F1 RID: 497
		public const string Randomize = "rand";

		// Token: 0x040001F2 RID: 498
		public const string RequiredContextualAlternates = "rclt";

		// Token: 0x040001F3 RID: 499
		public const string RakarForms = "rkrf";

		// Token: 0x040001F4 RID: 500
		public const string RequiredLigatures = "rlig";

		// Token: 0x040001F5 RID: 501
		public const string RephForm = "rphf";

		// Token: 0x040001F6 RID: 502
		public const string RightBounds = "rtbd";

		// Token: 0x040001F7 RID: 503
		public const string RightToLeftAlternates = "rtla";

		// Token: 0x040001F8 RID: 504
		public const string RightToLeftMirroredForms = "rtlm";

		// Token: 0x040001F9 RID: 505
		public const string RubyNotationForms = "ruby";

		// Token: 0x040001FA RID: 506
		public const string RequiredVariationAlternates = "rvrn";

		// Token: 0x040001FB RID: 507
		public const string StylisticAlternates = "salt";

		// Token: 0x040001FC RID: 508
		public const string ScientificInferiors = "sinf";

		// Token: 0x040001FD RID: 509
		public const string OpticalSize = "size";

		// Token: 0x040001FE RID: 510
		public const string SmallCapitals = "smcp";

		// Token: 0x040001FF RID: 511
		public const string SimplifiedForms = "smpl";

		// Token: 0x04000200 RID: 512
		public const string MathScriptStyleAlternates = "ssty";

		// Token: 0x04000201 RID: 513
		public const string StretchingGlyphDecomposition = "stch";

		// Token: 0x04000202 RID: 514
		public const string Subscript = "subs";

		// Token: 0x04000203 RID: 515
		public const string Superscript = "sups";

		// Token: 0x04000204 RID: 516
		public const string Swash = "swsh";

		// Token: 0x04000205 RID: 517
		public const string Titling = "titl";

		// Token: 0x04000206 RID: 518
		public const string TrailingJamoForms = "tjmo";

		// Token: 0x04000207 RID: 519
		public const string TraditionalNameForms = "tnam";

		// Token: 0x04000208 RID: 520
		public const string TabularFigures = "tnum";

		// Token: 0x04000209 RID: 521
		public const string TraditionalForms = "trad";

		// Token: 0x0400020A RID: 522
		public const string ThirdWidths = "twid";

		// Token: 0x0400020B RID: 523
		public const string Unicase = "unic";

		// Token: 0x0400020C RID: 524
		public const string AlternateVerticalMetrics = "valt";

		// Token: 0x0400020D RID: 525
		public const string KerningForAlternateProportionalVerticalMetrics = "vapk";

		// Token: 0x0400020E RID: 526
		public const string VattuVariants = "vatu";

		// Token: 0x0400020F RID: 527
		public const string VerticalContextualHalfWidthSpacing = "vchw";

		// Token: 0x04000210 RID: 528
		public const string VerticalAlternates = "vert";

		// Token: 0x04000211 RID: 529
		public const string AlternateVerticalHalfMetrics = "vhal";

		// Token: 0x04000212 RID: 530
		public const string VowelJamoForms = "vjmo";

		// Token: 0x04000213 RID: 531
		public const string VerticalKanaAlternates = "vkna";

		// Token: 0x04000214 RID: 532
		public const string VerticalKerning = "vkrn";

		// Token: 0x04000215 RID: 533
		public const string ProportionalAlternateVerticalMetrics = "vpal";

		// Token: 0x04000216 RID: 534
		public const string VerticalAlternatesAndRotation = "vrt2";

		// Token: 0x04000217 RID: 535
		public const string VerticalAlternatesForRotation = "vrtr";

		// Token: 0x04000218 RID: 536
		public const string SlashedZero = "zero";

		// Token: 0x04000219 RID: 537
		public const string HojoKanjiForms = "hojo";
	}
}
