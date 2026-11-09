using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x02000131 RID: 305
	[Guid("06dccc90-a058-5c88-87b7-6f3360a2fc16")]
	internal static class Nullable_Object
	{
		// Token: 0x06000A23 RID: 2595 RVA: 0x0001C87E File Offset: 0x0001AA7E
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};cinterface(IInspectable))";
		}

		// Token: 0x040001D6 RID: 470
		internal static Guid IID = new Guid(115133584U, 41048, 23688, 135, 183, 111, 51, 96, 162, 252, 22);

		// Token: 0x02000347 RID: 839
		[Guid("06dccc90-a058-5c88-87b7-6f3360a2fc16")]
		public struct Vftbl
		{
			// Token: 0x17000498 RID: 1176
			// (get) Token: 0x06001813 RID: 6163 RVA: 0x00043684 File Offset: 0x00041884
			// (set) Token: 0x06001814 RID: 6164 RVA: 0x0004368C File Offset: 0x0004188C
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

			// Token: 0x06001815 RID: 6165 RVA: 0x00043698 File Offset: 0x00041898
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_Object.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_Object.Vftbl>(Nullable_Object.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_Object.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001816 RID: 6166 RVA: 0x0004370C File Offset: 0x0004190C
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, IntPtr* __return_value__)
			{
				*__return_value__ = default(IntPtr);
				try
				{
					object obj = ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = MarshalInspectable<object>.FromManaged(obj, true);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000871 RID: 2161
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000872 RID: 2162
			private unsafe void* _Get_Value_0;

			// Token: 0x04000873 RID: 2163
			private static readonly Nullable_Object.Vftbl AbiToProjectionVftable = new Nullable_Object.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x04000874 RID: 2164
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
