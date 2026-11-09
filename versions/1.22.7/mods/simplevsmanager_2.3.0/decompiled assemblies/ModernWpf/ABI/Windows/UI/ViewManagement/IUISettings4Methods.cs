using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using WinRT;

namespace ABI.Windows.UI.ViewManagement
{
	// Token: 0x020000A1 RID: 161
	internal static class IUISettings4Methods
	{
		// Token: 0x06000666 RID: 1638 RVA: 0x0000E5A8 File Offset: 0x0000C7A8
		public unsafe static bool get_AdvancedEffectsEnabled(IObjectReference _obj)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			byte b = 0;
			method system.Int32_u0020(System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Byte& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, ref b, system.Int32_u0020(System.IntPtr,System.Byte&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return b > 0;
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0000E5E3 File Offset: 0x0000C7E3
		private static ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> MakeAdvancedEffectsEnabledChangedTable()
		{
			Interlocked.CompareExchange<ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>>>(ref IUISettings4Methods._AdvancedEffectsEnabledChanged_, new ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>>(), null);
			return IUISettings4Methods._AdvancedEffectsEnabledChanged_;
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x0000E5FD File Offset: 0x0000C7FD
		private static ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> _AdvancedEffectsEnabledChanged
		{
			get
			{
				return IUISettings4Methods._AdvancedEffectsEnabledChanged_ ?? IUISettings4Methods.MakeAdvancedEffectsEnabledChangedTable();
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0000E610 File Offset: 0x0000C810
		public unsafe static ValueTuple<Action<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>, Action<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> Get_AdvancedEffectsEnabledChanged(IObjectReference _obj, object _thisObj)
		{
			return IUISettings4Methods._AdvancedEffectsEnabledChanged.GetValue(_thisObj, delegate([Nullable(1)] object key)
			{
				IntPtr thisPtr = _obj.ThisPtr;
				return new _EventSource_global__Windows_Foundation_TypedEventHandler_global__Windows_UI_ViewManagement_UISettings__object_(_obj, *(*(IntPtr*)((void*)thisPtr) + (IntPtr)7 * (IntPtr)sizeof(method)), *(*(IntPtr*)((void*)thisPtr) + (IntPtr)8 * (IntPtr)sizeof(method)), 0);
			}).EventActions;
		}

		// Token: 0x0400012F RID: 303
		private static volatile ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> _AdvancedEffectsEnabledChanged_;
	}
}
