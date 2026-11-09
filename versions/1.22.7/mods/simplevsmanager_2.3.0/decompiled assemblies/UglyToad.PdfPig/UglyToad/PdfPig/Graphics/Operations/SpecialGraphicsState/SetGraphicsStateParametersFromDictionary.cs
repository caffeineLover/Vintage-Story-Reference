using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Operations.SpecialGraphicsState
{
	// Token: 0x02000137 RID: 311
	public class SetGraphicsStateParametersFromDictionary : IGraphicsStateOperation
	{
		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0002424F File Offset: 0x0002244F
		public string Operator
		{
			get
			{
				return "gs";
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x00024256 File Offset: 0x00022456
		public NameToken Name { get; }

		// Token: 0x060006F8 RID: 1784 RVA: 0x0002425E File Offset: 0x0002245E
		public SetGraphicsStateParametersFromDictionary(NameToken name)
		{
			this.Name = name;
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0002426D File Offset: 0x0002246D
		public void Run(IOperationContext operationContext)
		{
			operationContext.SetNamedGraphicsState(this.Name);
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x0002427B File Offset: 0x0002247B
		public void Write(Stream stream)
		{
			stream.WriteText("/" + this.Name.Data, false);
			stream.WriteWhiteSpace();
			stream.WriteText("gs", false);
			stream.WriteNewLine();
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x000242B4 File Offset: 0x000224B4
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(this.Name);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("gs");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0400035F RID: 863
		public const string Symbol = "gs";
	}
}
