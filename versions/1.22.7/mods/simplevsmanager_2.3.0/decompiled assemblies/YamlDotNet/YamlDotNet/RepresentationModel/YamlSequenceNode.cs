using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Core.ObjectPool;
using YamlDotNet.Serialization;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x020000AF RID: 175
	[NullableContext(1)]
	[Nullable(0)]
	[DebuggerDisplay("Count = {children.Count}")]
	public sealed class YamlSequenceNode : YamlNode, IEnumerable<YamlNode>, IEnumerable, IYamlConvertible
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060003DD RID: 989 RVA: 0x0000DB0C File Offset: 0x0000BD0C
		public IList<YamlNode> Children
		{
			get
			{
				return this.children;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0000DB14 File Offset: 0x0000BD14
		// (set) Token: 0x060003DF RID: 991 RVA: 0x0000DB1C File Offset: 0x0000BD1C
		public SequenceStyle Style { get; set; }

		// Token: 0x060003E0 RID: 992 RVA: 0x0000DB25 File Offset: 0x0000BD25
		internal YamlSequenceNode(IParser parser, DocumentLoadingState state)
		{
			this.Load(parser, state);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000DB40 File Offset: 0x0000BD40
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

		// Token: 0x060003E2 RID: 994 RVA: 0x0000DBA0 File Offset: 0x0000BDA0
		public YamlSequenceNode()
		{
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000DBB3 File Offset: 0x0000BDB3
		public YamlSequenceNode(params YamlNode[] children) : this(children)
		{
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000DBBC File Offset: 0x0000BDBC
		public YamlSequenceNode(IEnumerable<YamlNode> children)
		{
			foreach (YamlNode yamlNode in children)
			{
				this.children.Add(yamlNode);
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000DC1C File Offset: 0x0000BE1C
		public void Add(YamlNode child)
		{
			this.children.Add(child);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000DC2A File Offset: 0x0000BE2A
		public void Add(string child)
		{
			this.children.Add(new YamlScalarNode(child));
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000DC40 File Offset: 0x0000BE40
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

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000DCBC File Offset: 0x0000BEBC
		internal override void Emit(IEmitter emitter, EmitterState state)
		{
			emitter.Emit(new SequenceStart(base.Anchor, base.Tag, base.Tag.IsEmpty, this.Style));
			foreach (YamlNode yamlNode in this.children)
			{
				yamlNode.Save(emitter, state);
			}
			emitter.Emit(new SequenceEnd());
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0000DD48 File Offset: 0x0000BF48
		public override void Accept(IYamlVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x0000DD54 File Offset: 0x0000BF54
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

		// Token: 0x060003EB RID: 1003 RVA: 0x0000DDE4 File Offset: 0x0000BFE4
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

		// Token: 0x060003EC RID: 1004 RVA: 0x0000DE50 File Offset: 0x0000C050
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
			List<YamlNode>.Enumerator enumerator = default(List<YamlNode>.Enumerator);
			level.Decrement();
			yield break;
			yield break;
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x0000DE67 File Offset: 0x0000C067
		public override YamlNodeType NodeType
		{
			get
			{
				return YamlNodeType.Sequence;
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0000DE6C File Offset: 0x0000C06C
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

		// Token: 0x060003EF RID: 1007 RVA: 0x0000DF3C File Offset: 0x0000C13C
		public IEnumerator<YamlNode> GetEnumerator()
		{
			return this.Children.GetEnumerator();
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0000DF49 File Offset: 0x0000C149
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0000DF51 File Offset: 0x0000C151
		void IYamlConvertible.Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			this.Load(parser, new DocumentLoadingState());
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0000DF5F File Offset: 0x0000C15F
		void IYamlConvertible.Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			this.Emit(emitter, new EmitterState());
		}

		// Token: 0x04000114 RID: 276
		private readonly List<YamlNode> children = new List<YamlNode>();
	}
}
