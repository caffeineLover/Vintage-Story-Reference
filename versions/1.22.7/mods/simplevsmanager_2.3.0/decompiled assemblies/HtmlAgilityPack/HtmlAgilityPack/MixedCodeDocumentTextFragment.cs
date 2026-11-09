using System;
using System.Runtime.CompilerServices;

namespace HtmlAgilityPack
{
	// Token: 0x0200002B RID: 43
	[NullableContext(1)]
	[Nullable(0)]
	public class MixedCodeDocumentTextFragment : MixedCodeDocumentFragment
	{
		// Token: 0x06000273 RID: 627 RVA: 0x00011B0F File Offset: 0x0000FD0F
		internal MixedCodeDocumentTextFragment(MixedCodeDocument doc) : base(doc, MixedCodeDocumentFragmentType.Text)
		{
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000274 RID: 628 RVA: 0x00011B19 File Offset: 0x0000FD19
		// (set) Token: 0x06000275 RID: 629 RVA: 0x00011B21 File Offset: 0x0000FD21
		public string Text
		{
			get
			{
				return base.FragmentText;
			}
			set
			{
				base.FragmentText = value;
			}
		}
	}
}
