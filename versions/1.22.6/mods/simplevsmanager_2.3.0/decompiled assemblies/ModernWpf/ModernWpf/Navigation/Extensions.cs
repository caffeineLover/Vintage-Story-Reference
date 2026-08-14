using System;
using System.Windows.Navigation;

namespace ModernWpf.Navigation
{
	// Token: 0x02000231 RID: 561
	public static class Extensions
	{
		// Token: 0x06001083 RID: 4227 RVA: 0x0002F677 File Offset: 0x0002D877
		public static Type SourcePageType(this NavigatingCancelEventArgs e)
		{
			object content = e.Content;
			if (content == null)
			{
				return null;
			}
			return content.GetType();
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0002F68A File Offset: 0x0002D88A
		public static Type SourcePageType(this NavigationEventArgs e)
		{
			object content = e.Content;
			if (content == null)
			{
				return null;
			}
			return content.GetType();
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x0002F69D File Offset: 0x0002D89D
		public static object Parameter(this NavigatingCancelEventArgs e)
		{
			return e.ExtraData;
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x0002F6A5 File Offset: 0x0002D8A5
		public static object Parameter(this NavigationEventArgs e)
		{
			return e.ExtraData;
		}
	}
}
