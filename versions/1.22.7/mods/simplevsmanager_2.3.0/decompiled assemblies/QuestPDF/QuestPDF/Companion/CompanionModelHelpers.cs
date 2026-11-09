using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using QuestPDF.Drawing.Proxy;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Companion
{
	// Token: 0x02000130 RID: 304
	[NullableContext(1)]
	[Nullable(0)]
	internal static class CompanionModelHelpers
	{
		// Token: 0x06000AC4 RID: 2756 RVA: 0x0001BDC9 File Offset: 0x00019FC9
		internal static CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement ExtractHierarchy(this Element container)
		{
			return CompanionModelHelpers.<ExtractHierarchy>g__Traverse|0_0(Enumerable.Single<TreeNode<LayoutProxy>>(container.ExtractElementsOfType<LayoutProxy>()));
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x0001BDDB File Offset: 0x00019FDB
		[NullableContext(2)]
		private static CompanionCommands.UpdateDocumentStructure.SourceCodePath GetSourceCodePath(SourceCodePath? path)
		{
			if (path == null)
			{
				return null;
			}
			return new CompanionCommands.UpdateDocumentStructure.SourceCodePath
			{
				FilePath = path.Value.FilePath,
				LineNumber = path.Value.LineNumber
			};
		}

		// Token: 0x06000AC6 RID: 2758 RVA: 0x0001BE14 File Offset: 0x0001A014
		internal static CompanionCommands.ShowGenericException.StackFrame[] ParseStackTrace(this string stackTrace)
		{
			string[] array = stackTrace.Split(new string[]
			{
				Environment.NewLine
			}, 1);
			List<CompanionCommands.ShowGenericException.StackFrame> list = new List<CompanionCommands.ShowGenericException.StackFrame>();
			foreach (string text in array)
			{
				Match match = Regex.Match(text, "at\\s+(?<codeLocation>.+)\\s+in\\s(?<fileName>.+)\\s*:line\\s(?<lineNumber>\\d+)");
				Match match2 = Regex.Match(text, "at\\s+(?<codeLocation>.+)");
				if (match.Success)
				{
					list.Add(new CompanionCommands.ShowGenericException.StackFrame
					{
						CodeLocation = match.Groups["codeLocation"].Value,
						FileName = match.Groups["fileName"].Value,
						LineNumber = new int?(int.Parse(match.Groups["lineNumber"].Value))
					});
				}
				else if (match2.Success)
				{
					list.Add(new CompanionCommands.ShowGenericException.StackFrame
					{
						CodeLocation = match2.Groups["codeLocation"].Value
					});
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000AC7 RID: 2759 RVA: 0x0001BF14 File Offset: 0x0001A114
		internal static CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement ImproveHierarchyStructure(this CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement root)
		{
			CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement documentHierarchyElement = Enumerable.Single<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>(CompanionModelHelpers.<ImproveHierarchyStructure>g__FindDocumentStructurePointersThat|3_2(root, (DocumentStructureTypes x) => x == DocumentStructureTypes.Document));
			documentHierarchyElement.IsSingleChildContainer = false;
			List<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement> list = Enumerable.ToList<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>(CompanionModelHelpers.<ImproveHierarchyStructure>g__FindDocumentStructurePointersThat|3_2(documentHierarchyElement, (DocumentStructureTypes x) => x == DocumentStructureTypes.Page));
			foreach (CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement documentHierarchyElement2 in list)
			{
				documentHierarchyElement2.IsSingleChildContainer = false;
				documentHierarchyElement2.Children = Enumerable.ToList<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>(CompanionModelHelpers.<ImproveHierarchyStructure>g__FindDocumentStructurePointersThat|3_2(documentHierarchyElement2, delegate(DocumentStructureTypes x)
				{
					bool flag = x <= DocumentStructureTypes.Page;
					return !flag;
				}));
			}
			documentHierarchyElement.Children = list;
			if (list.Count == 1)
			{
				documentHierarchyElement.Children = Enumerable.Single<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>(list).Children;
			}
			return documentHierarchyElement;
		}

		// Token: 0x06000AC8 RID: 2760 RVA: 0x0001C010 File Offset: 0x0001A210
		[CompilerGenerated]
		internal unsafe static CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement <ExtractHierarchy>g__Traverse|0_0(TreeNode<LayoutProxy> node)
		{
			Element child = node.Value.Child;
			for (;;)
			{
				ElementProxy elementProxy = child as ElementProxy;
				if (elementProxy == null)
				{
					break;
				}
				child = elementProxy.Child;
			}
			if (child is Container)
			{
				return CompanionModelHelpers.<ExtractHierarchy>g__Traverse|0_0(Enumerable.Single<TreeNode<LayoutProxy>>(node.Children));
			}
			CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement hierarchyElement = CompanionModelHelpers.<ExtractHierarchy>g__MapToHierarchyElement|0_1(node);
			StyledBox styledBox = child as StyledBox;
			if (styledBox == null)
			{
				return hierarchyElement;
			}
			CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement[] array = Enumerable.ToArray<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>(Enumerable.Select<ValueTuple<string, string>, CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>(styledBox.GetCompanionCustomContent(), delegate([TupleElementNames(new string[]
			{
				"Type",
				"Hint"
			})] ValueTuple<string, string> x)
			{
				CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement documentHierarchyElement2 = hierarchyElement.<Clone>$();
				documentHierarchyElement2.ElementType = x.Item1;
				documentHierarchyElement2.Hint = x.Item2;
				return documentHierarchyElement2;
			}));
			if (array.Length == 0)
			{
				return hierarchyElement;
			}
			for (int i = 0; i <= array.Length - 2; i++)
			{
				CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement documentHierarchyElement = array[i];
				int num = 1;
				List<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement> list = new List<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>(num);
				CollectionsMarshal.SetCount<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>(list, num);
				Span<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement> span = CollectionsMarshal.AsSpan<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>(list);
				int num2 = 0;
				*span[num2] = array[i + 1];
				documentHierarchyElement.Children = list;
			}
			return Enumerable.First<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>(array);
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x0001C0F8 File Offset: 0x0001A2F8
		[CompilerGenerated]
		internal static CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement <ExtractHierarchy>g__MapToHierarchyElement|0_1(TreeNode<LayoutProxy> node)
		{
			LayoutProxy value = node.Value;
			Element child = value.Child;
			CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement documentHierarchyElement = new CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement();
			documentHierarchyElement.Element = child;
			documentHierarchyElement.ElementType = child.GetType().Name;
			documentHierarchyElement.Hint = child.GetCompanionHint();
			documentHierarchyElement.SearchableContent = child.GetCompanionSearchableContent();
			documentHierarchyElement.PageLocations = value.Snapshots;
			documentHierarchyElement.SourceCodeDeclarationPath = CompanionModelHelpers.GetSourceCodePath(child.CodeLocation);
			documentHierarchyElement.LayoutErrorMeasurements = value.LayoutErrorMeasurements;
			documentHierarchyElement.IsSingleChildContainer = (child is ContainerElement);
			IEnumerable<KeyValuePair<string, string>> companionProperties = child.GetCompanionProperties();
			ICollection<CompanionCommands.ElementProperty> collection;
			if (companionProperties == null)
			{
				collection = null;
			}
			else
			{
				collection = Enumerable.ToList<CompanionCommands.ElementProperty>(Enumerable.Select<KeyValuePair<string, string>, CompanionCommands.ElementProperty>(companionProperties, (KeyValuePair<string, string> x) => new CompanionCommands.ElementProperty
				{
					Label = x.Key,
					Value = x.Value
				}));
			}
			documentHierarchyElement.Properties = (collection ?? new List<CompanionCommands.ElementProperty>());
			documentHierarchyElement.Children = Enumerable.ToList<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>(Enumerable.Select<TreeNode<LayoutProxy>, CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>(node.Children, new Func<TreeNode<LayoutProxy>, CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>(CompanionModelHelpers.<ExtractHierarchy>g__Traverse|0_0)));
			return documentHierarchyElement;
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x0001C1E8 File Offset: 0x0001A3E8
		[CompilerGenerated]
		internal static ICollection<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement> <ImproveHierarchyStructure>g__FindDocumentStructurePointersThat|3_2(CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement root, Predicate<DocumentStructureTypes> predicate)
		{
			CompanionModelHelpers.<>c__DisplayClass3_0 CS$<>8__locals1;
			CS$<>8__locals1.predicate = predicate;
			CS$<>8__locals1.result = new List<CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement>();
			CompanionModelHelpers.<ImproveHierarchyStructure>g__Traverse|3_4(root, ref CS$<>8__locals1);
			return CS$<>8__locals1.result;
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x0001C218 File Offset: 0x0001A418
		[CompilerGenerated]
		internal static void <ImproveHierarchyStructure>g__Traverse|3_4(CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement element, ref CompanionModelHelpers.<>c__DisplayClass3_0 A_1)
		{
			DebugPointer debugPointer = element.Element as DebugPointer;
			DocumentStructureTypes documentStructureTypes;
			if (debugPointer != null && debugPointer.Type == DebugPointerType.DocumentStructure && Enum.TryParse<DocumentStructureTypes>(debugPointer.Label, ref documentStructureTypes) && A_1.predicate.Invoke(documentStructureTypes))
			{
				A_1.result.Add(element);
				return;
			}
			foreach (CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement element2 in element.Children)
			{
				CompanionModelHelpers.<ImproveHierarchyStructure>g__Traverse|3_4(element2, ref A_1);
			}
		}
	}
}
