using System;
using System.Runtime.InteropServices;
using WinRT;

namespace ABI.Windows.Foundation
{
	// Token: 0x020000B2 RID: 178
	[Guid("96369F54-8EB6-48F0-ABCE-C1B211E627C3")]
	internal struct ManagedIStringableVftbl
	{
		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x00011800 File Offset: 0x0000FA00
		// (set) Token: 0x06000704 RID: 1796 RVA: 0x00011808 File Offset: 0x0000FA08
		private method ToString_0
		{
			get
			{
				return this._ToString_0;
			}
			set
			{
				this._ToString_0 = value;
			}
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00011814 File Offset: 0x0000FA14
		unsafe static ManagedIStringableVftbl()
		{
			ManagedIStringableVftbl.AbiToProjectionVftable = new ManagedIStringableVftbl
			{
				IInspectableVftbl = IInspectable.Vftbl.AbiToProjectionVftable,
				_ToString_0 = ldftn(Do_Abi_ToString_0)
			};
			IntPtr* ptr = (IntPtr*)((void*)ComWrappersSupport.AllocateVtableMemory(typeof(ManagedIStringableVftbl), Marshal.SizeOf<IInspectable.Vftbl>() + sizeof(IntPtr)));
			Marshal.StructureToPtr<ManagedIStringableVftbl>(ManagedIStringableVftbl.AbiToProjectionVftable, (IntPtr)((void*)ptr), false);
			ManagedIStringableVftbl.AbiToProjectionVftablePtr = (IntPtr)((void*)ptr);
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x000118C0 File Offset: 0x0000FAC0
		[UnmanagedCallersOnly]
		private unsafe static int Do_Abi_ToString_0(IntPtr thisPtr, IntPtr* value)
		{
			try
			{
				string value2 = ComWrappersSupport.FindObject<object>(thisPtr).ToString();
				*value = MarshalString.FromManaged(value2);
			}
			catch (Exception ex)
			{
				ExceptionHelpers.SetErrorInfo(ex);
				return ExceptionHelpers.GetHRForException(ex);
			}
			return 0;
		}

		// Token: 0x04000141 RID: 321
		internal IInspectable.Vftbl IInspectableVftbl;

		// Token: 0x04000142 RID: 322
		private unsafe void* _ToString_0;

		// Token: 0x04000143 RID: 323
		private static readonly ManagedIStringableVftbl AbiToProjectionVftable;

		// Token: 0x04000144 RID: 324
		public static readonly IntPtr AbiToProjectionVftablePtr;

		// Token: 0x04000145 RID: 325
		internal static readonly Guid IID = new Guid(2520162132U, 36534, 18672, 171, 206, 193, 178, 17, 230, 39, 195);
	}
}
