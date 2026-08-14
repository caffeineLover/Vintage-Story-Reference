using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000064 RID: 100
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ChainedObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
	{
		// Token: 0x0600026E RID: 622 RVA: 0x000082E8 File Offset: 0x000064E8
		protected ChainedObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor)
		{
			this.nextVisitor = nextVisitor;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x000082F7 File Offset: 0x000064F7
		public virtual bool Enter([Nullable(2)] IPropertyDescriptor propertyDescriptor, IObjectDescriptor value, IEmitter context, ObjectSerializer serializer)
		{
			return this.nextVisitor.Enter(propertyDescriptor, value, context, serializer);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00008309 File Offset: 0x00006509
		public virtual bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value, IEmitter context, ObjectSerializer serializer)
		{
			return this.nextVisitor.EnterMapping(key, value, context, serializer);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000831B File Offset: 0x0000651B
		public virtual bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context, ObjectSerializer serializer)
		{
			return this.nextVisitor.EnterMapping(key, value, context, serializer);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000832D File Offset: 0x0000652D
		public virtual void VisitScalar(IObjectDescriptor scalar, IEmitter context, ObjectSerializer serializer)
		{
			this.nextVisitor.VisitScalar(scalar, context, serializer);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000833D File Offset: 0x0000653D
		public virtual void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, IEmitter context, ObjectSerializer serializer)
		{
			this.nextVisitor.VisitMappingStart(mapping, keyType, valueType, context, serializer);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00008351 File Offset: 0x00006551
		public virtual void VisitMappingEnd(IObjectDescriptor mapping, IEmitter context, ObjectSerializer serializer)
		{
			this.nextVisitor.VisitMappingEnd(mapping, context, serializer);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00008361 File Offset: 0x00006561
		public virtual void VisitSequenceStart(IObjectDescriptor sequence, Type elementType, IEmitter context, ObjectSerializer serializer)
		{
			this.nextVisitor.VisitSequenceStart(sequence, elementType, context, serializer);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00008373 File Offset: 0x00006573
		public virtual void VisitSequenceEnd(IObjectDescriptor sequence, IEmitter context, ObjectSerializer serializer)
		{
			this.nextVisitor.VisitSequenceEnd(sequence, context, serializer);
		}

		// Token: 0x0400009A RID: 154
		private readonly IObjectGraphVisitor<IEmitter> nextVisitor;
	}
}
