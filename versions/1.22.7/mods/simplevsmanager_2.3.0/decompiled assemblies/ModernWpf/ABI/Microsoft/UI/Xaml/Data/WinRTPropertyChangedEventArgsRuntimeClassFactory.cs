using System;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.Microsoft.UI.Xaml.Data
{
	// Token: 0x02000172 RID: 370
	[ObjectReferenceWrapper("_obj")]
	[Guid("7C0C27A8-0B41-5070-B160-FC9AE960A36C")]
	internal sealed class WinRTPropertyChangedEventArgsRuntimeClassFactory
	{
		// Token: 0x06000BDD RID: 3037 RVA: 0x00020F08 File Offset: 0x0001F108
		public static ObjectReference<WinRTPropertyChangedEventArgsRuntimeClassFactory.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<WinRTPropertyChangedEventArgsRuntimeClassFactory.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00020F10 File Offset: 0x0001F110
		public static implicit operator WinRTPropertyChangedEventArgsRuntimeClassFactory(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new WinRTPropertyChangedEventArgsRuntimeClassFactory(obj);
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00020F1D File Offset: 0x0001F11D
		public static implicit operator WinRTPropertyChangedEventArgsRuntimeClassFactory(ObjectReference<WinRTPropertyChangedEventArgsRuntimeClassFactory.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new WinRTPropertyChangedEventArgsRuntimeClassFactory(obj);
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000BE0 RID: 3040 RVA: 0x00020F2A File Offset: 0x0001F12A
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00020F37 File Offset: 0x0001F137
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00020F44 File Offset: 0x0001F144
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00020F51 File Offset: 0x0001F151
		public WinRTPropertyChangedEventArgsRuntimeClassFactory(IObjectReference obj) : this(obj.As<WinRTPropertyChangedEventArgsRuntimeClassFactory.Vftbl>())
		{
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00020F5F File Offset: 0x0001F15F
		public WinRTPropertyChangedEventArgsRuntimeClassFactory(ObjectReference<WinRTPropertyChangedEventArgsRuntimeClassFactory.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00020F70 File Offset: 0x0001F170
		public unsafe IObjectReference CreateInstance(string name, object baseInterface, out IObjectReference innerInterface)
		{
			IObjectReference objectReference = null;
			IntPtr intPtr = default(IntPtr);
			IntPtr intPtr2 = default(IntPtr);
			IObjectReference result;
			try
			{
				MarshalString.Pinnable pinnable = new MarshalString.Pinnable(name);
				try
				{
					fixed (char* ptr = pinnable.GetPinnableReference())
					{
						char* ptr2 = ptr;
						objectReference = MarshalInspectable<object>.CreateMarshaler(baseInterface, true);
						method createInstance_ = this._obj.Vftbl.CreateInstance_0;
						ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr*,System.IntPtr*), this.ThisPtr, MarshalString.GetAbi(ref pinnable), MarshalInspectable<object>.GetAbi(objectReference), &intPtr, &intPtr2, createInstance_));
						innerInterface = ObjectReference<IUnknownVftbl>.FromAbi(intPtr);
						result = ObjectReference<IUnknownVftbl>.Attach(ref intPtr2);
					}
				}
				finally
				{
					char* ptr = null;
				}
			}
			finally
			{
				MarshalInspectable<object>.DisposeMarshaler(objectReference);
				MarshalInspectable<object>.DisposeAbi(intPtr);
				MarshalInspectable<object>.DisposeAbi(intPtr2);
			}
			return result;
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00021030 File Offset: 0x0001F230
		public unsafe ObjectReferenceValue CreateInstance(string name)
		{
			IntPtr intPtr = default(IntPtr);
			IntPtr ptr = default(IntPtr);
			ObjectReferenceValue result;
			try
			{
				MarshalString.Pinnable pinnable = new MarshalString.Pinnable(name);
				try
				{
					fixed (char* ptr2 = pinnable.GetPinnableReference())
					{
						char* ptr3 = ptr2;
						method createInstance_ = this._obj.Vftbl.CreateInstance_0;
						ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr,System.IntPtr,System.IntPtr*,System.IntPtr*), this.ThisPtr, MarshalString.GetAbi(ref pinnable), IntPtr.Zero, &intPtr, &ptr, createInstance_));
						result = new ObjectReferenceValue(ptr);
					}
				}
				finally
				{
					char* ptr2 = null;
				}
			}
			finally
			{
				MarshalInspectable<object>.DisposeAbi(intPtr);
			}
			return result;
		}

		// Token: 0x04000206 RID: 518
		private readonly ObjectReference<WinRTPropertyChangedEventArgsRuntimeClassFactory.Vftbl> _obj;

		// Token: 0x020003B2 RID: 946
		[Guid("7C0C27A8-0B41-5070-B160-FC9AE960A36C")]
		public struct Vftbl
		{
			// Token: 0x170004E7 RID: 1255
			// (get) Token: 0x06001A4E RID: 6734 RVA: 0x00048858 File Offset: 0x00046A58
			public method CreateInstance_0
			{
				get
				{
					return this._CreateInstance_0;
				}
			}

			// Token: 0x04000956 RID: 2390
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000957 RID: 2391
			private unsafe void* _CreateInstance_0;
		}
	}
}
