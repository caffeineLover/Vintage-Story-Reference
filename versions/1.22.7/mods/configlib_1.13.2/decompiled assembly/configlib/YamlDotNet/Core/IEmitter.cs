using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Core
{
	// Token: 0x020001B2 RID: 434
	[NullableContext(1)]
	public interface IEmitter
	{
		// Token: 0x0600096A RID: 2410
		void Emit(ParsingEvent @event);
	}
}
