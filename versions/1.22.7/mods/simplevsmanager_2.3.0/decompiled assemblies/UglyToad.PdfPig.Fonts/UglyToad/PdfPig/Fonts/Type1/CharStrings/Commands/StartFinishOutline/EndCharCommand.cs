using System;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.StartFinishOutline
{
	// Token: 0x02000018 RID: 24
	internal static class EndCharCommand
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000AF RID: 175 RVA: 0x000060DD File Offset: 0x000042DD
		public static bool TakeFromStackBottom { get; } = 0;

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x000060E4 File Offset: 0x000042E4
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x000060EB File Offset: 0x000042EB
		public static LazyType1Command Lazy { get; } = new LazyType1Command("endchar", new Action<Type1BuildCharContext>(EndCharCommand.Run));

		// Token: 0x060000B2 RID: 178 RVA: 0x000060F2 File Offset: 0x000042F2
		public static void Run(Type1BuildCharContext context)
		{
			context.Stack.Clear();
		}

		// Token: 0x04000074 RID: 116
		public const string Name = "endchar";

		// Token: 0x04000075 RID: 117
		public static readonly byte First = 14;

		// Token: 0x04000076 RID: 118
		public static readonly byte? Second = default(byte?);
	}
}
