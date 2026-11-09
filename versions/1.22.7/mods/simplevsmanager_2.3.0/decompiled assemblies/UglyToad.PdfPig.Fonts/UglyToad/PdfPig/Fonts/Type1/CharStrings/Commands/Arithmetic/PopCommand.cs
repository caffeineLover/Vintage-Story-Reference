using System;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.Arithmetic
{
	// Token: 0x0200002E RID: 46
	internal static class PopCommand
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00007448 File Offset: 0x00005648
		public static bool TakeFromStackBottom { get; } = 0;

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600011F RID: 287 RVA: 0x0000744F File Offset: 0x0000564F
		public static bool ClearsOperandStack { get; } = 0;

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00007456 File Offset: 0x00005656
		public static LazyType1Command Lazy { get; } = new LazyType1Command("pop", new Action<Type1BuildCharContext>(PopCommand.Run));

		// Token: 0x06000121 RID: 289 RVA: 0x00007460 File Offset: 0x00005660
		public static void Run(Type1BuildCharContext context)
		{
			double value = context.PostscriptStack.PopTop();
			context.Stack.Push(value);
		}

		// Token: 0x040000FC RID: 252
		public const string Name = "pop";

		// Token: 0x040000FD RID: 253
		public static readonly byte First = 12;

		// Token: 0x040000FE RID: 254
		public static readonly byte? Second = new byte?((byte)17);
	}
}
