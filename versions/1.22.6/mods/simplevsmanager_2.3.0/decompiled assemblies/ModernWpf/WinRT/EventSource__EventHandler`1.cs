using System;
using ABI.System;

namespace WinRT
{
	// Token: 0x020001B6 RID: 438
	internal sealed class EventSource__EventHandler<T> : EventSource<EventHandler<T>>
	{
		// Token: 0x06000E0F RID: 3599 RVA: 0x0002AC81 File Offset: 0x00028E81
		internal EventSource__EventHandler(IObjectReference obj, method addHandler, method removeHandler, int index) : base(obj, addHandler, removeHandler, index)
		{
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0002AC8E File Offset: 0x00028E8E
		protected override ObjectReferenceValue CreateMarshaler(EventHandler<T> del)
		{
			return EventHandler<T>.CreateMarshaler2(del);
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0002AC96 File Offset: 0x00028E96
		protected override State CreateEventState()
		{
			return new EventSource__EventHandler<T>.EventState(this._obj.ThisPtr, this._index);
		}

		// Token: 0x020003FC RID: 1020
		private sealed class EventState : State
		{
			// Token: 0x06001B80 RID: 7040 RVA: 0x00049F96 File Offset: 0x00048196
			public EventState(IntPtr obj, int index) : base(obj, index)
			{
			}

			// Token: 0x06001B81 RID: 7041 RVA: 0x00049FA0 File Offset: 0x000481A0
			protected override Delegate GetEventInvoke()
			{
				return new EventHandler<T>(this.<GetEventInvoke>b__1_0);
			}
		}
	}
}
