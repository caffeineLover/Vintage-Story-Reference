using System;
using System.Runtime.Serialization;

namespace UglyToad.PdfPig.Fonts
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	public class InvalidFontFormatException : Exception
	{
		// Token: 0x06000029 RID: 41 RVA: 0x00002870 File Offset: 0x00000A70
		public InvalidFontFormatException()
		{
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002878 File Offset: 0x00000A78
		public InvalidFontFormatException(string message) : base(message)
		{
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002881 File Offset: 0x00000A81
		public InvalidFontFormatException(string message, Exception inner) : base(message, inner)
		{
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000288B File Offset: 0x00000A8B
		protected InvalidFontFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
