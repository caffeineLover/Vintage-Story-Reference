using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using VSImGui.API;

namespace VSImGui
{
	// Token: 0x02000009 RID: 9
	[NullableContext(1)]
	[Nullable(0)]
	internal class DrawCallbacksManager
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000055 RID: 85 RVA: 0x00002DAC File Offset: 0x00000FAC
		// (remove) Token: 0x06000056 RID: 86 RVA: 0x00002DE8 File Offset: 0x00000FE8
		public event DrawCallbackDelegate DrawCallback
		{
			add
			{
				this._callbacks.Add(this._nextId, value);
				Dictionary<int, bool> wasClosed = this._wasClosed;
				int nextId = this._nextId;
				this._nextId = nextId + 1;
				wasClosed.Add(nextId, false);
			}
			remove
			{
				int id = Enumerable.First<KeyValuePair<int, DrawCallbackDelegate>>(this._callbacks, (KeyValuePair<int, DrawCallbackDelegate> entry) => entry.Value == value).Key;
				this._callbacks.Remove(id);
				this._wasClosed.Remove(id);
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002E3C File Offset: 0x0000103C
		[NullableContext(0)]
		[return: TupleElementNames(new string[]
		{
			"open",
			"grab",
			"close"
		})]
		public ValueTuple<bool, bool, bool> Draw(float deltaSeconds)
		{
			bool open = false;
			bool grab = false;
			bool close = false;
			bool anyOpened = false;
			foreach (KeyValuePair<int, DrawCallbackDelegate> keyValuePair in this._callbacks)
			{
				int num;
				DrawCallbackDelegate drawCallbackDelegate;
				keyValuePair.Deconstruct(ref num, ref drawCallbackDelegate);
				int id = num;
				switch (drawCallbackDelegate(deltaSeconds))
				{
				case CallbackGUIStatus.Closed:
					if (!this._wasClosed[id])
					{
						this._wasClosed[id] = true;
						close = true;
					}
					break;
				case CallbackGUIStatus.GrabMouse:
					if (this._wasClosed[id])
					{
						open = true;
						this._wasClosed[id] = false;
					}
					grab = true;
					anyOpened = true;
					break;
				case CallbackGUIStatus.DontGrabMouse:
					if (this._wasClosed[id])
					{
						open = true;
						this._wasClosed[id] = false;
					}
					anyOpened = true;
					break;
				}
			}
			return new ValueTuple<bool, bool, bool>(open, grab, !anyOpened && close);
		}

		// Token: 0x04000018 RID: 24
		private int _nextId = 1;

		// Token: 0x04000019 RID: 25
		private readonly Dictionary<int, DrawCallbackDelegate> _callbacks = new Dictionary<int, DrawCallbackDelegate>();

		// Token: 0x0400001A RID: 26
		private readonly Dictionary<int, bool> _wasClosed = new Dictionary<int, bool>();
	}
}
