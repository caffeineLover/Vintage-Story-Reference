using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.Microsoft.UI.Xaml.Data
{
	// Token: 0x0200016B RID: 363
	[ObjectReferenceWrapper("_obj")]
	[Guid("62D0BD1E-B85F-5FCC-842A-7CB0DDA37FE5")]
	internal sealed class WinRTDataErrorsChangedEventArgsRuntimeClassFactory
	{
		// Token: 0x06000BC3 RID: 3011 RVA: 0x00020AE0 File Offset: 0x0001ECE0
		public static ObjectReference<WinRTDataErrorsChangedEventArgsRuntimeClassFactory.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<WinRTDataErrorsChangedEventArgsRuntimeClassFactory.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x00020AE8 File Offset: 0x0001ECE8
		public static implicit operator WinRTDataErrorsChangedEventArgsRuntimeClassFactory(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new WinRTDataErrorsChangedEventArgsRuntimeClassFactory(obj);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x00020AF5 File Offset: 0x0001ECF5
		public static implicit operator WinRTDataErrorsChangedEventArgsRuntimeClassFactory(ObjectReference<WinRTDataErrorsChangedEventArgsRuntimeClassFactory.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new WinRTDataErrorsChangedEventArgsRuntimeClassFactory(obj);
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x00020B02 File Offset: 0x0001ED02
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00020B0F File Offset: 0x0001ED0F
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x00020B1C File Offset: 0x0001ED1C
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x00020B29 File Offset: 0x0001ED29
		public WinRTDataErrorsChangedEventArgsRuntimeClassFactory(IObjectReference obj) : this(obj.As<WinRTDataErrorsChangedEventArgsRuntimeClassFactory.Vftbl>())
		{
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x00020B37 File Offset: 0x0001ED37
		public WinRTDataErrorsChangedEventArgsRuntimeClassFactory(ObjectReference<WinRTDataErrorsChangedEventArgsRuntimeClassFactory.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00020B48 File Offset: 0x0001ED48
		public unsafe IObjectReference CreateInstance(string name)
		{
			IntPtr intPtr = default(IntPtr);
			IObjectReference result;
			try
			{
				MarshalString.Pinnable pinnable = new MarshalString.Pinnable(name);
				try
				{
					fixed (char* ptr = pinnable.GetPinnableReference())
					{
						char* ptr2 = ptr;
						method createInstance_ = this._obj.Vftbl.CreateInstance_0;
						ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr*), this.ThisPtr, MarshalString.GetAbi(ref pinnable), &intPtr, createInstance_));
						result = ObjectReference<IUnknownVftbl>.Attach(ref intPtr);
					}
				}
				finally
				{
					char* ptr = null;
				}
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x00020BD8 File Offset: 0x0001EDD8
		public unsafe ObjectReferenceValue CreateInstanceForMarshaling(string name)
		{
			IntPtr ptr = default(IntPtr);
			MarshalString.Pinnable pinnable = new MarshalString.Pinnable(name);
			fixed (char* pinnableReference = pinnable.GetPinnableReference())
			{
				char* ptr2 = pinnableReference;
				method createInstance_ = this._obj.Vftbl.CreateInstance_0;
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr*), this.ThisPtr, MarshalString.GetAbi(ref pinnable), &ptr, createInstance_));
				return new ObjectReferenceValue(ptr);
			}
		}

		// Token: 0x040001FA RID: 506
		private readonly ObjectReference<WinRTDataErrorsChangedEventArgsRuntimeClassFactory.Vftbl> _obj;

		// Token: 0x020003A4 RID: 932
		[Guid("62D0BD1E-B85F-5FCC-842A-7CB0DDA37FE5")]
		public struct Vftbl
		{
			// Token: 0x170004E6 RID: 1254
			// (get) Token: 0x06001A14 RID: 6676 RVA: 0x00048519 File Offset: 0x00046719
			public method CreateInstance_0
			{
				get
				{
					return this._CreateInstance_0;
				}
			}

			// Token: 0x04000949 RID: 2377
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400094A RID: 2378
			private unsafe void* _CreateInstance_0;
		}
	}
}
