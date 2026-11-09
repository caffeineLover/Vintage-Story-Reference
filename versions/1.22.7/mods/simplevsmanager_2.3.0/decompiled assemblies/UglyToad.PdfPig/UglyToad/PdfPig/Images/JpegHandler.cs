using System;
using System.IO;

namespace UglyToad.PdfPig.Images
{
	// Token: 0x020000E9 RID: 233
	internal static class JpegHandler
	{
		// Token: 0x06000471 RID: 1137 RVA: 0x0001AF94 File Offset: 0x00019194
		public static JpegInformation GetInformation(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!JpegHandler.HasRecognizedHeader(stream))
			{
				throw new InvalidOperationException("The input stream did not start with the expected JPEG header [ 255 216 ]");
			}
			JpegMarker jpegMarker = JpegMarker.StartOfImage;
			byte[] buffer = new byte[2];
			while (jpegMarker != JpegMarker.EndOfImage)
			{
				switch (jpegMarker)
				{
				case JpegMarker.StartOfBaselineDctFrame:
				case JpegMarker.StartOfProgressiveDctFrame:
				{
					JpegHandler.ReadShort(stream, buffer);
					int bitsPerComponent = stream.ReadByte();
					ushort height = JpegHandler.ReadShort(stream, buffer);
					return new JpegInformation((int)JpegHandler.ReadShort(stream, buffer), (int)height, bitsPerComponent);
				}
				case (JpegMarker)193:
				case (JpegMarker)195:
				case JpegMarker.DefineHuffmanTable:
				case (JpegMarker)197:
				case (JpegMarker)198:
				case (JpegMarker)199:
				case (JpegMarker)200:
				case (JpegMarker)201:
				case (JpegMarker)202:
				case (JpegMarker)203:
				case (JpegMarker)204:
				case (JpegMarker)205:
				case (JpegMarker)206:
				case (JpegMarker)207:
				case JpegMarker.StartOfScan:
				case JpegMarker.DefineQuantizationTable:
				case (JpegMarker)220:
				case JpegMarker.DefineRestartInterval:
				case (JpegMarker)222:
				case (JpegMarker)223:
				case JpegMarker.ApplicationSpecific0:
				case JpegMarker.ApplicationSpecific1:
				case JpegMarker.ApplicationSpecific2:
				case JpegMarker.ApplicationSpecific3:
				case JpegMarker.ApplicationSpecific4:
				case JpegMarker.ApplicationSpecific5:
				case JpegMarker.ApplicationSpecific6:
				case JpegMarker.ApplicationSpecific7:
				case JpegMarker.ApplicationSpecific8:
				case JpegMarker.ApplicationSpecific9:
				case JpegMarker.ApplicationSpecific10:
				case JpegMarker.ApplicationSpecific11:
				case JpegMarker.ApplicationSpecific12:
				case JpegMarker.ApplicationSpecific13:
				case JpegMarker.ApplicationSpecific14:
				case JpegMarker.ApplicationSpecific15:
					goto IL_127;
				case JpegMarker.Restart0:
				case JpegMarker.Restart1:
				case JpegMarker.Restart2:
				case JpegMarker.Restart3:
				case JpegMarker.Restart4:
				case JpegMarker.Restart5:
				case JpegMarker.Restart6:
				case JpegMarker.Restart7:
				case JpegMarker.StartOfImage:
				case JpegMarker.EndOfImage:
					break;
				default:
					goto IL_127;
				}
				IL_13D:
				jpegMarker = (JpegMarker)JpegHandler.ReadSegmentMarker(stream, true);
				continue;
				IL_127:
				ushort num = JpegHandler.ReadShort(stream, buffer);
				stream.Seek((long)(num - 2), 1);
				goto IL_13D;
			}
			throw new InvalidOperationException("File was a valid JPEG but the width and height could not be determined.");
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0001B0FC File Offset: 0x000192FC
		private static bool HasRecognizedHeader(Stream stream)
		{
			byte[] array = new byte[2];
			return stream.Read(array, 0, 2) == 2 && array[0] == byte.MaxValue && array[1] == 216;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0001B134 File Offset: 0x00019334
		private static byte ReadSegmentMarker(Stream stream, bool skipData = false)
		{
			byte? b = default(byte?);
			int num;
			while ((num = stream.ReadByte()) != -1)
			{
				byte b2 = (byte)num;
				if (!skipData)
				{
					if (b == null && b2 != 255)
					{
						throw new InvalidOperationException();
					}
					if (b2 != 255)
					{
						return b2;
					}
				}
				if (b != null && b.Value == 255 && b2 != 255)
				{
					return b2;
				}
				b = new byte?(b2);
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0001B1AB File Offset: 0x000193AB
		private static ushort ReadShort(Stream stream, byte[] buffer)
		{
			if (stream.Read(buffer, 0, 2) != 2)
			{
				throw new InvalidOperationException("Failed to read a short where expected in the JPEG stream.");
			}
			return (ushort)(((int)buffer[0] << 8) + (int)buffer[1]);
		}

		// Token: 0x04000235 RID: 565
		private const byte MarkerStart = 255;

		// Token: 0x04000236 RID: 566
		private const byte StartOfImage = 216;
	}
}
