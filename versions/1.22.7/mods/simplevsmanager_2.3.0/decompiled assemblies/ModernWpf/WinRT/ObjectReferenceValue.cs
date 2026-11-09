using System;

namespace WinRT
{
	// Token: 0x020001A4 RID: 420
	internal readonly struct ObjectReferenceValue
	{
		// Token: 0x06000D92 RID: 3474 RVA: 0x0002834C File Offset: 0x0002654C
		internal ObjectReferenceValue(IntPtr ptr)
		{
			this = default(ObjectReferenceValue);
			this.ptr = ptr;
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x0002835C File Offset: 0x0002655C
		internal ObjectReferenceValue(IntPtr ptr, IntPtr referenceTracker, bool preventReleaseOnDispose, IObjectReference objRef)
		{
			this.ptr = ptr;
			this.referenceTracker = referenceTracker;
			this.preventReleaseOnDispose = preventReleaseOnDispose;
			this.objRef = objRef;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0002837B File Offset: 0x0002657B
		public static ObjectReferenceValue Attach(ref IntPtr thisPtr)
		{
			ObjectReferenceValue result = new ObjectReferenceValue(thisPtr);
			thisPtr = IntPtr.Zero;
			return result;
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x0002838B File Offset: 0x0002658B
		public IntPtr GetAbi()
		{
			return this.ptr;
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00028394 File Offset: 0x00026594
		public unsafe IntPtr Detach()
		{
			if (this.preventReleaseOnDispose && this.ptr != IntPtr.Zero)
			{
				method addRef = ((IntPtr*)((void*)this.ptr))->AddRef;
				object obj = calli(System.UInt32(System.IntPtr), this.ptr, addRef);
			}
			if (this.referenceTracker != IntPtr.Zero)
			{
				method releaseFromTrackerSource = ((IntPtr*)((void*)this.referenceTracker))->ReleaseFromTrackerSource;
				object obj2 = calli(System.Int32(System.IntPtr), this.referenceTracker, releaseFromTrackerSource);
			}
			return this.ptr;
		}

		// Token: 0x06000D97 RID: 3479 RVA: 0x00028414 File Offset: 0x00026614
		public unsafe void Dispose()
		{
			if (this.referenceTracker != IntPtr.Zero)
			{
				method releaseFromTrackerSource = ((IntPtr*)((void*)this.referenceTracker))->ReleaseFromTrackerSource;
				object obj = calli(System.Int32(System.IntPtr), this.referenceTracker, releaseFromTrackerSource);
			}
			if (!this.preventReleaseOnDispose && this.ptr != IntPtr.Zero)
			{
				method release = ((IntPtr*)((void*)this.ptr))->Release;
				object obj2 = calli(System.UInt32(System.IntPtr), this.ptr, release);
			}
		}

		// Token: 0x0400028A RID: 650
		internal readonly IntPtr ptr;

		// Token: 0x0400028B RID: 651
		internal readonly IntPtr referenceTracker;

		// Token: 0x0400028C RID: 652
		internal readonly bool preventReleaseOnDispose;

		// Token: 0x0400028D RID: 653
		internal readonly IObjectReference objRef;
	}
}
