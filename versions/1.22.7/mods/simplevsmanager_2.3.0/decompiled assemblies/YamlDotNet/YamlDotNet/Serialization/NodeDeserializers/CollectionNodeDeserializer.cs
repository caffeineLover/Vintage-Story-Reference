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
	// Token: 0x0200007A RID: 122
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class CollectionNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060002DB RID: 731 RVA: 0x000099A5 File Offset: 0x00007BA5
		public CollectionNodeDeserializer(IObjectFactory objectFactory, INamingConvention enumNamingConvention, ITypeInspector typeInspector)
		{
			if (objectFactory == null)
			{
				throw new ArgumentNullException("objectFactory");
			}
			this.objectFactory = objectFactory;
			this.enumNamingConvention = enumNamingConvention;
			this.typeInspector = typeInspector;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x000099D4 File Offset: 0x00007BD4
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value, ObjectDeserializer rootDeserializer)
		{
			bool canUpdate = true;
			Type implementationOfOpenGenericInterface = expectedType.GetImplementationOfOpenGenericInterface(typeof(ICollection));
			Type type;
			IList list;
			if (implementationOfOpenGenericInterface != null)
			{
				Type[] genericArguments = implementationOfOpenGenericInterface.GetGenericArguments();
				type = genericArguments[0];
				value = this.objectFactory.Create(expectedType);
				list = (value as IList);
				if (list == null)
				{
					Type implementationOfOpenGenericInterface2 = expectedType.GetImplementationOfOpenGenericInterface(typeof(IList));
					canUpdate = (implementationOfOpenGenericInterface2 != null);
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
			CollectionNodeDeserializer.DeserializeHelper(type, parser, nestedObjectDeserializer, list, canUpdate, this.enumNamingConvention, this.typeInspector, null);
			return true;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00009ACC File Offset: 0x00007CCC
		internal static void DeserializeHelper(Type tItem, IParser parser, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, IList result, bool canUpdate, INamingConvention enumNamingConvention, ITypeInspector typeInspector, [Nullable(2)] Action<int, object> promiseResolvedHandler = null)
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
					if (promiseResolvedHandler != null)
					{
						valuePromise.ValueAvailable += delegate(object v)
						{
							promiseResolvedHandler.Invoke(index, v);
						};
					}
					else
					{
						valuePromise.ValueAvailable += delegate(object v)
						{
							result[index] = TypeConverter.ChangeType(v, tItem, enumNamingConvention, typeInspector);
						};
					}
				}
				else
				{
					result.Add(TypeConverter.ChangeType(obj, tItem, enumNamingConvention, typeInspector));
				}
			}
		}

		// Token: 0x040000B2 RID: 178
		private readonly IObjectFactory objectFactory;

		// Token: 0x040000B3 RID: 179
		private readonly INamingConvention enumNamingConvention;

		// Token: 0x040000B4 RID: 180
		private readonly ITypeInspector typeInspector;
	}
}
