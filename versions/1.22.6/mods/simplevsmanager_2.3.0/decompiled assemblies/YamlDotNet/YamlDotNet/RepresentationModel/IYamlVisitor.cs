using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x020000A6 RID: 166
	[NullableContext(1)]
	public interface IYamlVisitor
	{
		// Token: 0x06000379 RID: 889
		void Visit(YamlStream stream);

		// Token: 0x0600037A RID: 890
		void Visit(YamlDocument document);

		// Token: 0x0600037B RID: 891
		void Visit(YamlScalarNode scalar);

		// Token: 0x0600037C RID: 892
		void Visit(YamlSequenceNode sequence);

		// Token: 0x0600037D RID: 893
		void Visit(YamlMappingNode mapping);
	}
}
