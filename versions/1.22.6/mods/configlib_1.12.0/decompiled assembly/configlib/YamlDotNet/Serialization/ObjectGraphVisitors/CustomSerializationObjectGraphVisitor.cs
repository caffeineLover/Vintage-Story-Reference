using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.ObjectGraphVisitors
{
	// Token: 0x02000129 RID: 297
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CustomSerializationObjectGraphVisitor : ChainedObjectGraphVisitor
	{
		// Token: 0x06000660 RID: 1632 RVA: 0x000160C8 File Offset: 0x000142C8
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

		// Token: 0x06000661 RID: 1633 RVA: 0x000160FC File Offset: 0x000142FC
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

		// Token: 0x040002E0 RID: 736
		private readonly IEnumerable<IYamlTypeConverter> typeConverters;

		// Token: 0x040002E1 RID: 737
		private readonly ObjectSerializer nestedObjectSerializer;
	}
}
