using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x02000034 RID: 52
	internal static class Util
	{
		// Token: 0x060001D9 RID: 473 RVA: 0x00006D86 File Offset: 0x00004F86
		public static Visibility VisibilityFromBool(bool visible)
		{
			if (!visible)
			{
				return 2;
			}
			return 0;
		}
	}
}
