using System;

namespace ModernWpf.Controls
{
	// Token: 0x02000077 RID: 119
	public class NumberBoxValueChangedEventArgs : EventArgs
	{
		// Token: 0x060005A1 RID: 1441 RVA: 0x0001457E File Offset: 0x0001277E
		public NumberBoxValueChangedEventArgs(double oldValue, double newValue)
		{
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x00014594 File Offset: 0x00012794
		public double OldValue { get; }

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x0001459C File Offset: 0x0001279C
		public double NewValue { get; }
	}
}
