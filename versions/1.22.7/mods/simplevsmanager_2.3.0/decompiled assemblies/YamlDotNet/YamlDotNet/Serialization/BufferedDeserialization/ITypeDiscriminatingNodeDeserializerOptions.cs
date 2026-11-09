using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.BufferedDeserialization.TypeDiscriminators;

namespace YamlDotNet.Serialization.BufferedDeserialization
{
	// Token: 0x0200009D RID: 157
	[NullableContext(1)]
	public interface ITypeDiscriminatingNodeDeserializerOptions
	{
		// Token: 0x06000356 RID: 854
		void AddTypeDiscriminator(ITypeDiscriminator discriminator);

		// Token: 0x06000357 RID: 855
		void AddKeyValueTypeDiscriminator<[Nullable(2)] T>(string discriminatorKey, IDictionary<string, Type> valueTypeMapping);

		// Token: 0x06000358 RID: 856 RVA: 0x0000C138 File Offset: 0x0000A338
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

		// Token: 0x06000359 RID: 857
		void AddUniqueKeyTypeDiscriminator<[Nullable(2)] T>(IDictionary<string, Type> uniqueKeyTypeMapping);

		// Token: 0x0600035A RID: 858 RVA: 0x0000C190 File Offset: 0x0000A390
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
