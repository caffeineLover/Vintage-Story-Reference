using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeTypeResolvers
{
	// Token: 0x0200013A RID: 314
	[NullableContext(1)]
	[Nullable(0)]
	public class MappingNodeTypeResolver : INodeTypeResolver
	{
		// Token: 0x060006BB RID: 1723 RVA: 0x0001726C File Offset: 0x0001546C
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

		// Token: 0x060006BC RID: 1724 RVA: 0x00017334 File Offset: 0x00015534
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

		// Token: 0x040002F8 RID: 760
		private readonly IDictionary<Type, Type> _mappings;
	}
}
