using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.CrossReference
{
	// Token: 0x020001DA RID: 474
	internal class CrossReferenceTablePartBuilder
	{
		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x000393F0 File Offset: 0x000375F0
		// (set) Token: 0x06000B34 RID: 2868 RVA: 0x000393F8 File Offset: 0x000375F8
		public long Offset { get; set; }

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x00039401 File Offset: 0x00037601
		// (set) Token: 0x06000B36 RID: 2870 RVA: 0x00039409 File Offset: 0x00037609
		public long Previous { get; set; }

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x00039412 File Offset: 0x00037612
		// (set) Token: 0x06000B38 RID: 2872 RVA: 0x0003941A File Offset: 0x0003761A
		public DictionaryToken Dictionary { get; set; }

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000B39 RID: 2873 RVA: 0x00039423 File Offset: 0x00037623
		// (set) Token: 0x06000B3A RID: 2874 RVA: 0x0003942B File Offset: 0x0003762B
		public CrossReferenceType XRefType { get; set; }

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000B3B RID: 2875 RVA: 0x00039434 File Offset: 0x00037634
		// (set) Token: 0x06000B3C RID: 2876 RVA: 0x0003943C File Offset: 0x0003763C
		public long? TiedToPreviousAtOffset { get; set; }

		// Token: 0x06000B3D RID: 2877 RVA: 0x00039448 File Offset: 0x00037648
		public void Add(long objectId, int generationNumber, long offset)
		{
			IndirectReference indirectReference;
			indirectReference..ctor(objectId, generationNumber);
			if (!this.objects.ContainsKey(indirectReference))
			{
				this.objects[indirectReference] = offset;
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00039479 File Offset: 0x00037679
		public CrossReferenceTablePart Build()
		{
			return new CrossReferenceTablePart(this.objects, this.Offset, this.Previous, this.Dictionary, this.XRefType, this.TiedToPreviousAtOffset);
		}

		// Token: 0x040005C6 RID: 1478
		private readonly Dictionary<IndirectReference, long> objects = new Dictionary<IndirectReference, long>();
	}
}
