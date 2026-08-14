using System;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.Arithmetic
{
	// Token: 0x0200002F RID: 47
	internal static class ReturnCommand
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000123 RID: 291 RVA: 0x000074C2 File Offset: 0x000056C2
		public static bool TakeFromStackBottom { get; } = 0;

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000124 RID: 292 RVA: 0x000074C9 File Offset: 0x000056C9
		public static bool ClearsOperandStack { get; } = 0;

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000125 RID: 293 RVA: 0x000074D0 File Offset: 0x000056D0
		public static LazyType1Command Lazy { get; } = new LazyType1Command("return", new Action<Type1BuildCharContext>(ReturnCommand.Run));

		// Token: 0x06000126 RID: 294 RVA: 0x000074D7 File Offset: 0x000056D7
		public static void Run(Type1BuildCharContext context)
		{
		}

		// Token: 0x04000102 RID: 258
		public const string Name = "return";

		// Token: 0x04000103 RID: 259
		public static readonly byte First = 11;

		// Token: 0x04000104 RID: 260
		public static readonly byte? Second = default(byte?);
	}
}
