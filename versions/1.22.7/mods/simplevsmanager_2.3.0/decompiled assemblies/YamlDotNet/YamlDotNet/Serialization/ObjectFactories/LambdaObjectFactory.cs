using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization.ObjectFactories
{
	// Token: 0x0200006E RID: 110
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class LambdaObjectFactory : ObjectFactoryBase
	{
		// Token: 0x060002B0 RID: 688 RVA: 0x00009393 File Offset: 0x00007593
		public LambdaObjectFactory(Func<Type, object> factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			this.factory = factory;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x000093B1 File Offset: 0x000075B1
		public override object Create(Type type)
		{
			return this.factory.Invoke(type);
		}

		// Token: 0x040000AD RID: 173
		private readonly Func<Type, object> factory;
	}
}
