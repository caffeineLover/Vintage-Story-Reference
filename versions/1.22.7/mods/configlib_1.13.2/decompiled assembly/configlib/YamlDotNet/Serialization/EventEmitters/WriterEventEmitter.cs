using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.EventEmitters
{
	// Token: 0x02000165 RID: 357
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WriterEventEmitter : IEventEmitter
	{
		// Token: 0x06000736 RID: 1846 RVA: 0x00018F81 File Offset: 0x00017181
		void IEventEmitter.Emit(AliasEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new AnchorAlias(eventInfo.Alias));
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00018F94 File Offset: 0x00017194
		void IEventEmitter.Emit(ScalarEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new Scalar(eventInfo.Anchor, eventInfo.Tag, eventInfo.RenderedValue, eventInfo.Style, eventInfo.IsPlainImplicit, eventInfo.IsQuotedImplicit));
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00018FC5 File Offset: 0x000171C5
		void IEventEmitter.Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new MappingStart(eventInfo.Anchor, eventInfo.Tag, eventInfo.IsImplicit, eventInfo.Style));
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00018FEA File Offset: 0x000171EA
		void IEventEmitter.Emit(MappingEndEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new MappingEnd());
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00018FF7 File Offset: 0x000171F7
		void IEventEmitter.Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new SequenceStart(eventInfo.Anchor, eventInfo.Tag, eventInfo.IsImplicit, eventInfo.Style));
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0001901C File Offset: 0x0001721C
		void IEventEmitter.Emit(SequenceEndEventInfo eventInfo, IEmitter emitter)
		{
			emitter.Emit(new SequenceEnd());
		}
	}
}
