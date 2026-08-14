using System;
using ABI.Windows.Foundation.Collections;
using Windows.Foundation.Collections;

namespace WinRT
{
	// Token: 0x020001BD RID: 445
	internal sealed class _EventSource_global__Windows_Foundation_Collections_MapChangedEventHandler_string__string_ : EventSource<Windows.Foundation.Collections.MapChangedEventHandler<string, string>>
	{
		// Token: 0x06000E23 RID: 3619 RVA: 0x0002AF6C File Offset: 0x0002916C
		internal _EventSource_global__Windows_Foundation_Collections_MapChangedEventHandler_string__string_(IObjectReference obj, method addHandler, method removeHandler, int index) : base(obj, addHandler, removeHandler, index)
		{
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x0002AF79 File Offset: 0x00029179
		protected override ObjectReferenceValue CreateMarshaler(Windows.Foundation.Collections.MapChangedEventHandler<string, string> del)
		{
			return ABI.Windows.Foundation.Collections.MapChangedEventHandler<string, string>.CreateMarshaler2(del);
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x0002AF81 File Offset: 0x00029181
		protected override State CreateEventState()
		{
			return new _EventSource_global__Windows_Foundation_Collections_MapChangedEventHandler_string__string_.EventState(this._obj.ThisPtr, this._index);
		}

		// Token: 0x02000400 RID: 1024
		private sealed class EventState : State
		{
			// Token: 0x06001B8C RID: 7052 RVA: 0x0004A091 File Offset: 0x00048291
			public EventState(IntPtr obj, int index) : base(obj, index)
			{
			}

			// Token: 0x06001B8D RID: 7053 RVA: 0x0004A09B File Offset: 0x0004829B
			protected override Delegate GetEventInvoke()
			{
				return delegate(Windows.Foundation.Collections.IObservableMap<string, string> sender, Windows.Foundation.Collections.IMapChangedEventArgs<string> @event)
				{
					Windows.Foundation.Collections.MapChangedEventHandler<string, string> mapChangedEventHandler = (Windows.Foundation.Collections.MapChangedEventHandler<string, string>)this.del;
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
