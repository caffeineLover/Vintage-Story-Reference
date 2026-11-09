using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x020000AB RID: 171
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class YamlNode
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060003AE RID: 942 RVA: 0x0000D530 File Offset: 0x0000B730
		// (set) Token: 0x060003AF RID: 943 RVA: 0x0000D538 File Offset: 0x0000B738
		public AnchorName Anchor { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x0000D541 File Offset: 0x0000B741
		// (set) Token: 0x060003B1 RID: 945 RVA: 0x0000D549 File Offset: 0x0000B749
		public TagName Tag { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x0000D552 File Offset: 0x0000B752
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x0000D55A File Offset: 0x0000B75A
		public Mark Start { get; private set; } = Mark.Empty;

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0000D563 File Offset: 0x0000B763
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x0000D56B File Offset: 0x0000B76B
		public Mark End { get; private set; } = Mark.Empty;

		// Token: 0x060003B6 RID: 950 RVA: 0x0000D574 File Offset: 0x0000B774
		internal void Load(NodeEvent yamlEvent, DocumentLoadingState state)
		{
			this.Tag = yamlEvent.Tag;
			if (!yamlEvent.Anchor.IsEmpty)
			{
				this.Anchor = yamlEvent.Anchor;
				state.AddAnchor(this);
			}
			this.Start = yamlEvent.Start;
			this.End = yamlEvent.End;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000D5C8 File Offset: 0x0000B7C8
		internal static YamlNode ParseNode(IParser parser, DocumentLoadingState state)
		{
			Scalar scalar;
			if (parser.Accept(out scalar))
			{
				return new YamlScalarNode(parser, state);
			}
			SequenceStart sequenceStart;
			if (parser.Accept(out sequenceStart))
			{
				return new YamlSequenceNode(parser, state);
			}
			MappingStart mappingStart;
			if (parser.Accept(out mappingStart))
			{
				return new YamlMappingNode(parser, state);
			}
			AnchorAlias anchorAlias;
			if (!parser.TryConsume(out anchorAlias))
			{
				throw new ArgumentException("The current event is of an unsupported type.", "parser");
			}
			YamlNode result;
			if (!state.TryGetNode(anchorAlias.Value, out result))
			{
				return new YamlAliasNode(anchorAlias.Value);
			}
			return result;
		}

		// Token: 0x060003B8 RID: 952
		internal abstract void ResolveAliases(DocumentLoadingState state);

		// Token: 0x060003B9 RID: 953 RVA: 0x0000D644 File Offset: 0x0000B844
		internal void Save(IEmitter emitter, EmitterState state)
		{
			if (!this.Anchor.IsEmpty && !state.EmittedAnchors.Add(this.Anchor))
			{
				emitter.Emit(new AnchorAlias(this.Anchor));
				return;
			}
			this.Emit(emitter, state);
		}

		// Token: 0x060003BA RID: 954
		internal abstract void Emit(IEmitter emitter, EmitterState state);

		// Token: 0x060003BB RID: 955
		public abstract void Accept(IYamlVisitor visitor);

		// Token: 0x060003BC RID: 956 RVA: 0x0000D690 File Offset: 0x0000B890
		public override string ToString()
		{
			RecursionLevel level = new RecursionLevel(1000);
			return this.ToString(level);
		}

		// Token: 0x060003BD RID: 957
		internal abstract string ToString(RecursionLevel level);

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060003BE RID: 958 RVA: 0x0000D6B0 File Offset: 0x0000B8B0
		public IEnumerable<YamlNode> AllNodes
		{
			get
			{
				RecursionLevel level = new RecursionLevel(1000);
				return this.SafeAllNodes(level);
			}
		}

		// Token: 0x060003BF RID: 959
		internal abstract IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level);

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060003C0 RID: 960
		public abstract YamlNodeType NodeType { get; }

		// Token: 0x060003C1 RID: 961 RVA: 0x0000D6CF File Offset: 0x0000B8CF
		public static implicit operator YamlNode(string value)
		{
			return new YamlScalarNode(value);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000D6D7 File Offset: 0x0000B8D7
		public static implicit operator YamlNode(string[] sequence)
		{
			return new YamlSequenceNode(Enumerable.Select<string, YamlNode>(sequence, (string i) => i));
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000D704 File Offset: 0x0000B904
		[return: Nullable(2)]
		public static explicit operator string(YamlNode node)
		{
			YamlScalarNode yamlScalarNode = node as YamlScalarNode;
			if (yamlScalarNode == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(79, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to convert a '");
				defaultInterpolatedStringHandler.AppendFormatted<YamlNodeType>(node.NodeType);
				defaultInterpolatedStringHandler.AppendLiteral("' to string. This conversion is valid only for Scalars.");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return yamlScalarNode.Value;
		}

		// Token: 0x17000055 RID: 85
		public YamlNode this[int index]
		{
			get
			{
				YamlSequenceNode yamlSequenceNode = this as YamlSequenceNode;
				if (yamlSequenceNode == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(77, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Accessed '");
					defaultInterpolatedStringHandler.AppendFormatted<YamlNodeType>(this.NodeType);
					defaultInterpolatedStringHandler.AppendLiteral("' with an invalid index: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(index);
					defaultInterpolatedStringHandler.AppendLiteral(". Only Sequences can be indexed by number.");
					throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				return yamlSequenceNode.Children[index];
			}
		}

		// Token: 0x17000056 RID: 86
		public YamlNode this[YamlNode key]
		{
			get
			{
				YamlMappingNode yamlMappingNode = this as YamlMappingNode;
				if (yamlMappingNode == null)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(73, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Accessed '");
					defaultInterpolatedStringHandler.AppendFormatted<YamlNodeType>(this.NodeType);
					defaultInterpolatedStringHandler.AppendLiteral("' with an invalid index: ");
					defaultInterpolatedStringHandler.AppendFormatted<YamlNode>(key);
					defaultInterpolatedStringHandler.AppendLiteral(". Only Mappings can be indexed by key.");
					throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				return yamlMappingNode.Children[key];
			}
		}

		// Token: 0x04000106 RID: 262
		private const int MaximumRecursionLevel = 1000;

		// Token: 0x04000107 RID: 263
		internal const string MaximumRecursionLevelReachedToStringValue = "WARNING! INFINITE RECURSION!";
	}
}
