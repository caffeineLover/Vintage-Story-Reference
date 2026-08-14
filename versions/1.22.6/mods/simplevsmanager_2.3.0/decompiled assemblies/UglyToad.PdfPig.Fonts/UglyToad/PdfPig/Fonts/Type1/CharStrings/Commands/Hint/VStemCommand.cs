using System;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.Hint
{
	// Token: 0x0200002A RID: 42
	internal static class VStemCommand
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00006EC0 File Offset: 0x000050C0
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00006EC7 File Offset: 0x000050C7
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00006ECE File Offset: 0x000050CE
		public static LazyType1Command Lazy { get; } = new LazyType1Command("vstem", new Action<Type1BuildCharContext>(VStemCommand.Run));

		// Token: 0x0600010D RID: 269 RVA: 0x00006ED5 File Offset: 0x000050D5
		public static void Run(Type1BuildCharContext context)
		{
			context.Stack.PopBottom();
			context.Stack.PopBottom();
			context.Stack.Clear();
		}

		// Token: 0x040000E0 RID: 224
		public const string Name = "vstem";

		// Token: 0x040000E1 RID: 225
		public static readonly byte First = 3;

		// Token: 0x040000E2 RID: 226
		public static readonly byte? Second = default(byte?);
	}
}
