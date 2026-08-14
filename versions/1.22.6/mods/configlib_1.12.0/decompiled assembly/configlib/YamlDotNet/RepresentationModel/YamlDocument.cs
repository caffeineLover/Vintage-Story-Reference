using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x02000184 RID: 388
	[NullableContext(1)]
	[Nullable(0)]
	public class YamlDocument
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x0001A9CF File Offset: 0x00018BCF
		// (set) Token: 0x060007DD RID: 2013 RVA: 0x0001A9D7 File Offset: 0x00018BD7
		public YamlNode RootNode { get; private set; }

		// Token: 0x060007DE RID: 2014 RVA: 0x0001A9E0 File Offset: 0x00018BE0
		public YamlDocument(YamlNode rootNode)
		{
			this.RootNode = rootNode;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0001A9EF File Offset: 0x00018BEF
		public YamlDocument(string rootNode)
		{
			this.RootNode = new YamlScalarNode(rootNode);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0001AA04 File Offset: 0x00018C04
		internal YamlDocument(IParser parser)
		{
			DocumentLoadingState documentLoadingState = new DocumentLoadingState();
			parser.Consume<DocumentStart>();
			DocumentEnd documentEnd;
			while (!parser.TryConsume(out documentEnd))
			{
				this.RootNode = YamlNode.ParseNode(parser, documentLoadingState);
				if (this.RootNode is YamlAliasNode)
				{
					throw new YamlException("A document cannot contain only an alias");
				}
			}
			documentLoadingState.ResolveAliases();
			if (this.RootNode == null)
			{
				throw new ArgumentException("Atempted to parse an empty document");
			}
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0001AA6E File Offset: 0x00018C6E
		private void AssignAnchors()
		{
			new YamlDocument.AnchorAssigningVisitor().AssignAnchors(this);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0001AA7B File Offset: 0x00018C7B
		internal void Save(IEmitter emitter, bool assignAnchors = true)
		{
			if (assignAnchors)
			{
				this.AssignAnchors();
			}
			emitter.Emit(new DocumentStart());
			this.RootNode.Save(emitter, new EmitterState());
			emitter.Emit(new DocumentEnd(false));
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0001AAAE File Offset: 0x00018CAE
		public void Accept(IYamlVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x0001AAB7 File Offset: 0x00018CB7
		public IEnumerable<YamlNode> AllNodes
		{
			get
			{
				return this.RootNode.AllNodes;
			}
		}

		// Token: 0x02000185 RID: 389
		[Nullable(0)]
		private class AnchorAssigningVisitor : YamlVisitorBase
		{
			// Token: 0x060007E5 RID: 2021 RVA: 0x0001AAC4 File Offset: 0x00018CC4
			public void AssignAnchors(YamlDocument document)
			{
				this.existingAnchors.Clear();
				this.visitedNodes.Clear();
				document.Accept(this);
				Random random = new Random();
				foreach (KeyValuePair<YamlNode, bool> keyValuePair in this.visitedNodes)
				{
					if (keyValuePair.Value)
					{
						AnchorName anchor;
						if (!keyValuePair.Key.Anchor.IsEmpty && !this.existingAnchors.Contains(keyValuePair.Key.Anchor))
						{
							anchor = keyValuePair.Key.Anchor;
						}
						else
						{
							do
							{
								anchor = new AnchorName(random.Next().ToString(CultureInfo.InvariantCulture));
							}
							while (this.existingAnchors.Contains(anchor));
						}
						this.existingAnchors.Add(anchor);
						keyValuePair.Key.Anchor = anchor;
					}
				}
			}

			// Token: 0x060007E6 RID: 2022 RVA: 0x0001ABC8 File Offset: 0x00018DC8
			private bool VisitNodeAndFindDuplicates(YamlNode node)
			{
				bool flag;
				if (this.visitedNodes.TryGetValue(node, ref flag))
				{
					if (!flag)
					{
						this.visitedNodes[node] = true;
					}
					return !flag;
				}
				this.visitedNodes.Add(node, false);
				return false;
			}

			// Token: 0x060007E7 RID: 2023 RVA: 0x0001AC08 File Offset: 0x00018E08
			public override void Visit(YamlScalarNode scalar)
			{
				this.VisitNodeAndFindDuplicates(scalar);
			}

			// Token: 0x060007E8 RID: 2024 RVA: 0x0001AC12 File Offset: 0x00018E12
			public override void Visit(YamlMappingNode mapping)
			{
				if (!this.VisitNodeAndFindDuplicates(mapping))
				{
					base.Visit(mapping);
				}
			}

			// Token: 0x060007E9 RID: 2025 RVA: 0x0001AC24 File Offset: 0x00018E24
			public override void Visit(YamlSequenceNode sequence)
			{
				if (!this.VisitNodeAndFindDuplicates(sequence))
				{
					base.Visit(sequence);
				}
			}

			// Token: 0x0400036E RID: 878
			private readonly HashSet<AnchorName> existingAnchors = new HashSet<AnchorName>();

			// Token: 0x0400036F RID: 879
			private readonly Dictionary<YamlNode, bool> visitedNodes = new Dictionary<YamlNode, bool>();
		}
	}
}
