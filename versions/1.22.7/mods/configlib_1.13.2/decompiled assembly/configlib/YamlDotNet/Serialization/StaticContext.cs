using System;
using System.Runtime.CompilerServices;
using YamlDotNet.Serialization.ObjectFactories;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000D1 RID: 209
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class StaticContext
	{
		// Token: 0x0600049E RID: 1182 RVA: 0x00011EA2 File Offset: 0x000100A2
		public virtual bool IsKnownType(Type type)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00011EA2 File Offset: 0x000100A2
		public virtual ITypeResolver GetTypeResolver()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00011EA2 File Offset: 0x000100A2
		public virtual StaticObjectFactory GetFactory()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00011EA2 File Offset: 0x000100A2
		public virtual ITypeInspector GetTypeInspector()
		{
			throw new NotImplementedException();
		}
	}
}
