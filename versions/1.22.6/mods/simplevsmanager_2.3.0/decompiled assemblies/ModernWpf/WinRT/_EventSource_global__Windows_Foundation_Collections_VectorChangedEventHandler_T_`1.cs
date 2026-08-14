using System;
using ABI.Windows.Foundation.Collections;
using Windows.Foundation.Collections;

namespace WinRT
{
	// Token: 0x020001BB RID: 443
	internal sealed class _EventSource_global__Windows_Foundation_Collections_VectorChangedEventHandler_T_<T> : EventSource<Windows.Foundation.Collections.VectorChangedEventHandler<T>>
	{
		// Token: 0x06000E1D RID: 3613 RVA: 0x0002AF12 File Offset: 0x00029112
		internal _EventSource_global__Windows_Foundation_Collections_VectorChangedEventHandler_T_(IObjectReference obj, method addHandler, method removeHandler, int index) : base(obj, addHandler, removeHandler, index)
		{
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x0002AF1F File Offset: 0x0002911F
		protected override ObjectReferenceValue CreateMarshaler(Windows.Foundation.Collections.VectorChangedEventHandler<T> del)
		{
			return ABI.Windows.Foundation.Collections.VectorChangedEventHandler<T>.CreateMarshaler2(del);
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x0002AF27 File Offset: 0x00029127
		protected override State CreateEventState()
		{
			return new _EventSource_global__Windows_Foundation_Collections_VectorChangedEventHandler_T_<T>.EventState(this._obj.ThisPtr, this._index);
		}

		// Token: 0x020003FE RID: 1022
		private sealed class EventState : State
		{
			// Token: 0x06001B86 RID: 7046 RVA: 0x0004A011 File Offset: 0x00048211
			public EventState(IntPtr obj, int index) : base(obj, index)
			{
			}

			// Token: 0x06001B87 RID: 7047 RVA: 0x0004A01B File Offset: 0x0004821B
			protected override Delegate GetEventInvoke()
			{
				return new Windows.Foundation.Collections.VectorChangedEventHandler<T>(this.<GetEventInvoke>b__1_0);
			}
		}
	}
}
