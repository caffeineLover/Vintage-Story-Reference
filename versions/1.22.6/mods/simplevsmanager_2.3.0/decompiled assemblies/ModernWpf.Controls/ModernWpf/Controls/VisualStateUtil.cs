using System;
using System.Windows;
using System.Windows.Controls;

namespace ModernWpf.Controls
{
	// Token: 0x02000032 RID: 50
	internal static class VisualStateUtil
	{
		// Token: 0x060001D6 RID: 470 RVA: 0x00006D35 File Offset: 0x00004F35
		public static void GoToStateIfGroupExists(Control control, string groupName, string stateName, bool useTransitions)
		{
			VisualStateManager.GoToState(control, stateName, useTransitions);
		}
	}
}
