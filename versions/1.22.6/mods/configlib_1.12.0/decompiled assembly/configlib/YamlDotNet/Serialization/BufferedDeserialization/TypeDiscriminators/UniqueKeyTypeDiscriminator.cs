using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators
{
	// Token: 0x0200017D RID: 381
	[NullableContext(1)]
	[Nullable(0)]
	public class UniqueKeyTypeDiscriminator : ITypeDiscriminator
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0001A2BC File Offset: 0x000184BC
		// (set) Token: 0x060007B7 RID: 1975 RVA: 0x0001A2C4 File Offset: 0x000184C4
		public Type BaseType { get; private set; }

		// Token: 0x060007B8 RID: 1976 RVA: 0x0001A2D0 File Offset: 0x000184D0
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

		// Token: 0x060007B9 RID: 1977 RVA: 0x0001A374 File Offset: 0x00018574
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

		// Token: 0x04000364 RID: 868
		private readonly IDictionary<string, Type> typeMapping;
	}
}
