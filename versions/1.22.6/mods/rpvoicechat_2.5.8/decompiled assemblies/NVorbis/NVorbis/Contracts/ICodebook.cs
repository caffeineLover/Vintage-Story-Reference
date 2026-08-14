using System;

namespace NVorbis.Contracts
{
	// Token: 0x02000022 RID: 34
	internal interface ICodebook
	{
		// Token: 0x0600019A RID: 410
		void Init(IPacket packet, IHuffman huffman);

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600019B RID: 411
		int Dimensions { get; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600019C RID: 412
		int Entries { get; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600019D RID: 413
		int MapType { get; }

		// Token: 0x17000081 RID: 129
		float this[int entry, int dim]
		{
			get;
		}

		// Token: 0x0600019F RID: 415
		int DecodeScalar(IPacket packet);
	}
}
