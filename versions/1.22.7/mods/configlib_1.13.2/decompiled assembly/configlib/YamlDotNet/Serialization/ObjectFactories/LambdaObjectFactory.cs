using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.ObjectFactories
{
	// Token: 0x02000132 RID: 306
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LambdaObjectFactory : ObjectFactoryBase
	{
		// Token: 0x06000664 RID: 1636 RVA: 0x0001670F File Offset: 0x0001490F
		public LambdaObjectFactory(Func<Type, object> factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			this.factory = factory;
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0001672D File Offset: 0x0001492D
		public override object Create(Type type)
		{
			return this.factory.Invoke(type);
		}

		// Token: 0x040002C9 RID: 713
		private readonly Func<Type, object> factory;
	}
}
