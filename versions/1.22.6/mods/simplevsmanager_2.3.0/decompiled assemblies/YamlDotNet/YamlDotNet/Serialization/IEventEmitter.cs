using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000018 RID: 24
	[NullableContext(1)]
	public interface IEventEmitter
	{
		// Token: 0x060000AB RID: 171
		void Emit(AliasEventInfo eventInfo, IEmitter emitter);

		// Token: 0x060000AC RID: 172
		void Emit(ScalarEventInfo eventInfo, IEmitter emitter);

		// Token: 0x060000AD RID: 173
		void Emit(MappingStartEventInfo eventInfo, IEmitter emitter);

		// Token: 0x060000AE RID: 174
		void Emit(MappingEndEventInfo eventInfo, IEmitter emitter);

		// Token: 0x060000AF RID: 175
		void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter);

		// Token: 0x060000B0 RID: 176
		void Emit(SequenceEndEventInfo eventInfo, IEmitter emitter);
	}
}
