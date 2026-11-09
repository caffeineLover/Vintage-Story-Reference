using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.PathConstruction
{
	// Token: 0x02000020 RID: 32
	internal static class RelativeRCurveToCommand
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x000066C5 File Offset: 0x000048C5
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x000066CC File Offset: 0x000048CC
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x000066D3 File Offset: 0x000048D3
		public static LazyType1Command Lazy { get; } = new LazyType1Command("rrcurveto", new Action<Type1BuildCharContext>(RelativeRCurveToCommand.Run));

		// Token: 0x060000DA RID: 218 RVA: 0x000066DC File Offset: 0x000048DC
		public static void Run(Type1BuildCharContext context)
		{
			double num = context.Stack.PopBottom();
			double num2 = context.Stack.PopBottom();
			double num3 = context.Stack.PopBottom();
			double num4 = context.Stack.PopBottom();
			double num5 = context.Stack.PopBottom();
			double num6 = context.Stack.PopBottom();
			double num7 = context.CurrentPosition.X + num;
			double num8 = context.CurrentPosition.Y + num2;
			double num9 = num7 + num3;
			double num10 = num8 + num4;
			double num11 = num9 + num5;
			double num12 = num10 + num6;
			context.Path[context.Path.Count - 1].BezierCurveTo(num7, num8, num9, num10, num11, num12);
			context.CurrentPosition = new PdfPoint(num11, num12);
			context.Stack.Clear();
		}

		// Token: 0x040000A4 RID: 164
		public const string Name = "rrcurveto";

		// Token: 0x040000A5 RID: 165
		public static readonly byte First = 8;

		// Token: 0x040000A6 RID: 166
		public static readonly byte? Second = default(byte?);
	}
}
