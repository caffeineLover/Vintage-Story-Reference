using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x02000111 RID: 273
	[Guid("000001da-0000-0000-C000-000000000046")]
	internal sealed class IContextCallback : WinRT.Interop.IContextCallback
	{
		// Token: 0x06000991 RID: 2449 RVA: 0x0001AFB7 File Offset: 0x000191B7
		public static ObjectReference<IContextCallback.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IContextCallback.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0001AFBF File Offset: 0x000191BF
		public static implicit operator IContextCallback(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IContextCallback(obj);
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0001AFCC File Offset: 0x000191CC
		public static implicit operator IContextCallback(ObjectReference<IContextCallback.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IContextCallback(obj);
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x0001AFD9 File Offset: 0x000191D9
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0001AFE6 File Offset: 0x000191E6
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0001AFF3 File Offset: 0x000191F3
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0001B000 File Offset: 0x00019200
		public IContextCallback(IObjectReference obj) : this(obj.As<IContextCallback.Vftbl>())
		{
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0001B00E File Offset: 0x0001920E
		public IContextCallback(ObjectReference<IContextCallback.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0001B020 File Offset: 0x00019220
		public unsafe void ContextCallback(PFNCONTEXTCALL pfnCallback, ComCallData* pParam, Guid riid, int iMethod)
		{
			IntPtr functionPointerForDelegate = Marshal.GetFunctionPointerForDelegate<PFNCONTEXTCALL>(pfnCallback);
			method contextCallback_ = this._obj.Vftbl.ContextCallback_4;
			int num = calli(System.Int32(System.IntPtr,System.IntPtr,WinRT.Interop.ComCallData*,System.Guid*,System.Int32,System.IntPtr), this.ThisPtr, functionPointerForDelegate, pParam, &riid, iMethod, IntPtr.Zero, contextCallback_);
			GC.KeepAlive(pfnCallback);
			Marshal.ThrowExceptionForHR(num);
		}

		// Token: 0x040001AF RID: 431
		internal static readonly Guid IID = new Guid(474, 0, 0, 192, 0, 0, 0, 0, 0, 0, 70);

		// Token: 0x040001B0 RID: 432
		private readonly ObjectReference<IContextCallback.Vftbl> _obj;

		// Token: 0x02000327 RID: 807
		[Guid("000001da-0000-0000-C000-000000000046")]
		public struct Vftbl
		{
			// Token: 0x1700047E RID: 1150
			// (get) Token: 0x06001791 RID: 6033 RVA: 0x000421D0 File Offset: 0x000403D0
			// (set) Token: 0x06001792 RID: 6034 RVA: 0x000421D8 File Offset: 0x000403D8
			public method ContextCallback_4
			{
				get
				{
					return this._ContextCallback;
				}
				set
				{
					this._ContextCallback = value;
				}
			}

			// Token: 0x04000813 RID: 2067
			private IUnknownVftbl IUnknownVftbl;

			// Token: 0x04000814 RID: 2068
			private unsafe void* _ContextCallback;
		}

		// Token: 0x02000328 RID: 808
		private struct ContextCallData
		{
			// Token: 0x04000815 RID: 2069
			public IntPtr delegateHandle;

			// Token: 0x04000816 RID: 2070
			public unsafe ComCallData* userData;
		}
	}
}
