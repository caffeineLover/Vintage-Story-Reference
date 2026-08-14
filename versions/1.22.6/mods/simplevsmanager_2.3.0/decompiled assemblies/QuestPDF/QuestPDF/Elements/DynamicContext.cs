using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000C3 RID: 195
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DynamicContext
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x0000ECA6 File Offset: 0x0000CEA6
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x0000ECAE File Offset: 0x0000CEAE
		internal IPageContext PageContext { get; set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x0000ECB7 File Offset: 0x0000CEB7
		// (set) Token: 0x060005E7 RID: 1511 RVA: 0x0000ECBF File Offset: 0x0000CEBF
		internal IDrawingCanvas Canvas { get; set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x0000ECC8 File Offset: 0x0000CEC8
		// (set) Token: 0x060005E9 RID: 1513 RVA: 0x0000ECD0 File Offset: 0x0000CED0
		[Nullable(2)]
		internal SemanticTreeManager SemanticTreeManager { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x0000ECD9 File Offset: 0x0000CED9
		// (set) Token: 0x060005EB RID: 1515 RVA: 0x0000ECE1 File Offset: 0x0000CEE1
		internal TextStyle TextStyle { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x0000ECEA File Offset: 0x0000CEEA
		// (set) Token: 0x060005ED RID: 1517 RVA: 0x0000ECF2 File Offset: 0x0000CEF2
		internal ContentDirection ContentDirection { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x0000ECFB File Offset: 0x0000CEFB
		// (set) Token: 0x060005EF RID: 1519 RVA: 0x0000ED03 File Offset: 0x0000CF03
		internal int ImageTargetDpi { get; set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x0000ED0C File Offset: 0x0000CF0C
		// (set) Token: 0x060005F1 RID: 1521 RVA: 0x0000ED14 File Offset: 0x0000CF14
		internal ImageCompressionQuality ImageCompressionQuality { get; set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0000ED1D File Offset: 0x0000CF1D
		// (set) Token: 0x060005F3 RID: 1523 RVA: 0x0000ED25 File Offset: 0x0000CF25
		internal bool UseOriginalImage { get; set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060005F4 RID: 1524 RVA: 0x0000ED2E File Offset: 0x0000CF2E
		internal List<Element> CreatedElements { get; } = new List<Element>();

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x0000ED36 File Offset: 0x0000CF36
		// (set) Token: 0x060005F6 RID: 1526 RVA: 0x0000ED3E File Offset: 0x0000CF3E
		public int PageNumber { get; internal set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x0000ED47 File Offset: 0x0000CF47
		// (set) Token: 0x060005F8 RID: 1528 RVA: 0x0000ED4F File Offset: 0x0000CF4F
		public int TotalPages { get; internal set; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x0000ED58 File Offset: 0x0000CF58
		// (set) Token: 0x060005FA RID: 1530 RVA: 0x0000ED60 File Offset: 0x0000CF60
		public Size AvailableSize { get; internal set; }

		// Token: 0x060005FB RID: 1531 RVA: 0x0000ED69 File Offset: 0x0000CF69
		public ICollection<PageElementLocation> GetContentCapturedPositions(string id)
		{
			return this.PageContext.GetContentCapturedPositions(id);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0000ED78 File Offset: 0x0000CF78
		public IDynamicElement CreateElement(Action<IContainer> content)
		{
			DynamicElement dynamicElement = new DynamicElement();
			this.CreatedElements.Add(dynamicElement);
			content.Invoke(dynamicElement);
			dynamicElement.ApplyInheritedAndGlobalTexStyle(this.TextStyle);
			dynamicElement.ApplyContentDirection(new ContentDirection?(this.ContentDirection));
			dynamicElement.ApplyDefaultImageConfiguration(this.ImageTargetDpi, this.ImageCompressionQuality, this.UseOriginalImage);
			dynamicElement.InjectDependencies(this.PageContext, this.Canvas);
			dynamicElement.VisitChildren(delegate(Element x)
			{
				IStateful stateful = x as IStateful;
				if (stateful == null)
				{
					return;
				}
				stateful.ResetState(true);
			});
			if (this.SemanticTreeManager != null)
			{
				dynamicElement.InjectSemanticTreeManager(this.SemanticTreeManager);
				dynamicElement.ApplySemanticParagraphs();
			}
			dynamicElement.Size = dynamicElement.Measure(Size.Max);
			return dynamicElement;
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x0000EE3C File Offset: 0x0000D03C
		internal void DisposeCreatedElements()
		{
			foreach (Element element in this.CreatedElements)
			{
				element.ReleaseDisposableChildren();
			}
			this.CreatedElements.Clear();
		}
	}
}
