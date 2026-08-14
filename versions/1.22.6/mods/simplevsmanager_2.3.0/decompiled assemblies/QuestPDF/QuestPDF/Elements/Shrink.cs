using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000F0 RID: 240
	internal sealed class Shrink : ContainerElement, IContentDirectionAware
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x00012F95 File Offset: 0x00011195
		// (set) Token: 0x060007D1 RID: 2001 RVA: 0x00012F9D File Offset: 0x0001119D
		public bool Vertical { get; set; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x00012FA6 File Offset: 0x000111A6
		// (set) Token: 0x060007D3 RID: 2003 RVA: 0x00012FAE File Offset: 0x000111AE
		public bool Horizontal { get; set; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x00012FB7 File Offset: 0x000111B7
		// (set) Token: 0x060007D5 RID: 2005 RVA: 0x00012FBF File Offset: 0x000111BF
		public ContentDirection ContentDirection { get; set; }

		// Token: 0x060007D6 RID: 2006 RVA: 0x00012FC8 File Offset: 0x000111C8
		internal override void Draw(Size availableSpace)
		{
			SpacePlan spacePlan = base.Measure(availableSpace);
			SpacePlanType type = spacePlan.Type;
			bool flag = type <= SpacePlanType.Wrap;
			if (flag)
			{
				return;
			}
			Size size = new Size(this.Horizontal ? spacePlan.Width : availableSpace.Width, this.Vertical ? spacePlan.Height : availableSpace.Height);
			Position vector = (this.ContentDirection == ContentDirection.RightToLeft) ? new Position(availableSpace.Width - size.Width, 0f) : Position.Zero;
			base.Canvas.Translate(vector);
			base.Draw(size);
			base.Canvas.Translate(vector.Reverse());
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00013078 File Offset: 0x00011278
		[NullableContext(2)]
		internal override string GetCompanionHint()
		{
			bool vertical = this.Vertical;
			bool horizontal = this.Horizontal;
			string result;
			if (vertical)
			{
				if (!horizontal)
				{
					result = "Vertical axis";
				}
				else
				{
					result = "Both axes";
				}
			}
			else if (!horizontal)
			{
				result = null;
			}
			else
			{
				result = "Horizontal axis";
			}
			return result;
		}
	}
}
