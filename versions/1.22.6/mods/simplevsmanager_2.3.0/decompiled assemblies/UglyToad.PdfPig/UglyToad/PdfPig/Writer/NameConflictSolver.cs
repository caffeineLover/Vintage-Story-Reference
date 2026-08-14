using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Writer
{
	// Token: 0x02000017 RID: 23
	internal class NameConflictSolver
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00005968 File Offset: 0x00003B68
		public NameConflictSolver(string prefix)
		{
			this.prefix = prefix;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005984 File Offset: 0x00003B84
		private string ExtractPrefix(string name)
		{
			if (name == null)
			{
				return this.prefix;
			}
			int num = 0;
			while (num < name.Length && (name.get_Chars(num) < '0' || name.get_Chars(num) > '9'))
			{
				num++;
			}
			if (num == 0)
			{
				return this.prefix;
			}
			return name.Substring(0, num);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x000059D4 File Offset: 0x00003BD4
		public string NewName(string orginalName = null)
		{
			string text = this.ExtractPrefix(orginalName);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(0, 2);
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.key);
			string text2 = defaultInterpolatedStringHandler.ToStringAndClear();
			while (this.xobjectNamesUsed.Contains(text2))
			{
				defaultInterpolatedStringHandler..ctor(0, 2);
				defaultInterpolatedStringHandler.AppendFormatted(text);
				int num = this.key + 1;
				this.key = num;
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				text2 = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			this.xobjectNamesUsed.Add(text2);
			return text2;
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00005A5E File Offset: 0x00003C5E
		public string FixName(string name)
		{
			if (this.xobjectNamesUsed.Contains(name))
			{
				return this.NewName(name);
			}
			this.xobjectNamesUsed.Add(name);
			return name;
		}

		// Token: 0x0400004F RID: 79
		private string prefix;

		// Token: 0x04000050 RID: 80
		private int key;

		// Token: 0x04000051 RID: 81
		private HashSet<string> xobjectNamesUsed = new HashSet<string>();
	}
}
