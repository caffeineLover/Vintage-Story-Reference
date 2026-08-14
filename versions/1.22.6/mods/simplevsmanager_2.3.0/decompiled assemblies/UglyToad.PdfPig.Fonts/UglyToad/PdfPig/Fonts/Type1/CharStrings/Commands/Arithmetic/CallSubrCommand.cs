using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.Arithmetic
{
	// Token: 0x0200002C RID: 44
	internal static class CallSubrCommand
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000114 RID: 276 RVA: 0x000072EA File Offset: 0x000054EA
		public static bool TakeFromStackBottom { get; } = 0;

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000115 RID: 277 RVA: 0x000072F1 File Offset: 0x000054F1
		public static bool ClearsOperandStack { get; } = 0;

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000116 RID: 278 RVA: 0x000072F8 File Offset: 0x000054F8
		public static LazyType1Command Lazy { get; } = new LazyType1Command("callsubr", new Action<Type1BuildCharContext>(CallSubrCommand.Run));

		// Token: 0x06000117 RID: 279 RVA: 0x00007300 File Offset: 0x00005500
		public static void Run(Type1BuildCharContext context)
		{
			int num = (int)context.Stack.PopTop();
			foreach (Union<double, LazyType1Command> union in context.Subroutines[num].Commands)
			{
				double value;
				LazyType1Command lazyType1Command;
				if (union.TryGetFirst(ref value))
				{
					context.Stack.Push(value);
				}
				else if (union.TryGetSecond(ref lazyType1Command))
				{
					lazyType1Command.Run(context);
				}
			}
		}

		// Token: 0x040000F0 RID: 240
		public const string Name = "callsubr";

		// Token: 0x040000F1 RID: 241
		public static readonly byte First = 10;

		// Token: 0x040000F2 RID: 242
		public static readonly byte? Second = default(byte?);
	}
}
