using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using WinRT;

namespace ABI.Windows.UI.ViewManagement
{
	// Token: 0x020000A3 RID: 163
	internal static class IUISettings5Methods
	{
		// Token: 0x06000670 RID: 1648 RVA: 0x0000E810 File Offset: 0x0000CA10
		public unsafe static bool get_AutoHideScrollBars(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			byte b = 0;
			method system.Int32_u0020(System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref b, system.Int32_u0020(System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return b > 0;
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0000E84B File Offset: 0x0000CA4B
		private static ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>>> MakeAutoHideScrollBarsChangedTable()
		{
			Interlocked.CompareExchange<ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>>>>(ref IUISettings5Methods._AutoHideScrollBarsChanged_, new ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>>>(), null);
			return IUISettings5Methods._AutoHideScrollBarsChanged_;
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x0000E865 File Offset: 0x0000CA65
		private static ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>>> _AutoHideScrollBarsChanged
		{
			get
			{
				return IUISettings5Methods._AutoHideScrollBarsChanged_ ?? IUISettings5Methods.MakeAutoHideScrollBarsChangedTable();
			}
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0000E878 File Offset: 0x0000CA78
		public unsafe static ValueTuple<Action<TypedEventHandler<Windows.UI.ViewManagement.UISettings, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>>, Action<TypedEventHandler<Windows.UI.ViewManagement.UISettings, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>>> Get_AutoHideScrollBarsChanged(IObjectReference _obj, object _thisObj)
		{
			return IUISettings5Methods._AutoHideScrollBarsChanged.GetValue(_thisObj, delegate([Nullable(1)] object key)
			{
				IntPtr thisPtr = _obj.ThisPtr;
				return new _EventSource_global__Windows_Foundation_TypedEventHandler_global__Windows_UI_ViewManagement_UISettings__global__Windows_UI_ViewManagement_UISettingsAutoHideScrollBarsChangedEventArgs_(_obj, *(*(IntPtr*)((void*)thisPtr) + (IntPtr)7 * (IntPtr)sizeof(method)), *(*(IntPtr*)((void*)thisPtr) + (IntPtr)8 * (IntPtr)sizeof(method)), 0);
			}).EventActions;
		}

		// Token: 0x04000132 RID: 306
		private static volatile ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, Windows.UI.ViewManagement.UISettingsAutoHideScrollBarsChangedEventArgs>>> _AutoHideScrollBarsChanged_;
	}
}
