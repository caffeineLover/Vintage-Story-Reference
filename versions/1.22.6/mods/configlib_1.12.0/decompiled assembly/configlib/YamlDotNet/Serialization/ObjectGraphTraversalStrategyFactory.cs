using System;
using System.Collections.Generic;

namespace YamlDotNet.Serialization
{
	// Token: 0x020000BD RID: 189
	// (Invoke) Token: 0x0600044C RID: 1100
	public delegate IObjectGraphTraversalStrategy ObjectGraphTraversalStrategyFactory(ITypeInspector typeInspector, ITypeResolver typeResolver, IEnumerable<IYamlTypeConverter> typeConverters, int maximumRecursion);
}
