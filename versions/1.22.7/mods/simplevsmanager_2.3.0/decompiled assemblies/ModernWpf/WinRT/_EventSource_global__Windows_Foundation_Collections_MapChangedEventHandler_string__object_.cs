using System;
using ABI.Windows.Foundation.Collections;
using Windows.Foundation.Collections;

namespace WinRT
{
	// Token: 0x020001BF RID: 447
	internal sealed class _EventSource_global__Windows_Foundation_Collections_MapChangedEventHandler_string__object_ : EventSource<Windows.Foundation.Collections.MapChangedEventHandler<string, object>>
	{
		// Token: 0x06000E29 RID: 3625 RVA: 0x0002AFC6 File Offset: 0x000291C6
		internal _EventSource_global__Windows_Foundation_Collections_MapChangedEventHandler_string__object_(IObjectReference obj, method addHandler, method removeHandler, int index) : base(obj, addHandler, removeHandler, index)
		{
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x0002AFD3 File Offset: 0x000291D3
		protected override ObjectReferenceValue CreateMarshaler(Windows.Foundation.Collections.MapChangedEventHandler<string, object> del)
		{
			return ABI.Windows.Foundation.Collections.MapChangedEventHandler<string, object>.CreateMarshaler2(del);
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x0002AFDB File Offset: 0x000291DB
		protected override State CreateEventState()
		{
			return new _EventSource_global__Windows_Foundation_Collections_MapChangedEventHandler_string__object_.EventState(this._obj.ThisPtr, this._index);
		}

		// Token: 0x02000402 RID: 1026
		private sealed class EventState : State
		{
			// Token: 0x06001B92 RID: 7058 RVA: 0x0004A111 File Offset: 0x00048311
			public EventState(IntPtr obj, int index) : base(obj, index)
			{
			}

			// Token: 0x06001B93 RID: 7059 RVA: 0x0004A11B File Offset: 0x0004831B
			protected override Delegate GetEventInvoke()
			{
				return delegate(Windows.Foundation.Collections.IObservableMap<string, object> sender, Windows.Foundation.Collections.IMapChangedEventArgs<string> @event)
				{
					Windows.Foundation.Collections.MapChangedEventHandler<string, object> mapChangedEventHandler = (Windows.Foundation.Collections.MapChangedEventHandler<string, object>)this.del;
					if (mapChangedEventHandler == null)
					{
						return;
					}
					mapChangedEventHandler.Invoke(sender, @event);
				};
			}
		}
	}
}
