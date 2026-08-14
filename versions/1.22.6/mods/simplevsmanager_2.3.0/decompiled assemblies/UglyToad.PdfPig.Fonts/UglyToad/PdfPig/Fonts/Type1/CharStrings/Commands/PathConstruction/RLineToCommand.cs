using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.PathConstruction
{
	// Token: 0x02000021 RID: 33
	internal static class RLineToCommand
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000067F0 File Offset: 0x000049F0
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000DD RID: 221 RVA: 0x000067F7 File Offset: 0x000049F7
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000DE RID: 222 RVA: 0x000067FE File Offset: 0x000049FE
		public static LazyType1Command Lazy { get; } = new LazyType1Command("rlineto", new Action<Type1BuildCharContext>(RLineToCommand.Run));

		// Token: 0x060000DF RID: 223 RVA: 0x00006808 File Offset: 0x00004A08
		public static void Run(Type1BuildCharContext context)
		{
			double num = context.Stack.PopBottom();
			double num2 = context.Stack.PopBottom();
			double num3 = context.CurrentPosition.X + num;
			double num4 = context.CurrentPosition.Y + num2;
			context.Path[context.Path.Count - 1].LineTo(num3, num4);
			context.CurrentPosition = new PdfPoint(num3, num4);
			context.Stack.Clear();
		}

		// Token: 0x040000AA RID: 170
		public const string Name = "rlineto";

		// Token: 0x040000AB RID: 171
		public static readonly byte First = 5;

		// Token: 0x040000AC RID: 172
		public static readonly byte? Second = default(byte?);
	}
}
