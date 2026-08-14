using System;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.Hint
{
	// Token: 0x02000028 RID: 40
	internal static class HStemCommand
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00006D90 File Offset: 0x00004F90
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00006D97 File Offset: 0x00004F97
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00006D9E File Offset: 0x00004F9E
		public static LazyType1Command Lazy { get; } = new LazyType1Command("hstem", new Action<Type1BuildCharContext>(HStemCommand.Run));

		// Token: 0x06000102 RID: 258 RVA: 0x00006DA5 File Offset: 0x00004FA5
		public static void Run(Type1BuildCharContext context)
		{
			context.Stack.PopBottom();
			context.Stack.PopBottom();
			context.Stack.Clear();
		}

		// Token: 0x040000D4 RID: 212
		public const string Name = "hstem";

		// Token: 0x040000D5 RID: 213
		public static readonly byte First = 1;

		// Token: 0x040000D6 RID: 214
		public static readonly byte? Second = default(byte?);
	}
}
