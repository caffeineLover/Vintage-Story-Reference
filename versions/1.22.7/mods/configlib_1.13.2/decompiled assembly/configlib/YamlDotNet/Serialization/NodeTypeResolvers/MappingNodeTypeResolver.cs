using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
	// Token: 0x02000136 RID: 310
	[NullableContext(1)]
	[Nullable(0)]
	public class MappingNodeTypeResolver : INodeTypeResolver
	{
		// Token: 0x0600067F RID: 1663 RVA: 0x00016924 File Offset: 0x00014B24
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
			this._mappings = mappings;
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x000169EC File Offset: 0x00014BEC
		public bool Resolve([Nullable(2)] NodeEvent nodeEvent, ref Type currentType)
		{
			Type type;
			if (this._mappings.TryGetValue(currentType, ref type))
			{
				currentType = type;
				return true;
			}
			return false;
		}

		// Token: 0x040002CA RID: 714
		private readonly IDictionary<Type, Type> _mappings;
	}
}
