using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations.ClippingPaths
{
	// Token: 0x0200015A RID: 346
	public class ModifyClippingByEvenOddIntersect : IGraphicsStateOperation
	{
		// Token: 0x1700021D RID: 541
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x0002585E File Offset: 0x00023A5E
		public string Operator
		{
			get
			{
				return "W*";
			}
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00025865 File Offset: 0x00023A65
		private ModifyClippingByEvenOddIntersect()
		{
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0002586D File Offset: 0x00023A6D
		public void Run(IOperationContext operationContext)
		{
			operationContext.ModifyClippingIntersect(1);
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00025876 File Offset: 0x00023A76
		public void Write(Stream stream)
		{
			stream.WriteText("W*", false);
			stream.WriteNewLine();
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0002588A File Offset: 0x00023A8A
		public override string ToString()
		{
			return "W*";
		}

		// Token: 0x040003BD RID: 957
		public const string Symbol = "W*";

		// Token: 0x040003BE RID: 958
		public static readonly ModifyClippingByEvenOddIntersect Value = new ModifyClippingByEvenOddIntersect();
	}
}
