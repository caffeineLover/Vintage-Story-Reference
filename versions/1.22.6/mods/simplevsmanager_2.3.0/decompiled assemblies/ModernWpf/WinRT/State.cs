using System;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020001B3 RID: 435
	internal abstract class State : IDisposable
	{
		// Token: 0x06000DF8 RID: 3576 RVA: 0x0002A62F File Offset: 0x0002882F
		protected State(IntPtr obj, int index)
		{
			this.obj = obj;
			this.index = index;
			this.eventInvoke = this.GetEventInvoke();
			this.cacheEntry = new WeakReference<State>(this);
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0002A660 File Offset: 0x00028860
		~State()
		{
			this.Dispose(false);
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0002A690 File Offset: 0x00028890
		public WeakReference<State> GetWeakReferenceForCache()
		{
			return this.cacheEntry;
		}

		// Token: 0x06000DFB RID: 3579
		protected abstract Delegate GetEventInvoke();

		// Token: 0x06000DFC RID: 3580 RVA: 0x0002A698 File Offset: 0x00028898
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				Cache.Remove(this.obj, this.index, this.cacheEntry);
				this.disposedValue = true;
			}
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0002A6C0 File Offset: 0x000288C0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0002A6D0 File Offset: 0x000288D0
		public void InitalizeReferenceTracking(IntPtr ptr)
		{
			this.eventInvokePtr = ptr;
			Guid iid = IReferenceTrackerTargetVftbl.IID;
			if (Marshal.QueryInterface(ptr, ref iid, ref this.referenceTrackerTargetPtr) != 0)
			{
				this.referenceTrackerTargetPtr = default(IntPtr);
				return;
			}
			Marshal.Release(this.referenceTrackerTargetPtr);
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0002A714 File Offset: 0x00028914
		public unsafe bool HasComReferences()
		{
			if (this.eventInvokePtr != (IntPtr)0)
			{
				IUnknownVftbl unknownVftbl = *(*(IntPtr*)((void*)this.eventInvokePtr));
				method system.UInt32_u0020(System.IntPtr) = unknownVftbl.AddRef;
				object obj = calli(System.UInt32(System.IntPtr), this.eventInvokePtr, system.UInt32_u0020(System.IntPtr));
				system.UInt32_u0020(System.IntPtr) = unknownVftbl.Release;
				if (calli(System.UInt32(System.IntPtr), this.eventInvokePtr, system.UInt32_u0020(System.IntPtr)))
				{
					return true;
				}
			}
			if (this.referenceTrackerTargetPtr != (IntPtr)0)
			{
				IReferenceTrackerTargetVftbl referenceTrackerTargetVftbl = *(*(IntPtr*)((void*)this.referenceTrackerTargetPtr));
				method system.UInt32_u0020(System.IntPtr) = referenceTrackerTargetVftbl.AddRefFromReferenceTracker;
				object obj2 = calli(System.UInt32(System.IntPtr), this.referenceTrackerTargetPtr, system.UInt32_u0020(System.IntPtr));
				system.UInt32_u0020(System.IntPtr) = referenceTrackerTargetVftbl.ReleaseFromReferenceTracker;
				if (calli(System.UInt32(System.IntPtr), this.referenceTrackerTargetPtr, system.UInt32_u0020(System.IntPtr)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040002AF RID: 687
		public EventRegistrationToken token;

		// Token: 0x040002B0 RID: 688
		public Delegate del;

		// Token: 0x040002B1 RID: 689
		public Delegate eventInvoke;

		// Token: 0x040002B2 RID: 690
		private bool disposedValue;

		// Token: 0x040002B3 RID: 691
		private readonly IntPtr obj;

		// Token: 0x040002B4 RID: 692
		private readonly int index;

		// Token: 0x040002B5 RID: 693
		private readonly WeakReference<State> cacheEntry;

		// Token: 0x040002B6 RID: 694
		private IntPtr eventInvokePtr;

		// Token: 0x040002B7 RID: 695
		private IntPtr referenceTrackerTargetPtr;
	}
}
