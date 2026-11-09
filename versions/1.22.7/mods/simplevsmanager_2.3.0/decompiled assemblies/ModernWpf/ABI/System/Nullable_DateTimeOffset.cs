using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x0200012F RID: 303
	[Guid("5541d8a7-497c-5aa4-86fc-7713adbf2a2c")]
	internal static class Nullable_DateTimeOffset
	{
		// Token: 0x06000A1D RID: 2589 RVA: 0x0001C6F5 File Offset: 0x0001A8F5
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};struct(Windows.Foundation.DateTime;i8))";
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0001C6FC File Offset: 0x0001A8FC
		internal unsafe static DateTimeOffset GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			DateTimeOffset dateTimeOffset = default(DateTimeOffset);
			DateTimeOffset result;
			try
			{
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_DateTimeOffset.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,ABI.System.DateTimeOffset*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,ABI.System.DateTimeOffset*), zero, &dateTimeOffset, system.Int32_u0020(System.IntPtr,ABI.System.DateTimeOffset*)));
				result = DateTimeOffset.FromAbi(dateTimeOffset);
			}
			finally
			{
				DateTimeOffset.DisposeAbi(dateTimeOffset);
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001D4 RID: 468
		internal static Guid IID = new Guid(1430378663, 18812, 23204, 134, 252, 119, 19, 173, 191, 42, 44);

		// Token: 0x02000345 RID: 837
		[Guid("5541d8a7-497c-5aa4-86fc-7713adbf2a2c")]
		public struct Vftbl
		{
			// Token: 0x17000496 RID: 1174
			// (get) Token: 0x0600180B RID: 6155 RVA: 0x000434C4 File Offset: 0x000416C4
			// (set) Token: 0x0600180C RID: 6156 RVA: 0x000434CC File Offset: 0x000416CC
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

			// Token: 0x0600180D RID: 6157 RVA: 0x000434D8 File Offset: 0x000416D8
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_DateTimeOffset.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_DateTimeOffset.Vftbl>(Nullable_DateTimeOffset.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_DateTimeOffset.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x0600180E RID: 6158 RVA: 0x0004354C File Offset: 0x0004174C
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, DateTimeOffset* __return_value__)
			{
				DateTimeOffset value = default(DateTimeOffset);
				*__return_value__ = default(DateTimeOffset);
				try
				{
					value = (DateTimeOffset)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = DateTimeOffset.FromManaged(value);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000869 RID: 2153
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400086A RID: 2154
			private unsafe void* _Get_Value_0;

			// Token: 0x0400086B RID: 2155
			private static readonly Nullable_DateTimeOffset.Vftbl AbiToProjectionVftable = new Nullable_DateTimeOffset.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x0400086C RID: 2156
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
