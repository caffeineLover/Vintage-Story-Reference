using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x0200010B RID: 267
	[Guid("82BA7092-4C88-427D-A7BC-16DD93FEB67E")]
	internal class IRestrictedErrorInfo : WinRT.Interop.IRestrictedErrorInfo
	{
		// Token: 0x0600096C RID: 2412 RVA: 0x0001AA23 File Offset: 0x00018C23
		public static ObjectReference<IRestrictedErrorInfo.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IRestrictedErrorInfo.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x0001AA2B File Offset: 0x00018C2B
		public static implicit operator IRestrictedErrorInfo(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IRestrictedErrorInfo(obj);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0001AA38 File Offset: 0x00018C38
		public static implicit operator IRestrictedErrorInfo(ObjectReference<IRestrictedErrorInfo.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IRestrictedErrorInfo(obj);
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x0001AA45 File Offset: 0x00018C45
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0001AA52 File Offset: 0x00018C52
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0001AA5F File Offset: 0x00018C5F
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x0001AA6C File Offset: 0x00018C6C
		public IRestrictedErrorInfo(IObjectReference obj) : this(obj.As<IRestrictedErrorInfo.Vftbl>())
		{
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0001AA7A File Offset: 0x00018C7A
		public IRestrictedErrorInfo(ObjectReference<IRestrictedErrorInfo.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0001AA8C File Offset: 0x00018C8C
		public unsafe void GetErrorDetails(out string description, out int error, out string restrictedDescription, out string capabilitySid)
		{
			IntPtr zero = IntPtr.Zero;
			IntPtr zero2 = IntPtr.Zero;
			IntPtr zero3 = IntPtr.Zero;
			try
			{
				try
				{
					fixed (int* ptr = &error)
					{
						int* ptr2 = ptr;
						method getErrorDetails_ = this._obj.Vftbl.GetErrorDetails_0;
						Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*,System.Int32*,System.IntPtr*,System.IntPtr*), this.ThisPtr, &zero, ptr2, &zero2, &zero3, getErrorDetails_));
					}
				}
				finally
				{
					int* ptr = null;
				}
				description = ((zero != IntPtr.Zero) ? Marshal.PtrToStringBSTR(zero) : string.Empty);
				restrictedDescription = ((zero2 != IntPtr.Zero) ? Marshal.PtrToStringBSTR(zero2) : string.Empty);
				capabilitySid = ((zero3 != IntPtr.Zero) ? Marshal.PtrToStringBSTR(zero3) : string.Empty);
			}
			finally
			{
				Marshal.FreeBSTR(zero);
				Marshal.FreeBSTR(zero2);
				Marshal.FreeBSTR(zero3);
			}
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x0001AB74 File Offset: 0x00018D74
		public unsafe string GetReference()
		{
			IntPtr intPtr = default(IntPtr);
			string result;
			try
			{
				method getReference_ = this._obj.Vftbl.GetReference_1;
				Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), this.ThisPtr, &intPtr, getReference_));
				result = ((intPtr != IntPtr.Zero) ? Marshal.PtrToStringBSTR(intPtr) : string.Empty);
			}
			finally
			{
				Marshal.FreeBSTR(intPtr);
			}
			return result;
		}

		// Token: 0x040001A9 RID: 425
		protected readonly ObjectReference<IRestrictedErrorInfo.Vftbl> _obj;

		// Token: 0x02000324 RID: 804
		[Guid("82BA7092-4C88-427D-A7BC-16DD93FEB67E")]
		public struct Vftbl
		{
			// Token: 0x17000479 RID: 1145
			// (get) Token: 0x0600178A RID: 6026 RVA: 0x000420F8 File Offset: 0x000402F8
			public method GetErrorDetails_0
			{
				get
				{
					return this._GetErrorDetails_0;
				}
			}

			// Token: 0x1700047A RID: 1146
			// (get) Token: 0x0600178B RID: 6027 RVA: 0x00042100 File Offset: 0x00040300
			public method GetReference_1
			{
				get
				{
					return this._GetReference_1;
				}
			}

			// Token: 0x04000809 RID: 2057
			internal IUnknownVftbl unknownVftbl;

			// Token: 0x0400080A RID: 2058
			private unsafe void* _GetErrorDetails_0;

			// Token: 0x0400080B RID: 2059
			private unsafe void* _GetReference_1;

			// Token: 0x02000424 RID: 1060
			// (Invoke) Token: 0x06001C32 RID: 7218
			public delegate int _GetErrorDetails(IntPtr thisPtr, out IntPtr description, out int error, out IntPtr restrictedDescription, out IntPtr capabilitySid);

			// Token: 0x02000425 RID: 1061
			// (Invoke) Token: 0x06001C36 RID: 7222
			public delegate int _GetReference(IntPtr thisPtr, out IntPtr reference);
		}
	}
}
