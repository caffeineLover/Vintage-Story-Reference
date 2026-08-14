using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Operations
{
	// Token: 0x02000112 RID: 274
	public class InvokeNamedXObject : IGraphicsStateOperation
	{
		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x0002206C File Offset: 0x0002026C
		public string Operator
		{
			get
			{
				return "Do";
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x00022073 File Offset: 0x00020273
		public NameToken Name { get; }

		// Token: 0x060005F4 RID: 1524 RVA: 0x0002207B File Offset: 0x0002027B
		public InvokeNamedXObject(NameToken name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.Name = name;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00022099 File Offset: 0x00020299
		public void Run(IOperationContext operationContext)
		{
			operationContext.ApplyXObject(this.Name);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x000220A7 File Offset: 0x000202A7
		public void Write(Stream stream)
		{
			stream.WriteText("/" + this.Name.Data, false);
			stream.WriteWhiteSpace();
			stream.WriteText("Do", false);
			stream.WriteNewLine();
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x000220E0 File Offset: 0x000202E0
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<NameToken>(this.Name);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted("Do");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040002F6 RID: 758
		public const string Symbol = "Do";
	}
}
