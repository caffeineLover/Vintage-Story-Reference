using System;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using ModernWpf.Controls;

namespace ModernWpf.Automation.Peers
{
	// Token: 0x02000230 RID: 560
	public class ProgressBarAutomationPeer : RangeBaseAutomationPeer, IRangeValueProvider
	{
		// Token: 0x0600107A RID: 4218 RVA: 0x0002F597 File Offset: 0x0002D797
		public ProgressBarAutomationPeer(ProgressBar owner) : base(owner)
		{
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x0002F5A0 File Offset: 0x0002D7A0
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == 3)
			{
				ProgressBar progressBar = base.Owner as ProgressBar;
				if (progressBar != null && progressBar.IsIndeterminate)
				{
					return null;
				}
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x0002F5D1 File Offset: 0x0002D7D1
		protected override string GetClassNameCore()
		{
			return "ProgressBar";
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x0002F5D8 File Offset: 0x0002D7D8
		protected override string GetNameCore()
		{
			string nameCore = base.GetNameCore();
			ProgressBar progressBar = base.Owner as ProgressBar;
			if (progressBar != null)
			{
				if (progressBar.ShowError)
				{
					return ProgressBarAutomationPeer.ResourceAccessor.GetLocalizedStringResource("ProgressBarErrorStatus");
				}
				if (progressBar.ShowPaused)
				{
					return ProgressBarAutomationPeer.ResourceAccessor.GetLocalizedStringResource("ProgressBarPausedStatus");
				}
				if (progressBar.IsIndeterminate)
				{
					return ProgressBarAutomationPeer.ResourceAccessor.GetLocalizedStringResource("ProgressBarIndeterminateStatus");
				}
			}
			return nameCore;
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x0002F644 File Offset: 0x0002D844
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return 12;
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x0002F648 File Offset: 0x0002D848
		bool IRangeValueProvider.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06001080 RID: 4224 RVA: 0x0002F64B File Offset: 0x0002D84B
		double IRangeValueProvider.SmallChange
		{
			get
			{
				return double.NaN;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06001081 RID: 4225 RVA: 0x0002F656 File Offset: 0x0002D856
		double IRangeValueProvider.LargeChange
		{
			get
			{
				return double.NaN;
			}
		}

		// Token: 0x0400044F RID: 1103
		private static readonly ResourceAccessor ResourceAccessor = new ResourceAccessor(typeof(ProgressBar));
	}
}
