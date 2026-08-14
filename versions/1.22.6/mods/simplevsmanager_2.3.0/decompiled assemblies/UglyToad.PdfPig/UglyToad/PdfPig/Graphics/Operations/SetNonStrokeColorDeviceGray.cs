using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations
{
	// Token: 0x02000118 RID: 280
	public class SetNonStrokeColorDeviceGray : IGraphicsStateOperation
	{
		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600061D RID: 1565 RVA: 0x00022675 File Offset: 0x00020875
		public string Operator
		{
			get
			{
				return "g";
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x0002267C File Offset: 0x0002087C
		public decimal Gray { get; }

		// Token: 0x0600061F RID: 1567 RVA: 0x00022684 File Offset: 0x00020884
		public SetNonStrokeColorDeviceGray(decimal gray)
		{
			this.Gray = gray;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00022693 File Offset: 0x00020893
		public void Run(IOperationContext operationContext)
		{
			operationContext.GetCurrentState().ColorSpaceContext.SetNonStrokingColorGray(this.Gray);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x000226AB File Offset: 0x000208AB
		public void Write(Stream stream)
		{
			stream.WriteNumberText(this.Gray, "g");
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x000226C0 File Offset: 0x000208C0
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Gray);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("g");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000307 RID: 775
		public const string Symbol = "g";
	}
}
