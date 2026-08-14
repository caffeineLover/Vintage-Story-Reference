using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000D0 RID: 208
	[NullableContext(2)]
	[Nullable(0)]
	internal sealed class Hyperlink : ContainerElement, IContentDirectionAware, ISemanticAware
	{
		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000667 RID: 1639 RVA: 0x0000FA2B File Offset: 0x0000DC2B
		// (set) Token: 0x06000668 RID: 1640 RVA: 0x0000FA33 File Offset: 0x0000DC33
		public SemanticTreeManager SemanticTreeManager { get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x0000FA3C File Offset: 0x0000DC3C
		// (set) Token: 0x0600066A RID: 1642 RVA: 0x0000FA44 File Offset: 0x0000DC44
		public ContentDirection ContentDirection { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x0000FA4D File Offset: 0x0000DC4D
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x0000FA55 File Offset: 0x0000DC55
		[Nullable(1)]
		public string Url { [NullableContext(1)] get; [NullableContext(1)] set; } = "https://www.questpdf.com";

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x0000FA5E File Offset: 0x0000DC5E
		// (set) Token: 0x0600066E RID: 1646 RVA: 0x0000FA66 File Offset: 0x0000DC66
		public string Description { get; set; }

		// Token: 0x0600066F RID: 1647 RVA: 0x0000FA70 File Offset: 0x0000DC70
		internal override void Draw(Size availableSpace)
		{
			SemanticTreeManager semanticTreeManager = this.SemanticTreeManager;
			if (semanticTreeManager != null && semanticTreeManager.IsCurrentContentArtifact())
			{
				base.Draw(availableSpace);
				return;
			}
			SpacePlan spacePlan = base.Measure(availableSpace);
			SpacePlanType type = spacePlan.Type;
			bool flag = type <= SpacePlanType.Wrap;
			if (flag)
			{
				return;
			}
			Position vector = (this.ContentDirection == ContentDirection.LeftToRight) ? Position.Zero : new Position(availableSpace.Width - spacePlan.Width, 0f);
			base.Canvas.Translate(vector);
			base.Canvas.DrawHyperlink(availableSpace, this.Url, this.Description);
			base.Canvas.Translate(vector.Reverse());
			base.Draw(availableSpace);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0000FB1A File Offset: 0x0000DD1A
		internal override string GetCompanionHint()
		{
			return this.Url;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0000FB22 File Offset: 0x0000DD22
		[return: Nullable(new byte[]
		{
			2,
			0,
			1,
			1
		})]
		internal override IEnumerable<KeyValuePair<string, string>> GetCompanionProperties()
		{
			Hyperlink.<GetCompanionProperties>d__18 <GetCompanionProperties>d__ = new Hyperlink.<GetCompanionProperties>d__18(-2);
			<GetCompanionProperties>d__.<>4__this = this;
			return <GetCompanionProperties>d__;
		}
	}
}
