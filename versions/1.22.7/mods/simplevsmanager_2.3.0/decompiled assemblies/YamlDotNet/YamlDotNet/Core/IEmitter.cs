using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Core
{
	// Token: 0x020000CA RID: 202
	[NullableContext(1)]
	public interface IEmitter
	{
		// Token: 0x0600050D RID: 1293
		void Emit(ParsingEvent @event);
	}
}
