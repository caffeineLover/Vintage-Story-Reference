using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Core
{
	// Token: 0x020001B6 RID: 438
	[NullableContext(1)]
	public interface IEmitter
	{
		// Token: 0x060009A6 RID: 2470
		void Emit(ParsingEvent @event);
	}
}
