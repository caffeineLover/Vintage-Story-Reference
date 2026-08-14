using System;
using System.IO;

namespace UglyToad.PdfPig.Graphics.Operations
{
	// Token: 0x02000111 RID: 273
	public interface IGraphicsStateOperation
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060005EF RID: 1519
		string Operator { get; }

		// Token: 0x060005F0 RID: 1520
		void Write(Stream stream);

		// Token: 0x060005F1 RID: 1521
		void Run(IOperationContext operationContext);
	}
}
