using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x02000188 RID: 392
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class YamlNode
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x0001B6F7 File Offset: 0x000198F7
		// (set) Token: 0x06000812 RID: 2066 RVA: 0x0001B6FF File Offset: 0x000198FF
		public AnchorName Anchor { get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x0001B708 File Offset: 0x00019908
		// (set) Token: 0x06000814 RID: 2068 RVA: 0x0001B710 File Offset: 0x00019910
		public TagName Tag { get; set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x0001B719 File Offset: 0x00019919
		// (set) Token: 0x06000816 RID: 2070 RVA: 0x0001B721 File Offset: 0x00019921
		public Mark Start { get; private set; } = Mark.Empty;

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x0001B72A File Offset: 0x0001992A
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x0001B732 File Offset: 0x00019932
		public Mark End { get; private set; } = Mark.Empty;

		// Token: 0x06000819 RID: 2073 RVA: 0x0001B73C File Offset: 0x0001993C
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

		// Token: 0x0600081A RID: 2074 RVA: 0x0001B790 File Offset: 0x00019990
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

		// Token: 0x0600081B RID: 2075
		internal abstract void ResolveAliases(DocumentLoadingState state);

		// Token: 0x0600081C RID: 2076 RVA: 0x0001B80C File Offset: 0x00019A0C
		internal void Save(IEmitter emitter, EmitterState state)
		{
			if (!this.Anchor.IsEmpty && !state.EmittedAnchors.Add(this.Anchor))
			{
				emitter.Emit(new AnchorAlias(this.Anchor));
				return;
			}
			this.Emit(emitter, state);
		}

		// Token: 0x0600081D RID: 2077
		internal abstract void Emit(IEmitter emitter, EmitterState state);

		// Token: 0x0600081E RID: 2078
		public abstract void Accept(IYamlVisitor visitor);

		// Token: 0x0600081F RID: 2079 RVA: 0x0001B858 File Offset: 0x00019A58
		public override string ToString()
		{
			RecursionLevel level = new RecursionLevel(1000);
			return this.ToString(level);
		}

		// Token: 0x06000820 RID: 2080
		internal abstract string ToString(RecursionLevel level);

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x0001B878 File Offset: 0x00019A78
		public IEnumerable<YamlNode> AllNodes
		{
			get
			{
				RecursionLevel level = new RecursionLevel(1000);
				return this.SafeAllNodes(level);
			}
		}

		// Token: 0x06000822 RID: 2082
		internal abstract IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level);

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000823 RID: 2083
		public abstract YamlNodeType NodeType { get; }

		// Token: 0x06000824 RID: 2084 RVA: 0x0001B897 File Offset: 0x00019A97
		public static implicit operator YamlNode(string value)
		{
			return new YamlScalarNode(value);
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0001B89F File Offset: 0x00019A9F
		public static implicit operator YamlNode(string[] sequence)
		{
			return new YamlSequenceNode(Enumerable.Select<string, YamlNode>(sequence, (string i) => i));
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0001B8CC File Offset: 0x00019ACC
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

		// Token: 0x170000DB RID: 219
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

		// Token: 0x170000DC RID: 220
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

		// Token: 0x0400037B RID: 891
		private const int MaximumRecursionLevel = 1000;

		// Token: 0x0400037C RID: 892
		internal const string MaximumRecursionLevelReachedToStringValue = "WARNING! INFINITE RECURSION!";
	}
}
