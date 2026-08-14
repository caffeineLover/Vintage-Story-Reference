using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020000C2 RID: 194
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerStepThrough]
	internal readonly struct CharacterAnalyzer<[Nullable(0)] TBuffer> where TBuffer : ILookAheadBuffer
	{
		// Token: 0x0600048D RID: 1165 RVA: 0x0000EC84 File Offset: 0x0000CE84
		public CharacterAnalyzer(TBuffer buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.<Buffer>k__BackingField = buffer;
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x0000ECAD File Offset: 0x0000CEAD
		public TBuffer Buffer
		{
			[CompilerGenerated]
			get
			{
				return this.<Buffer>k__BackingField;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x0000ECB8 File Offset: 0x0000CEB8
		public bool EndOfInput
		{
			get
			{
				TBuffer buffer = this.Buffer;
				return buffer.EndOfInput;
			}
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0000ECDC File Offset: 0x0000CEDC
		public char Peek(int offset)
		{
			TBuffer buffer = this.Buffer;
			return buffer.Peek(offset);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0000ED00 File Offset: 0x0000CF00
		public void Skip(int length)
		{
			TBuffer buffer = this.Buffer;
			buffer.Skip(length);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0000ED24 File Offset: 0x0000CF24
		public bool IsAlphaNumericDashOrUnderscore(int offset = 0)
		{
			TBuffer buffer = this.Buffer;
			char c = buffer.Peek(offset);
			return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_' || c == '-';
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x0000ED74 File Offset: 0x0000CF74
		public bool IsAscii(int offset = 0)
		{
			TBuffer buffer = this.Buffer;
			return buffer.Peek(offset) <= '\u007f';
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x0000EDA0 File Offset: 0x0000CFA0
		public bool IsPrintable(int offset = 0)
		{
			TBuffer buffer = this.Buffer;
			char c = buffer.Peek(offset);
			return c == '\t' || c == '\n' || c == '\r' || (c >= ' ' && c <= '~') || c == '\u0085' || (c >= '\u00a0' && c <= '퟿') || (c >= '' && c <= '�');
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0000EE0C File Offset: 0x0000D00C
		public bool IsDigit(int offset = 0)
		{
			TBuffer buffer = this.Buffer;
			char c = buffer.Peek(offset);
			return c >= '0' && c <= '9';
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0000EE40 File Offset: 0x0000D040
		public int AsDigit(int offset = 0)
		{
			TBuffer buffer = this.Buffer;
			return (int)(buffer.Peek(offset) - '0');
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0000EE68 File Offset: 0x0000D068
		public bool IsHex(int offset)
		{
			TBuffer buffer = this.Buffer;
			char c = buffer.Peek(offset);
			return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0000EEB0 File Offset: 0x0000D0B0
		public int AsHex(int offset)
		{
			TBuffer buffer = this.Buffer;
			char c = buffer.Peek(offset);
			if (c <= '9')
			{
				return (int)(c - '0');
			}
			if (c <= 'F')
			{
				return (int)(c - 'A' + '\n');
			}
			return (int)(c - 'a' + '\n');
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0000EEF1 File Offset: 0x0000D0F1
		public bool IsSpace(int offset = 0)
		{
			return this.Check(' ', offset);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0000EEFC File Offset: 0x0000D0FC
		public bool IsZero(int offset = 0)
		{
			return this.Check('\0', offset);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0000EF06 File Offset: 0x0000D106
		public bool IsTab(int offset = 0)
		{
			return this.Check('\t', offset);
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0000EF11 File Offset: 0x0000D111
		public bool IsWhite(int offset = 0)
		{
			return this.IsSpace(offset) || this.IsTab(offset);
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0000EF25 File Offset: 0x0000D125
		public bool IsBreak(int offset = 0)
		{
			return this.Check("\r\n\u0085\u2028\u2029", offset);
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x0000EF33 File Offset: 0x0000D133
		public bool IsCrLf(int offset = 0)
		{
			return this.Check('\r', offset) && this.Check('\n', offset + 1);
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x0000EF4D File Offset: 0x0000D14D
		public bool IsBreakOrZero(int offset = 0)
		{
			return this.IsBreak(offset) || this.IsZero(offset);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0000EF61 File Offset: 0x0000D161
		public bool IsWhiteBreakOrZero(int offset = 0)
		{
			return this.IsWhite(offset) || this.IsBreakOrZero(offset);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0000EF78 File Offset: 0x0000D178
		public bool Check(char expected, int offset = 0)
		{
			TBuffer buffer = this.Buffer;
			return buffer.Peek(offset) == expected;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0000EFA0 File Offset: 0x0000D1A0
		public bool Check(string expectedCharacters, int offset = 0)
		{
			TBuffer buffer = this.Buffer;
			char c = buffer.Peek(offset);
			return expectedCharacters.Contains(c);
		}
	}
}
