using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.ValueDeserializers
{
	// Token: 0x02000049 RID: 73
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class NodeValueDeserializer : IValueDeserializer
	{
		// Token: 0x0600020F RID: 527 RVA: 0x00006D5C File Offset: 0x00004F5C
		public NodeValueDeserializer(IList<INodeDeserializer> deserializers, IList<INodeTypeResolver> typeResolvers, ITypeConverter typeConverter, INamingConvention enumNamingConvention, ITypeInspector typeInspector)
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
			if (enumNamingConvention == null)
			{
				throw new ArgumentNullException("enumNamingConvention");
			}
			this.enumNamingConvention = enumNamingConvention;
			this.typeInspector = typeInspector;
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00006DD0 File Offset: 0x00004FD0
		[return: Nullable(2)]
		public object DeserializeValue(IParser parser, Type expectedType, SerializerState state, IValueDeserializer nestedObjectDeserializer)
		{
			NodeEvent nodeEvent;
			parser.Accept(out nodeEvent);
			Type typeFromEvent = this.GetTypeFromEvent(nodeEvent, expectedType);
			ObjectDeserializer rootDeserializer = (Type x) => this.DeserializeValue(parser, x, state, nestedObjectDeserializer);
			Mark mark;
			Mark mark2;
			try
			{
				Func<IParser, Type, object> <>9__1;
				foreach (INodeDeserializer nodeDeserializer in this.deserializers)
				{
					INodeDeserializer nodeDeserializer2 = nodeDeserializer;
					IParser parser2 = parser;
					Type expectedType2 = typeFromEvent;
					Func<IParser, Type, object> nestedObjectDeserializer2;
					if ((nestedObjectDeserializer2 = <>9__1) == null)
					{
						nestedObjectDeserializer2 = (<>9__1 = ((IParser r, Type t) => nestedObjectDeserializer.DeserializeValue(r, t, state, nestedObjectDeserializer)));
					}
					object value;
					bool flag = nodeDeserializer2.Deserialize(parser2, expectedType2, nestedObjectDeserializer2, out value, rootDeserializer);
					if (flag)
					{
						return this.typeConverter.ChangeType(value, expectedType, this.enumNamingConvention, this.typeInspector);
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

		// Token: 0x06000211 RID: 529 RVA: 0x00006F50 File Offset: 0x00005150
		private Type GetTypeFromEvent([Nullable(2)] NodeEvent nodeEvent, Type currentType)
		{
			foreach (INodeTypeResolver nodeTypeResolver in this.typeResolvers)
			{
				if (nodeTypeResolver.Resolve(nodeEvent, ref currentType))
				{
					break;
				}
			}
			return currentType;
		}

		// Token: 0x0400007E RID: 126
		private readonly IList<INodeDeserializer> deserializers;

		// Token: 0x0400007F RID: 127
		private readonly IList<INodeTypeResolver> typeResolvers;

		// Token: 0x04000080 RID: 128
		private readonly ITypeConverter typeConverter;

		// Token: 0x04000081 RID: 129
		private readonly INamingConvention enumNamingConvention;

		// Token: 0x04000082 RID: 130
		private readonly ITypeInspector typeInspector;
	}
}
