using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000127 RID: 295
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ChainedObjectGraphVisitor : IObjectGraphVisitor<IEmitter>
	{
		// Token: 0x06000655 RID: 1621 RVA: 0x00015FF4 File Offset: 0x000141F4
		protected ChainedObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor)
		{
			this.nextVisitor = nextVisitor;
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x00016003 File Offset: 0x00014203
		public virtual bool Enter(IObjectDescriptor value, IEmitter context)
		{
			return this.nextVisitor.Enter(value, context);
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x00016012 File Offset: 0x00014212
		public virtual bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value, IEmitter context)
		{
			return this.nextVisitor.EnterMapping(key, value, context);
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00016022 File Offset: 0x00014222
		public virtual bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, IEmitter context)
		{
			return this.nextVisitor.EnterMapping(key, value, context);
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x00016032 File Offset: 0x00014232
		public virtual void VisitScalar(IObjectDescriptor scalar, IEmitter context)
		{
			this.nextVisitor.VisitScalar(scalar, context);
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00016041 File Offset: 0x00014241
		public virtual void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, IEmitter context)
		{
			this.nextVisitor.VisitMappingStart(mapping, keyType, valueType, context);
		}

		// Token: 0x0600065B RID: 1627 RVA: 0x00016053 File Offset: 0x00014253
		public virtual void VisitMappingEnd(IObjectDescriptor mapping, IEmitter context)
		{
			this.nextVisitor.VisitMappingEnd(mapping, context);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00016062 File Offset: 0x00014262
		public virtual void VisitSequenceStart(IObjectDescriptor sequence, Type elementType, IEmitter context)
		{
			this.nextVisitor.VisitSequenceStart(sequence, elementType, context);
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00016072 File Offset: 0x00014272
		public virtual void VisitSequenceEnd(IObjectDescriptor sequence, IEmitter context)
		{
			this.nextVisitor.VisitSequenceEnd(sequence, context);
		}

		// Token: 0x040002DF RID: 735
		private readonly IObjectGraphVisitor<IEmitter> nextVisitor;
	}
}
