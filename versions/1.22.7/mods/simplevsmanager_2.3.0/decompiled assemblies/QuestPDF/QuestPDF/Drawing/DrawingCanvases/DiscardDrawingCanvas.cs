using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;
using QuestPDF.Skia.Text;

namespace QuestPDF.Drawing.DrawingCanvases
{
	// Token: 0x02000120 RID: 288
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class DiscardDrawingCanvas : IDrawingCanvas
	{
		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060009CD RID: 2509 RVA: 0x00019F59 File Offset: 0x00018159
		private Stack<Matrix4x4> MatrixStack { get; } = new Stack<Matrix4x4>();

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060009CE RID: 2510 RVA: 0x00019F61 File Offset: 0x00018161
		// (set) Token: 0x060009CF RID: 2511 RVA: 0x00019F69 File Offset: 0x00018169
		private Matrix4x4 CurrentMatrix { get; set; } = Matrix4x4.Identity;

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060009D0 RID: 2512 RVA: 0x00019F72 File Offset: 0x00018172
		// (set) Token: 0x060009D1 RID: 2513 RVA: 0x00019F7A File Offset: 0x0001817A
		private int CurrentZIndex { get; set; }

		// Token: 0x060009D2 RID: 2514 RVA: 0x00019F83 File Offset: 0x00018183
		public DocumentPageSnapshot GetSnapshot()
		{
			return new DocumentPageSnapshot();
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x00019F8A File Offset: 0x0001818A
		public void DrawSnapshot(DocumentPageSnapshot snapshot)
		{
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x00019F8C File Offset: 0x0001818C
		public void Save()
		{
			this.MatrixStack.Push(this.CurrentMatrix);
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x00019F9F File Offset: 0x0001819F
		public void Restore()
		{
			this.CurrentMatrix = this.MatrixStack.Pop();
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x00019FB2 File Offset: 0x000181B2
		public void SetZIndex(int index)
		{
			this.CurrentZIndex = index;
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x00019FBB File Offset: 0x000181BB
		public int GetZIndex()
		{
			return this.CurrentZIndex;
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x00019FC3 File Offset: 0x000181C3
		public SkCanvasMatrix GetCurrentMatrix()
		{
			return SkCanvasMatrix.FromMatrix4x4(this.CurrentMatrix);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x00019FD0 File Offset: 0x000181D0
		public void SetMatrix(SkCanvasMatrix matrix)
		{
			this.CurrentMatrix = matrix.ToMatrix4x4();
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00019FDF File Offset: 0x000181DF
		public void Translate(Position vector)
		{
			this.CurrentMatrix = Matrix4x4.CreateTranslation(vector.X, vector.Y, 0f) * this.CurrentMatrix;
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0001A008 File Offset: 0x00018208
		public void Scale(float scaleX, float scaleY)
		{
			this.CurrentMatrix = Matrix4x4.CreateScale(scaleX, scaleY, 1f) * this.CurrentMatrix;
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0001A027 File Offset: 0x00018227
		public void Rotate(float angle)
		{
			this.CurrentMatrix = Matrix4x4.CreateRotationZ(3.1415927f * angle / 180f) * this.CurrentMatrix;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0001A04C File Offset: 0x0001824C
		public void DrawLine(Position start, Position end, SkPaint paint)
		{
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0001A04E File Offset: 0x0001824E
		public void DrawRectangle(Position vector, Size size, SkPaint paint)
		{
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0001A050 File Offset: 0x00018250
		public void DrawComplexBorder(SkRoundedRect innerRect, SkRoundedRect outerRect, SkPaint paint)
		{
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0001A052 File Offset: 0x00018252
		public void DrawShadow(SkRoundedRect shadowRect, SkBoxShadow shadow)
		{
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0001A054 File Offset: 0x00018254
		public void DrawParagraph(SkParagraph paragraph, int lineFrom, int lineTo)
		{
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0001A056 File Offset: 0x00018256
		public void DrawImage(SkImage image, Size size)
		{
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x0001A058 File Offset: 0x00018258
		public void DrawPicture(SkPicture picture)
		{
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0001A05A File Offset: 0x0001825A
		public void DrawSvgPath(string path, Color color)
		{
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x0001A05C File Offset: 0x0001825C
		public void DrawSvg(SkSvgImage svgImage, Size size)
		{
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x0001A05E File Offset: 0x0001825E
		public void DrawOverflowArea(SkRect area)
		{
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0001A060 File Offset: 0x00018260
		public void ClipOverflowArea(SkRect availableSpace, SkRect requiredSpace)
		{
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x0001A062 File Offset: 0x00018262
		public void ClipRectangle(SkRect clipArea)
		{
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0001A064 File Offset: 0x00018264
		public void ClipRoundedRectangle(SkRoundedRect clipArea)
		{
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0001A066 File Offset: 0x00018266
		public void DrawHyperlink(Size size, string url, [Nullable(2)] string description)
		{
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0001A068 File Offset: 0x00018268
		public void DrawSectionLink(Size size, string sectionName, [Nullable(2)] string description)
		{
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0001A06A File Offset: 0x0001826A
		public void DrawSection(string sectionName)
		{
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0001A06C File Offset: 0x0001826C
		public void SetSemanticNodeId(int nodeId)
		{
		}
	}
}
