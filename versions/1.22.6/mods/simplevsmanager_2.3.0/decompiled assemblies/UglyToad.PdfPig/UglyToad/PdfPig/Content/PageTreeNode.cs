using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001F4 RID: 500
	public class PageTreeNode
	{
		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000C10 RID: 3088 RVA: 0x0003BC62 File Offset: 0x00039E62
		[NotNull]
		public DictionaryToken NodeDictionary { get; }

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000C11 RID: 3089 RVA: 0x0003BC6A File Offset: 0x00039E6A
		public IndirectReference Reference { get; }

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000C12 RID: 3090 RVA: 0x0003BC72 File Offset: 0x00039E72
		public bool IsPage { get; }

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000C13 RID: 3091 RVA: 0x0003BC7A File Offset: 0x00039E7A
		// (set) Token: 0x06000C14 RID: 3092 RVA: 0x0003BC82 File Offset: 0x00039E82
		public int? PageNumber { get; internal set; }

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000C15 RID: 3093 RVA: 0x0003BC8B File Offset: 0x00039E8B
		// (set) Token: 0x06000C16 RID: 3094 RVA: 0x0003BC93 File Offset: 0x00039E93
		[NotNull]
		public IReadOnlyList<PageTreeNode> Children { get; private set; }

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000C17 RID: 3095 RVA: 0x0003BC9C File Offset: 0x00039E9C
		// (set) Token: 0x06000C18 RID: 3096 RVA: 0x0003BCA4 File Offset: 0x00039EA4
		[CanBeNull]
		public PageTreeNode Parent { get; private set; }

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000C19 RID: 3097 RVA: 0x0003BCAD File Offset: 0x00039EAD
		public bool IsRoot
		{
			get
			{
				return this.Parent == null;
			}
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x0003BCB8 File Offset: 0x00039EB8
		internal PageTreeNode(DictionaryToken nodeDictionary, IndirectReference reference, bool isPage, int? pageNumber)
		{
			if (nodeDictionary == null)
			{
				throw new ArgumentNullException("nodeDictionary");
			}
			this.NodeDictionary = nodeDictionary;
			this.Reference = reference;
			this.IsPage = isPage;
			this.PageNumber = pageNumber;
			if (!this.IsPage && this.PageNumber != null)
			{
				throw new ArgumentException("Cannot define page number for a pages node.", "pageNumber");
			}
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x0003BD20 File Offset: 0x00039F20
		internal PageTreeNode WithChildren(IReadOnlyList<PageTreeNode> children)
		{
			if (children == null)
			{
				throw new ArgumentNullException("children");
			}
			this.Children = children;
			if (this.IsPage && this.Children.Count > 0)
			{
				throw new ArgumentException("Cannot define children on a page node.", "children");
			}
			foreach (PageTreeNode pageTreeNode in this.Children)
			{
				pageTreeNode.Parent = this;
			}
			return this;
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x0003BDA8 File Offset: 0x00039FA8
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (this.IsPage)
			{
				defaultInterpolatedStringHandler..ctor(9, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Page #");
				defaultInterpolatedStringHandler.AppendFormatted<int?>(this.PageNumber);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(this.NodeDictionary);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			defaultInterpolatedStringHandler..ctor(19, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Pages (");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.Children.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" children): ");
			defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(this.NodeDictionary);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
