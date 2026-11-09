using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Companion;

namespace QuestPDF.Drawing.DocumentCanvases
{
	// Token: 0x02000124 RID: 292
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class CompanionDocumentSnapshot
	{
		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x0001A8C8 File Offset: 0x00018AC8
		// (set) Token: 0x06000A41 RID: 2625 RVA: 0x0001A8D0 File Offset: 0x00018AD0
		public ICollection<CompanionPageSnapshot> Pictures { get; set; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x0001A8D9 File Offset: 0x00018AD9
		// (set) Token: 0x06000A43 RID: 2627 RVA: 0x0001A8E1 File Offset: 0x00018AE1
		public CompanionCommands.UpdateDocumentStructure.DocumentHierarchyElement Hierarchy { get; set; }
	}
}
