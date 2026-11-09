using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.PathConstruction
{
	// Token: 0x02000025 RID: 37
	internal static class VMoveToCommand
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00006B91 File Offset: 0x00004D91
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00006B98 File Offset: 0x00004D98
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00006B9F File Offset: 0x00004D9F
		public static LazyType1Command Lazy { get; } = new LazyType1Command("vmoveto", new Action<Type1BuildCharContext>(VMoveToCommand.Run));

		// Token: 0x060000F3 RID: 243 RVA: 0x00006BA8 File Offset: 0x00004DA8
		public static void Run(Type1BuildCharContext context)
		{
			double num = context.Stack.PopBottom();
			if (context.IsFlexing)
			{
				context.AddFlexPoint(new PdfPoint(0.0, num));
			}
			else
			{
				double num2 = context.CurrentPosition.Y + num;
				double x = context.CurrentPosition.X;
				context.CurrentPosition = new PdfPoint(x, num2);
				context.Path.Add(new PdfSubpath());
				context.Path[context.Path.Count - 1].MoveTo(x, num2);
			}
			context.Stack.Clear();
		}

		// Token: 0x040000C2 RID: 194
		public const string Name = "vmoveto";

		// Token: 0x040000C3 RID: 195
		public static readonly byte First = 4;

		// Token: 0x040000C4 RID: 196
		public static readonly byte? Second = default(byte?);
	}
}
