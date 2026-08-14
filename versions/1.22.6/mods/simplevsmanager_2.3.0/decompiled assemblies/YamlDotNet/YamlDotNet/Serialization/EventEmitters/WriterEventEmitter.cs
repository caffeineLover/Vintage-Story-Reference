using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.EventEmitters
{
	// Token: 0x02000091 RID: 145
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WriterEventEmitter : IEventEmitter
	{
		// Token: 0x0600032D RID: 813 RVA: 0x0000BAFC File Offset: 0x00009CFC
		void IEventEmitter.Emit(AliasEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new AnchorAlias(eventInfo.Alias));
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000BB0F File Offset: 0x00009D0F
		void IEventEmitter.Emit(ScalarEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new Scalar(eventInfo.Anchor, eventInfo.Tag, eventInfo.RenderedValue, eventInfo.Style, eventInfo.IsPlainImplicit, eventInfo.IsQuotedImplicit));
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000BB40 File Offset: 0x00009D40
		void IEventEmitter.Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new MappingStart(eventInfo.Anchor, eventInfo.Tag, eventInfo.IsImplicit, eventInfo.Style));
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000BB65 File Offset: 0x00009D65
		void IEventEmitter.Emit(MappingEndEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new MappingEnd());
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000BB72 File Offset: 0x00009D72
		void IEventEmitter.Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new SequenceStart(eventInfo.Anchor, eventInfo.Tag, eventInfo.IsImplicit, eventInfo.Style));
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000BB97 File Offset: 0x00009D97
		void IEventEmitter.Emit(SequenceEndEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new SequenceEnd());
		}
	}
}
