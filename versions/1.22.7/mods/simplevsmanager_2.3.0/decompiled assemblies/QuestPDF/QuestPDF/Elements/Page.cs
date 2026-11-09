using System;
using System.Runtime.CompilerServices;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000DD RID: 221
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Page : IComponent
	{
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x000113BD File Offset: 0x0000F5BD
		// (set) Token: 0x06000719 RID: 1817 RVA: 0x000113C5 File Offset: 0x0000F5C5
		public ContentDirection ContentDirection { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x000113CE File Offset: 0x0000F5CE
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x000113D6 File Offset: 0x0000F5D6
		public TextStyle DefaultTextStyle { get; set; } = TextStyle.Default;

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x000113DF File Offset: 0x0000F5DF
		// (set) Token: 0x0600071D RID: 1821 RVA: 0x000113E7 File Offset: 0x0000F5E7
		public Size MinSize { get; set; } = PageSizes.A4;

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x000113F0 File Offset: 0x0000F5F0
		// (set) Token: 0x0600071F RID: 1823 RVA: 0x000113F8 File Offset: 0x0000F5F8
		public Size MaxSize { get; set; } = PageSizes.A4;

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x00011401 File Offset: 0x0000F601
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x00011409 File Offset: 0x0000F609
		public float MarginLeft { get; set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x00011412 File Offset: 0x0000F612
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x0001141A File Offset: 0x0000F61A
		public float MarginRight { get; set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x00011423 File Offset: 0x0000F623
		// (set) Token: 0x06000725 RID: 1829 RVA: 0x0001142B File Offset: 0x0000F62B
		public float MarginTop { get; set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x00011434 File Offset: 0x0000F634
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x0001143C File Offset: 0x0000F63C
		public float MarginBottom { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x00011445 File Offset: 0x0000F645
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x0001144D File Offset: 0x0000F64D
		public Color BackgroundColor { get; set; } = Colors.Transparent;

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x00011456 File Offset: 0x0000F656
		// (set) Token: 0x0600072B RID: 1835 RVA: 0x0001145E File Offset: 0x0000F65E
		public Element Background { get; set; } = Empty.Instance;

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x00011467 File Offset: 0x0000F667
		// (set) Token: 0x0600072D RID: 1837 RVA: 0x0001146F File Offset: 0x0000F66F
		public Element Foreground { get; set; } = Empty.Instance;

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x00011478 File Offset: 0x0000F678
		// (set) Token: 0x0600072F RID: 1839 RVA: 0x00011480 File Offset: 0x0000F680
		public Element Header { get; set; } = Empty.Instance;

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x00011489 File Offset: 0x0000F689
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x00011491 File Offset: 0x0000F691
		public Element Content { get; set; } = Empty.Instance;

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x0001149A File Offset: 0x0000F69A
		// (set) Token: 0x06000733 RID: 1843 RVA: 0x000114A2 File Offset: 0x0000F6A2
		public Element Footer { get; set; } = Empty.Instance;

		// Token: 0x06000734 RID: 1844 RVA: 0x000114AC File Offset: 0x0000F6AC
		public void Compose(IContainer container)
		{
			container.DebugPointer(DebugPointerType.DocumentStructure, DocumentStructureTypes.Page.ToString()).ContentDirection(this.ContentDirection).DefaultTextStyle(this.DefaultTextStyle.DisableFontFeature("liga")).Layers(delegate(LayersDescriptor layers)
			{
				layers.Layer().ZIndex(int.MinValue).Background(this.BackgroundColor);
				layers.Layer().Repeat().DebugPointer(DebugPointerType.DocumentStructure, DocumentStructureTypes.Background.ToString()).Artifact(-8).Element(this.Background);
				layers.PrimaryLayer().MinWidth(this.MinSize.Width, Unit.Point).MinHeight(this.MinSize.Height, Unit.Point).MaxWidth(this.MaxSize.Width, Unit.Point).MaxHeight(this.MaxSize.Height, Unit.Point).EnforceSizeWhenEmpty().PaddingLeft(this.MarginLeft, Unit.Point).PaddingRight(this.MarginRight, Unit.Point).PaddingTop(this.MarginTop, Unit.Point).PaddingBottom(this.MarginBottom, Unit.Point).Decoration(delegate(DecorationDescriptor decoration)
				{
					decoration.Before().DebugPointer(DebugPointerType.DocumentStructure, DocumentStructureTypes.Header.ToString()).Element(this.Header);
					decoration.Content().NonTrackingElement(delegate(IContainer x)
					{
						if (!Page.<Compose>g__IsClose|56_1(this.MinSize.Width, this.MaxSize.Width))
						{
							return x;
						}
						return x.ExtendHorizontal();
					}).NonTrackingElement(delegate(IContainer x)
					{
						if (!Page.<Compose>g__IsClose|56_1(this.MinSize.Height, this.MaxSize.Height))
						{
							return x;
						}
						return x.ExtendVertical();
					}).DebugPointer(DebugPointerType.DocumentStructure, DocumentStructureTypes.Content.ToString()).Element(this.Content);
					decoration.After().DebugPointer(DebugPointerType.DocumentStructure, DocumentStructureTypes.Footer.ToString()).Element(this.Footer);
				});
				layers.Layer().Repeat().Artifact(-5).DebugPointer(DebugPointerType.DocumentStructure, DocumentStructureTypes.Foreground.ToString()).Element(this.Foreground);
			});
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00011792 File Offset: 0x0000F992
		[CompilerGenerated]
		internal static bool <Compose>g__IsClose|56_1(float x, float y)
		{
			return Math.Abs(x - y) < 0.01f;
		}
	}
}
