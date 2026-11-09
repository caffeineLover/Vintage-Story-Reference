using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.ValueDeserializers
{
	// Token: 0x020000F7 RID: 247
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NodeValueDeserializer : IValueDeserializer
	{
		// Token: 0x06000580 RID: 1408 RVA: 0x00014054 File Offset: 0x00012254
		public NodeValueDeserializer(IList<INodeDeserializer> deserializers, IList<INodeTypeResolver> typeResolvers, ITypeConverter typeConverter)
		{
			if (deserializers == null)
			{
				throw new ArgumentNullException("deserializers");
			}
			this.deserializers = deserializers;
			if (typeResolvers == null)
			{
				throw new ArgumentNullException("typeResolvers");
			}
			this.typeResolvers = typeResolvers;
			if (typeConverter == null)
			{
				throw new ArgumentNullException("typeConverter");
			}
			this.typeConverter = typeConverter;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x000140AC File Offset: 0x000122AC
		[return: Nullable(2)]
		public object DeserializeValue(IParser parser, Type expectedType, SerializerState state, IValueDeserializer nestedObjectDeserializer)
		{
			NodeEvent nodeEvent;
			parser.Accept(out nodeEvent);
			Type typeFromEvent = this.GetTypeFromEvent(nodeEvent, expectedType);
			Mark mark;
			Mark mark2;
			try
			{
				Func<IParser, Type, object> <>9__0;
				foreach (INodeDeserializer nodeDeserializer in this.deserializers)
				{
					Type expectedType2 = typeFromEvent;
					Func<IParser, Type, object> nestedObjectDeserializer2;
					if ((nestedObjectDeserializer2 = <>9__0) == null)
					{
						nestedObjectDeserializer2 = (<>9__0 = ((IParser r, Type t) => nestedObjectDeserializer.DeserializeValue(r, t, state, nestedObjectDeserializer)));
					}
					object value;
					if (nodeDeserializer.Deserialize(parser, expectedType2, nestedObjectDeserializer2, out value))
					{
						return this.typeConverter.ChangeType(value, expectedType);
					}
				}
			}
			catch (YamlException)
			{
				throw;
			}
			catch (Exception innerException)
			{
				mark = ((nodeEvent != null) ? nodeEvent.Start : Mark.Empty);
				mark2 = ((nodeEvent != null) ? nodeEvent.End : Mark.Empty);
				throw new YamlException(ref mark, ref mark2, "Exception during deserialization", innerException);
			}
			mark = ((nodeEvent != null) ? nodeEvent.Start : Mark.Empty);
			mark2 = ((nodeEvent != null) ? nodeEvent.End : Mark.Empty);
			throw new YamlException(ref mark, ref mark2, "No node deserializer was able to deserialize the node into type " + expectedType.AssemblyQualifiedName);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x000141F0 File Offset: 0x000123F0
		private Type GetTypeFromEvent([Nullable(2)] NodeEvent nodeEvent, Type currentType)
		{
			using (IEnumerator<INodeTypeResolver> enumerator = this.typeResolvers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Resolve(nodeEvent, ref currentType))
					{
						break;
					}
				}
			}
			return currentType;
		}

		// Token: 0x0400026B RID: 619
		private readonly IList<INodeDeserializer> deserializers;

		// Token: 0x0400026C RID: 620
		private readonly IList<INodeTypeResolver> typeResolvers;

		// Token: 0x0400026D RID: 621
		private readonly ITypeConverter typeConverter;
	}
}
