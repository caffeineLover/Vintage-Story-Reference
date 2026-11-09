using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x02000133 RID: 307
	[Guid("61C17706-2D65-11E0-9AE8-D48564015472")]
	internal static class Nullable_Delegate<T> where T : Delegate
	{
		// Token: 0x06000A28 RID: 2600 RVA: 0x0001C99D File Offset: 0x0001AB9D
		public static string GetGuidSignature()
		{
			return GuidGenerator.GetSignature(typeof(Nullable<T>));
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0001C9B0 File Offset: 0x0001ABB0
		internal unsafe static Nullable GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr = default(IntPtr);
			Nullable result;
			try
			{
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_Delegate<T>.PIID, ref zero));
				method system.Int32_u0020(System.IntPtr,System.IntPtr*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), zero, &intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr*)));
				result = new Nullable(Marshaler<T>.FromAbi.Invoke(intPtr));
			}
			finally
			{
				Marshaler<T>.DisposeAbi.Invoke(intPtr);
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0001CA4C File Offset: 0x0001AC4C
		// Note: this type is marked as 'beforefieldinit'.
		static Nullable_Delegate()
		{
			Nullable_Delegate<T>.PIID = Nullable_Delegate<T>.Vftbl.PIID;
		}

		// Token: 0x040001D8 RID: 472
		public static Guid PIID;

		// Token: 0x02000349 RID: 841
		[Guid("61C17706-2D65-11E0-9AE8-D48564015472")]
		public struct Vftbl
		{
			// Token: 0x0600181B RID: 6171 RVA: 0x00043834 File Offset: 0x00041A34
			unsafe static Vftbl()
			{
				Nullable_Delegate<T>.Vftbl.PIID = GuidGenerator.CreateIID(typeof(Nullable<T>));
				Nullable_Delegate<T>.Vftbl.AbiToProjectionVftable = new Nullable_Delegate<T>.Vftbl
				{
					IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
					_Get_Value_0 = new Nullable_Delegates.GetValueDelegate(Nullable_Delegate<T>.Vftbl.Do_Abi_get_Value_0)
				};
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_Delegate<T>.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<IInspectable.Vftbl>(Nullable_Delegate<T>.Vftbl.AbiToProjectionVftable.IInspectableVftbl, (IntPtr)((void*)ptr), false);
				ptr[(IntPtr)6 * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = Marshal.GetFunctionPointerForDelegate<Nullable_Delegates.GetValueDelegate>(Nullable_Delegate<T>.Vftbl.AbiToProjectionVftable._Get_Value_0);
				Nullable_Delegate<T>.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x0600181C RID: 6172 RVA: 0x000438E0 File Offset: 0x00041AE0
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, IntPtr* __return_value__)
			{
				T t = default(T);
				*__return_value__ = default(IntPtr);
				try
				{
					t = ComWrappersSupport.FindObject<T>(thisPtr);
					*__return_value__ = (IntPtr)Marshaler<T>.FromManaged.Invoke(t);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000879 RID: 2169
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400087A RID: 2170
			private Nullable_Delegates.GetValueDelegate _Get_Value_0;

			// Token: 0x0400087B RID: 2171
			public static Guid PIID;

			// Token: 0x0400087C RID: 2172
			private static readonly Nullable_Delegate<T>.Vftbl AbiToProjectionVftable;

			// Token: 0x0400087D RID: 2173
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
