using System;

namespace UglyToad.PdfPig.Images
{
	// Token: 0x020000EB RID: 235
	internal enum JpegMarker : byte
	{
		// Token: 0x0400023B RID: 571
		StartOfBaselineDctFrame = 192,
		// Token: 0x0400023C RID: 572
		StartOfProgressiveDctFrame = 194,
		// Token: 0x0400023D RID: 573
		DefineHuffmanTable = 196,
		// Token: 0x0400023E RID: 574
		StartOfScan = 218,
		// Token: 0x0400023F RID: 575
		DefineQuantizationTable,
		// Token: 0x04000240 RID: 576
		DefineRestartInterval = 221,
		// Token: 0x04000241 RID: 577
		Restart0 = 208,
		// Token: 0x04000242 RID: 578
		Restart1,
		// Token: 0x04000243 RID: 579
		Restart2,
		// Token: 0x04000244 RID: 580
		Restart3,
		// Token: 0x04000245 RID: 581
		Restart4,
		// Token: 0x04000246 RID: 582
		Restart5,
		// Token: 0x04000247 RID: 583
		Restart6,
		// Token: 0x04000248 RID: 584
		Restart7,
		// Token: 0x04000249 RID: 585
		StartOfImage,
		// Token: 0x0400024A RID: 586
		EndOfImage,
		// Token: 0x0400024B RID: 587
		ApplicationSpecific0 = 224,
		// Token: 0x0400024C RID: 588
		ApplicationSpecific1,
		// Token: 0x0400024D RID: 589
		ApplicationSpecific2,
		// Token: 0x0400024E RID: 590
		ApplicationSpecific3,
		// Token: 0x0400024F RID: 591
		ApplicationSpecific4,
		// Token: 0x04000250 RID: 592
		ApplicationSpecific5,
		// Token: 0x04000251 RID: 593
		ApplicationSpecific6,
		// Token: 0x04000252 RID: 594
		ApplicationSpecific7,
		// Token: 0x04000253 RID: 595
		ApplicationSpecific8,
		// Token: 0x04000254 RID: 596
		ApplicationSpecific9,
		// Token: 0x04000255 RID: 597
		ApplicationSpecific10,
		// Token: 0x04000256 RID: 598
		ApplicationSpecific11,
		// Token: 0x04000257 RID: 599
		ApplicationSpecific12,
		// Token: 0x04000258 RID: 600
		ApplicationSpecific13,
		// Token: 0x04000259 RID: 601
		ApplicationSpecific14,
		// Token: 0x0400025A RID: 602
		ApplicationSpecific15,
		// Token: 0x0400025B RID: 603
		Comment = 254
	}
}
