using System;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.PathConstruction
{
	// Token: 0x0200001C RID: 28
	internal static class ClosePathCommand
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00006385 File Offset: 0x00004585
		public static bool TakeFromStackBottom { get; } = 0;

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x0000638C File Offset: 0x0000458C
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00006393 File Offset: 0x00004593
		public static LazyType1Command Lazy { get; } = new LazyType1Command("closepath", new Action<Type1BuildCharContext>(ClosePathCommand.Run));

		// Token: 0x060000C6 RID: 198 RVA: 0x0000639A File Offset: 0x0000459A
		public static void Run(Type1BuildCharContext context)
		{
			context.Path[context.Path.Count - 1].CloseSubpath();
			context.Stack.Clear();
		}

		// Token: 0x0400008C RID: 140
		public const string Name = "closepath";

		// Token: 0x0400008D RID: 141
		public static readonly byte First = 9;

		// Token: 0x0400008E RID: 142
		public static readonly byte? Second = default(byte?);
	}
}
