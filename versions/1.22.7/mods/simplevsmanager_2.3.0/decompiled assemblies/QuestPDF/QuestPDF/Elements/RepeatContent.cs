using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Elements.Text;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000E1 RID: 225
	internal sealed class RepeatContent : ContainerElement, IStateful, ISemanticAware
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x000119A4 File Offset: 0x0000FBA4
		// (set) Token: 0x06000751 RID: 1873 RVA: 0x000119AC File Offset: 0x0000FBAC
		[Nullable(2)]
		public SemanticTreeManager SemanticTreeManager { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x000119B5 File Offset: 0x0000FBB5
		// (set) Token: 0x06000753 RID: 1875 RVA: 0x000119BD File Offset: 0x0000FBBD
		public RepeatContent.RepeatContextType RepeatContext { get; set; } = RepeatContent.RepeatContextType.Other;

		// Token: 0x06000754 RID: 1876 RVA: 0x000119C8 File Offset: 0x0000FBC8
		internal override void Draw(Size availableSpace)
		{
			RepeatContent.<>c__DisplayClass9_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			this.OptimizeContentCacheBehavior();
			CS$<>8__locals1.childMeasurement = base.Child.Measure(availableSpace);
			if (this.SemanticTreeManager == null)
			{
				base.Draw(availableSpace);
				this.<Draw>g__ResetChildrenIfNecessary|9_0(ref CS$<>8__locals1);
				return;
			}
			if (this.IsFullyRendered)
			{
				RepeatContent.RepeatContextType repeatContext = this.RepeatContext;
				int num;
				if (repeatContext != RepeatContent.RepeatContextType.PageHeader)
				{
					if (repeatContext != RepeatContent.RepeatContextType.PageFooter)
					{
						num = -2;
					}
					else
					{
						num = -4;
					}
				}
				else
				{
					num = -3;
				}
				int semanticNodeId = num;
				base.Canvas.SetSemanticNodeId(semanticNodeId);
				this.SemanticTreeManager.BeginArtifactContent();
			}
			base.Draw(availableSpace);
			if (this.IsFullyRendered)
			{
				this.SemanticTreeManager.EndArtifactContent();
			}
			this.<Draw>g__ResetChildrenIfNecessary|9_0(ref CS$<>8__locals1);
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x00011A6E File Offset: 0x0000FC6E
		// (set) Token: 0x06000756 RID: 1878 RVA: 0x00011A76 File Offset: 0x0000FC76
		private bool IsContentOptimizationExecuted { get; set; }

		// Token: 0x06000757 RID: 1879 RVA: 0x00011A7F File Offset: 0x0000FC7F
		private void OptimizeContentCacheBehavior()
		{
			if (this.IsContentOptimizationExecuted)
			{
				return;
			}
			this.IsContentOptimizationExecuted = true;
			base.Child.VisitChildren(delegate(Element x)
			{
				TextBlock textBlock = x as TextBlock;
				if (textBlock != null)
				{
					textBlock.ClearInternalCacheAfterFullRender = false;
				}
				Lazy lazy = x as Lazy;
				if (lazy != null)
				{
					lazy.ClearCacheAfterFullRender = false;
				}
			});
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x00011ABB File Offset: 0x0000FCBB
		// (set) Token: 0x06000759 RID: 1881 RVA: 0x00011AC3 File Offset: 0x0000FCC3
		private bool IsFullyRendered { get; set; }

		// Token: 0x0600075A RID: 1882 RVA: 0x00011ACC File Offset: 0x0000FCCC
		public void ResetState(bool hardReset = false)
		{
			if (hardReset)
			{
				this.IsFullyRendered = false;
			}
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00011AD8 File Offset: 0x0000FCD8
		[NullableContext(1)]
		public object GetState()
		{
			return this.IsFullyRendered;
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00011AE5 File Offset: 0x0000FCE5
		[NullableContext(1)]
		public void SetState(object state)
		{
			this.IsFullyRendered = (bool)state;
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00011B04 File Offset: 0x0000FD04
		[CompilerGenerated]
		private void <Draw>g__ResetChildrenIfNecessary|9_0(ref RepeatContent.<>c__DisplayClass9_0 A_1)
		{
			if (A_1.childMeasurement.Type != SpacePlanType.FullRender)
			{
				return;
			}
			base.Child.VisitChildren(delegate(Element x)
			{
				IStateful stateful = x as IStateful;
				if (stateful == null)
				{
					return;
				}
				stateful.ResetState(false);
			});
			this.IsFullyRendered = true;
		}

		// Token: 0x020001C0 RID: 448
		public enum RepeatContextType
		{
			// Token: 0x0400068B RID: 1675
			PageHeader,
			// Token: 0x0400068C RID: 1676
			PageFooter,
			// Token: 0x0400068D RID: 1677
			Other
		}
	}
}
