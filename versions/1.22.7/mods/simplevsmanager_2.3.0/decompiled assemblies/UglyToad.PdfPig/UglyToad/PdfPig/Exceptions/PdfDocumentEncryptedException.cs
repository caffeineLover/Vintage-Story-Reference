using System;
using System.Runtime.Serialization;
using UglyToad.PdfPig.Encryption;

namespace UglyToad.PdfPig.Exceptions
{
	// Token: 0x020001CB RID: 459
	[Serializable]
	public class PdfDocumentEncryptedException : Exception
	{
		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x000371B7 File Offset: 0x000353B7
		internal EncryptionDictionary Dictionary { get; }

		// Token: 0x06000AE1 RID: 2785 RVA: 0x000371BF File Offset: 0x000353BF
		public PdfDocumentEncryptedException()
		{
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x000371C7 File Offset: 0x000353C7
		public PdfDocumentEncryptedException(string message) : base(message)
		{
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x000371D0 File Offset: 0x000353D0
		public PdfDocumentEncryptedException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x000371DA File Offset: 0x000353DA
		internal PdfDocumentEncryptedException(string message, EncryptionDictionary dictionary) : base(message)
		{
			this.Dictionary = dictionary;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x000371EA File Offset: 0x000353EA
		internal PdfDocumentEncryptedException(string message, EncryptionDictionary dictionary, Exception inner) : base(message, inner)
		{
			this.Dictionary = dictionary;
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x000371FB File Offset: 0x000353FB
		protected PdfDocumentEncryptedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
