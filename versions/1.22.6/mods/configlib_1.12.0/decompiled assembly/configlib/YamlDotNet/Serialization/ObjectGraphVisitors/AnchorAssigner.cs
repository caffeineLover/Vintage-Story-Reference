using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000124 RID: 292
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnchorAssigner : PreProcessingPhaseObjectGraphVisitorSkeleton, IAliasProvider
	{
		// Token: 0x06000644 RID: 1604 RVA: 0x00015DE0 File Offset: 0x00013FE0
		public AnchorAssigner(IEnumerable<IYamlTypeConverter> typeConverters) : base(typeConverters)
		{
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00015DF4 File Offset: 0x00013FF4
		protected override bool Enter(IObjectDescriptor value)
		{
			AnchorAssigner.AnchorAssignment anchorAssignment;
			if (value.Value != null && this.assignments.TryGetValue(value.Value, ref anchorAssignment))
			{
				if (anchorAssignment.Anchor.IsEmpty)
				{
					anchorAssignment.Anchor = new AnchorName("o" + this.nextId.ToString(CultureInfo.InvariantCulture));
					this.nextId += 1U;
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
		protected override bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value)
		{
			return true;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
		protected override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value)
		{
			return true;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00007270 File Offset: 0x00005470
		protected override void VisitScalar(IObjectDescriptor scalar)
		{
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x00015E61 File Offset: 0x00014061
		protected override void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType)
		{
			this.VisitObject(mapping);
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00007270 File Offset: 0x00005470
		protected override void VisitMappingEnd(IObjectDescriptor mapping)
		{
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00015E61 File Offset: 0x00014061
		protected override void VisitSequenceStart(IObjectDescriptor sequence, Type elementType)
		{
			this.VisitObject(sequence);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00007270 File Offset: 0x00005470
		protected override void VisitSequenceEnd(IObjectDescriptor sequence)
		{
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x00015E6A File Offset: 0x0001406A
		private void VisitObject(IObjectDescriptor value)
		{
			if (value.Value != null)
			{
				this.assignments.Add(value.Value, new AnchorAssigner.AnchorAssignment());
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00015E8C File Offset: 0x0001408C
		AnchorName IAliasProvider.GetAlias(object target)
		{
			AnchorAssigner.AnchorAssignment anchorAssignment;
			if (target != null && this.assignments.TryGetValue(target, ref anchorAssignment))
			{
				return anchorAssignment.Anchor;
			}
			return AnchorName.Empty;
		}

		// Token: 0x040002D9 RID: 729
		private readonly IDictionary<object, AnchorAssigner.AnchorAssignment> assignments = new Dictionary<object, AnchorAssigner.AnchorAssignment>();

		// Token: 0x040002DA RID: 730
		private uint nextId;

		// Token: 0x02000125 RID: 293
		[NullableContext(0)]
		private class AnchorAssignment
		{
			// Token: 0x040002DB RID: 731
			public AnchorName Anchor;
		}
	}
}
