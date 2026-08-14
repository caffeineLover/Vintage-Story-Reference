using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.MathTools;

namespace Vintagestory.Common;

public static class ArrayConvert
{
	public static byte[] UshortToByte(ushort[] shorts)
	{
		byte[] output = GC.AllocateUninitializedArray<byte>(shorts.Length * 2);
		UshortToByte(shorts, output);
		return output;
	}

	public unsafe static void UshortToByte(ushort[] shorts, byte[] output)
	{
		fixed (ushort* pSrc = shorts)
		{
			fixed (byte* pDst = output)
			{
				Buffer.MemoryCopy(pSrc, pDst, output.Length, output.Length);
			}
		}
	}

	internal static ushort[] ByteToUshort(byte[] data)
	{
		return ByteToUshort(data, data.Length);
	}

	internal static ushort[] ByteToUshort(byte[] data, int length)
	{
		ushort[] output = GC.AllocateUninitializedArray<ushort>(length / 2);
		ByteToUshort(data, output);
		return output;
	}

	internal unsafe static void ByteToUshort(byte[] data, ushort[] output)
	{
		fixed (byte* pSrc = data)
		{
			fixed (ushort* pDst = output)
			{
				int lengthInBytes = output.Length * 2;
				Buffer.MemoryCopy(pSrc, pDst, lengthInBytes, lengthInBytes);
			}
		}
	}

	internal static byte[] ByteToByte(byte[] data, int length)
	{
		return ByteToByte(data, length, 0);
	}

	internal unsafe static byte[] ByteToByte(byte[] data, int length, int reserveOffset)
	{
		if (length == 0)
		{
			if (reserveOffset != 0)
			{
				return new byte[reserveOffset];
			}
			return Array.Empty<byte>();
		}
		if ((uint)length > (uint)data.Length)
		{
			throw new ArgumentException("Source data buffer length (" + data.Length + ") is less than specified copy length: " + length);
		}
		byte[] output = GC.AllocateUninitializedArray<byte>(length + reserveOffset);
		if (reserveOffset != 0)
		{
			Array.Clear(output, 0, reserveOffset);
		}
		fixed (byte* pSrc = data)
		{
			fixed (byte* pDst = output)
			{
				Buffer.MemoryCopy(pSrc, pDst + reserveOffset, length, length);
			}
		}
		return output;
	}

	internal unsafe static byte[] Build(int lengthA, byte[] dataA, byte[] data, int length)
	{
		if (length + lengthA == 0)
		{
			return new byte[4];
		}
		byte[] array = GC.AllocateUninitializedArray<byte>(length + lengthA + 4);
		array[0] = (byte)lengthA;
		array[1] = (byte)(lengthA >> 8);
		array[2] = (byte)(lengthA >> 16);
		array[3] = (byte)(lengthA >> 24);
		fixed (byte* pSrcA = dataA)
		{
			fixed (byte* pSrcB = data)
			{
				fixed (byte* pDst = array)
				{
					Buffer.MemoryCopy(pSrcA, pDst + 4, lengthA, lengthA);
					Buffer.MemoryCopy(pSrcB, pDst + 4 + lengthA, length, length);
				}
			}
		}
		return array;
	}

	internal unsafe static byte[] Build(int lengthA, int[] intData, byte[] data, int length)
	{
		if (length + lengthA == 0)
		{
			return new byte[4];
		}
		lengthA *= 4;
		byte[] array = GC.AllocateUninitializedArray<byte>(length + lengthA + 4);
		int intLengthToWrite = -lengthA;
		array[0] = (byte)intLengthToWrite;
		array[1] = (byte)(intLengthToWrite >> 8);
		array[2] = (byte)(intLengthToWrite >> 16);
		array[3] = (byte)(intLengthToWrite >> 24);
		fixed (int* pSrcA = intData)
		{
			fixed (byte* pSrcB = data)
			{
				fixed (byte* pDst = array)
				{
					Buffer.MemoryCopy(pSrcA, pDst + 4, lengthA, lengthA);
					Buffer.MemoryCopy(pSrcB, pDst + 4 + lengthA, length, length);
				}
			}
		}
		return array;
	}

