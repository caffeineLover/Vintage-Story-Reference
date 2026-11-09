using System;

namespace ModernWpf
{
	// Token: 0x0200021F RID: 543
	internal static class PackUriHelper
	{
		// Token: 0x06000FCA RID: 4042 RVA: 0x0002D04F File Offset: 0x0002B24F
		public static Uri GetAbsoluteUri(string path)
		{
			return new Uri("pack://application:,,,/ModernWpf;component/" + path);
		}
	}
}
