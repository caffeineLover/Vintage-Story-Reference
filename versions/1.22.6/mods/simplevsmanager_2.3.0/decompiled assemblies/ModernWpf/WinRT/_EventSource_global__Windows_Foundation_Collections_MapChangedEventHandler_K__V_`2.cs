using System;
using ABI.Windows.Foundation.Collections;
using Windows.Foundation.Collections;

namespace WinRT
{
	// Token: 0x020001BC RID: 444
	internal sealed class _EventSource_global__Windows_Foundation_Collections_MapChangedEventHandler_K__V_<K, V> : EventSource<Windows.Foundation.Collections.MapChangedEventHandler<K, V>>
	{
		// Token: 0x06000E20 RID: 3616 RVA: 0x0002AF3F File Offset: 0x0002913F
		internal _EventSource_global__Windows_Foundation_Collections_MapChangedEventHandler_K__V_(IObjectReference obj, method addHandler, method removeHandler, int index) : base(obj, addHandler, removeHandler, index)
		{
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x0002AF4C File Offset: 0x0002914C
		protected override ObjectReferenceValue CreateMarshaler(Windows.Foundation.Collections.MapChangedEventHandler<K, V> del)
		{
			return ABI.Windows.Foundation.Collections.MapChangedEventHandler<K, V>.CreateMarshaler2(del);
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x0002AF54 File Offset: 0x00029154
		protected override State CreateEventState()
		{
			return new _EventSource_global__Windows_Foundation_Collections_MapChangedEventHandler_K__V_<K, V>.EventState(this._obj.ThisPtr, this._index);
		}

		// Token: 0x020003FF RID: 1023
		private sealed class EventState : State
		{
			// Token: 0x06001B89 RID: 7049 RVA: 0x0004A051 File Offset: 0x00048251
			public EventState(IntPtr obj, int index) : base(obj, index)
			{
			}

			// Token: 0x06001B8A RID: 7050 RVA: 0x0004A05B File Offset: 0x0004825B
			protected override Delegate GetEventInvoke()
			{
				return new Windows.Foundation.Collections.MapChangedEventHandler<K, V>(this.<GetEventInvoke>b__1_0);
			}
		}
	}
}
