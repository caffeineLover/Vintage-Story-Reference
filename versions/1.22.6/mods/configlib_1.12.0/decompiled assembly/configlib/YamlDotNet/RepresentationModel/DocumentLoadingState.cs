using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x0200017E RID: 382
	[NullableContext(1)]
	[Nullable(0)]
	internal class DocumentLoadingState
	{
		// Token: 0x060007BB RID: 1979 RVA: 0x0001A3C8 File Offset: 0x000185C8
		public void AddAnchor(YamlNode node)
		{
			if (node.Anchor.IsEmpty)
			{
				throw new ArgumentException("The specified node does not have an anchor");
			}
			if (this.anchors.ContainsKey(node.Anchor))
			{
				this.anchors[node.Anchor] = node;
				return;
			}
			this.anchors.Add(node.Anchor, node);
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0001A428 File Offset: 0x00018628
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

		// Token: 0x060007BD RID: 1981 RVA: 0x0001A481 File Offset: 0x00018681
		[NullableContext(2)]
		public bool TryGetNode(AnchorName anchor, [NotNullWhen(true)] out YamlNode node)
		{
			return this.anchors.TryGetValue(anchor, ref node);
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0001A490 File Offset: 0x00018690
		public void AddNodeWithUnresolvedAliases(YamlNode node)
		{
			this.nodesWithUnresolvedAliases.Add(node);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0001A4A0 File Offset: 0x000186A0
		public void ResolveAliases()
		{
			foreach (YamlNode yamlNode in this.nodesWithUnresolvedAliases)
			{
				yamlNode.ResolveAliases(this);
			}
		}

		// Token: 0x04000365 RID: 869
		private readonly IDictionary<AnchorName, YamlNode> anchors = new Dictionary<AnchorName, YamlNode>();

		// Token: 0x04000366 RID: 870
		private readonly IList<YamlNode> nodesWithUnresolvedAliases = new List<YamlNode>();
	}
}
