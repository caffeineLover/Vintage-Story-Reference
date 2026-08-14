using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x02000180 RID: 384
	[NullableContext(1)]
	public interface IYamlVisitor
	{
		// Token: 0x060007C3 RID: 1987
		void Visit(YamlStream stream);

		// Token: 0x060007C4 RID: 1988
		void Visit(YamlDocument document);

		// Token: 0x060007C5 RID: 1989
		void Visit(YamlScalarNode scalar);

		// Token: 0x060007C6 RID: 1990
		void Visit(YamlSequenceNode sequence);

		// Token: 0x060007C7 RID: 1991
		void Visit(YamlMappingNode mapping);
	}
}
