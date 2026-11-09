using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Graphics.Core;

namespace UglyToad.PdfPig.Graphics.Operations.General
{
	// Token: 0x02000153 RID: 339
	public class SetLineCap : IGraphicsStateOperation
	{
		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0002542A File Offset: 0x0002362A
		public string Operator
		{
			get
			{
				return "J";
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x00025431 File Offset: 0x00023631
		public LineCapStyle Cap { get; }

		// Token: 0x060007B8 RID: 1976 RVA: 0x00025439 File Offset: 0x00023639
		public SetLineCap(int cap) : this((LineCapStyle)cap)
		{
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00025442 File Offset: 0x00023642
		public SetLineCap(LineCapStyle cap)
		{
			if (cap < LineCapStyle.Butt || cap > LineCapStyle.ProjectingSquare)
			{
				throw new ArgumentException("Invalid argument passed for line cap style. Should be 0, 1 or 2; instead got: " + cap.ToString());
			}
			this.Cap = cap;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00025476 File Offset: 0x00023676
		public void Run(IOperationContext operationContext)
		{
			operationContext.SetLineCap(this.Cap);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x00025484 File Offset: 0x00023684
		public void Write(Stream stream)
		{
			stream.WriteNumberText((int)this.Cap, "J");
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0002549C File Offset: 0x0002369C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>((int)this.Cap);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("J");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040003AF RID: 943
		public const string Symbol = "J";
	}
}
