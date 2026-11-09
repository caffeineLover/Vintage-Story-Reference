using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.ObjectFactories;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200003A RID: 58
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class StaticContext
	{
		// Token: 0x0600017E RID: 382 RVA: 0x00004FB9 File Offset: 0x000031B9
		public virtual bool IsKnownType(Type type)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00004FC0 File Offset: 0x000031C0
		public virtual ITypeResolver GetTypeResolver()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00004FC7 File Offset: 0x000031C7
		public virtual StaticObjectFactory GetFactory()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00004FCE File Offset: 0x000031CE
		public virtual ITypeInspector GetTypeInspector()
		{
			throw new NotImplementedException();
		}
	}
}
