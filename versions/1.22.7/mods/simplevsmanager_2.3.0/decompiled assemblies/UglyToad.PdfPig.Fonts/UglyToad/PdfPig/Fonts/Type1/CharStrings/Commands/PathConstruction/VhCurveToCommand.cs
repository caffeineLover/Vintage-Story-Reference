using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.PathConstruction
{
	// Token: 0x02000023 RID: 35
	internal static class VhCurveToCommand
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x000069BA File Offset: 0x00004BBA
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x000069C1 File Offset: 0x00004BC1
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x000069C8 File Offset: 0x00004BC8
		public static LazyType1Command Lazy { get; } = new LazyType1Command("vhcurveto", new Action<Type1BuildCharContext>(VhCurveToCommand.Run));

		// Token: 0x060000E9 RID: 233 RVA: 0x000069D0 File Offset: 0x00004BD0
		public static void Run(Type1BuildCharContext context)
		{
			double num = context.Stack.PopBottom();
			double num2 = context.Stack.PopBottom();
			double num3 = context.Stack.PopBottom();
			double num4 = context.Stack.PopBottom();
			double x = context.CurrentPosition.X;
			double num5 = context.CurrentPosition.Y + num;
			double num6 = x + num2;
			double num7 = num5 + num3;
			double num8 = num6 + num4;
			double num9 = num7;
			context.Path[context.Path.Count - 1].BezierCurveTo(x, num5, num6, num7, num8, num9);
			context.CurrentPosition = new PdfPoint(num8, num9);
			context.Stack.Clear();
		}

		// Token: 0x040000B6 RID: 182
		public const string Name = "vhcurveto";

		// Token: 0x040000B7 RID: 183
		public static readonly byte First = 30;

		// Token: 0x040000B8 RID: 184
		public static readonly byte? Second = default(byte?);
	}
}
