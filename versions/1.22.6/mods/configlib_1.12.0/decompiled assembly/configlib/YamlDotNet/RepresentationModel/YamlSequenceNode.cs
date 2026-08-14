using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Helpers;
using YamlDotNet.Serialization;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x0200018E RID: 398
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerDisplay("Count = {children.Count}")]
	public sealed class YamlSequenceNode : YamlNode, IEnumerable<YamlNode>, IEnumerable, IYamlConvertible
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x0001BD77 File Offset: 0x00019F77
		public IList<YamlNode> Children
		{
			get
			{
				return this.children;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x0001BD7F File Offset: 0x00019F7F
		// (set) Token: 0x0600084D RID: 2125 RVA: 0x0001BD87 File Offset: 0x00019F87
		public SequenceStyle Style { get; set; }

		// Token: 0x0600084E RID: 2126 RVA: 0x0001BD90 File Offset: 0x00019F90
		internal YamlSequenceNode(IParser parser, DocumentLoadingState state)
		{
			this.Load(parser, state);
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0001BDAC File Offset: 0x00019FAC
		private void Load(IParser parser, DocumentLoadingState state)
		{
			SequenceStart sequenceStart = parser.Consume<SequenceStart>();
			base.Load(sequenceStart, state);
			this.Style = sequenceStart.Style;
			bool flag = false;
			SequenceEnd sequenceEnd;
			while (!parser.TryConsume(out sequenceEnd))
			{
				YamlNode yamlNode = YamlNode.ParseNode(parser, state);
				this.children.Add(yamlNode);
				flag |= (yamlNode is YamlAliasNode);
			}
			if (flag)
			{
				state.AddNodeWithUnresolvedAliases(this);
			}
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x0001BE0C File Offset: 0x0001A00C
		public YamlSequenceNode()
		{
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x0001BE1F File Offset: 0x0001A01F
		public YamlSequenceNode(params YamlNode[] children) : this(children)
		{
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0001BE28 File Offset: 0x0001A028
		public YamlSequenceNode(IEnumerable<YamlNode> children)
		{
			foreach (YamlNode yamlNode in children)
			{
				this.children.Add(yamlNode);
			}
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0001BE88 File Offset: 0x0001A088
		public void Add(YamlNode child)
		{
			this.children.Add(child);
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x0001BE96 File Offset: 0x0001A096
		public void Add(string child)
		{
			this.children.Add(new YamlScalarNode(child));
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0001BEAC File Offset: 0x0001A0AC
		internal override void ResolveAliases(DocumentLoadingState state)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				if (this.children[i] is YamlAliasNode)
				{
					this.children[i] = state.GetNode(this.children[i].Anchor, this.children[i].Start, this.children[i].End);
				}
			}
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0001BF28 File Offset: 0x0001A128
		internal override void Emit(IEmitter emitter, EmitterState state)
		{
			emitter.Emit(new SequenceStart(base.Anchor, base.Tag, base.Tag.IsEmpty, this.Style));
			foreach (YamlNode yamlNode in this.children)
			{
				yamlNode.Save(emitter, state);
			}
			emitter.Emit(new SequenceEnd());
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0001BFAC File Offset: 0x0001A1AC
		public override void Accept(IYamlVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0001BFB8 File Offset: 0x0001A1B8
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			YamlSequenceNode yamlSequenceNode = obj as YamlSequenceNode;
			if (yamlSequenceNode == null || !object.Equals(base.Tag, yamlSequenceNode.Tag) || this.children.Count != yamlSequenceNode.children.Count)
			{
				return false;
			}
			for (int i = 0; i < this.children.Count; i++)
			{
				if (!object.Equals(this.children[i], yamlSequenceNode.children[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0001C044 File Offset: 0x0001A244
		public override int GetHashCode()
		{
			int num = 0;
			foreach (YamlNode o in this.children)
			{
				num = HashCode.CombineHashCodes(num, o);
			}
			num = HashCode.CombineHashCodes(num, base.Tag);
			return num;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x0001C0A8 File Offset: 0x0001A2A8
		internal override IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level)
		{
			level.Increment();
			yield return this;
			foreach (YamlNode yamlNode in this.children)
			{
				foreach (YamlNode yamlNode2 in yamlNode.SafeAllNodes(level))
				{
					yield return yamlNode2;
				}
				IEnumerator<YamlNode> enumerator2 = null;
			}
			IEnumerator<YamlNode> enumerator = null;
			level.Decrement();
			yield break;
			yield break;
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x0001C0BF File Offset: 0x0001A2BF
		public override YamlNodeType NodeType
		{
			get
			{
				return YamlNodeType.Sequence;
			}
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x0001C0C4 File Offset: 0x0001A2C4
		internal override string ToString(RecursionLevel level)
		{
			if (!level.TryIncrement())
			{
				return "WARNING! INFINITE RECURSION!";
			}
			string result;
			using (StringBuilderPool.BuilderWrapper builderWrapper = StringBuilderPool.Rent())
			{
				StringBuilder builder = builderWrapper.Builder;
				builder.Append("[ ");
				foreach (YamlNode yamlNode in this.children)
				{
					if (builder.Length > 2)
					{
						builder.Append(", ");
					}
					builder.Append(yamlNode.ToString(level));
				}
				builder.Append(" ]");
				level.Decrement();
				result = builder.ToString();
			}
			return result;
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x0001C18C File Offset: 0x0001A38C
		public IEnumerator<YamlNode> GetEnumerator()
		{
			return this.Children.GetEnumerator();
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x0001C199 File Offset: 0x0001A399
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x0001C1A1 File Offset: 0x0001A3A1
		void IYamlConvertible.Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			this.Load(parser, new DocumentLoadingState());
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0001B333 File Offset: 0x00019533
		void IYamlConvertible.Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			this.Emit(emitter, new EmitterState());
		}

		// Token: 0x0400038F RID: 911
		private readonly IList<YamlNode> children = new List<YamlNode>();
	}
}
