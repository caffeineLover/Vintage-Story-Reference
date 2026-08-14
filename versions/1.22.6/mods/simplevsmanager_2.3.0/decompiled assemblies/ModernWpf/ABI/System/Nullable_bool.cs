using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x0200012D RID: 301
	[Guid("3c00fd60-2950-5939-a21a-2d12c5a01b8a")]
	internal static class Nullable_bool
	{
		// Token: 0x06000A17 RID: 2583 RVA: 0x0001C57A File Offset: 0x0001A77A
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};b1)";
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0001C584 File Offset: 0x0001A784
		internal unsafe static bool GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			bool result;
			try
			{
				bool flag = false;
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_bool.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,System.Boolean*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Boolean*), zero, &flag, system.Int32_u0020(System.IntPtr,System.Boolean*)));
				result = flag;
			}
			finally
			{
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001D2 RID: 466
		internal static Guid IID = new Guid(1006697824, 10576, 22841, 162, 26, 45, 18, 197, 160, 27, 138);

		// Token: 0x02000343 RID: 835
		[Guid("3c00fd60-2950-5939-a21a-2d12c5a01b8a")]
		public struct Vftbl
		{
			// Token: 0x17000494 RID: 1172
			// (get) Token: 0x06001803 RID: 6147 RVA: 0x0004331C File Offset: 0x0004151C
			// (set) Token: 0x06001804 RID: 6148 RVA: 0x00043324 File Offset: 0x00041524
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

			// Token: 0x06001805 RID: 6149 RVA: 0x00043330 File Offset: 0x00041530
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_bool.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_bool.Vftbl>(Nullable_bool.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_bool.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001806 RID: 6150 RVA: 0x000433A4 File Offset: 0x000415A4
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, bool* __return_value__)
			{
				*__return_value__ = false;
				try
				{
					bool flag = (bool)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = flag;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000861 RID: 2145
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000862 RID: 2146
			private unsafe void* _Get_Value_0;

			// Token: 0x04000863 RID: 2147
			private static readonly Nullable_bool.Vftbl AbiToProjectionVftable = new Nullable_bool.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x04000864 RID: 2148
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
