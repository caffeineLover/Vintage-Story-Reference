using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000B4 RID: 180
	[NullableContext(2)]
	[Nullable(0)]
	internal class ArtifactTag : ContainerElement, ISemanticAware
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x0000D92F File Offset: 0x0000BB2F
		// (set) Token: 0x06000566 RID: 1382 RVA: 0x0000D937 File Offset: 0x0000BB37
		public SemanticTreeManager SemanticTreeManager { get; set; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x0000D940 File Offset: 0x0000BB40
		// (set) Token: 0x06000568 RID: 1384 RVA: 0x0000D948 File Offset: 0x0000BB48
		public int Id { get; set; }

		// Token: 0x06000569 RID: 1385 RVA: 0x0000D954 File Offset: 0x0000BB54
		internal override void Draw(Size availableSpace)
		{
			if (this.SemanticTreeManager == null)
			{
				base.Draw(availableSpace);
				return;
			}
			base.Canvas.SetSemanticNodeId(this.Id);
			this.SemanticTreeManager.BeginArtifactContent();
			Element child = base.Child;
			if (child != null)
			{
				child.Draw(availableSpace);
			}
			this.SemanticTreeManager.EndArtifactContent();
		}
	}
}
