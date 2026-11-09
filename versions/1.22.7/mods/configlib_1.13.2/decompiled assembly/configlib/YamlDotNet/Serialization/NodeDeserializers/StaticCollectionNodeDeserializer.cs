using System;
using System.Collections;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.ObjectFactories;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000153 RID: 339
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StaticCollectionNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060006F9 RID: 1785 RVA: 0x00018446 File Offset: 0x00016646
		public StaticCollectionNodeDeserializer(StaticObjectFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			this.factory = factory;
		}

		// Token: 0x060006FA RID: 1786 RVA: 0x00018464 File Offset: 0x00016664
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

		// Token: 0x060006FB RID: 1787 RVA: 0x000184B8 File Offset: 0x000166B8
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

		// Token: 0x040002FA RID: 762
		private readonly StaticObjectFactory factory;
	}
}
