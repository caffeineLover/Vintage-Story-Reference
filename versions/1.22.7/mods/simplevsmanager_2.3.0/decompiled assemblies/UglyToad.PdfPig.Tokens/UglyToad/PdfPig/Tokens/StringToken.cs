using System;
using System.Text;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Tokens
{
	// Token: 0x02000012 RID: 18
	public class StringToken : IDataToken<string>, IToken, IEquatable<IToken>
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000066 RID: 102 RVA: 0x000055E9 File Offset: 0x000037E9
		public string Data { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000055F1 File Offset: 0x000037F1
		public StringToken.Encoding EncodedWith { get; }

		// Token: 0x06000068 RID: 104 RVA: 0x000055F9 File Offset: 0x000037F9
		public StringToken(string data, StringToken.Encoding encodedWith = StringToken.Encoding.Iso88591)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			this.Data = data;
			this.EncodedWith = encodedWith;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00005620 File Offset: 0x00003820
		public byte[] GetBytes()
		{
			StringToken.Encoding encodedWith = this.EncodedWith;
			if (encodedWith == StringToken.Encoding.Utf16)
			{
				return System.Text.Encoding.Unicode.GetBytes(this.Data);
			}
			if (encodedWith == StringToken.Encoding.Utf16BE)
			{
				byte[] bytes = System.Text.Encoding.BigEndianUnicode.GetBytes(this.Data);
				byte[] array = new byte[bytes.Length + 2];
				array[0] = 254;
				array[1] = byte.MaxValue;
				Array.Copy(bytes, 0, array, 2, bytes.Length);
				return array;
			}
			return OtherEncodings.StringAsLatin1Bytes(this.Data);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00005694 File Offset: 0x00003894
		public bool Equals(IToken obj)
		{
			if (this == obj)
			{
				return true;
			}
			StringToken stringToken = obj as StringToken;
			return stringToken != null && this.EncodedWith.Equals(stringToken.EncodedWith) && this.Data.Equals(stringToken.Data);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000056E7 File Offset: 0x000038E7
		public override string ToString()
		{
			return "(" + this.Data + ")";
		}

		// Token: 0x02000016 RID: 22
		public enum Encoding : byte
		{
			// Token: 0x0400026C RID: 620
			Iso88591,
			// Token: 0x0400026D RID: 621
			Utf16,
			// Token: 0x0400026E RID: 622
			Utf16BE
		}
	}
}
