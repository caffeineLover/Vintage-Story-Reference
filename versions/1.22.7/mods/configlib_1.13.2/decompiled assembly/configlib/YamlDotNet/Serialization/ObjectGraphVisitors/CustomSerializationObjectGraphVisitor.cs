using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000125 RID: 293
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CustomSerializationObjectGraphVisitor : ChainedObjectGraphVisitor
	{
		// Token: 0x06000624 RID: 1572 RVA: 0x00015780 File Offset: 0x00013980
		public CustomSerializationObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor, IEnumerable<IYamlTypeConverter> typeConverters, ObjectSerializer nestedObjectSerializer) : base(nextVisitor)
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
			this.nestedObjectSerializer = nestedObjectSerializer;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x000157B4 File Offset: 0x000139B4
		public override bool Enter(IObjectDescriptor value, IEmitter context)
		{
			IYamlTypeConverter yamlTypeConverter = Enumerable.FirstOrDefault<IYamlTypeConverter>(this.typeConverters, (IYamlTypeConverter t) => t.Accepts(value.Type));
			if (yamlTypeConverter != null)
			{
				yamlTypeConverter.WriteYaml(context, value.Value, value.Type);
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
			return base.Enter(value, context);
		}

		// Token: 0x040002B2 RID: 690
		private readonly IEnumerable<IYamlTypeConverter> typeConverters;

		// Token: 0x040002B3 RID: 691
		private readonly ObjectSerializer nestedObjectSerializer;
	}
}
