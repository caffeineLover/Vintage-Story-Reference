using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000AF RID: 175
	internal static class BoxedArrayIReferenceArrayImpl<T>
	{
		// Token: 0x060006EE RID: 1774 RVA: 0x0001148C File Offset: 0x0000F68C
		unsafe static BoxedArrayIReferenceArrayImpl()
		{
			BoxedArrayIReferenceArrayImpl<T>.AbiToProjectionVftable = new IReferenceArray<T>.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_get_Value_0 = (void*)Marshal.GetFunctionPointerForDelegate(BoxedArrayIReferenceArrayImpl<T>.DelegateCache = new IReferenceArray_Delegates.get_Value_0(BoxedArrayIReferenceArrayImpl<T>.Do_Abi_get_Value_0))
			};
			IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(BoxedArrayIReferenceArrayImpl<T>), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
			Marshal.StructureToPtr<IInspectable.Vftbl>(BoxedArrayIReferenceArrayImpl<T>.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
			ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = (IntPtr)BoxedArrayIReferenceArrayImpl<T>.AbiToProjectionVftable.GetValue_0;
			BoxedArrayIReferenceArrayImpl<T>.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x00011534 File Offset: 0x0000F734
		private static int Do_Abi_get_Value_0(IntPtr thisPtr, out int ____return_value__Size, out IntPtr __return_value__)
		{
			__return_value__ = default(IntPtr);
			____return_value__Size = 0;
			try
			{
				T[] array = (T[])ComWrappersSupport.FindObject<object>(thisPtr);
				ValueTuple<int, IntPtr> valueTuple = Marshaler<T>.FromManagedArray.Invoke(array);
				____return_value__Size = valueTuple.Item1;
				__return_value__ = valueTuple.Item2;
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0400013C RID: 316
		private static readonly IReferenceArray<T>.Vftbl AbiToProjectionVftable;

		// Token: 0x0400013D RID: 317
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x0400013E RID: 318
		private static readonly Delegate DelegateCache;
	}
}
