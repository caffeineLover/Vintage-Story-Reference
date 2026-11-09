using System;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk.Structs
{
	// Token: 0x02000046 RID: 70
	internal class SideInfoIndices
	{
		// Token: 0x0600013D RID: 317 RVA: 0x00014688 File Offset: 0x00012888
		internal void Reset()
		{
			Arrays.MemSetSbyte(this.GainsIndices, 0, 4);
			Arrays.MemSetSbyte(this.LTPIndex, 0, 4);
			Arrays.MemSetSbyte(this.NLSFIndices, 0, 17);
			this.lagIndex = 0;
			this.contourIndex = 0;
			this.signalType = 0;
			this.quantOffsetType = 0;
			this.NLSFInterpCoef_Q2 = 0;
			this.PERIndex = 0;
			this.LTP_scaleIndex = 0;
			this.Seed = 0;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000146F8 File Offset: 0x000128F8
		internal void Assign(SideInfoIndices other)
		{
			Arrays.MemCopy(other.GainsIndices, 0, this.GainsIndices, 0, 4);
			Arrays.MemCopy(other.LTPIndex, 0, this.LTPIndex, 0, 4);
			Arrays.MemCopy(other.NLSFIndices, 0, this.NLSFIndices, 0, 17);
			this.lagIndex = other.lagIndex;
			this.contourIndex = other.contourIndex;
			this.signalType = other.signalType;
			this.quantOffsetType = other.quantOffsetType;
			this.NLSFInterpCoef_Q2 = other.NLSFInterpCoef_Q2;
			this.PERIndex = other.PERIndex;
			this.LTP_scaleIndex = other.LTP_scaleIndex;
			this.Seed = other.Seed;
		}

		// Token: 0x040001BA RID: 442
		internal readonly sbyte[] GainsIndices = new sbyte[4];

		// Token: 0x040001BB RID: 443
		internal readonly sbyte[] LTPIndex = new sbyte[4];

		// Token: 0x040001BC RID: 444
		internal readonly sbyte[] NLSFIndices = new sbyte[17];

		// Token: 0x040001BD RID: 445
		internal short lagIndex;

		// Token: 0x040001BE RID: 446
		internal sbyte contourIndex;

		// Token: 0x040001BF RID: 447
		internal sbyte signalType;

		// Token: 0x040001C0 RID: 448
		internal sbyte quantOffsetType;

		// Token: 0x040001C1 RID: 449
		internal sbyte NLSFInterpCoef_Q2;

		// Token: 0x040001C2 RID: 450
		internal sbyte PERIndex;

		// Token: 0x040001C3 RID: 451
		internal sbyte LTP_scaleIndex;

		// Token: 0x040001C4 RID: 452
		internal sbyte Seed;
	}
}
