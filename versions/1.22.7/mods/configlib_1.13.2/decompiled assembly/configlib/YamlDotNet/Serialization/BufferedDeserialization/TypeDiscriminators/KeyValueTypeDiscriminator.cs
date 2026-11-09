using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators
{
	// Token: 0x02000178 RID: 376
	[NullableContext(1)]
	[Nullable(0)]
	public class KeyValueTypeDiscriminator : ITypeDiscriminator
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x0001985B File Offset: 0x00017A5B
		// (set) Token: 0x06000776 RID: 1910 RVA: 0x00019863 File Offset: 0x00017A63
		public Type BaseType { get; private set; }

		// Token: 0x06000777 RID: 1911 RVA: 0x0001986C File Offset: 0x00017A6C
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

		// Token: 0x06000778 RID: 1912 RVA: 0x00019914 File Offset: 0x00017B14
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

		// Token: 0x04000333 RID: 819
		private readonly string targetKey;

		// Token: 0x04000334 RID: 820
		private readonly IDictionary<string, Type> typeMapping;
	}
}
