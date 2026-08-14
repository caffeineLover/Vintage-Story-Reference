using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Graphics.Core;

namespace UglyToad.PdfPig.Graphics.Operations.General
{
	// Token: 0x02000155 RID: 341
	public class SetLineJoin : IGraphicsStateOperation
	{
		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x0002562F File Offset: 0x0002382F
		public string Operator
		{
			get
			{
				return "j";
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x00025636 File Offset: 0x00023836
		public LineJoinStyle Join { get; }

		// Token: 0x060007C5 RID: 1989 RVA: 0x0002563E File Offset: 0x0002383E
		public SetLineJoin(int join) : this((LineJoinStyle)join)
		{
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00025647 File Offset: 0x00023847
		public SetLineJoin(LineJoinStyle join)
		{
			if (join < LineJoinStyle.Miter || join > LineJoinStyle.Bevel)
			{
				throw new ArgumentException("Invalid argument passed for line join style. Should be 0, 1 or 2; instead got: " + join.ToString());
			}
			this.Join = join;
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0002567B File Offset: 0x0002387B
		public void Run(IOperationContext operationContext)
		{
			operationContext.SetLineJoin(this.Join);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00025689 File Offset: 0x00023889
		public void Write(Stream stream)
		{
			stream.WriteNumberText((int)this.Join, "j");
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x000256A4 File Offset: 0x000238A4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>((int)this.Join);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("j");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040003B3 RID: 947
		public const string Symbol = "j";
	}
}
