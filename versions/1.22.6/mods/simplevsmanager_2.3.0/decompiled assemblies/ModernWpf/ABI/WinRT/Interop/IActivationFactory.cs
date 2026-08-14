using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WinRT;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x0200010C RID: 268
	[ObjectReferenceWrapper("_obj")]
	[Guid("00000035-0000-0000-C000-000000000046")]
	internal class IActivationFactory : WinRT.Interop.IActivationFactory
	{
		// Token: 0x06000976 RID: 2422 RVA: 0x0001ABE8 File Offset: 0x00018DE8
		internal static ObjectReference<IActivationFactory.Vftbl> FromAbi(IntPtr thisPtr)
		{
			return ObjectReference<IActivationFactory.Vftbl>.FromAbi(thisPtr);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0001ABF0 File Offset: 0x00018DF0
		public static implicit operator IActivationFactory(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IActivationFactory(obj);
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000978 RID: 2424 RVA: 0x0001ABFD File Offset: 0x00018DFD
		public IObjectReference ObjRef
		{
			get
			{
				return this._obj;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x0001AC05 File Offset: 0x00018E05
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0001AC12 File Offset: 0x00018E12
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0001AC1F File Offset: 0x00018E1F
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0001AC2C File Offset: 0x00018E2C
		public IActivationFactory(IObjectReference obj) : this(obj.As<IActivationFactory.Vftbl>())
		{
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0001AC3A File Offset: 0x00018E3A
		internal IActivationFactory(ObjectReference<IActivationFactory.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0001AC4C File Offset: 0x00018E4C
		public unsafe IntPtr ActivateInstance()
		{
			IntPtr result = default(IntPtr);
			method activateInstance_ = this._obj.Vftbl.ActivateInstance_0;
			ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), this.ThisPtr, &result, activateInstance_));
			return result;
		}

		// Token: 0x040001AA RID: 426
		protected readonly ObjectReference<IActivationFactory.Vftbl> _obj;

		// Token: 0x02000325 RID: 805
		[Guid("00000035-0000-0000-C000-000000000046")]
		public struct Vftbl
		{
			// Token: 0x0600178C RID: 6028 RVA: 0x00042108 File Offset: 0x00040308
			unsafe static Vftbl()
			{
				*(IActivationFactory.Vftbl*)((void*)IActivationFactory.Vftbl.AbiToProjectionVftablePtr) = new IActivationFactory.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					ActivateInstance_0 = ldftn(Do_Abi_ActivateInstance_0)
				};
			}

			// Token: 0x0600178D RID: 6029 RVA: 0x00042168 File Offset: 0x00040368
			[UnmanagedCallersOnly(CallConvs = new Type[]
			{
				typeof(CallConvStdcall)
			})]
			private unsafe static int Do_Abi_ActivateInstance_0(IntPtr thisPtr, IntPtr* result)
			{
				IntPtr intPtr = default(IntPtr);
				*result = default(IntPtr);
				try
				{
					intPtr = ComWrappersSupport.FindObject<WinRT.Interop.IActivationFactory>(thisPtr).ActivateInstance();
					*result = intPtr;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0400080C RID: 2060
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400080D RID: 2061
			public method ActivateInstance_0;

			// Token: 0x0400080E RID: 2062
			public static readonly IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IActivationFactory.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr));
		}
	}
}
