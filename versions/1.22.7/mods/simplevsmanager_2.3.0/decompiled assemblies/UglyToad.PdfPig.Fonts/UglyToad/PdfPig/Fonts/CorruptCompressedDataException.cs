using System;
using System.Runtime.Serialization;

namespace UglyToad.PdfPig.Fonts
{
	// Token: 0x02000004 RID: 4
	[Serializable]
	public class CorruptCompressedDataException : Exception
	{
		// Token: 0x0600001A RID: 26 RVA: 0x0000237E File Offset: 0x0000057E
		public CorruptCompressedDataException()
		{
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002386 File Offset: 0x00000586
		public CorruptCompressedDataException(string message) : base(message)
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000238F File Offset: 0x0000058F
		public CorruptCompressedDataException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002399 File Offset: 0x00000599
		protected CorruptCompressedDataException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
