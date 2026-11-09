using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.DrawingCanvases;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Drawing.DocumentCanvases
{
	// Token: 0x02000128 RID: 296
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class PdfDocumentCanvas : IDocumentCanvas, IDisposable
	{
		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000A6E RID: 2670 RVA: 0x0001ADE6 File Offset: 0x00018FE6
		private SkWriteStream WriteStream { get; }

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x0001ADEE File Offset: 0x00018FEE
		private DocumentMetadata DocumentMetadata { get; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x0001ADF6 File Offset: 0x00018FF6
		private DocumentSettings DocumentSettings { get; }

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x0001ADFE File Offset: 0x00018FFE
		// (set) Token: 0x06000A72 RID: 2674 RVA: 0x0001AE06 File Offset: 0x00019006
		[Nullable(2)]
		private SkPdfTag SemanticTag { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x0001AE0F File Offset: 0x0001900F
		// (set) Token: 0x06000A74 RID: 2676 RVA: 0x0001AE17 File Offset: 0x00019017
		[Nullable(2)]
		private SkDocument Document { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x0001AE20 File Offset: 0x00019020
		// (set) Token: 0x06000A76 RID: 2678 RVA: 0x0001AE28 File Offset: 0x00019028
		[Nullable(2)]
		private SkCanvas CurrentPageCanvas { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x0001AE31 File Offset: 0x00019031
		private ProxyDrawingCanvas DrawingCanvas { get; } = new ProxyDrawingCanvas();

		// Token: 0x06000A78 RID: 2680 RVA: 0x0001AE39 File Offset: 0x00019039
		public PdfDocumentCanvas(SkWriteStream stream, DocumentMetadata documentMetadata, DocumentSettings documentSettings)
		{
			this.WriteStream = stream;
			this.DocumentMetadata = documentMetadata;
			this.DocumentSettings = documentSettings;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0001AE64 File Offset: 0x00019064
		private SkDocument CreatePdf()
		{
			SkDocument result;
			using (SkText skText = new SkText(this.DocumentMetadata.Title))
			{
				using (SkText skText2 = new SkText(this.DocumentMetadata.Author))
				{
					using (SkText skText3 = new SkText(this.DocumentMetadata.Subject))
					{
						using (SkText skText4 = new SkText(this.DocumentMetadata.Keywords))
						{
							using (SkText skText5 = new SkText(this.DocumentMetadata.Creator))
							{
								using (SkText skText6 = new SkText(this.DocumentMetadata.Producer))
								{
									using (SkText skText7 = new SkText(this.DocumentMetadata.Language))
									{
										SkPdfDocumentMetadata skPdfDocumentMetadata = default(SkPdfDocumentMetadata);
										skPdfDocumentMetadata.Title = skText;
										skPdfDocumentMetadata.Author = skText2;
										skPdfDocumentMetadata.Subject = skText3;
										skPdfDocumentMetadata.Keywords = skText4;
										skPdfDocumentMetadata.Creator = skText5;
										skPdfDocumentMetadata.Producer = skText6;
										skPdfDocumentMetadata.Language = skText7;
										skPdfDocumentMetadata.CreationDate = new SkDateTime(this.DocumentMetadata.CreationDate);
										skPdfDocumentMetadata.ModificationDate = new SkDateTime(this.DocumentMetadata.ModifiedDate);
										skPdfDocumentMetadata.PDFA_Conformance = PdfDocumentCanvas.GetPDFAConformanceLevel(this.DocumentSettings.PDFA_Conformance);
										skPdfDocumentMetadata.PDFUA_Conformance = PdfDocumentCanvas.GetPDFUAConformanceLevel(this.DocumentSettings.PDFUA_Conformance);
										skPdfDocumentMetadata.RasterDPI = (float)this.DocumentSettings.ImageRasterDpi;
										skPdfDocumentMetadata.CompressDocument = this.DocumentSettings.CompressDocument;
										SkPdfTag semanticTag = this.SemanticTag;
										skPdfDocumentMetadata.SemanticNodeRoot = ((semanticTag != null) ? semanticTag.Instance : IntPtr.Zero);
										SkPdfDocumentMetadata metadata = skPdfDocumentMetadata;
										try
										{
											result = SkPdfDocument.Create(this.WriteStream, metadata);
										}
										catch (TypeInitializationException inner)
										{
											throw new InitializationException("PDF", inner);
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0001B128 File Offset: 0x00019328
		private static QuestPDF.Skia.PDFA_Conformance GetPDFAConformanceLevel(QuestPDF.Infrastructure.PDFA_Conformance conformanceLevel)
		{
			switch (conformanceLevel)
			{
			case QuestPDF.Infrastructure.PDFA_Conformance.None:
				return QuestPDF.Skia.PDFA_Conformance.None;
			case QuestPDF.Infrastructure.PDFA_Conformance.PDFA_2A:
				return QuestPDF.Skia.PDFA_Conformance.PDFA_2A;
			case QuestPDF.Infrastructure.PDFA_Conformance.PDFA_2B:
				return QuestPDF.Skia.PDFA_Conformance.PDFA_2B;
			case QuestPDF.Infrastructure.PDFA_Conformance.PDFA_2U:
				return QuestPDF.Skia.PDFA_Conformance.PDFA_2U;
			case QuestPDF.Infrastructure.PDFA_Conformance.PDFA_3A:
				return QuestPDF.Skia.PDFA_Conformance.PDFA_3A;
			case QuestPDF.Infrastructure.PDFA_Conformance.PDFA_3B:
				return QuestPDF.Skia.PDFA_Conformance.PDFA_3B;
			case QuestPDF.Infrastructure.PDFA_Conformance.PDFA_3U:
				return QuestPDF.Skia.PDFA_Conformance.PDFA_3U;
			}
			throw new ArgumentOutOfRangeException("conformanceLevel", conformanceLevel, "Unsupported PDF/A conformance level");
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x0001B194 File Offset: 0x00019394
		private static QuestPDF.Skia.PDFUA_Conformance GetPDFUAConformanceLevel(QuestPDF.Infrastructure.PDFUA_Conformance conformanceLevel)
		{
			QuestPDF.Skia.PDFUA_Conformance result;
			if (conformanceLevel != QuestPDF.Infrastructure.PDFUA_Conformance.None)
			{
				if (conformanceLevel != QuestPDF.Infrastructure.PDFUA_Conformance.PDFUA_1)
				{
					throw new ArgumentOutOfRangeException("conformanceLevel", conformanceLevel, "Unsupported PDF/UA conformance level");
				}
				result = QuestPDF.Skia.PDFUA_Conformance.PDFUA_1;
			}
			else
			{
				result = QuestPDF.Skia.PDFUA_Conformance.None;
			}
			return result;
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0001B1CC File Offset: 0x000193CC
		~PdfDocumentCanvas()
		{
			this.WarnThatFinalizerIsReached<PdfDocumentCanvas>();
			this.Dispose();
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0001B200 File Offset: 0x00019400
		public void Dispose()
		{
			SkDocument document = this.Document;
			if (document != null)
			{
				document.Dispose();
			}
			SkCanvas currentPageCanvas = this.CurrentPageCanvas;
			if (currentPageCanvas != null)
			{
				currentPageCanvas.Dispose();
			}
			ProxyDrawingCanvas drawingCanvas = this.DrawingCanvas;
			if (drawingCanvas != null)
			{
				drawingCanvas.Dispose();
			}
			SkPdfTag semanticTag = this.SemanticTag;
			if (semanticTag != null)
			{
				semanticTag.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0001B257 File Offset: 0x00019457
		[NullableContext(2)]
		public void SetSemanticTree(SemanticTreeNode semanticTree)
		{
			if (semanticTree == null)
			{
				SkPdfTag semanticTag = this.SemanticTag;
				if (semanticTag != null)
				{
					semanticTag.Dispose();
				}
				this.SemanticTag = null;
				return;
			}
			this.SemanticTag = PdfDocumentCanvas.<SetSemanticTree>g__Convert|30_0(semanticTree);
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0001B284 File Offset: 0x00019484
		public void BeginDocument()
		{
			if (this.Document == null)
			{
				this.Document = this.CreatePdf();
			}
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x0001B2A7 File Offset: 0x000194A7
		public void EndDocument()
		{
			SkDocument document = this.Document;
			if (document != null)
			{
				document.Close();
			}
			SkDocument document2 = this.Document;
			if (document2 == null)
			{
				return;
			}
			document2.Dispose();
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x0001B2CC File Offset: 0x000194CC
		public void BeginPage(Size size)
		{
			SkDocument document = this.Document;
			this.CurrentPageCanvas = ((document != null) ? document.BeginPage(size.Width, size.Height) : null);
			this.DrawingCanvas.Target = new SkiaDrawingCanvas(size.Width, size.Height);
			this.DrawingCanvas.SetZIndex(0);
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x0001B328 File Offset: 0x00019528
		public void EndPage()
		{
			using (DocumentPageSnapshot snapshot = this.DrawingCanvas.GetSnapshot())
			{
				snapshot.DrawOnSkCanvas(this.CurrentPageCanvas);
				this.CurrentPageCanvas.Save();
				this.CurrentPageCanvas.Dispose();
				this.CurrentPageCanvas = null;
				this.Document.EndPage();
			}
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x0001B394 File Offset: 0x00019594
		public IDrawingCanvas GetDrawingCanvas()
		{
			return this.DrawingCanvas;
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x0001B39C File Offset: 0x0001959C
		[CompilerGenerated]
		internal static SkPdfTag <SetSemanticTree>g__Convert|30_0(SemanticTreeNode node)
		{
			SkPdfTag skPdfTag = SkPdfTag.Create(node.NodeId, node.Type, node.Alt, node.Lang);
			IEnumerable<SemanticTreeNode> children = node.Children;
			Func<SemanticTreeNode, SkPdfTag> func;
			if ((func = PdfDocumentCanvas.<>O.<0>__Convert) == null)
			{
				func = (PdfDocumentCanvas.<>O.<0>__Convert = new Func<SemanticTreeNode, SkPdfTag>(PdfDocumentCanvas.<SetSemanticTree>g__Convert|30_0));
			}
			SkPdfTag[] children2 = Enumerable.ToArray<SkPdfTag>(Enumerable.Select<SemanticTreeNode, SkPdfTag>(children, func));
			skPdfTag.SetChildren(children2);
			foreach (SemanticTreeNode.Attribute attribute in node.Attributes)
			{
				skPdfTag.AddAttribute(attribute.Owner, attribute.Name, attribute.Value);
			}
			return skPdfTag;
		}

		// Token: 0x02000203 RID: 515
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000788 RID: 1928
			[Nullable(0)]
			public static Func<SemanticTreeNode, SkPdfTag> <0>__Convert;
		}
	}
}
