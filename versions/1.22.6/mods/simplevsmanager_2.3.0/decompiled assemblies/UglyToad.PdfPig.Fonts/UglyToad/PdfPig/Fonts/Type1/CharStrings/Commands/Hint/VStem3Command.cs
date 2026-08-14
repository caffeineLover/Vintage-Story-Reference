using System;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.Hint
{
	// Token: 0x02000029 RID: 41
	internal class VStem3Command
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00006E04 File Offset: 0x00005004
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00006E0B File Offset: 0x0000500B
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00006E12 File Offset: 0x00005012
		public static LazyType1Command Lazy { get; } = new LazyType1Command("vstem3", new Action<Type1BuildCharContext>(VStem3Command.Run));

		// Token: 0x06000107 RID: 263 RVA: 0x00006E1C File Offset: 0x0000501C
		public static void Run(Type1BuildCharContext context)
		{
			context.Stack.PopBottom();
			context.Stack.PopBottom();
			context.Stack.PopBottom();
			context.Stack.PopBottom();
			context.Stack.PopBottom();
			context.Stack.PopBottom();
			context.Stack.Clear();
		}

		// Token: 0x040000DA RID: 218
		public const string Name = "vstem3";

		// Token: 0x040000DB RID: 219
		public static readonly byte First = 12;

		// Token: 0x040000DC RID: 220
		public static readonly byte? Second = new byte?((byte)1);
	}
}
