using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Graphics.Operations;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Graphics
{
	// Token: 0x02000108 RID: 264
	internal interface IGraphicsStateOperationFactory
	{
		// Token: 0x06000586 RID: 1414
		[CanBeNull]
		IGraphicsStateOperation Create(OperatorToken op, IReadOnlyList<IToken> operands);
	}
}
