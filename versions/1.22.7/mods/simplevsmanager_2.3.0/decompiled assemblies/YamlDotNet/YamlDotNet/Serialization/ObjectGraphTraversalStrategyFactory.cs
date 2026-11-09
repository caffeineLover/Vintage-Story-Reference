using System;
using System.Collections.Generic;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000034 RID: 52
	// (Invoke) Token: 0x0600011D RID: 285
	public delegate IObjectGraphTraversalStrategy ObjectGraphTraversalStrategyFactory(ITypeInspector typeInspector, ITypeResolver typeResolver, IEnumerable<IYamlTypeConverter> typeConverters, int maximumRecursion);
}
