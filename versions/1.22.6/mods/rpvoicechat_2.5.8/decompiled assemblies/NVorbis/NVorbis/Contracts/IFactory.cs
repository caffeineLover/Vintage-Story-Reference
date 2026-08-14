using System;

namespace NVorbis.Contracts
{
	// Token: 0x02000025 RID: 37
	internal interface IFactory
	{
		// Token: 0x060001AC RID: 428
		ICodebook CreateCodebook();

		// Token: 0x060001AD RID: 429
		IFloor CreateFloor(IPacket packet);

		// Token: 0x060001AE RID: 430
		IResidue CreateResidue(IPacket packet);

		// Token: 0x060001AF RID: 431
		IMapping CreateMapping(IPacket packet);

		// Token: 0x060001B0 RID: 432
		IMode CreateMode();

		// Token: 0x060001B1 RID: 433
		IMdct CreateMdct();

		// Token: 0x060001B2 RID: 434
		IHuffman CreateHuffman();
	}
}
