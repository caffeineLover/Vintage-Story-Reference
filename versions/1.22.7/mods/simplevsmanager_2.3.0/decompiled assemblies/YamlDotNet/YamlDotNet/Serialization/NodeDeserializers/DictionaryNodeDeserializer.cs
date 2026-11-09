using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Helpers;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x0200007C RID: 124
	[NullableContext(1)]
	[Nullable(0)]
	public class DictionaryNodeDeserializer : DictionaryDeserializer, INodeDeserializer
	{
		// Token: 0x060002E1 RID: 737 RVA: 0x00009DD2 File Offset: 0x00007FD2
		public DictionaryNodeDeserializer(IObjectFactory objectFactory, bool duplicateKeyChecking) : base(duplicateKeyChecking)
		{
			if (objectFactory == null)
			{
				throw new ArgumentNullException("objectFactory");
			}
			this.objectFactory = objectFactory;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00009DF4 File Offset: 0x00007FF4
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value, ObjectDeserializer rootDeserializer)
		{
			Type implementationOfOpenGenericInterface = expectedType.GetImplementationOfOpenGenericInterface(typeof(IDictionary));
			Type type;
			Type type2;
			IDictionary dictionary;
			if (implementationOfOpenGenericInterface != null)
			{
				Type[] genericArguments = implementationOfOpenGenericInterface.GetGenericArguments();
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
			this.Deserialize(type, type2, parser, nestedObjectDeserializer, dictionary, rootDeserializer);
			return true;
		}

		// Token: 0x040000B6 RID: 182
		private readonly IObjectFactory objectFactory;
	}
}
