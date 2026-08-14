using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x02000125 RID: 293
	[Guid("6ec9e41b-6709-5647-9918-a1270110fc4e")]
	internal static class Nullable_short
	{
		// Token: 0x060009FF RID: 2559 RVA: 0x0001BFB7 File Offset: 0x0001A1B7
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};i2)";
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0001BFC0 File Offset: 0x0001A1C0
		internal unsafe static short GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			short result;
			try
			{
				short num = 0;
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_short.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,System.Int16*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int16*), zero, &num, system.Int32_u0020(System.IntPtr,System.Int16*)));
				result = num;
			}
			finally
			{
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001CA RID: 458
		internal static Guid IID = new Guid(1858724891, 26377, 22087, 153, 24, 161, 39, 1, 16, 252, 78);

		// Token: 0x0200033B RID: 827
		[Guid("6ec9e41b-6709-5647-9918-a1270110fc4e")]
		public struct Vftbl
		{
			// Token: 0x1700048C RID: 1164
			// (get) Token: 0x060017E3 RID: 6115 RVA: 0x00042C9C File Offset: 0x00040E9C
			// (set) Token: 0x060017E4 RID: 6116 RVA: 0x00042CA4 File Offset: 0x00040EA4
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

			// Token: 0x060017E5 RID: 6117 RVA: 0x00042CB0 File Offset: 0x00040EB0
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_short.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_short.Vftbl>(Nullable_short.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_short.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060017E6 RID: 6118 RVA: 0x00042D24 File Offset: 0x00040F24
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, short* __return_value__)
			{
				*__return_value__ = 0;
				try
				{
					short num = (short)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = num;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000841 RID: 2113
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000842 RID: 2114
			private unsafe void* _Get_Value_0;

			// Token: 0x04000843 RID: 2115
			private static readonly Nullable_short.Vftbl AbiToProjectionVftable = new Nullable_short.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x04000844 RID: 2116
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
