using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace VintageStoryModManager.Helpers
{
	// Token: 0x020000C4 RID: 196
	[NullableContext(1)]
	[Nullable(0)]
	public class BindingProxy : Freezable
	{
		// Token: 0x06000FFD RID: 4093 RVA: 0x00054CB8 File Offset: 0x00052EB8
		protected override Freezable CreateInstanceCore()
		{
			return new BindingProxy();
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x06000FFE RID: 4094 RVA: 0x00054CBF File Offset: 0x00052EBF
		// (set) Token: 0x06000FFF RID: 4095 RVA: 0x00054CCC File Offset: 0x00052ECC
		public object Data
		{
			get
			{
				return base.GetValue(BindingProxy.DataProperty);
			}
			set
			{
				base.SetValue(BindingProxy.DataProperty, value);
			}
		}

		// Token: 0x04000701 RID: 1793
		public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new PropertyMetadata(null));
	}
}
