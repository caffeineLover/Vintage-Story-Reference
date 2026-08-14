using System;
using System.Runtime.CompilerServices;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000E2 RID: 226
	internal sealed class Rotate : ContainerElement
	{
		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x00011B51 File Offset: 0x0000FD51
		// (set) Token: 0x06000760 RID: 1888 RVA: 0x00011B59 File Offset: 0x0000FD59
		public float Angle { get; set; }

		// Token: 0x06000761 RID: 1889 RVA: 0x00011B62 File Offset: 0x0000FD62
		internal override void Draw(Size availableSpace)
		{
			base.Canvas.Rotate(this.Angle);
			Element child = base.Child;
			if (child != null)
			{
				child.Draw(availableSpace);
			}
			base.Canvas.Rotate(-this.Angle);
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x00011B9C File Offset: 0x0000FD9C
		[NullableContext(2)]
		internal override string GetCompanionHint()
		{
			if (this.Angle == 0f)
			{
				return "No rotation";
			}
			float value = Math.Abs(this.Angle);
			string text = (this.Angle > 0f) ? "clockwise" : "counter-clockwise";
			return value.FormatAsCompanionNumber() + " deg " + text;
		}
	}
}
