using System;
using System.IO;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x02000016 RID: 22
	public static class WritingExtensions
	{
		// Token: 0x060000AD RID: 173 RVA: 0x000050E0 File Offset: 0x000032E0
		public static void WriteUInt(this Stream stream, long value)
		{
			stream.WriteUInt((uint)value);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000050EC File Offset: 0x000032EC
		public static void WriteUInt(this Stream stream, uint value)
		{
			byte[] array = new byte[]
			{
				(byte)(value >> 24),
				(byte)(value >> 16),
				(byte)(value >> 8),
				(byte)value
			};
			stream.Write(array, 0, 4);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00005125 File Offset: 0x00003325
		public static void WriteUShort(this Stream stream, int value)
		{
			stream.WriteUShort((ushort)value);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005130 File Offset: 0x00003330
		public static void WriteUShort(this Stream stream, ushort value)
		{
			byte[] array = new byte[]
			{
				(byte)(value >> 8),
				(byte)value
			};
			stream.Write(array, 0, 2);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005159 File Offset: 0x00003359
		public static void WriteShort(this Stream stream, ushort value)
		{
			stream.WriteShort((short)value);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00005164 File Offset: 0x00003364
		public static void WriteShort(this Stream stream, short value)
		{
			byte[] array = new byte[]
			{
				(byte)(value >> 8),
				(byte)value
			};
			stream.Write(array, 0, 2);
		}
	}
}
