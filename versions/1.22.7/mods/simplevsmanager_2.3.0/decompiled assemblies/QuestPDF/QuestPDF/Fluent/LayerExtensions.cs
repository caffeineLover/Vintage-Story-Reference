using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x02000092 RID: 146
	public static class LayerExtensions
	{
		// Token: 0x0600040A RID: 1034 RVA: 0x0000ACB0 File Offset: 0x00008EB0
		[NullableContext(1)]
		public static void Layers(this IContainer element, Action<LayersDescriptor> handler)
		{
			LayersDescriptor layersDescriptor = new LayersDescriptor();
			handler.Invoke(layersDescriptor);
			layersDescriptor.Validate();
			element.Element(layersDescriptor.Layers);
		}
	}
}
