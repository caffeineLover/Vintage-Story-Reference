using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200001F RID: 31
	[NullableContext(1)]
	public interface IObjectFactory
	{
		// Token: 0x060000BC RID: 188
		object Create(Type type);

		// Token: 0x060000BD RID: 189
		[return: Nullable(2)]
		object CreatePrimitive(Type type);

		// Token: 0x060000BE RID: 190
		bool GetDictionary(IObjectDescriptor descriptor, [Nullable(2)] out IDictionary dictionary, [Nullable(new byte[]
		{
			2,
			1
		})] out Type[] genericArguments);

		// Token: 0x060000BF RID: 191
		Type GetValueType(Type type);

		// Token: 0x060000C0 RID: 192
		void ExecuteOnDeserializing(object value);

		// Token: 0x060000C1 RID: 193
		void ExecuteOnDeserialized(object value);

		// Token: 0x060000C2 RID: 194
		void ExecuteOnSerializing(object value);

		// Token: 0x060000C3 RID: 195
		void ExecuteOnSerialized(object value);
	}
}
