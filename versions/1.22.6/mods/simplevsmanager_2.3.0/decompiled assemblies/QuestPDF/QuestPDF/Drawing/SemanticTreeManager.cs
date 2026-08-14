using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace QuestPDF.Drawing
{
	// Token: 0x02000111 RID: 273
	[NullableContext(1)]
	[Nullable(0)]
	internal class SemanticTreeManager
	{
		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x00018831 File Offset: 0x00016A31
		// (set) Token: 0x0600096E RID: 2414 RVA: 0x00018839 File Offset: 0x00016A39
		private int CurrentNodeId { get; set; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x00018842 File Offset: 0x00016A42
		// (set) Token: 0x06000970 RID: 2416 RVA: 0x0001884A File Offset: 0x00016A4A
		[Nullable(2)]
		private SemanticTreeNode Root { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x00018853 File Offset: 0x00016A53
		// (set) Token: 0x06000972 RID: 2418 RVA: 0x0001885B File Offset: 0x00016A5B
		private Stack<SemanticTreeNode> Stack { get; set; } = new Stack<SemanticTreeNode>();

		// Token: 0x06000973 RID: 2419 RVA: 0x00018864 File Offset: 0x00016A64
		public SemanticTreeManager()
		{
			this.PopulateWithTopLevelNode();
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0001887D File Offset: 0x00016A7D
		private void PopulateWithTopLevelNode()
		{
			this.AddNode(new SemanticTreeNode
			{
				NodeId = this.GetNextNodeId(),
				Type = "Document"
			});
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x000188A4 File Offset: 0x00016AA4
		public int GetNextNodeId()
		{
			int currentNodeId = this.CurrentNodeId;
			this.CurrentNodeId = currentNodeId + 1;
			return this.CurrentNodeId;
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x000188C7 File Offset: 0x00016AC7
		public void AddNode(SemanticTreeNode node)
		{
			if (this.Root == null)
			{
				this.Root = node;
				this.Stack.Push(node);
				return;
			}
			SemanticTreeNode semanticTreeNode = this.Stack.Peek();
			if (semanticTreeNode == null)
			{
				return;
			}
			semanticTreeNode.Children.Add(node);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00018900 File Offset: 0x00016B00
		public void PushOnStack(SemanticTreeNode node)
		{
			this.Stack.Push(node);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0001890E File Offset: 0x00016B0E
		public void PopStack()
		{
			this.Stack.Pop();
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0001891C File Offset: 0x00016B1C
		public SemanticTreeNode PeekStack()
		{
			return this.Stack.Peek();
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00018929 File Offset: 0x00016B29
		public void Reset()
		{
			this.CurrentNodeId = 0;
			this.Root = null;
			this.Stack.Clear();
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00018944 File Offset: 0x00016B44
		[NullableContext(2)]
		public SemanticTreeNode GetSemanticTree()
		{
			return this.Root;
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x0001894C File Offset: 0x00016B4C
		// (set) Token: 0x0600097D RID: 2429 RVA: 0x00018954 File Offset: 0x00016B54
		private int ArtifactNestingLevel { get; set; }

		// Token: 0x0600097E RID: 2430 RVA: 0x00018960 File Offset: 0x00016B60
		public void BeginArtifactContent()
		{
			int artifactNestingLevel = this.ArtifactNestingLevel;
			this.ArtifactNestingLevel = artifactNestingLevel + 1;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00018980 File Offset: 0x00016B80
		public void EndArtifactContent()
		{
			int artifactNestingLevel = this.ArtifactNestingLevel;
			this.ArtifactNestingLevel = artifactNestingLevel - 1;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0001899D File Offset: 0x00016B9D
		public bool IsCurrentContentArtifact()
		{
			return this.ArtifactNestingLevel > 0;
		}
	}
}
