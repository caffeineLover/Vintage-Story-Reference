using System;
using System.Runtime.InteropServices;
using Windows.UI.ViewManagement;
using WinRT;

namespace ABI.Windows.UI.ViewManagement
{
	// Token: 0x020000A6 RID: 166
	[Guid("87AFD4B2-9146-5F02-8F6B-06D454174C0F")]
	internal interface IUISettingsAutoHideScrollBarsChangedEventArgs : Windows.UI.ViewManagement.IUISettingsAutoHideScrollBarsChangedEventArgs
	{
		// Token: 0x0600067A RID: 1658 RVA: 0x0000EA78 File Offset: 0x0000CC78
		unsafe static IUISettingsAutoHideScrollBarsChangedEventArgs()
		{
			*(IInspectable.Vftbl*)((void*)IUISettingsAutoHideScrollBarsChangedEventArgs.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
		}

		// Token: 0x04000135 RID: 309
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IUISettingsAutoHideScrollBarsChangedEventArgs), sizeof(IInspectable.Vftbl) + 0);
	}
}
