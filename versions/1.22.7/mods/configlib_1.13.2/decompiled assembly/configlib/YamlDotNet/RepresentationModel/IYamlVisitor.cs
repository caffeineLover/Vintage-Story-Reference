using System;
using System.Runtime.CompilerServices;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x0200017C RID: 380
	[NullableContext(1)]
	public interface IYamlVisitor
	{
		// Token: 0x06000787 RID: 1927
		void Visit(YamlStream stream);

		// Token: 0x06000788 RID: 1928
		void Visit(YamlDocument document);

		// Token: 0x06000789 RID: 1929
		void Visit(YamlScalarNode scalar);

		// Token: 0x0600078A RID: 1930
		void Visit(YamlSequenceNode sequence);

		// Token: 0x0600078B RID: 1931
		void Visit(YamlMappingNode mapping);
	}
}
