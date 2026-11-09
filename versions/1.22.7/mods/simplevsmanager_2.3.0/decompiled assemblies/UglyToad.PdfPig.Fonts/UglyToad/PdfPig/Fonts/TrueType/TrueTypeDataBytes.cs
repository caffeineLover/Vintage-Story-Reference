using System;
using System.Runtime.CompilerServices;
using System.Text;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.TrueType
{
	// Token: 0x02000032 RID: 50
	public class TrueTypeDataBytes
	{
		// Token: 0x06000135 RID: 309 RVA: 0x00007808 File Offset: 0x00005A08
		public TrueTypeDataBytes(byte[] bytes) : this(new ByteArrayInputBytes(bytes))
		{
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00007816 File Offset: 0x00005A16
		public TrueTypeDataBytes(IInputBytes inputBytes)
		{
			if (inputBytes == null)
			{
				throw new ArgumentNullException("inputBytes");
			}
			this.inputBytes = inputBytes;
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00007841 File Offset: 0x00005A41
		public long Position
		{
			get
			{
				return this.inputBytes.CurrentOffset;
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000138 RID: 312 RVA: 0x0000784E File Offset: 0x00005A4E
		public long Length
		{
			get
			{
				return this.inputBytes.Length;
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000785B File Offset: 0x00005A5B
		public float Read32Fixed()
		{
			return (float)this.ReadSignedShort() + (float)this.ReadUnsignedShort() / 65536f;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00007872 File Offset: 0x00005A72
		public short ReadSignedShort()
		{
			this.ReadBuffered(this.internalBuffer, 2);
			return (short)(((int)this.internalBuffer[0] << 8) + (int)this.internalBuffer[1]);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00007896 File Offset: 0x00005A96
		public ushort ReadUnsignedShort()
		{
			this.ReadBuffered(this.internalBuffer, 2);
			return (ushort)(((int)this.internalBuffer[0] << 8) + (int)this.internalBuffer[1]);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000078BA File Offset: 0x00005ABA
		public byte ReadByte()
		{
			this.ReadBuffered(this.internalBuffer, 1);
			return this.internalBuffer[0];
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000078D4 File Offset: 0x00005AD4
		public string ReadTag()
		{
			string result;
			if (this.TryReadString(4, Encoding.UTF8, out result))
			{
				return result;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(42, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Could not read Tag from TrueType file at ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(this.inputBytes.CurrentOffset);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00007934 File Offset: 0x00005B34
		public bool TryReadString(int bytesToRead, Encoding encoding, out string result)
		{
			result = null;
			if (encoding == null)
			{
				return false;
			}
			byte[] array = new byte[bytesToRead];
			if (this.ReadBuffered(array, bytesToRead))
			{
				result = encoding.GetString(array, 0, array.Length);
				return true;
			}
			return false;
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000796A File Offset: 0x00005B6A
		public uint ReadUnsignedInt()
		{
			this.ReadBuffered(this.internalBuffer, 4);
			return (uint)(((ulong)this.internalBuffer[0] << 24) + ((ulong)this.internalBuffer[1] << 16) + (ulong)((long)((long)this.internalBuffer[2] << 8)) + (ulong)((long)this.internalBuffer[3]));
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000079AA File Offset: 0x00005BAA
		public int ReadSignedInt()
		{
			this.ReadBuffered(this.internalBuffer, 4);
			return ((int)this.internalBuffer[0] << 24) + ((int)this.internalBuffer[1] << 16) + ((int)this.internalBuffer[2] << 8) + (int)this.internalBuffer[3];
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000079E8 File Offset: 0x00005BE8
		public long ReadLong()
		{
			long num = (long)this.ReadSignedInt();
			int num2 = this.ReadSignedInt();
			return (num << 32) + ((long)num2 & (long)((ulong)-1));
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00007A0C File Offset: 0x00005C0C
		public DateTime ReadInternationalDate()
		{
			long num = this.ReadLong();
			DateTime dateTime;
			dateTime..ctor(1904, 1, 1, 0, 0, 0, 1);
			DateTime result;
			try
			{
				result = dateTime.AddSeconds((double)num);
			}
			catch (ArgumentOutOfRangeException)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(68, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid date offset (");
				defaultInterpolatedStringHandler.AppendFormatted<long>(num);
				defaultInterpolatedStringHandler.AppendLiteral(" seconds) encountered in TrueType header table.");
				throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return result;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00007A88 File Offset: 0x00005C88
		public void Seek(long position)
		{
			this.inputBytes.Seek(position);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00007A98 File Offset: 0x00005C98
		public int ReadSignedByte()
		{
			this.ReadBuffered(this.internalBuffer, 1);
			byte b = this.internalBuffer[0];
			if (b >= 127)
			{
				return (int)b - 256;
			}
			return (int)b;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00007ACC File Offset: 0x00005CCC
		public ushort[] ReadUnsignedShortArray(int length)
		{
			ushort[] array = new ushort[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = this.ReadUnsignedShort();
			}
			return array;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00007AF8 File Offset: 0x00005CF8
		public byte[] ReadByteArray(int length)
		{
			byte[] array = new byte[length];
			this.ReadBuffered(array, length);
			return array;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00007B18 File Offset: 0x00005D18
		public uint[] ReadUnsignedIntArray(int length)
		{
			uint[] array = new uint[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = this.ReadUnsignedInt();
			}
			return array;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00007B44 File Offset: 0x00005D44
		public short[] ReadShortArray(int length)
		{
			short[] array = new short[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = this.ReadSignedShort();
			}
			return array;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00007B70 File Offset: 0x00005D70
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(14, 2);
			defaultInterpolatedStringHandler.AppendLiteral("@: ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(this.Position);
			defaultInterpolatedStringHandler.AppendLiteral(" of ");
			defaultInterpolatedStringHandler.AppendFormatted<long>(this.inputBytes.Length);
			defaultInterpolatedStringHandler.AppendLiteral(" bytes.");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00007BD1 File Offset: 0x00005DD1
		private bool ReadBuffered(byte[] buffer, int length)
		{
			return this.inputBytes.Read(buffer, new int?(length)) >= length;
		}

		// Token: 0x04000110 RID: 272
		private readonly byte[] internalBuffer = new byte[16];

		// Token: 0x04000111 RID: 273
		private readonly IInputBytes inputBytes;
	}
}
