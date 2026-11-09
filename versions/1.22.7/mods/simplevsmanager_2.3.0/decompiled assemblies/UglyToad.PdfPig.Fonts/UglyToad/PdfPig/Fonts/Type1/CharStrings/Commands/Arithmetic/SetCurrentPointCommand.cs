using System;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.Arithmetic
{
	// Token: 0x02000030 RID: 48
	internal static class SetCurrentPointCommand
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00007514 File Offset: 0x00005714
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000129 RID: 297 RVA: 0x0000751B File Offset: 0x0000571B
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00007522 File Offset: 0x00005722
		public static LazyType1Command Lazy { get; } = new LazyType1Command("setcurrentpoint", new Action<Type1BuildCharContext>(SetCurrentPointCommand.Run));

		// Token: 0x0600012B RID: 299 RVA: 0x00007529 File Offset: 0x00005729
		public static void Run(Type1BuildCharContext context)
		{
			context.Stack.PopBottom();
			context.Stack.PopBottom();
			context.Stack.Clear();
		}

		// Token: 0x04000108 RID: 264
		public const string Name = "setcurrentpoint";

		// Token: 0x04000109 RID: 265
		public static readonly byte First = 12;

		// Token: 0x0400010A RID: 266
		public static readonly byte? Second = new byte?((byte)33);
	}
}
