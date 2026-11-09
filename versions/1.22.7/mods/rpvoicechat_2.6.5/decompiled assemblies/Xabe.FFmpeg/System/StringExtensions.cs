using System;
using System.Linq;

namespace System
{
	// Token: 0x02000003 RID: 3
	public static class StringExtensions
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002078 File Offset: 0x00000278
		public static string Escape(this string output)
		{
			if (output == null)
			{
				return output;
			}
			if ((Enumerable.Last<char>(output) == '"' && Enumerable.First<char>(output) == '"') || (Enumerable.Last<char>(output) == '\'' && Enumerable.First<char>(output) == '\''))
			{
				output = output.Substring(1, output.Length - 2);
			}
			output = "\"" + output + "\"";
			return output;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020D8 File Offset: 0x000002D8
		public static string Unescape(this string output)
		{
			if (output == null || output.Length < 2)
			{
				return output;
			}
			if ((Enumerable.Last<char>(output) == '"' && Enumerable.First<char>(output) == '"') || (Enumerable.Last<char>(output) == '\'' && Enumerable.First<char>(output) == '\''))
			{
				return output.Substring(1, output.Length - 2);
			}
			return output;
		}
	}
}
