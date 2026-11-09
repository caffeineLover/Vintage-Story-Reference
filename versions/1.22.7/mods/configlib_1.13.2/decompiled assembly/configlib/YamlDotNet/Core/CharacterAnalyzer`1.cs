using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001A6 RID: 422
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerStepThrough]
	internal sealed class CharacterAnalyzer<TBuffer> where TBuffer : class, ILookAheadBuffer
	{
		// Token: 0x060008E6 RID: 2278 RVA: 0x0001C914 File Offset: 0x0001AB14
		public CharacterAnalyzer(TBuffer buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.<Buffer>k__BackingField = buffer;
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060008E7 RID: 2279 RVA: 0x0001C942 File Offset: 0x0001AB42
		public TBuffer Buffer
		{
			[CompilerGenerated]
			get
			{
				return this.<Buffer>k__BackingField;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060008E8 RID: 2280 RVA: 0x0001C94A File Offset: 0x0001AB4A
		public bool EndOfInput
		{
			get
			{
				return this.Buffer.EndOfInput;
			}
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0001C95C File Offset: 0x0001AB5C
		public char Peek(int offset)
		{
			return this.Buffer.Peek(offset);
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0001C96F File Offset: 0x0001AB6F
		public void Skip(int length)
		{
			this.Buffer.Skip(length);
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0001C984 File Offset: 0x0001AB84
		public bool IsAlphaNumericDashOrUnderscore(int offset = 0)
		{
			char c = this.Buffer.Peek(offset);
			return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_' || c == '-';
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0001C9CD File Offset: 0x0001ABCD
		public bool IsAscii(int offset = 0)
		{
			return this.Buffer.Peek(offset) <= '\u007f';
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0001C9E8 File Offset: 0x0001ABE8
		public bool IsPrintable(int offset = 0)
		{
			char c = this.Buffer.Peek(offset);
			return c == '\t' || c == '\n' || c == '\r' || (c >= ' ' && c <= '~') || c == '\u0085' || (c >= '\u00a0' && c <= '퟿') || (c >= '' && c <= '�');
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0001CA50 File Offset: 0x0001AC50
		public bool IsDigit(int offset = 0)
		{
			char c = this.Buffer.Peek(offset);
			return c >= '0' && c <= '9';
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0001CA7E File Offset: 0x0001AC7E
		public int AsDigit(int offset = 0)
		{
			return (int)(this.Buffer.Peek(offset) - '0');
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0001CA94 File Offset: 0x0001AC94
		public bool IsHex(int offset)
		{
			char c = this.Buffer.Peek(offset);
			return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0001CAD8 File Offset: 0x0001ACD8
		public int AsHex(int offset)
		{
			char c = this.Buffer.Peek(offset);
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

		// Token: 0x060008F2 RID: 2290 RVA: 0x0001CB15 File Offset: 0x0001AD15
		public bool IsSpace(int offset = 0)
		{
			return this.Check(' ', offset);
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0001CB20 File Offset: 0x0001AD20
		public bool IsZero(int offset = 0)
		{
			return this.Check('\0', offset);
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0001CB2A File Offset: 0x0001AD2A
		public bool IsTab(int offset = 0)
		{
			return this.Check('\t', offset);
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0001CB35 File Offset: 0x0001AD35
		public bool IsWhite(int offset = 0)
		{
			return this.IsSpace(offset) || this.IsTab(offset);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0001CB49 File Offset: 0x0001AD49
		public bool IsBreak(int offset = 0)
		{
			return this.Check("\r\n\u0085\u2028\u2029", offset);
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0001CB57 File Offset: 0x0001AD57
		public bool IsCrLf(int offset = 0)
		{
			return this.Check('\r', offset) && this.Check('\n', offset + 1);
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0001CB71 File Offset: 0x0001AD71
		public bool IsBreakOrZero(int offset = 0)
		{
			return this.IsBreak(offset) || this.IsZero(offset);
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0001CB85 File Offset: 0x0001AD85
		public bool IsWhiteBreakOrZero(int offset = 0)
		{
			return this.IsWhite(offset) || this.IsBreakOrZero(offset);
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0001CB99 File Offset: 0x0001AD99
		public bool Check(char expected, int offset = 0)
		{
			return this.Buffer.Peek(offset) == expected;
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0001CBB0 File Offset: 0x0001ADB0
		public bool Check(string expectedCharacters, int offset = 0)
		{
			char c = this.Buffer.Peek(offset);
			return expectedCharacters.IndexOf(c) != -1;
		}
	}
}
