using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x0200012B RID: 299
	[Guid("2f2d6c29-5473-5f3e-92e7-96572bb990e2")]
	internal static class Nullable_double
	{
		// Token: 0x06000A11 RID: 2577 RVA: 0x0001C3FF File Offset: 0x0001A5FF
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};f8)";
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0001C408 File Offset: 0x0001A608
		internal unsafe static double GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			double result;
			try
			{
				double num = 0.0;
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_double.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,System.Double*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Double*), zero, &num, system.Int32_u0020(System.IntPtr,System.Double*)));
				result = num;
			}
			finally
			{
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001D0 RID: 464
		internal static Guid IID = new Guid(791505961, 21619, 24382, 146, 231, 150, 87, 43, 185, 144, 226);

		// Token: 0x02000341 RID: 833
		[Guid("2f2d6c29-5473-5f3e-92e7-96572bb990e2")]
		public struct Vftbl
		{
			// Token: 0x17000492 RID: 1170
			// (get) Token: 0x060017FB RID: 6139 RVA: 0x00043174 File Offset: 0x00041374
			// (set) Token: 0x060017FC RID: 6140 RVA: 0x0004317C File Offset: 0x0004137C
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

			// Token: 0x060017FD RID: 6141 RVA: 0x00043188 File Offset: 0x00041388
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_double.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_double.Vftbl>(Nullable_double.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_double.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060017FE RID: 6142 RVA: 0x000431FC File Offset: 0x000413FC
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, double* __return_value__)
			{
				*__return_value__ = 0.0;
				try
				{
					double num = (double)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = num;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000859 RID: 2137
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400085A RID: 2138
			private unsafe void* _Get_Value_0;

			// Token: 0x0400085B RID: 2139
			private static readonly Nullable_double.Vftbl AbiToProjectionVftable = new Nullable_double.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x0400085C RID: 2140
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
