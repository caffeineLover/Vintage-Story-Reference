using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x02000129 RID: 297
	[Guid("6755e376-53bb-568b-a11d-17239868309e")]
	internal static class Nullable_ulong
	{
		// Token: 0x06000A0B RID: 2571 RVA: 0x0001C292 File Offset: 0x0001A492
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};u8)";
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0001C29C File Offset: 0x0001A49C
		internal unsafe static ulong GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			ulong result;
			try
			{
				ulong num = 0UL;
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_ulong.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,System.UInt64*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt64*), zero, &num, system.Int32_u0020(System.IntPtr,System.UInt64*)));
				result = num;
			}
			finally
			{
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001CE RID: 462
		internal static Guid IID = new Guid(1733682038, 21435, 22155, 161, 29, 23, 35, 152, 104, 48, 158);

		// Token: 0x0200033F RID: 831
		[Guid("6755e376-53bb-568b-a11d-17239868309e")]
		public struct Vftbl
		{
			// Token: 0x17000490 RID: 1168
			// (get) Token: 0x060017F3 RID: 6131 RVA: 0x00042FD0 File Offset: 0x000411D0
			// (set) Token: 0x060017F4 RID: 6132 RVA: 0x00042FD8 File Offset: 0x000411D8
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

			// Token: 0x060017F5 RID: 6133 RVA: 0x00042FE4 File Offset: 0x000411E4
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_ulong.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_ulong.Vftbl>(Nullable_ulong.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_ulong.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060017F6 RID: 6134 RVA: 0x00043058 File Offset: 0x00041258
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, ulong* __return_value__)
			{
				*__return_value__ = 0UL;
				try
				{
					ulong num = (ulong)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = num;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000851 RID: 2129
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000852 RID: 2130
			private unsafe void* _Get_Value_0;

			// Token: 0x04000853 RID: 2131
			private static readonly Nullable_ulong.Vftbl AbiToProjectionVftable = new Nullable_ulong.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x04000854 RID: 2132
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
