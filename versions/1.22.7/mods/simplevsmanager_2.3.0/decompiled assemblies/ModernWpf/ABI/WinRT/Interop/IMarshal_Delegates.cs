using System;
using WinRT.Interop;

namespace ABI.WinRT.Interop
{
	// Token: 0x02000113 RID: 275
	internal static class IMarshal_Delegates
	{
		// Token: 0x0200032A RID: 810
		// (Invoke) Token: 0x0600179D RID: 6045
		public unsafe delegate int GetUnmarshalClass_0(IntPtr thisPtr, Guid* riid, IntPtr pv, MSHCTX dwDestContext, IntPtr pvDestContext, MSHLFLAGS mshlFlags, Guid* pCid);

		// Token: 0x0200032B RID: 811
		// (Invoke) Token: 0x060017A1 RID: 6049
		public unsafe delegate int GetMarshalSizeMax_1(IntPtr thisPtr, Guid* riid, IntPtr pv, MSHCTX dwDestContext, IntPtr pvDestContext, MSHLFLAGS mshlflags, uint* pSize);

		// Token: 0x0200032C RID: 812
		// (Invoke) Token: 0x060017A5 RID: 6053
		public unsafe delegate int MarshalInterface_2(IntPtr thisPtr, IntPtr pStm, Guid* riid, IntPtr pv, MSHCTX dwDestContext, IntPtr pvDestContext, MSHLFLAGS mshlflags);

		// Token: 0x0200032D RID: 813
		// (Invoke) Token: 0x060017A9 RID: 6057
		public unsafe delegate int UnmarshalInterface_3(IntPtr thisPtr, IntPtr pStm, Guid* riid, IntPtr* ppv);

		// Token: 0x0200032E RID: 814
		// (Invoke) Token: 0x060017AD RID: 6061
		public delegate int ReleaseMarshalData_4(IntPtr thisPtr, IntPtr pStm);

		// Token: 0x0200032F RID: 815
		// (Invoke) Token: 0x060017B1 RID: 6065
		public delegate int DisconnectObject_5(IntPtr thisPtr, uint dwReserved);
	}
}
