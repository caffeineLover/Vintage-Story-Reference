using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Helpers;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.ObjectFactories
{
	// Token: 0x02000137 RID: 311
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ObjectFactoryBase : IObjectFactory
	{
		// Token: 0x060006A2 RID: 1698
		public abstract object Create(Type type);

		// Token: 0x060006A3 RID: 1699 RVA: 0x00017083 File Offset: 0x00015283
		[return: Nullable(2)]
		public virtual object CreatePrimitive(Type type)
		{
			if (!type.IsValueType())
			{
				return null;
			}
			return Activator.CreateInstance(type);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00007270 File Offset: 0x00005470
		public virtual void ExecuteOnDeserialized(object value)
		{
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00007270 File Offset: 0x00005470
		public virtual void ExecuteOnDeserializing(object value)
		{
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00007270 File Offset: 0x00005470
		public virtual void ExecuteOnSerialized(object value)
		{
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00007270 File Offset: 0x00005470
		public virtual void ExecuteOnSerializing(object value)
		{
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00017098 File Offset: 0x00015298
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

		// Token: 0x060006A9 RID: 1705 RVA: 0x00017104 File Offset: 0x00015304
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
