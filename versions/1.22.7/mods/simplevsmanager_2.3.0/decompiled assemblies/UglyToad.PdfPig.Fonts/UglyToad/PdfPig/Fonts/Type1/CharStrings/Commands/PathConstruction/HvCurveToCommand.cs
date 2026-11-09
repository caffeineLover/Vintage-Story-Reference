using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.PathConstruction
{
	// Token: 0x0200001F RID: 31
	internal static class HvCurveToCommand
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x000065BA File Offset: 0x000047BA
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x000065C1 File Offset: 0x000047C1
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x000065C8 File Offset: 0x000047C8
		public static LazyType1Command Lazy { get; } = new LazyType1Command("hvcurveto", new Action<Type1BuildCharContext>(HvCurveToCommand.Run));

		// Token: 0x060000D5 RID: 213 RVA: 0x000065D0 File Offset: 0x000047D0
		public static void Run(Type1BuildCharContext context)
		{
			double num = context.Stack.PopBottom();
			double num2 = context.Stack.PopBottom();
			double num3 = context.Stack.PopBottom();
			double num4 = context.Stack.PopBottom();
			double num5 = context.CurrentPosition.X + num;
			double y = context.CurrentPosition.Y;
			double num6 = num5 + num2;
			double num7 = y + num3;
			double num8 = num6;
			double num9 = num7 + num4;
			context.Path[context.Path.Count - 1].BezierCurveTo(num5, y, num6, num7, num8, num9);
			context.CurrentPosition = new PdfPoint(num8, num9);
			context.Stack.Clear();
		}

		// Token: 0x0400009E RID: 158
		public const string Name = "hvcurveto";

		// Token: 0x0400009F RID: 159
		public static readonly byte First = 31;

		// Token: 0x040000A0 RID: 160
		public static readonly byte? Second = default(byte?);
	}
}
