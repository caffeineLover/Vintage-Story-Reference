using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.StartFinishOutline
{
	// Token: 0x0200001A RID: 26
	internal static class SbwCommand
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x000061DE File Offset: 0x000043DE
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000061E5 File Offset: 0x000043E5
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000061EC File Offset: 0x000043EC
		public static LazyType1Command Lazy { get; } = new LazyType1Command("sbw", new Action<Type1BuildCharContext>(SbwCommand.Run));

		// Token: 0x060000BC RID: 188 RVA: 0x000061F4 File Offset: 0x000043F4
		public static void Run(Type1BuildCharContext context)
		{
			double num = context.Stack.PopBottom();
			double num2 = context.Stack.PopBottom();
			double widthX = context.Stack.PopBottom();
			double widthY = context.Stack.PopBottom();
			context.LeftSideBearingX = num;
			context.LeftSideBearingY = num2;
			context.WidthX = widthX;
			context.WidthY = widthY;
			context.CurrentPosition = new PdfPoint(num, num2);
			context.Stack.Clear();
		}

		// Token: 0x04000080 RID: 128
		public const string Name = "sbw";

		// Token: 0x04000081 RID: 129
		public static readonly byte First = 12;

		// Token: 0x04000082 RID: 130
		public static readonly byte? Second = new byte?((byte)7);
	}
}
