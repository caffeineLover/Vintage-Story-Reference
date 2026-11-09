using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.Type1.Parser
{
	// Token: 0x02000011 RID: 17
	internal class Type1Token
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00004DC1 File Offset: 0x00002FC1
		public Type1Token.TokenType Type { get; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00004DC9 File Offset: 0x00002FC9
		public string Text { get; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00004DD1 File Offset: 0x00002FD1
		public virtual bool IsPrivateDictionary
		{
			get
			{
				return this.Type == Type1Token.TokenType.Literal && string.Equals(this.Text, "Private", 5);
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004DEF File Offset: 0x00002FEF
		public Type1Token(char c, Type1Token.TokenType type) : this(c.ToString(), type)
		{
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004DFF File Offset: 0x00002FFF
		public Type1Token(string text, Type1Token.TokenType type)
		{
			this.Text = text;
			this.Type = type;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004E15 File Offset: 0x00003015
		public int AsInt()
		{
			return (int)this.AsDecimal();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004E22 File Offset: 0x00003022
		public decimal AsDecimal()
		{
			return decimal.Parse(this.Text, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004E34 File Offset: 0x00003034
		public bool AsBool()
		{
			return string.Equals(this.Text, "true", 5);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004E48 File Offset: 0x00003048
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(19, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Token[type=");
			defaultInterpolatedStringHandler.AppendFormatted<Type1Token.TokenType>(this.Type);
			defaultInterpolatedStringHandler.AppendLiteral(", text=");
			defaultInterpolatedStringHandler.AppendFormatted(this.Text);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x020000BF RID: 191
		public enum TokenType
		{
			// Token: 0x04000443 RID: 1091
			None,
			// Token: 0x04000444 RID: 1092
			String,
			// Token: 0x04000445 RID: 1093
			Name,
			// Token: 0x04000446 RID: 1094
			Literal,
			// Token: 0x04000447 RID: 1095
			Real,
			// Token: 0x04000448 RID: 1096
			Integer,
			// Token: 0x04000449 RID: 1097
			StartArray,
			// Token: 0x0400044A RID: 1098
			EndArray,
			// Token: 0x0400044B RID: 1099
			StartProc,
			// Token: 0x0400044C RID: 1100
			EndProc,
			// Token: 0x0400044D RID: 1101
			StartDict,
			// Token: 0x0400044E RID: 1102
			EndDict,
			// Token: 0x0400044F RID: 1103
			Charstring
		}
	}
}
