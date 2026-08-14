using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000EE RID: 238
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ShowIf : ContainerElement
	{
		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00012E58 File Offset: 0x00011058
		// (set) Token: 0x060007C3 RID: 1987 RVA: 0x00012E60 File Offset: 0x00011060
		public Predicate<ShowIfContext> VisibilityPredicate { get; set; }

		// Token: 0x060007C4 RID: 1988 RVA: 0x00012E69 File Offset: 0x00011069
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (!this.CheckVisibility())
			{
				return SpacePlan.Empty();
			}
			return base.Measure(availableSpace);
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00012E80 File Offset: 0x00011080
		internal override void Draw(Size availableSpace)
		{
			if (this.CheckVisibility())
			{
				base.Draw(availableSpace);
			}
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00012E94 File Offset: 0x00011094
		private bool CheckVisibility()
		{
			ShowIfContext showIfContext = new ShowIfContext
			{
				PageNumber = base.PageContext.CurrentPage,
				TotalPages = (base.PageContext.IsInitialRenderingPhase ? default(int?) : new int?(base.PageContext.DocumentLength))
			};
			return this.VisibilityPredicate.Invoke(showIfContext);
		}
	}
}
