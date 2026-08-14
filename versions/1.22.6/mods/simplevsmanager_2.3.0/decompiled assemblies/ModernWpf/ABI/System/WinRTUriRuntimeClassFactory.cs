using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.System
{
	// Token: 0x0200013A RID: 314
	[ObjectReferenceWrapper("_obj")]
	[Guid("44A9796F-723E-4FDF-A218-033E75B0C084")]
	internal sealed class WinRTUriRuntimeClassFactory
	{
		// Token: 0x06000A53 RID: 2643 RVA: 0x0001CF21 File Offset: 0x0001B121
		public static ObjectReference<WinRTUriRuntimeClassFactory.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<WinRTUriRuntimeClassFactory.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0001CF29 File Offset: 0x0001B129
		public static implicit operator WinRTUriRuntimeClassFactory(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new WinRTUriRuntimeClassFactory(obj);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0001CF36 File Offset: 0x0001B136
		public static implicit operator WinRTUriRuntimeClassFactory(ObjectReference<WinRTUriRuntimeClassFactory.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new WinRTUriRuntimeClassFactory(obj);
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0001CF43 File Offset: 0x0001B143
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x0001CF50 File Offset: 0x0001B150
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0001CF5D File Offset: 0x0001B15D
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0001CF6A File Offset: 0x0001B16A
		public WinRTUriRuntimeClassFactory(IObjectReference obj) : this(obj.As<WinRTUriRuntimeClassFactory.Vftbl>())
		{
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x0001CF78 File Offset: 0x0001B178
		public WinRTUriRuntimeClassFactory(ObjectReference<WinRTUriRuntimeClassFactory.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x0001CF88 File Offset: 0x0001B188
		public unsafe IObjectReference CreateUri(string uri)
		{
			IntPtr intPtr = default(IntPtr);
			MarshalString.Pinnable pinnable = new MarshalString.Pinnable(uri);
			fixed (char* pinnableReference = pinnable.GetPinnableReference())
			{
				char* ptr = pinnableReference;
				method createUri_ = this._obj.Vftbl.CreateUri_0;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), this.ThisPtr, MarshalString.GetAbi(ref pinnable), ref intPtr, createUri_));
				return ObjectReference<IUnknownVftbl>.Attach(ref intPtr);
			}
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x0001CFE8 File Offset: 0x0001B1E8
		public unsafe ObjectReferenceValue CreateUriForMarshaling(string uri)
		{
			IntPtr ptr = default(IntPtr);
			MarshalString.Pinnable pinnable = new MarshalString.Pinnable(uri);
			fixed (char* pinnableReference = pinnable.GetPinnableReference())
			{
				char* ptr2 = pinnableReference;
				method createUri_ = this._obj.Vftbl.CreateUri_0;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), this.ThisPtr, MarshalString.GetAbi(ref pinnable), ref ptr, createUri_));
				return new ObjectReferenceValue(ptr);
			}
		}

		// Token: 0x040001E4 RID: 484
		private readonly ObjectReference<WinRTUriRuntimeClassFactory.Vftbl> _obj;

		// Token: 0x02000351 RID: 849
		[Guid("44A9796F-723E-4FDF-A218-033E75B0C084")]
		public struct Vftbl
		{
			// Token: 0x1700049A RID: 1178
			// (get) Token: 0x06001828 RID: 6184 RVA: 0x000439AB File Offset: 0x00041BAB
			public method CreateUri_0
			{
				get
				{
					return this._CreateUri_0;
				}
			}

			// Token: 0x04000888 RID: 2184
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000889 RID: 2185
			private unsafe void* _CreateUri_0;

			// Token: 0x0400088A RID: 2186
			public IntPtr _CreateWithRelativeUri;
		}
	}
}
