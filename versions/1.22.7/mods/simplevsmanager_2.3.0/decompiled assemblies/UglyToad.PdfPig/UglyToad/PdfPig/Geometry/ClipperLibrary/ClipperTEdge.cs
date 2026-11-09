using System;

namespace UglyToad.PdfPig.Geometry.ClipperLibrary
{
	// Token: 0x020001A6 RID: 422
	internal class ClipperTEdge
	{
		// Token: 0x040004F0 RID: 1264
		internal ClipperIntPoint Bot;

		// Token: 0x040004F1 RID: 1265
		internal ClipperIntPoint Curr;

		// Token: 0x040004F2 RID: 1266
		internal ClipperIntPoint Top;

		// Token: 0x040004F3 RID: 1267
		internal ClipperIntPoint Delta;

		// Token: 0x040004F4 RID: 1268
		internal double Dx;

		// Token: 0x040004F5 RID: 1269
		internal ClipperPolyType PolyTyp;

		// Token: 0x040004F6 RID: 1270
		internal ClipperEdgeSide Side;

		// Token: 0x040004F7 RID: 1271
		internal int WindDelta;

		// Token: 0x040004F8 RID: 1272
		internal int WindCnt;

		// Token: 0x040004F9 RID: 1273
		internal int WindCnt2;

		// Token: 0x040004FA RID: 1274
		internal int OutIdx;

		// Token: 0x040004FB RID: 1275
		internal ClipperTEdge Next;

		// Token: 0x040004FC RID: 1276
		internal ClipperTEdge Prev;

		// Token: 0x040004FD RID: 1277
		internal ClipperTEdge NextInLML;

		// Token: 0x040004FE RID: 1278
		internal ClipperTEdge NextInAEL;

		// Token: 0x040004FF RID: 1279
		internal ClipperTEdge PrevInAEL;

		// Token: 0x04000500 RID: 1280
		internal ClipperTEdge NextInSEL;

		// Token: 0x04000501 RID: 1281
		internal ClipperTEdge PrevInSEL;
	}
}