	internal static int GetInt(byte[] output)
	{
		int a0 = output[0] & 0xFF;
		int a1 = output[1] & 0xFF;
		int a2 = output[2] & 0xFF;
		return ((((output[3] & 0xFF) << 8) + a2 << 8) + a1 << 8) + a0;
	}

	public static byte[] IntToByte(int[] ints)
	{
		byte[] output = GC.AllocateUninitializedArray<byte>(ints.Length * 4);
		IntToByte(ints, output);
		return output;
	}

	public unsafe static void IntToByte(int[] ints, byte[] output)
	{
		fixed (int* pSrc = ints)
		{
			fixed (byte* pDst = output)
			{
				int length = ints.Length * 4;
				Buffer.MemoryCopy(pSrc, pDst, output.Length, length);
			}
		}
	}

	public static int[] ByteToInt(byte[] data)
	{
		int[] output = GC.AllocateUninitializedArray<int>(data.Length / 4);
		ByteToInt(data, output);
		return output;
	}

	public unsafe static void ByteToInt(byte[] data, int[] output)
	{
		fixed (byte* pSrc = data)
		{
			fixed (int* pDst = output)
			{
				int length = output.Length * 4;
				Buffer.MemoryCopy(pSrc, pDst, length, length);
			}
		}
	}

	public unsafe static void ByteToInt(byte[] data, int[] output, int length)
	{
		fixed (byte* pSrc = data)
		{
			fixed (int* pDst = output)
			{
				Buffer.MemoryCopy(pSrc, pDst, output.Length * 4, length * 4);
			}
		}
	}

	public unsafe static void ByteToInt(byte[] data, int offset, int[] output, int length)
	{
		fixed (byte* pSrc = data)
		{
			fixed (int* pDst = output)
			{
				Buffer.MemoryCopy(pSrc + offset, pDst, output.Length * 4, length * 4);
			}
		}
	}

	public unsafe static void ByteToIntArrays(byte[] data, int[][] output, int count, Func<int[]> newArray)
	{
		for (int j = 0; j < count; j++)
		{
			if (output[j] == null)
			{
				output[j] = newArray();
			}
		}
		fixed (byte* pSrc = data)
		{
			int offset = 0;
			for (int i = 0; i < count; i++)
			{
				int[] obj = output[i];
				int length = obj.Length * 4;
				fixed (int* pDst = obj)
				{
					Buffer.MemoryCopy(pSrc + offset, pDst, length, length);
				}
				offset += length;
			}
		}
	}

	public unsafe static void ByteToUint(byte[] data, uint[] output, int length)
	{
		fixed (byte* pByte = data)
		{
			uint* pInt = (uint*)pByte;
			for (int i = 0; i < length; i++)
			{
				output[i] = pInt[i];
			}
		}
	}

	public static T[] Copy<T>(this IEnumerable<T> data, long index, long length)
	{
		T[] result = GC.AllocateUninitializedArray<T>((int)length);
		Array.Copy(data.ToArray(), index, result, 0L, length);
		return result;
	}

	internal static void IntToInt(int[] src, int[] dest, int offset)
	{
		Array.Copy(src, 0, dest, offset, src.Length);
	}

	public static Vec3f[] ToVec3fs(this byte[] bytes)
	{
		return bytes.ToFloats().ToVec3fs();
	}

	public static Vec2f[] ToVec2fs(this byte[] bytes)
	{
		return bytes.ToFloats().ToVec2fs();
	}

	public static Vec4s[] ToVec4Ss(this byte[] bytes)
	{
		return bytes.ToShorts().ToVec4ss();
	}

	public static Vec4us[] ToVec4uss(this byte[] bytes)
	{
		return bytes.ToUShorts().ToVec4uss();
	}

	public static int[] ToInts(this ushort[] shorts)
	{
		int[] ints = GC.AllocateUninitializedArray<int>(shorts.Length);
		for (int i = 0; i < shorts.Length; i++)
		{
			ints[i] = shorts[i];
		}
		return ints;
	}

	public static Vec4s[] ToVec4ss(this IEnumerable<short> shorts1)
	{
		short[] shorts2 = shorts1.ToArray();
		Vec4s[] vecs = new Vec4s[shorts2.Length / 4];
		for (int i = 0; i < vecs.Length; i++)
		{
			vecs[i] = new Vec4s(shorts2[i * 4], shorts2[i * 4 + 1], shorts2[i * 4 + 2], shorts2[i * 4 + 3]);
		}
		return vecs;
	}

