using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x0200017A RID: 378
	[NullableContext(1)]
	[Nullable(0)]
	internal class DocumentLoadingState
	{
		// Token: 0x0600077F RID: 1919 RVA: 0x00019A80 File Offset: 0x00017C80
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

		// Token: 0x06000780 RID: 1920 RVA: 0x00019AE0 File Offset: 0x00017CE0
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

		// Token: 0x06000781 RID: 1921 RVA: 0x00019B39 File Offset: 0x00017D39
		[NullableContext(2)]
		public bool TryGetNode(AnchorName anchor, [NotNullWhen(true)] out YamlNode node)
		{
			return this.anchors.TryGetValue(anchor, ref node);
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00019B48 File Offset: 0x00017D48
		public void AddNodeWithUnresolvedAliases(YamlNode node)
		{
			this.nodesWithUnresolvedAliases.Add(node);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x00019B58 File Offset: 0x00017D58
		public void ResolveAliases()
		{
			foreach (YamlNode yamlNode in this.nodesWithUnresolvedAliases)
			{
				yamlNode.ResolveAliases(this);
			}
		}

		// Token: 0x04000337 RID: 823
		private readonly IDictionary<AnchorName, YamlNode> anchors = new Dictionary<AnchorName, YamlNode>();

		// Token: 0x04000338 RID: 824
		private readonly IList<YamlNode> nodesWithUnresolvedAliases = new List<YamlNode>();
	}
}
