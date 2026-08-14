using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000069 RID: 105
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class EmittingObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
	{
		// Token: 0x06000282 RID: 642 RVA: 0x00008609 File Offset: 0x00006809
		public EmittingObjectGraphVisitor(IEventEmitter eventEmitter)
		{
			this.eventEmitter = eventEmitter;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00008618 File Offset: 0x00006818
		bool IObjectGraphVisitor<IEmitter>.Enter([Nullable(2)] IPropertyDescriptor propertyDescriptor, IObjectDescriptor value, IEmitter context, ObjectSerializer serializer)
		{
			return true;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000861B File Offset: 0x0000681B
		bool IObjectGraphVisitor<IEmitter>.EnterMapping(IObjectDescriptor key, IObjectDescriptor value, IEmitter context, ObjectSerializer serializer)
		{
			return true;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000861E File Offset: 0x0000681E
		bool IObjectGraphVisitor<IEmitter>.EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context, ObjectSerializer serializer)
		{
			return true;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00008621 File Offset: 0x00006821
		void IObjectGraphVisitor<IEmitter>.VisitScalar(IObjectDescriptor scalar, IEmitter context, ObjectSerializer serializer)
		{
			this.eventEmitter.Emit(new ScalarEventInfo(scalar), context);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00008635 File Offset: 0x00006835
		void IObjectGraphVisitor<IEmitter>.VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, IEmitter context, ObjectSerializer serializer)
		{
			this.eventEmitter.Emit(new MappingStartEventInfo(mapping), context);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000864A File Offset: 0x0000684A
		void IObjectGraphVisitor<IEmitter>.VisitMappingEnd(IObjectDescriptor mapping, IEmitter context, ObjectSerializer serializer)
		{
			this.eventEmitter.Emit(new MappingEndEventInfo(mapping), context);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000865E File Offset: 0x0000685E
		void IObjectGraphVisitor<IEmitter>.VisitSequenceStart(IObjectDescriptor sequence, Type elementType, IEmitter context, ObjectSerializer serializer)
		{
			this.eventEmitter.Emit(new SequenceStartEventInfo(sequence), context);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00008672 File Offset: 0x00006872
		void IObjectGraphVisitor<IEmitter>.VisitSequenceEnd(IObjectDescriptor sequence, IEmitter context, ObjectSerializer serializer)
		{
			this.eventEmitter.Emit(new SequenceEndEventInfo(sequence), context);
		}

		// Token: 0x0400009F RID: 159
		private readonly IEventEmitter eventEmitter;
	}
}
