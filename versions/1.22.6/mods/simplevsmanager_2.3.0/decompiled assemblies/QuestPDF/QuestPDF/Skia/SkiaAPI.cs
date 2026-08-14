using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.Exceptions;

namespace QuestPDF.Skia
{
	// Token: 0x0200000D RID: 13
	[NullableContext(1)]
	[Nullable(0)]
	internal static class SkiaAPI
	{
		// Token: 0x0600005A RID: 90 RVA: 0x00002A8C File Offset: 0x00000C8C
		public static void EnsureNotNull(IntPtr instance)
		{
			if (instance == IntPtr.Zero)
			{
				throw new InitializationException("QuestPDF cannot instantiate native object.");
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002AA1 File Offset: 0x00000CA1
		public static void WarnThatFinalizerIsReached<[Nullable(0)] T>(this T disposableObject) where T : IDisposable
		{
		}

		// Token: 0x04000029 RID: 41
		public const string LibraryName = "QuestPdfSkia";
	}
}
