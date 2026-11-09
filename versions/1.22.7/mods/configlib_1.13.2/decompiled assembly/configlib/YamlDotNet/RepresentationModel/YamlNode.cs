using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x02000184 RID: 388
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class YamlNode
	{
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x0001ADAF File Offset: 0x00018FAF
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x0001ADB7 File Offset: 0x00018FB7
		public AnchorName Anchor { get; set; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x0001ADC0 File Offset: 0x00018FC0
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x0001ADC8 File Offset: 0x00018FC8
		public TagName Tag { get; set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x0001ADD1 File Offset: 0x00018FD1
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x0001ADD9 File Offset: 0x00018FD9
		public Mark Start { get; private set; } = Mark.Empty;

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x0001ADE2 File Offset: 0x00018FE2
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x0001ADEA File Offset: 0x00018FEA
		public Mark End { get; private set; } = Mark.Empty;

		// Token: 0x060007DD RID: 2013 RVA: 0x0001ADF4 File Offset: 0x00018FF4
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

		// Token: 0x060007DE RID: 2014 RVA: 0x0001AE48 File Offset: 0x00019048
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

		// Token: 0x060007DF RID: 2015
		internal abstract void ResolveAliases(DocumentLoadingState state);

		// Token: 0x060007E0 RID: 2016 RVA: 0x0001AEC4 File Offset: 0x000190C4
		internal void Save(IEmitter emitter, EmitterState state)
		{
			if (!this.Anchor.IsEmpty && !state.EmittedAnchors.Add(this.Anchor))
			{
				emitter.Emit(new AnchorAlias(this.Anchor));
				return;
			}
			this.Emit(emitter, state);
		}

		// Token: 0x060007E1 RID: 2017
		internal abstract void Emit(IEmitter emitter, EmitterState state);

		// Token: 0x060007E2 RID: 2018
		public abstract void Accept(IYamlVisitor visitor);

		// Token: 0x060007E3 RID: 2019 RVA: 0x0001AF10 File Offset: 0x00019110
		public override string ToString()
		{
			RecursionLevel level = new RecursionLevel(1000);
			return this.ToString(level);
		}

		// Token: 0x060007E4 RID: 2020
		internal abstract string ToString(RecursionLevel level);

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x0001AF30 File Offset: 0x00019130
		public IEnumerable<YamlNode> AllNodes
		{
			get
			{
				RecursionLevel level = new RecursionLevel(1000);
				return this.SafeAllNodes(level);
			}
		}

		// Token: 0x060007E6 RID: 2022
		internal abstract IEnumerable<YamlNode> SafeAllNodes(RecursionLevel level);

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060007E7 RID: 2023
		public abstract YamlNodeType NodeType { get; }

		// Token: 0x060007E8 RID: 2024 RVA: 0x0001AF4F File Offset: 0x0001914F
		public static implicit operator YamlNode(string value)
		{
			return new YamlScalarNode(value);
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0001AF57 File Offset: 0x00019157
		public static implicit operator YamlNode(string[] sequence)
		{
			return new YamlSequenceNode(Enumerable.Select<string, YamlNode>(sequence, (string i) => i));
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x0001AF84 File Offset: 0x00019184
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

		// Token: 0x0400034D RID: 845
		private const int MaximumRecursionLevel = 1000;

		// Token: 0x0400034E RID: 846
		internal const string MaximumRecursionLevelReachedToStringValue = "WARNING! INFINITE RECURSION!";
	}
}
