using System;
using System.Collections;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000143 RID: 323
	public abstract class CollectionDeserializer
	{
		// Token: 0x060006E1 RID: 1761 RVA: 0x00017640 File Offset: 0x00015840
		[NullableContext(1)]
		protected static void DeserializeHelper(Type tItem, IParser parser, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, IList result, bool canUpdate, IObjectFactory objectFactory)
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
					if (!canUpdate)
					{
						Mark mark = (parsingEvent != null) ? parsingEvent.Start : Mark.Empty;
						Mark mark2 = (parsingEvent != null) ? parsingEvent.End : Mark.Empty;
						throw new ForwardAnchorNotSupportedException(ref mark, ref mark2, "Forward alias references are not allowed because this type does not implement IList<>");
					}
					int index = result.Add(objectFactory.CreatePrimitive(tItem));
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
	}
}
