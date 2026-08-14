using System;
using System.Collections;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.ObjectFactories;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000083 RID: 131
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class StaticCollectionNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060002F7 RID: 759 RVA: 0x0000B018 File Offset: 0x00009218
		public StaticCollectionNodeDeserializer(StaticObjectFactory factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			this.factory = factory;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000B038 File Offset: 0x00009238
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value, ObjectDeserializer rootDeserializer)
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

		// Token: 0x060002F9 RID: 761 RVA: 0x0000B08C File Offset: 0x0000928C
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

		// Token: 0x040000CB RID: 203
		private readonly StaticObjectFactory factory;
	}
}
