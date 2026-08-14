using System;
using System.Runtime.InteropServices;
using ABI.System;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000B3 RID: 179
	internal static class BoxedValueIReferenceImpl<T>
	{
		// Token: 0x06000707 RID: 1799 RVA: 0x00011904 File Offset: 0x0000FB04
		unsafe static BoxedValueIReferenceImpl()
		{
			BoxedValueIReferenceImpl<T>.AbiToProjectionVftable = new Nullable<T>.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				get_Value_0 = Delegate.CreateDelegate(Nullable<T>.Vftbl.get_Value_0_Type, typeof(BoxedValueIReferenceImpl<T>).GetMethod("Do_Abi_get_Value_0", 40).MakeGenericMethod(new Type[]
				{
					Marshaler<T>.AbiType
				}))
			};
			IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(BoxedValueIReferenceImpl<T>), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
			Marshal.StructureToPtr<IInspectable.Vftbl>(BoxedValueIReferenceImpl<T>.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
			ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate(BoxedValueIReferenceImpl<T>.AbiToProjectionVftable.get_Value_0);
			BoxedValueIReferenceImpl<T>.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x000119C4 File Offset: 0x0000FBC4
		private unsafe static int Do_Abi_get_Value_0<TAbi>(void* thisPtr, out TAbi __return_value__)
		{
			T t = default(T);
			__return_value__ = default(TAbi);
			try
			{
				t = (T)((object)ComWrappersSupport.FindObject<object>(new IntPtr(thisPtr)));
				__return_value__ = (TAbi)((object)Marshaler<T>.FromManaged.Invoke(t));
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x04000146 RID: 326
		private static Nullable<T>.Vftbl AbiToProjectionVftable;

		// Token: 0x04000147 RID: 327
		public static IntPtr AbiToProjectionVftablePtr;
	}
}
