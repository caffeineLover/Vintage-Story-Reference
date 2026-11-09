using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.Serialization
{
	// Token: 0x02000020 RID: 32
	[NullableContext(1)]
	public interface IObjectGraphTraversalStrategy
	{
		// Token: 0x060000C4 RID: 196
		void Traverse<[Nullable(2)] TContext>(IObjectDescriptor graph, IObjectGraphVisitor<TContext> visitor, TContext context, ObjectSerializer serializer);
	}
}
