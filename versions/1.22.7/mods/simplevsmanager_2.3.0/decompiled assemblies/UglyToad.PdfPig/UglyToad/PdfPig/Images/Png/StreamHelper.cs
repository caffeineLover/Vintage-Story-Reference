using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x02000102 RID: 258
	internal static class StreamHelper
	{
		// Token: 0x060004E9 RID: 1257 RVA: 0x0001D717 File Offset: 0x0001B917
		public static int ReadBigEndianInt32(Stream stream)
		{
			return ((int)StreamHelper.ReadOrTerminate(stream) << 24) + ((int)StreamHelper.ReadOrTerminate(stream) << 16) + ((int)StreamHelper.ReadOrTerminate(stream) << 8) + (int)StreamHelper.ReadOrTerminate(stream);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001D73C File Offset: 0x0001B93C
		public static int ReadBigEndianInt32(byte[] bytes, int offset)
		{
			return ((int)bytes[offset] << 24) + ((int)bytes[1 + offset] << 16) + ((int)bytes[2 + offset] << 8) + (int)bytes[3 + offset];
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001D75B File Offset: 0x0001B95B
		public static void WriteBigEndianInt32(Stream stream, int value)
		{
			stream.WriteByte((byte)(value >> 24));
			stream.WriteByte((byte)(value >> 16));
			stream.WriteByte((byte)(value >> 8));
			stream.WriteByte((byte)value);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001D788 File Offset: 0x0001B988
		private static byte ReadOrTerminate(Stream stream)
		{
			int num = stream.ReadByte();
			if (num == -1)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(29, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unexpected end of stream at ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(stream.Position);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return (byte)num;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001D7DC File Offset: 0x0001B9DC
		public static bool TryReadHeaderBytes(Stream stream, out byte[] bytes)
		{
			bytes = new byte[8];
			return stream.Read(bytes, 0, 8) == 8;
		}
	}
}
