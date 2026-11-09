using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Operations.TextState
{
	// Token: 0x02000123 RID: 291
	public class SetHorizontalScaling : IGraphicsStateOperation
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x0002309D File Offset: 0x0002129D
		public string Operator
		{
			get
			{
				return "Tz";
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x000230A4 File Offset: 0x000212A4
		public decimal Scale { get; }

		// Token: 0x0600066E RID: 1646 RVA: 0x000230AC File Offset: 0x000212AC
		public SetHorizontalScaling(decimal scale)
		{
			this.Scale = scale;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x000230BB File Offset: 0x000212BB
		public void Run(IOperationContext operationContext)
		{
			operationContext.SetHorizontalScaling((double)this.Scale);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x000230CF File Offset: 0x000212CF
		public void Write(Stream stream)
		{
			stream.WriteNumberText(this.Scale, "Tz");
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x000230E4 File Offset: 0x000212E4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<decimal>(this.Scale);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("Tz");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000329 RID: 809
		public const string Symbol = "Tz";
	}
}
