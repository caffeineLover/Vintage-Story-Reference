using System;
using System.Collections;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.ObjectFactories;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000157 RID: 343
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StaticCollectionNodeDeserializer : INodeDeserializer
	{
		// Token: 0x06000735 RID: 1845 RVA: 0x00018D8E File Offset: 0x00016F8E
		public StaticCollectionNodeDeserializer(StaticObjectFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			this.factory = factory;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x00018DAC File Offset: 0x00016FAC
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value)
		{
			if (!this.factory.IsList(expectedType))
			{
				value = null;
				return false;
			}
			IList list = this.factory.Create(expectedType) as IList;
			value = list;
			StaticCollectionNodeDeserializer.DeserializeHelper(this.factory.GetValueType(expectedType), parser, nestedObjectDeserializer, list, this.factory);
			return true;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00018E00 File Offset: 0x00017000
		internal static void DeserializeHelper(Type tItem, IParser parser, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, IList result, IObjectFactory factory)
		{
			parser.Consume<SequenceStart>();
			SequenceEnd sequenceEnd;
			while (!parser.TryConsume(out sequenceEnd))
			{
				ParsingEvent parsingEvent = parser.Current;
				object obj = nestedObjectDeserializer.Invoke(parser, tItem);
				IValuePromise valuePromise = obj as IValuePromise;
				if (valuePromise != null)
				{
					int index = result.Add(factory.CreatePrimitive(tItem));
					valuePromise.ValueAvailable += delegate(object v)
					{
						result[index] = v;
					};
				}
				else
				{
					result.Add(obj);
				}
			}
		}

		// Token: 0x04000328 RID: 808
		private readonly StaticObjectFactory factory;
	}
}
