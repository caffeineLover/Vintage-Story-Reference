using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.EventEmitters
{
	// Token: 0x0200008E RID: 142
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ChainedEventEmitter : IEventEmitter
	{
		// Token: 0x0600031A RID: 794 RVA: 0x0000B40D File Offset: 0x0000960D
		protected ChainedEventEmitter(IEventEmitter nextEmitter)
		{
			if (nextEmitter == null)
			{
				throw new ArgumentNullException("nextEmitter");
			}
			this.nextEmitter = nextEmitter;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000B42B File Offset: 0x0000962B
		public virtual void Emit(AliasEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000B43A File Offset: 0x0000963A
		public virtual void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000B449 File Offset: 0x00009649
		public virtual void Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000B458 File Offset: 0x00009658
		public virtual void Emit(MappingEndEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000B467 File Offset: 0x00009667
		public virtual void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000B476 File Offset: 0x00009676
		public virtual void Emit(SequenceEndEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x040000D6 RID: 214
		protected readonly IEventEmitter nextEmitter;
	}
}