	public static Vec4us[] ToVec4uss(this IEnumerable<ushort> shorts1)
	{
		ushort[] shorts2 = shorts1.ToArray();
		Vec4us[] vecs = new Vec4us[shorts2.Length / 4];
		for (int i = 0; i < vecs.Length; i++)
		{
			vecs[i] = new Vec4us(shorts2[i * 4], shorts2[i * 4 + 1], shorts2[i * 4 + 2], shorts2[i * 4 + 3]);
		}
		return vecs;
	}

	public static Vec3f[] ToVec3fs(this IEnumerable<float> floats1)
	{
		float[] floats2 = floats1.ToArray();
		Vec3f[] vecs = new Vec3f[floats2.Length / 3];
		for (int i = 0; i < vecs.Length; i++)
		{
			vecs[i] = new Vec3f(floats2[i * 3], floats2[i * 3 + 1], floats2[i * 3 + 2]);
		}
		return vecs;
	}

	public static Vec2f[] ToVec2fs(this IEnumerable<float> floats1)
	{
		float[] floats2 = floats1.ToArray();
		Vec2f[] vecs = new Vec2f[floats2.Length / 2];
		for (int i = 0; i < vecs.Length; i++)
		{
			vecs[i] = new Vec2f(floats2[i * 2], floats2[i * 2 + 1]);
		}
		return vecs;
	}

	public static int[] ToInts(this IEnumerable<byte> bytes)
	{
		int[] ints = GC.AllocateUninitializedArray<int>(bytes.Count() / 4);
		for (int i = 0; i < ints.Length; i++)
		{
			ints[i] = BitConverter.ToInt32(bytes.Copy(i * 4, 4L), 0);
		}
		return ints;
	}

	public static ulong[] BytesToULongs(this IEnumerable<byte> bytes)
	{
		return bytes.ToUShorts().FourShortsToULong();
	}

	public static ulong[] FourShortsToULong(this IEnumerable<ushort> shorts1)
	{
		long[] longs = shorts1.FourShortsToLong();
		ulong[] ulongs = new ulong[longs.Count()];
		for (int i = 0; i < ulongs.Length; i++)
		{
			ulongs[i] = (ulong)longs[i];
		}
		return ulongs;
	}

	public static long[] FourShortsToLong(this IEnumerable<ushort> shorts1)
	{
		ushort[] shorts2 = shorts1.ToArray();
		long[] longs = new long[shorts2.Count() / 4];
		for (int i = 0; i < longs.Length; i++)
		{
			long tmp = (shorts2[i * 4] << 16) | shorts2[i * 4 + 1] | (shorts2[i * 4 + 2] << 16) | shorts2[i * 4 + 3];
			longs[i] = tmp;
		}
		return longs;
	}

	public static float[] ToFloats(this IEnumerable<byte> bytes)
	{
		float[] floats = new float[bytes.Count() / 4];
		for (int i = 0; i < floats.Length; i++)
		{
			floats[i] = BitConverter.ToSingle(bytes.Copy(i * 4, 4L), 0);
		}
		return floats;
	}

	public static ushort[] ToUShorts(this IEnumerable<byte> bytes)
	{
		ushort[] shorts = new ushort[bytes.Count() / 2];
		for (int i = 0; i < shorts.Length; i++)
		{
			shorts[i] = BitConverter.ToUInt16(bytes.Copy(i * 2, 2L), 0);
		}
		return shorts;
	}

	public static short[] ToShorts(this IEnumerable<byte> bytes)
	{
		short[] shorts = new short[bytes.Count() / 2];
		for (int i = 0; i < shorts.Length; i++)
		{
			shorts[i] = BitConverter.ToInt16(bytes.Copy(i * 2, 2L), 0);
		}
		return shorts;
	}

	public static int GetRoundedUpSize(int value)
	{
		int num = value - 1;
		int num2 = num | (num >> 1);
		int num3 = num2 | (num2 >> 2);
		int num4 = num3 | (num3 >> 4);
		int num5 = num4 | (num4 >> 8);
		return (num5 | (num5 >> 16)) + 1;
	}
}
