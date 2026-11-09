using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020001A3 RID: 419
	internal sealed class ObjectReferenceWithContext<T> : ObjectReference<T>
	{
		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000D84 RID: 3460 RVA: 0x00028077 File Offset: 0x00026277
		private ConcurrentDictionary<IntPtr, ObjectReference<T>> CachedContext
		{
			get
			{
				return this.__cachedContext ?? this.Make_CachedContext();
			}
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0002808B File Offset: 0x0002628B
		private ConcurrentDictionary<IntPtr, ObjectReference<T>> Make_CachedContext()
		{
			Interlocked.CompareExchange<ConcurrentDictionary<IntPtr, ObjectReference<T>>>(ref this.__cachedContext, new ConcurrentDictionary<IntPtr, ObjectReference<T>>(), null);
			return this.__cachedContext;
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000D86 RID: 3462 RVA: 0x000280A7 File Offset: 0x000262A7
		private AgileReference AgileReference
		{
			get
			{
				if (!this._isAgileReferenceSet)
				{
					return this.Make_AgileReference();
				}
				return this.__agileReference;
			}
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x000280C2 File Offset: 0x000262C2
		private AgileReference Make_AgileReference()
		{
			Context.CallInContext(this._contextCallbackPtr, this._contextToken, new Action(this.<Make_AgileReference>g__InitAgileReference|10_0), null);
			this._isAgileReferenceSet = true;
			return this.__agileReference;
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x000280F3 File Offset: 0x000262F3
		internal ObjectReferenceWithContext(IntPtr thisPtr, IntPtr contextCallbackPtr, IntPtr contextToken) : base(thisPtr)
		{
			this._contextCallbackPtr = contextCallbackPtr;
			this._contextToken = contextToken;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x0002810A File Offset: 0x0002630A
		internal ObjectReferenceWithContext(IntPtr thisPtr, IntPtr contextCallbackPtr, IntPtr contextToken, Guid iid) : this(thisPtr, contextCallbackPtr, contextToken)
		{
			this._iid = iid;
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00028120 File Offset: 0x00026320
		private protected override IntPtr GetThisPtrForCurrentContext()
		{
			ObjectReference<T> currentContext = this.GetCurrentContext();
			if (currentContext == null)
			{
				return base.GetThisPtrForCurrentContext();
			}
			return currentContext.ThisPtr;
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x00028144 File Offset: 0x00026344
		private protected override T GetVftblForCurrentContext()
		{
			ObjectReference<T> currentContext = this.GetCurrentContext();
			if (currentContext == null)
			{
				return base.GetVftblForCurrentContext();
			}
			return currentContext.Vftbl;
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x00028168 File Offset: 0x00026368
		private ObjectReference<T> GetCurrentContext()
		{
			IntPtr contextToken = Context.GetContextToken();
			if (this._contextCallbackPtr == IntPtr.Zero || contextToken == this._contextToken)
			{
				return null;
			}
			return this.CachedContext.GetOrAdd(contextToken, new Func<IntPtr, ObjectReference<T>>(this.<GetCurrentContext>g__CreateForCurrentContext|16_0));
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x000281B8 File Offset: 0x000263B8
		protected override void Release()
		{
			if (this.__cachedContext != null)
			{
				this.CachedContext.Clear();
			}
			Context.CallInContext(this._contextCallbackPtr, this._contextToken, new Action(base.Release), new Action(base.ReleaseWithoutContext));
			Context.DisposeContextCallback(this._contextCallbackPtr);
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0002820E File Offset: 0x0002640E
		public override ObjectReference<IUnknownVftbl> AsKnownPtr(IntPtr ptr)
		{
			this.AddRef(true);
			return new ObjectReferenceWithContext<IUnknownVftbl>(ptr, Context.GetContextCallback(), Context.GetContextToken())
			{
				IsAggregated = base.IsAggregated,
				PreventReleaseOnDispose = base.IsAggregated,
				ReferenceTrackerPtr = base.ReferenceTrackerPtr
			};
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0002824C File Offset: 0x0002644C
		public override int TryAs<U>(Guid iid, out ObjectReference<U> objRef)
		{
			objRef = null;
			method queryInterface = base.VftblIUnknown.QueryInterface;
			IntPtr intPtr;
			object obj = calli(System.Int32(System.IntPtr,System.Guid&,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), base.ThisPtr, ref iid, ref intPtr, queryInterface);
			if (obj >= 0)
			{
				if (base.IsAggregated)
				{
					Marshal.Release(intPtr);
				}
				base.AddRefFromTrackerSource();
				objRef = new ObjectReferenceWithContext<U>(intPtr, Context.GetContextCallback(), Context.GetContextToken(), iid)
				{
					IsAggregated = base.IsAggregated,
					PreventReleaseOnDispose = base.IsAggregated,
					ReferenceTrackerPtr = base.ReferenceTrackerPtr
				};
			}
			return obj;
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x000282CB File Offset: 0x000264CB
		[CompilerGenerated]
		private void <Make_AgileReference>g__InitAgileReference|10_0()
		{
			Interlocked.CompareExchange<AgileReference>(ref this.__agileReference, new AgileReference(this), null);
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x000282E0 File Offset: 0x000264E0
		[CompilerGenerated]
		private ObjectReference<T> <GetCurrentContext>g__CreateForCurrentContext|16_0(IntPtr _)
		{
			AgileReference agileReference = this.AgileReference;
			if (agileReference == null)
			{
				return null;
			}
			ObjectReference<T> result;
			try
			{
				if (this._iid == Guid.Empty)
				{
					result = agileReference.Get<T>(GuidGenerator.GetIID(typeof(T)));
				}
				else
				{
					result = agileReference.Get<T>(this._iid);
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x04000284 RID: 644
		private readonly IntPtr _contextCallbackPtr;

		// Token: 0x04000285 RID: 645
		private readonly IntPtr _contextToken;

		// Token: 0x04000286 RID: 646
		private volatile ConcurrentDictionary<IntPtr, ObjectReference<T>> __cachedContext;

		// Token: 0x04000287 RID: 647
		private volatile bool _isAgileReferenceSet;

		// Token: 0x04000288 RID: 648
		private volatile AgileReference __agileReference;

		// Token: 0x04000289 RID: 649
		private readonly Guid _iid;
	}
}
