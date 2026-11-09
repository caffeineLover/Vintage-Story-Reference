using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000122 RID: 290
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnchorAssigningObjectGraphVisitor : ChainedObjectGraphVisitor
	{
		// Token: 0x06000614 RID: 1556 RVA: 0x00015570 File Offset: 0x00013770
		public AnchorAssigningObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor, IEventEmitter eventEmitter, IAliasProvider aliasProvider) : base(nextVisitor)
		{
			this.eventEmitter = eventEmitter;
			this.aliasProvider = aliasProvider;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x00015594 File Offset: 0x00013794
		public override bool Enter(IObjectDescriptor value, IEmitter context)
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
			return base.Enter(value, context);
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x000155F8 File Offset: 0x000137F8
		public override void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, IEmitter context)
		{
			AnchorName alias = this.aliasProvider.GetAlias(mapping.NonNullValue());
			this.eventEmitter.Emit(new MappingStartEventInfo(mapping)
			{
				Anchor = alias
			}, context);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00015634 File Offset: 0x00013834
		public override void VisitSequenceStart(IObjectDescriptor sequence, Type elementType, IEmitter context)
		{
			AnchorName alias = this.aliasProvider.GetAlias(sequence.NonNullValue());
			this.eventEmitter.Emit(new SequenceStartEventInfo(sequence)
			{
				Anchor = alias
			}, context);
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x0001566C File Offset: 0x0001386C
		public override void VisitScalar(IObjectDescriptor scalar, IEmitter context)
		{
			ScalarEventInfo scalarEventInfo = new ScalarEventInfo(scalar);
			if (scalar.Value != null)
			{
				scalarEventInfo.Anchor = this.aliasProvider.GetAlias(scalar.Value);
			}
			this.eventEmitter.Emit(scalarEventInfo, context);
		}

		// Token: 0x040002AE RID: 686
		private readonly IEventEmitter eventEmitter;

		// Token: 0x040002AF RID: 687
		private readonly IAliasProvider aliasProvider;

		// Token: 0x040002B0 RID: 688
		private readonly HashSet<AnchorName> emittedAliases = new HashSet<AnchorName>();
	}
}
