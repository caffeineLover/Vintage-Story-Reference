using System;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.Arithmetic
{
	// Token: 0x0200002D RID: 45
	internal static class DivCommand
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000119 RID: 281 RVA: 0x000073C3 File Offset: 0x000055C3
		public static bool TakeFromStackBottom { get; } = 0;

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600011A RID: 282 RVA: 0x000073CA File Offset: 0x000055CA
		public static bool ClearsOperandStack { get; } = 0;

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600011B RID: 283 RVA: 0x000073D1 File Offset: 0x000055D1
		public static LazyType1Command Lazy { get; } = new LazyType1Command("div", new Action<Type1BuildCharContext>(DivCommand.Run));

		// Token: 0x0600011C RID: 284 RVA: 0x000073D8 File Offset: 0x000055D8
		public static void Run(Type1BuildCharContext context)
		{
			double num = context.Stack.PopTop();
			double value = context.Stack.PopTop() / num;
			context.Stack.Push(value);
		}

		// Token: 0x040000F6 RID: 246
		public const string Name = "div";

		// Token: 0x040000F7 RID: 247
		public static readonly byte First = 12;

		// Token: 0x040000F8 RID: 248
		public static readonly byte? Second = new byte?((byte)12);
	}
}
