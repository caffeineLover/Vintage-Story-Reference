using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.EventEmitters
{
	// Token: 0x02000169 RID: 361
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WriterEventEmitter : IEventEmitter
	{
		// Token: 0x06000772 RID: 1906 RVA: 0x000198C9 File Offset: 0x00017AC9
		void IEventEmitter.Emit(AliasEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new AnchorAlias(eventInfo.Alias));
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x000198DC File Offset: 0x00017ADC
		void IEventEmitter.Emit(ScalarEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new Scalar(eventInfo.Anchor, eventInfo.Tag, eventInfo.RenderedValue, eventInfo.Style, eventInfo.IsPlainImplicit, eventInfo.IsQuotedImplicit));
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0001990D File Offset: 0x00017B0D
		void IEventEmitter.Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new MappingStart(eventInfo.Anchor, eventInfo.Tag, eventInfo.IsImplicit, eventInfo.Style));
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00019932 File Offset: 0x00017B32
		void IEventEmitter.Emit(MappingEndEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new MappingEnd());
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0001993F File Offset: 0x00017B3F
		void IEventEmitter.Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new SequenceStart(eventInfo.Anchor, eventInfo.Tag, eventInfo.IsImplicit, eventInfo.Style));
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00019964 File Offset: 0x00017B64
		void IEventEmitter.Emit(SequenceEndEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new SequenceEnd());
		}
	}
}
