using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000096 RID: 150
	[NullableContext(1)]
	public interface IEventEmitter
	{
		// Token: 0x060003BF RID: 959
		void Emit(AliasEventInfo eventInfo, IEmitter emitter);

		// Token: 0x060003C0 RID: 960
		void Emit(ScalarEventInfo eventInfo, IEmitter emitter);

		// Token: 0x060003C1 RID: 961
		void Emit(MappingStartEventInfo eventInfo, IEmitter emitter);

		// Token: 0x060003C2 RID: 962
		void Emit(MappingEndEventInfo eventInfo, IEmitter emitter);

		// Token: 0x060003C3 RID: 963
		void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter);

		// Token: 0x060003C4 RID: 964
		void Emit(SequenceEndEventInfo eventInfo, IEmitter emitter);
	}
}
