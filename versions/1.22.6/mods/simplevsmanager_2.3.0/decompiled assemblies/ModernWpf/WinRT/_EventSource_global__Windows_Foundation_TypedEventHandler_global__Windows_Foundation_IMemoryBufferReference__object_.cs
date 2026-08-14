using System;
using ABI.Windows.Foundation;
using Windows.Foundation;

namespace WinRT
{
	// Token: 0x020001C0 RID: 448
	internal sealed class _EventSource_global__Windows_Foundation_TypedEventHandler_global__Windows_Foundation_IMemoryBufferReference__object_ : EventSource<Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>>
	{
		// Token: 0x06000E2C RID: 3628 RVA: 0x0002AFF3 File Offset: 0x000291F3
		internal _EventSource_global__Windows_Foundation_TypedEventHandler_global__Windows_Foundation_IMemoryBufferReference__object_(IObjectReference obj, method addHandler, method removeHandler, int index) : base(obj, addHandler, removeHandler, index)
		{
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x0002B000 File Offset: 0x00029200
		protected override ObjectReferenceValue CreateMarshaler(Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object> del)
		{
			return ABI.Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>.CreateMarshaler2(del);
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x0002B008 File Offset: 0x00029208
		protected override State CreateEventState()
		{
			return new _EventSource_global__Windows_Foundation_TypedEventHandler_global__Windows_Foundation_IMemoryBufferReference__object_.EventState(this._obj.ThisPtr, this._index);
		}

		// Token: 0x02000403 RID: 1027
		private sealed class EventState : State
		{
			// Token: 0x06001B95 RID: 7061 RVA: 0x0004A151 File Offset: 0x00048351
			public EventState(IntPtr obj, int index) : base(obj, index)
			{
			}

			// Token: 0x06001B96 RID: 7062 RVA: 0x0004A15B File Offset: 0x0004835B
			protected override Delegate GetEventInvoke()
			{
				return delegate(Windows.Foundation.IMemoryBufferReference sender, object args)
				{
					Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object> typedEventHandler = (Windows.Foundation.TypedEventHandler<Windows.Foundation.IMemoryBufferReference, object>)this.del;
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
