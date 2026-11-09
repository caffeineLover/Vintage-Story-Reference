using System;

namespace WinRT
{
	// Token: 0x020001B5 RID: 437
	internal abstract class EventSource<TDelegate> where TDelegate : class, MulticastDelegate
	{
		// Token: 0x06000E08 RID: 3592 RVA: 0x0002AA40 File Offset: 0x00028C40
		protected EventSource(IObjectReference obj, method addHandler, method removeHandler, int index = 0)
		{
			this._obj = obj;
			this._addHandler = addHandler;
			this._removeHandler = removeHandler;
			this._index = index;
			this._state = Cache.GetState(obj, index);
			this._handlerTuple = new ValueTuple<Action<TDelegate>, Action<TDelegate>>(new Action<TDelegate>(this.Subscribe), new Action<TDelegate>(this.Unsubscribe));
		}

		// Token: 0x06000E09 RID: 3593
		protected abstract ObjectReferenceValue CreateMarshaler(TDelegate del);

		// Token: 0x06000E0A RID: 3594
		protected abstract State CreateEventState();

		// Token: 0x06000E0B RID: 3595 RVA: 0x0002AAA4 File Offset: 0x00028CA4
		public void Subscribe(TDelegate del)
		{
			lock (this)
			{
				State state = null;
				bool flag2 = this._state == null || !this._state.TryGetTarget(ref state) || !state.HasComReferences();
				if (flag2)
				{
					state = this.CreateEventState();
					this._state = state.GetWeakReferenceForCache();
					Cache.Create(this._obj, this._index, this._state);
				}
				state.del = (TDelegate)((object)Delegate.Combine(state.del, del));
				if (flag2)
				{
					TDelegate tdelegate = (TDelegate)((object)state.eventInvoke);
					using (ObjectReferenceValue objectReferenceValue = this.CreateMarshaler(tdelegate))
					{
						IntPtr abi = objectReferenceValue.GetAbi();
						state.InitalizeReferenceTracking(abi);
						method addHandler = this._addHandler;
						ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,WinRT.EventRegistrationToken& modreq(System.Runtime.InteropServices.OutAttribute)), this._obj.ThisPtr, abi, ref state.token, addHandler));
					}
				}
			}
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0002ABB4 File Offset: 0x00028DB4
		public void Unsubscribe(TDelegate del)
		{
			State state;
			if (this._state == null || !this._state.TryGetTarget(ref state))
			{
				return;
			}
			lock (this)
			{
				bool del2 = state.del != null;
				state.del = (TDelegate)((object)Delegate.Remove(state.del, del));
				if (del2 && state.del == null)
				{
					this.UnsubscribeFromNative(state);
				}
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000E0D RID: 3597 RVA: 0x0002AC3C File Offset: 0x00028E3C
		public ValueTuple<Action<TDelegate>, Action<TDelegate>> EventActions
		{
			get
			{
				return this._handlerTuple;
			}
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0002AC44 File Offset: 0x00028E44
		private void UnsubscribeFromNative(State state)
		{
			method removeHandler = this._removeHandler;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,WinRT.EventRegistrationToken), this._obj.ThisPtr, state.token, removeHandler));
			state.Dispose();
			this._state = null;
		}

		// Token: 0x040002BC RID: 700
		protected readonly IObjectReference _obj;

		// Token: 0x040002BD RID: 701
		protected readonly int _index;

		// Token: 0x040002BE RID: 702
		private readonly method _addHandler;

		// Token: 0x040002BF RID: 703
		private readonly method _removeHandler;

		// Token: 0x040002C0 RID: 704
		protected WeakReference<State> _state;

		// Token: 0x040002C1 RID: 705
		private readonly ValueTuple<Action<TDelegate>, Action<TDelegate>> _handlerTuple;
	}
}
