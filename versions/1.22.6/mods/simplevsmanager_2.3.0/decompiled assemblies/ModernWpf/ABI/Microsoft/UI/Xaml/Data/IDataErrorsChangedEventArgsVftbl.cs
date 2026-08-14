using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.Microsoft.UI.Xaml.Data
{
	// Token: 0x0200016A RID: 362
	[Guid("D026DD64-5F26-5F15-A86A-0DEC8A431796")]
	internal struct IDataErrorsChangedEventArgsVftbl
	{
		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x00020AD0 File Offset: 0x0001ECD0
		public method get_PropertyName_0
		{
			get
			{
				return this._get_PropertyName_0;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x00020AD8 File Offset: 0x0001ECD8
		public method put_PropertyName_1
		{
			get
			{
				return this._put_PropertyName_1;
			}
		}

		// Token: 0x040001F7 RID: 503
		internal IInspectable.Vftbl IInspectableVftbl;

		// Token: 0x040001F8 RID: 504
		private unsafe void* _get_PropertyName_0;

		// Token: 0x040001F9 RID: 505
		private unsafe void* _put_PropertyName_1;
	}
}
