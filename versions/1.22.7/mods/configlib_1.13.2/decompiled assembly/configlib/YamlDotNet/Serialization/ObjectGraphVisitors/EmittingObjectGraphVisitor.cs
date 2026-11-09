using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000129 RID: 297
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EmittingObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
	{
		// Token: 0x0600062F RID: 1583 RVA: 0x000159C3 File Offset: 0x00013BC3
		public EmittingObjectGraphVisitor(IEventEmitter eventEmitter)
		{
			this.eventEmitter = eventEmitter;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x0000206C File Offset: 0x0000026C
		bool IObjectGraphVisitor<IEmitter>.Enter(IObjectDescriptor value, IEmitter context)
		{
			return true;
		}

		// Token: 0x06000631 RID: 1585 RVA: 0x0000206C File Offset: 0x0000026C
		bool IObjectGraphVisitor<IEmitter>.EnterMapping(IObjectDescriptor key, IObjectDescriptor value, IEmitter context)
		{
			return true;
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x0000206C File Offset: 0x0000026C
		bool IObjectGraphVisitor<IEmitter>.EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context)
		{
			return true;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x000159D2 File Offset: 0x00013BD2
		void IObjectGraphVisitor<IEmitter>.VisitScalar(IObjectDescriptor scalar, IEmitter context)
		{
			this.eventEmitter.Emit(new ScalarEventInfo(scalar), context);
		}

		// Token: 0x06000634 RID: 1588 RVA: 0x000159E6 File Offset: 0x00013BE6
		void IObjectGraphVisitor<IEmitter>.VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, IEmitter context)
		{
			this.eventEmitter.Emit(new MappingStartEventInfo(mapping), context);
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x000159FB File Offset: 0x00013BFB
		void IObjectGraphVisitor<IEmitter>.VisitMappingEnd(IObjectDescriptor mapping, IEmitter context)
		{
			this.eventEmitter.Emit(new MappingEndEventInfo(mapping), context);
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00015A0F File Offset: 0x00013C0F
		void IObjectGraphVisitor<IEmitter>.VisitSequenceStart(IObjectDescriptor sequence, Type elementType, IEmitter context)
		{
			this.eventEmitter.Emit(new SequenceStartEventInfo(sequence), context);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00015A23 File Offset: 0x00013C23
		void IObjectGraphVisitor<IEmitter>.VisitSequenceEnd(IObjectDescriptor sequence, IEmitter context)
		{
			this.eventEmitter.Emit(new SequenceEndEventInfo(sequence), context);
		}

		// Token: 0x040002B7 RID: 695
		private readonly IEventEmitter eventEmitter;
	}
}
