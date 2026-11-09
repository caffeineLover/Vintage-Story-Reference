using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel
{
	// Token: 0x02000180 RID: 384
	[NullableContext(1)]
	[Nullable(0)]
	public class YamlDocument
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x0001A087 File Offset: 0x00018287
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x0001A08F File Offset: 0x0001828F
		public YamlNode RootNode { get; private set; }

		// Token: 0x060007A2 RID: 1954 RVA: 0x0001A098 File Offset: 0x00018298
		public YamlDocument(YamlNode rootNode)
		{
			this.RootNode = rootNode;
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0001A0A7 File Offset: 0x000182A7
		public YamlDocument(string rootNode)
		{
			this.RootNode = new YamlScalarNode(rootNode);
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0001A0BC File Offset: 0x000182BC
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

		// Token: 0x060007A5 RID: 1957 RVA: 0x0001A126 File Offset: 0x00018326
		private void AssignAnchors()
		{
			new YamlDocument.AnchorAssigningVisitor().AssignAnchors(this);
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0001A133 File Offset: 0x00018333
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

		// Token: 0x060007A7 RID: 1959 RVA: 0x0001A166 File Offset: 0x00018366
		public void Accept(IYamlVisitor visitor)
		{
			visitor.Visit(this);
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0001A16F File Offset: 0x0001836F
		public IEnumerable<YamlNode> AllNodes
		{
			get
			{
				return this.RootNode.AllNodes;
			}
		}

		// Token: 0x02000181 RID: 385
		[Nullable(0)]
		private class AnchorAssigningVisitor : YamlVisitorBase
		{
			// Token: 0x060007A9 RID: 1961 RVA: 0x0001A17C File Offset: 0x0001837C
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

			// Token: 0x060007AA RID: 1962 RVA: 0x0001A280 File Offset: 0x00018480
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

			// Token: 0x060007AB RID: 1963 RVA: 0x0001A2C0 File Offset: 0x000184C0
			public override void Visit(YamlScalarNode scalar)
			{
				this.VisitNodeAndFindDuplicates(scalar);
			}

			// Token: 0x060007AC RID: 1964 RVA: 0x0001A2CA File Offset: 0x000184CA
			public override void Visit(YamlMappingNode mapping)
			{
				if (!this.VisitNodeAndFindDuplicates(mapping))
				{
					base.Visit(mapping);
				}
			}

			// Token: 0x060007AD RID: 1965 RVA: 0x0001A2DC File Offset: 0x000184DC
			public override void Visit(YamlSequenceNode sequence)
			{
				if (!this.VisitNodeAndFindDuplicates(sequence))
				{
					base.Visit(sequence);
				}
			}

			// Token: 0x04000340 RID: 832
			private readonly HashSet<AnchorName> existingAnchors = new HashSet<AnchorName>();

			// Token: 0x04000341 RID: 833
			private readonly Dictionary<YamlNode, bool> visitedNodes = new Dictionary<YamlNode, bool>();
		}
	}
}
