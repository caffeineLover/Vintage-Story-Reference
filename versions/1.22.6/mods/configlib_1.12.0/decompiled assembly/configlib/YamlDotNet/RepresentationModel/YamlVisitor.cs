using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x02000191 RID: 401
	[NullableContext(1)]
	[Nullable(0)]
	[Obsolete("Use YamlVisitorBase")]
	public abstract class YamlVisitor : IYamlVisitor
	{
		// Token: 0x06000878 RID: 2168 RVA: 0x00007270 File Offset: 0x00005470
		protected virtual void Visit(YamlStream stream)
		{
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x00007270 File Offset: 0x00005470
		protected virtual void Visited(YamlStream stream)
		{
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00007270 File Offset: 0x00005470
		protected virtual void Visit(YamlDocument document)
		{
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00007270 File Offset: 0x00005470
		protected virtual void Visited(YamlDocument document)
		{
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00007270 File Offset: 0x00005470
		protected virtual void Visit(YamlScalarNode scalar)
		{
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00007270 File Offset: 0x00005470
		protected virtual void Visited(YamlScalarNode scalar)
		{
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00007270 File Offset: 0x00005470
		protected virtual void Visit(YamlSequenceNode sequence)
		{
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00007270 File Offset: 0x00005470
		protected virtual void Visited(YamlSequenceNode sequence)
		{
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00007270 File Offset: 0x00005470
		protected virtual void Visit(YamlMappingNode mapping)
		{
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00007270 File Offset: 0x00005470
		protected virtual void Visited(YamlMappingNode mapping)
		{
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0001C578 File Offset: 0x0001A778
		protected virtual void VisitChildren(YamlStream stream)
		{
			foreach (YamlDocument yamlDocument in stream.Documents)
			{
				yamlDocument.Accept(this);
			}
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0001C5C4 File Offset: 0x0001A7C4
		protected virtual void VisitChildren(YamlDocument document)
		{
			if (document.RootNode != null)
			{
				document.RootNode.Accept(this);
			}
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0001C5DC File Offset: 0x0001A7DC
		protected virtual void VisitChildren(YamlSequenceNode sequence)
		{
			foreach (YamlNode yamlNode in sequence.Children)
			{
				yamlNode.Accept(this);
			}
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0001C628 File Offset: 0x0001A828
		protected virtual void VisitChildren(YamlMappingNode mapping)
		{
			foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair in mapping.Children)
			{
				keyValuePair.Key.Accept(this);
				keyValuePair.Value.Accept(this);
			}
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0001C688 File Offset: 0x0001A888
		void IYamlVisitor.Visit(YamlStream stream)
		{
			this.Visit(stream);
			this.VisitChildren(stream);
			this.Visited(stream);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0001C69F File Offset: 0x0001A89F
		void IYamlVisitor.Visit(YamlDocument document)
		{
			this.Visit(document);
			this.VisitChildren(document);
			this.Visited(document);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0001C6B6 File Offset: 0x0001A8B6
		void IYamlVisitor.Visit(YamlScalarNode scalar)
		{
			this.Visit(scalar);
			this.Visited(scalar);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0001C6C6 File Offset: 0x0001A8C6
		void IYamlVisitor.Visit(YamlSequenceNode sequence)
		{
			this.Visit(sequence);
			this.VisitChildren(sequence);
			this.Visited(sequence);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0001C6DD File Offset: 0x0001A8DD
		void IYamlVisitor.Visit(YamlMappingNode mapping)
		{
			this.Visit(mapping);
			this.VisitChildren(mapping);
			this.Visited(mapping);
		}
	}
}
