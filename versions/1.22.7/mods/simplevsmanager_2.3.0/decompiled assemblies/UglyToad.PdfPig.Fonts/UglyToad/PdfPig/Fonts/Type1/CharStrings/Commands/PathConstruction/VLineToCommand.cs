using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.PathConstruction
{
	// Token: 0x02000024 RID: 36
	internal static class VLineToCommand
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00006AC5 File Offset: 0x00004CC5
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00006ACC File Offset: 0x00004CCC
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00006AD3 File Offset: 0x00004CD3
		public static LazyType1Command Lazy { get; } = new LazyType1Command("vlineto", new Action<Type1BuildCharContext>(VLineToCommand.Run));

		// Token: 0x060000EE RID: 238 RVA: 0x00006ADC File Offset: 0x00004CDC
		public static void Run(Type1BuildCharContext context)
		{
			double num = context.Stack.PopBottom();
			double num2 = context.CurrentPosition.Y + num;
			context.Path[context.Path.Count - 1].LineTo(context.CurrentPosition.X, num2);
			context.CurrentPosition = new PdfPoint(context.CurrentPosition.X, num2);
			context.Stack.Clear();
		}

		// Token: 0x040000BC RID: 188
		public const string Name = "vlineto";

		// Token: 0x040000BD RID: 189
		public static readonly byte First = 7;

		// Token: 0x040000BE RID: 190
		public static readonly byte? Second = default(byte?);
	}
}
