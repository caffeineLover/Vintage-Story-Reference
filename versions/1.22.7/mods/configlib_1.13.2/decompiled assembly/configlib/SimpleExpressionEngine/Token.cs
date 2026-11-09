using System;

namespace SimpleExpressionEngine
{
	// Token: 0x0200005A RID: 90
	public enum Token
	{
		// Token: 0x0400012A RID: 298
		EOF,
		// Token: 0x0400012B RID: 299
		Add,
		// Token: 0x0400012C RID: 300
		Subtract,
		// Token: 0x0400012D RID: 301
		Multiply,
		// Token: 0x0400012E RID: 302
		Divide,
		// Token: 0x0400012F RID: 303
		OpenParenthesis,
		// Token: 0x04000130 RID: 304
		CloseParenthesis,
		// Token: 0x04000131 RID: 305
		Comma,
		// Token: 0x04000132 RID: 306
		Identifier,
		// Token: 0x04000133 RID: 307
		Number
	}
}
