using System;
using System.Runtime.CompilerServices;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000DF RID: 223
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Placeholder : IComponent
	{
		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000743 RID: 1859 RVA: 0x00011816 File Offset: 0x0000FA16
		// (set) Token: 0x06000744 RID: 1860 RVA: 0x0001181E File Offset: 0x0000FA1E
		public string Text { get; set; }

		// Token: 0x06000745 RID: 1861 RVA: 0x00011828 File Offset: 0x0000FA28
		public void Compose(IContainer container)
		{
			container.Background(Colors.Grey.Lighten2).Padding(5f, Unit.Point).AlignMiddle().AlignCenter().Element(delegate(IContainer x)
			{
				if (string.IsNullOrWhiteSpace(this.Text))
				{
					x.Height(24f, Unit.Point).Width(24f, Unit.Point).SvgPath("M19,19H5V5H19M19,3H5A2,2 0 0,0 3,5V19A2,2 0 0,0 5,21H19A2,2 0 0,0 21,19V5A2,2 0 0,0 19,3M13.96,12.29L11.21,15.83L9.25,13.47L6.5,17H17.5L13.96,12.29Z", Colors.Black);
					return;
				}
				x.Text(this.Text).FontSize(14f);
			}, "x =>\r\n                {\r\n                    if (string.IsNullOrWhiteSpace(Text))\r\n                        x.Height(imageSvgSize).Width(imageSvgSize).SvgPath(imageSvgPath, Colors.Black);\r\n                    \r\n                    else\r\n                        x.Text(Text).FontSize(14);\r\n                }", "Compose", "/_/Source/QuestPDF/Elements/Placeholder.cs", 21);
		}
	}
}
