using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000C2 RID: 194
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class DynamicHost : Element, IStateful, IContentDirectionAware, ISemanticAware
	{
		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060005CB RID: 1483 RVA: 0x0000E950 File Offset: 0x0000CB50
		// (set) Token: 0x060005CC RID: 1484 RVA: 0x0000E958 File Offset: 0x0000CB58
		[Nullable(2)]
		public SemanticTreeManager SemanticTreeManager { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x0000E961 File Offset: 0x0000CB61
		private DynamicComponentProxy Child { get; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x0000E969 File Offset: 0x0000CB69
		// (set) Token: 0x060005CF RID: 1487 RVA: 0x0000E971 File Offset: 0x0000CB71
		private object InitialComponentState { get; set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x0000E97A File Offset: 0x0000CB7A
		// (set) Token: 0x060005D1 RID: 1489 RVA: 0x0000E982 File Offset: 0x0000CB82
		internal TextStyle TextStyle { get; set; } = TextStyle.Default;

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0000E98B File Offset: 0x0000CB8B
		// (set) Token: 0x060005D3 RID: 1491 RVA: 0x0000E993 File Offset: 0x0000CB93
		public ContentDirection ContentDirection { get; set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x0000E99C File Offset: 0x0000CB9C
		// (set) Token: 0x060005D5 RID: 1493 RVA: 0x0000E9A4 File Offset: 0x0000CBA4
		internal int? ImageTargetDpi { get; set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0000E9AD File Offset: 0x0000CBAD
		// (set) Token: 0x060005D7 RID: 1495 RVA: 0x0000E9B5 File Offset: 0x0000CBB5
		internal ImageCompressionQuality? ImageCompressionQuality { get; set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0000E9BE File Offset: 0x0000CBBE
		// (set) Token: 0x060005D9 RID: 1497 RVA: 0x0000E9C6 File Offset: 0x0000CBC6
		internal bool UseOriginalImage { get; set; }

		// Token: 0x060005DA RID: 1498 RVA: 0x0000E9CF File Offset: 0x0000CBCF
		public DynamicHost(DynamicComponentProxy child)
		{
			this.Child = child;
			this.InitialComponentState = this.Child.GetState.Invoke();
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x0000EA00 File Offset: 0x0000CC00
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (this.IsRendered)
			{
				return SpacePlan.Empty();
			}
			if (availableSpace.IsCloseToZero())
			{
				return SpacePlan.PartialRender(Size.Zero);
			}
			DynamicContext dynamicContext = this.CreateContext(availableSpace);
			DynamicComponentComposeResult dynamicComponentComposeResult = this.ComposeContent(dynamicContext, false);
			Element element = (dynamicComponentComposeResult.Content as Element) ?? Empty.Instance;
			SpacePlan spacePlan = element.Measure(availableSpace);
			dynamicContext.DisposeCreatedElements();
			element.ReleaseDisposableChildren();
			SpacePlanType type = spacePlan.Type;
			bool flag = type - SpacePlanType.Wrap <= 1;
			if (flag)
			{
				throw new DocumentLayoutException("Dynamic component generated content that does not fit on a single page.");
			}
			if (!dynamicComponentComposeResult.HasMoreContent)
			{
				return SpacePlan.FullRender(spacePlan);
			}
			return SpacePlan.PartialRender(spacePlan);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0000EAAC File Offset: 0x0000CCAC
		internal override void Draw(Size availableSpace)
		{
			DynamicContext dynamicContext = this.CreateContext(availableSpace);
			DynamicComponentComposeResult dynamicComponentComposeResult = this.ComposeContent(dynamicContext, true);
			Element element = dynamicComponentComposeResult.Content as Element;
			if (element != null)
			{
				element.Draw(availableSpace);
			}
			dynamicContext.DisposeCreatedElements();
			element.ReleaseDisposableChildren();
			if (!dynamicComponentComposeResult.HasMoreContent)
			{
				this.IsRendered = true;
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x0000EAFC File Offset: 0x0000CCFC
		private DynamicContext CreateContext(Size availableSize)
		{
			return new DynamicContext
			{
				PageContext = base.PageContext,
				Canvas = base.Canvas,
				SemanticTreeManager = this.SemanticTreeManager,
				TextStyle = this.TextStyle,
				ContentDirection = this.ContentDirection,
				ImageTargetDpi = this.ImageTargetDpi.Value,
				ImageCompressionQuality = this.ImageCompressionQuality.Value,
				UseOriginalImage = this.UseOriginalImage,
				PageNumber = base.PageContext.CurrentPage,
				TotalPages = (base.PageContext.IsInitialRenderingPhase ? int.MaxValue : base.PageContext.DocumentLength),
				AvailableSize = availableSize
			};
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0000EBBC File Offset: 0x0000CDBC
		private DynamicComponentComposeResult ComposeContent(DynamicContext context, bool acceptNewState)
		{
			object obj = this.Child.GetState.Invoke();
			DynamicComponentComposeResult result = this.Child.Compose.Invoke(context);
			if (!acceptNewState)
			{
				this.Child.SetState.Invoke(obj);
			}
			return result;
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x0000EBFF File Offset: 0x0000CDFF
		// (set) Token: 0x060005E0 RID: 1504 RVA: 0x0000EC07 File Offset: 0x0000CE07
		private bool IsRendered { get; set; }

		// Token: 0x060005E1 RID: 1505 RVA: 0x0000EC10 File Offset: 0x0000CE10
		public void ResetState(bool hardReset = false)
		{
			this.IsRendered = false;
			this.Child.SetState.Invoke(this.InitialComponentState);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0000EC30 File Offset: 0x0000CE30
		public object GetState()
		{
			return new DynamicHost.DynamicState
			{
				IsRendered = this.IsRendered,
				ChildState = this.Child.GetState.Invoke()
			};
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0000EC70 File Offset: 0x0000CE70
		public void SetState(object state)
		{
			DynamicHost.DynamicState dynamicState = (DynamicHost.DynamicState)state;
			this.IsRendered = dynamicState.IsRendered;
			this.Child.SetState.Invoke(dynamicState.ChildState);
		}

		// Token: 0x020001AA RID: 426
		[NullableContext(0)]
		public struct DynamicState
		{
			// Token: 0x0400064C RID: 1612
			public bool IsRendered;

			// Token: 0x0400064D RID: 1613
			[Nullable(1)]
			public object ChildState;
		}
	}
}
