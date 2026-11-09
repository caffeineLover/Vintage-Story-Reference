using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x02000122 RID: 290
	[Guid("fd416dfb-2a07-52eb-aae3-dfce14116c05")]
	internal static class Nullable_string
	{
		// Token: 0x060009F6 RID: 2550 RVA: 0x0001BD7C File Offset: 0x00019F7C
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};string)";
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0001BD84 File Offset: 0x00019F84
		internal unsafe static Nullable GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr = default(IntPtr);
			Nullable result;
			try
			{
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_string.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,System.IntPtr*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.IntPtr*), zero, &intPtr, system.Int32_u0020(System.IntPtr,System.IntPtr*)));
				result = new Nullable(MarshalString.FromAbi(intPtr));
			}
			finally
			{
				MarshalString.DisposeAbi(intPtr);
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001C7 RID: 455
		internal static Guid IID = new Guid(4248923643U, 10759, 21227, 170, 227, 223, 206, 20, 17, 108, 5);

		// Token: 0x02000338 RID: 824
		[Guid("fd416dfb-2a07-52eb-aae3-dfce14116c05")]
		public struct Vftbl
		{
			// Token: 0x17000489 RID: 1161
			// (get) Token: 0x060017D7 RID: 6103 RVA: 0x00042A2C File Offset: 0x00040C2C
			// (set) Token: 0x060017D8 RID: 6104 RVA: 0x00042A34 File Offset: 0x00040C34
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

			// Token: 0x060017D9 RID: 6105 RVA: 0x00042A40 File Offset: 0x00040C40
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_string.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_string.Vftbl>(Nullable_string.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_string.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060017DA RID: 6106 RVA: 0x00042AB4 File Offset: 0x00040CB4
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, IntPtr* __return_value__)
			{
				*__return_value__ = default(IntPtr);
				try
				{
					string value = (string)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = MarshalString.FromManaged(value);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000835 RID: 2101
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000836 RID: 2102
			private unsafe void* _Get_Value_0;

			// Token: 0x04000837 RID: 2103
			private static readonly Nullable_string.Vftbl AbiToProjectionVftable = new Nullable_string.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x04000838 RID: 2104
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
