using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.PathConstruction;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.Arithmetic
{
	// Token: 0x0200002B RID: 43
	internal static class CallOtherSubrCommand
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600010F RID: 271 RVA: 0x00006F34 File Offset: 0x00005134
		public static bool TakeFromStackBottom { get; } = 0;

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00006F3B File Offset: 0x0000513B
		public static bool ClearsOperandStack { get; } = 0;

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00006F42 File Offset: 0x00005142
		public static LazyType1Command Lazy { get; } = new LazyType1Command("callothersubr", new Action<Type1BuildCharContext>(CallOtherSubrCommand.Run));

		// Token: 0x06000112 RID: 274 RVA: 0x00006F4C File Offset: 0x0000514C
		public static void Run(Type1BuildCharContext context)
		{
			int num = (int)context.Stack.PopTop();
			int num2 = (int)context.Stack.PopTop();
			List<double> list = new List<double>(num2);
			for (int i = 0; i < num2; i++)
			{
				list.Add(context.Stack.PopTop());
			}
			switch (num)
			{
			case 0:
			{
				context.IsFlexing = false;
				if (context.FlexPoints.Count < 7)
				{
					throw new NotSupportedException("There must be at least 7 flex points defined by an other subroutine.");
				}
				PdfPoint pdfPoint = context.FlexPoints[0].Translate(context.CurrentPosition.X, context.CurrentPosition.Y);
				PdfPoint pdfPoint2 = context.FlexPoints[1].Translate(pdfPoint.X, pdfPoint.Y).Translate(-context.CurrentPosition.X, -context.CurrentPosition.Y);
				context.Stack.Push(pdfPoint2.X);
				context.Stack.Push(pdfPoint2.Y);
				context.Stack.Push(context.FlexPoints[2].X);
				context.Stack.Push(context.FlexPoints[2].Y);
				context.Stack.Push(context.FlexPoints[3].X);
				context.Stack.Push(context.FlexPoints[3].Y);
				RelativeRCurveToCommand.Run(context);
				context.Stack.Push(context.FlexPoints[4].X);
				context.Stack.Push(context.FlexPoints[4].Y);
				context.Stack.Push(context.FlexPoints[5].X);
				context.Stack.Push(context.FlexPoints[5].Y);
				context.Stack.Push(context.FlexPoints[6].X);
				context.Stack.Push(context.FlexPoints[6].Y);
				RelativeRCurveToCommand.Run(context);
				context.ClearFlexPoints();
				return;
			}
			case 1:
				context.PostscriptStack.Clear();
				context.PostscriptStack.Push(context.CurrentPosition.X);
				context.PostscriptStack.Push(context.CurrentPosition.Y);
				context.IsFlexing = true;
				return;
			case 2:
				context.PostscriptStack.Push(context.CurrentPosition.X);
				context.PostscriptStack.Push(context.CurrentPosition.Y);
				return;
			case 3:
				if (list.Count != 1)
				{
					throw new InvalidOperationException("The hint replacement subroutine only takes a single argument.");
				}
				context.PostscriptStack.Clear();
				context.PostscriptStack.Push(list[0]);
				return;
			default:
				context.PostscriptStack.Clear();
				for (int j = 0; j < list.Count; j++)
				{
					context.PostscriptStack.Push(list[j]);
				}
				return;
			}
		}

		// Token: 0x040000E6 RID: 230
		private const int FlexEnd = 0;

		// Token: 0x040000E7 RID: 231
		private const int FlexBegin = 1;

		// Token: 0x040000E8 RID: 232
		private const int FlexMiddle = 2;

		// Token: 0x040000E9 RID: 233
		private const int HintReplacement = 3;

		// Token: 0x040000EA RID: 234
		public const string Name = "callothersubr";

		// Token: 0x040000EB RID: 235
		public static readonly byte First = 12;

		// Token: 0x040000EC RID: 236
		public static readonly byte? Second = new byte?((byte)16);
	}
}
