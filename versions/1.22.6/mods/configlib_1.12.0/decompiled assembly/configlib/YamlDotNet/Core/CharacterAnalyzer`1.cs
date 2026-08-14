using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Core
{
	// Token: 0x020001AA RID: 426
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerStepThrough]
	internal sealed class CharacterAnalyzer<TBuffer> where TBuffer : class, ILookAheadBuffer
	{
		// Token: 0x06000922 RID: 2338 RVA: 0x0001D25C File Offset: 0x0001B45C
		public CharacterAnalyzer(TBuffer buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.<Buffer>k__BackingField = buffer;
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x0001D28A File Offset: 0x0001B48A
		public TBuffer Buffer
		{
			[CompilerGenerated]
			get
			{
				return this.<Buffer>k__BackingField;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000924 RID: 2340 RVA: 0x0001D292 File Offset: 0x0001B492
		public bool EndOfInput
		{
			get
			{
				return this.Buffer.EndOfInput;
			}
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0001D2A4 File Offset: 0x0001B4A4
		public char Peek(int offset)
		{
			return this.Buffer.Peek(offset);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0001D2B7 File Offset: 0x0001B4B7
		public void Skip(int length)
		{
			this.Buffer.Skip(length);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0001D2CC File Offset: 0x0001B4CC
		public bool IsAlphaNumericDashOrUnderscore(int offset = 0)
		{
			char c = this.Buffer.Peek(offset);
			return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_' || c == '-';
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0001D315 File Offset: 0x0001B515
		public bool IsAscii(int offset = 0)
		{
			return this.Buffer.Peek(offset) <= '\u007f';
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x0001D330 File Offset: 0x0001B530
		public bool IsPrintable(int offset = 0)
		{
			char c = this.Buffer.Peek(offset);
			return c == '\t' || c == '\n' || c == '\r' || (c >= ' ' && c <= '~') || c == '\u0085' || (c >= '\u00a0' && c <= '퟿') || (c >= '' && c <= '�');
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0001D398 File Offset: 0x0001B598
		public bool IsDigit(int offset = 0)
		{
			char c = this.Buffer.Peek(offset);
			return c >= '0' && c <= '9';
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0001D3C6 File Offset: 0x0001B5C6
		public int AsDigit(int offset = 0)
		{
			return (int)(this.Buffer.Peek(offset) - '0');
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0001D3DC File Offset: 0x0001B5DC
		public bool IsHex(int offset)
		{
			char c = this.Buffer.Peek(offset);
			return (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x0001D420 File Offset: 0x0001B620
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

		// Token: 0x0600092E RID: 2350 RVA: 0x0001D45D File Offset: 0x0001B65D
		public bool IsSpace(int offset = 0)
		{
			return this.Check(' ', offset);
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0001D468 File Offset: 0x0001B668
		public bool IsZero(int offset = 0)
		{
			return this.Check('\0', offset);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0001D472 File Offset: 0x0001B672
		public bool IsTab(int offset = 0)
		{
			return this.Check('\t', offset);
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0001D47D File Offset: 0x0001B67D
		public bool IsWhite(int offset = 0)
		{
			return this.IsSpace(offset) || this.IsTab(offset);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x0001D491 File Offset: 0x0001B691
		public bool IsBreak(int offset = 0)
		{
			return this.Check("\r\n\u0085\u2028\u2029", offset);
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x0001D49F File Offset: 0x0001B69F
		public bool IsCrLf(int offset = 0)
		{
			return this.Check('\r', offset) && this.Check('\n', offset + 1);
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x0001D4B9 File Offset: 0x0001B6B9
		public bool IsBreakOrZero(int offset = 0)
		{
			return this.IsBreak(offset) || this.IsZero(offset);
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x0001D4CD File Offset: 0x0001B6CD
		public bool IsWhiteBreakOrZero(int offset = 0)
		{
			return this.IsWhite(offset) || this.IsBreakOrZero(offset);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x0001D4E1 File Offset: 0x0001B6E1
		public bool Check(char expected, int offset = 0)
		{
			return this.Buffer.Peek(offset) == expected;
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x0001D4F8 File Offset: 0x0001B6F8
		public bool Check(string expectedCharacters, int offset = 0)
		{
			char c = this.Buffer.Peek(offset);
			return expectedCharacters.IndexOf(c) != -1;
		}
	}
}
