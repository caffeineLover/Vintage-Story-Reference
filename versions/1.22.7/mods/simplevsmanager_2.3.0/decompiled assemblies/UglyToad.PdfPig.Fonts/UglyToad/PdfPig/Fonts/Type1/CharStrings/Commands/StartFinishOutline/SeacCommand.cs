using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Encodings;

namespace UglyToad.PdfPig.Fonts.Type1.CharStrings.Commands.StartFinishOutline
{
	// Token: 0x0200001B RID: 27
	internal static class SeacCommand
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000BE RID: 190 RVA: 0x000062A1 File Offset: 0x000044A1
		public static bool TakeFromStackBottom { get; } = 1;

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000BF RID: 191 RVA: 0x000062A8 File Offset: 0x000044A8
		public static bool ClearsOperandStack { get; } = 1;

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x000062AF File Offset: 0x000044AF
		public static LazyType1Command Lazy { get; } = new LazyType1Command("seac", new Action<Type1BuildCharContext>(SeacCommand.Run));

		// Token: 0x060000C1 RID: 193 RVA: 0x000062B8 File Offset: 0x000044B8
		public static void Run(Type1BuildCharContext context)
		{
			context.Stack.PopBottom();
			context.Stack.PopBottom();
			context.Stack.PopBottom();
			int num = (int)context.Stack.PopBottom();
			int num2 = (int)context.Stack.PopBottom();
			string characterName = StandardEncoding.Instance.CodeToNameMap[num];
			string characterName2 = StandardEncoding.Instance.CodeToNameMap[num2];
			IReadOnlyList<PdfSubpath> character = context.GetCharacter(characterName);
			context.GetCharacter(characterName2);
			context.SetPath(character);
			context.Stack.Clear();
		}

		// Token: 0x04000086 RID: 134
		public const string Name = "seac";

		// Token: 0x04000087 RID: 135
		public static readonly byte First = 12;

		// Token: 0x04000088 RID: 136
		public static readonly byte? Second = new byte?((byte)6);
	}
}
