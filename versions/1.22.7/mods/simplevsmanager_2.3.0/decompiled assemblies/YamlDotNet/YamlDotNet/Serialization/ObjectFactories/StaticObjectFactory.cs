using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.ObjectFactories
{
	// Token: 0x02000070 RID: 112
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class StaticObjectFactory : IObjectFactory
	{
		// Token: 0x060002BB RID: 699
		public abstract object Create(Type type);

		// Token: 0x060002BC RID: 700
		public abstract Array CreateArray(Type type, int count);

		// Token: 0x060002BD RID: 701
		public abstract bool IsDictionary(Type type);

		// Token: 0x060002BE RID: 702
		public abstract bool IsArray(Type type);

		// Token: 0x060002BF RID: 703
		public abstract bool IsList(Type type);

		// Token: 0x060002C0 RID: 704
		public abstract Type GetKeyType(Type type);

		// Token: 0x060002C1 RID: 705
		public abstract Type GetValueType(Type type);

		// Token: 0x060002C2 RID: 706 RVA: 0x00009490 File Offset: 0x00007690
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

		// Token: 0x060002C3 RID: 707 RVA: 0x00009563 File Offset: 0x00007763
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

		// Token: 0x060002C4 RID: 708
		public abstract void ExecuteOnDeserializing(object value);

		// Token: 0x060002C5 RID: 709
		public abstract void ExecuteOnDeserialized(object value);

		// Token: 0x060002C6 RID: 710
		public abstract void ExecuteOnSerializing(object value);

		// Token: 0x060002C7 RID: 711
		public abstract void ExecuteOnSerialized(object value);
	}
}
