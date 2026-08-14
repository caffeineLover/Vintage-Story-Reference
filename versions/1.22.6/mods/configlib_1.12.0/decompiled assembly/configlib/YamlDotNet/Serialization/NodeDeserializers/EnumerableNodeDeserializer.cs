using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x0200014E RID: 334
	public sealed class EnumerableNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060006F8 RID: 1784 RVA: 0x00017D1C File Offset: 0x00015F1C
		[NullableContext(1)]
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value)
		{
			Type type;
			if (expectedType == typeof(IEnumerable))
			{
				type = typeof(object);
			}
			else
			{
				Type implementedGenericInterface = ReflectionUtility.GetImplementedGenericInterface(expectedType, typeof(IEnumerable));
				if (implementedGenericInterface != expectedType)
				{
					value = null;
					return false;
				}
				type = implementedGenericInterface.GetGenericArguments()[0];
			}
			Type type2 = typeof(List).MakeGenericType(new Type[]
			{
				type
			});
			value = nestedObjectDeserializer.Invoke(parser, type2);
			return true;
		}
	}
}
