using System;
using WinRT;

namespace ABI.System
{
	// Token: 0x02000119 RID: 281
	internal sealed class EventHandlerEventSource : EventSource<EventHandler>
	{
		// Token: 0x060009C7 RID: 2503 RVA: 0x0001B86C File Offset: 0x00019A6C
		internal EventHandlerEventSource(IObjectReference obj, method addHandler, method removeHandler) : base(obj, addHandler, removeHandler, 0)
		{
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0001B878 File Offset: 0x00019A78
		protected override ObjectReferenceValue CreateMarshaler(EventHandler del)
		{
			return EventHandler.CreateMarshaler2(del);
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0001B880 File Offset: 0x00019A80
		protected override State CreateEventState()
		{
			return new EventHandlerEventSource.EventState(this._obj.ThisPtr, this._index);
		}

		// Token: 0x02000332 RID: 818
		private sealed class EventState : State
		{
			// Token: 0x060017C4 RID: 6084 RVA: 0x00042704 File Offset: 0x00040904
			public EventState(IntPtr obj, int index) : base(obj, index)
			{
			}

			// Token: 0x060017C5 RID: 6085 RVA: 0x0004270E File Offset: 0x0004090E
			protected override Delegate GetEventInvoke()
			{
				return delegate(object obj, EventArgs e)
				{
					EventHandler eventHandler = (EventHandler)this.del;
					if (eventHandler != null)
					{
						eventHandler.Invoke(obj, e);
					}
				};
			}
		}
	}
}
