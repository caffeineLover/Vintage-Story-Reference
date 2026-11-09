using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x020000A4 RID: 164
	[NullableContext(1)]
	[Nullable(0)]
	internal class DocumentLoadingState
	{
		// Token: 0x06000371 RID: 881 RVA: 0x0000C704 File Offset: 0x0000A904
		public void AddAnchor(YamlNode node)
		{
			if (node.Anchor.IsEmpty)
			{
				throw new ArgumentException("The specified node does not have an anchor");
			}
			this.anchors[node.Anchor] = node;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000C740 File Offset: 0x0000A940
		public YamlNode GetNode(AnchorName anchor, Mark start, Mark end)
		{
			YamlNode result;
			if (this.anchors.TryGetValue(anchor, ref result))
			{
				return result;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(29, 1);
			defaultInterpolatedStringHandler.AppendLiteral("The anchor '");
			defaultInterpolatedStringHandler.AppendFormatted<AnchorName>(anchor);
			defaultInterpolatedStringHandler.AppendLiteral("' does not exists");
			throw new AnchorNotFoundException(ref start, ref end, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000C799 File Offset: 0x0000A999
		[NullableContext(2)]
		public bool TryGetNode(AnchorName anchor, [NotNullWhen(true)] out YamlNode node)
		{
			return this.anchors.TryGetValue(anchor, ref node);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0000C7A8 File Offset: 0x0000A9A8
		public void AddNodeWithUnresolvedAliases(YamlNode node)
		{
			this.nodesWithUnresolvedAliases.Add(node);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0000C7B8 File Offset: 0x0000A9B8
		public void ResolveAliases()
		{
			foreach (YamlNode yamlNode in this.nodesWithUnresolvedAliases)
			{
				yamlNode.ResolveAliases(this);
			}
		}

		// Token: 0x040000FF RID: 255
		private readonly Dictionary<AnchorName, YamlNode> anchors = new Dictionary<AnchorName, YamlNode>();

		// Token: 0x04000100 RID: 256
		private readonly List<YamlNode> nodesWithUnresolvedAliases = new List<YamlNode>();
	}
}
