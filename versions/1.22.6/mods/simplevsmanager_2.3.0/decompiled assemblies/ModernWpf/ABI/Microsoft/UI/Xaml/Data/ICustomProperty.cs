using System;
using System.Runtime.InteropServices;
using ABI.System;
using Microsoft.UI.Xaml.Data;
using WinRT;

namespace ABI.Microsoft.UI.Xaml.Data
{
	// Token: 0x0200016C RID: 364
	[Guid("30DA92C0-23E8-42A0-AE7C-734A0E5D2782")]
	internal sealed class ICustomProperty
	{
		// Token: 0x020003A5 RID: 933
		[Guid("30DA92C0-23E8-42A0-AE7C-734A0E5D2782")]
		public struct Vftbl
		{
			// Token: 0x06001A15 RID: 6677 RVA: 0x00048524 File Offset: 0x00046724
			unsafe static Vftbl()
			{
				IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(ICustomProperty.Vftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 8));
				Marshal.StructureToPtr<ICustomProperty.Vftbl>(ICustomProperty.Vftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
				ICustomProperty.Vftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
			}

			// Token: 0x06001A16 RID: 6678 RVA: 0x000485F4 File Offset: 0x000467F4
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_GetValue_2(IntPtr thisPtr, IntPtr target, IntPtr* result)
			{
				*result = default(IntPtr);
				try
				{
					object value = ComWrappersSupport.FindObject<Microsoft.UI.Xaml.Data.ICustomProperty>(thisPtr).GetValue(MarshalInspectable<object>.FromAbi(target));
					*result = MarshalInspectable<object>.FromManaged(value, true);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001A17 RID: 6679 RVA: 0x00048648 File Offset: 0x00046848
			[UnmanagedCallersOnly]
			private static int Do_Abi_SetValue_3(IntPtr thisPtr, IntPtr target, IntPtr value)
			{
				try
				{
					ComWrappersSupport.FindObject<Microsoft.UI.Xaml.Data.ICustomProperty>(thisPtr).SetValue(MarshalInspectable<object>.FromAbi(target), MarshalInspectable<object>.FromAbi(value));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001A18 RID: 6680 RVA: 0x00048690 File Offset: 0x00046890
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_GetIndexedValue_4(IntPtr thisPtr, IntPtr target, IntPtr index, IntPtr* result)
			{
				try
				{
					object indexedValue = ComWrappersSupport.FindObject<Microsoft.UI.Xaml.Data.ICustomProperty>(thisPtr).GetIndexedValue(MarshalInspectable<object>.FromAbi(target), MarshalInspectable<object>.FromAbi(index));
					*result = MarshalInspectable<object>.FromManaged(indexedValue, true);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001A19 RID: 6681 RVA: 0x000486E4 File Offset: 0x000468E4
			[UnmanagedCallersOnly]
			private static int Do_Abi_SetIndexedValue_5(IntPtr thisPtr, IntPtr target, IntPtr value, IntPtr index)
			{
				try
				{
					ComWrappersSupport.FindObject<Microsoft.UI.Xaml.Data.ICustomProperty>(thisPtr).SetIndexedValue(MarshalInspectable<object>.FromAbi(target), MarshalInspectable<object>.FromAbi(value), MarshalInspectable<object>.FromAbi(index));
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001A1A RID: 6682 RVA: 0x00048734 File Offset: 0x00046934
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_CanRead_7(IntPtr thisPtr, byte* value)
			{
				try
				{
					*value = (ComWrappersSupport.FindObject<Microsoft.UI.Xaml.Data.ICustomProperty>(thisPtr).CanRead ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001A1B RID: 6683 RVA: 0x0004877C File Offset: 0x0004697C
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_CanWrite_6(IntPtr thisPtr, byte* value)
			{
				try
				{
					*value = (ComWrappersSupport.FindObject<Microsoft.UI.Xaml.Data.ICustomProperty>(thisPtr).CanWrite ? 1 : 0);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001A1C RID: 6684 RVA: 0x000487C4 File Offset: 0x000469C4
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Name_1(IntPtr thisPtr, IntPtr* value)
			{
				try
				{
					string name = ComWrappersSupport.FindObject<Microsoft.UI.Xaml.Data.ICustomProperty>(thisPtr).Name;
					*value = MarshalString.FromManaged(name);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x06001A1D RID: 6685 RVA: 0x0004880C File Offset: 0x00046A0C
			[UnmanagedCallersOnly]
			private unsafe static int Do_Abi_get_Type_0(IntPtr thisPtr, Type* value)
			{
				try
				{
					Type type = ComWrappersSupport.FindObject<Microsoft.UI.Xaml.Data.ICustomProperty>(thisPtr).Type;
					*value = Type.FromManaged(type);
				}
				catch (Exception ex)
				{
					ExceptionHelpers.SetErrorInfo(ex);
					return ExceptionHelpers.GetHRForException(ex);
				}
				return 0;
			}

			// Token: 0x0400094B RID: 2379
			internal IInspectable.Vftbl IInspectableVftbl;

			// Token: 0x0400094C RID: 2380
			public unsafe void* get_Type_0;

			// Token: 0x0400094D RID: 2381
			public unsafe void* get_Name_1;

			// Token: 0x0400094E RID: 2382
			public unsafe void* GetValue_2;

			// Token: 0x0400094F RID: 2383
			public unsafe void* SetValue_3;

			// Token: 0x04000950 RID: 2384
			public unsafe void* GetIndexedValue_4;

			// Token: 0x04000951 RID: 2385
			public unsafe void* SetIndexedValue_5;

			// Token: 0x04000952 RID: 2386
			public unsafe void* get_CanWrite_6;

			// Token: 0x04000953 RID: 2387
			public unsafe void* get_CanRead_7;

			// Token: 0x04000954 RID: 2388
			private static readonly ICustomProperty.Vftbl AbiToProjectionVftable = new ICustomProperty.Vftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				get_Type_0 = ldftn(Do_Abi_get_Type_0),
				get_Name_1 = ldftn(Do_Abi_get_Name_1),
				GetValue_2 = ldftn(Do_Abi_GetValue_2),
				SetValue_3 = ldftn(Do_Abi_SetValue_3),
				GetIndexedValue_4 = ldftn(Do_Abi_GetIndexedValue_4),
				SetIndexedValue_5 = ldftn(Do_Abi_SetIndexedValue_5),
				get_CanWrite_6 = ldftn(Do_Abi_get_CanWrite_6),
				get_CanRead_7 = ldftn(Do_Abi_get_CanRead_7)
			};

			// Token: 0x04000955 RID: 2389
			public static readonly IntPtr AbiToProjectionVftablePtr;
		}
	}
}
