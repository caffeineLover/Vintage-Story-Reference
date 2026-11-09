using System;

namespace ModernWpf.Controls
{
	// Token: 0x02000083 RID: 131
	internal class RadioButtonsTestHooks
	{
		// Token: 0x0600068C RID: 1676 RVA: 0x00017EDF File Offset: 0x000160DF
		public static RadioButtonsTestHooks EnsureGlobalTestHooks()
		{
			if (RadioButtonsTestHooks.s_testHooks == null)
			{
				RadioButtonsTestHooks.s_testHooks = new RadioButtonsTestHooks();
			}
			return RadioButtonsTestHooks.s_testHooks;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00017EF7 File Offset: 0x000160F7
		public static void SetTestHooksEnabled(RadioButtons radioButtons, bool enabled)
		{
			if (radioButtons != null)
			{
				radioButtons.SetTestHooksEnabled(enabled);
			}
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00017F03 File Offset: 0x00016103
		public static void NotifyLayoutChanged(RadioButtons sender)
		{
			RadioButtonsTestHooks.EnsureGlobalTestHooks();
			TypedEventHandler<RadioButtons, object> layoutChanged = RadioButtonsTestHooks.LayoutChanged;
			if (layoutChanged == null)
			{
				return;
			}
			layoutChanged.Invoke(sender, null);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00017F1C File Offset: 0x0001611C
		public static int GetRows(RadioButtons radioButtons)
		{
			if (radioButtons != null)
			{
				return radioButtons.GetRows();
			}
			return -1;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00017F29 File Offset: 0x00016129
		public static int GetColumns(RadioButtons radioButtons)
		{
			if (radioButtons != null)
			{
				return radioButtons.GetColumns();
			}
			return -1;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00017F36 File Offset: 0x00016136
		public static int GetLargerColumns(RadioButtons radioButtons)
		{
			if (radioButtons != null)
			{
				return radioButtons.GetLargerColumns();
			}
			return -1;
		}

		// Token: 0x04000301 RID: 769
		public static TypedEventHandler<RadioButtons, object> LayoutChanged;

		// Token: 0x04000302 RID: 770
		private static RadioButtonsTestHooks s_testHooks;
	}
}
