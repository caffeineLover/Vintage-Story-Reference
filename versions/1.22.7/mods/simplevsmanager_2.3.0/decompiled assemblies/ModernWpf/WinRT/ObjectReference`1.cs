using System;
using System.Runtime.CompilerServices;

namespace WinRT
{
	// Token: 0x020001A2 RID: 418
	internal class ObjectReference<T> : IObjectReference
	{
		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000D7C RID: 3452 RVA: 0x00027F2D File Offset: 0x0002612D
		public T Vftbl
		{
			get
			{
				base.ThrowIfDisposed();
				return this.GetVftblForCurrentContext();
			}
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00027F3B File Offset: 0x0002613B
		public static ObjectReference<T> Attach(ref IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			ObjectReference<T> result = new ObjectReference<T>(thisPtr);
			thisPtr = IntPtr.Zero;
			return result;
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00027F5B File Offset: 0x0002615B
		private ObjectReference(IntPtr thisPtr, T vftblT) : base(thisPtr)
		{
			this._vftbl = vftblT;
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x00027F6B File Offset: 0x0002616B
		private protected ObjectReference(IntPtr thisPtr) : this(thisPtr, ObjectReference<T>.GetVtable(thisPtr))
		{
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x00027F7C File Offset: 0x0002617C
		public static ObjectReference<T> FromAbi(IntPtr thisPtr, T vftblT)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			ObjectReference<T> objectReference = new ObjectReference<T>(thisPtr, vftblT);
			method addRef = objectReference.VftblIUnknown.AddRef;
			object obj = calli(System.UInt32(System.IntPtr), objectReference.ThisPtr, addRef);
			return objectReference;
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x00027FC0 File Offset: 0x000261C0
		public static ObjectReference<T> FromAbi(IntPtr thisPtr)
		{
			if (thisPtr == IntPtr.Zero)
			{
				return null;
			}
			T vtable = ObjectReference<T>.GetVtable(thisPtr);
			return ObjectReference<T>.FromAbi(thisPtr, vtable);
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x00027FEC File Offset: 0x000261EC
		private unsafe static T GetVtable(IntPtr thisPtr)
		{
			VftblPtr vftblPtr = *Unsafe.AsRef<VftblPtr>(thisPtr.ToPointer());
			T result;
			if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
			{
				result = (T)((object)typeof(T).GetConstructor(548, null, new Type[]
				{
					typeof(IntPtr)
				}, null).Invoke(new object[]
				{
					thisPtr
				}));
			}
			else
			{
				result = *Unsafe.AsRef<T>(vftblPtr.Vftbl.ToPointer());
			}
			return result;
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x0002806F File Offset: 0x0002626F
		private protected virtual T GetVftblForCurrentContext()
		{
			return this._vftbl;
		}

		// Token: 0x04000283 RID: 643
		private readonly T _vftbl;
	}
}
