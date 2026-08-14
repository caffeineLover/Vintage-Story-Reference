using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace QuestPDF.Drawing
{
	// Token: 0x02000110 RID: 272
	[NullableContext(1)]
	[Nullable(0)]
	internal class SemanticTreeNode
	{
		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x000187B4 File Offset: 0x000169B4
		// (set) Token: 0x06000963 RID: 2403 RVA: 0x000187BC File Offset: 0x000169BC
		public int NodeId { get; set; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x000187C5 File Offset: 0x000169C5
		// (set) Token: 0x06000965 RID: 2405 RVA: 0x000187CD File Offset: 0x000169CD
		public string Type { get; set; } = "";

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x000187D6 File Offset: 0x000169D6
		// (set) Token: 0x06000967 RID: 2407 RVA: 0x000187DE File Offset: 0x000169DE
		[Nullable(2)]
		public string Alt { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x000187E7 File Offset: 0x000169E7
		// (set) Token: 0x06000969 RID: 2409 RVA: 0x000187EF File Offset: 0x000169EF
		[Nullable(2)]
		public string Lang { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x000187F8 File Offset: 0x000169F8
		public IList<SemanticTreeNode> Children { get; } = new List<SemanticTreeNode>();

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x00018800 File Offset: 0x00016A00
		public ICollection<SemanticTreeNode.Attribute> Attributes { get; } = new List<SemanticTreeNode.Attribute>();

		// Token: 0x020001F3 RID: 499
		[Nullable(0)]
		public class Attribute
		{
			// Token: 0x170002F0 RID: 752
			// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x00020E3D File Offset: 0x0001F03D
			// (set) Token: 0x06000DB4 RID: 3508 RVA: 0x00020E45 File Offset: 0x0001F045
			public string Owner { get; set; }

			// Token: 0x170002F1 RID: 753
			// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x00020E4E File Offset: 0x0001F04E
			// (set) Token: 0x06000DB6 RID: 3510 RVA: 0x00020E56 File Offset: 0x0001F056
			public string Name { get; set; }

			// Token: 0x170002F2 RID: 754
			// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x00020E5F File Offset: 0x0001F05F
			// (set) Token: 0x06000DB8 RID: 3512 RVA: 0x00020E67 File Offset: 0x0001F067
			public object Value { get; set; }
		}
	}
}
