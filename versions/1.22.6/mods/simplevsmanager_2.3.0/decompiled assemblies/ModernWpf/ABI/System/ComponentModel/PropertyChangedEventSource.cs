using System;
using System.ComponentModel;
using WinRT;

namespace ABI.System.ComponentModel
{
	// Token: 0x0200014B RID: 331
	internal sealed class PropertyChangedEventSource : EventSource<PropertyChangedEventHandler>
	{
		// Token: 0x06000A9A RID: 2714 RVA: 0x0001D8F8 File Offset: 0x0001BAF8
		internal PropertyChangedEventSource(IObjectReference obj, method addHandler, method removeHandler) : base(obj, addHandler, removeHandler, 0)
		{
		}

		// Token: 0x06000A9B RID: 2715 RVA: 0x0001D904 File Offset: 0x0001BB04
		protected override ObjectReferenceValue CreateMarshaler(PropertyChangedEventHandler del)
		{
			return PropertyChangedEventHandler.CreateMarshaler2(del);
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x0001D90C File Offset: 0x0001BB0C
		protected override State CreateEventState()
		{
			return new PropertyChangedEventSource.EventState(this._obj.ThisPtr, this._index);
		}

		// Token: 0x02000363 RID: 867
		private sealed class EventState : State
		{
			// Token: 0x06001878 RID: 6264 RVA: 0x000441E8 File Offset: 0x000423E8
			public EventState(IntPtr obj, int index) : base(obj, index)
			{
			}

			// Token: 0x06001879 RID: 6265 RVA: 0x000441F2 File Offset: 0x000423F2
			protected override Delegate GetEventInvoke()
			{
				return delegate(object obj, PropertyChangedEventArgs e)
				{
					PropertyChangedEventHandler propertyChangedEventHandler = (PropertyChangedEventHandler)this.del;
					if (propertyChangedEventHandler != null)
					{
						propertyChangedEventHandler.Invoke(obj, e);
					}
				};
			}
		}
	}
}
