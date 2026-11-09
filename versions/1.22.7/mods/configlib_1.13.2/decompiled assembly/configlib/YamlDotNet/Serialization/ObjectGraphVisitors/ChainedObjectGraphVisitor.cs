using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000123 RID: 291
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ChainedObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
	{
		// Token: 0x06000619 RID: 1561 RVA: 0x000156AC File Offset: 0x000138AC
		protected ChainedObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor)
		{
			this.nextVisitor = nextVisitor;
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x000156BB File Offset: 0x000138BB
		public virtual bool Enter(IObjectDescriptor value, IEmitter context)
		{
			return this.nextVisitor.Enter(value, context);
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x000156CA File Offset: 0x000138CA
		public virtual bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value, IEmitter context)
		{
			return this.nextVisitor.EnterMapping(key, value, context);
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x000156DA File Offset: 0x000138DA
		public virtual bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context)
		{
			return this.nextVisitor.EnterMapping(key, value, context);
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x000156EA File Offset: 0x000138EA
		public virtual void VisitScalar(IObjectDescriptor scalar, IEmitter context)
		{
			this.nextVisitor.VisitScalar(scalar, context);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x000156F9 File Offset: 0x000138F9
		public virtual void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, IEmitter context)
		{
			this.nextVisitor.VisitMappingStart(mapping, keyType, valueType, context);
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0001570B File Offset: 0x0001390B
		public virtual void VisitMappingEnd(IObjectDescriptor mapping, IEmitter context)
		{
			this.nextVisitor.VisitMappingEnd(mapping, context);
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0001571A File Offset: 0x0001391A
		public virtual void VisitSequenceStart(IObjectDescriptor sequence, Type elementType, IEmitter context)
		{
			this.nextVisitor.VisitSequenceStart(sequence, elementType, context);
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0001572A File Offset: 0x0001392A
		public virtual void VisitSequenceEnd(IObjectDescriptor sequence, IEmitter context)
		{
			this.nextVisitor.VisitSequenceEnd(sequence, context);
		}

		// Token: 0x040002B1 RID: 689
		private readonly IObjectGraphVisitor<IEmitter> nextVisitor;
	}
}
