using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x0200001F RID: 31
	internal class AppBarElementVisualStateManager : VisualStateManager
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600013D RID: 317 RVA: 0x000054D9 File Offset: 0x000036D9
		// (set) Token: 0x0600013E RID: 318 RVA: 0x000054E1 File Offset: 0x000036E1
		internal bool CanChangeCommonState { get; set; }

		// Token: 0x0600013F RID: 319 RVA: 0x000054EA File Offset: 0x000036EA
		protected override bool GoToStateCore(FrameworkElement control, FrameworkElement stateGroupsRoot, string stateName, VisualStateGroup group, VisualState state, bool useTransitions)
		{
			return state != null && (group.Name != "CommonStates" || this.CanChangeCommonState) && base.GoToStateCore(control, stateGroupsRoot, stateName, group, state, useTransitions);
		}
	}
}
