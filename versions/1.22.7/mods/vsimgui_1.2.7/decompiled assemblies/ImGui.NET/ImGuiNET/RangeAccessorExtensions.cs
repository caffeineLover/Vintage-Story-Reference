using System;
using System.Text;

namespace ImGuiNET
{
	// Token: 0x02000089 RID: 137
	public static class RangeAccessorExtensions
	{
		// Token: 0x0600092D RID: 2349 RVA: 0x000166D5 File Offset: 0x000148D5
		public unsafe static string GetStringASCII(this RangeAccessor<byte> stringAccessor)
		{
			return Encoding.ASCII.GetString((byte*)stringAccessor.Data, stringAccessor.Count);
		}
	}
}
