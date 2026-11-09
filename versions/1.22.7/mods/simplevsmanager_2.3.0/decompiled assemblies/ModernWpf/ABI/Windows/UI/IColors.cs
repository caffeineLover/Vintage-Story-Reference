using System;
using System.Runtime.InteropServices;
using Windows.UI;
using WinRT;

namespace ABI.Windows.UI
{
	// Token: 0x02000097 RID: 151
	[Guid("9B8C9326-4CA6-4CE5-8994-9EFF65CABDCC")]
	internal interface IColors : Windows.UI.IColors
	{
		// Token: 0x0600051A RID: 1306 RVA: 0x00007627 File Offset: 0x00005827
		unsafe static IColors()
		{
			*(IInspectable.Vftbl*)((void*)IColors.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
		}

		// Token: 0x04000126 RID: 294
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IColors), sizeof(IInspectable.Vftbl) + 0);
	}
}
