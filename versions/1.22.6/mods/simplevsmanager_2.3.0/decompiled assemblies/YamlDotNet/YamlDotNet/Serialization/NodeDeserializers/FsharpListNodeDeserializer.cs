using System;
using System.Collections;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Helpers;

namespace YamlDotNet.Serialization.NodeDeserializers
{
	// Token: 0x0200007E RID: 126
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FsharpListNodeDeserializer : INodeDeserializer
	{
		// Token: 0x060002E5 RID: 741 RVA: 0x00009F57 File Offset: 0x00008157
		public FsharpListNodeDeserializer(ITypeInspector typeInspector, INamingConvention enumNamingConvention)
		{
			this.typeInspector = typeInspector;
			this.enumNamingConvention = enumNamingConvention;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00009F70 File Offset: 0x00008170
		public bool Deserialize(IParser parser, Type expectedType, [Nullable(new byte[]
		{
			1,
			1,
			1,
			2
		})] Func<IParser, Type, object> nestedObjectDeserializer, [Nullable(2)] out object value, ObjectDeserializer rootDeserializer)
		{
			if (!FsharpHelper.IsFsharpListType(expectedType))
			{
				value = false;
				return false;
			}
			Type type = expectedType.GetGenericArguments()[0];
			Type t = expectedType.GetGenericTypeDefinition().MakeGenericType(new Type[]
			{
				type
			});
			ArrayList arrayList = new ArrayList();
			CollectionNodeDeserializer.DeserializeHelper(type, parser, nestedObjectDeserializer, arrayList, true, this.enumNamingConvention, this.typeInspector, null);
			Array array = Array.CreateInstance(type, arrayList.Count);
			arrayList.CopyTo(array, 0);
			object obj = FsharpHelper.CreateFsharpListFromArray(t, type, array);
			value = obj;
			return true;
		}

		// Token: 0x040000B7 RID: 183
		private readonly ITypeInspector typeInspector;

		// Token: 0x040000B8 RID: 184
		private readonly INamingConvention enumNamingConvention;
	}
}
