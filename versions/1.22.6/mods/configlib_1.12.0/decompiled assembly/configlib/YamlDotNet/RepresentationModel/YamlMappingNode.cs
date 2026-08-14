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
	// Token: 0x02000186 RID: 390
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlMappingNode : YamlNode, IEnumerable<KeyValuePair<YamlNode, YamlNode>>, IEnumerable, IYamlConvertible
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x0001AC54 File Offset: 0x00018E54
		public IOrderedDictionary<YamlNode, YamlNode> Children
		{
			get
			{
				return this.children;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x0001AC5C File Offset: 0x00018E5C
		// (set) Token: 0x060007ED RID: 2029 RVA: 0x0001AC64 File Offset: 0x00018E64
		public MappingStyle Style { get; set; }

		// Token: 0x060007EE RID: 2030 RVA: 0x0001AC6D File Offset: 0x00018E6D
		internal YamlMappingNode(IParser parser, DocumentLoadingState state)
		{
			this.Load(parser, state);
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x0001AC88 File Offset: 0x00018E88
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

		// Token: 0x060007F0 RID: 2032 RVA: 0x0001AD34 File Offset: 0x00018F34
		public YamlMappingNode()
		{
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x0001AD47 File Offset: 0x00018F47
		public YamlMappingNode([Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] params KeyValuePair<YamlNode, YamlNode>[] children) : this(children)
		{
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x0001AD50 File Offset: 0x00018F50
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

		// Token: 0x060007F3 RID: 2035 RVA: 0x0001ADB0 File Offset: 0x00018FB0
		public YamlMappingNode(params YamlNode[] children) : this(children)
		{
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0001ADBC File Offset: 0x00018FBC
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

		// Token: 0x060007F5 RID: 2037 RVA: 0x0001AE30 File Offset: 0x00019030
		public void Add(YamlNode key, YamlNode value)
		{
			this.children.Add(key, value);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x0001AE3F File Offset: 0x0001903F
		public void Add(string key, YamlNode value)
		{
			this.children.Add(new YamlScalarNode(key), value);
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0001AE53 File Offset: 0x00019053
		public void Add(YamlNode key, string value)
		{
			this.children.Add(key, new YamlScalarNode(value));
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0001AE67 File Offset: 0x00019067
		public void Add(string key, string value)
		{
			this.children.Add(new YamlScalarNode(key), new YamlScalarNode(value));
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0001AE80 File Offset: 0x00019080
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

		// Token: 0x060007FA RID: 2042 RVA: 0x0001B038 File Offset: 0x00019238
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

		// Token: 0x060007FB RID: 2043 RVA: 0x0001B0C4 File Offset: 0x000192C4
		public override void Accept(IYamlVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0001B0D0 File Offset: 0x000192D0
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

		// Token: 0x060007FD RID: 2045 RVA: 0x0001B18C File Offset: 0x0001938C
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

		// Token: 0x060007FE RID: 2046 RVA: 0x0001B1F8 File Offset: 0x000193F8
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
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x0000D1E2 File Offset: 0x0000B3E2
		public override YamlNodeType NodeType
		{
			get
			{
				return YamlNodeType.Mapping;
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0001B210 File Offset: 0x00019410
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

		// Token: 0x06000801 RID: 2049 RVA: 0x0001B310 File Offset: 0x00019510
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

		// Token: 0x06000802 RID: 2050 RVA: 0x0001B31D File Offset: 0x0001951D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x0001B325 File Offset: 0x00019525
		void IYamlConvertible.Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			this.Load(parser, new DocumentLoadingState());
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0001B333 File Offset: 0x00019533
		void IYamlConvertible.Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			this.Emit(emitter, new EmitterState());
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x0001B344 File Offset: 0x00019544
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

		// Token: 0x04000370 RID: 880
		private readonly OrderedDictionary<YamlNode, YamlNode> children = new OrderedDictionary<YamlNode, YamlNode>();
	}
}
