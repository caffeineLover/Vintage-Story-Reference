using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x02000109 RID: 265
	[Guid("04a2dbf3-df83-116c-0946-0812abf6e07d")]
	internal class ILanguageExceptionErrorInfo : WinRT.Interop.ILanguageExceptionErrorInfo
	{
		// Token: 0x0600095A RID: 2394 RVA: 0x0001A8A4 File Offset: 0x00018AA4
		public static ObjectReference<ILanguageExceptionErrorInfo.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<ILanguageExceptionErrorInfo.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0001A8AC File Offset: 0x00018AAC
		public static implicit operator ILanguageExceptionErrorInfo(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new ILanguageExceptionErrorInfo(obj);
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0001A8B9 File Offset: 0x00018AB9
		public static implicit operator ILanguageExceptionErrorInfo(ObjectReference<ILanguageExceptionErrorInfo.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new ILanguageExceptionErrorInfo(obj);
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x0001A8C6 File Offset: 0x00018AC6
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0001A8D3 File Offset: 0x00018AD3
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x0001A8E0 File Offset: 0x00018AE0
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x0001A8ED File Offset: 0x00018AED
		public ILanguageExceptionErrorInfo(IObjectReference obj) : this(obj.As<ILanguageExceptionErrorInfo.Vftbl>())
		{
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0001A8FB File Offset: 0x00018AFB
		public ILanguageExceptionErrorInfo(ObjectReference<ILanguageExceptionErrorInfo.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0001A90C File Offset: 0x00018B0C
		public unsafe IObjectReference GetLanguageException()
		{
			IntPtr zero = IntPtr.Zero;
			IObjectReference result;
			try
			{
				method getLanguageException_ = this._obj.Vftbl.GetLanguageException_0;
				Marshal.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), this.ThisPtr, &zero, getLanguageException_));
				result = ObjectReference<IUnknownVftbl>.Attach(ref zero);
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					method release = ((IntPtr*)((void*)zero))->Release;
					object obj = calli(System.UInt32(System.IntPtr), zero, release);
				}
			}
			return result;
		}

		// Token: 0x040001A7 RID: 423
		protected readonly ObjectReference<ILanguageExceptionErrorInfo.Vftbl> _obj;

		// Token: 0x02000322 RID: 802
		[Guid("04a2dbf3-df83-116c-0946-0812abf6e07d")]
		public struct Vftbl
		{
			// Token: 0x17000477 RID: 1143
			// (get) Token: 0x06001785 RID: 6021 RVA: 0x00042034 File Offset: 0x00040234
			public method GetLanguageException_0
			{
				get
				{
					return this._GetLanguageException_0;
				}
			}

			// Token: 0x04000803 RID: 2051
			internal IUnknownVftbl IUnknownVftbl;

			// Token: 0x04000804 RID: 2052
			private unsafe void* _GetLanguageException_0;
		}
	}
}
