using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000D7 RID: 215
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Lazy : ContainerElement, ISemanticAware, IContentDirectionAware, IStateful
	{
		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x000106DE File Offset: 0x0000E8DE
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x000106E6 File Offset: 0x0000E8E6
		[Nullable(2)]
		public SemanticTreeManager SemanticTreeManager { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x000106EF File Offset: 0x0000E8EF
		// (set) Token: 0x060006B9 RID: 1721 RVA: 0x000106F7 File Offset: 0x0000E8F7
		public Action<IContainer> ContentSource { get; set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x00010700 File Offset: 0x0000E900
		// (set) Token: 0x060006BB RID: 1723 RVA: 0x00010708 File Offset: 0x0000E908
		public bool IsCacheable { get; set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x00010711 File Offset: 0x0000E911
		// (set) Token: 0x060006BD RID: 1725 RVA: 0x00010719 File Offset: 0x0000E919
		internal TextStyle TextStyle { get; set; } = TextStyle.Default;

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x00010722 File Offset: 0x0000E922
		// (set) Token: 0x060006BF RID: 1727 RVA: 0x0001072A File Offset: 0x0000E92A
		public ContentDirection ContentDirection { get; set; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x00010733 File Offset: 0x0000E933
		// (set) Token: 0x060006C1 RID: 1729 RVA: 0x0001073B File Offset: 0x0000E93B
		internal int? ImageTargetDpi { get; set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x00010744 File Offset: 0x0000E944
		// (set) Token: 0x060006C3 RID: 1731 RVA: 0x0001074C File Offset: 0x0000E94C
		internal ImageCompressionQuality? ImageCompressionQuality { get; set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x00010755 File Offset: 0x0000E955
		// (set) Token: 0x060006C5 RID: 1733 RVA: 0x0001075D File Offset: 0x0000E95D
		internal bool UseOriginalImage { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x00010766 File Offset: 0x0000E966
		// (set) Token: 0x060006C7 RID: 1735 RVA: 0x0001076E File Offset: 0x0000E96E
		internal bool ClearCacheAfterFullRender { get; set; } = true;

		// Token: 0x060006C8 RID: 1736 RVA: 0x00010777 File Offset: 0x0000E977
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (this.IsRendered)
			{
				return SpacePlan.Empty();
			}
			this.PopulateContent();
			return base.Child.Measure(availableSpace);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001079C File Offset: 0x0000E99C
		internal override void Draw(Size availableSpace)
		{
			if (this.IsRendered)
			{
				return;
			}
			this.PopulateContent();
			Element child = base.Child;
			bool flag = child != null && child.Measure(availableSpace).Type == SpacePlanType.FullRender;
			Element child2 = base.Child;
			if (child2 != null)
			{
				child2.Draw(availableSpace);
			}
			if (flag && this.ClearCacheAfterFullRender)
			{
				this.IsRendered = true;
				base.Child.ReleaseDisposableChildren();
				base.Child = Empty.Instance;
			}
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001080C File Offset: 0x0000EA0C
		private void PopulateContent()
		{
			if (!(base.Child is Empty))
			{
				return;
			}
			Container container = new Container();
			base.Child = container;
			this.ContentSource.Invoke(container);
			container.ApplyInheritedAndGlobalTexStyle(this.TextStyle);
			container.ApplyContentDirection(new ContentDirection?(this.ContentDirection));
			container.ApplyDefaultImageConfiguration(this.ImageTargetDpi.Value, this.ImageCompressionQuality.Value, this.UseOriginalImage);
			container.InjectDependencies(base.PageContext, base.Canvas);
			container.VisitChildren(delegate(Element x)
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
				container.InjectSemanticTreeManager(this.SemanticTreeManager);
				container.ApplySemanticParagraphs();
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060006CB RID: 1739 RVA: 0x000108D6 File Offset: 0x0000EAD6
		// (set) Token: 0x060006CC RID: 1740 RVA: 0x000108DE File Offset: 0x0000EADE
		private bool IsRendered { get; set; }

		// Token: 0x060006CD RID: 1741 RVA: 0x000108E7 File Offset: 0x0000EAE7
		public void ResetState(bool hardReset = false)
		{
			if (hardReset)
			{
				this.IsRendered = false;
			}
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x000108F3 File Offset: 0x0000EAF3
		public object GetState()
		{
			return this.IsRendered;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00010900 File Offset: 0x0000EB00
		public void SetState(object state)
		{
			this.IsRendered = (bool)state;
		}
	}
}
