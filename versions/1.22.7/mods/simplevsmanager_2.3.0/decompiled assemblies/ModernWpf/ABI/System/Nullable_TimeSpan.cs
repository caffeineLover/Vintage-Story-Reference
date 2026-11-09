using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x02000130 RID: 304
	[Guid("604d0c4c-91de-5c2a-935f-362f13eaf800")]
	internal static class Nullable_TimeSpan
	{
		// Token: 0x06000A20 RID: 2592 RVA: 0x0001C7BA File Offset: 0x0001A9BA
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};struct(Windows.Foundation.TimeSpan;i8))";
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0001C7C4 File Offset: 0x0001A9C4
		internal unsafe static TimeSpan GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			TimeSpan timeSpan = default(TimeSpan);
			TimeSpan result;
			try
			{
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_TimeSpan.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,ABI.System.TimeSpan*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,ABI.System.TimeSpan*), zero, &timeSpan, system.Int32_u0020(System.IntPtr,ABI.System.TimeSpan*)));
				result = TimeSpan.FromAbi(timeSpan);
			}
			finally
			{
				TimeSpan.DisposeAbi(timeSpan);
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001D5 RID: 469
		internal static Guid IID = new Guid(1615662156U, 37342, 23594, 147, 95, 54, 47, 19, 234, 248, 0);

		// Token: 0x02000346 RID: 838
		[Guid("604d0c4c-91de-5c2a-935f-362f13eaf800")]
		public struct Vftbl
		{
			// Token: 0x17000497 RID: 1175
			// (get) Token: 0x0600180F RID: 6159 RVA: 0x000435A4 File Offset: 0x000417A4
			// (set) Token: 0x06001810 RID: 6160 RVA: 0x000435AC File Offset: 0x000417AC
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

			// Token: 0x06001811 RID: 6161 RVA: 0x000435B8 File Offset: 0x000417B8
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_TimeSpan.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_TimeSpan.Vftbl>(Nullable_TimeSpan.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_TimeSpan.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001812 RID: 6162 RVA: 0x0004362C File Offset: 0x0004182C
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, TimeSpan* __return_value__)
			{
				TimeSpan value = default(TimeSpan);
				*__return_value__ = default(TimeSpan);
				try
				{
					value = (TimeSpan)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = TimeSpan.FromManaged(value);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0400086D RID: 2157
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400086E RID: 2158
			private unsafe void* _Get_Value_0;

			// Token: 0x0400086F RID: 2159
			private static readonly Nullable_TimeSpan.Vftbl AbiToProjectionVftable = new Nullable_TimeSpan.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x04000870 RID: 2160
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
