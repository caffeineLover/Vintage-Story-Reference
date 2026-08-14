using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000E9 RID: 233
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Section : ContainerElement, IStateful
	{
		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x000128E7 File Offset: 0x00010AE7
		// (set) Token: 0x06000795 RID: 1941 RVA: 0x000128EF File Offset: 0x00010AEF
		public string SectionName { get; set; }

		// Token: 0x06000796 RID: 1942 RVA: 0x000128F8 File Offset: 0x00010AF8
		internal override void Draw(Size availableSpace)
		{
			if (!this.IsRendered)
			{
				string documentLocationName = base.PageContext.GetDocumentLocationName(this.SectionName);
				base.Canvas.DrawSection(documentLocationName);
				this.IsRendered = true;
			}
			base.PageContext.SetSectionPage(this.SectionName);
			base.Draw(availableSpace);
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x0001294A File Offset: 0x00010B4A
		// (set) Token: 0x06000798 RID: 1944 RVA: 0x00012952 File Offset: 0x00010B52
		private bool IsRendered { get; set; }

		// Token: 0x06000799 RID: 1945 RVA: 0x0001295B File Offset: 0x00010B5B
		public void ResetState(bool hardReset = false)
		{
			this.IsRendered = false;
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x00012964 File Offset: 0x00010B64
		public object GetState()
		{
			return this.IsRendered;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x00012971 File Offset: 0x00010B71
		public void SetState(object state)
		{
			this.IsRendered = (bool)state;
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0001297F File Offset: 0x00010B7F
		[NullableContext(2)]
		internal override string GetCompanionHint()
		{
			return this.SectionName;
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x00012987 File Offset: 0x00010B87
		[NullableContext(2)]
		internal override string GetCompanionSearchableContent()
		{
			return this.SectionName;
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0001298F File Offset: 0x00010B8F
		[return: Nullable(new byte[]
		{
			2,
			0,
			1,
			1
		})]
		internal override IEnumerable<KeyValuePair<string, string>> GetCompanionProperties()
		{
			Section.<GetCompanionProperties>d__14 <GetCompanionProperties>d__ = new Section.<GetCompanionProperties>d__14(-2);
			<GetCompanionProperties>d__.<>4__this = this;
			return <GetCompanionProperties>d__;
		}
	}
}
