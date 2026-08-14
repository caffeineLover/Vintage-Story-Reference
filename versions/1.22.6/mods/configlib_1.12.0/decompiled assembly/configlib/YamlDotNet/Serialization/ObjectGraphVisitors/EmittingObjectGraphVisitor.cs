using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x0200012D RID: 301
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EmittingObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
	{
		// Token: 0x0600066B RID: 1643 RVA: 0x0001630B File Offset: 0x0001450B
		public EmittingObjectGraphVisitor(IEventEmitter eventEmitter)
		{
			this.eventEmitter = eventEmitter;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
		bool IObjectGraphVisitor<IEmitter>.Enter(IObjectDescriptor value, IEmitter context)
		{
			return true;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
		bool IObjectGraphVisitor<IEmitter>.EnterMapping(IObjectDescriptor key, IObjectDescriptor value, IEmitter context)
		{
			return true;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
		bool IObjectGraphVisitor<IEmitter>.EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context)
		{
			return true;
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x0001631A File Offset: 0x0001451A
		void IObjectGraphVisitor<IEmitter>.VisitScalar(IObjectDescriptor scalar, IEmitter context)
		{
			this.eventEmitter.Emit(new ScalarEventInfo(scalar), context);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0001632E File Offset: 0x0001452E
		void IObjectGraphVisitor<IEmitter>.VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, IEmitter context)
		{
			this.eventEmitter.Emit(new MappingStartEventInfo(mapping), context);
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00016343 File Offset: 0x00014543
		void IObjectGraphVisitor<IEmitter>.VisitMappingEnd(IObjectDescriptor mapping, IEmitter context)
		{
			this.eventEmitter.Emit(new MappingEndEventInfo(mapping), context);
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x00016357 File Offset: 0x00014557
		void IObjectGraphVisitor<IEmitter>.VisitSequenceStart(IObjectDescriptor sequence, Type elementType, IEmitter context)
		{
			this.eventEmitter.Emit(new SequenceStartEventInfo(sequence), context);
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001636B File Offset: 0x0001456B
		void IObjectGraphVisitor<IEmitter>.VisitSequenceEnd(IObjectDescriptor sequence, IEmitter context)
		{
			this.eventEmitter.Emit(new SequenceEndEventInfo(sequence), context);
		}

		// Token: 0x040002E5 RID: 741
		private readonly IEventEmitter eventEmitter;
	}
}
