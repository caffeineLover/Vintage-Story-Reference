using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x0200012E RID: 302
	[Guid("7d50f649-632c-51f9-849a-ee49428933ea")]
	internal static class Nullable_guid
	{
		// Token: 0x06000A1A RID: 2586 RVA: 0x0001C632 File Offset: 0x0001A832
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};g16)";
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0001C63C File Offset: 0x0001A83C
		internal unsafe static Guid GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			Guid result;
			try
			{
				Guid guid = default(Guid);
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_guid.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,System.Guid*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Guid*), zero, &guid, system.Int32_u0020(System.IntPtr,System.Guid*)));
				result = guid;
			}
			finally
			{
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001D3 RID: 467
		internal static Guid IID = new Guid(2102457929, 25388, 20985, 132, 154, 238, 73, 66, 137, 51, 234);

		// Token: 0x02000344 RID: 836
		[Guid("7d50f649-632c-51f9-849a-ee49428933ea")]
		public struct Vftbl
		{
			// Token: 0x17000495 RID: 1173
			// (get) Token: 0x06001807 RID: 6151 RVA: 0x000433E8 File Offset: 0x000415E8
			// (set) Token: 0x06001808 RID: 6152 RVA: 0x000433F0 File Offset: 0x000415F0
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

			// Token: 0x06001809 RID: 6153 RVA: 0x000433FC File Offset: 0x000415FC
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_guid.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_guid.Vftbl>(Nullable_guid.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_guid.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x0600180A RID: 6154 RVA: 0x00043470 File Offset: 0x00041670
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, Guid* __return_value__)
			{
				Guid guid = default(Guid);
				*__return_value__ = default(Guid);
				try
				{
					guid = (Guid)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = guid;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000865 RID: 2149
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000866 RID: 2150
			private unsafe void* _Get_Value_0;

			// Token: 0x04000867 RID: 2151
			private static readonly Nullable_guid.Vftbl AbiToProjectionVftable = new Nullable_guid.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x04000868 RID: 2152
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
