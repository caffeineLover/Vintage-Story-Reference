using System;
using System.IO;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x020000F6 RID: 246
	internal interface IChunkVisitor
	{
		// Token: 0x0600049F RID: 1183
		void Visit(Stream stream, ImageHeader header, ChunkHeader chunkHeader, byte[] data, byte[] crc);
	}
}
