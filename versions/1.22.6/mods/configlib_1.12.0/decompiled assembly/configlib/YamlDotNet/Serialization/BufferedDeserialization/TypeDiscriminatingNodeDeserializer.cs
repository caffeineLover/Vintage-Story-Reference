using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators;

namespace YamlDotNet.Serialization.BufferedDeserialization
{
	// Token: 0x02000178 RID: 376
	[NullableContext(1)]
	[Nullable(0)]
	public class TypeDiscriminatingNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060007A7 RID: 1959 RVA: 0x00019F8F File Offset: 0x0001818F
		public TypeDiscriminatingNodeDeserializer(IList<INodeDeserializer> innerDeserializers, IList<ITypeDiscriminator> typeDiscriminators, int maxDepthToBuffer, int maxLengthToBuffer)
		{
			this.innerDeserializers = innerDeserializers;
			this.typeDiscriminators = typeDiscriminators;
			this.maxDepthToBuffer = maxDepthToBuffer;
			this.maxLengthToBuffer = maxLengthToBuffer;
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00019FB4 File Offset: 0x000181B4
		public bool Deserialize(IParser reader, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value)
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
			using (IEnumerator<INodeDeserializer> enumerator2 = this.innerDeserializers.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.Deserialize(parserBuffer, expectedType2, nestedObjectDeserializer, out value))
					{
						return true;
					}
				}
			}
			value = null;
			return false;
		}

		// Token: 0x0400035A RID: 858
		private readonly IList<INodeDeserializer> innerDeserializers;

		// Token: 0x0400035B RID: 859
		private readonly IList<ITypeDiscriminator> typeDiscriminators;

		// Token: 0x0400035C RID: 860
		private readonly int maxDepthToBuffer;

		// Token: 0x0400035D RID: 861
		private readonly int maxLengthToBuffer;
	}
}
