using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics.Operations;
using UglyToad.PdfPig.Logging;

namespace UglyToad.PdfPig.Parser
{
	// Token: 0x020000C4 RID: 196
	internal interface IPageContentParser
	{
		// Token: 0x060003BF RID: 959
		IReadOnlyList<IGraphicsStateOperation> Parse(int pageNumber, IInputBytes inputBytes, ILog log);
	}
}
