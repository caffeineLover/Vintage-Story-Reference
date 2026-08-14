using System;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;
using WinRT.Interop;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000B0 RID: 176
	[ObjectReferenceWrapper("_obj")]
	[Guid("61C17707-2D65-11E0-9AE8-D48564015472")]
	internal sealed class IReferenceArray<T> : Windows.Foundation.IReferenceArray<T>
	{
		// Token: 0x060006F0 RID: 1776 RVA: 0x00011598 File Offset: 0x0000F798
		public static IObjectReference CreateMarshaler(object value)
		{
			if (value != null)
			{
				return ComWrappersSupport.CreateCCWForObject<IUnknownVftbl>(value, IReferenceArray<T>.PIID);
			}
			return null;
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x000115AA File Offset: 0x0000F7AA
		public static ObjectReferenceValue CreateMarshaler2(object value)
		{
			return ComWrappersSupport.CreateCCWForObjectForMarshaling(value, IReferenceArray<T>.PIID);
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x000115B7 File Offset: 0x0000F7B7
		public static IntPtr GetAbi(IObjectReference m)
		{
			if (m == null)
			{
				return IntPtr.Zero;
			}
			return m.ThisPtr;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x000115C8 File Offset: 0x0000F7C8
		public static object FromAbi(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}
			IReferenceArray<T>.Vftbl vftbl = new IReferenceArray<T>.Vftbl(ptr);
			return new IReferenceArray<T>(ObjectReference<IReferenceArray<T>.Vftbl>.FromAbi(ptr, vftbl)).Value;
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00011600 File Offset: 0x0000F800
		internal unsafe static object GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			int num = 0;
			IntPtr intPtr = default(IntPtr);
			object result;
			try
			{
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref IReferenceArray<T>.PIID, ref zero));
				method system.Int32_u0020(System.IntPtr,System.Int32*,System.IntPtr*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32*,System.IntPtr*), zero, &num, &intPtr, system.Int32_u0020(System.IntPtr,System.Int32*,System.IntPtr*)));
				result = Marshaler<T>.FromAbiArray.Invoke(new ValueTuple<int, IntPtr>(num, intPtr));
			}
			finally
			{
				Marshaler<T>.DisposeAbiArray.Invoke(new ValueTuple<int, IntPtr>(num, intPtr));
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x000116A4 File Offset: 0x0000F8A4
		public unsafe static void CopyManaged(object o, IntPtr dest)
		{
			*(IntPtr*)dest.ToPointer() = IReferenceArray<T>.CreateMarshaler2(o).Detach();
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x000116C8 File Offset: 0x0000F8C8
		public static IntPtr FromManaged(object value)
		{
			if (value == null)
			{
				return IntPtr.Zero;
			}
			return IReferenceArray<T>.CreateMarshaler2(value).Detach();
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x000116EC File Offset: 0x0000F8EC
		public static void DisposeMarshaler(IObjectReference m)
		{
			if (m != null)
			{
				m.Dispose();
			}
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x000116F7 File Offset: 0x0000F8F7
		public static void DisposeAbi(IntPtr abi)
		{
			MarshalInspectable<object>.DisposeAbi(abi);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x000116FF File Offset: 0x0000F8FF
		public static string GetGuidSignature()
		{
			return GuidGenerator.GetSignature(typeof(IReferenceArray<T>));
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00011710 File Offset: 0x0000F910
		public static implicit operator IReferenceArray<T>(IObjectReference obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IReferenceArray<T>(obj);
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x0001171D File Offset: 0x0000F91D
		public static implicit operator IReferenceArray<T>(ObjectReference<IReferenceArray<T>.Vftbl> obj)
		{
			if (obj == null)
			{
				return null;
			}
			return new IReferenceArray<T>(obj);
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0001172A File Offset: 0x0000F92A
		public IntPtr ThisPtr
		{
			get
			{
				return this._obj.ThisPtr;
			}
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x00011737 File Offset: 0x0000F937
		public ObjectReference<I> AsInterface<I>()
		{
			return this._obj.As<I>();
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00011744 File Offset: 0x0000F944
		public A As<A>()
		{
			return this._obj.AsType<A>();
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x00011751 File Offset: 0x0000F951
		public IReferenceArray(IObjectReference obj) : this(obj.As<IReferenceArray<T>.Vftbl>())
		{
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x0001175F File Offset: 0x0000F95F
		public IReferenceArray(ObjectReference<IReferenceArray<T>.Vftbl> obj)
		{
			this._obj = obj;
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x00011770 File Offset: 0x0000F970
		public T[] Value
		{
			get
			{
				int num = 0;
				IntPtr intPtr = default(IntPtr);
				T[] result;
				try
				{
					method getValue_ = this._obj.Vftbl.GetValue_0;
					ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32& modreq(System.Runtime.InteropServices.OutAttribute),System.IntPtr& modreq(System.Runtime.InteropServices.OutAttribute)), this.ThisPtr, ref num, ref intPtr, getValue_));
					result = Marshaler<T>.FromAbiArray.Invoke(new ValueTuple<int, IntPtr>(num, intPtr));
				}
				finally
				{
					Marshaler<T>.DisposeAbiArray.Invoke(new ValueTuple<int, IntPtr>(num, intPtr));
				}
				return result;
			}
		}

		// Token: 0x06000702 RID: 1794 RVA: 0x000117F4 File Offset: 0x0000F9F4
		// Note: this type is marked as 'beforefieldinit'.
		static IReferenceArray()
		{
			IReferenceArray<T>.PIID = IReferenceArray<T>.Vftbl.PIID;
		}

		// Token: 0x0400013F RID: 319
		public static Guid PIID;

		// Token: 0x04000140 RID: 320
		private readonly ObjectReference<IReferenceArray<T>.Vftbl> _obj;

		// Token: 0x020002F5 RID: 757
		[Guid("61C17707-2D65-11E0-9AE8-D48564015472")]
		public struct Vftbl
		{
			// Token: 0x17000437 RID: 1079
			// (get) Token: 0x06001678 RID: 5752 RVA: 0x0003FCC8 File Offset: 0x0003DEC8
			// (set) Token: 0x06001679 RID: 5753 RVA: 0x0003FCD0 File Offset: 0x0003DED0
			internal method GetValue_0
			{
				get
				{
					return this._get_Value_0;
				}
				set
				{
					this._get_Value_0 = value;
				}
			}

			// Token: 0x0600167A RID: 5754 RVA: 0x0003FCDC File Offset: 0x0003DEDC
			internal unsafe Vftbl(IntPtr thisPtr)
			{
				this = default(IReferenceArray<T>.Vftbl);
				VftblPtr vftblPtr = Marshal.PtrToStructure<VftblPtr>(thisPtr);
				IntPtr* ptr = (IntPtr*)((void*)vftblPtr.Vftbl);
				this.IInspectableVftbl = Marshal.PtrToStructure<IInspectable.Vftbl>(vftblPtr.Vftbl);
				this.GetValue_0 = (void*)ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)];
			}

			// Token: 0x0600167B RID: 5755 RVA: 0x0003FD2B File Offset: 0x0003DF2B
			// Note: this type is marked as 'beforefieldinit'.
			static Vftbl()
			{
				IReferenceArray<T>.Vftbl.PIID = GuidGenerator.CreateIID(typeof(IReferenceArray<T>));
			}

			// Token: 0x0400078C RID: 1932
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400078D RID: 1933
			internal unsafe void* _get_Value_0;

			// Token: 0x0400078E RID: 1934
			public static Guid PIID;
		}
	}
}
