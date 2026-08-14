using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x02000128 RID: 296
	[Guid("4dda9e24-e69f-5c6a-a0a6-93427365af2a")]
	internal static class Nullable_long
	{
		// Token: 0x06000A08 RID: 2568 RVA: 0x0001C1DD File Offset: 0x0001A3DD
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};i8)";
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0001C1E4 File Offset: 0x0001A3E4
		internal unsafe static long GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			long result;
			try
			{
				long num = 0L;
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_long.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,System.Int64*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int64*), zero, &num, system.Int32_u0020(System.IntPtr,System.Int64*)));
				result = num;
			}
			finally
			{
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001CD RID: 461
		internal static Guid IID = new Guid(1306172964U, 59039, 23658, 160, 166, 147, 66, 115, 101, 175, 42);

		// Token: 0x0200033E RID: 830
		[Guid("4dda9e24-e69f-5c6a-a0a6-93427365af2a")]
		public struct Vftbl
		{
			// Token: 0x1700048F RID: 1167
			// (get) Token: 0x060017EF RID: 6127 RVA: 0x00042F00 File Offset: 0x00041100
			// (set) Token: 0x060017F0 RID: 6128 RVA: 0x00042F08 File Offset: 0x00041108
			public method Get_Value_0
			{
				get
				{
					return this._Get_Value_0;
				}
				set
				{
					this._Get_Value_0 = value;
				}
			}

			// Token: 0x060017F1 RID: 6129 RVA: 0x00042F14 File Offset: 0x00041114
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_long.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_long.Vftbl>(Nullable_long.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_long.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060017F2 RID: 6130 RVA: 0x00042F88 File Offset: 0x00041188
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, long* __return_value__)
			{
				*__return_value__ = 0L;
				try
				{
					long num = (long)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = num;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0400084D RID: 2125
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400084E RID: 2126
			private unsafe void* _Get_Value_0;

			// Token: 0x0400084F RID: 2127
			private static readonly Nullable_long.Vftbl AbiToProjectionVftable = new Nullable_long.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x04000850 RID: 2128
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
