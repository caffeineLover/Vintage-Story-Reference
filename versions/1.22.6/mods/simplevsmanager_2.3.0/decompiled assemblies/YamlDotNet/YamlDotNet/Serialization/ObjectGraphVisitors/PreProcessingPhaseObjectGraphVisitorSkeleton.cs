using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x0200006A RID: 106
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class PreProcessingPhaseObjectGraphVisitorSkeleton : IObjectGraphVisitor<Nothing>
	{
		// Token: 0x0600028B RID: 651 RVA: 0x00008688 File Offset: 0x00006888
		public PreProcessingPhaseObjectGraphVisitorSkeleton(IEnumerable<IYamlTypeConverter> typeConverters)
		{
			IYamlTypeConverter[] array = ((typeConverters != null) ? Enumerable.ToArray<IYamlTypeConverter>(typeConverters) : null) ?? Array.Empty<IYamlTypeConverter>();
			this.typeConverters = array;
			this.typeConverterCache = new TypeConverterCache(array);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x000086C4 File Offset: 0x000068C4
		bool IObjectGraphVisitor<Nothing>.Enter([Nullable(2)] IPropertyDescriptor propertyDescriptor, IObjectDescriptor value, Nothing context, ObjectSerializer serializer)
		{
			IYamlTypeConverter yamlTypeConverter;
			if (this.typeConverterCache.TryGetConverterForType(value.Type, out yamlTypeConverter))
			{
				return false;
			}
			IYamlConvertible yamlConvertible = value.Value as IYamlConvertible;
			if (yamlConvertible != null)
			{
				return false;
			}
			IYamlSerializable yamlSerializable = value.Value as IYamlSerializable;
			return yamlSerializable == null && this.Enter(value, serializer);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00008713 File Offset: 0x00006913
		bool IObjectGraphVisitor<Nothing>.EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, Nothing context, ObjectSerializer serializer)
		{
			return this.EnterMapping(key, value, serializer);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000871F File Offset: 0x0000691F
		bool IObjectGraphVisitor<Nothing>.EnterMapping(IObjectDescriptor key, IObjectDescriptor value, Nothing context, ObjectSerializer serializer)
		{
			return this.EnterMapping(key, value, serializer);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000872B File Offset: 0x0000692B
		void IObjectGraphVisitor<Nothing>.VisitMappingEnd(IObjectDescriptor mapping, Nothing context, ObjectSerializer serializer)
		{
			this.VisitMappingEnd(mapping, serializer);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00008735 File Offset: 0x00006935
		void IObjectGraphVisitor<Nothing>.VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, Nothing context, ObjectSerializer serializer)
		{
			this.VisitMappingStart(mapping, keyType, valueType, serializer);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00008742 File Offset: 0x00006942
		void IObjectGraphVisitor<Nothing>.VisitScalar(IObjectDescriptor scalar, Nothing context, ObjectSerializer serializer)
		{
			this.VisitScalar(scalar, serializer);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000874C File Offset: 0x0000694C
		void IObjectGraphVisitor<Nothing>.VisitSequenceEnd(IObjectDescriptor sequence, Nothing context, ObjectSerializer serializer)
		{
			this.VisitSequenceEnd(sequence, serializer);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00008756 File Offset: 0x00006956
		void IObjectGraphVisitor<Nothing>.VisitSequenceStart(IObjectDescriptor sequence, Type elementType, Nothing context, ObjectSerializer serializer)
		{
			this.VisitSequenceStart(sequence, elementType, serializer);
		}

		// Token: 0x06000294 RID: 660
		protected abstract bool Enter(IObjectDescriptor value, ObjectSerializer serializer);

		// Token: 0x06000295 RID: 661
		protected abstract bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, ObjectSerializer serializer);

		// Token: 0x06000296 RID: 662
		protected abstract bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value, ObjectSerializer serializer);

		// Token: 0x06000297 RID: 663
		protected abstract void VisitMappingEnd(IObjectDescriptor mapping, ObjectSerializer serializer);

		// Token: 0x06000298 RID: 664
		protected abstract void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, ObjectSerializer serializer);

		// Token: 0x06000299 RID: 665
		protected abstract void VisitScalar(IObjectDescriptor scalar, ObjectSerializer serializer);

		// Token: 0x0600029A RID: 666
		protected abstract void VisitSequenceEnd(IObjectDescriptor sequence, ObjectSerializer serializer);

		// Token: 0x0600029B RID: 667
		protected abstract void VisitSequenceStart(IObjectDescriptor sequence, Type elementType, ObjectSerializer serializer);

		// Token: 0x040000A0 RID: 160
		protected readonly IEnumerable<IYamlTypeConverter> typeConverters;

		// Token: 0x040000A1 RID: 161
		private readonly TypeConverterCache typeConverterCache;
	}
}
