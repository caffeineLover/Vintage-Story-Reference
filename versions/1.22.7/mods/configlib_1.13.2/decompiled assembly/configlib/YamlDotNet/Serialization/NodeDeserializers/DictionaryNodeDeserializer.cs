using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Helpers;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000149 RID: 329
	[NullableContext(1)]
	[Nullable(0)]
	public class DictionaryNodeDeserializer : DictionaryDeserializer, INodeDeserializer
	{
		// Token: 0x060006BA RID: 1722 RVA: 0x000172D6 File Offset: 0x000154D6
		public DictionaryNodeDeserializer(IObjectFactory objectFactory, bool duplicateKeyChecking) : base(duplicateKeyChecking)
		{
			if (objectFactory == null)
			{
				throw new ArgumentNullException("objectFactory");
			}
			this.objectFactory = objectFactory;
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x000172F8 File Offset: 0x000154F8
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value)
		{
			Type implementedGenericInterface = ReflectionUtility.GetImplementedGenericInterface(expectedType, typeof(IDictionary));
			Type type;
			Type type2;
			IDictionary dictionary;
			if (implementedGenericInterface != null)
			{
				Type[] genericArguments = implementedGenericInterface.GetGenericArguments();
				type = genericArguments[0];
				type2 = genericArguments[1];
				value = this.objectFactory.Create(expectedType);
				dictionary = (value as IDictionary);
				if (dictionary == null)
				{
					dictionary = (IDictionary)Activator.CreateInstance(typeof(GenericDictionaryToNonGenericAdapter<, >).MakeGenericType(new Type[]
					{
						type,
						type2
					}), new object[]
					{
						value
					});
				}
			}
			else
			{
				if (!typeof(IDictionary).IsAssignableFrom(expectedType))
				{
					value = null;
					return false;
				}
				type = typeof(object);
				type2 = typeof(object);
				value = this.objectFactory.Create(expectedType);
				dictionary = (IDictionary)value;
			}
			this.Deserialize(type, type2, parser, nestedObjectDeserializer, dictionary);
			return true;
		}

		// Token: 0x040002E3 RID: 739
		private readonly IObjectFactory objectFactory;
	}
}
