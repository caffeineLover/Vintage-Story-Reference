using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics
{
	// Token: 0x0200010A RID: 266
	public interface IOperationContext
	{
		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600058E RID: 1422
		// (set) Token: 0x0600058F RID: 1423
		PdfPoint CurrentPosition { get; set; }

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000590 RID: 1424
		TextMatrices TextMatrices { get; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000591 RID: 1425
		int StackSize { get; }

		// Token: 0x06000592 RID: 1426
		CurrentGraphicsState GetCurrentState();

		// Token: 0x06000593 RID: 1427
		void PopState();

		// Token: 0x06000594 RID: 1428
		void PushState();

		// Token: 0x06000595 RID: 1429
		void ShowText(IInputBytes bytes);

		// Token: 0x06000596 RID: 1430
		void ShowPositionedText(IReadOnlyList<IToken> tokens);

		// Token: 0x06000597 RID: 1431
		void ApplyXObject(NameToken xObjectName);

		// Token: 0x06000598 RID: 1432
		void BeginSubpath();

		// Token: 0x06000599 RID: 1433
		PdfPoint? CloseSubpath();

		// Token: 0x0600059A RID: 1434
		void StrokePath(bool close);

		// Token: 0x0600059B RID: 1435
		void FillPath(FillingRule fillingRule, bool close);

		// Token: 0x0600059C RID: 1436
		void FillStrokePath(FillingRule fillingRule, bool close);

		// Token: 0x0600059D RID: 1437
		void MoveTo(double x, double y);

		// Token: 0x0600059E RID: 1438
		void BezierCurveTo(double x1, double y1, double x2, double y2, double x3, double y3);

		// Token: 0x0600059F RID: 1439
		void BezierCurveTo(double x2, double y2, double x3, double y3);

		// Token: 0x060005A0 RID: 1440
		void LineTo(double x, double y);

		// Token: 0x060005A1 RID: 1441
		void Rectangle(double x, double y, double width, double height);

		// Token: 0x060005A2 RID: 1442
		void EndPath();

		// Token: 0x060005A3 RID: 1443
		void ClosePath();

		// Token: 0x060005A4 RID: 1444
		void BeginMarkedContent(NameToken name, NameToken propertyDictionaryName, DictionaryToken properties);

		// Token: 0x060005A5 RID: 1445
		void EndMarkedContent();

		// Token: 0x060005A6 RID: 1446
		void SetNamedGraphicsState(NameToken stateName);

		// Token: 0x060005A7 RID: 1447
		void BeginInlineImage();

		// Token: 0x060005A8 RID: 1448
		void SetInlineImageProperties(IReadOnlyDictionary<NameToken, IToken> properties);

		// Token: 0x060005A9 RID: 1449
		void EndInlineImage(IReadOnlyList<byte> bytes);

		// Token: 0x060005AA RID: 1450
		void ModifyClippingIntersect(FillingRule clippingRule);

		// Token: 0x060005AB RID: 1451
		void SetFlatnessTolerance(decimal tolerance);

		// Token: 0x060005AC RID: 1452
		void SetLineCap(LineCapStyle cap);

		// Token: 0x060005AD RID: 1453
		void SetLineDashPattern(LineDashPattern pattern);

		// Token: 0x060005AE RID: 1454
		void SetLineJoin(LineJoinStyle join);

		// Token: 0x060005AF RID: 1455
		void SetLineWidth(decimal width);

		// Token: 0x060005B0 RID: 1456
		void SetMiterLimit(decimal limit);

		// Token: 0x060005B1 RID: 1457
		void MoveToNextLineWithOffset();

		// Token: 0x060005B2 RID: 1458
		void SetFontAndSize(NameToken font, double size);

		// Token: 0x060005B3 RID: 1459
		void SetHorizontalScaling(double scale);

		// Token: 0x060005B4 RID: 1460
		void SetTextLeading(double leading);

		// Token: 0x060005B5 RID: 1461
		void SetTextRenderingMode(TextRenderingMode mode);

		// Token: 0x060005B6 RID: 1462
		void SetTextRise(double rise);

		// Token: 0x060005B7 RID: 1463
		void SetWordSpacing(double spacing);

		// Token: 0x060005B8 RID: 1464
		void ModifyCurrentTransformationMatrix(double[] value);

		// Token: 0x060005B9 RID: 1465
		void SetCharacterSpacing(double spacing);

		// Token: 0x060005BA RID: 1466
		void PaintShading(NameToken shading);
	}
}
