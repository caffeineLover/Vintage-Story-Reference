using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x0200007D RID: 125
	public sealed class EnumerableNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060002E3 RID: 739 RVA: 0x00009ED4 File Offset: 0x000080D4
		[NullableContext(1)]
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value, ObjectDeserializer rootDeserializer)
		{
			Type type;
			if (expectedType == typeof(IEnumerable))
			{
				type = typeof(object);
			}
			else
			{
				Type implementationOfOpenGenericInterface = expectedType.GetImplementationOfOpenGenericInterface(typeof(IEnumerable));
				if (implementationOfOpenGenericInterface != expectedType)
				{
					value = null;
					return false;
				}
				type = implementationOfOpenGenericInterface.GetGenericArguments()[0];
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
