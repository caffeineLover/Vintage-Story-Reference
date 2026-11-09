using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000EA RID: 234
	[NullableContext(2)]
	[Nullable(0)]
	internal sealed class SectionLink : ContainerElement
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x000129A7 File Offset: 0x00010BA7
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x000129AF File Offset: 0x00010BAF
		[Nullable(1)]
		public string SectionName { [NullableContext(1)] get; [NullableContext(1)] set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x000129B8 File Offset: 0x00010BB8
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x000129C0 File Offset: 0x00010BC0
		public string Description { get; set; }

		// Token: 0x060007A4 RID: 1956 RVA: 0x000129CC File Offset: 0x00010BCC
		internal override void Draw(Size availableSpace)
		{
			SpacePlan spacePlan = base.Measure(availableSpace);
			SpacePlanType type = spacePlan.Type;
			bool flag = type <= SpacePlanType.Wrap;
			if (flag)
			{
				return;
			}
			string documentLocationName = base.PageContext.GetDocumentLocationName(this.SectionName);
			base.Canvas.DrawSectionLink(spacePlan, documentLocationName, this.Description);
			base.Draw(availableSpace);
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00012A27 File Offset: 0x00010C27
		internal override string GetCompanionHint()
		{
			return this.SectionName;
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00012A2F File Offset: 0x00010C2F
		internal override string GetCompanionSearchableContent()
		{
			return this.SectionName;
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x00012A37 File Offset: 0x00010C37
		[return: Nullable(new byte[]
		{
			2,
			0,
			1,
			1
		})]
		internal override IEnumerable<KeyValuePair<string, string>> GetCompanionProperties()
		{
			SectionLink.<GetCompanionProperties>d__11 <GetCompanionProperties>d__ = new SectionLink.<GetCompanionProperties>d__11(-2);
			<GetCompanionProperties>d__.<>4__this = this;
			return <GetCompanionProperties>d__;
		}
	}
}
