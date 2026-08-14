using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.StartFinishOutline
{
	// Token: 0x02000019 RID: 25
	internal static class HsbwCommand
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x0000613A File Offset: 0x0000433A
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00006141 File Offset: 0x00004341
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00006148 File Offset: 0x00004348
		public static LazyType1Command Lazy { get; } = new LazyType1Command("hsbw", new Action<Type1BuildCharContext>(HsbwCommand.Run));

		// Token: 0x060000B7 RID: 183 RVA: 0x00006150 File Offset: 0x00004350
		public static void Run(Type1BuildCharContext context)
		{
			double num = context.Stack.PopBottom();
			double widthX = context.Stack.PopBottom();
			context.LeftSideBearingX = num;
			context.WidthX = widthX;
			context.CurrentPosition = new PdfPoint(num, 0.0);
			context.Stack.Clear();
		}

		// Token: 0x0400007A RID: 122
		public const string Name = "hsbw";

		// Token: 0x0400007B RID: 123
		public static readonly byte First = 13;

		// Token: 0x0400007C RID: 124
		public static readonly byte? Second = default(byte?);
	}
}
