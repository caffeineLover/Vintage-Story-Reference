using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x02000124 RID: 292
	[Guid("95500129-fbf6-5afc-89df-70642d741990")]
	internal static class Nullable_sbyte
	{
		// Token: 0x060009FC RID: 2556 RVA: 0x0001BF04 File Offset: 0x0001A104
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};i1)";
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0001BF0C File Offset: 0x0001A10C
		internal unsafe static sbyte GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			sbyte result;
			try
			{
				sbyte b = 0;
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_sbyte.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,System.SByte*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.SByte*), zero, &b, system.Int32_u0020(System.IntPtr,System.SByte*)));
				result = b;
			}
			finally
			{
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001C9 RID: 457
		internal static Guid IID = new Guid(2505048361U, 64502, 23292, 137, 223, 112, 100, 45, 116, 25, 144);

		// Token: 0x0200033A RID: 826
		[Guid("95500129-fbf6-5afc-89df-70642d741990")]
		public struct Vftbl
		{
			// Token: 0x1700048B RID: 1163
			// (get) Token: 0x060017DF RID: 6111 RVA: 0x00042BD0 File Offset: 0x00040DD0
			// (set) Token: 0x060017E0 RID: 6112 RVA: 0x00042BD8 File Offset: 0x00040DD8
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

			// Token: 0x060017E1 RID: 6113 RVA: 0x00042BE4 File Offset: 0x00040DE4
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_sbyte.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_sbyte.Vftbl>(Nullable_sbyte.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_sbyte.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060017E2 RID: 6114 RVA: 0x00042C58 File Offset: 0x00040E58
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, sbyte* __return_value__)
			{
				*__return_value__ = 0;
				try
				{
					sbyte b = (sbyte)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = b;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0400083D RID: 2109
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400083E RID: 2110
			private unsafe void* _Get_Value_0;

			// Token: 0x0400083F RID: 2111
			private static readonly Nullable_sbyte.Vftbl AbiToProjectionVftable = new Nullable_sbyte.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x04000840 RID: 2112
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
