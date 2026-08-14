using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x02000192 RID: 402
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class YamlVisitorBase : IYamlVisitor
	{
		// Token: 0x0600088C RID: 2188 RVA: 0x0001C6F4 File Offset: 0x0001A8F4
		public virtual void Visit(YamlStream stream)
		{
			this.VisitChildren(stream);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0001C6FD File Offset: 0x0001A8FD
		public virtual void Visit(YamlDocument document)
		{
			this.VisitChildren(document);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00007270 File Offset: 0x00005470
		public virtual void Visit(YamlScalarNode scalar)
		{
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0001C706 File Offset: 0x0001A906
		public virtual void Visit(YamlSequenceNode sequence)
		{
			this.VisitChildren(sequence);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0001C70F File Offset: 0x0001A90F
		public virtual void Visit(YamlMappingNode mapping)
		{
			this.VisitChildren(mapping);
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0001C718 File Offset: 0x0001A918
		protected virtual void VisitPair(YamlNode key, YamlNode value)
		{
			key.Accept(this);
			value.Accept(this);
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0001C728 File Offset: 0x0001A928
		protected virtual void VisitChildren(YamlStream stream)
		{
			foreach (YamlDocument yamlDocument in stream.Documents)
			{
				yamlDocument.Accept(this);
			}
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0001C5C4 File Offset: 0x0001A7C4
		protected virtual void VisitChildren(YamlDocument document)
		{
			if (document.RootNode != null)
			{
				document.RootNode.Accept(this);
			}
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0001C774 File Offset: 0x0001A974
		protected virtual void VisitChildren(YamlSequenceNode sequence)
		{
			foreach (YamlNode yamlNode in sequence.Children)
			{
				yamlNode.Accept(this);
			}
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0001C7C0 File Offset: 0x0001A9C0
		protected virtual void VisitChildren(YamlMappingNode mapping)
		{
			foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair in mapping.Children)
			{
				this.VisitPair(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}
}
