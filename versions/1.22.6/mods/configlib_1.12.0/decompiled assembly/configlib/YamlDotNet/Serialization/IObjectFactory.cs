using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200009D RID: 157
	[NullableContext(1)]
	public interface IObjectFactory
	{
		// Token: 0x060003CF RID: 975
		object Create(Type type);

		// Token: 0x060003D0 RID: 976
		[return: Nullable(2)]
		object CreatePrimitive(Type type);

		// Token: 0x060003D1 RID: 977
		bool GetDictionary(IObjectDescriptor descriptor, [Nullable(2)] out IDictionary dictionary, [Nullable(new byte[]
		{
			2,
			1
		})] out Type[] genericArguments);

		// Token: 0x060003D2 RID: 978
		Type GetValueType(Type type);

		// Token: 0x060003D3 RID: 979
		void ExecuteOnDeserializing(object value);

		// Token: 0x060003D4 RID: 980
		void ExecuteOnDeserialized(object value);

		// Token: 0x060003D5 RID: 981
		void ExecuteOnSerializing(object value);

		// Token: 0x060003D6 RID: 982
		void ExecuteOnSerialized(object value);
	}
}
