using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000066 RID: 102
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CustomSerializationObjectGraphVisitor : ChainedObjectGraphVisitor
	{
		// Token: 0x06000279 RID: 633 RVA: 0x000083C8 File Offset: 0x000065C8
		public CustomSerializationObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor, IEnumerable<IYamlTypeConverter> typeConverters, ObjectSerializer nestedObjectSerializer) : base(nextVisitor)
		{
			this.typeConverters = new TypeConverterCache(typeConverters);
			this.nestedObjectSerializer = nestedObjectSerializer;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x000083E4 File Offset: 0x000065E4
		public override bool Enter([Nullable(2)] IPropertyDescriptor propertyDescriptor, IObjectDescriptor value, IEmitter context, ObjectSerializer serializer)
		{
			if (((propertyDescriptor != null) ? propertyDescriptor.ConverterType : null) != null)
			{
				IYamlTypeConverter converterByType = this.typeConverters.GetConverterByType(propertyDescriptor.ConverterType);
				converterByType.WriteYaml(context, value.Value, value.Type, serializer);
				return false;
			}
			IYamlTypeConverter yamlTypeConverter;
			if (this.typeConverters.TryGetConverterForType(value.Type, out yamlTypeConverter))
			{
				yamlTypeConverter.WriteYaml(context, value.Value, value.Type, serializer);
				return false;
			}
			IYamlConvertible yamlConvertible = value.Value as IYamlConvertible;
			if (yamlConvertible != null)
			{
				yamlConvertible.Write(context, this.nestedObjectSerializer);
				return false;
			}
			IYamlSerializable yamlSerializable = value.Value as IYamlSerializable;
			if (yamlSerializable != null)
			{
				yamlSerializable.WriteYaml(context);
				return false;
			}
			return base.Enter(propertyDescriptor, value, context, serializer);
		}

		// Token: 0x0400009B RID: 155
		private readonly TypeConverterCache typeConverters;

		// Token: 0x0400009C RID: 156
		private readonly ObjectSerializer nestedObjectSerializer;
	}
}
