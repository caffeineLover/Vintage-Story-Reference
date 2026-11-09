using System;
using System.Runtime.Serialization;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x0200000B RID: 11
	[Serializable]
	public class PdfDocumentFormatException : Exception
	{
		// Token: 0x06000027 RID: 39 RVA: 0x0000252D File Offset: 0x0000072D
		public PdfDocumentFormatException()
		{
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002535 File Offset: 0x00000735
		public PdfDocumentFormatException(string message) : base(message)
		{
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000253E File Offset: 0x0000073E
		public PdfDocumentFormatException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002548 File Offset: 0x00000748
		protected PdfDocumentFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
