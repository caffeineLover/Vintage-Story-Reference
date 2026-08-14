using System;
using System.Runtime.CompilerServices;
using System.Text;
using QuestPDF.Drawing;
using QuestPDF.Elements.Text;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000EB RID: 235
	[NullableContext(2)]
	[Nullable(0)]
	internal class SemanticTag : ContainerElement, ISemanticAware
	{
		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x00012A4F File Offset: 0x00010C4F
		// (set) Token: 0x060007AA RID: 1962 RVA: 0x00012A57 File Offset: 0x00010C57
		public SemanticTreeManager SemanticTreeManager { get; set; }

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x00012A60 File Offset: 0x00010C60
		// (set) Token: 0x060007AC RID: 1964 RVA: 0x00012A68 File Offset: 0x00010C68
		public SemanticTreeNode SemanticTreeNode { get; private set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x00012A71 File Offset: 0x00010C71
		// (set) Token: 0x060007AE RID: 1966 RVA: 0x00012A79 File Offset: 0x00010C79
		[Nullable(1)]
		public string TagType { [NullableContext(1)] get; [NullableContext(1)] set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x00012A82 File Offset: 0x00010C82
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x00012A8A File Offset: 0x00010C8A
		public string Alt { get; set; }

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x00012A93 File Offset: 0x00010C93
		// (set) Token: 0x060007B2 RID: 1970 RVA: 0x00012A9B File Offset: 0x00010C9B
		public string Lang { get; set; }

		// Token: 0x060007B3 RID: 1971 RVA: 0x00012AA4 File Offset: 0x00010CA4
		internal override void Draw(Size availableSpace)
		{
			if (this.SemanticTreeManager != null && !this.SemanticTreeManager.IsCurrentContentArtifact())
			{
				this.RegisterCurrentSemanticNode();
				this.SemanticTreeManager.PushOnStack(this.SemanticTreeNode);
				base.Canvas.SetSemanticNodeId(this.SemanticTreeNode.NodeId);
				Element child = base.Child;
				if (child != null)
				{
					child.Draw(availableSpace);
				}
				this.SemanticTreeManager.PopStack();
				return;
			}
			Element child2 = base.Child;
			if (child2 == null)
			{
				return;
			}
			child2.Draw(availableSpace);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00012B24 File Offset: 0x00010D24
		internal void RegisterCurrentSemanticNode()
		{
			if (this.SemanticTreeNode != null)
			{
				return;
			}
			string tagType = this.TagType;
			bool flag;
			if (tagType != null)
			{
				int length = tagType.Length;
				if (length != 1)
				{
					if (length != 2)
					{
						goto IL_C0;
					}
					switch (tagType.get_Chars(1))
					{
					case '1':
						if (!(tagType == "H1"))
						{
							goto IL_C0;
						}
						break;
					case '2':
						if (!(tagType == "H2"))
						{
							goto IL_C0;
						}
						break;
					case '3':
						if (!(tagType == "H3"))
						{
							goto IL_C0;
						}
						break;
					case '4':
						if (!(tagType == "H4"))
						{
							goto IL_C0;
						}
						break;
					case '5':
						if (!(tagType == "H5"))
						{
							goto IL_C0;
						}
						break;
					case '6':
						if (!(tagType == "H6"))
						{
							goto IL_C0;
						}
						break;
					default:
						goto IL_C0;
					}
				}
				else if (!(tagType == "H"))
				{
					goto IL_C0;
				}
				flag = true;
				goto IL_C2;
			}
			IL_C0:
			flag = false;
			IL_C2:
			if (flag)
			{
				this.UpdateHeaderText();
			}
			if (this.TagType == "Link")
			{
				this.UpdateDescriptionOfInnerLink();
			}
			int nextNodeId = this.SemanticTreeManager.GetNextNodeId();
			this.SemanticTreeNode = new SemanticTreeNode
			{
				NodeId = nextNodeId,
				Type = this.TagType,
				Alt = this.Alt,
				Lang = this.Lang
			};
			this.SemanticTreeManager.AddNode(this.SemanticTreeNode);
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00012C68 File Offset: 0x00010E68
		private void UpdateHeaderText()
		{
			if (!string.IsNullOrWhiteSpace(this.Alt))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			SemanticTag.<UpdateHeaderText>g__Traverse|22_0(stringBuilder, base.Child);
			this.Alt = stringBuilder.ToString();
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x00012CA4 File Offset: 0x00010EA4
		private void UpdateDescriptionOfInnerLink()
		{
			if (string.IsNullOrWhiteSpace(this.Alt))
			{
				return;
			}
			ContainerElement containerElement;
			for (Element element = base.Child; element != null; element = ((containerElement != null) ? containerElement.Child : null))
			{
				Hyperlink hyperlink = element as Hyperlink;
				if (hyperlink != null)
				{
					hyperlink.Description = this.Alt;
					return;
				}
				SectionLink sectionLink = element as SectionLink;
				if (sectionLink != null)
				{
					sectionLink.Description = this.Alt;
					return;
				}
				containerElement = (element as ContainerElement);
			}
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x00012D0C File Offset: 0x00010F0C
		internal override string GetCompanionHint()
		{
			string text = this.TagType;
			if (!string.IsNullOrWhiteSpace(this.Alt))
			{
				text = text + " (" + this.Alt + ")";
			}
			return text;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00012D45 File Offset: 0x00010F45
		internal override string GetCompanionSearchableContent()
		{
			return this.TagType;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00012D58 File Offset: 0x00010F58
		[NullableContext(1)]
		[CompilerGenerated]
		internal static void <UpdateHeaderText>g__Traverse|22_0(StringBuilder builder, Element element)
		{
			TextBlock textBlock = element as TextBlock;
			if (textBlock != null)
			{
				if (builder.Length > 0)
				{
					builder.Append(' ');
				}
				builder.Append(textBlock.Text);
				return;
			}
			ContainerElement containerElement = element as ContainerElement;
			if (containerElement != null)
			{
				SemanticTag.<UpdateHeaderText>g__Traverse|22_0(builder, containerElement.Child);
				return;
			}
			foreach (Element element2 in element.GetChildren())
			{
				SemanticTag.<UpdateHeaderText>g__Traverse|22_0(builder, element2);
			}
		}
	}
}
