using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.ObjectFactories
{
	// Token: 0x02000138 RID: 312
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class StaticObjectFactory : IObjectFactory
	{
		// Token: 0x060006AB RID: 1707
		public abstract object Create(Type type);

		// Token: 0x060006AC RID: 1708
		public abstract Array CreateArray(Type type, int count);

		// Token: 0x060006AD RID: 1709
		public abstract bool IsDictionary(Type type);

		// Token: 0x060006AE RID: 1710
		public abstract bool IsArray(Type type);

		// Token: 0x060006AF RID: 1711
		public abstract bool IsList(Type type);

		// Token: 0x060006B0 RID: 1712
		public abstract Type GetKeyType(Type type);

		// Token: 0x060006B1 RID: 1713
		public abstract Type GetValueType(Type type);

		// Token: 0x060006B2 RID: 1714 RVA: 0x00017140 File Offset: 0x00015340
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

		// Token: 0x060006B3 RID: 1715 RVA: 0x00017213 File Offset: 0x00015413
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

		// Token: 0x060006B4 RID: 1716
		public abstract void ExecuteOnDeserializing(object value);

		// Token: 0x060006B5 RID: 1717
		public abstract void ExecuteOnDeserialized(object value);

		// Token: 0x060006B6 RID: 1718
		public abstract void ExecuteOnSerializing(object value);

		// Token: 0x060006B7 RID: 1719
		public abstract void ExecuteOnSerialized(object value);
	}
}
