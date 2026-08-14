using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.ObjectFactories;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000D5 RID: 213
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class StaticContext
	{
		// Token: 0x060004DA RID: 1242 RVA: 0x000127EA File Offset: 0x000109EA
		public virtual bool IsKnownType(Type type)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x000127EA File Offset: 0x000109EA
		public virtual ITypeResolver GetTypeResolver()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x000127EA File Offset: 0x000109EA
		public virtual StaticObjectFactory GetFactory()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x000127EA File Offset: 0x000109EA
		public virtual ITypeInspector GetTypeInspector()
		{
			throw new NotImplementedException();
		}
	}
}
