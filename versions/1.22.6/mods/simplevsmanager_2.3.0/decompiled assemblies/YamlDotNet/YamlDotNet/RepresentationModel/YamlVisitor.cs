using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x020000B1 RID: 177
	[NullableContext(1)]
	[Nullable(0)]
	[Obsolete("Use YamlVisitorBase")]
	public abstract class YamlVisitor : IYamlVisitor
	{
		// Token: 0x06000400 RID: 1024 RVA: 0x0000E0F7 File Offset: 0x0000C2F7
		protected virtual void Visit(YamlStream stream)
		{
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0000E0F9 File Offset: 0x0000C2F9
		protected virtual void Visited(YamlStream stream)
		{
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0000E0FB File Offset: 0x0000C2FB
		protected virtual void Visit(YamlDocument document)
		{
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000E0FD File Offset: 0x0000C2FD
		protected virtual void Visited(YamlDocument document)
		{
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000E0FF File Offset: 0x0000C2FF
		protected virtual void Visit(YamlScalarNode scalar)
		{
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000E101 File Offset: 0x0000C301
		protected virtual void Visited(YamlScalarNode scalar)
		{
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000E103 File Offset: 0x0000C303
		protected virtual void Visit(YamlSequenceNode sequence)
		{
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000E105 File Offset: 0x0000C305
		protected virtual void Visited(YamlSequenceNode sequence)
		{
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0000E107 File Offset: 0x0000C307
		protected virtual void Visit(YamlMappingNode mapping)
		{
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0000E109 File Offset: 0x0000C309
		protected virtual void Visited(YamlMappingNode mapping)
		{
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000E10C File Offset: 0x0000C30C
		protected virtual void VisitChildren(YamlStream stream)
		{
			foreach (YamlDocument yamlDocument in stream.Documents)
			{
				yamlDocument.Accept(this);
			}
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000E15C File Offset: 0x0000C35C
		protected virtual void VisitChildren(YamlDocument document)
		{
			if (document.RootNode != null)
			{
				document.RootNode.Accept(this);
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0000E174 File Offset: 0x0000C374
		protected virtual void VisitChildren(YamlSequenceNode sequence)
		{
			foreach (YamlNode yamlNode in sequence.Children)
			{
				yamlNode.Accept(this);
			}
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0000E1C4 File Offset: 0x0000C3C4
		protected virtual void VisitChildren(YamlMappingNode mapping)
		{
			foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair in mapping.Children)
			{
				keyValuePair.Key.Accept(this);
				keyValuePair.Value.Accept(this);
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0000E224 File Offset: 0x0000C424
		void IYamlVisitor.Visit(YamlStream stream)
		{
			this.Visit(stream);
			this.VisitChildren(stream);
			this.Visited(stream);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0000E23B File Offset: 0x0000C43B
		void IYamlVisitor.Visit(YamlDocument document)
		{
			this.Visit(document);
			this.VisitChildren(document);
			this.Visited(document);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0000E252 File Offset: 0x0000C452
		void IYamlVisitor.Visit(YamlScalarNode scalar)
		{
			this.Visit(scalar);
			this.Visited(scalar);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000E262 File Offset: 0x0000C462
		void IYamlVisitor.Visit(YamlSequenceNode sequence)
		{
			this.Visit(sequence);
			this.VisitChildren(sequence);
			this.Visited(sequence);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000E279 File Offset: 0x0000C479
		void IYamlVisitor.Visit(YamlMappingNode mapping)
		{
			this.Visit(mapping);
			this.VisitChildren(mapping);
			this.Visited(mapping);
		}
	}
}
