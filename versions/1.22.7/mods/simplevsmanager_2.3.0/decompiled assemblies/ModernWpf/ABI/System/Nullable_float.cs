using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x0200012A RID: 298
	[Guid("719cc2ba-3e76-5def-9f1a-38d85a145ea8")]
	internal static class Nullable_float
	{
		// Token: 0x06000A0E RID: 2574 RVA: 0x0001C347 File Offset: 0x0001A547
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};f4)";
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0001C350 File Offset: 0x0001A550
		internal unsafe static float GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			float result;
			try
			{
				float num = 0f;
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_float.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,System.Single*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Single*), zero, &num, system.Int32_u0020(System.IntPtr,System.Single*)));
				result = num;
			}
			finally
			{
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001CF RID: 463
		internal static Guid IID = new Guid(1906098874, 15990, 24047, 159, 26, 56, 216, 90, 20, 94, 168);

		// Token: 0x02000340 RID: 832
		[Guid("719cc2ba-3e76-5def-9f1a-38d85a145ea8")]
		public struct Vftbl
		{
			// Token: 0x17000491 RID: 1169
			// (get) Token: 0x060017F7 RID: 6135 RVA: 0x000430A0 File Offset: 0x000412A0
			// (set) Token: 0x060017F8 RID: 6136 RVA: 0x000430A8 File Offset: 0x000412A8
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

			// Token: 0x060017F9 RID: 6137 RVA: 0x000430B4 File Offset: 0x000412B4
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_float.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_float.Vftbl>(Nullable_float.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_float.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060017FA RID: 6138 RVA: 0x00043128 File Offset: 0x00041328
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, float* __return_value__)
			{
				*__return_value__ = 0f;
				try
				{
					float num = (float)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = num;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000855 RID: 2133
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000856 RID: 2134
			private unsafe void* _Get_Value_0;

			// Token: 0x04000857 RID: 2135
			private static readonly Nullable_float.Vftbl AbiToProjectionVftable = new Nullable_float.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x04000858 RID: 2136
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
