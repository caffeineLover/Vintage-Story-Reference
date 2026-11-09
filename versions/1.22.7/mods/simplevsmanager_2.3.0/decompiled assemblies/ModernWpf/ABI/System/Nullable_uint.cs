using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x02000127 RID: 295
	[Guid("513ef3af-e784-5325-a91e-97c2b8111cf3")]
	internal static class Nullable_uint
	{
		// Token: 0x06000A05 RID: 2565 RVA: 0x0001C125 File Offset: 0x0001A325
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};u4)";
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0001C12C File Offset: 0x0001A32C
		internal unsafe static uint GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			uint result;
			try
			{
				uint num = 0U;
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_uint.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,System.UInt32*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.UInt32*), zero, &num, system.Int32_u0020(System.IntPtr,System.UInt32*)));
				result = num;
			}
			finally
			{
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001CC RID: 460
		internal static Guid IID = new Guid(1363080111U, 59268, 21285, 169, 30, 151, 194, 184, 17, 28, 243);

		// Token: 0x0200033D RID: 829
		[Guid("513ef3af-e784-5325-a91e-97c2b8111cf3")]
		public struct Vftbl
		{
			// Token: 0x1700048E RID: 1166
			// (get) Token: 0x060017EB RID: 6123 RVA: 0x00042E34 File Offset: 0x00041034
			// (set) Token: 0x060017EC RID: 6124 RVA: 0x00042E3C File Offset: 0x0004103C
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

			// Token: 0x060017ED RID: 6125 RVA: 0x00042E48 File Offset: 0x00041048
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_uint.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_uint.Vftbl>(Nullable_uint.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_uint.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x060017EE RID: 6126 RVA: 0x00042EBC File Offset: 0x000410BC
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, uint* __return_value__)
			{
				*__return_value__ = 0U;
				try
				{
					uint num = (uint)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = num;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000849 RID: 2121
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400084A RID: 2122
			private unsafe void* _Get_Value_0;

			// Token: 0x0400084B RID: 2123
			private static readonly Nullable_uint.Vftbl AbiToProjectionVftable = new Nullable_uint.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x0400084C RID: 2124
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
