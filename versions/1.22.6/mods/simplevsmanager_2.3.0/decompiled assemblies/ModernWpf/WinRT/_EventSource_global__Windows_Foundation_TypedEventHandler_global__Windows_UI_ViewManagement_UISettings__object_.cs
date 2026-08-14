using System;
using ABI.Windows.Foundation;
using Windows.Foundation;
using Windows.UI.ViewManagement;

namespace WinRT
{
	// Token: 0x020001BA RID: 442
	internal sealed class _EventSource_global__Windows_Foundation_TypedEventHandler_global__Windows_UI_ViewManagement_UISettings__object_ : EventSource<Windows.Foundation.TypedEventHandler<UISettings, object>>
	{
		// Token: 0x06000E1A RID: 3610 RVA: 0x0002AEE5 File Offset: 0x000290E5
		internal _EventSource_global__Windows_Foundation_TypedEventHandler_global__Windows_UI_ViewManagement_UISettings__object_(IObjectReference obj, method addHandler, method removeHandler, int index) : base(obj, addHandler, removeHandler, index)
		{
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x0002AEF2 File Offset: 0x000290F2
		protected override ObjectReferenceValue CreateMarshaler(Windows.Foundation.TypedEventHandler<UISettings, object> del)
		{
			return ABI.Windows.Foundation.TypedEventHandler<UISettings, object>.CreateMarshaler2(del);
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x0002AEFA File Offset: 0x000290FA
		protected override State CreateEventState()
		{
			return new _EventSource_global__Windows_Foundation_TypedEventHandler_global__Windows_UI_ViewManagement_UISettings__object_.EventState(this._obj.ThisPtr, this._index);
		}

		// Token: 0x020003FD RID: 1021
		private sealed class EventState : State
		{
			// Token: 0x06001B83 RID: 7043 RVA: 0x00049FD4 File Offset: 0x000481D4
			public EventState(IntPtr obj, int index) : base(obj, index)
			{
			}

			// Token: 0x06001B84 RID: 7044 RVA: 0x00049FDE File Offset: 0x000481DE
			protected override Delegate GetEventInvoke()
			{
				return delegate(UISettings sender, object args)
				{
					Windows.Foundation.TypedEventHandler<UISettings, object> typedEventHandler = (Windows.Foundation.TypedEventHandler<UISettings, object>)this.del;
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
