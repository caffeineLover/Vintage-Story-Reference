using System;

namespace Concentus.Common.CPlusPlus
{
	// Token: 0x0200007C RID: 124
	internal static class Arrays
	{
		// Token: 0x0600045B RID: 1115 RVA: 0x00027A00 File Offset: 0x00025C00
		internal static T[][] InitTwoDimensionalArray<T>(int x, int y)
		{
			T[][] array = new T[x][];
			for (int i = 0; i < x; i++)
			{
				array[i] = new T[y];
			}
			return array;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00027A2C File Offset: 0x00025C2C
		internal static Pointer<Pointer<T>> InitTwoDimensionalArrayPointer<T>(int x, int y)
		{
			Pointer<Pointer<T>> pointer = Pointer.Malloc<Pointer<T>>(x);
			for (int i = 0; i < x; i++)
			{
				pointer[i] = Pointer.Malloc<T>(y);
			}
			return pointer;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00027A5C File Offset: 0x00025C5C
		internal static T[][][] InitThreeDimensionalArray<T>(int x, int y, int z)
		{
			T[][][] array = new T[x][][];
			for (int i = 0; i < x; i++)
			{
				array[i] = new T[y][];
				for (int j = 0; j < y; j++)
				{
					array[i][j] = new T[z];
				}
			}
			return array;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00027AA0 File Offset: 0x00025CA0
		internal static void MemSetByte(byte[] array, byte value)
		{
			MemoryExtensions.AsSpan<byte>(array).Fill(value);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00027ABC File Offset: 0x00025CBC
		internal static void MemSetInt(int[] array, int value, int length)
		{
			MemoryExtensions.AsSpan<int>(array, 0, length).Fill(value);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00027ADC File Offset: 0x00025CDC
		internal static void MemSetShort(short[] array, short value, int length)
		{
			MemoryExtensions.AsSpan<short>(array, 0, length).Fill(value);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00027AFC File Offset: 0x00025CFC
		internal static void MemSetFloat(float[] array, float value, int length)
		{
			MemoryExtensions.AsSpan<float>(array, 0, length).Fill(value);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00027B1C File Offset: 0x00025D1C
		internal static void MemSetSbyte(sbyte[] array, sbyte value, int length)
		{
			MemoryExtensions.AsSpan<sbyte>(array, 0, length).Fill(value);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00027B3C File Offset: 0x00025D3C
		internal static void MemSetWithOffset<T>(T[] array, T value, int offset, int length)
		{
			MemoryExtensions.AsSpan<T>(array, offset, length).Fill(value);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00027B5C File Offset: 0x00025D5C
		internal static void MemSetWithOffset<T>(Span<T> array, T value, int offset, int length)
		{
			array.Slice(offset, length).Fill(value);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00027B7B File Offset: 0x00025D7B
		internal static void MemMoveByte(byte[] array, int src_idx, int dst_idx, int length)
		{
			if (src_idx == dst_idx || length == 0)
			{
				return;
			}
			Buffer.BlockCopy(array, src_idx, array, dst_idx, length);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00027B90 File Offset: 0x00025D90
		internal static void MemMoveByte(Span<byte> array, int src_idx, int dst_idx, int length)
		{
			if (src_idx == dst_idx || length == 0)
			{
				return;
			}
			array.Slice(src_idx, length).CopyTo(array.Slice(dst_idx, length));
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00027BBF File Offset: 0x00025DBF
		internal static void MemCopy(int[] src, int src_idx, int[] dst, int dst_idx, int length)
		{
			if (length == 0)
			{
				return;
			}
			Buffer.BlockCopy(src, src_idx * 4, dst, dst_idx * 4, length * 4);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00027BD7 File Offset: 0x00025DD7
		internal static void MemCopy(short[] src, int src_idx, short[] dst, int dst_idx, int length)
		{
			if (length == 0)
			{
				return;
			}
			Buffer.BlockCopy(src, src_idx * 2, dst, dst_idx * 2, length * 2);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00027BEF File Offset: 0x00025DEF
		internal static void MemCopy(sbyte[] src, int src_idx, sbyte[] dst, int dst_idx, int length)
		{
			if (length == 0)
			{
				return;
			}
			Buffer.BlockCopy(src, src_idx, dst, dst_idx, length);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00027C01 File Offset: 0x00025E01
		internal static void MemMoveInt(int[] array, int src_idx, int dst_idx, int length)
		{
			if (src_idx == dst_idx || length == 0)
			{
				return;
			}
			Buffer.BlockCopy(array, src_idx * 4, array, dst_idx * 4, length * 4);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00027C1B File Offset: 0x00025E1B
		internal static void MemMoveShort(short[] array, int src_idx, int dst_idx, int length)
		{
			if (src_idx == dst_idx || length == 0)
			{
				return;
			}
			Buffer.BlockCopy(array, src_idx * 2, array, dst_idx * 2, length * 2);
		}
	}
}
