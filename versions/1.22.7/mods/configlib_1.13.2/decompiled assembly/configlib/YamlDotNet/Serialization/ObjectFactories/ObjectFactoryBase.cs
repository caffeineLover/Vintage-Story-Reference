using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Helpers;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.ObjectFactories
{
	// Token: 0x02000133 RID: 307
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ObjectFactoryBase : IObjectFactory
	{
		// Token: 0x06000666 RID: 1638
		public abstract object Create(Type type);

		// Token: 0x06000667 RID: 1639 RVA: 0x0001673B File Offset: 0x0001493B
		[return: Nullable(2)]
		public virtual object CreatePrimitive(Type type)
		{
			if (!type.IsValueType())
			{
				return null;
			}
			return Activator.CreateInstance(type);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00002176 File Offset: 0x00000376
		public virtual void ExecuteOnDeserialized(object value)
		{
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00002176 File Offset: 0x00000376
		public virtual void ExecuteOnDeserializing(object value)
		{
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00002176 File Offset: 0x00000376
		public virtual void ExecuteOnSerialized(object value)
		{
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x00002176 File Offset: 0x00000376
		public virtual void ExecuteOnSerializing(object value)
		{
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00016750 File Offset: 0x00014950
		public virtual bool GetDictionary(IObjectDescriptor descriptor, [Nullable(2)] out IDictionary dictionary, [Nullable(new byte[]
		{
			2,
			1
		})] out Type[] genericArguments)
		{
			Type implementedGenericInterface = ReflectionUtility.GetImplementedGenericInterface(descriptor.Type, typeof(IDictionary));
			if (implementedGenericInterface != null)
			{
				genericArguments = implementedGenericInterface.GetGenericArguments();
				object obj = Activator.CreateInstance(typeof(GenericDictionaryToNonGenericAdapter<, >).MakeGenericType(genericArguments), new object[]
				{
					descriptor.Value
				});
				dictionary = (obj as IDictionary);
				return true;
			}
			genericArguments = null;
			dictionary = null;
			return false;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x000167BC File Offset: 0x000149BC
		public virtual Type GetValueType(Type type)
		{
			Type implementedGenericInterface = ReflectionUtility.GetImplementedGenericInterface(type, typeof(IEnumerable));
			if (!(implementedGenericInterface != null))
			{
				return typeof(object);
			}
			return implementedGenericInterface.GetGenericArguments()[0];
		}
	}
}
