using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x0200012C RID: 300
	[Guid("fb393ef3-bbac-5bd5-9144-84f23576f415")]
	internal static class Nullable_char
	{
		// Token: 0x06000A14 RID: 2580 RVA: 0x0001C4C4 File Offset: 0x0001A6C4
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};c2)";
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0001C4CC File Offset: 0x0001A6CC
		internal unsafe static char GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			char result;
			try
			{
				char c = '\0';
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_char.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,System.Char*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,System.Char*), zero, &c, system.Int32_u0020(System.IntPtr,System.Char*)));
				result = c;
			}
			finally
			{
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001D1 RID: 465
		internal static Guid IID = new Guid(4214832883U, 48044, 23509, 145, 68, 132, 242, 53, 118, 244, 21);

		// Token: 0x02000342 RID: 834
		[Guid("fb393ef3-bbac-5bd5-9144-84f23576f415")]
		public struct Vftbl
		{
			// Token: 0x17000493 RID: 1171
			// (get) Token: 0x060017FF RID: 6143 RVA: 0x00043250 File Offset: 0x00041450
			// (set) Token: 0x06001800 RID: 6144 RVA: 0x00043258 File Offset: 0x00041458
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

			// Token: 0x06001801 RID: 6145 RVA: 0x00043264 File Offset: 0x00041464
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_char.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_char.Vftbl>(Nullable_char.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_char.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001802 RID: 6146 RVA: 0x000432D8 File Offset: 0x000414D8
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, char* __return_value__)
			{
				*__return_value__ = '\0';
				try
				{
					char c = (char)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = c;
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0400085D RID: 2141
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400085E RID: 2142
			private unsafe void* _Get_Value_0;

			// Token: 0x0400085F RID: 2143
			private static readonly Nullable_char.Vftbl AbiToProjectionVftable = new Nullable_char.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x04000860 RID: 2144
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
