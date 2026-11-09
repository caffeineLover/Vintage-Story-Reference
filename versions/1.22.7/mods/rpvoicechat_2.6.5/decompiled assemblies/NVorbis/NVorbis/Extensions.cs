using System;
using NVorbis.Contracts;

namespace NVorbis
{
	// Token: 0x02000004 RID: 4
	public static class Extensions
	{
		// Token: 0x06000026 RID: 38 RVA: 0x0000298C File Offset: 0x00000B8C
		public static int Read(this IPacket packet, byte[] buffer, int index, int count)
		{
			if (index < 0 || index >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0 || index + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			for (int i = 0; i < count; i++)
			{
				int num;
				byte b = (byte)packet.TryPeekBits(8, out num);
				if (num == 0)
				{
					return i;
				}
				buffer[index++] = b;
				packet.SkipBits(8);
			}
			return count;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000029F4 File Offset: 0x00000BF4
		public static byte[] ReadBytes(this IPacket packet, int count)
		{
			byte[] array = new byte[count];
			int num = packet.Read(array, 0, count);
			if (num < count)
			{
				byte[] array2 = new byte[num];
				Buffer.BlockCopy(array, 0, array2, 0, num);
				return array2;
			}
			return array;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002A2A File Offset: 0x00000C2A
		public static bool ReadBit(this IPacket packet)
		{
			return packet.ReadBits(1) == 1UL;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002A38 File Offset: 0x00000C38
		public static byte PeekByte(this IPacket packet)
		{
			int num;
			return (byte)packet.TryPeekBits(8, out num);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002A4F File Offset: 0x00000C4F
		public static byte ReadByte(this IPacket packet)
		{
			return (byte)packet.ReadBits(8);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A59 File Offset: 0x00000C59
		public static short ReadInt16(this IPacket packet)
		{
			return (short)packet.ReadBits(16);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002A64 File Offset: 0x00000C64
		public static int ReadInt32(this IPacket packet)
		{
			return (int)packet.ReadBits(32);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002A6F File Offset: 0x00000C6F
		public static long ReadInt64(this IPacket packet)
		{
			return (long)packet.ReadBits(64);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002A79 File Offset: 0x00000C79
		public static ushort ReadUInt16(this IPacket packet)
		{
			return (ushort)packet.ReadBits(16);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002A84 File Offset: 0x00000C84
		public static uint ReadUInt32(this IPacket packet)
		{
			return (uint)packet.ReadBits(32);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002A8F File Offset: 0x00000C8F
		public static ulong ReadUInt64(this IPacket packet)
		{
			return packet.ReadBits(64);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002A99 File Offset: 0x00000C99
		public static void SkipBytes(this IPacket packet, int count)
		{
			packet.SkipBits(count * 8);
		}
	}
}
