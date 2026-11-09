using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x0200018D RID: 397
	[NullableContext(1)]
	[Nullable(0)]
	[Obsolete("Use YamlVisitorBase")]
	public abstract class YamlVisitor : IYamlVisitor
	{
		// Token: 0x0600083C RID: 2108 RVA: 0x00002176 File Offset: 0x00000376
		protected virtual void Visit(YamlStream stream)
		{
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00002176 File Offset: 0x00000376
		protected virtual void Visited(YamlStream stream)
		{
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00002176 File Offset: 0x00000376
		protected virtual void Visit(YamlDocument document)
		{
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x00002176 File Offset: 0x00000376
		protected virtual void Visited(YamlDocument document)
		{
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x00002176 File Offset: 0x00000376
		protected virtual void Visit(YamlScalarNode scalar)
		{
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00002176 File Offset: 0x00000376
		protected virtual void Visited(YamlScalarNode scalar)
		{
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x00002176 File Offset: 0x00000376
		protected virtual void Visit(YamlSequenceNode sequence)
		{
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00002176 File Offset: 0x00000376
		protected virtual void Visited(YamlSequenceNode sequence)
		{
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00002176 File Offset: 0x00000376
		protected virtual void Visit(YamlMappingNode mapping)
		{
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00002176 File Offset: 0x00000376
		protected virtual void Visited(YamlMappingNode mapping)
		{
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0001BC30 File Offset: 0x00019E30
		protected virtual void VisitChildren(YamlStream stream)
		{
			foreach (YamlDocument yamlDocument in stream.Documents)
			{
				yamlDocument.Accept(this);
			}
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0001BC7C File Offset: 0x00019E7C
		protected virtual void VisitChildren(YamlDocument document)
		{
			if (document.RootNode != null)
			{
				document.RootNode.Accept(this);
			}
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0001BC94 File Offset: 0x00019E94
		protected virtual void VisitChildren(YamlSequenceNode sequence)
		{
			foreach (YamlNode yamlNode in sequence.Children)
			{
				yamlNode.Accept(this);
			}
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0001BCE0 File Offset: 0x00019EE0
		protected virtual void VisitChildren(YamlMappingNode mapping)
		{
			foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair in mapping.Children)
			{
				keyValuePair.Key.Accept(this);
				keyValuePair.Value.Accept(this);
			}
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0001BD40 File Offset: 0x00019F40
		void IYamlVisitor.Visit(YamlStream stream)
		{
			this.Visit(stream);
			this.VisitChildren(stream);
			this.Visited(stream);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0001BD57 File Offset: 0x00019F57
		void IYamlVisitor.Visit(YamlDocument document)
		{
			this.Visit(document);
			this.VisitChildren(document);
			this.Visited(document);
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x0001BD6E File Offset: 0x00019F6E
		void IYamlVisitor.Visit(YamlScalarNode scalar)
		{
			this.Visit(scalar);
			this.Visited(scalar);
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x0001BD7E File Offset: 0x00019F7E
		void IYamlVisitor.Visit(YamlSequenceNode sequence)
		{
			this.Visit(sequence);
			this.VisitChildren(sequence);
			this.Visited(sequence);
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0001BD95 File Offset: 0x00019F95
		void IYamlVisitor.Visit(YamlMappingNode mapping)
		{
			this.Visit(mapping);
			this.VisitChildren(mapping);
			this.Visited(mapping);
		}
	}
}
