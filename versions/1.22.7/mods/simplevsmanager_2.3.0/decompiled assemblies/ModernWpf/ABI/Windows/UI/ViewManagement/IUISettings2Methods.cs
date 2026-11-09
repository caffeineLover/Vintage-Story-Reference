using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using WinRT;

namespace ABI.Windows.UI.ViewManagement
{
	// Token: 0x0200009D RID: 157
	internal static class IUISettings2Methods
	{
		// Token: 0x06000652 RID: 1618 RVA: 0x0000E0C0 File Offset: 0x0000C2C0
		public unsafe static double get_TextScaleFactor(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			double result = 0.0;
			method system.Int32_u0020(System.IntPtr,System.Double&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Double& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref result, system.Int32_u0020(System.IntPtr,System.Double&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x0000E100 File Offset: 0x0000C300
		private static ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> MakeTextScaleFactorChangedTable()
		{
			Interlocked.CompareExchange<ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>>>(ref IUISettings2Methods._TextScaleFactorChanged_, new ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>>(), null);
			return IUISettings2Methods._TextScaleFactorChanged_;
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x0000E11A File Offset: 0x0000C31A
		private static ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> _TextScaleFactorChanged
		{
			get
			{
				return IUISettings2Methods._TextScaleFactorChanged_ ?? IUISettings2Methods.MakeTextScaleFactorChangedTable();
			}
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0000E12C File Offset: 0x0000C32C
		public unsafe static ValueTuple<Action<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>, Action<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> Get_TextScaleFactorChanged(IObjectReference _obj, object _thisObj)
		{
			return IUISettings2Methods._TextScaleFactorChanged.GetValue(_thisObj, delegate([Nullable(1)] object key)
			{
				IntPtr thisPtr = _obj.ThisPtr;
				return new _EventSource_global__Windows_Foundation_TypedEventHandler_global__Windows_UI_ViewManagement_UISettings__object_(_obj, *(*(IntPtr*)((void*)thisPtr) + (IntPtr)7 * (IntPtr)sizeof(method)), *(*(IntPtr*)((void*)thisPtr) + (IntPtr)8 * (IntPtr)sizeof(method)), 0);
			}).EventActions;
		}

		// Token: 0x04000129 RID: 297
		private static volatile ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> _TextScaleFactorChanged_;
	}
}
