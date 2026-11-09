using System;
using ABI.Windows.Foundation;
using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace WinRT
{
	// Token: 0x020001BE RID: 446
	internal sealed class _EventSource_global__Windows_Foundation_TypedEventHandler_global__Windows_UI_ViewManagement_UISettings__global__Windows_UI_ViewManagement_UISettingsAutoHideScrollBarsChangedEventArgs_ : EventSource<Windows.Foundation.TypedEventHandler<UISettings, UISettingsAutoHideScrollBarsChangedEventArgs>>
	{
		// Token: 0x06000E26 RID: 3622 RVA: 0x0002AF99 File Offset: 0x00029199
		internal _EventSource_global__Windows_Foundation_TypedEventHandler_global__Windows_UI_ViewManagement_UISettings__global__Windows_UI_ViewManagement_UISettingsAutoHideScrollBarsChangedEventArgs_(IObjectReference obj, method addHandler, method removeHandler, int index) : base(obj, addHandler, removeHandler, index)
		{
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x0002AFA6 File Offset: 0x000291A6
		protected override ObjectReferenceValue CreateMarshaler(Windows.Foundation.TypedEventHandler<UISettings, UISettingsAutoHideScrollBarsChangedEventArgs> del)
		{
			return ABI.Windows.Foundation.TypedEventHandler<UISettings, UISettingsAutoHideScrollBarsChangedEventArgs>.CreateMarshaler2(del);
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x0002AFAE File Offset: 0x000291AE
		protected override State CreateEventState()
		{
			return new _EventSource_global__Windows_Foundation_TypedEventHandler_global__Windows_UI_ViewManagement_UISettings__global__Windows_UI_ViewManagement_UISettingsAutoHideScrollBarsChangedEventArgs_.EventState(this._obj.ThisPtr, this._index);
		}

		// Token: 0x02000401 RID: 1025
		private sealed class EventState : State
		{
			// Token: 0x06001B8F RID: 7055 RVA: 0x0004A0D1 File Offset: 0x000482D1
			public EventState(IntPtr obj, int index) : base(obj, index)
			{
			}

			// Token: 0x06001B90 RID: 7056 RVA: 0x0004A0DB File Offset: 0x000482DB
			protected override Delegate GetEventInvoke()
			{
				return delegate(UISettings sender, UISettingsAutoHideScrollBarsChangedEventArgs args)
				{
					Windows.Foundation.TypedEventHandler<UISettings, UISettingsAutoHideScrollBarsChangedEventArgs> typedEventHandler = (Windows.Foundation.TypedEventHandler<UISettings, UISettingsAutoHideScrollBarsChangedEventArgs>)this.del;
					if (typedEventHandler == null)
					{
						return;
					}
					typedEventHandler.Invoke(sender, args);
				};
			}
		}
	}
}
