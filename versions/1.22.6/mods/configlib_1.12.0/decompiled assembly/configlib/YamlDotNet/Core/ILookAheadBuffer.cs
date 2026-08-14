using System;

namespace YamlDotNet.Core
{
	// Token: 0x020001B7 RID: 439
	internal interface ILookAheadBuffer
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060009A7 RID: 2471
		bool EndOfInput { get; }

		// Token: 0x060009A8 RID: 2472
		char Peek(int offset);

		// Token: 0x060009A9 RID: 2473
		void Skip(int length);
	}
}
