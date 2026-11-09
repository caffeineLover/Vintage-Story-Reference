using System;
using System.Collections.Specialized;
using WinRT;

namespace ABI.System.Collections.Specialized
{
	// Token: 0x02000154 RID: 340
	internal sealed class NotifyCollectionChangedEventSource : EventSource<NotifyCollectionChangedEventHandler>
	{
		// Token: 0x06000AD9 RID: 2777 RVA: 0x0001E3C8 File Offset: 0x0001C5C8
		internal NotifyCollectionChangedEventSource(IObjectReference obj, method addHandler, method removeHandler) : base(obj, addHandler, removeHandler, 0)
		{
		}

		// Token: 0x06000ADA RID: 2778 RVA: 0x0001E3D4 File Offset: 0x0001C5D4
		protected override ObjectReferenceValue CreateMarshaler(NotifyCollectionChangedEventHandler del)
		{
			return NotifyCollectionChangedEventHandler.CreateMarshaler2(del);
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0001E3DC File Offset: 0x0001C5DC
		protected override State CreateEventState()
		{
			return new NotifyCollectionChangedEventSource.EventState(this._obj.ThisPtr, this._index);
		}

		// Token: 0x0200037C RID: 892
		private sealed class EventState : State
		{
			// Token: 0x06001911 RID: 6417 RVA: 0x0004517C File Offset: 0x0004337C
			public EventState(IntPtr obj, int index) : base(obj, index)
			{
			}

			// Token: 0x06001912 RID: 6418 RVA: 0x00045186 File Offset: 0x00043386
			protected override Delegate GetEventInvoke()
			{
				return delegate(object obj, NotifyCollectionChangedEventArgs e)
				{
					NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler = (NotifyCollectionChangedEventHandler)this.del;
					if (notifyCollectionChangedEventHandler != null)
					{
						notifyCollectionChangedEventHandler.Invoke(obj, e);
					}
				};
			}
		}
	}
}
