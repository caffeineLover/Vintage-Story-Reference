using System;
using System.Runtime.InteropServices;

namespace WinRT.Interop
{
	// Token: 0x020001D0 RID: 464
	[WindowsRuntimeType("Windows.Foundation.UniversalApiContract")]
	[Guid("00000003-0000-0000-c000-000000000046")]
	internal interface IMarshal
	{
		// Token: 0x06000E60 RID: 3680
		unsafe void GetUnmarshalClass(Guid* riid, IntPtr pv, MSHCTX dwDestContext, IntPtr pvDestContext, MSHLFLAGS mshlFlags, Guid* pCid);

		// Token: 0x06000E61 RID: 3681
		unsafe void GetMarshalSizeMax(Guid* riid, IntPtr pv, MSHCTX dwDestContext, IntPtr pvDestContext, MSHLFLAGS mshlflags, uint* pSize);

		// Token: 0x06000E62 RID: 3682
		unsafe void MarshalInterface(IntPtr pStm, Guid* riid, IntPtr pv, MSHCTX dwDestContext, IntPtr pvDestContext, MSHLFLAGS mshlflags);

		// Token: 0x06000E63 RID: 3683
		unsafe void UnmarshalInterface(IntPtr pStm, Guid* riid, IntPtr* ppv);

		// Token: 0x06000E64 RID: 3684
		void ReleaseMarshalData(IntPtr pStm);

		// Token: 0x06000E65 RID: 3685
		void DisconnectObject(uint dwReserved);
	}
}
