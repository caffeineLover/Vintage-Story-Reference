using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x0200012E RID: 302
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class PreProcessingPhaseObjectGraphVisitorSkeleton : IObjectGraphVisitor<Nothing>
	{
		// Token: 0x06000674 RID: 1652 RVA: 0x00016380 File Offset: 0x00014580
		public PreProcessingPhaseObjectGraphVisitorSkeleton(IEnumerable<IYamlTypeConverter> typeConverters)
		{
			IEnumerable<IYamlTypeConverter> enumerable;
			if (typeConverters == null)
			{
				enumerable = Enumerable.Empty<IYamlTypeConverter>();
			}
			else
			{
				IEnumerable<IYamlTypeConverter> enumerable2 = Enumerable.ToList<IYamlTypeConverter>(typeConverters);
				enumerable = enumerable2;
			}
			this.typeConverters = enumerable;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x000163AC File Offset: 0x000145AC
		bool IObjectGraphVisitor<Nothing>.Enter(IObjectDescriptor value, Nothing context)
		{
			return Enumerable.FirstOrDefault<IYamlTypeConverter>(this.typeConverters, (IYamlTypeConverter t) => t.Accepts(value.Type)) == null && !(value.Value is IYamlConvertible) && !(value.Value is IYamlSerializable) && this.Enter(value);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x00016415 File Offset: 0x00014615
		bool IObjectGraphVisitor<Nothing>.EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, Nothing context)
		{
			return this.EnterMapping(key, value);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001641F File Offset: 0x0001461F
		bool IObjectGraphVisitor<Nothing>.EnterMapping(IObjectDescriptor key, IObjectDescriptor value, Nothing context)
		{
			return this.EnterMapping(key, value);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00016429 File Offset: 0x00014629
		void IObjectGraphVisitor<Nothing>.VisitMappingEnd(IObjectDescriptor mapping, Nothing context)
		{
			this.VisitMappingEnd(mapping);
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00016432 File Offset: 0x00014632
		void IObjectGraphVisitor<Nothing>.VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, Nothing context)
		{
			this.VisitMappingStart(mapping, keyType, valueType);
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001643D File Offset: 0x0001463D
		void IObjectGraphVisitor<Nothing>.VisitScalar(IObjectDescriptor scalar, Nothing context)
		{
			this.VisitScalar(scalar);
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00016446 File Offset: 0x00014646
		void IObjectGraphVisitor<Nothing>.VisitSequenceEnd(IObjectDescriptor sequence, Nothing context)
		{
			this.VisitSequenceEnd(sequence);
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0001644F File Offset: 0x0001464F
		void IObjectGraphVisitor<Nothing>.VisitSequenceStart(IObjectDescriptor sequence, Type elementType, Nothing context)
		{
			this.VisitSequenceStart(sequence, elementType);
		}

		// Token: 0x0600067D RID: 1661
		protected abstract bool Enter(IObjectDescriptor value);

		// Token: 0x0600067E RID: 1662
		protected abstract bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value);

		// Token: 0x0600067F RID: 1663
		protected abstract bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value);

		// Token: 0x06000680 RID: 1664
		protected abstract void VisitMappingEnd(IObjectDescriptor mapping);

		// Token: 0x06000681 RID: 1665
		protected abstract void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType);

		// Token: 0x06000682 RID: 1666
		protected abstract void VisitScalar(IObjectDescriptor scalar);

		// Token: 0x06000683 RID: 1667
		protected abstract void VisitSequenceEnd(IObjectDescriptor sequence);

		// Token: 0x06000684 RID: 1668
		protected abstract void VisitSequenceStart(IObjectDescriptor sequence, Type elementType);

		// Token: 0x040002E6 RID: 742
		protected readonly IEnumerable<IYamlTypeConverter> typeConverters;
	}
}
