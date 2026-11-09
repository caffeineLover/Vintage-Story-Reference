using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x0200017E RID: 382
	[NullableContext(1)]
	[Nullable(0)]
	internal class YamlAliasNode : YamlNode
	{
		// Token: 0x0600078F RID: 1935 RVA: 0x00019F1B File Offset: 0x0001811B
		internal YamlAliasNode(AnchorName anchor)
		{
			base.Anchor = anchor;
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00019F2A File Offset: 0x0001812A
		internal override void ResolveAliases(DocumentLoadingState state)
		{
			throw new NotSupportedException("Resolving an alias on an alias node does not make sense");
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00019F36 File Offset: 0x00018136
		internal override void Emit(IEmitter emitter, EmitterState state)
		{
			throw new NotSupportedException("A YamlAliasNode is an implementation detail and should never be saved.");
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00019F42 File Offset: 0x00018142
		public override void Accept(IYamlVisitor visitor)
		{
			throw new NotSupportedException("A YamlAliasNode is an implementation detail and should never be visited.");
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00019F50 File Offset: 0x00018150
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			YamlAliasNode yamlAliasNode = obj as YamlAliasNode;
			return yamlAliasNode != null && this.Equals(yamlAliasNode) && object.Equals(base.Anchor, yamlAliasNode.Anchor);
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00019F8D File Offset: 0x0001818D
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00019F98 File Offset: 0x00018198
		internal override string ToString(RecursionLevel level)
		{
			return "*" + base.Anchor.ToString();
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00019FC3 File Offset: 0x000181C3
		internal override IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level)
		{
			yield return this;
			yield break;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x0000206F File Offset: 0x0000026F
		public override YamlNodeType NodeType
		{
			get
			{
				return YamlNodeType.Alias;
			}
		}
	}
}
