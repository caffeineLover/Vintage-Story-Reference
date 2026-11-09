using System;
using System.Reflection;
using System.Runtime.InteropServices;
using ABI.System;
using Microsoft.UI.Xaml.Data;
using WinRT;

namespace ABI.Microsoft.UI.Xaml.Data
{
	// Token: 0x0200016F RID: 367
	[Guid("7C925755-3E48-42B4-8677-76372267033F")]
	internal struct ManagedCustomPropertyProviderVftbl
	{
		// Token: 0x06000BD7 RID: 3031 RVA: 0x00020CD0 File Offset: 0x0001EED0
		unsafe static ManagedCustomPropertyProviderVftbl()
		{
			ManagedCustomPropertyProviderVftbl.AbiToProjectionVftable = new ManagedCustomPropertyProviderVftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				GetCustomProperty_0 = ldftn(Do_Abi_GetCustomProperty_0),
				GetIndexedProperty_1 = ldftn(Do_Abi_GetIndexedProperty_1),
				GetStringRepresentation_2 = ldftn(Do_Abi_GetStringRepresentation_2),
				get_Type_3 = ldftn(Do_Abi_get_Type_3)
			};
			IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(ManagedCustomPropertyProviderVftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr) * 4));
			Marshal.StructureToPtr<ManagedCustomPropertyProviderVftbl>(ManagedCustomPropertyProviderVftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
			ManagedCustomPropertyProviderVftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x00020D98 File Offset: 0x0001EF98
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetCustomProperty_0(IntPtr thisPtr, IntPtr name, IntPtr* result)
		{
			Microsoft.UI.Xaml.Data.ICustomProperty customProperty = null;
			try
			{
				string text = MarshalString.FromAbi(name);
				PropertyInfo property = ComWrappersSupport.FindObject<object>(thisPtr).GetType().GetProperty(text, 28);
				if (property != null)
				{
					customProperty = new ManagedCustomProperty(property);
				}
				*result = MarshalInterface<Microsoft.UI.Xaml.Data.ICustomProperty>.FromManaged(customProperty);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00020DF8 File Offset: 0x0001EFF8
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetIndexedProperty_1(IntPtr thisPtr, IntPtr name, Type type, IntPtr* result)
		{
			Microsoft.UI.Xaml.Data.ICustomProperty customProperty = null;
			try
			{
				string text = MarshalString.FromAbi(name);
				PropertyInfo property = ComWrappersSupport.FindObject<object>(thisPtr).GetType().GetProperty(text, 28, null, null, new Type[]
				{
					Type.FromAbi(type)
				}, null);
				if (property != null)
				{
					customProperty = new ManagedCustomProperty(property);
				}
				*result = MarshalInterface<Microsoft.UI.Xaml.Data.ICustomProperty>.FromManaged(customProperty);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x00020E6C File Offset: 0x0001F06C
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_GetStringRepresentation_2(IntPtr thisPtr, IntPtr* result)
		{
			try
			{
				string value = ComWrappersSupport.FindObject<object>(thisPtr).ToString();
				*result = MarshalString.FromManaged(value);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00020EB4 File Offset: 0x0001F0B4
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_get_Type_3(IntPtr thisPtr, Type* value)
		{
			try
			{
				Type type = ComWrappersSupport.FindObject<object>(thisPtr).GetType();
				*value = Type.FromManaged(type);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x040001FC RID: 508
		internal IInspectable.Vftbl IInspectableVftbl;

		// Token: 0x040001FD RID: 509
		private unsafe void* GetCustomProperty_0;

		// Token: 0x040001FE RID: 510
		private unsafe void* GetIndexedProperty_1;

		// Token: 0x040001FF RID: 511
		private unsafe void* GetStringRepresentation_2;

		// Token: 0x04000200 RID: 512
		private unsafe void* get_Type_3;

		// Token: 0x04000201 RID: 513
		private static readonly ManagedCustomPropertyProviderVftbl AbiToProjectionVftable;

		// Token: 0x04000202 RID: 514
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x04000203 RID: 515
		internal static readonly Guid IID = new Guid(2089965397, 15944, 17076, 134, 119, 118, 55, 34, 103, 3, 63);
	}
}
