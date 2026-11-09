using System;
using System.Collections;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Serialization.ObjectFactories;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000156 RID: 342
	[NullableContext(1)]
	[Nullable(0)]
	public class StaticDictionaryNodeDeserializer : DictionaryDeserializer, INodeDeserializer
	{
		// Token: 0x060006FF RID: 1791 RVA: 0x00018564 File Offset: 0x00016764
		public StaticDictionaryNodeDeserializer(StaticObjectFactory objectFactory, bool duplicateKeyChecking) : base(duplicateKeyChecking)
		{
			if (objectFactory == null)
			{
				throw new ArgumentNullException("objectFactory");
			}
			this._objectFactory = objectFactory;
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x00018584 File Offset: 0x00016784
		public bool Deserialize(IParser reader, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value)
		{
			if (!this._objectFactory.IsDictionary(expectedType))
			{
				value = null;
				return false;
			}
			IDictionary dictionary = this._objectFactory.Create(expectedType) as IDictionary;
			if (dictionary == null)
			{
				value = null;
				return false;
			}
			Type keyType = this._objectFactory.GetKeyType(expectedType);
			Type valueType = this._objectFactory.GetValueType(expectedType);
			value = dictionary;
			base.Deserialize(keyType, valueType, reader, nestedObjectDeserializer, dictionary);
			return true;
		}

		// Token: 0x040002FE RID: 766
		private readonly StaticObjectFactory _objectFactory;
	}
}
