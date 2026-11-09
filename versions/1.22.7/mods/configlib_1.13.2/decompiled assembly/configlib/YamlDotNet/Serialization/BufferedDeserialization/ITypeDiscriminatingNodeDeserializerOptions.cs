using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators;

namespace YamlDotNet.Serialization.BufferedDeserialization
{
	// Token: 0x02000170 RID: 368
	[NullableContext(1)]
	public interface ITypeDiscriminatingNodeDeserializerOptions
	{
		// Token: 0x0600075A RID: 1882
		void AddTypeDiscriminator(ITypeDiscriminator discriminator);

		// Token: 0x0600075B RID: 1883
		void AddKeyValueTypeDiscriminator<[Nullable(2)] T>(string discriminatorKey, IDictionary<string, Type> valueTypeMapping);

		// Token: 0x0600075C RID: 1884 RVA: 0x00019488 File Offset: 0x00017688
		void AddKeyValueTypeDiscriminator<[Nullable(2)] T>(string discriminatorKey, [Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] params ValueTuple<string, Type>[] valueTypeMapping)
		{
			Func<ValueTuple<string, Type>, string> func;
			if ((func = ITypeDiscriminatingNodeDeserializerOptions.<>c__2<T>.<>9__2_0) == null)
			{
				func = (ITypeDiscriminatingNodeDeserializerOptions.<>c__2<T>.<>9__2_0 = new Func<ValueTuple<string, Type>, string>(ITypeDiscriminatingNodeDeserializerOptions.<>c__2<T>.<>9.<AddKeyValueTypeDiscriminator>b__2_0));
			}
			Func<ValueTuple<string, Type>, Type> func2;
			if ((func2 = ITypeDiscriminatingNodeDeserializerOptions.<>c__2<T>.<>9__2_1) == null)
			{
				func2 = (ITypeDiscriminatingNodeDeserializerOptions.<>c__2<T>.<>9__2_1 = new Func<ValueTuple<string, Type>, Type>(ITypeDiscriminatingNodeDeserializerOptions.<>c__2<T>.<>9.<AddKeyValueTypeDiscriminator>b__2_1));
			}
			this.AddKeyValueTypeDiscriminator<T>(discriminatorKey, Enumerable.ToDictionary<ValueTuple<string, Type>, string, Type>(valueTypeMapping, func, func2));
		}

		// Token: 0x0600075D RID: 1885
		void AddUniqueKeyTypeDiscriminator<[Nullable(2)] T>(IDictionary<string, Type> uniqueKeyTypeMapping);

		// Token: 0x0600075E RID: 1886 RVA: 0x000194E0 File Offset: 0x000176E0
		[NullableContext(2)]
		void AddUniqueKeyTypeDiscriminator<T>([Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] params ValueTuple<string, Type>[] uniqueKeyTypeMapping)
		{
			Func<ValueTuple<string, Type>, string> func;
			if ((func = ITypeDiscriminatingNodeDeserializerOptions.<>c__4<T>.<>9__4_0) == null)
			{
				func = (ITypeDiscriminatingNodeDeserializerOptions.<>c__4<T>.<>9__4_0 = new Func<ValueTuple<string, Type>, string>(ITypeDiscriminatingNodeDeserializerOptions.<>c__4<T>.<>9.<AddUniqueKeyTypeDiscriminator>b__4_0));
			}
			Func<ValueTuple<string, Type>, Type> func2;
			if ((func2 = ITypeDiscriminatingNodeDeserializerOptions.<>c__4<T>.<>9__4_1) == null)
			{
				func2 = (ITypeDiscriminatingNodeDeserializerOptions.<>c__4<T>.<>9__4_1 = new Func<ValueTuple<string, Type>, Type>(ITypeDiscriminatingNodeDeserializerOptions.<>c__4<T>.<>9.<AddUniqueKeyTypeDiscriminator>b__4_1));
			}
			this.AddUniqueKeyTypeDiscriminator<T>(Enumerable.ToDictionary<ValueTuple<string, Type>, string, Type>(uniqueKeyTypeMapping, func, func2));
		}
	}
}
