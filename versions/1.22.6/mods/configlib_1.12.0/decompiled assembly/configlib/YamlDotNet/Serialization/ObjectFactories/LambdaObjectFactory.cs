using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.ObjectFactories
{
	// Token: 0x02000136 RID: 310
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LambdaObjectFactory : ObjectFactoryBase
	{
		// Token: 0x060006A0 RID: 1696 RVA: 0x00017057 File Offset: 0x00015257
		public LambdaObjectFactory(Func<Type, object> factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			this.factory = factory;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00017075 File Offset: 0x00015275
		public override object Create(Type type)
		{
			return this.factory.Invoke(type);
		}

		// Token: 0x040002F7 RID: 759
		private readonly Func<Type, object> factory;
	}
}
