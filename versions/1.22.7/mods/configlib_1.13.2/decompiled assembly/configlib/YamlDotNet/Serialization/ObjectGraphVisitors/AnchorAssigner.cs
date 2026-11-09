using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000120 RID: 288
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnchorAssigner : PreProcessingPhaseObjectGraphVisitorSkeleton, IAliasProvider
	{
		// Token: 0x06000608 RID: 1544 RVA: 0x00015498 File Offset: 0x00013698
		public AnchorAssigner(IEnumerable<IYamlTypeConverter> typeConverters) : base(typeConverters)
		{
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x000154AC File Offset: 0x000136AC
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

		// Token: 0x0600060A RID: 1546 RVA: 0x0000206C File Offset: 0x0000026C
		protected override bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value)
		{
			return true;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0000206C File Offset: 0x0000026C
		protected override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value)
		{
			return true;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00002176 File Offset: 0x00000376
		protected override void VisitScalar(IObjectDescriptor scalar)
		{
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00015519 File Offset: 0x00013719
		protected override void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType)
		{
			this.VisitObject(mapping);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00002176 File Offset: 0x00000376
		protected override void VisitMappingEnd(IObjectDescriptor mapping)
		{
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x00015519 File Offset: 0x00013719
		protected override void VisitSequenceStart(IObjectDescriptor sequence, Type elementType)
		{
			this.VisitObject(sequence);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x00002176 File Offset: 0x00000376
		protected override void VisitSequenceEnd(IObjectDescriptor sequence)
		{
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x00015522 File Offset: 0x00013722
		private void VisitObject(IObjectDescriptor value)
		{
			if (value.Value != null)
			{
				this.assignments.Add(value.Value, new AnchorAssigner.AnchorAssignment());
			}
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x00015544 File Offset: 0x00013744
		AnchorName IAliasProvider.GetAlias(object target)
		{
			AnchorAssigner.AnchorAssignment anchorAssignment;
			if (target != null && this.assignments.TryGetValue(target, ref anchorAssignment))
			{
				return anchorAssignment.Anchor;
			}
			return AnchorName.Empty;
		}

		// Token: 0x040002AB RID: 683
		private readonly IDictionary<object, AnchorAssigner.AnchorAssignment> assignments = new Dictionary<object, AnchorAssigner.AnchorAssignment>();

		// Token: 0x040002AC RID: 684
		private uint nextId;

		// Token: 0x02000121 RID: 289
		[NullableContext(0)]
		private class AnchorAssignment
		{
			// Token: 0x040002AD RID: 685
			public AnchorName Anchor;
		}
	}
}
