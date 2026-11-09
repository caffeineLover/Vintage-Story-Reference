using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.TextState
{
	// Token: 0x02000121 RID: 289
	public class SetCharacterSpacing : IGraphicsStateOperation
	{
		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x00022F26 File Offset: 0x00021126
		public string Operator
		{
			get
			{
				return "Tc";
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x00022F2D File Offset: 0x0002112D
		public decimal Spacing { get; }

		// Token: 0x06000661 RID: 1633 RVA: 0x00022F35 File Offset: 0x00021135
		public SetCharacterSpacing(decimal spacing)
		{
			this.Spacing = spacing;
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00022F44 File Offset: 0x00021144
		public void Run(IOperationContext operationContext)
		{
			operationContext.SetCharacterSpacing((double)this.Spacing);
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00022F58 File Offset: 0x00021158
		public void Write(Stream stream)
		{
			stream.WriteNumberText(this.Spacing, "Tc");
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00022F6C File Offset: 0x0002116C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Spacing);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("Tc");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000324 RID: 804
		public const string Symbol = "Tc";
	}
}
