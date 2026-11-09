using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000092 RID: 146
	[NullableContext(1)]
	public interface IEventEmitter
	{
		// Token: 0x06000383 RID: 899
		void Emit(AliasEventInfo eventInfo, IEmitter emitter);

		// Token: 0x06000384 RID: 900
		void Emit(ScalarEventInfo eventInfo, IEmitter emitter);

		// Token: 0x06000385 RID: 901
		void Emit(MappingStartEventInfo eventInfo, IEmitter emitter);

		// Token: 0x06000386 RID: 902
		void Emit(MappingEndEventInfo eventInfo, IEmitter emitter);

		// Token: 0x06000387 RID: 903
		void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter);

		// Token: 0x06000388 RID: 904
		void Emit(SequenceEndEventInfo eventInfo, IEmitter emitter);
	}
}
