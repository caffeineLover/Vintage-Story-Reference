using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.System
{
	// Token: 0x02000132 RID: 306
	[Guid("3830ad99-d8da-53f3-989b-fc92ad222778")]
	internal static class Nullable_Type
	{
		// Token: 0x06000A25 RID: 2597 RVA: 0x0001C8CA File Offset: 0x0001AACA
		public static string GetGuidSignature()
		{
			return "pinterface({61c17706-2d65-11e0-9ae8-d48564015472};struct(Windows.UI.Xaml.Interop.TypeName;string;enum(Windows.UI.Xaml.Interop.TypeKind;i4)))";
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0001C8D4 File Offset: 0x0001AAD4
		internal unsafe static Nullable GetValue(IInspectable inspectable)
		{
			IntPtr zero = IntPtr.Zero;
			Type type = default(Type);
			Nullable result;
			try
			{
				ExceptionHelpers.ThrowExceptionForHR(Marshal.QueryInterface(inspectable.ThisPtr, ref Nullable_Type.IID, ref zero));
				method system.Int32_u0020(System.IntPtr,ABI.System.Type*) = *(*(IntPtr*)((void*)zero) + (IntPtr)6 * (IntPtr)sizeof(method));
				ExceptionHelpers.ThrowExceptionForHR(calli(System.Int32(System.IntPtr,ABI.System.Type*), zero, &type, system.Int32_u0020(System.IntPtr,ABI.System.Type*)));
				result = new Nullable(Type.FromAbi(type));
			}
			finally
			{
				Type.DisposeAbi(type);
				Marshal.Release(zero);
			}
			return result;
		}

		// Token: 0x040001D7 RID: 471
		internal static Guid IID = new Guid(942714265U, 55514, 21491, 152, 155, 252, 146, 173, 34, 39, 120);

		// Token: 0x02000348 RID: 840
		[Guid("3830ad99-d8da-53f3-989b-fc92ad222778")]
		public struct Vftbl
		{
			// Token: 0x17000499 RID: 1177
			// (get) Token: 0x06001817 RID: 6167 RVA: 0x00043758 File Offset: 0x00041958
			// (set) Token: 0x06001818 RID: 6168 RVA: 0x00043760 File Offset: 0x00041960
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

			// Token: 0x06001819 RID: 6169 RVA: 0x0004376C File Offset: 0x0004196C
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(Nullable_Type.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
				Marshal.StructureToPtr<Nullable_Type.Vftbl>(Nullable_Type.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				Nullable_Type.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x0600181A RID: 6170 RVA: 0x000437E0 File Offset: 0x000419E0
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Value_0(IntPtr thisPtr, Type* __return_value__)
			{
				*__return_value__ = default(Type);
				try
				{
					Type value = (Type)ComWrappersSupport.FindObject<object>(thisPtr);
					*__return_value__ = Type.FromManaged(value);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x04000875 RID: 2165
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x04000876 RID: 2166
			private unsafe void* _Get_Value_0;

			// Token: 0x04000877 RID: 2167
			private static readonly Nullable_Type.Vftbl AbiToProjectionVftable = new Nullable_Type.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_Get_Value_0 = ldftn(Do_Abi_get_Value_0)
			};

			// Token: 0x04000878 RID: 2168
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
