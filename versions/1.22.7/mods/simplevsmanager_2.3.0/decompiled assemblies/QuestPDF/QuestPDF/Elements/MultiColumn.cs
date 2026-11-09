using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Drawing.DrawingCanvases;
using QuestPDF.Drawing.Proxy;
using QuestPDF.Elements.Text;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Elements
{
	// Token: 0x020000DB RID: 219
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class MultiColumn : Element, IContentDirectionAware, IDisposable
	{
		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x00010CB0 File Offset: 0x0000EEB0
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x00010CB8 File Offset: 0x0000EEB8
		internal Element Content { get; set; } = Empty.Instance;

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x00010CC1 File Offset: 0x0000EEC1
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x00010CC9 File Offset: 0x0000EEC9
		internal Element Spacer { get; set; } = Empty.Instance;

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x00010CD2 File Offset: 0x0000EED2
		// (set) Token: 0x060006F1 RID: 1777 RVA: 0x00010CDA File Offset: 0x0000EEDA
		public int ColumnCount { get; set; } = 2;

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x00010CE3 File Offset: 0x0000EEE3
		// (set) Token: 0x060006F3 RID: 1779 RVA: 0x00010CEB File Offset: 0x0000EEEB
		public bool BalanceHeight { get; set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00010CF4 File Offset: 0x0000EEF4
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x00010CFC File Offset: 0x0000EEFC
		public float Spacing { get; set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x00010D05 File Offset: 0x0000EF05
		// (set) Token: 0x060006F7 RID: 1783 RVA: 0x00010D0D File Offset: 0x0000EF0D
		public ContentDirection ContentDirection { get; set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x00010D16 File Offset: 0x0000EF16
		private ProxyDrawingCanvas ChildrenCanvas { get; } = new ProxyDrawingCanvas();

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x00010D1E File Offset: 0x0000EF1E
		// (set) Token: 0x060006FA RID: 1786 RVA: 0x00010D26 File Offset: 0x0000EF26
		private TreeNode<MultiColumnChildDrawingObserver>[] State { get; set; }

		// Token: 0x060006FB RID: 1787 RVA: 0x00010D30 File Offset: 0x0000EF30
		~MultiColumn()
		{
			this.WarnThatFinalizerIsReached<MultiColumn>();
			this.Dispose();
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x00010D64 File Offset: 0x0000EF64
		public void Dispose()
		{
			ProxyDrawingCanvas childrenCanvas = this.ChildrenCanvas;
			if (childrenCanvas != null)
			{
				childrenCanvas.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00010D7D File Offset: 0x0000EF7D
		internal override void CreateProxy([Nullable(new byte[]
		{
			1,
			2,
			2
		})] Func<Element, Element> create)
		{
			this.Content = create.Invoke(this.Content);
			this.Spacer = create.Invoke(this.Spacer);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00010DA3 File Offset: 0x0000EFA3
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		internal override IEnumerable<Element> GetChildren()
		{
			MultiColumn.<GetChildren>d__34 <GetChildren>d__ = new MultiColumn.<GetChildren>d__34(-2);
			<GetChildren>d__.<>4__this = this;
			return <GetChildren>d__;
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00010DB4 File Offset: 0x0000EFB4
		private void BuildState()
		{
			if (this.State != null)
			{
				return;
			}
			this.VisitChildren(delegate(Element child)
			{
				child.CreateProxy(delegate(Element x)
				{
					if (!(x is IStateful))
					{
						return x;
					}
					return new MultiColumnChildDrawingObserver
					{
						Child = x
					};
				});
			});
			this.State = Enumerable.ToArray<TreeNode<MultiColumnChildDrawingObserver>>(this.ExtractElementsOfType<MultiColumnChildDrawingObserver>());
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00010E00 File Offset: 0x0000F000
		internal override SpacePlan Measure(Size availableSpace)
		{
			MultiColumn.<>c__DisplayClass36_0 CS$<>8__locals1 = new MultiColumn.<>c__DisplayClass36_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.availableSpace = availableSpace;
			this.BuildState();
			this.OptimizeTextCacheBehavior();
			if (this.Content.Canvas != this.ChildrenCanvas)
			{
				this.Content.InjectDependencies(base.PageContext, this.ChildrenCanvas);
			}
			this.ChildrenCanvas.Target = new DiscardDrawingCanvas();
			return CS$<>8__locals1.<Measure>g__FindPerfectSpace|1();
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00010E6B File Offset: 0x0000F06B
		private Size GetAvailableSpaceForColumn(Size totalSpace)
		{
			return new Size((totalSpace.Width - this.Spacing * (float)(this.ColumnCount - 1)) / (float)this.ColumnCount, totalSpace.Height);
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x00010E98 File Offset: 0x0000F098
		internal override void Draw(Size availableSpace)
		{
			MultiColumn.<>c__DisplayClass38_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.availableSpace = availableSpace;
			Size availableSpaceForColumn = this.GetAvailableSpaceForColumn(CS$<>8__locals1.availableSpace);
			Size availableSpace2 = new Size(this.Spacing, CS$<>8__locals1.availableSpace.Height);
			CS$<>8__locals1.horizontalOffset = 0f;
			this.ChildrenCanvas.Target = base.Canvas;
			foreach (int num in Enumerable.Range(1, this.ColumnCount))
			{
				SpacePlan spacePlan = this.Content.Measure(availableSpaceForColumn);
				Size size = new Size(availableSpaceForColumn.Width, spacePlan.Height);
				Position vector = this.<Draw>g__GetTargetOffset|38_0(size.Width, ref CS$<>8__locals1);
				base.Canvas.Translate(vector);
				this.Content.Draw(size);
				base.Canvas.Translate(vector.Reverse());
				CS$<>8__locals1.horizontalOffset += availableSpaceForColumn.Width;
				SpacePlanType type = spacePlan.Type;
				bool flag = type == SpacePlanType.Empty || type == SpacePlanType.FullRender;
				if (flag)
				{
					break;
				}
				SpacePlan spacePlan2 = this.Spacer.Measure(availableSpace2);
				if (num != this.ColumnCount && spacePlan2.Type != SpacePlanType.Wrap)
				{
					Position vector2 = this.<Draw>g__GetTargetOffset|38_0(this.Spacing, ref CS$<>8__locals1);
					base.Canvas.Translate(vector2);
					this.Spacer.Draw(availableSpace2);
					base.Canvas.Translate(vector2.Reverse());
					CS$<>8__locals1.horizontalOffset += this.Spacing;
				}
			}
			this.ResetObserverState(false);
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00011058 File Offset: 0x0000F258
		private void ResetObserverState(bool restoreChildState)
		{
			MultiColumn.<>c__DisplayClass39_0 CS$<>8__locals1;
			CS$<>8__locals1.restoreChildState = restoreChildState;
			TreeNode<MultiColumnChildDrawingObserver>[] state = this.State;
			for (int i = 0; i < state.Length; i++)
			{
				MultiColumn.<ResetObserverState>g__Traverse|39_0(state[i], ref CS$<>8__locals1);
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x0001108C File Offset: 0x0000F28C
		// (set) Token: 0x06000705 RID: 1797 RVA: 0x00011094 File Offset: 0x0000F294
		private bool IsTextOptimizationExecuted { get; set; }

		// Token: 0x06000706 RID: 1798 RVA: 0x0001109D File Offset: 0x0000F29D
		private void OptimizeTextCacheBehavior()
		{
			if (this.IsTextOptimizationExecuted)
			{
				return;
			}
			this.IsTextOptimizationExecuted = true;
			this.Content.VisitChildren(delegate(Element x)
			{
				TextBlock textBlock = x as TextBlock;
				if (textBlock != null)
				{
					textBlock.ClearInternalCacheAfterFullRender = false;
				}
			});
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x00011109 File Offset: 0x0000F309
		[CompilerGenerated]
		private Position <Draw>g__GetTargetOffset|38_0(float contentWidth, ref MultiColumn.<>c__DisplayClass38_0 A_2)
		{
			if (this.ContentDirection != ContentDirection.LeftToRight)
			{
				return new Position(A_2.availableSpace.Width - A_2.horizontalOffset - contentWidth, 0f);
			}
			return new Position(A_2.horizontalOffset, 0f);
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00011144 File Offset: 0x0000F344
		[CompilerGenerated]
		internal static void <ResetObserverState>g__Traverse|39_0(TreeNode<MultiColumnChildDrawingObserver> node, ref MultiColumn.<>c__DisplayClass39_0 A_1)
		{
			MultiColumnChildDrawingObserver value = node.Value;
			if (!value.HasBeenDrawn)
			{
				return;
			}
			if (A_1.restoreChildState)
			{
				value.RestoreState();
			}
			value.ResetDrawingState();
			foreach (TreeNode<MultiColumnChildDrawingObserver> node2 in node.Children)
			{
				MultiColumn.<ResetObserverState>g__Traverse|39_0(node2, ref A_1);
			}
		}
	}
}
