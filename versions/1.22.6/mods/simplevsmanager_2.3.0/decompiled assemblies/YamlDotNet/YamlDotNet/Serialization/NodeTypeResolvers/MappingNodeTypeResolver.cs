using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
	// Token: 0x02000072 RID: 114
	[NullableContext(1)]
	[Nullable(0)]
	public class MappingNodeTypeResolver : INodeTypeResolver
	{
		// Token: 0x060002CB RID: 715 RVA: 0x000095CC File Offset: 0x000077CC
		public MappingNodeTypeResolver(IDictionary<Type, Type> mappings)
		{
			if (mappings == null)
			{
				throw new ArgumentNullException("mappings");
			}
			foreach (KeyValuePair<Type, Type> keyValuePair in mappings)
			{
				if (!keyValuePair.Key.IsAssignableFrom(keyValuePair.Value))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Type '");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(keyValuePair.Value);
					defaultInterpolatedStringHandler.AppendLiteral("' does not implement type '");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(keyValuePair.Key);
					defaultInterpolatedStringHandler.AppendLiteral("'.");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			this.mappings = mappings;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00009694 File Offset: 0x00007894
		public bool Resolve([Nullable(2)] NodeEvent nodeEvent, ref Type currentType)
		{
			Type type;
			if (this.mappings.TryGetValue(currentType, ref type))
			{
				currentType = type;
				return true;
			}
			return false;
		}

		// Token: 0x040000AE RID: 174
		private readonly IDictionary<Type, Type> mappings;
	}
}
