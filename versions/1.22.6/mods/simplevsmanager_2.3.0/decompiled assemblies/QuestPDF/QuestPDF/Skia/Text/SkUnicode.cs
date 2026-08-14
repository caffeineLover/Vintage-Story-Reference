using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia.Text
{
	// Token: 0x02000030 RID: 48
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SkUnicode
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00004360 File Offset: 0x00002560
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00004368 File Offset: 0x00002568
		public IntPtr Instance { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00004371 File Offset: 0x00002571
		public static SkUnicode Global { get; } = new SkUnicode();

		// Token: 0x06000116 RID: 278 RVA: 0x00004378 File Offset: 0x00002578
		private SkUnicode()
		{
			this.Instance = SkUnicode.API.unicode_create();
		}

		// Token: 0x02000156 RID: 342
		[NullableContext(0)]
		private static class API
		{
			// Token: 0x06000B40 RID: 2880
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr unicode_create();
		}
	}
}
