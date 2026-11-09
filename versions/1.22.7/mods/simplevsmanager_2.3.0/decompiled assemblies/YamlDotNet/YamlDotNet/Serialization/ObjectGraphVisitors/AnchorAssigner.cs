using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000062 RID: 98
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AnchorAssigner : PreProcessingPhaseObjectGraphVisitorSkeleton, IAliasProvider
	{
		// Token: 0x0600025E RID: 606 RVA: 0x000080B9 File Offset: 0x000062B9
		public AnchorAssigner(IEnumerable<IYamlTypeConverter> typeConverters) : base(typeConverters)
		{
		}

		// Token: 0x0600025F RID: 607 RVA: 0x000080D0 File Offset: 0x000062D0
		protected override bool Enter(IObjectDescriptor value, ObjectSerializer serializer)
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

		// Token: 0x06000260 RID: 608 RVA: 0x0000813D File Offset: 0x0000633D
		protected override bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value, ObjectSerializer serializer)
		{
			return true;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00008140 File Offset: 0x00006340
		protected override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, ObjectSerializer serializer)
		{
			return true;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00008143 File Offset: 0x00006343
		protected override void VisitScalar(IObjectDescriptor scalar, ObjectSerializer serializer)
		{
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00008145 File Offset: 0x00006345
		protected override void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, ObjectSerializer serializer)
		{
			this.VisitObject(mapping);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000814E File Offset: 0x0000634E
		protected override void VisitMappingEnd(IObjectDescriptor mapping, ObjectSerializer serializer)
		{
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00008150 File Offset: 0x00006350
		protected override void VisitSequenceStart(IObjectDescriptor sequence, Type elementType, ObjectSerializer serializer)
		{
			this.VisitObject(sequence);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00008159 File Offset: 0x00006359
		protected override void VisitSequenceEnd(IObjectDescriptor sequence, ObjectSerializer serializer)
		{
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000815B File Offset: 0x0000635B
		private void VisitObject(IObjectDescriptor value)
		{
			if (value.Value != null)
			{
				this.assignments.Add(value.Value, new AnchorAssigner.AnchorAssignment());
			}
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000817C File Offset: 0x0000637C
		AnchorName IAliasProvider.GetAlias(object target)
		{
			AnchorAssigner.AnchorAssignment anchorAssignment;
			if (target != null && this.assignments.TryGetValue(target, ref anchorAssignment))
			{
				return anchorAssignment.Anchor;
			}
			return AnchorName.Empty;
		}

		// Token: 0x04000095 RID: 149
		private readonly Dictionary<object, AnchorAssigner.AnchorAssignment> assignments = new Dictionary<object, AnchorAssigner.AnchorAssignment>();

		// Token: 0x04000096 RID: 150
		private uint nextId;

		// Token: 0x02000176 RID: 374
		[NullableContext(0)]
		private class AnchorAssignment
		{
			// Token: 0x040002F1 RID: 753
			public AnchorName Anchor;
		}
	}
}
