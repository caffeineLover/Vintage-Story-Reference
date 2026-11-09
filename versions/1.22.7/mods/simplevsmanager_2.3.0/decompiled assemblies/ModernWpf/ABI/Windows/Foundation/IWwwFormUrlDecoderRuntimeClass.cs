using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000E7 RID: 231
	[Guid("D45A0451-F225-4542-9296-0E1DF5D254DF")]
	internal interface IWwwFormUrlDecoderRuntimeClass : Windows.Foundation.IWwwFormUrlDecoderRuntimeClass, IEnumerable<Windows.Foundation.IWwwFormUrlDecoderEntry>, IEnumerable, IReadOnlyList<Windows.Foundation.IWwwFormUrlDecoderEntry>, IReadOnlyCollection<Windows.Foundation.IWwwFormUrlDecoderEntry>
	{
		// Token: 0x06000862 RID: 2146 RVA: 0x00017F98 File Offset: 0x00016198
		unsafe static IWwwFormUrlDecoderRuntimeClass()
		{
			*(IInspectable.Vftbl*)((void*)IWwwFormUrlDecoderRuntimeClass.AbiToProjectionVftablePtr) = IInspectable.Vftbl.AbiToProjectionVftable;
			*(IntPtr*)((byte*)((void*)IWwwFormUrlDecoderRuntimeClass.AbiToProjectionVftablePtr) + (IntPtr)6 * (IntPtr)sizeof(method)) = ldftn(Do_Abi_GetFirstValueByName_0);
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x00017FF8 File Offset: 0x000161F8
		[UnmanagedCallersOnly(CallConvs = new Type[]
		{
			typeof(CallConvStdcall)
		})]
		private unsafe static int Do_Abi_GetFirstValueByName_0(IntPtr thisPtr, IntPtr name, IntPtr* phstrValue)
		{
			*phstrValue = default(IntPtr);
			try
			{
				string firstValueByName = ComWrappersSupport.FindObject<Windows.Foundation.IWwwFormUrlDecoderRuntimeClass>(thisPtr).GetFirstValueByName(MarshalString.FromAbi(name));
				*phstrValue = MarshalString.FromManaged(firstValueByName);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x0001804C File Offset: 0x0001624C
		int IReadOnlyCollection<Windows.Foundation.IWwwFormUrlDecoderEntry>.Count
		{
			get
			{
				return ((IReadOnlyList<Windows.Foundation.IWwwFormUrlDecoderEntry>)((IWinRTObject)this)).Count;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x0001805E File Offset: 0x0001625E
		Windows.Foundation.IWwwFormUrlDecoderEntry IReadOnlyList<Windows.Foundation.IWwwFormUrlDecoderEntry>.Item
		{
			get
			{
				return ((IReadOnlyList<Windows.Foundation.IWwwFormUrlDecoderEntry>)((IWinRTObject)this))[index];
			}
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00018071 File Offset: 0x00016271
		IEnumerator<Windows.Foundation.IWwwFormUrlDecoderEntry> GetEnumerator()
		{
			return ((IReadOnlyList<Windows.Foundation.IWwwFormUrlDecoderEntry>)((IWinRTObject)this)).GetEnumerator();
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00018083 File Offset: 0x00016283
		IEnumerator GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0400018C RID: 396
		public static IntPtr AbiToProjectionVftablePtr = ComWrappersSupport.AllocateVtableMemory(typeof(IWwwFormUrlDecoderRuntimeClass), sizeof(IInspectable.Vftbl) + sizeof(IntPtr));
	}
}
