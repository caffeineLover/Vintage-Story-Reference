using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Vintagestory.API.Datastructures;

public class FastMemoryStream : Stream
{
	private byte[] buffer;

	private int bufferlength;

	private const int MaxLength = 2147483616;

	/// <summary>
	/// When serializing to a buffer, indicates the count of bytes written so far
	/// </summary>
	public override long Position { get; set; }

	/// <summary>
	/// When deserializing from a buffer, this is the full buffer length
	/// </summary>
	public override long Length => bufferlength;

	public override bool CanSeek => false;

	public override bool CanRead => true;

	public override bool CanWrite => true;

	public FastMemoryStream()
	{
		bufferlength = 1024;
		buffer = new byte[1024];
		Position = 0L;
	}

	public FastMemoryStream(int capacity)
	{
		bufferlength = capacity;
		buffer = new byte[capacity];
		Position = 0L;
	}

	public FastMemoryStream(byte[] buffer, int length)
	{
		bufferlength = length;
		this.buffer = buffer;
		Position = 0L;
	}

	public override void SetLength(long value)
	{
		if (value > 2147483616)
		{
			throw new IndexOutOfRangeException("FastMemoryStream limited to 2GB in size");
		}
		bufferlength = Math.Min((int)value, buffer.Length);
	}

	public byte[] ToArray()
	{
		return FastCopy(buffer, bufferlength, (int)Position);
	}

	public byte[] GetBuffer()
	{
		return buffer;
	}

	public override int Read(byte[] destBuffer, int offset, int count)
	{
		long origPosition = Position;
		long bufferlength = this.bufferlength;
		byte[] streamBuffer = buffer;
		for (int i = 0; i < count; i++)
		{
			if (origPosition + i >= bufferlength)
			{
				Position += i;
				return i;
			}
			destBuffer[offset + i] = streamBuffer[origPosition + i];
		}
		Position += count;
		return count;
	}

	public override void Write(byte[] srcBuffer, int srcOffset, int count)
	{
		if (count <= 0)
		{
			return;
		}
		CheckCapacity(count);
		if (count < 128)
		{
			byte[] buffer = this.buffer;
			uint pos = (uint)Position;
			uint i = (uint)srcOffset;
			uint srcLimit = (uint)(srcOffset + count);
			while (i < srcLimit)
			{
				buffer[pos++] = srcBuffer[i++];
			}
		}
		else
		{
			Array.Copy(srcBuffer, srcOffset, this.buffer, (int)Position, count);
		}
		Position += count;
	}

