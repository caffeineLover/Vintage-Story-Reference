using System;
using System.Collections;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.ObjectFactories;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000084 RID: 132
	[NullableContext(1)]
	[Nullable(0)]
	public class StaticDictionaryNodeDeserializer : DictionaryDeserializer, INodeDeserializer
	{
		// Token: 0x060002FA RID: 762 RVA: 0x0000B124 File Offset: 0x00009324
		public StaticDictionaryNodeDeserializer(StaticObjectFactory objectFactory, bool duplicateKeyChecking) : base(duplicateKeyChecking)
		{
			if (objectFactory == null)
			{
				throw new ArgumentNullException("objectFactory");
			}
			this.objectFactory = objectFactory;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000B144 File Offset: 0x00009344
		public bool Deserialize(IParser reader, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value, ObjectDeserializer rootDeserializer)
		{
			if (!this.objectFactory.IsDictionary(expectedType))
			{
				value = null;
				return false;
			}
			IDictionary dictionary = this.objectFactory.Create(expectedType) as IDictionary;
			if (dictionary == null)
			{
				value = null;
				return false;
			}
			Type keyType = this.objectFactory.GetKeyType(expectedType);
			Type valueType = this.objectFactory.GetValueType(expectedType);
			value = dictionary;
			base.Deserialize(keyType, valueType, reader, nestedObjectDeserializer, dictionary, rootDeserializer);
			return true;
		}

		// Token: 0x040000CC RID: 204
		private readonly StaticObjectFactory objectFactory;
	}
}
