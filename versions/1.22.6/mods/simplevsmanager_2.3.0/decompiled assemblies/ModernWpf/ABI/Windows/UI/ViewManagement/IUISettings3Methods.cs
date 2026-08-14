using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using WinRT;

namespace ABI.Windows.UI.ViewManagement
{
	// Token: 0x0200009F RID: 159
	internal static class IUISettings3Methods
	{
		// Token: 0x0600065C RID: 1628 RVA: 0x0000E334 File Offset: 0x0000C534
		public unsafe static Windows.UI.Color GetColorValue(IObjectReference _obj, UIColorType desiredColor)
		{
			IntPtr thisPtr = _obj.ThisPtr;
			Windows.UI.Color result = default(Windows.UI.Color);
			method system.Int32_u0020(System.IntPtr,Windows.UI.ViewManagement.UIColorType,Windows.UI.Color&_u0020modreq(System.Runtime.InteropServices.OutAttribute)) = *(*(IntPtr*)((void*)thisPtr) + (IntPtr)6 * (IntPtr)sizeof(method));
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,Windows.UI.ViewManagement.UIColorType,Windows.UI.Color& modreq(System.Runtime.InteropServices.OutAttribute)), thisPtr, desiredColor, ref result, system.Int32_u0020(System.IntPtr,Windows.UI.ViewManagement.UIColorType,Windows.UI.Color&_u0020modreq(System.Runtime.InteropServices.OutAttribute))));
			return result;
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x0000E373 File Offset: 0x0000C573
		private static ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> MakeColorValuesChangedTable()
		{
			Interlocked.CompareExchange<ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>>>(ref IUISettings3Methods._ColorValuesChanged_, new ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>>(), null);
			return IUISettings3Methods._ColorValuesChanged_;
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x0000E38D File Offset: 0x0000C58D
		private static ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> _ColorValuesChanged
		{
			get
			{
				return IUISettings3Methods._ColorValuesChanged_ ?? IUISettings3Methods.MakeColorValuesChangedTable();
			}
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0000E3A0 File Offset: 0x0000C5A0
		public unsafe static ValueTuple<Action<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>, Action<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> Get_ColorValuesChanged(IObjectReference _obj, object _thisObj)
		{
			return IUISettings3Methods._ColorValuesChanged.GetValue(_thisObj, delegate([Nullable(1)] object key)
			{
				IntPtr thisPtr = _obj.ThisPtr;
				return new _EventSource_global__Windows_Foundation_TypedEventHandler_global__Windows_UI_ViewManagement_UISettings__object_(_obj, *(*(IntPtr*)((void*)thisPtr) + (IntPtr)7 * (IntPtr)sizeof(method)), *(*(IntPtr*)((void*)thisPtr) + (IntPtr)8 * (IntPtr)sizeof(method)), 0);
			}).EventActions;
		}

		// Token: 0x0400012C RID: 300
		private static volatile ConditionalWeakTable<object, EventSource<TypedEventHandler<Windows.UI.ViewManagement.UISettings, object>>> _ColorValuesChanged_;
	}
}
