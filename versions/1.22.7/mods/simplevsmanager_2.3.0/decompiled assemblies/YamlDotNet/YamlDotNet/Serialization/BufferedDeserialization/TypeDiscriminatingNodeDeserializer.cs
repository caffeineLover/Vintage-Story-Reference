using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators;

namespace YamlDotNet.Serialization.BufferedDeserialization
{
	// Token: 0x0200009F RID: 159
	[NullableContext(1)]
	[Nullable(0)]
	public class TypeDiscriminatingNodeDeserializer : INodeDeserializer
	{
		// Token: 0x0600035F RID: 863 RVA: 0x0000C2CF File Offset: 0x0000A4CF
		public TypeDiscriminatingNodeDeserializer(IList<INodeDeserializer> innerDeserializers, IList<ITypeDiscriminator> typeDiscriminators, int maxDepthToBuffer, int maxLengthToBuffer)
		{
			this.innerDeserializers = innerDeserializers;
			this.typeDiscriminators = typeDiscriminators;
			this.maxDepthToBuffer = maxDepthToBuffer;
			this.maxLengthToBuffer = maxLengthToBuffer;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0000C2F4 File Offset: 0x0000A4F4
		public bool Deserialize(IParser reader, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value, ObjectDeserializer rootDeserializer)
		{
			MappingStart mappingStart;
			if (!reader.Accept(out mappingStart))
			{
				value = null;
				return false;
			}
			IEnumerable<ITypeDiscriminator> enumerable = Enumerable.Where<ITypeDiscriminator>(this.typeDiscriminators, (ITypeDiscriminator t) => t.BaseType.IsAssignableFrom(expectedType));
			if (!Enumerable.Any<ITypeDiscriminator>(enumerable))
			{
				value = null;
				return false;
			}
			Mark start = reader.Current.Start;
			Type expectedType2 = expectedType;
			ParserBuffer parserBuffer;
			try
			{
				parserBuffer = new ParserBuffer(reader, this.maxDepthToBuffer, this.maxLengthToBuffer);
			}
			catch (Exception innerException)
			{
				Mark end = reader.Current.End;
				throw new YamlException(ref start, ref end, "Failed to buffer yaml node", innerException);
			}
			try
			{
				foreach (ITypeDiscriminator typeDiscriminator in enumerable)
				{
					parserBuffer.Reset();
					Type type;
					if (typeDiscriminator.TryDiscriminate(parserBuffer, out type))
					{
						expectedType2 = type;
						break;
					}
				}
			}
			catch (Exception innerException2)
			{
				Mark end = reader.Current.End;
				throw new YamlException(ref start, ref end, "Failed to discriminate type", innerException2);
			}
			parserBuffer.Reset();
			foreach (INodeDeserializer nodeDeserializer in this.innerDeserializers)
			{
				if (nodeDeserializer.Deserialize(parserBuffer, expectedType2, nestedObjectDeserializer, out value, rootDeserializer))
				{
					return true;
				}
			}
			value = null;
			return false;
		}

		// Token: 0x040000F5 RID: 245
		private readonly IList<INodeDeserializer> innerDeserializers;

		// Token: 0x040000F6 RID: 246
		private readonly IList<ITypeDiscriminator> typeDiscriminators;

		// Token: 0x040000F7 RID: 247
		private readonly int maxDepthToBuffer;

		// Token: 0x040000F8 RID: 248
		private readonly int maxLengthToBuffer;
	}
}
