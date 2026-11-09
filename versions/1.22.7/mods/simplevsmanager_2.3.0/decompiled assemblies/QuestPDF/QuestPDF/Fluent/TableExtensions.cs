using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x020000A8 RID: 168
	public static class TableExtensions
	{
		// Token: 0x060004C3 RID: 1219 RVA: 0x0000C4F8 File Offset: 0x0000A6F8
		[NullableContext(1)]
		public static void Table(this IContainer element, Action<TableDescriptor> handler)
		{
			TableDescriptor tableDescriptor = new TableDescriptor();
			TableDescriptor tableDescriptor2 = tableDescriptor;
			SemanticTag semanticTag = element as SemanticTag;
			tableDescriptor2.EnableAutomatedSemanticTagging = (semanticTag != null && semanticTag.TagType == "Table");
			handler.Invoke(tableDescriptor);
			element.Element(tableDescriptor.CreateElement());
		}
	}
}
