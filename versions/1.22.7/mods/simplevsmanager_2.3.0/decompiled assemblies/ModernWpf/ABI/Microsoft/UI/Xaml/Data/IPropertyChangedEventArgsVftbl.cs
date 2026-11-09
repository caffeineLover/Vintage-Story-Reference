using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.Microsoft.UI.Xaml.Data
{
	// Token: 0x02000171 RID: 369
	[Guid("63D0C952-396B-54F4-AF8C-BA8724A427BF")]
	internal struct IPropertyChangedEventArgsVftbl
	{
		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x00020F00 File Offset: 0x0001F100
		public method get_PropertyName_0
		{
			get
			{
				return this._get_PropertyName_0;
			}
		}

		// Token: 0x04000204 RID: 516
		internal IInspectable.Vftbl IInspectableVftbl;

		// Token: 0x04000205 RID: 517
		private unsafe void* _get_PropertyName_0;
	}
}
