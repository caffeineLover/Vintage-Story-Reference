using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x02000083 RID: 131
	public class CompactFontFormatData
	{
		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x00015194 File Offset: 0x00013394
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x0001519C File Offset: 0x0001339C
		public int Position { get; private set; } = -1;

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x000151A5 File Offset: 0x000133A5
		public int Length
		{
			get
			{
				return this.dataBytes.Count;
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x000151B2 File Offset: 0x000133B2
		[DebuggerStepThrough]
		public CompactFontFormatData(IReadOnlyList<byte> dataBytes)
		{
			this.dataBytes = dataBytes;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x000151C8 File Offset: 0x000133C8
		public string ReadString(int length, Encoding encoding)
		{
			byte[] array = new byte[length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.ReadByte();
			}
			return encoding.GetString(array);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x000151FA File Offset: 0x000133FA
		public byte ReadCard8()
		{
			return this.ReadByte();
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00015202 File Offset: 0x00013402
		public ushort ReadCard16()
		{
			return (ushort)((int)this.ReadByte() << 8 | (int)this.ReadByte());
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00015214 File Offset: 0x00013414
		public byte ReadOffsize()
		{
			return this.ReadByte();
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0001521C File Offset: 0x0001341C
		public int ReadOffset(int offsetSize)
		{
			int num = 0;
			for (int i = 0; i < offsetSize; i++)
			{
				num = (num << 8 | (int)this.ReadByte());
			}
			return num;
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00015244 File Offset: 0x00013444
		public byte ReadByte()
		{
			int position = this.Position;
			this.Position = position + 1;
			if (this.Position >= this.dataBytes.Count)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(63, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot read byte at position ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Position);
				defaultInterpolatedStringHandler.AppendLiteral(" of an array which is ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.dataBytes.Count);
				defaultInterpolatedStringHandler.AppendLiteral(" bytes long.");
				throw new IndexOutOfRangeException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return this.dataBytes[this.Position];
		}

		// Token: 0x060002FE RID: 766 RVA: 0x000152DF File Offset: 0x000134DF
		public byte Peek()
		{
			return this.dataBytes[this.Position + 1];
		}

		// Token: 0x060002FF RID: 767 RVA: 0x000152F4 File Offset: 0x000134F4
		public bool CanRead()
		{
			return this.Position < this.dataBytes.Count - 1;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0001530B File Offset: 0x0001350B
		public void Seek(int offset)
		{
			this.Position = offset - 1;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00015316 File Offset: 0x00013516
		public long ReadLong()
		{
			return (long)((int)this.ReadCard16() << 16 | (int)this.ReadCard16());
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00015329 File Offset: 0x00013529
		public int ReadSid()
		{
			return (int)this.ReadByte() << 8 | (int)this.ReadByte();
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0001533C File Offset: 0x0001353C
		public byte[] ReadBytes(int length)
		{
			byte[] array = new byte[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = this.ReadByte();
			}
			return array;
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00015368 File Offset: 0x00013568
		public CompactFontFormatData SnapshotPortion(int startLocation, int length)
		{
			if (length == 0)
			{
				return new CompactFontFormatData(EmptyArray<byte>.Instance);
			}
			if (startLocation > this.dataBytes.Count - 1 || startLocation + length > this.dataBytes.Count)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(120, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Attempted to create a snapshot of an invalid portion of the data. Length was ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.dataBytes.Count);
				defaultInterpolatedStringHandler.AppendLiteral(", requested start: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(startLocation);
				defaultInterpolatedStringHandler.AppendLiteral(" and requested length: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(length);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			byte[] array = new byte[length];
			int num = 0;
			for (int i = startLocation; i < startLocation + length; i++)
			{
				array[num] = this.dataBytes[i];
				num++;
			}
			return new CompactFontFormatData(array);
		}

		// Token: 0x04000301 RID: 769
		private readonly IReadOnlyList<byte> dataBytes;
	}
}
