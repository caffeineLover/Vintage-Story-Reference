using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Companion;
using QuestPDF.Drawing.DrawingCanvases;
using QuestPDF.Elements;
using QuestPDF.Elements.Text;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Drawing.Proxy
{
	// Token: 0x02000117 RID: 279
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class LayoutProxy : ElementProxy
	{
		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060009A6 RID: 2470 RVA: 0x000198FE File Offset: 0x00017AFE
		public List<CompanionCommands.UpdateDocumentStructure.PageLocation> Snapshots { get; } = new List<CompanionCommands.UpdateDocumentStructure.PageLocation>();

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060009A7 RID: 2471 RVA: 0x00019906 File Offset: 0x00017B06
		public List<CompanionCommands.UpdateDocumentStructure.LayoutErrorMeasurement> LayoutErrorMeasurements { get; } = new List<CompanionCommands.UpdateDocumentStructure.LayoutErrorMeasurement>();

		// Token: 0x060009A8 RID: 2472 RVA: 0x0001990E File Offset: 0x00017B0E
		public LayoutProxy(Element child)
		{
			base.Child = child;
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x00019934 File Offset: 0x00017B34
		internal override void Draw(Size availableSpace)
		{
			Size size = this.<Draw>g__ProvideIntrinsicSize|7_0() ? base.Child.Measure(availableSpace) : availableSpace;
			base.Draw(availableSpace);
			if (base.Canvas is DiscardDrawingCanvas)
			{
				return;
			}
			SkCanvasMatrix currentMatrix = base.Canvas.GetCurrentMatrix();
			this.Snapshots.Add(new CompanionCommands.UpdateDocumentStructure.PageLocation
			{
				PageNumber = base.PageContext.CurrentPage,
				Left = currentMatrix.TranslateX,
				Top = currentMatrix.TranslateY,
				Right = currentMatrix.TranslateX + size.Width,
				Bottom = currentMatrix.TranslateY + size.Height
			});
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x000199E0 File Offset: 0x00017BE0
		internal void CaptureLayoutErrorMeasurement()
		{
			Element child = base.Child;
			for (;;)
			{
				OverflowDebuggingProxy overflowDebuggingProxy = child as OverflowDebuggingProxy;
				if (overflowDebuggingProxy != null)
				{
					if (overflowDebuggingProxy.AvailableSpace == null)
					{
						break;
					}
					SpacePlan? spacePlan = overflowDebuggingProxy.SpacePlan;
					if (spacePlan == null)
					{
						break;
					}
					List<CompanionCommands.UpdateDocumentStructure.LayoutErrorMeasurement> layoutErrorMeasurements = this.LayoutErrorMeasurements;
					CompanionCommands.UpdateDocumentStructure.LayoutErrorMeasurement layoutErrorMeasurement = new CompanionCommands.UpdateDocumentStructure.LayoutErrorMeasurement();
					layoutErrorMeasurement.PageNumber = base.PageContext.CurrentPage;
					layoutErrorMeasurement.AvailableSpace = new CompanionCommands.ElementSize
					{
						Width = overflowDebuggingProxy.AvailableSpace.Value.Width,
						Height = overflowDebuggingProxy.AvailableSpace.Value.Height
					};
					CompanionCommands.ElementSize elementSize = new CompanionCommands.ElementSize();
					spacePlan = overflowDebuggingProxy.SpacePlan;
					elementSize.Width = spacePlan.Value.Width;
					spacePlan = overflowDebuggingProxy.SpacePlan;
					elementSize.Height = spacePlan.Value.Height;
					layoutErrorMeasurement.MeasurementSize = elementSize;
					spacePlan = overflowDebuggingProxy.SpacePlan;
					layoutErrorMeasurement.SpacePlanType = ((spacePlan != null) ? new SpacePlanType?(spacePlan.GetValueOrDefault().Type) : default(SpacePlanType?));
					spacePlan = overflowDebuggingProxy.SpacePlan;
					layoutErrorMeasurement.WrapReason = ((spacePlan != null) ? spacePlan.GetValueOrDefault().WrapReason : null);
					layoutErrorMeasurement.IsLayoutErrorRootCause = (overflowDebuggingProxy.Child.GetType() == typeof(LayoutOverflowVisualization));
					layoutErrorMeasurements.Add(layoutErrorMeasurement);
				}
				ElementProxy elementProxy = child as ElementProxy;
				if (elementProxy == null)
				{
					break;
				}
				child = elementProxy.Child;
			}
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x00019B58 File Offset: 0x00017D58
		[CompilerGenerated]
		private bool <Draw>g__ProvideIntrinsicSize|7_0()
		{
			Element child = base.Child;
			return child is TextBlock || child is AspectRatio || child is Unconstrained || child is SemanticTag || child is ArtifactTag;
		}
	}
}
