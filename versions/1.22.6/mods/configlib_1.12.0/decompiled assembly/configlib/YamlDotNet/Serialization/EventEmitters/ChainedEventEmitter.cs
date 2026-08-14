using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.EventEmitters
{
	// Token: 0x02000166 RID: 358
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ChainedEventEmitter : IEventEmitter
	{
		// Token: 0x06000759 RID: 1881 RVA: 0x0001910A File Offset: 0x0001730A
		protected ChainedEventEmitter(IEventEmitter nextEmitter)
		{
			if (nextEmitter == null)
			{
				throw new ArgumentNullException("nextEmitter");
			}
			this.nextEmitter = nextEmitter;
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00019128 File Offset: 0x00017328
		public virtual void Emit(AliasEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00019137 File Offset: 0x00017337
		public virtual void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x00019146 File Offset: 0x00017346
		public virtual void Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x00019155 File Offset: 0x00017355
		public virtual void Emit(MappingEndEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00019164 File Offset: 0x00017364
		public virtual void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00019173 File Offset: 0x00017373
		public virtual void Emit(SequenceEndEventInfo eventInfo, IEmitter emitter)
		{
			this.nextEmitter.Emit(eventInfo, emitter);
		}

		// Token: 0x04000339 RID: 825
		protected readonly IEventEmitter nextEmitter;
	}
}
