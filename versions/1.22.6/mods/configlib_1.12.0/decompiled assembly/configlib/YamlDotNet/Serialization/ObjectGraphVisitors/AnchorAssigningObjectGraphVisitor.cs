using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000126 RID: 294
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnchorAssigningObjectGraphVisitor : ChainedObjectGraphVisitor
	{
		// Token: 0x06000650 RID: 1616 RVA: 0x00015EB8 File Offset: 0x000140B8
		public AnchorAssigningObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor, IEventEmitter eventEmitter, IAliasProvider aliasProvider) : base(nextVisitor)
		{
			this.eventEmitter = eventEmitter;
			this.aliasProvider = aliasProvider;
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00015EDC File Offset: 0x000140DC
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

		// Token: 0x06000652 RID: 1618 RVA: 0x00015F40 File Offset: 0x00014140
		public override void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, IEmitter context)
		{
			AnchorName alias = this.aliasProvider.GetAlias(mapping.NonNullValue());
			this.eventEmitter.Emit(new MappingStartEventInfo(mapping)
			{
				Anchor = alias
			}, context);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00015F7C File Offset: 0x0001417C
		public override void VisitSequenceStart(IObjectDescriptor sequence, Type elementType, IEmitter context)
		{
			AnchorName alias = this.aliasProvider.GetAlias(sequence.NonNullValue());
			this.eventEmitter.Emit(new SequenceStartEventInfo(sequence)
			{
				Anchor = alias
			}, context);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00015FB4 File Offset: 0x000141B4
		public override void VisitScalar(IObjectDescriptor scalar, IEmitter context)
		{
			ScalarEventInfo scalarEventInfo = new ScalarEventInfo(scalar);
			if (scalar.Value != null)
			{
				scalarEventInfo.Anchor = this.aliasProvider.GetAlias(scalar.Value);
			}
			this.eventEmitter.Emit(scalarEventInfo, context);
		}

		// Token: 0x040002DC RID: 732
		private readonly IEventEmitter eventEmitter;

		// Token: 0x040002DD RID: 733
		private readonly IAliasProvider aliasProvider;

		// Token: 0x040002DE RID: 734
		private readonly HashSet<AnchorName> emittedAliases = new HashSet<AnchorName>();
	}
}
