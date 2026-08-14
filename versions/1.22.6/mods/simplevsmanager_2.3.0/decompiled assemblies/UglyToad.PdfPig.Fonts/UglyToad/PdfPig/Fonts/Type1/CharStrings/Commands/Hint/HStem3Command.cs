using System;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.Hint
{
	// Token: 0x02000027 RID: 39
	internal static class HStem3Command
	{
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00006CDE File Offset: 0x00004EDE
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00006CE5 File Offset: 0x00004EE5
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00006CEC File Offset: 0x00004EEC
		public static LazyType1Command Lazy { get; } = new LazyType1Command("hstem3", new Action<Type1BuildCharContext>(HStem3Command.Run));

		// Token: 0x060000FD RID: 253 RVA: 0x00006CF4 File Offset: 0x00004EF4
		public static void Run(Type1BuildCharContext context)
		{
			context.Stack.PopBottom();
			context.Stack.PopBottom();
			context.Stack.PopBottom();
			context.Stack.PopBottom();
			context.Stack.PopBottom();
			context.Stack.PopBottom();
			context.Stack.Clear();
		}

		// Token: 0x040000CE RID: 206
		public const string Name = "hstem3";

		// Token: 0x040000CF RID: 207
		public static readonly byte First = 12;

		// Token: 0x040000D0 RID: 208
		public static readonly byte? Second = new byte?((byte)2);
	}
}
