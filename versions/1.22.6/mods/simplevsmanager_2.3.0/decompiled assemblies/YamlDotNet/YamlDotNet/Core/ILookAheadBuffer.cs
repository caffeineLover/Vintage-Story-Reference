using System;

namespace YamlDotNet.Core
{
	// Token: 0x020000CB RID: 203
	internal interface ILookAheadBuffer
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600050E RID: 1294
		bool EndOfInput { get; }

		// Token: 0x0600050F RID: 1295
		char Peek(int offset);

		// Token: 0x06000510 RID: 1296
		void Skip(int length);
	}
}
