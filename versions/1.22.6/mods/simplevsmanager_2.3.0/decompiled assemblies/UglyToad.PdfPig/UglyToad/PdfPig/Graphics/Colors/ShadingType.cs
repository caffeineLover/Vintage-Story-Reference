using System;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000188 RID: 392
	public enum ShadingType : byte
	{
		// Token: 0x04000475 RID: 1141
		FunctionBased = 1,
		// Token: 0x04000476 RID: 1142
		Axial,
		// Token: 0x04000477 RID: 1143
		Radial,
		// Token: 0x04000478 RID: 1144
		FreeFormGouraud,
		// Token: 0x04000479 RID: 1145
		LatticeFormGouraud,
		// Token: 0x0400047A RID: 1146
		CoonsPatch,
		// Token: 0x0400047B RID: 1147
		TensorProductPatch
	}
}
