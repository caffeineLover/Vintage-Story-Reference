using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Core.ObjectPool;
using YamlDotNet.Helpers;
using YamlDotNet.Serialization;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x020000AA RID: 170
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class YamlMappingNode : YamlNode, IEnumerable<KeyValuePair<YamlNode, YamlNode>>, IEnumerable, IYamlConvertible
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000393 RID: 915 RVA: 0x0000CD3F File Offset: 0x0000AF3F
		public IOrderedDictionary<YamlNode, YamlNode> Children
		{
			get
			{
				return this.children;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000394 RID: 916 RVA: 0x0000CD47 File Offset: 0x0000AF47
		// (set) Token: 0x06000395 RID: 917 RVA: 0x0000CD4F File Offset: 0x0000AF4F
		public MappingStyle Style { get; set; }

		// Token: 0x06000396 RID: 918 RVA: 0x0000CD58 File Offset: 0x0000AF58
		internal YamlMappingNode(IParser parser, DocumentLoadingState state)
		{
			this.Load(parser, state);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000CD74 File Offset: 0x0000AF74
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
				if (!this.children.TryAdd(yamlNode, yamlNode2))
				{
					Mark start = yamlNode.Start;
					Mark end = yamlNode.End;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(14, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Duplicate key ");
					defaultInterpolatedStringHandler.AppendFormatted<YamlNode>(yamlNode);
					throw new YamlException(ref start, ref end, defaultInterpolatedStringHandler.ToStringAndClear());
				}
				flag |= (yamlNode is YamlAliasNode || yamlNode2 is YamlAliasNode);
			}
			if (flag)
			{
				state.AddNodeWithUnresolvedAliases(this);
			}
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000CE29 File Offset: 0x0000B029
		public YamlMappingNode()
		{
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000CE3C File Offset: 0x0000B03C
		public YamlMappingNode([Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] params KeyValuePair<YamlNode, YamlNode>[] children) : this(children)
		{
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000CE48 File Offset: 0x0000B048
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

		// Token: 0x0600039B RID: 923 RVA: 0x0000CEA8 File Offset: 0x0000B0A8
		public YamlMappingNode(params YamlNode[] children) : this(children)
		{
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000CEB4 File Offset: 0x0000B0B4
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

		// Token: 0x0600039D RID: 925 RVA: 0x0000CF28 File Offset: 0x0000B128
		public void Add(YamlNode key, YamlNode value)
		{
			this.children.Add(key, value);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000CF37 File Offset: 0x0000B137
		public void Add(string key, YamlNode value)
		{
			this.children.Add(new YamlScalarNode(key), value);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000CF4B File Offset: 0x0000B14B
		public void Add(YamlNode key, string value)
		{
			this.children.Add(key, new YamlScalarNode(value));
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000CF5F File Offset: 0x0000B15F
		public void Add(string key, string value)
		{
			this.children.Add(new YamlScalarNode(key), new YamlScalarNode(value));
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000CF78 File Offset: 0x0000B178
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

		// Token: 0x060003A2 RID: 930 RVA: 0x0000D130 File Offset: 0x0000B330
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

		// Token: 0x060003A3 RID: 931 RVA: 0x0000D1BC File Offset: 0x0000B3BC
		public override void Accept(IYamlVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000D1C8 File Offset: 0x0000B3C8
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

		// Token: 0x060003A5 RID: 933 RVA: 0x0000D288 File Offset: 0x0000B488
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			foreach (KeyValuePair<YamlNode, YamlNode> keyValuePair in this.children)
			{
				num = HashCode.CombineHashCodes(num, keyValuePair.Key);
				num = (keyValuePair.Value.Anchor.IsEmpty ? HashCode.CombineHashCodes(num, keyValuePair.Value) : HashCode.CombineHashCodes(num, keyValuePair.Value.Anchor));
			}
			return num;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000D324 File Offset: 0x0000B524
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

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0000D33B File Offset: 0x0000B53B
		public override YamlNodeType NodeType
		{
			get
			{
				return YamlNodeType.Mapping;
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000D340 File Offset: 0x0000B540
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

		// Token: 0x060003A9 RID: 937 RVA: 0x0000D440 File Offset: 0x0000B640
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

		// Token: 0x060003AA RID: 938 RVA: 0x0000D44D File Offset: 0x0000B64D
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000D455 File Offset: 0x0000B655
		void IYamlConvertible.Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
		{
			this.Load(parser, new DocumentLoadingState());
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000D463 File Offset: 0x0000B663
		void IYamlConvertible.Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
		{
			this.Emit(emitter, new EmitterState());
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000D474 File Offset: 0x0000B674
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
						string text = Convert.ToString(value, CultureInfo.InvariantCulture);
						yamlNode = (text ?? string.Empty);
					}
					yamlMappingNode.Add(propertyInfo.Name, yamlNode);
				}
			}
			return yamlMappingNode;
		}

		// Token: 0x04000104 RID: 260
		private readonly OrderedDictionary<YamlNode, YamlNode> children = new OrderedDictionary<YamlNode, YamlNode>();
	}
}
