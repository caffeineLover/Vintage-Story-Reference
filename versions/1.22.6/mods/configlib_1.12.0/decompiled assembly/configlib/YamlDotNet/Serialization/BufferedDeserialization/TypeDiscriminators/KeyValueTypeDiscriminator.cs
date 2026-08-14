using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators
{
	// Token: 0x0200017C RID: 380
	[NullableContext(1)]
	[Nullable(0)]
	public class KeyValueTypeDiscriminator : ITypeDiscriminator
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x0001A1A3 File Offset: 0x000183A3
		// (set) Token: 0x060007B2 RID: 1970 RVA: 0x0001A1AB File Offset: 0x000183AB
		public Type BaseType { get; private set; }

		// Token: 0x060007B3 RID: 1971 RVA: 0x0001A1B4 File Offset: 0x000183B4
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

		// Token: 0x060007B4 RID: 1972 RVA: 0x0001A25C File Offset: 0x0001845C
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

		// Token: 0x04000361 RID: 865
		private readonly string targetKey;

		// Token: 0x04000362 RID: 866
		private readonly IDictionary<string, Type> typeMapping;
	}
}
