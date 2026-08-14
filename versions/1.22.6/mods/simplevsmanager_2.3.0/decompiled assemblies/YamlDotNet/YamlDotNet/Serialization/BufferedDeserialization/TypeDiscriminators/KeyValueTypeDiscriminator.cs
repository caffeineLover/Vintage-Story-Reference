using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators
{
	// Token: 0x020000A2 RID: 162
	[NullableContext(1)]
	[Nullable(0)]
	public class KeyValueTypeDiscriminator : ITypeDiscriminator
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000367 RID: 871 RVA: 0x0000C4DC File Offset: 0x0000A6DC
		// (set) Token: 0x06000368 RID: 872 RVA: 0x0000C4E4 File Offset: 0x0000A6E4
		public Type BaseType { get; private set; }

		// Token: 0x06000369 RID: 873 RVA: 0x0000C4F0 File Offset: 0x0000A6F0
		public KeyValueTypeDiscriminator(Type baseType, string targetKey, IDictionary<string, Type> typeMapping)
		{
			foreach (KeyValuePair<string, Type> keyValuePair in typeMapping)
			{
				if (!baseType.IsAssignableFrom(keyValuePair.Value))
				{
					string text = "typeMapping";
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 2);
					defaultInterpolatedStringHandler.AppendFormatted<Type>(keyValuePair.Value);
					defaultInterpolatedStringHandler.AppendLiteral(" is not a assignable to ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(baseType);
					throw new ArgumentOutOfRangeException(text, defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			this.BaseType = baseType;
			this.targetKey = targetKey;
			this.typeMapping = typeMapping;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x0000C598 File Offset: 0x0000A798
		public bool TryDiscriminate(IParser parser, [Nullable(2)] out Type suggestedType)
		{
			Scalar scalar3;
			ParsingEvent parsingEvent;
			if (parser.TryFindMappingEntry((Scalar scalar) => this.targetKey == scalar.Value, out scalar3, out parsingEvent))
			{
				Scalar scalar2 = parsingEvent as Scalar;
				Type type;
				if (scalar2 != null && this.typeMapping.TryGetValue(scalar2.Value, ref type))
				{
					suggestedType = type;
					return true;
				}
			}
			suggestedType = null;
			return false;
		}

		// Token: 0x040000FB RID: 251
		private readonly string targetKey;

		// Token: 0x040000FC RID: 252
		private readonly IDictionary<string, Type> typeMapping;
	}
}
