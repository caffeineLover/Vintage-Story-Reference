using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x02000126 RID: 294
	[Guid("5ab7d2c3-6b62-5e71-a4b6-2d49c4f238fd")]
	internal static class Nullable_ushort
	{
		// Token: 0x06000A02 RID: 2562 RVA: 0x0001C06A File Offset: 0x0001A26A
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};u2)";
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0001C074 File Offset: 0x0001A274
		internal unsafe static ushort GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			ushort result;
			try
			{
				ushort num = 0;
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_ushort.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,System.UInt16*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt16*), zero, &num, system.Int32_u0020(System.IntPtr,System.UInt16*)));
				result = num;
			}
			finally
			{
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001CB RID: 459
		internal static Guid IID = new Guid(1521996483, 27490, 24177, 164, 182, 45, 73, 196, 242, 56, 253);

		// Token: 0x0200033C RID: 828
		[Guid("5ab7d2c3-6b62-5e71-a4b6-2d49c4f238fd")]
		public struct Vftbl
		{
			// Token: 0x1700048D RID: 1165
			// (get) Token: 0x060017E7 RID: 6119 RVA: 0x00042D68 File Offset: 0x00040F68
			// (set) Token: 0x060017E8 RID: 6120 RVA: 0x00042D70 File Offset: 0x00040F70
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

			// Token: 0x060017E9 RID: 6121 RVA: 0x00042D7C File Offset: 0x00040F7C
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_ushort.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_ushort.Vftbl>(Nullable_ushort.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_ushort.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060017EA RID: 6122 RVA: 0x00042DF0 File Offset: 0x00040FF0
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, ushort* __return_value__)
			{
				*__return_value__ = 0;
				try
				{
					ushort num = (ushort)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = num;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000845 RID: 2117
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000846 RID: 2118
			private unsafe void* _Get_Value_0;

			// Token: 0x04000847 RID: 2119
			private static readonly Nullable_ushort.Vftbl AbiToProjectionVftable = new Nullable_ushort.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x04000848 RID: 2120
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
