using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.ObjectFactories
{
	// Token: 0x02000134 RID: 308
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class StaticObjectFactory : IObjectFactory
	{
		// Token: 0x0600066F RID: 1647
		public abstract object Create(Type type);

		// Token: 0x06000670 RID: 1648
		public abstract Array CreateArray(Type type, int count);

		// Token: 0x06000671 RID: 1649
		public abstract bool IsDictionary(Type type);

		// Token: 0x06000672 RID: 1650
		public abstract bool IsArray(Type type);

		// Token: 0x06000673 RID: 1651
		public abstract bool IsList(Type type);

		// Token: 0x06000674 RID: 1652
		public abstract Type GetKeyType(Type type);

		// Token: 0x06000675 RID: 1653
		public abstract Type GetValueType(Type type);

		// Token: 0x06000676 RID: 1654 RVA: 0x000167F8 File Offset: 0x000149F8
		[return: Nullable(2)]
		public virtual object CreatePrimitive(Type type)
		{
			switch (Type.GetTypeCode(type))
			{
			case 3:
				return false;
			case 4:
				return '\0';
			case 5:
				return 0;
			case 6:
				return 0;
			case 7:
				return 0;
			case 8:
				return 0;
			case 9:
				return 0;
			case 10:
				return 0U;
			case 11:
				return 0L;
			case 12:
				return 0UL;
			case 13:
				return 0f;
			case 14:
				return 0.0;
			case 15:
				return 0m;
			case 16:
				return default(DateTime);
			default:
				return null;
			}
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x000168CB File Offset: 0x00014ACB
		public bool GetDictionary(IObjectDescriptor descriptor, [Nullable(2)] out IDictionary dictionary, [Nullable(new byte[]
		{
			2,
			1
		})] out Type[] genericArguments)
		{
			dictionary = null;
			genericArguments = null;
			return false;
		}

		// Token: 0x06000678 RID: 1656
		public abstract void ExecuteOnDeserializing(object value);

		// Token: 0x06000679 RID: 1657
		public abstract void ExecuteOnDeserialized(object value);

		// Token: 0x0600067A RID: 1658
		public abstract void ExecuteOnSerializing(object value);

		// Token: 0x0600067B RID: 1659
		public abstract void ExecuteOnSerialized(object value);
	}
}
