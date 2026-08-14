using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokenization.Scanner;

namespace UglyToad.PdfPig.Parser.FileStructure
{
	// Token: 0x020000D1 RID: 209
	internal class CrossReferenceOffsetValidator
	{
		// Token: 0x060003EF RID: 1007 RVA: 0x00018234 File Offset: 0x00016434
		public CrossReferenceOffsetValidator(XrefOffsetValidator offsetValidator)
		{
			this.offsetValidator = offsetValidator;
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00018244 File Offset: 0x00016444
		public long Validate(long crossReferenceOffset, ISeekableTokenScanner scanner, IInputBytes bytes, bool isLenientParsing)
		{
			long num = this.offsetValidator.CheckXRefOffset(crossReferenceOffset, scanner, bytes, isLenientParsing);
			if (num > -1L)
			{
				crossReferenceOffset = num;
			}
			return crossReferenceOffset;
		}

		// Token: 0x04000201 RID: 513
		private readonly XrefOffsetValidator offsetValidator;
	}
}
