using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WinRT.Interop;

namespace WinRT
{
	// Token: 0x020001A1 RID: 417
	internal abstract class IObjectReference : IDisposable
	{
		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000D53 RID: 3411 RVA: 0x000277BB File Offset: 0x000259BB
		public IntPtr ThisPtr
		{
			get
			{
				this.ThrowIfDisposed();
				return this.GetThisPtrForCurrentContext();
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000D54 RID: 3412 RVA: 0x000277C9 File Offset: 0x000259C9
		private protected IntPtr ThisPtrFromOriginalContext
		{
			get
			{
				this.ThrowIfDisposed();
				return this._thisPtr;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000D55 RID: 3413 RVA: 0x000277D7 File Offset: 0x000259D7
		// (set) Token: 0x06000D56 RID: 3414 RVA: 0x000277DF File Offset: 0x000259DF
		internal bool IsAggregated { get; set; }

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000D57 RID: 3415 RVA: 0x000277E8 File Offset: 0x000259E8
		// (set) Token: 0x06000D58 RID: 3416 RVA: 0x000277F0 File Offset: 0x000259F0
		internal bool PreventReleaseOnDispose { get; set; }

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000D59 RID: 3417 RVA: 0x000277F9 File Offset: 0x000259F9
		// (set) Token: 0x06000D5A RID: 3418 RVA: 0x00027801 File Offset: 0x00025A01
		internal bool PreventReleaseFromTrackerSourceOnDispose { get; set; }

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000D5B RID: 3419 RVA: 0x0002780A File Offset: 0x00025A0A
		// (set) Token: 0x06000D5C RID: 3420 RVA: 0x00027814 File Offset: 0x00025A14
		internal IntPtr ReferenceTrackerPtr
		{
			get
			{
				return this._referenceTrackerPtr;
			}
			set
			{
				this._referenceTrackerPtr = value;
				if (this._referenceTrackerPtr != IntPtr.Zero)
				{
					method addRef = this.ReferenceTracker.IUnknownVftbl.AddRef;
					object obj = calli(System.UInt32(System.IntPtr), this._referenceTrackerPtr, addRef);
					this.AddRefFromTrackerSource();
				}
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000D5D RID: 3421 RVA: 0x00027861 File Offset: 0x00025A61
		internal unsafe IReferenceTrackerVftbl ReferenceTracker
		{
			get
			{
				this.ThrowIfDisposed();
				return *(*(IntPtr*)((void*)this.ReferenceTrackerPtr));
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000D5E RID: 3422 RVA: 0x0002787A File Offset: 0x00025A7A
		protected unsafe IUnknownVftbl VftblIUnknown
		{
			get
			{
				this.ThrowIfDisposed();
				return *(*(IntPtr*)((void*)this.ThisPtr));
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000D5F RID: 3423 RVA: 0x00027893 File Offset: 0x00025A93
		private protected unsafe IUnknownVftbl VftblIUnknownFromOriginalContext
		{
			get
			{
				this.ThrowIfDisposed();
				return *(*(IntPtr*)((void*)this.ThisPtrFromOriginalContext));
			}
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x000278AC File Offset: 0x00025AAC
		protected IObjectReference(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				throw new ArgumentNullException("thisPtr");
			}
			this._thisPtr = thisPtr;
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x000278E0 File Offset: 0x00025AE0
		~IObjectReference()
		{
			this.Dispose(false);
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00027910 File Offset: 0x00025B10
		public ObjectReference<T> As<T>()
		{
			return this.As<T>(GuidGenerator.GetIID(typeof(T)));
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00027928 File Offset: 0x00025B28
		public ObjectReference<T> As<T>(Guid iid)
		{
			ObjectReference<T> result;
			Marshal.ThrowExceptionForHR(this.TryAs<T>(iid, out result));
			return result;
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x00027944 File Offset: 0x00025B44
		public unsafe TInterface AsInterface<TInterface>()
		{
			if (CustomAttributeExtensions.IsDefined(typeof(TInterface), typeof(ComImportAttribute)))
			{
				Guid guid = typeof(TInterface).GUID;
				method queryInterface = this.VftblIUnknown.QueryInterface;
				IntPtr intPtr;
				Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Guid&,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), this.ThisPtr, ref guid, ref intPtr, queryInterface));
				try
				{
					return (TInterface)((object)Marshal.GetObjectForIUnknown(intPtr));
				}
				finally
				{
					method release = Marshal.PtrToStructure<IUnknownVftbl>(Unsafe.AsRef<VftblPtr>(intPtr.ToPointer())->Vftbl).Release;
					object obj = calli(System.UInt32(System.IntPtr), intPtr, release);
				}
			}
			return (TInterface)((object)new IInspectable(this));
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x00027A00 File Offset: 0x00025C00
		public int TryAs<T>(out ObjectReference<T> objRef)
		{
			return this.TryAs<T>(GuidGenerator.GetIID(typeof(T)), out objRef);
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00027A18 File Offset: 0x00025C18
		public virtual int TryAs<T>(Guid iid, out ObjectReference<T> objRef)
		{
			objRef = null;
			this.ThrowIfDisposed();
			method queryInterface = this.VftblIUnknown.QueryInterface;
			IntPtr intPtr;
			object obj = calli(System.Int32(System.IntPtr,System.Guid&,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), this.ThisPtr, ref iid, ref intPtr, queryInterface);
			if (obj >= 0)
			{
				if (this.IsAggregated)
				{
					Marshal.Release(intPtr);
				}
				this.AddRefFromTrackerSource();
				objRef = ObjectReference<T>.Attach(ref intPtr);
				objRef.IsAggregated = this.IsAggregated;
				objRef.PreventReleaseOnDispose = this.IsAggregated;
				objRef.ReferenceTrackerPtr = this.ReferenceTrackerPtr;
			}
			return obj;
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x00027A96 File Offset: 0x00025C96
		public virtual ObjectReference<IUnknownVftbl> AsKnownPtr(IntPtr ptr)
		{
			this.AddRef(true);
			ObjectReference<IUnknownVftbl> objectReference = ObjectReference<IUnknownVftbl>.Attach(ref ptr);
			objectReference.IsAggregated = this.IsAggregated;
			objectReference.PreventReleaseOnDispose = this.IsAggregated;
			objectReference.ReferenceTrackerPtr = this.ReferenceTrackerPtr;
			return objectReference;
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x00027ACC File Offset: 0x00025CCC
		public virtual int TryAs(Guid iid, out IntPtr ppv)
		{
			ppv = IntPtr.Zero;
			this.ThrowIfDisposed();
			method queryInterface = this.VftblIUnknown.QueryInterface;
			IntPtr intPtr;
			object obj = calli(System.Int32(System.IntPtr,System.Guid&,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), this.ThisPtr, ref iid, ref intPtr, queryInterface);
			if (obj >= 0)
			{
				ppv = intPtr;
			}
			return obj;
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x00027B0C File Offset: 0x00025D0C
		public IObjectReference As(Guid iid)
		{
			return this.As<IUnknownVftbl>(iid);
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x00027B18 File Offset: 0x00025D18
		public T AsType<T>()
		{
			this.ThrowIfDisposed();
			ConstructorInfo constructor = typeof(T).GetConstructor(new Type[]
			{
				typeof(IObjectReference)
			});
			if (constructor != null)
			{
				ConstructorInfo constructorInfo = constructor;
				object[] array = new IObjectReference[]
				{
					this
				};
				return (T)((object)constructorInfo.Invoke(array));
			}
			throw new InvalidOperationException("Target type is not a projected interface.");
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x00027B79 File Offset: 0x00025D79
		public IntPtr GetRef()
		{
			this.ThrowIfDisposed();
			this.AddRef(false);
			return this.ThisPtr;
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00027B90 File Offset: 0x00025D90
		protected void ThrowIfDisposed()
		{
			if (this.disposed)
			{
				object disposedLock = this._disposedLock;
				lock (disposedLock)
				{
					if (this.disposed)
					{
						throw new ObjectDisposedException("ObjectReference");
					}
				}
			}
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x00027BE8 File Offset: 0x00025DE8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00027BF8 File Offset: 0x00025DF8
		protected virtual void Dispose(bool disposing)
		{
			object disposedLock = this._disposedLock;
			lock (disposedLock)
			{
				if (!this.disposed)
				{
					if (!this.PreventReleaseOnDispose)
					{
						this.Release();
					}
					this.DisposeTrackerSource();
					this.disposed = true;
				}
			}
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x00027C58 File Offset: 0x00025E58
		internal bool Resurrect()
		{
			object disposedLock = this._disposedLock;
			bool result;
			lock (disposedLock)
			{
				if (!this.disposed)
				{
					result = false;
				}
				else
				{
					this.disposed = false;
					this.ResurrectTrackerSource();
					this.AddRef();
					GC.ReRegisterForFinalize(this);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x00027CBC File Offset: 0x00025EBC
		protected virtual void AddRef(bool refFromTrackerSource)
		{
			method addRef = this.VftblIUnknown.AddRef;
			object obj = calli(System.UInt32(System.IntPtr), this.ThisPtr, addRef);
			if (refFromTrackerSource)
			{
				this.AddRefFromTrackerSource();
			}
		}

		// Token: 0x06000D71 RID: 3441 RVA: 0x00027CEE File Offset: 0x00025EEE
		protected virtual void AddRef()
		{
			this.AddRef(true);
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x00027CF8 File Offset: 0x00025EF8
		protected virtual void Release()
		{
			this.ReleaseFromTrackerSource();
			method release = this.VftblIUnknown.Release;
			object obj = calli(System.UInt32(System.IntPtr), this.ThisPtr, release);
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x00027D28 File Offset: 0x00025F28
		private protected void ReleaseWithoutContext()
		{
			this.ReleaseFromTrackerSource();
			method release = this.VftblIUnknownFromOriginalContext.Release;
			object obj = calli(System.UInt32(System.IntPtr), this.ThisPtrFromOriginalContext, release);
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x00027D58 File Offset: 0x00025F58
		internal bool IsReferenceToManagedObject
		{
			get
			{
				return this.VftblIUnknown.Equals(IUnknownVftbl.AbiToProjectionVftbl);
			}
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00027D78 File Offset: 0x00025F78
		internal void AddRefFromTrackerSource()
		{
			if (this.ReferenceTrackerPtr != IntPtr.Zero)
			{
				method addRefFromTrackerSource = this.ReferenceTracker.AddRefFromTrackerSource;
				object obj = calli(System.Int32(System.IntPtr), this.ReferenceTrackerPtr, addRefFromTrackerSource);
			}
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x00027DB4 File Offset: 0x00025FB4
		internal void ReleaseFromTrackerSource()
		{
			if (this.ReferenceTrackerPtr != IntPtr.Zero)
			{
				method releaseFromTrackerSource = this.ReferenceTracker.ReleaseFromTrackerSource;
				object obj = calli(System.Int32(System.IntPtr), this.ReferenceTrackerPtr, releaseFromTrackerSource);
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x00027DF0 File Offset: 0x00025FF0
		private void ResurrectTrackerSource()
		{
			if (this.ReferenceTrackerPtr != IntPtr.Zero)
			{
				IReferenceTrackerVftbl referenceTracker = this.ReferenceTracker;
				method addRef = referenceTracker.IUnknownVftbl.AddRef;
				object obj = calli(System.UInt32(System.IntPtr), this.ReferenceTrackerPtr, addRef);
				if (!this.PreventReleaseFromTrackerSourceOnDispose)
				{
					referenceTracker = this.ReferenceTracker;
					method addRefFromTrackerSource = referenceTracker.AddRefFromTrackerSource;
					object obj2 = calli(System.Int32(System.IntPtr), this.ReferenceTrackerPtr, addRefFromTrackerSource);
				}
			}
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x00027E54 File Offset: 0x00026054
		private void DisposeTrackerSource()
		{
			if (this.ReferenceTrackerPtr != IntPtr.Zero)
			{
				IReferenceTrackerVftbl referenceTracker;
				if (!this.PreventReleaseFromTrackerSourceOnDispose)
				{
					referenceTracker = this.ReferenceTracker;
					method releaseFromTrackerSource = referenceTracker.ReleaseFromTrackerSource;
					object obj = calli(System.Int32(System.IntPtr), this.ReferenceTrackerPtr, releaseFromTrackerSource);
				}
				referenceTracker = this.ReferenceTracker;
				method release = referenceTracker.IUnknownVftbl.Release;
				object obj2 = calli(System.UInt32(System.IntPtr), this.ReferenceTrackerPtr, release);
			}
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x00027EB8 File Offset: 0x000260B8
		private protected virtual IntPtr GetThisPtrForCurrentContext()
		{
			return this.ThisPtrFromOriginalContext;
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x00027EC0 File Offset: 0x000260C0
		public ObjectReferenceValue AsValue()
		{
			return new ObjectReferenceValue(this.ThisPtr, IntPtr.Zero, true, this);
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x00027ED4 File Offset: 0x000260D4
		public ObjectReferenceValue AsValue(Guid iid)
		{
			method queryInterface = this.VftblIUnknown.QueryInterface;
			IntPtr intPtr;
			Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Guid&,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), this.ThisPtr, ref iid, ref intPtr, queryInterface));
			if (this.IsAggregated)
			{
				Marshal.Release(intPtr);
			}
			this.AddRefFromTrackerSource();
			return new ObjectReferenceValue(intPtr, this.ReferenceTrackerPtr, this.IsAggregated, this);
		}

		// Token: 0x0400027C RID: 636
		protected bool disposed;

		// Token: 0x0400027D RID: 637
		private readonly IntPtr _thisPtr;

		// Token: 0x0400027E RID: 638
		private object _disposedLock = new object();

		// Token: 0x0400027F RID: 639
		private IntPtr _referenceTrackerPtr;
	}
}
