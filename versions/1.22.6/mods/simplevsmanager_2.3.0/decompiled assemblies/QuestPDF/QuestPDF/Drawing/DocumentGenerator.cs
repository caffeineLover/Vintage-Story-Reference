using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Companion;
using QuestPDF.Drawing.DocumentCanvases;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Drawing.Proxy;
using QuestPDF.Elements;
using QuestPDF.Elements.Text;
using QuestPDF.Elements.Text.Items;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Drawing
{
	// Token: 0x0200010D RID: 269
	[NullableContext(1)]
	[Nullable(0)]
	internal static class DocumentGenerator
	{
		// Token: 0x06000938 RID: 2360 RVA: 0x00017388 File Offset: 0x00015588
		static DocumentGenerator()
		{
			SkNativeDependencyCompatibilityChecker.Test();
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00017390 File Offset: 0x00015590
		internal static void GeneratePdf(SkWriteStream stream, IDocument document)
		{
			DocumentGenerator.ValidateLicense();
			DocumentMetadata metadata = document.GetMetadata();
			DocumentSettings settings = document.GetSettings();
			using (PdfDocumentCanvas pdfDocumentCanvas = new PdfDocumentCanvas(stream, metadata, settings))
			{
				DocumentGenerator.RenderDocument(pdfDocumentCanvas, document, settings);
			}
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x000173E0 File Offset: 0x000155E0
		internal static void GenerateXps(SkWriteStream stream, IDocument document)
		{
			DocumentGenerator.ValidateLicense();
			DocumentSettings settings = document.GetSettings();
			using (XpsDocumentCanvas xpsDocumentCanvas = new XpsDocumentCanvas(stream, settings))
			{
				DocumentGenerator.RenderDocument(xpsDocumentCanvas, document, settings);
			}
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00017428 File Offset: 0x00015628
		internal static ICollection<byte[]> GenerateImages(IDocument document, ImageGenerationSettings imageGenerationSettings)
		{
			DocumentGenerator.ValidateLicense();
			DocumentSettings settings = document.GetSettings();
			settings.ImageRasterDpi = imageGenerationSettings.RasterDpi;
			ICollection<byte[]> images;
			using (ImageDocumentCanvas imageDocumentCanvas = new ImageDocumentCanvas(imageGenerationSettings))
			{
				DocumentGenerator.RenderDocument(imageDocumentCanvas, document, settings);
				images = imageDocumentCanvas.Images;
			}
			return images;
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00017480 File Offset: 0x00015680
		internal static ICollection<string> GenerateSvg(IDocument document)
		{
			DocumentGenerator.ValidateLicense();
			ICollection<string> images;
			using (SvgDocumentCanvas svgDocumentCanvas = new SvgDocumentCanvas())
			{
				DocumentGenerator.RenderDocument(svgDocumentCanvas, document, document.GetSettings());
				images = svgDocumentCanvas.Images;
			}
			return images;
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x000174CC File Offset: 0x000156CC
		internal static void ValidateLicense()
		{
			if (Settings.License != null)
			{
				return;
			}
			throw new Exception("\n\n\n\nWelcome to QuestPDF! \ud83d\udc4b \n\nQuestPDF is an open-source library committed to long-term sustainability and continuous improvement. \n\nTo maintain high-quality development and support while keeping the library free for most users, we use a fair pricing model where only larger organizations help by providing necessary funding for the project. \n\nIf your organization’s annual gross revenue exceeds $1M USD, a Commercial License is required for production use (you can freely evaluate and test QuestPDF in non-production environments at no cost). In that case, please share this information with your team or management. By purchasing a license, you directly contribute to the ongoing development and reliability of QuestPDF. \n\nFor details on the license types and determining which applies to you, please visit: https://www.questpdf.com/license/ \n\nWe trust our users. To continue, no license key is needed. Instead, simply select and configure the appropriate license in your code. For example, if you qualify for the Community license, add: \n\n> QuestPDF.Settings.License = LicenseType.Community; \n\nThank you for supporting QuestPDF! ❤️ By choosing the right license, you help ensure that our project remains transparent, sustainable, and continuously improving for everyone. \ud83d\ude80 \n\n\n\n")
			{
				HelpLink = "https://www.questpdf.com/pricing.html"
			};
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00017500 File Offset: 0x00015700
		internal static CompanionDocumentSnapshot GenerateCompanionContent(IDocument document)
		{
			CompanionDocumentSnapshot content;
			using (CompanionDocumentCanvas companionDocumentCanvas = new CompanionDocumentCanvas())
			{
				DocumentGenerator.RenderDocument(companionDocumentCanvas, document, DocumentSettings.Default);
				content = companionDocumentCanvas.GetContent();
			}
			return content;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00017544 File Offset: 0x00015744
		internal static void RenderDocument(IDocumentCanvas canvas, IDocument document, DocumentSettings settings)
		{
			MergedDocument mergedDocument = document as MergedDocument;
			if (mergedDocument != null)
			{
				DocumentGenerator.RenderMergedDocument(canvas, mergedDocument, settings);
				return;
			}
			SemanticTreeManager semanticTreeManager = DocumentGenerator.CreateSemanticTreeManager(settings);
			bool useOriginalImages = canvas is ImageDocumentCanvas;
			Container container = DocumentGenerator.ConfigureContent(document, settings, semanticTreeManager, useOriginalImages);
			if (canvas is CompanionDocumentCanvas)
			{
				container.VisitChildren(delegate(Element x)
				{
					x.CreateProxy((Element y) => new LayoutProxy(y));
				});
			}
			try
			{
				PageContext pageContext = new PageContext();
				DocumentGenerator.RenderPass(pageContext, new FreeDocumentCanvas(), container);
				pageContext.ProceedToNextRenderingPhase();
				canvas.ConfigureWithSemanticTree(semanticTreeManager);
				canvas.BeginDocument();
				DocumentGenerator.RenderPass(pageContext, canvas, container);
				canvas.EndDocument();
				CompanionDocumentCanvas companionDocumentCanvas = canvas as CompanionDocumentCanvas;
				if (companionDocumentCanvas != null)
				{
					companionDocumentCanvas.Hierarchy = container.ExtractHierarchy();
				}
			}
			finally
			{
				container.ReleaseDisposableChildren();
			}
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00017610 File Offset: 0x00015810
		private static void RenderMergedDocument(IDocumentCanvas canvas, MergedDocument document, DocumentSettings settings)
		{
			bool useOriginalImages = canvas is ImageDocumentCanvas;
			PageContext sharedPageContent = new PageContext();
			bool useSharedPageContext = document.PageNumberStrategy == MergedDocumentPageNumberStrategy.Continuous;
			SemanticTreeManager semanticTreeManager = DocumentGenerator.CreateSemanticTreeManager(settings);
			var list = Enumerable.ToList(Enumerable.Select(Enumerable.Range(0, document.Documents.Count), (int index) => new <>f__AnonymousType0<int, Container, PageContext>(index, DocumentGenerator.ConfigureContent(document.Documents[index], settings, semanticTreeManager, useOriginalImages), useSharedPageContext ? sharedPageContent : new PageContext())));
			try
			{
				foreach (var <>f__AnonymousType in list)
				{
					<>f__AnonymousType.PageContext.SetDocumentId(<>f__AnonymousType.DocumentId);
				}
				foreach (var <>f__AnonymousType2 in list)
				{
					DocumentGenerator.RenderPass(<>f__AnonymousType2.PageContext, new FreeDocumentCanvas(), <>f__AnonymousType2.Content);
					<>f__AnonymousType2.PageContext.ProceedToNextRenderingPhase();
				}
				canvas.ConfigureWithSemanticTree(semanticTreeManager);
				canvas.BeginDocument();
				foreach (var <>f__AnonymousType3 in list)
				{
					DocumentGenerator.RenderPass(<>f__AnonymousType3.PageContext, canvas, <>f__AnonymousType3.Content);
					<>f__AnonymousType3.Content.ReleaseDisposableChildren();
				}
				canvas.EndDocument();
			}
			finally
			{
				list.ForEach(delegate(x)
				{
					x.Content.ReleaseDisposableChildren();
				});
			}
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x000177E8 File Offset: 0x000159E8
		[return: Nullable(2)]
		private static SemanticTreeManager CreateSemanticTreeManager(DocumentSettings settings)
		{
			DocumentGenerator.<>c__DisplayClass9_0 CS$<>8__locals1;
			CS$<>8__locals1.settings = settings;
			if (!DocumentGenerator.<CreateSemanticTreeManager>g__IsDocumentSemanticAware|9_0(ref CS$<>8__locals1))
			{
				return null;
			}
			return new SemanticTreeManager();
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00017810 File Offset: 0x00015A10
		private static void ConfigureWithSemanticTree(this IDocumentCanvas canvas, [Nullable(2)] SemanticTreeManager semanticTreeManager)
		{
			if (semanticTreeManager == null)
			{
				return;
			}
			SemanticTreeNode semanticTree = semanticTreeManager.GetSemanticTree();
			semanticTreeManager.Reset();
			canvas.SetSemanticTree(semanticTree);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00017838 File Offset: 0x00015A38
		private static Container ConfigureContent(IDocument document, DocumentSettings settings, [Nullable(2)] SemanticTreeManager semanticTreeManager, bool useOriginalImages)
		{
			DocumentContainer documentContainer = new DocumentContainer();
			document.Compose(documentContainer);
			Container container = documentContainer.Compose();
			container.ApplyInheritedAndGlobalTexStyle(TextStyle.Default);
			container.ApplyContentDirection(new ContentDirection?(settings.ContentDirection));
			container.ApplyDefaultImageConfiguration(settings.ImageRasterDpi, settings.ImageCompressionQuality, useOriginalImages);
			if (Settings.EnableCaching)
			{
				container.ApplyCaching();
			}
			if (semanticTreeManager != null)
			{
				container.ApplySemanticParagraphs();
				container.InjectSemanticTreeManager(semanticTreeManager);
			}
			return container;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x000178A8 File Offset: 0x00015AA8
		private static void RenderPass(PageContext pageContext, IDocumentCanvas canvas, ContainerElement content)
		{
			DocumentGenerator.<>c__DisplayClass12_0 CS$<>8__locals1;
			CS$<>8__locals1.content = content;
			CS$<>8__locals1.pageContext = pageContext;
			CS$<>8__locals1.canvas = canvas;
			CS$<>8__locals1.content.InjectDependencies(CS$<>8__locals1.pageContext, CS$<>8__locals1.canvas.GetDrawingCanvas());
			CS$<>8__locals1.content.VisitChildren(delegate(Element x)
			{
				IStateful stateful = x as IStateful;
				if (stateful == null)
				{
					return;
				}
				stateful.ResetState(true);
			});
			SpacePlan spacePlan;
			do
			{
				CS$<>8__locals1.pageContext.IncrementPageNumber();
				spacePlan = CS$<>8__locals1.content.Measure(Size.Max);
				if (spacePlan.Type == SpacePlanType.Wrap)
				{
					CS$<>8__locals1.pageContext.DecrementPageNumber();
					CS$<>8__locals1.canvas.EndDocument();
					if (!CompanionService.IsCompanionAttached)
					{
						DocumentGenerator.<RenderPass>g__ThrowLayoutException|12_2(ref CS$<>8__locals1);
					}
					DocumentGenerator.<RenderPass>g__ApplyLayoutDebugging|12_1(ref CS$<>8__locals1);
				}
				try
				{
					CS$<>8__locals1.canvas.BeginPage(spacePlan);
					CS$<>8__locals1.content.Draw(spacePlan);
				}
				catch (Exception inner)
				{
					CS$<>8__locals1.canvas.EndDocument();
					throw new DocumentDrawingException("An exception occured during document drawing.", inner);
				}
				CS$<>8__locals1.canvas.EndPage();
			}
			while (spacePlan.Type != SpacePlanType.FullRender);
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x000179C8 File Offset: 0x00015BC8
		internal static void InjectSemanticTreeManager(this Element content, SemanticTreeManager semanticTreeManager)
		{
			content.VisitChildren(delegate(Element x)
			{
				ISemanticAware semanticAware = x as ISemanticAware;
				if (semanticAware != null)
				{
					semanticAware.SemanticTreeManager = semanticTreeManager;
					return;
				}
				TextBlock textBlock = x as TextBlock;
				if (textBlock != null)
				{
					foreach (TextBlockElement textBlockElement in Enumerable.OfType<TextBlockElement>(textBlock.Items))
					{
						textBlockElement.Element.InjectSemanticTreeManager(semanticTreeManager);
					}
				}
			});
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x000179F4 File Offset: 0x00015BF4
		internal static void InjectDependencies(this Element content, IPageContext pageContext, IDrawingCanvas canvas)
		{
			content.VisitChildren(delegate(Element x)
			{
				if (x == null)
				{
					return;
				}
				x.PageContext = pageContext;
				x.Canvas = canvas;
			});
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x00017A27 File Offset: 0x00015C27
		[NullableContext(2)]
		internal static void ApplyCaching(this Element content)
		{
			if (DocumentGenerator.<ApplyCaching>g__Traverse|15_1(content) && content != null)
			{
				content.CreateProxy((Element x) => new SnapshotCacheRecorderProxy(x));
			}
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00017A5C File Offset: 0x00015C5C
		[NullableContext(2)]
		internal static void ApplyContentDirection(this Element content, ContentDirection? direction = null)
		{
			if (content == null)
			{
				return;
			}
			ContentDirectionSetter contentDirectionSetter = content as ContentDirectionSetter;
			if (contentDirectionSetter != null)
			{
				contentDirectionSetter.Child.ApplyContentDirection(new ContentDirection?(contentDirectionSetter.ContentDirection));
				return;
			}
			IContentDirectionAware contentDirectionAware = content as IContentDirectionAware;
			if (contentDirectionAware != null)
			{
				contentDirectionAware.ContentDirection = (direction ?? contentDirectionAware.ContentDirection);
			}
			foreach (Element content2 in content.GetChildren())
			{
				content2.ApplyContentDirection(direction);
			}
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00017AF8 File Offset: 0x00015CF8
		[NullableContext(2)]
		internal static void ApplyDefaultImageConfiguration(this Element content, int imageRasterDpi, ImageCompressionQuality imageCompressionQuality, bool useOriginalImages)
		{
			content.VisitChildren(delegate(Element x)
			{
				QuestPDF.Elements.Image image = x as QuestPDF.Elements.Image;
				if (image != null)
				{
					QuestPDF.Elements.Image image2 = image;
					int? num = image2.TargetDpi;
					int num2 = num.GetValueOrDefault();
					if (num == null)
					{
						num2 = imageRasterDpi;
						QuestPDF.Elements.Image image3 = image2;
						int? num3 = new int?(num2);
						image3.TargetDpi = num3;
					}
					image2 = image;
					ImageCompressionQuality? imageCompressionQuality2 = image2.CompressionQuality;
					ImageCompressionQuality imageCompressionQuality3 = imageCompressionQuality2.GetValueOrDefault();
					if (imageCompressionQuality2 == null)
					{
						imageCompressionQuality3 = imageCompressionQuality;
						QuestPDF.Elements.Image image4 = image2;
						ImageCompressionQuality? imageCompressionQuality4 = new ImageCompressionQuality?(imageCompressionQuality3);
						image4.CompressionQuality = imageCompressionQuality4;
					}
					image.UseOriginalImage |= useOriginalImages;
				}
				DynamicImage dynamicImage = x as DynamicImage;
				if (dynamicImage != null)
				{
					DynamicImage dynamicImage2 = dynamicImage;
					int? num = dynamicImage2.TargetDpi;
					int num2 = num.GetValueOrDefault();
					if (num == null)
					{
						num2 = imageRasterDpi;
						DynamicImage dynamicImage3 = dynamicImage2;
						int? num3 = new int?(num2);
						dynamicImage3.TargetDpi = num3;
					}
					dynamicImage2 = dynamicImage;
					ImageCompressionQuality? imageCompressionQuality2 = dynamicImage2.CompressionQuality;
					ImageCompressionQuality imageCompressionQuality3 = imageCompressionQuality2.GetValueOrDefault();
					if (imageCompressionQuality2 == null)
					{
						imageCompressionQuality3 = imageCompressionQuality;
						DynamicImage dynamicImage4 = dynamicImage2;
						ImageCompressionQuality? imageCompressionQuality4 = new ImageCompressionQuality?(imageCompressionQuality3);
						dynamicImage4.CompressionQuality = imageCompressionQuality4;
					}
					dynamicImage.UseOriginalImage |= useOriginalImages;
				}
				DynamicHost dynamicHost = x as DynamicHost;
				if (dynamicHost != null)
				{
					DynamicHost dynamicHost2 = dynamicHost;
					int? num = dynamicHost2.ImageTargetDpi;
					int num2 = num.GetValueOrDefault();
					if (num == null)
					{
						num2 = imageRasterDpi;
						DynamicHost dynamicHost3 = dynamicHost2;
						int? num3 = new int?(num2);
						dynamicHost3.ImageTargetDpi = num3;
					}
					dynamicHost2 = dynamicHost;
					ImageCompressionQuality? imageCompressionQuality2 = dynamicHost2.ImageCompressionQuality;
					ImageCompressionQuality imageCompressionQuality3 = imageCompressionQuality2.GetValueOrDefault();
					if (imageCompressionQuality2 == null)
					{
						imageCompressionQuality3 = imageCompressionQuality;
						DynamicHost dynamicHost4 = dynamicHost2;
						ImageCompressionQuality? imageCompressionQuality4 = new ImageCompressionQuality?(imageCompressionQuality3);
						dynamicHost4.ImageCompressionQuality = imageCompressionQuality4;
					}
					dynamicHost.UseOriginalImage |= useOriginalImages;
				}
				Lazy lazy = x as Lazy;
				if (lazy != null)
				{
					Lazy lazy2 = lazy;
					int? num = lazy2.ImageTargetDpi;
					int num2 = num.GetValueOrDefault();
					if (num == null)
					{
						num2 = imageRasterDpi;
						Lazy lazy3 = lazy2;
						int? num3 = new int?(num2);
						lazy3.ImageTargetDpi = num3;
					}
					lazy2 = lazy;
					ImageCompressionQuality? imageCompressionQuality2 = lazy2.ImageCompressionQuality;
					ImageCompressionQuality imageCompressionQuality3 = imageCompressionQuality2.GetValueOrDefault();
					if (imageCompressionQuality2 == null)
					{
						imageCompressionQuality3 = imageCompressionQuality;
						Lazy lazy4 = lazy2;
						ImageCompressionQuality? imageCompressionQuality4 = new ImageCompressionQuality?(imageCompressionQuality3);
						lazy4.ImageCompressionQuality = imageCompressionQuality4;
					}
					lazy.UseOriginalImage |= useOriginalImages;
				}
				TextBlock textBlock = x as TextBlock;
				if (textBlock != null)
				{
					foreach (TextBlockElement textBlockElement in Enumerable.OfType<TextBlockElement>(textBlock.Items))
					{
						textBlockElement.Element.ApplyDefaultImageConfiguration(imageRasterDpi, imageCompressionQuality, useOriginalImages);
					}
				}
			});
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00017B34 File Offset: 0x00015D34
		internal static void ApplyInheritedAndGlobalTexStyle([Nullable(2)] this Element content, TextStyle documentDefaultTextStyle)
		{
			if (content == null)
			{
				return;
			}
			TextBlock textBlock = content as TextBlock;
			if (textBlock != null)
			{
				textBlock.DefaultTextStyle = textBlock.DefaultTextStyle.ApplyInheritedStyle(documentDefaultTextStyle).ApplyGlobalStyle();
				foreach (ITextBlockItem textBlockItem in textBlock.Items)
				{
					TextBlockSpan textBlockSpan = textBlockItem as TextBlockSpan;
					if (textBlockSpan != null)
					{
						textBlockSpan.Style = textBlockSpan.Style.ApplyInheritedStyle(documentDefaultTextStyle).ApplyGlobalStyle();
					}
					TextBlockElement textBlockElement = textBlockItem as TextBlockElement;
					if (textBlockElement != null)
					{
						textBlockElement.Element.ApplyInheritedAndGlobalTexStyle(documentDefaultTextStyle);
					}
				}
				return;
			}
			DynamicHost dynamicHost = content as DynamicHost;
			if (dynamicHost != null)
			{
				dynamicHost.TextStyle = dynamicHost.TextStyle.ApplyInheritedStyle(documentDefaultTextStyle);
			}
			Lazy lazy = content as Lazy;
			if (lazy != null)
			{
				lazy.TextStyle = lazy.TextStyle.ApplyInheritedStyle(documentDefaultTextStyle);
			}
			DefaultTextStyle defaultTextStyle = content as DefaultTextStyle;
			if (defaultTextStyle != null)
			{
				documentDefaultTextStyle = defaultTextStyle.TextStyle.ApplyInheritedStyle(documentDefaultTextStyle);
			}
			ContainerElement containerElement = content as ContainerElement;
			if (containerElement != null)
			{
				containerElement.Child.ApplyInheritedAndGlobalTexStyle(documentDefaultTextStyle);
				return;
			}
			foreach (Element content2 in content.GetChildren())
			{
				content2.ApplyInheritedAndGlobalTexStyle(documentDefaultTextStyle);
			}
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x00017C90 File Offset: 0x00015E90
		internal static void ApplySemanticParagraphs(this Element root)
		{
			DocumentGenerator.<>c__DisplayClass19_0 CS$<>8__locals1;
			CS$<>8__locals1.isFooterContext = false;
			DocumentGenerator.<ApplySemanticParagraphs>g__Traverse|19_0(root, ref CS$<>8__locals1);
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x00017CB0 File Offset: 0x00015EB0
		[CompilerGenerated]
		internal static bool <CreateSemanticTreeManager>g__IsDocumentSemanticAware|9_0(ref DocumentGenerator.<>c__DisplayClass9_0 A_0)
		{
			if (A_0.settings.PDFUA_Conformance != QuestPDF.Infrastructure.PDFUA_Conformance.None)
			{
				return true;
			}
			QuestPDF.Infrastructure.PDFA_Conformance pdfa_Conformance = A_0.settings.PDFA_Conformance;
			return pdfa_Conformance == QuestPDF.Infrastructure.PDFA_Conformance.PDFA_2A || pdfa_Conformance == QuestPDF.Infrastructure.PDFA_Conformance.PDFA_3A;
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00017CEC File Offset: 0x00015EEC
		[CompilerGenerated]
		internal static void <RenderPass>g__ApplyLayoutDebugging|12_1(ref DocumentGenerator.<>c__DisplayClass12_0 A_0)
		{
			A_0.content.VisitChildren(delegate(Element x)
			{
				SnapshotCacheRecorderProxy snapshotCacheRecorderProxy = x as SnapshotCacheRecorderProxy;
				if (snapshotCacheRecorderProxy == null)
				{
					return;
				}
				snapshotCacheRecorderProxy.Dispose();
			});
			A_0.content.RemoveExistingProxiesOfType<SnapshotCacheRecorderProxy>();
			A_0.content.ApplyLayoutOverflowDetection();
			A_0.content.Measure(Size.Max);
			TreeNode<OverflowDebuggingProxy> treeNode = Enumerable.Single<TreeNode<OverflowDebuggingProxy>>(A_0.content.ExtractElementsOfType<OverflowDebuggingProxy>());
			treeNode.StopMeasuring();
			treeNode.TryToFixTheLayoutOverflowIssue();
			A_0.content.ApplyContentDirection(default(ContentDirection?));
			A_0.content.InjectDependencies(A_0.pageContext, A_0.canvas.GetDrawingCanvas());
			A_0.content.VisitChildren(delegate(Element x)
			{
				LayoutProxy layoutProxy = x as LayoutProxy;
				if (layoutProxy == null)
				{
					return;
				}
				layoutProxy.CaptureLayoutErrorMeasurement();
			});
			A_0.content.RemoveExistingProxiesOfType<OverflowDebuggingProxy>();
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x00017DCC File Offset: 0x00015FCC
		[CompilerGenerated]
		internal static void <RenderPass>g__ThrowLayoutException|12_2(ref DocumentGenerator.<>c__DisplayClass12_0 A_0)
		{
			string text = "\n";
			string text2 = text + text;
			string text3 = "The provided document content contains conflicting size constraints. For example, some elements may require more space than is available. " + text2;
			if (Settings.EnableDebugging)
			{
				ValueTuple<ICollection<Element>, TreeNode<OverflowDebuggingProxy>> valueTuple = DocumentGenerator.<RenderPass>g__GenerateLayoutExceptionDebuggingInfo|12_3(ref A_0);
				ICollection<Element> item = valueTuple.Item1;
				TreeNode<OverflowDebuggingProxy> item2 = valueTuple.Item2;
				string text4 = item.FormatAncestors();
				string text5 = item2.FormatLayoutSubtree();
				string text6 = text3;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(285, 8);
				defaultInterpolatedStringHandler.AppendLiteral("The layout issue is likely present in the following part of the document: ");
				defaultInterpolatedStringHandler.AppendFormatted(text2);
				defaultInterpolatedStringHandler.AppendFormatted(text4);
				defaultInterpolatedStringHandler.AppendFormatted(text2);
				defaultInterpolatedStringHandler.AppendLiteral("To learn more, please analyse the document measurement of the problematic location: ");
				defaultInterpolatedStringHandler.AppendFormatted(text2);
				defaultInterpolatedStringHandler.AppendFormatted(text5);
				defaultInterpolatedStringHandler.AppendFormatted("Legend: \n\ud83d\udea8 - Element that is likely the root cause of the layout issue based on library heuristics and prediction. \n\ud83d\udd34 - Element that cannot be drawn due to the provided layout constraints. This element likely causes the layout issue, or one of its descendant children is responsible for the problem. \n\ud83d\udfe1 - Element that can be partially drawn on the page and will also be rendered on the consecutive page. In more complex layouts, this element may also cause issues or contain a child that is the actual root cause.\n\ud83d\udfe2 - Element that is successfully and completely drawn on the page.\n⚪️ - Element that has not been drawn on the faulty page. Its children are omitted.\n");
				defaultInterpolatedStringHandler.AppendFormatted(text2);
				defaultInterpolatedStringHandler.AppendLiteral("This detailed information is generated because you run the application with a debugger attached or with the ");
				defaultInterpolatedStringHandler.AppendFormatted("QuestPDF.Settings.EnableDebugging");
				defaultInterpolatedStringHandler.AppendLiteral(" flag set to true. ");
				text3 = text6 + defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				text3 += "To further investigate the location of the root cause, please run the application with a debugger attached or set the QuestPDF.Settings.EnableDebugging flag to true. The library will generate additional debugging information such as probable code problem location and detailed layout measurement overview.";
			}
			throw new DocumentLayoutException(text3);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00017EC4 File Offset: 0x000160C4
		[CompilerGenerated]
		[return: TupleElementNames(new string[]
		{
			"ancestors",
			"layout"
		})]
		[return: Nullable(new byte[]
		{
			0,
			1,
			1,
			1,
			1
		})]
		internal static ValueTuple<ICollection<Element>, TreeNode<OverflowDebuggingProxy>> <RenderPass>g__GenerateLayoutExceptionDebuggingInfo|12_3(ref DocumentGenerator.<>c__DisplayClass12_0 A_0)
		{
			A_0.content.RemoveExistingProxies();
			A_0.content.ApplyLayoutOverflowDetection();
			A_0.content.Measure(Size.Max);
			TreeNode<OverflowDebuggingProxy> treeNode = Enumerable.Single<TreeNode<OverflowDebuggingProxy>>(A_0.content.ExtractElementsOfType<OverflowDebuggingProxy>());
			treeNode.StopMeasuring();
			treeNode.TryToFixTheLayoutOverflowIssue();
			TreeNode<OverflowDebuggingProxy> node = Enumerable.First<TreeNode<OverflowDebuggingProxy>>(treeNode.FindLayoutOverflowVisualizationNodes());
			ICollection<Element> collection = Enumerable.ToArray<Element>(Enumerable.Reverse<Element>(Enumerable.Where<Element>(Enumerable.Select<TreeNode<OverflowDebuggingProxy>, Element>(node.ExtractAncestors<OverflowDebuggingProxy>(), (TreeNode<OverflowDebuggingProxy> x) => x.Value.Child), (Element x) => x is DebugPointer || x is SourceCodePointer)));
			TreeNode<OverflowDebuggingProxy> treeNode2 = Enumerable.First<TreeNode<OverflowDebuggingProxy>>(Enumerable.First<TreeNode<OverflowDebuggingProxy>>(node.ExtractAncestors<OverflowDebuggingProxy>(), delegate(TreeNode<OverflowDebuggingProxy> x)
			{
				Element child = x.Value.Child;
				return child is SourceCodePointer || child is DebugPointer;
			}).Children);
			return new ValueTuple<ICollection<Element>, TreeNode<OverflowDebuggingProxy>>(collection, treeNode2);
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00017FB4 File Offset: 0x000161B4
		[NullableContext(2)]
		[CompilerGenerated]
		internal static bool <ApplyCaching>g__Traverse|15_1(Element content)
		{
			TextBlock textBlock = content as TextBlock;
			if (textBlock != null)
			{
				foreach (ITextBlockItem textBlockItem in textBlock.Items)
				{
					if (textBlockItem is TextBlockPageNumber)
					{
						return false;
					}
					TextBlockElement textBlockElement = textBlockItem as TextBlockElement;
					if (textBlockElement != null && !DocumentGenerator.<ApplyCaching>g__Traverse|15_1(textBlockElement.Element))
					{
						return false;
					}
				}
				return true;
			}
			Lazy lazy = content as Lazy;
			if (lazy != null)
			{
				return lazy.IsCacheable;
			}
			if (content is DynamicHost)
			{
				return false;
			}
			ContainerElement containerElement = content as ContainerElement;
			if (containerElement != null)
			{
				return DocumentGenerator.<ApplyCaching>g__Traverse|15_1(containerElement.Child);
			}
			MultiColumn multiColumn = content as MultiColumn;
			if (multiColumn != null)
			{
				bool result = DocumentGenerator.<ApplyCaching>g__Traverse|15_1(multiColumn.Content) && DocumentGenerator.<ApplyCaching>g__Traverse|15_1(multiColumn.Spacer);
				multiColumn.Content.RemoveExistingProxies();
				multiColumn.Spacer.RemoveExistingProxies();
				return result;
			}
			bool[] canApplyCachingPerChild = Enumerable.ToArray<bool>(Enumerable.Select<Element, bool>(content.GetChildren(), new Func<Element, bool>(DocumentGenerator.<ApplyCaching>g__Traverse|15_1)));
			if (Enumerable.All<bool>(canApplyCachingPerChild, (bool x) => x))
			{
				return true;
			}
			Row row = content as Row;
			if (row != null)
			{
				if (Enumerable.Any<RowItem>(row.Items, (RowItem x) => x.Type == RowItemType.Auto))
				{
					return false;
				}
			}
			int childIndex = 0;
			content.CreateProxy(delegate(Element x)
			{
				int childIndex;
				bool flag = canApplyCachingPerChild[childIndex];
				childIndex = childIndex;
				childIndex++;
				if (!flag)
				{
					return x;
				}
				return new SnapshotCacheRecorderProxy(x);
			});
			return false;
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00018164 File Offset: 0x00016364
		[CompilerGenerated]
		internal static void <ApplySemanticParagraphs>g__Traverse|19_0(Element element, ref DocumentGenerator.<>c__DisplayClass19_0 A_1)
		{
			SemanticTag semanticTag = element as SemanticTag;
			bool flag;
			if (semanticTag != null)
			{
				string tagType = semanticTag.TagType;
				if (tagType != null)
				{
					switch (tagType.Length)
					{
					case 1:
					{
						char c = tagType.get_Chars(0);
						if (c != 'H' && c != 'P')
						{
							goto IL_F2;
						}
						break;
					}
					case 2:
						switch (tagType.get_Chars(1))
						{
						case '1':
							if (!(tagType == "H1"))
							{
								goto IL_F2;
							}
							break;
						case '2':
							if (!(tagType == "H2"))
							{
								goto IL_F2;
							}
							break;
						case '3':
							if (!(tagType == "H3"))
							{
								goto IL_F2;
							}
							break;
						case '4':
							if (!(tagType == "H4"))
							{
								goto IL_F2;
							}
							break;
						case '5':
							if (!(tagType == "H5"))
							{
								goto IL_F2;
							}
							break;
						case '6':
							if (!(tagType == "H6"))
							{
								goto IL_F2;
							}
							break;
						default:
							goto IL_F2;
						}
						break;
					case 3:
						if (!(tagType == "Lbl"))
						{
							goto IL_F2;
						}
						break;
					default:
						goto IL_F2;
					}
					flag = true;
					goto IL_F4;
				}
			}
			IL_F2:
			flag = false;
			IL_F4:
			if (flag)
			{
				return;
			}
			if (!(element is ArtifactTag))
			{
				DebugPointer debugPointer = element as DebugPointer;
				if (debugPointer != null && debugPointer.Type == DebugPointerType.DocumentStructure && debugPointer.Label == "Footer")
				{
					A_1.isFooterContext = true;
					DocumentGenerator.<ApplySemanticParagraphs>g__Traverse|19_0(debugPointer.Child, ref A_1);
					A_1.isFooterContext = false;
					return;
				}
				ContainerElement containerElement = element as ContainerElement;
				if (containerElement != null)
				{
					TextBlock textBlock = containerElement.Child as TextBlock;
					if (textBlock == null)
					{
						DocumentGenerator.<ApplySemanticParagraphs>g__Traverse|19_0(containerElement.Child, ref A_1);
						return;
					}
					bool flag2 = Enumerable.Any<ITextBlockItem>(textBlock.Items, (ITextBlockItem x) => x is TextBlockPageNumber);
					if (A_1.isFooterContext && flag2)
					{
						return;
					}
					containerElement.CreateProxy((Element x) => new SemanticTag
					{
						Child = x,
						TagType = "P"
					});
					return;
				}
				else
				{
					foreach (Element element2 in element.GetChildren())
					{
						DocumentGenerator.<ApplySemanticParagraphs>g__Traverse|19_0(element2, ref A_1);
					}
				}
			}
		}
	}
}
