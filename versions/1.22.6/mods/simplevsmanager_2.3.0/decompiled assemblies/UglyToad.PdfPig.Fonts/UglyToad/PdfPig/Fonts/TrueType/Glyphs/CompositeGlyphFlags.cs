using System;

namespace UglyToad.PdfPig.Fonts.TrueType.Glyphs
{
	// Token: 0x02000064 RID: 100
	[Flags]
	public enum CompositeGlyphFlags : ushort
	{
		// Token: 0x040002A5 RID: 677
		Args1And2AreWords = 1,
		// Token: 0x040002A6 RID: 678
		ArgsAreXAndYValues = 2,
		// Token: 0x040002A7 RID: 679
		RoundXAndYToGrid = 4,
		// Token: 0x040002A8 RID: 680
		WeHaveAScale = 8,
		// Token: 0x040002A9 RID: 681
		Reserved = 16,
		// Token: 0x040002AA RID: 682
		MoreComponents = 32,
		// Token: 0x040002AB RID: 683
		WeHaveAnXAndYScale = 64,
		// Token: 0x040002AC RID: 684
		WeHaveATwoByTwo = 128,
		// Token: 0x040002AD RID: 685
		WeHaveInstructions = 256,
		// Token: 0x040002AE RID: 686
		UseMyMetrics = 512
	}
}
