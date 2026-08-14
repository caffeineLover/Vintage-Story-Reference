using System;
using System.Text;
using Vintagestory.API.Common;

public class CitoMemoryStream : CitoStream
{
	private const int byteHighestBit = 128;

	private byte[] buffer_;

	private int bufferlength;

	private int position_;

	private readonly BoxedArray ba;

	public CitoMemoryStream()
	{
		bufferlength = 16;
		buffer_ = new byte[16];
		position_ = 0;
	}

	public CitoMemoryStream(BoxedArray reusableBuffer)
	{
		ba = reusableBuffer.CheckCreated();
		bufferlength = ba.buffer.Length;
		buffer_ = ba.buffer;
		position_ = 0;
	}

	public CitoMemoryStream(byte[] buffer, int length)
	{
		bufferlength = length;
		buffer_ = buffer;
		position_ = 0;
	}

	public override int Position()
	{
		return position_;
	}

	internal int GetLength()
	{
		return bufferlength;
	}

	internal void SetLength(int value)
	{
		bufferlength = Math.Min(value, buffer_.Length);
	}

	public byte[] ToArray()
	{
		return buffer_;
	}

	public byte[] GetBuffer()
	{
		return buffer_;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		int ourOffset = position_;
		int maxCount = bufferlength - ourOffset;
		if (count > maxCount)
		{
			count = maxCount;
		}
		byte[] ourBuffer = buffer_;
		for (int i = 0; i < count; i++)
		{
			buffer[offset + i] = ourBuffer[ourOffset + i];
		}
		position_ = ourOffset + count;
		return count;
	}

	public override bool CanSeek()
	{
		return false;
	}

	public override void Seek(int length, CitoSeekOrigin seekOrigin)
	{
		if (seekOrigin == CitoSeekOrigin.Current)
		{
			position_ += length;
		}
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (count <= 0)
		{
			return;
		}
		EnsureCapacityFor(count);
		if (count > 200)
		{
			Array.Copy(buffer, offset, buffer_, position_, count);
			position_ += count;
			return;
		}
		int i = offset;
		count += offset;
		int dWordBoundary = (i + 3) / 4 * 4;
		byte[] ourBuffer = buffer_;
		int ourPosition = position_;
		while (i < dWordBoundary)
		{
			ourBuffer[ourPosition++] = buffer[i++];
		}
		int fastLoopLimit = count / 4 * 4;
		for (i += 3; i < buffer.Length && i < fastLoopLimit; i += 4)
		{
			ourBuffer[ourPosition] = buffer[i - 3];
			ourBuffer[ourPosition + 1] = buffer[i - 2];
			ourBuffer[ourPosition + 2] = buffer[i - 1];
			ourBuffer[ourPosition + 3] = buffer[i];
			ourPosition += 4;
		}
		i -= 3;
		while (i < count)
		{
			ourBuffer[ourPosition++] = buffer[i++];
		}
		position_ = ourPosition;
	}

	private void EnsureCapacityFor(int count)
	{
		if (position_ + count > bufferlength)
		{
			int newSize = bufferlength * 2;
			if (newSize < position_ + count)
			{
				newSize = position_ + count;
			}
			buffer_ = FastCopy(buffer_, position_, newSize);
			bufferlength = newSize;
		}
	}

	public override void Seek_(int p, CitoSeekOrigin seekOrigin)
	{
	}

	public override int ReadByte()
	{
		if (position_ >= bufferlength)
		{
			return -1;
		}
		return buffer_[position_++];
	}

	public override void WriteByte(byte p)
	{
		if (position_ >= bufferlength)
		{
			buffer_ = FastCopy(buffer_, position_, bufferlength *= 2);
		}
		buffer_[position_++] = p;
	}

	public override void WriteSmallInt(int v)
	{
		if (v < 128)
		{
			WriteByte((byte)v);
			return;
		}
		if (position_ >= bufferlength - 1)
		{
			buffer_ = FastCopy(buffer_, position_, bufferlength *= 2);
		}
		buffer_[position_++] = (byte)(v | 0x80);
		buffer_[position_++] = (byte)(v >> 7);
	}

