using System;
using System.ComponentModel;
using System.Windows;

namespace ModernWpf.DesignTime
{
	// Token: 0x02000262 RID: 610
	public abstract class IntellisenseResourcesBase : ResourceDictionary, ISupportInitialize
	{
		// Token: 0x17000365 RID: 869
		// (get) Token: 0x0600115E RID: 4446 RVA: 0x00033204 File Offset: 0x00031404
		// (set) Token: 0x0600115F RID: 4447 RVA: 0x0003320C File Offset: 0x0003140C
		public Uri Source
		{
			get
			{
				return base.Source;
			}
			set
			{
				if (DesignMode.DesignModeEnabled)
				{
					base.Source = value;
				}
			}
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x0003321C File Offset: 0x0003141C
		public void EndInit()
		{
			base.Clear();
			base.MergedDictionaries.Clear();
			base.EndInit();
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x00033235 File Offset: 0x00031435
		void ISupportInitialize.EndInit()
		{
			this.EndInit();
		}
	}
}
