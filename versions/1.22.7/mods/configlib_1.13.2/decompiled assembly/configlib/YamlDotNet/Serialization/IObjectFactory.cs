using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000099 RID: 153
	[NullableContext(1)]
	public interface IObjectFactory
	{
		// Token: 0x06000393 RID: 915
		object Create(Type type);

		// Token: 0x06000394 RID: 916
		[return: Nullable(2)]
		object CreatePrimitive(Type type);

		// Token: 0x06000395 RID: 917
		bool GetDictionary(IObjectDescriptor descriptor, [Nullable(2)] out IDictionary dictionary, [Nullable(new byte[]
		{
			2,
			1
		})] out Type[] genericArguments);

		// Token: 0x06000396 RID: 918
		Type GetValueType(Type type);

		// Token: 0x06000397 RID: 919
		void ExecuteOnDeserializing(object value);

		// Token: 0x06000398 RID: 920
		void ExecuteOnDeserialized(object value);

		// Token: 0x06000399 RID: 921
		void ExecuteOnSerializing(object value);

		// Token: 0x0600039A RID: 922
		void ExecuteOnSerialized(object value);
	}
}
