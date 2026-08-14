using System;
using System.Runtime.CompilerServices;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000BB RID: 187
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class DebugArea : IComponent
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x0000E35B File Offset: 0x0000C55B
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x0000E363 File Offset: 0x0000C563
		[Nullable(2)]
		public IElement Child { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x0000E36C File Offset: 0x0000C56C
		// (set) Token: 0x060005A5 RID: 1445 RVA: 0x0000E374 File Offset: 0x0000C574
		public string Text { get; set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x0000E37D File Offset: 0x0000C57D
		// (set) Token: 0x060005A7 RID: 1447 RVA: 0x0000E385 File Offset: 0x0000C585
		public Color Color { get; set; } = Colors.Red.Medium;

		// Token: 0x060005A8 RID: 1448 RVA: 0x0000E390 File Offset: 0x0000C590
		public void Compose(IContainer container)
		{
			Color backgroundColor = this.Color.WithAlpha(64);
			container.Border(1f, Unit.Point).BorderColor(this.Color).Layers(delegate(LayersDescriptor layers)
			{
				layers.PrimaryLayer().Element(this.Child);
				layers.Layer().Background(backgroundColor);
				layers.Layer().ShowIf(!string.IsNullOrWhiteSpace(this.Text)).AlignCenter().Shrink().Background(Colors.White).Padding(2f, Unit.Point).Text(this.Text).FontColor(this.Color).FontFamily(new string[]
				{
					"Consolas"
				}).FontSize(8f);
			});
		}
	}
}
