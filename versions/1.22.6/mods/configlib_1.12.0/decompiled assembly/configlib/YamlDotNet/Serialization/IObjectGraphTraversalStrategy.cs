using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200009E RID: 158
	[NullableContext(1)]
	public interface IObjectGraphTraversalStrategy
	{
		// Token: 0x060003D7 RID: 983
		void Traverse<[Nullable(2)] TContext>(IObjectDescriptor graph, IObjectGraphVisitor<TContext> visitor, TContext context);
	}
}
