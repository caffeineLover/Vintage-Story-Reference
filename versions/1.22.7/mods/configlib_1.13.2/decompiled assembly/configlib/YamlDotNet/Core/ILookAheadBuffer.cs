using System;

namespace YamlDotNet.Core
{
	// Token: 0x020001B3 RID: 435
	internal interface ILookAheadBuffer
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600096B RID: 2411
		bool EndOfInput { get; }

		// Token: 0x0600096C RID: 2412
		char Peek(int offset);

		// Token: 0x0600096D RID: 2413
		void Skip(int length);
	}
}
