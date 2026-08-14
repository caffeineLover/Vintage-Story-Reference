using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.PathConstruction
{
	// Token: 0x0200001D RID: 29
	internal static class HLineToCommand
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x000063FF File Offset: 0x000045FF
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00006406 File Offset: 0x00004606
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000CA RID: 202 RVA: 0x0000640D File Offset: 0x0000460D
		public static LazyType1Command Lazy { get; } = new LazyType1Command("hlineto", new Action<Type1BuildCharContext>(HLineToCommand.Run));

		// Token: 0x060000CB RID: 203 RVA: 0x00006414 File Offset: 0x00004614
		public static void Run(Type1BuildCharContext context)
		{
			double num = context.Stack.PopBottom();
			double num2 = context.CurrentPosition.X + num;
			context.Path[context.Path.Count - 1].LineTo(num2, context.CurrentPosition.Y);
			context.CurrentPosition = new PdfPoint(num2, context.CurrentPosition.Y);
			context.Stack.Clear();
		}

		// Token: 0x04000092 RID: 146
		public const string Name = "hlineto";

		// Token: 0x04000093 RID: 147
		public static readonly byte First = 6;

		// Token: 0x04000094 RID: 148
		public static readonly byte? Second = default(byte?);
	}
}
