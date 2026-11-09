using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Skia;
using QuestPDF.Skia.Text;

namespace QuestPDF.Infrastructure
{
	// Token: 0x0200004B RID: 75
	[NullableContext(1)]
	internal interface IDrawingCanvas
	{
		// Token: 0x060001B9 RID: 441
		DocumentPageSnapshot GetSnapshot();

		// Token: 0x060001BA RID: 442
		void DrawSnapshot(DocumentPageSnapshot snapshot);

		// Token: 0x060001BB RID: 443
		void Save();

		// Token: 0x060001BC RID: 444
		void Restore();

		// Token: 0x060001BD RID: 445
		void SetZIndex(int index);

		// Token: 0x060001BE RID: 446
		int GetZIndex();

		// Token: 0x060001BF RID: 447
		SkCanvasMatrix GetCurrentMatrix();

		// Token: 0x060001C0 RID: 448
		void SetMatrix(SkCanvasMatrix matrix);

		// Token: 0x060001C1 RID: 449
		void Translate(Position vector);

		// Token: 0x060001C2 RID: 450
		void Scale(float scaleX, float scaleY);

		// Token: 0x060001C3 RID: 451
		void Rotate(float angle);

		// Token: 0x060001C4 RID: 452
		void DrawLine(Position start, Position end, SkPaint paint);

		// Token: 0x060001C5 RID: 453
		void DrawRectangle(Position vector, Size size, SkPaint paint);

		// Token: 0x060001C6 RID: 454
		void DrawComplexBorder(SkRoundedRect innerRect, SkRoundedRect outerRect, SkPaint paint);

		// Token: 0x060001C7 RID: 455
		void DrawShadow(SkRoundedRect shadowRect, SkBoxShadow shadow);

		// Token: 0x060001C8 RID: 456
		void DrawParagraph(SkParagraph paragraph, int lineFrom, int lineTo);

		// Token: 0x060001C9 RID: 457
		void DrawImage(SkImage image, Size size);

		// Token: 0x060001CA RID: 458
		void DrawPicture(SkPicture picture);

		// Token: 0x060001CB RID: 459
		void DrawSvgPath(string path, Color color);

		// Token: 0x060001CC RID: 460
		void DrawSvg(SkSvgImage svgImage, Size size);

		// Token: 0x060001CD RID: 461
		void DrawOverflowArea(SkRect area);

		// Token: 0x060001CE RID: 462
		void ClipOverflowArea(SkRect availableSpace, SkRect requiredSpace);

		// Token: 0x060001CF RID: 463
		void ClipRectangle(SkRect clipArea);

		// Token: 0x060001D0 RID: 464
		void ClipRoundedRectangle(SkRoundedRect clipArea);

		// Token: 0x060001D1 RID: 465
		void DrawHyperlink(Size size, string url, [Nullable(2)] string description);

		// Token: 0x060001D2 RID: 466
		void DrawSectionLink(Size size, string sectionName, [Nullable(2)] string description);

		// Token: 0x060001D3 RID: 467
		void DrawSection(string sectionName);

		// Token: 0x060001D4 RID: 468
		void SetSemanticNodeId(int nodeId);
	}
}
