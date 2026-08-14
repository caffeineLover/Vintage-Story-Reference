using System;

namespace ImGuiNET
{
	// Token: 0x02000030 RID: 48
	[Flags]
	public enum ImGuiColorEditFlags
	{
		// Token: 0x0400012F RID: 303
		None = 0,
		// Token: 0x04000130 RID: 304
		NoAlpha = 2,
		// Token: 0x04000131 RID: 305
		NoPicker = 4,
		// Token: 0x04000132 RID: 306
		NoOptions = 8,
		// Token: 0x04000133 RID: 307
		NoSmallPreview = 16,
		// Token: 0x04000134 RID: 308
		NoInputs = 32,
		// Token: 0x04000135 RID: 309
		NoTooltip = 64,
		// Token: 0x04000136 RID: 310
		NoLabel = 128,
		// Token: 0x04000137 RID: 311
		NoSidePreview = 256,
		// Token: 0x04000138 RID: 312
		NoDragDrop = 512,
		// Token: 0x04000139 RID: 313
		NoBorder = 1024,
		// Token: 0x0400013A RID: 314
		AlphaBar = 65536,
		// Token: 0x0400013B RID: 315
		AlphaPreview = 131072,
		// Token: 0x0400013C RID: 316
		AlphaPreviewHalf = 262144,
		// Token: 0x0400013D RID: 317
		HDR = 524288,
		// Token: 0x0400013E RID: 318
		DisplayRGB = 1048576,
		// Token: 0x0400013F RID: 319
		DisplayHSV = 2097152,
		// Token: 0x04000140 RID: 320
		DisplayHex = 4194304,
		// Token: 0x04000141 RID: 321
		Uint8 = 8388608,
		// Token: 0x04000142 RID: 322
		Float = 16777216,
		// Token: 0x04000143 RID: 323
		PickerHueBar = 33554432,
		// Token: 0x04000144 RID: 324
		PickerHueWheel = 67108864,
		// Token: 0x04000145 RID: 325
		InputRGB = 134217728,
		// Token: 0x04000146 RID: 326
		InputHSV = 268435456,
		// Token: 0x04000147 RID: 327
		DefaultOptions = 177209344,
		// Token: 0x04000148 RID: 328
		DisplayMask = 7340032,
		// Token: 0x04000149 RID: 329
		DataTypeMask = 25165824,
		// Token: 0x0400014A RID: 330
		PickerMask = 100663296,
		// Token: 0x0400014B RID: 331
		InputMask = 402653184
	}
}
