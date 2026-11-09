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
	// Token: 0x0200018A RID: 394
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerDisplay("Count = {children.Count}")]
	public sealed class YamlSequenceNode : YamlNode, IEnumerable<YamlNode>, IEnumerable, IYamlConvertible
	{
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x0001B42F File Offset: 0x0001962F
		public IList<YamlNode> Children
		{
			get
			{
				return this.children;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x0001B437 File Offset: 0x00019637
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x0001B43F File Offset: 0x0001963F
		public SequenceStyle Style { get; set; }

		// Token: 0x06000812 RID: 2066 RVA: 0x0001B448 File Offset: 0x00019648
		internal YamlSequenceNode(IParser parser, DocumentLoadingState state)
		{
			this.Load(parser, state);
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0001B464 File Offset: 0x00019664
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

		// Token: 0x06000814 RID: 2068 RVA: 0x0001B4C4 File Offset: 0x000196C4
		public YamlSequenceNode()
		{
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0001B4D7 File Offset: 0x000196D7
		public YamlSequenceNode(params YamlNode[] children) : this(children)
		{
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0001B4E0 File Offset: 0x000196E0
		public YamlSequenceNode(IEnumerable<YamlNode> children)
		{
			foreach (YamlNode yamlNode in children)
			{
				this.children.Add(yamlNode);
			}
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0001B540 File Offset: 0x00019740
		public void Add(YamlNode child)
		{
			this.children.Add(child);
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0001B54E File Offset: 0x0001974E
		public void Add(string child)
		{
			this.children.Add(new YamlScalarNode(child));
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0001B564 File Offset: 0x00019764
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

		// Token: 0x0600081A RID: 2074 RVA: 0x0001B5E0 File Offset: 0x000197E0
		internal override void Emit(IEmitter emitter, EmitterState state)
		{
			emitter.Emit(new SequenceStart(base.Anchor, base.Tag, base.Tag.IsEmpty, this.Style));
			foreach (YamlNode yamlNode in this.children)
			{
				yamlNode.Save(emitter, state);
			}
			emitter.Emit(new SequenceEnd());
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0001B664 File Offset: 0x00019864
		public override void Accept(IYamlVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0001B670 File Offset: 0x00019870
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

		// Token: 0x0600081D RID: 2077 RVA: 0x0001B6FC File Offset: 0x000198FC
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

		// Token: 0x0600081E RID: 2078 RVA: 0x0001B760 File Offset: 0x00019960
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
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x0001B777 File Offset: 0x00019977
		public override YamlNodeType NodeType
		{
			get
			{
				return YamlNodeType.Sequence;
			}
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x0001B77C File Offset: 0x0001997C
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

		// Token: 0x06000821 RID: 2081 RVA: 0x0001B844 File Offset: 0x00019A44
		public IEnumerator<YamlNode> GetEnumerator()
		{
			return this.Children.GetEnumerator();
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0001B851 File Offset: 0x00019A51
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0001B859 File Offset: 0x00019A59
		void IYamlConvertible.Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			this.Load(parser, new DocumentLoadingState());
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0001A9EB File Offset: 0x00018BEB
		void IYamlConvertible.Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			this.Emit(emitter, new EmitterState());
		}

		// Token: 0x04000361 RID: 865
		private readonly IList<YamlNode> children = new List<YamlNode>();
	}
}
