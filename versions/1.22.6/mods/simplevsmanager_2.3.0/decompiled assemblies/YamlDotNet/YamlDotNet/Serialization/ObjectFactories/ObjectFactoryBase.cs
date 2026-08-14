using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Helpers;

namespace YamlDotNet.Serialization.ObjectFactories
{
	// Token: 0x0200006F RID: 111
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ObjectFactoryBase : IObjectFactory
	{
		// Token: 0x060002B2 RID: 690
		public abstract object Create(Type type);

		// Token: 0x060002B3 RID: 691 RVA: 0x000093BF File Offset: 0x000075BF
		[return: Nullable(2)]
		public virtual object CreatePrimitive(Type type)
		{
			if (!type.IsValueType())
			{
				return null;
			}
			return Activator.CreateInstance(type);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x000093D1 File Offset: 0x000075D1
		public virtual void ExecuteOnDeserialized(object value)
		{
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x000093D3 File Offset: 0x000075D3
		public virtual void ExecuteOnDeserializing(object value)
		{
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x000093D5 File Offset: 0x000075D5
		public virtual void ExecuteOnSerialized(object value)
		{
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x000093D7 File Offset: 0x000075D7
		public virtual void ExecuteOnSerializing(object value)
		{
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x000093DC File Offset: 0x000075DC
		public virtual bool GetDictionary(IObjectDescriptor descriptor, [Nullable(2)] out IDictionary dictionary, [Nullable(new byte[]
		{
			2,
			1
		})] out Type[] genericArguments)
		{
			Type implementationOfOpenGenericInterface = descriptor.Type.GetImplementationOfOpenGenericInterface(typeof(IDictionary));
			if (implementationOfOpenGenericInterface != null)
			{
				genericArguments = implementationOfOpenGenericInterface.GetGenericArguments();
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

		// Token: 0x060002B9 RID: 697 RVA: 0x00009448 File Offset: 0x00007648
		public virtual Type GetValueType(Type type)
		{
			Type implementationOfOpenGenericInterface = type.GetImplementationOfOpenGenericInterface(typeof(IEnumerable));
			return (implementationOfOpenGenericInterface != null) ? implementationOfOpenGenericInterface.GetGenericArguments()[0] : typeof(object);
		}
	}
}
