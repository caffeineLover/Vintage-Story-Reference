using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000B4 RID: 180
	[Guid("9E365E57-48B2-4160-956F-C7385120BBFC")]
	internal struct IUriRuntimeClassVftbl
	{
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x00011A2C File Offset: 0x0000FC2C
		public method get_RawUri_10
		{
			get
			{
				return this._get_RawUri_10;
			}
		}

		// Token: 0x04000148 RID: 328
		internal IInspectable.Vftbl IInspectableVftbl;

		// Token: 0x04000149 RID: 329
		public IntPtr get_AbsoluteUri_0;

		// Token: 0x0400014A RID: 330
		public IntPtr get_DisplayUri_1;

		// Token: 0x0400014B RID: 331
		public IntPtr get_Domain_2;

		// Token: 0x0400014C RID: 332
		public IntPtr get_Extension_3;

		// Token: 0x0400014D RID: 333
		public IntPtr get_Fragment_4;

		// Token: 0x0400014E RID: 334
		public IntPtr get_Host_5;

		// Token: 0x0400014F RID: 335
		public IntPtr get_Password_6;

		// Token: 0x04000150 RID: 336
		public IntPtr get_Path_7;

		// Token: 0x04000151 RID: 337
		public IntPtr get_Query_8;

		// Token: 0x04000152 RID: 338
		public IntPtr get_QueryParsed_9;

		// Token: 0x04000153 RID: 339
		public unsafe void* _get_RawUri_10;

		// Token: 0x04000154 RID: 340
		public IntPtr get_SchemeName_11;

		// Token: 0x04000155 RID: 341
		public IntPtr get_UserName_12;

		// Token: 0x04000156 RID: 342
		public IntPtr get_Port_13;

		// Token: 0x04000157 RID: 343
		public IntPtr get_Suspicious_14;

		// Token: 0x04000158 RID: 344
		public IntPtr Equals_15;

		// Token: 0x04000159 RID: 345
		public IntPtr CombineUri_16;
	}
}
