using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Helpers;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x02000142 RID: 322
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CollectionNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060006AA RID: 1706 RVA: 0x00016DE6 File Offset: 0x00014FE6
		public CollectionNodeDeserializer(IObjectFactory objectFactory)
		{
			if (objectFactory == null)
			{
				throw new ArgumentNullException("objectFactory");
			}
			this.objectFactory = objectFactory;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00016E04 File Offset: 0x00015004
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value)
		{
			bool canUpdate = true;
			Type implementedGenericInterface = ReflectionUtility.GetImplementedGenericInterface(expectedType, typeof(ICollection));
			Type type;
			IList list;
			if (implementedGenericInterface != null)
			{
				type = implementedGenericInterface.GetGenericArguments()[0];
				value = this.objectFactory.Create(expectedType);
				list = (value as IList);
				if (list == null)
				{
					canUpdate = (ReflectionUtility.GetImplementedGenericInterface(expectedType, typeof(IList)) != null);
					list = (IList)Activator.CreateInstance(typeof(GenericCollectionToNonGenericAdapter<>).MakeGenericType(new Type[]
					{
						type
					}), new object[]
					{
						value
					});
				}
			}
			else
			{
				if (!typeof(IList).IsAssignableFrom(expectedType))
				{
					value = null;
					return false;
				}
				type = typeof(object);
				value = this.objectFactory.Create(expectedType);
				list = (IList)value;
			}
			CollectionNodeDeserializer.DeserializeHelper(type, parser, nestedObjectDeserializer, list, canUpdate);
			return true;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00016EE8 File Offset: 0x000150E8
		internal static void DeserializeHelper(Type tItem, IParser parser, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, IList result, bool canUpdate)
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
					int index = result.Add(tItem.IsValueType() ? Activator.CreateInstance(tItem) : null);
					valuePromise.ValueAvailable += delegate(object v)
					{
						result[index] = TypeConverter.ChangeType(v, tItem);
					};
				}
				else
				{
					result.Add(TypeConverter.ChangeType(obj, tItem));
				}
			}
		}

		// Token: 0x040002D5 RID: 725
		private readonly IObjectFactory objectFactory;
	}
}
