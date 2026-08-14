using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.PathConstruction
{
	// Token: 0x0200001E RID: 30
	internal static class HMoveToCommand
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000064C9 File Offset: 0x000046C9
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000CE RID: 206 RVA: 0x000064D0 File Offset: 0x000046D0
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000CF RID: 207 RVA: 0x000064D7 File Offset: 0x000046D7
		public static LazyType1Command Lazy { get; } = new LazyType1Command("hmoveto", new Action<Type1BuildCharContext>(HMoveToCommand.Run));

		// Token: 0x060000D0 RID: 208 RVA: 0x000064E0 File Offset: 0x000046E0
		public static void Run(Type1BuildCharContext context)
		{
			double num = context.Stack.PopBottom();
			if (context.IsFlexing)
			{
				context.AddFlexPoint(new PdfPoint(num, 0.0));
			}
			else
			{
				double num2 = context.CurrentPosition.X + num;
				double y = context.CurrentPosition.Y;
				context.CurrentPosition = new PdfPoint(num2, y);
				context.Path.Add(new PdfSubpath());
				context.Path[context.Path.Count - 1].MoveTo(num2, y);
			}
			context.Stack.Clear();
		}

		// Token: 0x04000098 RID: 152
		public const string Name = "hmoveto";

		// Token: 0x04000099 RID: 153
		public static readonly byte First = 22;

		// Token: 0x0400009A RID: 154
		public static readonly byte? Second = default(byte?);
	}
}
