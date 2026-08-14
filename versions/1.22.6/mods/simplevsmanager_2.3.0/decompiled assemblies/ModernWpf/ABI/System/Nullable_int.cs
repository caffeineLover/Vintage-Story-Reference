using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x02000121 RID: 289
	[Guid("548cefbd-bc8a-5fa0-8df2-957440fc8bf4")]
	internal static class Nullable_int
	{
		// Token: 0x060009F3 RID: 2547 RVA: 0x0001BCBF File Offset: 0x00019EBF
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};i4)";
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0001BCC8 File Offset: 0x00019EC8
		internal unsafe static int GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			int result;
			try
			{
				int num = 0;
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_int.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,System.Int32*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Int32*), zero, &num, system.Int32_u0020(System.IntPtr,System.Int32*)));
				result = num;
			}
			finally
			{
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001C6 RID: 454
		internal static Guid IID = new Guid(1418522557U, 48266, 24480, 141, 242, 149, 116, 64, 252, 139, 244);

		// Token: 0x02000337 RID: 823
		[Guid("548cefbd-bc8a-5fa0-8df2-957440fc8bf4")]
		public struct Vftbl
		{
			// Token: 0x17000488 RID: 1160
			// (get) Token: 0x060017D3 RID: 6099 RVA: 0x00042960 File Offset: 0x00040B60
			// (set) Token: 0x060017D4 RID: 6100 RVA: 0x00042968 File Offset: 0x00040B68
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

			// Token: 0x060017D5 RID: 6101 RVA: 0x00042974 File Offset: 0x00040B74
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_int.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_int.Vftbl>(Nullable_int.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_int.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060017D6 RID: 6102 RVA: 0x000429E8 File Offset: 0x00040BE8
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, int* __return_value__)
			{
				*__return_value__ = 0;
				try
				{
					int num = (int)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = num;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000831 RID: 2097
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000832 RID: 2098
			private unsafe void* _Get_Value_0;

			// Token: 0x04000833 RID: 2099
			private static readonly Nullable_int.Vftbl AbiToProjectionVftable = new Nullable_int.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x04000834 RID: 2100
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
