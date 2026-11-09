using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x0200012A RID: 298
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class PreProcessingPhaseObjectGraphVisitorSkeleton : IObjectGraphVisitor<Nothing>
	{
		// Token: 0x06000638 RID: 1592 RVA: 0x00015A38 File Offset: 0x00013C38
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

		// Token: 0x06000639 RID: 1593 RVA: 0x00015A64 File Offset: 0x00013C64
		bool IObjectGraphVisitor<Nothing>.Enter(IObjectDescriptor value, Nothing context)
		{
			return Enumerable.FirstOrDefault<IYamlTypeConverter>(this.typeConverters, (IYamlTypeConverter t) => t.Accepts(value.Type)) == null && !(value.Value is IYamlConvertible) && !(value.Value is IYamlSerializable) && this.Enter(value);
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00015ACD File Offset: 0x00013CCD
		bool IObjectGraphVisitor<Nothing>.EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, Nothing context)
		{
			return this.EnterMapping(key, value);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00015AD7 File Offset: 0x00013CD7
		bool IObjectGraphVisitor<Nothing>.EnterMapping(IObjectDescriptor key, IObjectDescriptor value, Nothing context)
		{
			return this.EnterMapping(key, value);
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x00015AE1 File Offset: 0x00013CE1
		void IObjectGraphVisitor<Nothing>.VisitMappingEnd(IObjectDescriptor mapping, Nothing context)
		{
			this.VisitMappingEnd(mapping);
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00015AEA File Offset: 0x00013CEA
		void IObjectGraphVisitor<Nothing>.VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, Nothing context)
		{
			this.VisitMappingStart(mapping, keyType, valueType);
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00015AF5 File Offset: 0x00013CF5
		void IObjectGraphVisitor<Nothing>.VisitScalar(IObjectDescriptor scalar, Nothing context)
		{
			this.VisitScalar(scalar);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x00015AFE File Offset: 0x00013CFE
		void IObjectGraphVisitor<Nothing>.VisitSequenceEnd(IObjectDescriptor sequence, Nothing context)
		{
			this.VisitSequenceEnd(sequence);
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00015B07 File Offset: 0x00013D07
		void IObjectGraphVisitor<Nothing>.VisitSequenceStart(IObjectDescriptor sequence, Type elementType, Nothing context)
		{
			this.VisitSequenceStart(sequence, elementType);
		}

		// Token: 0x06000641 RID: 1601
		protected abstract bool Enter(IObjectDescriptor value);

		// Token: 0x06000642 RID: 1602
		protected abstract bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value);

		// Token: 0x06000643 RID: 1603
		protected abstract bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value);

		// Token: 0x06000644 RID: 1604
		protected abstract void VisitMappingEnd(IObjectDescriptor mapping);

		// Token: 0x06000645 RID: 1605
		protected abstract void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType);

		// Token: 0x06000646 RID: 1606
		protected abstract void VisitScalar(IObjectDescriptor scalar);

		// Token: 0x06000647 RID: 1607
		protected abstract void VisitSequenceEnd(IObjectDescriptor sequence);

		// Token: 0x06000648 RID: 1608
		protected abstract void VisitSequenceStart(IObjectDescriptor sequence, Type elementType);

		// Token: 0x040002B8 RID: 696
		protected readonly IEnumerable<IYamlTypeConverter> typeConverters;
	}
}
