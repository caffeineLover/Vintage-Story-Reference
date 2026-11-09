using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.ClippingPaths
{
	// Token: 0x0200015B RID: 347
	public class ModifyClippingByNonZeroWindingIntersect : IGraphicsStateOperation
	{
		// Token: 0x1700021E RID: 542
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x0002589D File Offset: 0x00023A9D
		public string Operator
		{
			get
			{
				return "W";
			}
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x000258A4 File Offset: 0x00023AA4
		private ModifyClippingByNonZeroWindingIntersect()
		{
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x000258AC File Offset: 0x00023AAC
		public void Run(IOperationContext operationContext)
		{
			operationContext.ModifyClippingIntersect(2);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x000258B5 File Offset: 0x00023AB5
		public void Write(Stream stream)
		{
			stream.WriteText("W", false);
			stream.WriteNewLine();
		}

		// Token: 0x060007EC RID: 2028 RVA: 0x000258C9 File Offset: 0x00023AC9
		public override string ToString()
		{
			return "W";
		}

		// Token: 0x040003BF RID: 959
		public const string Symbol = "W";

		// Token: 0x040003C0 RID: 960
		public static readonly ModifyClippingByNonZeroWindingIntersect Value = new ModifyClippingByNonZeroWindingIntersect();
	}
}
