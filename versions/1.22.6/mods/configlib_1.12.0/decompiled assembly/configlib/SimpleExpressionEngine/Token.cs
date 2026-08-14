using System;

namespace SimpleExpressionEngine
{
	// Token: 0x0200005E RID: 94
	public enum Token
	{
		// Token: 0x04000158 RID: 344
		EOF,
		// Token: 0x04000159 RID: 345
		Add,
		// Token: 0x0400015A RID: 346
		Subtract,
		// Token: 0x0400015B RID: 347
		Multiply,
		// Token: 0x0400015C RID: 348
		Divide,
		// Token: 0x0400015D RID: 349
		OpenParenthesis,
		// Token: 0x0400015E RID: 350
		CloseParenthesis,
		// Token: 0x0400015F RID: 351
		Comma,
		// Token: 0x04000160 RID: 352
		Identifier,
		// Token: 0x04000161 RID: 353
		Number
	}
}
