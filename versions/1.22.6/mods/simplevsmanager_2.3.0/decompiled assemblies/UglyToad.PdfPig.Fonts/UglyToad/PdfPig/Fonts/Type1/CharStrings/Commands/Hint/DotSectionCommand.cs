using System;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.Hint
{
	// Token: 0x02000026 RID: 38
	internal static class DotSectionCommand
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00006C81 File Offset: 0x00004E81
		public static bool TakeFromStackBottom { get; } = 0;

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00006C88 File Offset: 0x00004E88
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00006C8F File Offset: 0x00004E8F
		public static LazyType1Command Lazy { get; } = new LazyType1Command("dotsection", new Action<Type1BuildCharContext>(DotSectionCommand.Run));

		// Token: 0x060000F8 RID: 248 RVA: 0x00006C96 File Offset: 0x00004E96
		public static void Run(Type1BuildCharContext context)
		{
			context.Stack.Clear();
		}

		// Token: 0x040000C8 RID: 200
		public const string Name = "dotsection";

		// Token: 0x040000C9 RID: 201
		public static readonly byte First = 12;

		// Token: 0x040000CA RID: 202
		public static readonly byte? Second = new byte?(0);
	}
}
