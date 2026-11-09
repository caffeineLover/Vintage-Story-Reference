using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x0200009A RID: 154
	[NullableContext(1)]
	public interface IObjectGraphTraversalStrategy
	{
		// Token: 0x0600039B RID: 923
		void Traverse<[Nullable(2)] TContext>(IObjectDescriptor graph, IObjectGraphVisitor<TContext> visitor, TContext context);
	}
}
