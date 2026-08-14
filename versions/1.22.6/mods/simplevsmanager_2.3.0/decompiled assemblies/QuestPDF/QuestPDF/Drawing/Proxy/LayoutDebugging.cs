using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using QuestPDF.Elements;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Drawing.Proxy
{
	// Token: 0x02000115 RID: 277
	[NullableContext(1)]
	[Nullable(0)]
	internal static class LayoutDebugging
	{
		// Token: 0x0600098B RID: 2443 RVA: 0x00018AD8 File Offset: 0x00016CD8
		internal static SpacePlan TryMeasureWithOverflow(this Element element, Size availableSpace)
		{
			LayoutDebugging.<>c__DisplayClass0_0 CS$<>8__locals1;
			CS$<>8__locals1.element = element;
			CS$<>8__locals1.availableSpace = availableSpace;
			SpacePlan? spacePlan = LayoutDebugging.<TryMeasureWithOverflow>g__TryVerticalOverflow|0_1(ref CS$<>8__locals1);
			if (spacePlan != null)
			{
				return spacePlan.GetValueOrDefault();
			}
			SpacePlan? spacePlan2 = LayoutDebugging.<TryMeasureWithOverflow>g__TryHorizontalOverflow|0_2(ref CS$<>8__locals1);
			if (spacePlan2 != null)
			{
				return spacePlan2.GetValueOrDefault();
			}
			SpacePlan? spacePlan3 = LayoutDebugging.<TryMeasureWithOverflow>g__TryUnconstrainedOverflow|0_3(ref CS$<>8__locals1);
			if (spacePlan3 == null)
			{
				return SpacePlan.Wrap("Extending the available space does not allow the child to fit on the page.");
			}
			return spacePlan3.GetValueOrDefault();
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00018B4A File Offset: 0x00016D4A
		public static void ApplyLayoutOverflowDetection(this Element container)
		{
			container.VisitChildren(delegate(Element x)
			{
				x.CreateProxy(delegate(Element y)
				{
					if (!(y is ElementProxy))
					{
						return new OverflowDebuggingProxy(y);
					}
					return y;
				});
			});
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x00018B71 File Offset: 0x00016D71
		public static void TryToFixTheLayoutOverflowIssue(this TreeNode<OverflowDebuggingProxy> hierarchyRoot)
		{
			LayoutDebugging.<TryToFixTheLayoutOverflowIssue>g__Traverse|2_0(hierarchyRoot);
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x00018B79 File Offset: 0x00016D79
		public static void RemoveExistingProxies(this Element content)
		{
			content.RemoveExistingProxiesOfType<ElementProxy>();
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00018B81 File Offset: 0x00016D81
		public static void RemoveExistingProxiesOfType<[Nullable(0)] TProxy>(this Element content) where TProxy : ElementProxy
		{
			Action<Element> handler;
			if ((handler = LayoutDebugging.<>c__4<TProxy>.<>9__4_0) == null)
			{
				handler = (LayoutDebugging.<>c__4<TProxy>.<>9__4_0 = new Action<Element>(LayoutDebugging.<>c__4<TProxy>.<>9.<RemoveExistingProxiesOfType>b__4_0));
			}
			content.VisitChildren(handler);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00018BA8 File Offset: 0x00016DA8
		public static void StopMeasuring(this TreeNode<OverflowDebuggingProxy> parent)
		{
			parent.Value.StopMeasuring();
			foreach (TreeNode<OverflowDebuggingProxy> parent2 in parent.Children)
			{
				parent2.StopMeasuring();
			}
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00018C00 File Offset: 0x00016E00
		public static IEnumerable<TreeNode<OverflowDebuggingProxy>> FindLayoutOverflowVisualizationNodes(this TreeNode<OverflowDebuggingProxy> rootNode)
		{
			LayoutDebugging.<>c__DisplayClass6_0 CS$<>8__locals1;
			CS$<>8__locals1.result = new List<TreeNode<OverflowDebuggingProxy>>();
			LayoutDebugging.<FindLayoutOverflowVisualizationNodes>g__Traverse|6_0(rootNode, ref CS$<>8__locals1);
			return CS$<>8__locals1.result;
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00018C28 File Offset: 0x00016E28
		public static string FormatAncestors(this IEnumerable<Element> ancestors)
		{
			LayoutDebugging.<>c__DisplayClass7_0 CS$<>8__locals1;
			CS$<>8__locals1.result = new StringBuilder();
			foreach (Element node in ancestors)
			{
				LayoutDebugging.<FormatAncestors>g__Format|7_0(node, ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.result.ToString();
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00018C88 File Offset: 0x00016E88
		public static string FormatLayoutSubtree(this TreeNode<OverflowDebuggingProxy> root)
		{
			LayoutDebugging.<>c__DisplayClass8_0 CS$<>8__locals1;
			CS$<>8__locals1.indentationCache = Enumerable.ToArray<string>(Enumerable.Select<int, string>(Enumerable.Select<int, int>(Enumerable.Range(0, 128), (int x) => x * 3), (int x) => new string(' ', x)));
			CS$<>8__locals1.indentationLevel = 0;
			CS$<>8__locals1.result = new StringBuilder();
			LayoutDebugging.<FormatLayoutSubtree>g__Traverse|8_2(root, ref CS$<>8__locals1);
			return CS$<>8__locals1.result.ToString();
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x00018D1C File Offset: 0x00016F1C
		[return: TupleElementNames(new string[]
		{
			"Property",
			"Value"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		public static IEnumerable<ValueTuple<string, string>> GetElementConfiguration(this IElement element)
		{
			return Enumerable.Select(Enumerable.Where(Enumerable.Where(Enumerable.Where(Enumerable.Select(element.GetType().GetProperties(), (PropertyInfo x) => new <>f__AnonymousType1<string, object>(x.Name.PrettifyName(), x.GetValue(element))), x => !(x.Value is IElement)), x => x.Value is string || !(x.Value is IEnumerable)), x => !(x.Value is TextStyle)), delegate(x)
			{
				string property = x.Property;
				object value = x.Value;
				return new ValueTuple<string, string>(property, ((value != null) ? value.ToString() : null) ?? "-");
			});
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x00018DE8 File Offset: 0x00016FE8
		[CompilerGenerated]
		internal static SpacePlan? <TryMeasureWithOverflow>g__TryOverflow|0_0(Size targetSpace, ref LayoutDebugging.<>c__DisplayClass0_0 A_1)
		{
			SpacePlan spacePlan = A_1.element.Measure(targetSpace);
			if (spacePlan.Type != SpacePlanType.Wrap)
			{
				return new SpacePlan?(spacePlan);
			}
			return default(SpacePlan?);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00018E1B File Offset: 0x0001701B
		[CompilerGenerated]
		internal static SpacePlan? <TryMeasureWithOverflow>g__TryVerticalOverflow|0_1(ref LayoutDebugging.<>c__DisplayClass0_0 A_0)
		{
			return LayoutDebugging.<TryMeasureWithOverflow>g__TryOverflow|0_0(new Size(A_0.availableSpace.Width, 14400f), ref A_0);
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x00018E38 File Offset: 0x00017038
		[CompilerGenerated]
		internal static SpacePlan? <TryMeasureWithOverflow>g__TryHorizontalOverflow|0_2(ref LayoutDebugging.<>c__DisplayClass0_0 A_0)
		{
			return LayoutDebugging.<TryMeasureWithOverflow>g__TryOverflow|0_0(new Size(14400f, A_0.availableSpace.Height), ref A_0);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00018E55 File Offset: 0x00017055
		[CompilerGenerated]
		internal static SpacePlan? <TryMeasureWithOverflow>g__TryUnconstrainedOverflow|0_3(ref LayoutDebugging.<>c__DisplayClass0_0 A_0)
		{
			return LayoutDebugging.<TryMeasureWithOverflow>g__TryOverflow|0_0(new Size(14400f, 14400f), ref A_0);
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00018E6C File Offset: 0x0001706C
		[CompilerGenerated]
		internal static void <TryToFixTheLayoutOverflowIssue>g__Traverse|2_0(TreeNode<OverflowDebuggingProxy> element)
		{
			LayoutDebugging.<>c__DisplayClass2_0 CS$<>8__locals1;
			CS$<>8__locals1.element = element;
			Element child = CS$<>8__locals1.element.Value.Child;
			bool flag = child is DebugPointer || child is SourceCodePointer || child is Container;
			if (flag)
			{
				LayoutDebugging.<TryToFixTheLayoutOverflowIssue>g__Traverse|2_0(Enumerable.First<TreeNode<OverflowDebuggingProxy>>(CS$<>8__locals1.element.Children));
				return;
			}
			if (CS$<>8__locals1.element.Value.AvailableSpace == null)
			{
				return;
			}
			SpacePlan? spacePlan = CS$<>8__locals1.element.Value.SpacePlan;
			if (spacePlan == null)
			{
				return;
			}
			spacePlan = CS$<>8__locals1.element.Value.SpacePlan;
			if ((((spacePlan != null) ? new SpacePlanType?(spacePlan.GetValueOrDefault().Type) : default(SpacePlanType?)) ?? SpacePlanType.Wrap) == SpacePlanType.Empty)
			{
				return;
			}
			spacePlan = CS$<>8__locals1.element.Value.SpacePlan;
			if (!(((spacePlan != null) ? new SpacePlanType?(spacePlan.GetValueOrDefault().Type) : default(SpacePlanType?)) != SpacePlanType.FullRender))
			{
				return;
			}
			spacePlan = CS$<>8__locals1.element.Value.SpacePlan;
			if (!(((spacePlan != null) ? new SpacePlanType?(spacePlan.GetValueOrDefault().Type) : default(SpacePlanType?)) != SpacePlanType.PartialRender))
			{
				foreach (TreeNode<OverflowDebuggingProxy> element2 in CS$<>8__locals1.element.Children)
				{
					LayoutDebugging.<TryToFixTheLayoutOverflowIssue>g__Traverse|2_0(element2);
				}
				return;
			}
			flag = Enumerable.All<TreeNode<OverflowDebuggingProxy>>(CS$<>8__locals1.element.Children, delegate(TreeNode<OverflowDebuggingProxy> x)
			{
				SpacePlan? spacePlan2;
				return !(((x.Value.SpacePlan != null) ? new SpacePlanType?(spacePlan2.GetValueOrDefault().Type) : default(SpacePlanType?)) == SpacePlanType.Wrap);
			});
			if (flag)
			{
				SpacePlanType spacePlanType = LayoutDebugging.<TryToFixTheLayoutOverflowIssue>g__MeasureElementWithExtendedSpace|2_3(ref CS$<>8__locals1);
				bool flag2 = spacePlanType - SpacePlanType.PartialRender <= 1;
				flag = flag2;
			}
			if (flag)
			{
				CS$<>8__locals1.element.Value.CreateProxy((Element x) => new LayoutOverflowVisualization
				{
					Child = x
				});
				return;
			}
			foreach (TreeNode<OverflowDebuggingProxy> element3 in Enumerable.ToList<TreeNode<OverflowDebuggingProxy>>(Enumerable.Where<TreeNode<OverflowDebuggingProxy>>(CS$<>8__locals1.element.Children, delegate(TreeNode<OverflowDebuggingProxy> x)
			{
				SpacePlan? spacePlan2;
				return ((x.Value.SpacePlan != null) ? new SpacePlanType?(spacePlan2.GetValueOrDefault().Type) : default(SpacePlanType?)) == SpacePlanType.Wrap;
			})))
			{
				LayoutDebugging.<TryToFixTheLayoutOverflowIssue>g__Traverse|2_0(element3);
			}
			if (LayoutDebugging.<TryToFixTheLayoutOverflowIssue>g__MeasureElementWithExtendedSpace|2_3(ref CS$<>8__locals1) != SpacePlanType.Wrap)
			{
				return;
			}
			foreach (TreeNode<OverflowDebuggingProxy> element4 in Enumerable.ToList<TreeNode<OverflowDebuggingProxy>>(Enumerable.Where<TreeNode<OverflowDebuggingProxy>>(CS$<>8__locals1.element.Children, delegate(TreeNode<OverflowDebuggingProxy> x)
			{
				SpacePlan? spacePlan2;
				return ((x.Value.SpacePlan != null) ? new SpacePlanType?(spacePlan2.GetValueOrDefault().Type) : default(SpacePlanType?)) == SpacePlanType.PartialRender;
			})))
			{
				LayoutDebugging.<TryToFixTheLayoutOverflowIssue>g__Traverse|2_0(element4);
			}
			if (LayoutDebugging.<TryToFixTheLayoutOverflowIssue>g__MeasureElementWithExtendedSpace|2_3(ref CS$<>8__locals1) != SpacePlanType.Wrap)
			{
				return;
			}
			CS$<>8__locals1.element.Value.CreateProxy((Element x) => new LayoutOverflowVisualization
			{
				Child = x
			});
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x000191D4 File Offset: 0x000173D4
		[CompilerGenerated]
		internal static SpacePlanType <TryToFixTheLayoutOverflowIssue>g__MeasureElementWithExtendedSpace|2_3(ref LayoutDebugging.<>c__DisplayClass2_0 A_0)
		{
			return A_0.element.Value.TryMeasureWithOverflow(A_0.element.Value.AvailableSpace.Value).Type;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x00019210 File Offset: 0x00017410
		[CompilerGenerated]
		internal static void <FindLayoutOverflowVisualizationNodes>g__Traverse|6_0(TreeNode<OverflowDebuggingProxy> node, ref LayoutDebugging.<>c__DisplayClass6_0 A_1)
		{
			if (node.Value.Child is LayoutOverflowVisualization)
			{
				A_1.result.Add(node);
			}
			foreach (TreeNode<OverflowDebuggingProxy> node2 in node.Children)
			{
				LayoutDebugging.<FindLayoutOverflowVisualizationNodes>g__Traverse|6_0(node2, ref A_1);
			}
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0001927C File Offset: 0x0001747C
		[CompilerGenerated]
		internal static void <FormatAncestors>g__Format|7_0(Element node, ref LayoutDebugging.<>c__DisplayClass7_0 A_1)
		{
			DebugPointer debugPointer = node as DebugPointer;
			if (debugPointer != null)
			{
				StringBuilder result = A_1.result;
				StringBuilder stringBuilder = result;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
				appendInterpolatedStringHandler..ctor(3, 1, result);
				appendInterpolatedStringHandler.AppendLiteral("-> ");
				appendInterpolatedStringHandler.AppendFormatted(debugPointer.Label);
				stringBuilder.AppendLine(ref appendInterpolatedStringHandler);
			}
			else
			{
				SourceCodePointer sourceCodePointer = node as SourceCodePointer;
				if (sourceCodePointer != null)
				{
					StringBuilder result = A_1.result;
					StringBuilder stringBuilder2 = result;
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
					appendInterpolatedStringHandler..ctor(16, 1, result);
					appendInterpolatedStringHandler.AppendLiteral("-> In method:   ");
					appendInterpolatedStringHandler.AppendFormatted(sourceCodePointer.MethodName);
					stringBuilder2.AppendLine(ref appendInterpolatedStringHandler);
					result = A_1.result;
					StringBuilder stringBuilder3 = result;
					appendInterpolatedStringHandler..ctor(16, 1, result);
					appendInterpolatedStringHandler.AppendLiteral("   Called from: ");
					appendInterpolatedStringHandler.AppendFormatted(sourceCodePointer.CalledFrom);
					stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
					result = A_1.result;
					StringBuilder stringBuilder4 = result;
					appendInterpolatedStringHandler..ctor(16, 1, result);
					appendInterpolatedStringHandler.AppendLiteral("   Source path: ");
					appendInterpolatedStringHandler.AppendFormatted(sourceCodePointer.FilePath);
					stringBuilder4.AppendLine(ref appendInterpolatedStringHandler);
					result = A_1.result;
					StringBuilder stringBuilder5 = result;
					appendInterpolatedStringHandler..ctor(16, 1, result);
					appendInterpolatedStringHandler.AppendLiteral("   Line number: ");
					appendInterpolatedStringHandler.AppendFormatted<int>(sourceCodePointer.LineNumber);
					stringBuilder5.AppendLine(ref appendInterpolatedStringHandler);
				}
			}
			A_1.result.AppendLine();
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x000193B4 File Offset: 0x000175B4
		[CompilerGenerated]
		internal static void <FormatLayoutSubtree>g__Traverse|8_2(TreeNode<OverflowDebuggingProxy> parent, ref LayoutDebugging.<>c__DisplayClass8_0 A_1)
		{
			OverflowDebuggingProxy value = parent.Value;
			if (value.Child is Container)
			{
				LayoutDebugging.<FormatLayoutSubtree>g__Traverse|8_2(Enumerable.First<TreeNode<OverflowDebuggingProxy>>(parent.Children), ref A_1);
				return;
			}
			string text = A_1.indentationCache[A_1.indentationLevel];
			foreach (string text2 in LayoutDebugging.<FormatLayoutSubtree>g__Format|8_3(value))
			{
				StringBuilder result = A_1.result;
				StringBuilder stringBuilder = result;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(0, 2, result);
				appendInterpolatedStringHandler.AppendFormatted(text);
				appendInterpolatedStringHandler.AppendFormatted(text2);
				stringBuilder.AppendLine(ref appendInterpolatedStringHandler);
			}
			A_1.result.AppendLine();
			A_1.result.AppendLine();
			if (value.AvailableSpace == null || value.SpacePlan == null)
			{
				return;
			}
			int indentationLevel = A_1.indentationLevel;
			A_1.indentationLevel = indentationLevel + 1;
			foreach (TreeNode<OverflowDebuggingProxy> parent2 in parent.Children)
			{
				LayoutDebugging.<FormatLayoutSubtree>g__Traverse|8_2(parent2, ref A_1);
			}
			indentationLevel = A_1.indentationLevel;
			A_1.indentationLevel = indentationLevel - 1;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x000194FC File Offset: 0x000176FC
		[CompilerGenerated]
		internal static IEnumerable<string> <FormatLayoutSubtree>g__Format|8_3(OverflowDebuggingProxy proxy)
		{
			LayoutDebugging.<<FormatLayoutSubtree>g__Format|8_3>d <<FormatLayoutSubtree>g__Format|8_3>d = new LayoutDebugging.<<FormatLayoutSubtree>g__Format|8_3>d(-2);
			<<FormatLayoutSubtree>g__Format|8_3>d.<>3__proxy = proxy;
			return <<FormatLayoutSubtree>g__Format|8_3>d;
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0001950C File Offset: 0x0001770C
		[CompilerGenerated]
		internal static string <FormatLayoutSubtree>g__GetTitle|8_4(ref LayoutDebugging.<>c__DisplayClass8_1 A_0)
		{
			string name = A_0.child.GetType().Name;
			if (A_0.proxy.Child is LayoutOverflowVisualization)
			{
				return "\ud83d\udea8 " + name + " \ud83d\udea8";
			}
			SpacePlan? spacePlan;
			SpacePlanType? spacePlanType = (A_0.proxy.SpacePlan != null) ? new SpacePlanType?(spacePlan.GetValueOrDefault().Type) : default(SpacePlanType?);
			string text;
			if (spacePlanType != null)
			{
				switch (spacePlanType.GetValueOrDefault())
				{
				case SpacePlanType.Empty:
					text = "\ud83d\udfe2";
					goto IL_B7;
				case SpacePlanType.Wrap:
					text = "\ud83d\udd34";
					goto IL_B7;
				case SpacePlanType.PartialRender:
					text = "\ud83d\udfe1";
					goto IL_B7;
				case SpacePlanType.FullRender:
					text = "\ud83d\udfe2";
					goto IL_B7;
				}
			}
			text = "⚪️";
			IL_B7:
			return text + " " + name;
		}

		// Token: 0x040003E7 RID: 999
		public const string LayoutVisualizationLegend = "Legend: \n\ud83d\udea8 - Element that is likely the root cause of the layout issue based on library heuristics and prediction. \n\ud83d\udd34 - Element that cannot be drawn due to the provided layout constraints. This element likely causes the layout issue, or one of its descendant children is responsible for the problem. \n\ud83d\udfe1 - Element that can be partially drawn on the page and will also be rendered on the consecutive page. In more complex layouts, this element may also cause issues or contain a child that is the actual root cause.\n\ud83d\udfe2 - Element that is successfully and completely drawn on the page.\n⚪️ - Element that has not been drawn on the faulty page. Its children are omitted.\n";
	}
}
