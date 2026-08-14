using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000E5 RID: 229
	[DynamicInterfaceCastableImplementation]
	[Guid("125E7431-F678-4E8E-B670-20A9B06C512D")]
	internal interface IWwwFormUrlDecoderEntry : Windows.Foundation.IWwwFormUrlDecoderEntry
	{
		// Token: 0x0600085C RID: 2140 RVA: 0x00017DAC File Offset: 0x00015FAC
		unsafe static IWwwFormUrlDecoderEntry()
		{
			*(IInspectable.Vftbl*)((void*)IWwwFormUrlDecoderEntry.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IWwwFormUrlDecoderEntry.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Name_0);
			*(IntPtr*)((byte*)((void*)IWwwFormUrlDecoderEntry.AbiToProjectionVftablePtr) + (IntPtr)7 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_get_Value_1);
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x00017E28 File Offset: 0x00016028
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Name_0(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string name = ComWrappersSupport.FindObject<Windows.Foundation.IWwwFormUrlDecoderEntry>(thisPtr).Name;
				*value = MarshalString.FromManaged(name);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x00017E78 File Offset: 0x00016078
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_get_Value_1(IntPtr thisPtr, IntPtr* value)
		{
			*value = default(IntPtr);
			try
			{
				string value2 = ComWrappersSupport.FindObject<Windows.Foundation.IWwwFormUrlDecoderEntry>(thisPtr).Value;
				*value = MarshalString.FromManaged(value2);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x00017EC8 File Offset: 0x000160C8
		string Windows.Foundation.IWwwFormUrlDecoderEntry.Name
		{
			get
			{
				return IWwwFormUrlDecoderEntryMethods.get_Name(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IWwwFormUrlDecoderEntry).TypeHandle));
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x00017EE9 File Offset: 0x000160E9
		string Windows.Foundation.IWwwFormUrlDecoderEntry.Value
		{
			get
			{
				return IWwwFormUrlDecoderEntryMethods.get_Value(((IWinRTObject)this).GetObjectReferenceForType(typeof(Windows.Foundation.IWwwFormUrlDecoderEntry).TypeHandle));
			}
		}

		// Token: 0x0400018B RID: 395
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IWwwFormUrlDecoderEntry), sizeof(IInspectable.Vftbl) + sizeof(IntPtr) * 2);
	}
}
