using System;
using System.Windows;

namespace ModernWpf.Controls.Primitives
{
	// Token: 0x02000281 RID: 641
	public class BindingProxy : Freezable
	{
		// Token: 0x170003AF RID: 943
		// (get) Token: 0x060012C1 RID: 4801 RVA: 0x00037064 File Offset: 0x00035264
		// (set) Token: 0x060012C2 RID: 4802 RVA: 0x00037071 File Offset: 0x00035271
		public object Value
		{
			get
			{
				return base.GetValue(BindingProxy.ValueProperty);
			}
			set
			{
				base.SetValue(BindingProxy.ValueProperty, value);
			}
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x0003707F File Offset: 0x0003527F
		protected override Freezable CreateInstanceCore()
		{
			return new BindingProxy();
		}

		// Token: 0x04000623 RID: 1571
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(BindingProxy));
	}
}
