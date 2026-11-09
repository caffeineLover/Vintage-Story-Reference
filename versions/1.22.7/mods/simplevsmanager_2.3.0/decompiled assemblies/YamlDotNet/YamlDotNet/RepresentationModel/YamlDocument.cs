using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x020000A9 RID: 169
	[NullableContext(1)]
	[Nullable(0)]
	public class YamlDocument
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600038A RID: 906 RVA: 0x0000CC3A File Offset: 0x0000AE3A
		// (set) Token: 0x0600038B RID: 907 RVA: 0x0000CC42 File Offset: 0x0000AE42
		public YamlNode RootNode { get; private set; }

		// Token: 0x0600038C RID: 908 RVA: 0x0000CC4B File Offset: 0x0000AE4B
		public YamlDocument(YamlNode rootNode)
		{
			this.RootNode = rootNode;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000CC5A File Offset: 0x0000AE5A
		public YamlDocument(string rootNode)
		{
			this.RootNode = new YamlScalarNode(rootNode);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000CC70 File Offset: 0x0000AE70
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

		// Token: 0x0600038F RID: 911 RVA: 0x0000CCDC File Offset: 0x0000AEDC
		private void AssignAnchors()
		{
			YamlDocument.AnchorAssigningVisitor anchorAssigningVisitor = new YamlDocument.AnchorAssigningVisitor();
			anchorAssigningVisitor.AssignAnchors(this);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000CCF6 File Offset: 0x0000AEF6
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

		// Token: 0x06000391 RID: 913 RVA: 0x0000CD29 File Offset: 0x0000AF29
		public void Accept(IYamlVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000392 RID: 914 RVA: 0x0000CD32 File Offset: 0x0000AF32
		public IEnumerable<YamlNode> AllNodes
		{
			get
			{
				return this.RootNode.AllNodes;
			}
		}

		// Token: 0x0200018C RID: 396
		[Nullable(0)]
		private class AnchorAssigningVisitor : YamlVisitorBase
		{
			// Token: 0x0600086C RID: 2156 RVA: 0x00019DDC File Offset: 0x00017FDC
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

			// Token: 0x0600086D RID: 2157 RVA: 0x00019EE0 File Offset: 0x000180E0
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

			// Token: 0x0600086E RID: 2158 RVA: 0x00019F20 File Offset: 0x00018120
			public override void Visit(YamlScalarNode scalar)
			{
				this.VisitNodeAndFindDuplicates(scalar);
			}

			// Token: 0x0600086F RID: 2159 RVA: 0x00019F2A File Offset: 0x0001812A
			public override void Visit(YamlMappingNode mapping)
			{
				if (!this.VisitNodeAndFindDuplicates(mapping))
				{
					base.Visit(mapping);
				}
			}

			// Token: 0x06000870 RID: 2160 RVA: 0x00019F3C File Offset: 0x0001813C
			public override void Visit(YamlSequenceNode sequence)
			{
				if (!this.VisitNodeAndFindDuplicates(sequence))
				{
					base.Visit(sequence);
				}
			}

			// Token: 0x04000324 RID: 804
			private readonly HashSet<AnchorName> existingAnchors = new HashSet<AnchorName>();

			// Token: 0x04000325 RID: 805
			private readonly Dictionary<YamlNode, bool> visitedNodes = new Dictionary<YamlNode, bool>();
		}
	}
}
