using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.PathConstruction
{
	// Token: 0x02000022 RID: 34
	internal static class RMoveToCommand
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x000068C2 File Offset: 0x00004AC2
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x000068C9 File Offset: 0x00004AC9
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x000068D0 File Offset: 0x00004AD0
		public static LazyType1Command Lazy { get; } = new LazyType1Command("rmoveto", new Action<Type1BuildCharContext>(RMoveToCommand.Run));

		// Token: 0x060000E4 RID: 228 RVA: 0x000068D8 File Offset: 0x00004AD8
		public static void Run(Type1BuildCharContext context)
		{
			double num = context.Stack.PopBottom();
			double num2 = context.Stack.PopBottom();
			if (context.IsFlexing)
			{
				context.AddFlexPoint(new PdfPoint(num, num2));
			}
			else
			{
				double num3 = context.CurrentPosition.X + num;
				double num4 = context.CurrentPosition.Y + num2;
				context.CurrentPosition = new PdfPoint(num3, num4);
				context.Path.Add(new PdfSubpath());
				context.Path[context.Path.Count - 1].MoveTo(num3, num4);
			}
			context.Stack.Clear();
		}

		// Token: 0x040000B0 RID: 176
		public const string Name = "rmoveto";

		// Token: 0x040000B1 RID: 177
		public static readonly byte First = 21;

		// Token: 0x040000B2 RID: 178
		public static readonly byte? Second = default(byte?);
	}
}
