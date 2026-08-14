using System;
using System.Runtime.CompilerServices;

namespace WinRT
{
	// Token: 0x020001B9 RID: 441
	internal static class ProjectionInitializer
	{
		// Token: 0x06000E19 RID: 3609 RVA: 0x0002AECF File Offset: 0x000290CF
		[ModuleInitializer]
		internal static void InitalizeProjection()
		{
			ComWrappersSupport.RegisterProjectionAssembly(typeof(ProjectionInitializer).Assembly);
		}
	}
}
