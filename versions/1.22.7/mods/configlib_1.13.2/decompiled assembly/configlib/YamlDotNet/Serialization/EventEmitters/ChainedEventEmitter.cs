using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.EventEmitters
{
	// Token: 0x02000162 RID: 354
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ChainedEventEmitter : IEventEmitter
	{
		// Token: 0x0600071D RID: 1821 RVA: 0x000187C2 File Offset: 0x000169C2
		protected ChainedEventEmitter(IEventEmitter nextEmitter)
		{
			if (nextEmitter == null)
			{
				throw new ArgumentNullException("nextEmitter");
			}
			this.nextEmitter = nextEmitter;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x000187E0 File Offset: 0x000169E0
		public virtual void Emit(AliasEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x000187EF File Offset: 0x000169EF
		public virtual void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x000187FE File Offset: 0x000169FE
		public virtual void Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0001880D File Offset: 0x00016A0D
		public virtual void Emit(MappingEndEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0001881C File Offset: 0x00016A1C
		public virtual void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0001882B File Offset: 0x00016A2B
		public virtual void Emit(SequenceEndEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x0400030B RID: 779
		protected readonly IEventEmitter nextEmitter;
	}
}
