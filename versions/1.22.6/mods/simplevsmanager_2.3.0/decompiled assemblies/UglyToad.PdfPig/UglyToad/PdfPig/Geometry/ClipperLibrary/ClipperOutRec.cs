using System;

namespace UglyToad.PdfPig.Geometry.ClipperLibrary
{
	// Token: 0x020001A0 RID: 416
	internal class ClipperOutRec
	{
		// Token: 0x040004DB RID: 1243
		public int Idx;

		// Token: 0x040004DC RID: 1244
		public bool IsHole;

		// Token: 0x040004DD RID: 1245
		public bool IsOpen;

		// Token: 0x040004DE RID: 1246
		public ClipperOutRec FirstLeft;

		// Token: 0x040004DF RID: 1247
		public ClipperOutPt Pts;

		// Token: 0x040004E0 RID: 1248
		public ClipperOutPt BottomPt;

		// Token: 0x040004E1 RID: 1249
		public ClipperPolyNode PolyNode;
	}
}
