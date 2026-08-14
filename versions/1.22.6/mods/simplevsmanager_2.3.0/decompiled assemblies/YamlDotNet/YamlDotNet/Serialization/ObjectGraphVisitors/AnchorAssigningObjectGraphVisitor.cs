using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000063 RID: 99
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnchorAssigningObjectGraphVisitor : ChainedObjectGraphVisitor
	{
		// Token: 0x06000269 RID: 617 RVA: 0x000081A8 File Offset: 0x000063A8
		public AnchorAssigningObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor, IEventEmitter eventEmitter, IAliasProvider aliasProvider) : base(nextVisitor)
		{
			this.eventEmitter = eventEmitter;
			this.aliasProvider = aliasProvider;
		}

		// Token: 0x0600026A RID: 618 RVA: 0x000081CC File Offset: 0x000063CC
		public override bool Enter([Nullable(2)] IPropertyDescriptor propertyDescriptor, IObjectDescriptor value, IEmitter context, ObjectSerializer serializer)
		{
			if (value.Value != null)
			{
				AnchorName alias = this.aliasProvider.GetAlias(value.Value);
				if (!alias.IsEmpty && !this.emittedAliases.Add(alias))
				{
					AliasEventInfo aliasEventInfo = new AliasEventInfo(value, alias);
					this.eventEmitter.Emit(aliasEventInfo, context);
					return aliasEventInfo.NeedsExpansion;
				}
			}
			return base.Enter(propertyDescriptor, value, context, serializer);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00008234 File Offset: 0x00006434
		public override void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, IEmitter context, ObjectSerializer serializer)
		{
			AnchorName alias = this.aliasProvider.GetAlias(mapping.NonNullValue());
			this.eventEmitter.Emit(new MappingStartEventInfo(mapping)
			{
				Anchor = alias
			}, context);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00008270 File Offset: 0x00006470
		public override void VisitSequenceStart(IObjectDescriptor sequence, Type elementType, IEmitter context, ObjectSerializer serializer)
		{
			AnchorName alias = this.aliasProvider.GetAlias(sequence.NonNullValue());
			this.eventEmitter.Emit(new SequenceStartEventInfo(sequence)
			{
				Anchor = alias
			}, context);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x000082A8 File Offset: 0x000064A8
		public override void VisitScalar(IObjectDescriptor scalar, IEmitter context, ObjectSerializer serializer)
		{
			ScalarEventInfo scalarEventInfo = new ScalarEventInfo(scalar);
			if (scalar.Value != null)
			{
				scalarEventInfo.Anchor = this.aliasProvider.GetAlias(scalar.Value);
			}
			this.eventEmitter.Emit(scalarEventInfo, context);
		}

		// Token: 0x04000097 RID: 151
		private readonly IEventEmitter eventEmitter;

		// Token: 0x04000098 RID: 152
		private readonly IAliasProvider aliasProvider;

		// Token: 0x04000099 RID: 153
		private readonly HashSet<AnchorName> emittedAliases = new HashSet<AnchorName>();
	}
}
