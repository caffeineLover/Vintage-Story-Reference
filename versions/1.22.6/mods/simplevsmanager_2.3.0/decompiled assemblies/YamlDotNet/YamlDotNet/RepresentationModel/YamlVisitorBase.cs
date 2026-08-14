using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x020000B2 RID: 178
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class YamlVisitorBase : IYamlVisitor
	{
		// Token: 0x06000414 RID: 1044 RVA: 0x0000E298 File Offset: 0x0000C498
		public virtual void Visit(YamlStream stream)
		{
			this.VisitChildren(stream);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0000E2A1 File Offset: 0x0000C4A1
		public virtual void Visit(YamlDocument document)
		{
			this.VisitChildren(document);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x0000E2AA File Offset: 0x0000C4AA
		public virtual void Visit(YamlScalarNode scalar)
		{
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0000E2AC File Offset: 0x0000C4AC
		public virtual void Visit(YamlSequenceNode sequence)
		{
			this.VisitChildren(sequence);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0000E2B5 File Offset: 0x0000C4B5
		public virtual void Visit(YamlMappingNode mapping)
		{
			this.VisitChildren(mapping);
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000E2BE File Offset: 0x0000C4BE
		protected virtual void VisitPair(YamlNode key, YamlNode value)
		{
			key.Accept(this);
			value.Accept(this);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000E2D0 File Offset: 0x0000C4D0
		protected virtual void VisitChildren(YamlStream stream)
		{
			foreach (YamlDocument yamlDocument in stream.Documents)
			{
				yamlDocument.Accept(this);
			}
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000E320 File Offset: 0x0000C520
		protected virtual void VisitChildren(YamlDocument document)
		{
			if (document.RootNode != null)
			{
				document.RootNode.Accept(this);
			}
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000E338 File Offset: 0x0000C538
		protected virtual void VisitChildren(YamlSequenceNode sequence)
		{
			foreach (YamlNode yamlNode in sequence.Children)
			{
				yamlNode.Accept(this);
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000E388 File Offset: 0x0000C588
		protected virtual void VisitChildren(YamlMappingNode mapping)
		{
			foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair in mapping.Children)
			{
				this.VisitPair(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}
}
