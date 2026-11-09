using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x02000123 RID: 291
	[Guid("e5198cc8-2873-55f5-b0a1-84ff9e4aad62")]
	internal static class Nullable_byte
	{
		// Token: 0x060009F9 RID: 2553 RVA: 0x0001BE49 File Offset: 0x0001A049
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};u1)";
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0001BE50 File Offset: 0x0001A050
		internal unsafe static byte GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			byte result;
			try
			{
				byte b = 0;
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_byte.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,System.Byte*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Byte*), zero, &b, system.Int32_u0020(System.IntPtr,System.Byte*)));
				result = b;
			}
			finally
			{
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001C8 RID: 456
		internal static Guid IID = new Guid(3843656904U, 10355, 22005, 176, 161, 132, byte.MaxValue, 158, 74, 173, 98);

		// Token: 0x02000339 RID: 825
		[Guid("e5198cc8-2873-55f5-b0a1-84ff9e4aad62")]
		public struct Vftbl
		{
			// Token: 0x1700048A RID: 1162
			// (get) Token: 0x060017DB RID: 6107 RVA: 0x00042B04 File Offset: 0x00040D04
			// (set) Token: 0x060017DC RID: 6108 RVA: 0x00042B0C File Offset: 0x00040D0C
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

			// Token: 0x060017DD RID: 6109 RVA: 0x00042B18 File Offset: 0x00040D18
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_byte.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_byte.Vftbl>(Nullable_byte.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_byte.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060017DE RID: 6110 RVA: 0x00042B8C File Offset: 0x00040D8C
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, byte* __return_value__)
			{
				*__return_value__ = 0;
				try
				{
					byte b = (byte)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = b;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000839 RID: 2105
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400083A RID: 2106
			private unsafe void* _Get_Value_0;

			// Token: 0x0400083B RID: 2107
			private static readonly Nullable_byte.Vftbl AbiToProjectionVftable = new Nullable_byte.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x0400083C RID: 2108
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
