using System;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x0200002D RID: 45
	[NullableContext(1)]
	[Nullable(0)]
	internal class Trace
	{
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600027C RID: 636 RVA: 0x00011CB6 File Offset: 0x0000FEB6
		internal static Trace Current
		{
			get
			{
				if (Trace._current == null)
				{
					Trace._current = new Trace();
				}
				return Trace._current;
			}
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00011CCE File Offset: 0x0000FECE
		private void WriteLineIntern(string message, string category)
		{
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00011CD0 File Offset: 0x0000FED0
		public static void WriteLine(string message, string category)
		{
			Trace.Current.WriteLineIntern(message, category);
		}

		// Token: 0x040000E3 RID: 227
		internal static Trace _current;
	}
}
