using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x020000A8 RID: 168
	[NullableContext(1)]
	[Nullable(0)]
	internal class YamlAliasNode : YamlNode
	{
		// Token: 0x06000381 RID: 897 RVA: 0x0000CB7F File Offset: 0x0000AD7F
		internal YamlAliasNode(AnchorName anchor)
		{
			base.Anchor = anchor;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000CB8E File Offset: 0x0000AD8E
		internal override void ResolveAliases(DocumentLoadingState state)
		{
			throw new NotSupportedException("Resolving an alias on an alias node does not make sense");
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000CB9A File Offset: 0x0000AD9A
		internal override void Emit(IEmitter emitter, EmitterState state)
		{
			throw new NotSupportedException("A YamlAliasNode is an implementation detail and should never be saved.");
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000CBA6 File Offset: 0x0000ADA6
		public override void Accept(IYamlVisitor visitor)
		{
			throw new NotSupportedException("A YamlAliasNode is an implementation detail and should never be visited.");
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000CBB4 File Offset: 0x0000ADB4
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			YamlAliasNode yamlAliasNode = obj as YamlAliasNode;
			return yamlAliasNode != null && this.Equals(yamlAliasNode) && object.Equals(base.Anchor, yamlAliasNode.Anchor);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000CBF1 File Offset: 0x0000ADF1
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000CBFC File Offset: 0x0000ADFC
		internal override string ToString(RecursionLevel level)
		{
			return "*" + base.Anchor.ToString();
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000CC27 File Offset: 0x0000AE27
		internal override IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level)
		{
			yield return this;
			yield break;
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000389 RID: 905 RVA: 0x0000CC37 File Offset: 0x0000AE37
		public override YamlNodeType NodeType
		{
			get
			{
				return YamlNodeType.Alias;
			}
		}
	}
}
