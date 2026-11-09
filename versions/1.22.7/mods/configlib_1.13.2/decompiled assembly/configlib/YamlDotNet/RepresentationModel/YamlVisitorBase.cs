using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x0200018E RID: 398
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class YamlVisitorBase : IYamlVisitor
	{
		// Token: 0x06000850 RID: 2128 RVA: 0x0001BDAC File Offset: 0x00019FAC
		public virtual void Visit(YamlStream stream)
		{
			this.VisitChildren(stream);
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0001BDB5 File Offset: 0x00019FB5
		public virtual void Visit(YamlDocument document)
		{
			this.VisitChildren(document);
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x00002176 File Offset: 0x00000376
		public virtual void Visit(YamlScalarNode scalar)
		{
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0001BDBE File Offset: 0x00019FBE
		public virtual void Visit(YamlSequenceNode sequence)
		{
			this.VisitChildren(sequence);
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0001BDC7 File Offset: 0x00019FC7
		public virtual void Visit(YamlMappingNode mapping)
		{
			this.VisitChildren(mapping);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0001BDD0 File Offset: 0x00019FD0
		protected virtual void VisitPair(YamlNode key, YamlNode value)
		{
			key.Accept(this);
			value.Accept(this);
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0001BDE0 File Offset: 0x00019FE0
		protected virtual void VisitChildren(YamlStream stream)
		{
			foreach (YamlDocument yamlDocument in stream.Documents)
			{
				yamlDocument.Accept(this);
			}
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0001BC7C File Offset: 0x00019E7C
		protected virtual void VisitChildren(YamlDocument document)
		{
			if (document.RootNode != null)
			{
				document.RootNode.Accept(this);
			}
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0001BE2C File Offset: 0x0001A02C
		protected virtual void VisitChildren(YamlSequenceNode sequence)
		{
			foreach (YamlNode yamlNode in sequence.Children)
			{
				yamlNode.Accept(this);
			}
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0001BE78 File Offset: 0x0001A078
		protected virtual void VisitChildren(YamlMappingNode mapping)
		{
			foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair in mapping.Children)
			{
				this.VisitPair(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}
}
