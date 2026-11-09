using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Helpers;
using YamlDotNet.Serialization;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x02000182 RID: 386
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlMappingNode : YamlNode, IEnumerable<KeyValuePair<YamlNode, YamlNode>>, IEnumerable, IYamlConvertible
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x0001A30C File Offset: 0x0001850C
		public IOrderedDictionary<YamlNode, YamlNode> Children
		{
			get
			{
				return this.children;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x0001A314 File Offset: 0x00018514
		// (set) Token: 0x060007B1 RID: 1969 RVA: 0x0001A31C File Offset: 0x0001851C
		public MappingStyle Style { get; set; }

		// Token: 0x060007B2 RID: 1970 RVA: 0x0001A325 File Offset: 0x00018525
		internal YamlMappingNode(IParser parser, DocumentLoadingState state)
		{
			this.Load(parser, state);
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0001A340 File Offset: 0x00018540
		private void Load(IParser parser, DocumentLoadingState state)
		{
			MappingStart mappingStart = parser.Consume<MappingStart>();
			base.Load(mappingStart, state);
			this.Style = mappingStart.Style;
			bool flag = false;
			MappingEnd mappingEnd;
			while (!parser.TryConsume(out mappingEnd))
			{
				YamlNode yamlNode = YamlNode.ParseNode(parser, state);
				YamlNode yamlNode2 = YamlNode.ParseNode(parser, state);
				try
				{
					this.children.Add(yamlNode, yamlNode2);
				}
				catch (ArgumentException innerException)
				{
					Mark start = yamlNode.Start;
					Mark end = yamlNode.End;
					throw new YamlException(ref start, ref end, "Duplicate key", innerException);
				}
				flag |= (yamlNode is YamlAliasNode || yamlNode2 is YamlAliasNode);
			}
			if (flag)
			{
				state.AddNodeWithUnresolvedAliases(this);
			}
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0001A3EC File Offset: 0x000185EC
		public YamlMappingNode()
		{
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0001A3FF File Offset: 0x000185FF
		public YamlMappingNode([Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] params KeyValuePair<YamlNode, YamlNode>[] children) : this(children)
		{
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0001A408 File Offset: 0x00018608
		public YamlMappingNode([Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] IEnumerable<KeyValuePair<YamlNode, YamlNode>> children)
		{
			foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair in children)
			{
				this.children.Add(keyValuePair);
			}
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0001A468 File Offset: 0x00018668
		public YamlMappingNode(params YamlNode[] children) : this(children)
		{
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0001A474 File Offset: 0x00018674
		public YamlMappingNode(IEnumerable<YamlNode> children)
		{
			foreach (YamlNode key in children)
			{
				IEnumerator<YamlNode> enumerator;
				if (!enumerator.MoveNext())
				{
					throw new ArgumentException("When constructing a mapping node with a sequence, the number of elements of the sequence must be even.");
				}
				this.Add(key, enumerator.Current);
			}
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0001A4E8 File Offset: 0x000186E8
		public void Add(YamlNode key, YamlNode value)
		{
			this.children.Add(key, value);
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0001A4F7 File Offset: 0x000186F7
		public void Add(string key, YamlNode value)
		{
			this.children.Add(new YamlScalarNode(key), value);
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0001A50B File Offset: 0x0001870B
		public void Add(YamlNode key, string value)
		{
			this.children.Add(key, new YamlScalarNode(value));
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0001A51F File Offset: 0x0001871F
		public void Add(string key, string value)
		{
			this.children.Add(new YamlScalarNode(key), new YamlScalarNode(value));
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x0001A538 File Offset: 0x00018738
		internal override void ResolveAliases(DocumentLoadingState state)
		{
			Dictionary<YamlNode, YamlNode> dictionary = null;
			Dictionary<YamlNode, YamlNode> dictionary2 = null;
			foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair in this.children)
			{
				if (keyValuePair.Key is YamlAliasNode)
				{
					if (dictionary == null)
					{
						dictionary = new Dictionary<YamlNode, YamlNode>();
					}
					dictionary.Add(keyValuePair.Key, state.GetNode(keyValuePair.Key.Anchor, keyValuePair.Key.Start, keyValuePair.Key.End));
				}
				if (keyValuePair.Value is YamlAliasNode)
				{
					if (dictionary2 == null)
					{
						dictionary2 = new Dictionary<YamlNode, YamlNode>();
					}
					dictionary2.Add(keyValuePair.Key, state.GetNode(keyValuePair.Value.Anchor, keyValuePair.Value.Start, keyValuePair.Value.End));
				}
			}
			if (dictionary2 != null)
			{
				foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair2 in dictionary2)
				{
					this.children[keyValuePair2.Key] = keyValuePair2.Value;
				}
			}
			if (dictionary != null)
			{
				foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair3 in dictionary)
				{
					YamlNode yamlNode = this.children[keyValuePair3.Key];
					this.children.Remove(keyValuePair3.Key);
					this.children.Add(keyValuePair3.Value, yamlNode);
				}
			}
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0001A6F0 File Offset: 0x000188F0
		internal override void Emit(IEmitter emitter, EmitterState state)
		{
			emitter.Emit(new MappingStart(base.Anchor, base.Tag, true, this.Style));
			foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair in this.children)
			{
				keyValuePair.Key.Save(emitter, state);
				keyValuePair.Value.Save(emitter, state);
			}
			emitter.Emit(new MappingEnd());
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0001A77C File Offset: 0x0001897C
		public override void Accept(IYamlVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0001A788 File Offset: 0x00018988
		[NullableContext(2)]
		public override bool Equals(object obj)
		{
			YamlMappingNode yamlMappingNode = obj as YamlMappingNode;
			if (yamlMappingNode == null || !object.Equals(base.Tag, yamlMappingNode.Tag) || this.children.Count != yamlMappingNode.children.Count)
			{
				return false;
			}
			foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair in this.children)
			{
				YamlNode yamlNode;
				if (!yamlMappingNode.children.TryGetValue(keyValuePair.Key, ref yamlNode) || !object.Equals(keyValuePair.Value, yamlNode))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0001A844 File Offset: 0x00018A44
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair in this.children)
			{
				num = HashCode.CombineHashCodes(num, keyValuePair.Key);
				num = HashCode.CombineHashCodes(num, keyValuePair.Value);
			}
			return num;
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x0001A8B0 File Offset: 0x00018AB0
		internal override IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level)
		{
			level.Increment();
			yield return this;
			foreach (KeyValuePair<YamlNode, YamlNode> child in this.children)
			{
				foreach (YamlNode yamlNode in child.Key.SafeAllNodes(level))
				{
					yield return yamlNode;
				}
				IEnumerator<YamlNode> enumerator2 = null;
				foreach (YamlNode yamlNode2 in child.Value.SafeAllNodes(level))
				{
					yield return yamlNode2;
				}
				enumerator2 = null;
				child = default(KeyValuePair<YamlNode, YamlNode>);
			}
			IEnumerator<KeyValuePair<YamlNode, YamlNode>> enumerator = null;
			level.Decrement();
			yield break;
			yield break;
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x0000206C File Offset: 0x0000026C
		public override YamlNodeType NodeType
		{
			get
			{
				return YamlNodeType.Mapping;
			}
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0001A8C8 File Offset: 0x00018AC8
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
				builder.Append("{ ");
				foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair in this.children)
				{
					if (builder.Length > 2)
					{
						builder.Append(", ");
					}
					builder.Append("{ ").Append(keyValuePair.Key.ToString(level)).Append(", ").Append(keyValuePair.Value.ToString(level)).Append(" }");
				}
				builder.Append(" }");
				level.Decrement();
				result = builder.ToString();
			}
			return result;
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x0001A9C8 File Offset: 0x00018BC8
		[return: Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		public IEnumerator<KeyValuePair<YamlNode, YamlNode>> GetEnumerator()
		{
			return this.children.GetEnumerator();
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x0001A9D5 File Offset: 0x00018BD5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x0001A9DD File Offset: 0x00018BDD
		void IYamlConvertible.Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			this.Load(parser, new DocumentLoadingState());
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0001A9EB File Offset: 0x00018BEB
		void IYamlConvertible.Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			this.Emit(emitter, new EmitterState());
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0001A9FC File Offset: 0x00018BFC
		public static YamlMappingNode FromObject(object mapping)
		{
			if (mapping == null)
			{
				throw new ArgumentNullException("mapping");
			}
			YamlMappingNode yamlMappingNode = new YamlMappingNode();
			foreach (PropertyInfo propertyInfo in mapping.GetType().GetPublicProperties())
			{
				if (propertyInfo.CanRead && propertyInfo.GetGetMethod(false).GetParameters().Length == 0)
				{
					object value = propertyInfo.GetValue(mapping, null);
					YamlNode yamlNode = value as YamlNode;
					if (yamlNode == null)
					{
						yamlNode = (Convert.ToString(value) ?? string.Empty);
					}
					yamlMappingNode.Add(propertyInfo.Name, yamlNode);
				}
			}
			return yamlMappingNode;
		}

		// Token: 0x04000342 RID: 834
		private readonly OrderedDictionary<YamlNode, YamlNode> children = new OrderedDictionary<YamlNode, YamlNode>();
	}
}
