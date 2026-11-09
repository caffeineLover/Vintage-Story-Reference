using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators
{
	// Token: 0x02000179 RID: 377
	[NullableContext(1)]
	[Nullable(0)]
	public class UniqueKeyTypeDiscriminator : ITypeDiscriminator
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x00019974 File Offset: 0x00017B74
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x0001997C File Offset: 0x00017B7C
		public Type BaseType { get; private set; }

		// Token: 0x0600077C RID: 1916 RVA: 0x00019988 File Offset: 0x00017B88
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

		// Token: 0x0600077D RID: 1917 RVA: 0x00019A2C File Offset: 0x00017C2C
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

		// Token: 0x04000336 RID: 822
		private readonly IDictionary<string, Type> typeMapping;
	}
}
