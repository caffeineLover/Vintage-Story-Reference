using System;
using System.Runtime.InteropServices;

namespace WinRT
{
	// Token: 0x02000195 RID: 405
	internal static class MarshalExtensions
	{
		// Token: 0x06000CC9 RID: 3273 RVA: 0x00024FF8 File Offset: 0x000231F8
		public static void Dispose(this GCHandle handle)
		{
			if (handle.IsAllocated)
			{
				handle.Free();
			}
		}
	}
}
