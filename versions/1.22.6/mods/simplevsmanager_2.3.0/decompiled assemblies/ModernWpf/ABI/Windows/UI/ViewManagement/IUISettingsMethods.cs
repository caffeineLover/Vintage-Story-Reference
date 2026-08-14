using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using WinRT;

namespace ABI.Windows.UI.ViewManagement
{
	// Token: 0x0200009B RID: 155
	internal static class IUISettingsMethods
	{
		// Token: 0x06000637 RID: 1591 RVA: 0x0000D820 File Offset: 0x0000BA20
		public unsafe static Windows.UI.Color UIElementColor(IObjectReference _obj, UIElementType desiredElement)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			Windows.UI.Color result = default(Windows.UI.Color);
			method system.Int32_u0020(System.IntPtr,Windows.UI.ViewManagement.UIElementType,Windows.UI.Color&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)18 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.UI.ViewManagement.UIElementType,Windows.UI.Color& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, desiredElement, ref result, system.Int32_u0020(System.IntPtr,Windows.UI.ViewManagement.UIElementType,Windows.UI.Color&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x0000D860 File Offset: 0x0000BA60
		public unsafe static bool get_AnimationsEnabled(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			byte b = 0;
			method system.Int32_u0020(System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)12 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref b, system.Int32_u0020(System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return b > 0;
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0000D89C File Offset: 0x0000BA9C
		public unsafe static uint get_CaretBlinkRate(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			uint result = 0U;
			method system.Int32_u0020(System.IntPtr,System.UInt32&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)14 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,System.UInt32&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0000D8D8 File Offset: 0x0000BAD8
		public unsafe static bool get_CaretBrowsingEnabled(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			byte b = 0;
			method system.Int32_u0020(System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)13 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref b, system.Int32_u0020(System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return b > 0;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0000D914 File Offset: 0x0000BB14
		public unsafe static uint get_CaretWidth(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			uint result = 0U;
			method system.Int32_u0020(System.IntPtr,System.UInt32&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)15 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,System.UInt32&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0000D950 File Offset: 0x0000BB50
		public unsafe static Size get_CursorSize(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			Size result = default(Size);
			method system.Int32_u0020(System.IntPtr,Windows.Foundation.Size&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)7 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.Foundation.Size& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,Windows.Foundation.Size&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x0000D990 File Offset: 0x0000BB90
		public unsafe static uint get_DoubleClickTime(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			uint result = 0U;
			method system.Int32_u0020(System.IntPtr,System.UInt32&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)16 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,System.UInt32&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x0000D9CC File Offset: 0x0000BBCC
		public unsafe static HandPreference get_HandPreference(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			HandPreference result = HandPreference.LeftHanded;
			method system.Int32_u0020(System.IntPtr,Windows.UI.ViewManagement.HandPreference&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.UI.ViewManagement.HandPreference& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,Windows.UI.ViewManagement.HandPreference&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x0000DA04 File Offset: 0x0000BC04
		public unsafe static uint get_MessageDuration(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			uint result = 0U;
			method system.Int32_u0020(System.IntPtr,System.UInt32&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)11 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,System.UInt32&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x0000DA40 File Offset: 0x0000BC40
		public unsafe static uint get_MouseHoverTime(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			uint result = 0U;
			method system.Int32_u0020(System.IntPtr,System.UInt32&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)17 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,System.UInt32&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0000DA7C File Offset: 0x0000BC7C
		public unsafe static Size get_ScrollBarArrowSize(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			Size result = default(Size);
			method system.Int32_u0020(System.IntPtr,Windows.Foundation.Size&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)9 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.Foundation.Size& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,Windows.Foundation.Size&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0000DABC File Offset: 0x0000BCBC
		public unsafe static Size get_ScrollBarSize(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			Size result = default(Size);
			method system.Int32_u0020(System.IntPtr,Windows.Foundation.Size&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)8 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.Foundation.Size& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,Windows.Foundation.Size&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0000DAFC File Offset: 0x0000BCFC
		public unsafe static Size get_ScrollBarThumbBoxSize(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			Size result = default(Size);
			method system.Int32_u0020(System.IntPtr,Windows.Foundation.Size&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)10 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.Foundation.Size& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,Windows.Foundation.Size&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}
	}
}