	public override void WriteKey(byte field, byte wiretype)
	{
		WriteSmallInt(new Key(field, wiretype));
	}

	private byte[] FastCopy(byte[] buffer, int oldLength, int newSize)
	{
		byte[] buffer2 = new byte[newSize];
		if (oldLength > 256)
		{
			Array.Copy(buffer, 0, buffer2, 0, oldLength);
		}
		else
		{
			int i = 0;
			if (oldLength >= 4)
			{
				int fastLoopLength = oldLength / 4 * 4;
				for (i = 3; i < buffer.Length && i < fastLoopLength; i += 4)
				{
					buffer2[i - 3] = buffer[i - 3];
					buffer2[i - 2] = buffer[i - 2];
					buffer2[i - 1] = buffer[i - 1];
					buffer2[i] = buffer[i];
				}
				i -= 3;
			}
			for (; i < oldLength; i++)
			{
				buffer2[i] = buffer[i];
			}
		}
		if (ba != null)
		{
			ba.buffer = buffer2;
		}
		return buffer2;
	}

	public override string ReadString(int byteCount)
	{
		string result = Encoding.UTF8.GetString(buffer_, position_, byteCount);
		position_ += byteCount;
		return result;
	}

	public override void WriteString(string s, int byteCount)
	{
		EnsureCapacityFor(byteCount);
		position_ += Encoding.UTF8.GetBytes(s, 0, s.Length, buffer_, position_);
	}

