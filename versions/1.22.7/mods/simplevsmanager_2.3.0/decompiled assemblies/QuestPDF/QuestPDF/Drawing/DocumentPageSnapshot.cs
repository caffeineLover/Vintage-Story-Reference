using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Skia;

namespace QuestPDF.Drawing
{
	// Token: 0x0200010E RID: 270
	[NullableContext(1)]
	[Nullable(0)]
	internal class DocumentPageSnapshot : IDisposable
	{
		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x0001838C File Offset: 0x0001658C
		// (set) Token: 0x06000953 RID: 2387 RVA: 0x00018394 File Offset: 0x00016594
		public List<DocumentPageSnapshot.LayerSnapshot> Layers { get; set; }

		// Token: 0x06000954 RID: 2388 RVA: 0x000183A0 File Offset: 0x000165A0
		~DocumentPageSnapshot()
		{
			this.WarnThatFinalizerIsReached<DocumentPageSnapshot>();
			this.Dispose();
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x000183D4 File Offset: 0x000165D4
		public void Dispose()
		{
			foreach (DocumentPageSnapshot.LayerSnapshot layerSnapshot in this.Layers)
			{
				layerSnapshot.Picture.Dispose();
			}
			this.Layers.Clear();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0001843C File Offset: 0x0001663C
		public void DrawOnSkCanvas(SkCanvas canvas)
		{
			foreach (DocumentPageSnapshot.LayerSnapshot layerSnapshot in Enumerable.OrderBy<DocumentPageSnapshot.LayerSnapshot, int>(this.Layers, (DocumentPageSnapshot.LayerSnapshot x) => x.ZIndex))
			{
				canvas.DrawPicture(layerSnapshot.Picture);
			}
		}

		// Token: 0x020001EE RID: 494
		[Nullable(0)]
		public class LayerSnapshot
		{
			// Token: 0x170002EE RID: 750
			// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x00020DC0 File Offset: 0x0001EFC0
			// (set) Token: 0x06000DA7 RID: 3495 RVA: 0x00020DC8 File Offset: 0x0001EFC8
			public int ZIndex { get; set; }

			// Token: 0x170002EF RID: 751
			// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x00020DD1 File Offset: 0x0001EFD1
			// (set) Token: 0x06000DA9 RID: 3497 RVA: 0x00020DD9 File Offset: 0x0001EFD9
			public SkPicture Picture { get; set; }
		}
	}
}
