using System;
using System.Collections.Generic;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000B9 RID: 185
	// (Invoke) Token: 0x06000410 RID: 1040
	public delegate IObjectGraphTraversalStrategy ObjectGraphTraversalStrategyFactory(ITypeInspector typeInspector, ITypeResolver typeResolver, IEnumerable<IYamlTypeConverter> typeConverters, int maximumRecursion);
}