	public static void NetworkTest(ILogger Logger)
	{
		int a = int.MaxValue;
		int b = int.MinValue;
		int ba = 129;
		int bb = 2000000;
		int bc = -2000000;
		int c = -1;
		uint d = uint.MaxValue;
		long e = long.MaxValue;
		long ea = 129L;
		long eb = 2000000L;
		long ec = -2000000L;
		long f = long.MinValue;
		long g = -1L;
		ulong h = ulong.MaxValue;
		CitoMemoryStream ms = new CitoMemoryStream();
		int position = ms.Position();
		bool sizesOK = true;
		ProtocolParser.WriteUInt32(ms, a);
		if (ms.Position() - position != ProtocolParser.GetSize(a))
		{
			sizesOK = false;
			Logger.Notification("wrongsize a");
		}
		position = ms.Position();
		ProtocolParser.WriteUInt32(ms, b);
		if (ms.Position() - position != ProtocolParser.GetSize(b))
		{
			sizesOK = false;
			Logger.Notification("wrongsize b");
		}
		position = ms.Position();
		ProtocolParser.WriteUInt32(ms, ba);
		if (ms.Position() - position != ProtocolParser.GetSize(ba))
		{
			sizesOK = false;
			Logger.Notification("wrongsize ba");
		}
		position = ms.Position();
		ProtocolParser.WriteUInt32(ms, bb);
		if (ms.Position() - position != ProtocolParser.GetSize(bb))
		{
			sizesOK = false;
			Logger.Notification("wrongsize bb");
		}
		position = ms.Position();
		ProtocolParser.WriteUInt32(ms, bc);
		if (ms.Position() - position != ProtocolParser.GetSize(bc))
		{
			sizesOK = false;
			Logger.Notification("wrongsize bc");
		}
		position = ms.Position();
		ProtocolParser.WriteUInt32(ms, c);
		if (ms.Position() - position != ProtocolParser.GetSize(c))
		{
			sizesOK = false;
			Logger.Notification("wrongsize c");
		}
		position = ms.Position();
		ProtocolParser.WriteUInt32(ms, (int)d);
		if (ms.Position() - position != ProtocolParser.GetSize((int)d))
		{
			sizesOK = false;
			Logger.Notification("wrongsize d");
		}
		position = ms.Position();
		ProtocolParser.WriteUInt64(ms, e);
		if (ms.Position() - position != ProtocolParser.GetSize(e))
		{
			sizesOK = false;
			Logger.Notification("wrongsize e");
		}
		position = ms.Position();
		ProtocolParser.WriteUInt64(ms, ea);
		if (ms.Position() - position != ProtocolParser.GetSize(ea))
		{
			sizesOK = false;
			Logger.Notification("wrongsize ea");
		}
		position = ms.Position();
		ProtocolParser.WriteUInt64(ms, eb);
		if (ms.Position() - position != ProtocolParser.GetSize(eb))
		{
			sizesOK = false;
			Logger.Notification("wrongsize eb");
		}
		position = ms.Position();
		ProtocolParser.WriteUInt64(ms, ec);
		if (ms.Position() - position != ProtocolParser.GetSize(ec))
		{
			sizesOK = false;
			Logger.Notification("wrongsize ec");
		}
		position = ms.Position();
		ProtocolParser.WriteUInt64(ms, f);
		if (ms.Position() - position != ProtocolParser.GetSize(f))
		{
			sizesOK = false;
			Logger.Notification("wrongsize f");
		}
		position = ms.Position();
		ProtocolParser.WriteUInt64(ms, g);
		if (ms.Position() - position != ProtocolParser.GetSize(g))
		{
			sizesOK = false;
			Logger.Notification("wrongsize g");
		}
		position = ms.Position();
		ProtocolParser.WriteUInt64(ms, (long)h);
		if (ms.Position() - position != ProtocolParser.GetSize((long)h))
		{
			sizesOK = false;
			Logger.Notification("wrongsize h");
		}
		position = ms.Position();
		ProtocolParser.WriteString(ms, "testString");
		if (ms.Position() - position != ProtocolParser.GetSize("testString"))
		{
			sizesOK = false;
			Logger.Notification("wrongsize string");
		}
		ms.position_ = 0;
		Logger.Notification("Test positive int.   Wrote " + a + "  Read " + ProtocolParser.ReadUInt32(ms));
		Logger.Notification("Test negative int.   Wrote " + b + "  Read " + ProtocolParser.ReadUInt32(ms));
		Logger.Notification("Test positive int.   Wrote " + ba + "  Read " + ProtocolParser.ReadUInt32(ms));
		Logger.Notification("Test positive int.   Wrote " + bb + "  Read " + ProtocolParser.ReadUInt32(ms));
		Logger.Notification("Test negative int.   Wrote " + bc + "  Read " + ProtocolParser.ReadUInt32(ms));
		Logger.Notification("Test negative int.   Wrote " + c + "  Read " + ProtocolParser.ReadUInt32(ms));
		Logger.Notification("Test unsigned uint.  Wrote " + d + "  Read " + (uint)ProtocolParser.ReadUInt32(ms));
		Logger.Notification("Test positive long.  Wrote " + e + "  Read " + ProtocolParser.ReadUInt64(ms));
		Logger.Notification("Test positive long.  Wrote " + ea + "  Read " + ProtocolParser.ReadUInt64(ms));
		Logger.Notification("Test positive long.  Wrote " + eb + "  Read " + ProtocolParser.ReadUInt64(ms));
		Logger.Notification("Test negative long.  Wrote " + ec + "  Read " + ProtocolParser.ReadUInt64(ms));
		Logger.Notification("Test negative long.  Wrote " + f + "  Read " + ProtocolParser.ReadUInt64(ms));
		Logger.Notification("Test negative long.  Wrote " + g + "  Read " + ProtocolParser.ReadUInt64(ms));
		Logger.Notification("Test unsigned ulong.  Wrote " + h + "  Read " + (ulong)ProtocolParser.ReadUInt64(ms));
		Logger.Notification("Test string.  Wrote 'testString'  Read '" + ProtocolParser.ReadString(ms) + "'");
		Logger.Notification(sizesOK ? "All sizes were OK" : "Size error!");
	}
}
