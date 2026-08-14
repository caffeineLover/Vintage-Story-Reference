using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x02000182 RID: 386
	[NullableContext(1)]
	[Nullable(0)]
	internal class YamlAliasNode : YamlNode
	{
		// Token: 0x060007CB RID: 1995 RVA: 0x0001A863 File Offset: 0x00018A63
		internal YamlAliasNode(AnchorName anchor)
		{
			base.Anchor = anchor;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0001A872 File Offset: 0x00018A72
		internal override void ResolveAliases(DocumentLoadingState state)
		{
			throw new NotSupportedException("Resolving an alias on an alias node does not make sense");
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0001A87E File Offset: 0x00018A7E
		internal override void Emit(IEmitter emitter, EmitterState state)
		{
			throw new NotSupportedException("A YamlAliasNode is an implementation detail and should never be saved.");
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0001A88A File Offset: 0x00018A8A
		public override void Accept(IYamlVisitor visitor)
		{
			throw new NotSupportedException("A YamlAliasNode is an implementation detail and should never be visited.");
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x0001A898 File Offset: 0x00018A98
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			YamlAliasNode yamlAliasNode = obj as YamlAliasNode;
			return yamlAliasNode != null && this.Equals(yamlAliasNode) && object.Equals(base.Anchor, yamlAliasNode.Anchor);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0001A8D5 File Offset: 0x00018AD5
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0001A8E0 File Offset: 0x00018AE0
		internal override string ToString(RecursionLevel level)
		{
			return "*" + base.Anchor.ToString();
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x0001A90B File Offset: 0x00018B0B
		internal override IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level)
		{
			yield return this;
			yield break;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x0000D44C File Offset: 0x0000B64C
		public override YamlNodeType NodeType
		{
			get
			{
				return YamlNodeType.Alias;
			}
		}
	}
}