	public void Write(FastMemoryStream src)
	{
		Write(src.buffer, 0, (int)src.Position);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void CheckCapacity(int count)
	{
		if (Position + count <= bufferlength)
		{
			return;
		}
		int newSize = ((bufferlength <= 1073741808) ? (bufferlength * 2) : (Math.Min(bufferlength, 1879048164) + 268435452));
		if (Position + count > newSize)
		{
			if (Position + count > 2147483616)
			{
				throw new IndexOutOfRangeException("FastMemoryStream limited to 2GB in size");
			}
			newSize = (int)Position + count;
		}
		buffer = FastCopy(buffer, (int)Position, newSize);
		bufferlength = newSize;
	}

	public override int ReadByte()
	{
		if (Position >= bufferlength)
		{
			return -1;
		}
		return buffer[Position++];
	}

	public override void WriteByte(byte p)
	{
		CheckCapacity(1);
		buffer[Position++] = p;
	}

	public void WriteTwoBytes(int v)
	{
		CheckCapacity(2);
		buffer[Position] = (byte)(v | 0x80);
		buffer[Position + 1] = (byte)(v >> 7);
		Position += 2L;
	}

	public void WriteThreeBytes(int v)
	{
		CheckCapacity(3);
		buffer[Position] = (byte)(v | 0x80);
		buffer[Position + 1] = (byte)((v >> 7) | 0x80);
		buffer[Position + 2] = (byte)(v >> 14);
		Position += 3L;
	}

	public void WriteUTF8String(string s, int lengthInBytes)
	{
		CheckCapacity(lengthInBytes);
		Position += Encoding.UTF8.GetBytes(s, 0, s.Length, buffer, (int)Position);
	}

	/// <summary>
	/// Used for Protobuf serialization if we need to go back and re-write a varInt value, for example the count of entities written
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="v"></param>
	/// <param name="size"></param>
	/// <exception cref="T:System.Exception"></exception>
	public void WriteAt(int pos, int v, int size)
	{
		if (v < 0 && size != 5)
		{
			throw new Exception("cannot retroactively write a negative number");
		}
		switch (size)
		{
		case 1:
			if (v >= 128)
			{
				throw new Exception("unsupported increase in count while serializing: " + v + " was " + buffer[pos]);
			}
			buffer[pos] = (byte)v;
			break;
		case 2:
			if (v >= 16384)
			{
				throw new Exception("unsupported increase in count while serializing: " + v + " was " + buffer[pos] + " " + buffer[pos + 1]);
			}
			buffer[pos] = (byte)(v | 0x80);
			buffer[pos + 1] = (byte)(v >> 7);
			break;
		case 3:
			if (v >= 2097152)
			{
				throw new Exception("unsupported increase in count while serializing: " + v + " was " + buffer[pos] + " " + buffer[pos + 1] + " " + buffer[pos + 2]);
			}
			buffer[pos] = (byte)(v | 0x80);
			buffer[pos + 1] = (byte)((v >> 7) | 0x80);
			buffer[pos + 2] = (byte)(v >> 14);
			break;
		case 4:
			if (v >= 268435456)
			{
				throw new Exception("unsupported increase in count while serializing: " + v + " was " + buffer[pos] + " " + buffer[pos + 1] + " " + buffer[pos + 2] + " " + buffer[pos + 3]);
			}
			buffer[pos] = (byte)(v | 0x80);
			buffer[pos + 1] = (byte)((v >> 7) | 0x80);
			buffer[pos + 2] = (byte)((v >> 14) | 0x80);
			buffer[pos + 3] = (byte)(v >> 21);
			break;
		case 5:
			buffer[pos] = (byte)(v | 0x80);
			buffer[pos + 1] = (byte)((v >> 7) | 0x80);
			buffer[pos + 2] = (byte)((v >> 14) | 0x80);
			buffer[pos + 3] = (byte)((v >> 21) | 0x80);
			buffer[pos + 4] = (byte)(v >> 28);
			break;
		}
	}

	public void WriteInt32(int v)
	{
		CheckCapacity(4);
		buffer[Position] = (byte)v;
		buffer[Position + 1] = (byte)(v >> 8);
		buffer[Position + 2] = (byte)(v >> 16);
		buffer[Position + 3] = (byte)(v >> 24);
		Position += 4L;
	}

	public void Write(float v)
	{
		CheckCapacity(4);
		BinaryPrimitives.WriteSingleLittleEndian(new Span<byte>(buffer, (int)Position, 4), v);
		Position += 4L;
	}

	public void Write(float[] vv)
	{
		CheckCapacity(4 * vv.Length);
		foreach (float v in vv)
		{
			BinaryPrimitives.WriteSingleLittleEndian(new Span<byte>(buffer, (int)Position, 4), v);
			Position += 4L;
		}
	}

	public override void Write(ReadOnlySpan<byte> inputBuffer)
	{
		int length = inputBuffer.Length;
		CheckCapacity(length);
		Span<byte> streamBuffer = new Span<byte>(buffer, (int)Position, length);
		inputBuffer.CopyTo(streamBuffer);
		Position += length;
	}

	private unsafe static byte[] FastCopy(byte[] buffer, int oldLength, int newSize)
	{
		if (newSize < oldLength)
		{
			oldLength = newSize;
		}
		byte[] array = GC.AllocateUninitializedArray<byte>(newSize);
		fixed (byte* pSrc = buffer)
		{
			fixed (byte* pDst = array)
			{
				Buffer.MemoryCopy(pSrc, pDst, oldLength, oldLength);
			}
		}
		return array;
	}

	public override void Flush()
	{
	}

	public void Reset()
	{
		Position = 0L;
		bufferlength = buffer.Length;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return -1L;
	}

	/// <summary>
	/// Note this moves bytes within the current buffer, without allocating any new byte[] array
	/// </summary>
	/// <param name="newStart"></param>
	public void RemoveFromStart(int newStart)
	{
		int oldLength = (int)Position;
		int newLength = oldLength - newStart;
		if (newLength >= 128)
		{
			Array.Copy(this.buffer, newStart, this.buffer, 0, newLength);
		}
		else
		{
			byte[] buffer = this.buffer;
			uint i = (uint)newStart;
			uint j = 0u;
			if (newLength > 15)
			{
				for (uint srcLimit = (uint)(oldLength - 3); i < buffer.Length && i < srcLimit; i += 4)
				{
					buffer[j] = buffer[i];
					buffer[j + 1] = buffer[i + 1];
					buffer[j + 2] = buffer[i + 2];
					buffer[j + 3] = buffer[i + 3];
					j += 4;
				}
			}
			while (i < oldLength)
			{
				buffer[j++] = buffer[i++];
			}
		}
		Position -= newStart;
	}
}
