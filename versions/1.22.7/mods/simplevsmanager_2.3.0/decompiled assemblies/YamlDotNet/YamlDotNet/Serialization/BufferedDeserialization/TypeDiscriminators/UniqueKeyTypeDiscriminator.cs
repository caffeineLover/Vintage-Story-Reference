using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators
{
	// Token: 0x020000A3 RID: 163
	[NullableContext(1)]
	[Nullable(0)]
	public class UniqueKeyTypeDiscriminator : ITypeDiscriminator
	{
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600036C RID: 876 RVA: 0x0000C5F8 File Offset: 0x0000A7F8
		// (set) Token: 0x0600036D RID: 877 RVA: 0x0000C600 File Offset: 0x0000A800
		public Type BaseType { get; private set; }

		// Token: 0x0600036E RID: 878 RVA: 0x0000C60C File Offset: 0x0000A80C
		public UniqueKeyTypeDiscriminator(Type baseType, IDictionary<string, Type> typeMapping)
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
			this.typeMapping = typeMapping;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000C6B0 File Offset: 0x0000A8B0
		public bool TryDiscriminate(IParser parser, [Nullable(2)] out Type suggestedType)
		{
			Scalar scalar2;
			ParsingEvent parsingEvent;
			if (parser.TryFindMappingEntry((Scalar scalar) => this.typeMapping.ContainsKey(scalar.Value), out scalar2, out parsingEvent))
			{
				suggestedType = this.typeMapping[scalar2.Value];
				return true;
			}
			suggestedType = null;
			return false;
		}

		// Token: 0x040000FE RID: 254
		private readonly IDictionary<string, Type> typeMapping;
	}
}
