using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.IO
{
	// Token: 0x020000E6 RID: 230
	internal class RandomAccessBuffer : IDisposable
	{
		// Token: 0x06000442 RID: 1090 RVA: 0x0001A4D9 File Offset: 0x000186D9
		public RandomAccessBuffer() : this(RandomAccessBuffer.DefaultChunkSize)
		{
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0001A4E8 File Offset: 0x000186E8
		private RandomAccessBuffer(int definedChunkSize)
		{
			this.chunkSize = RandomAccessBuffer.DefaultChunkSize;
			base..ctor();
			this.bufferList = new List<byte[]>();
			this.chunkSize = definedChunkSize;
			this.currentBuffer = new byte[this.chunkSize];
			this.bufferList.Add(this.currentBuffer);
			this.pointer = 0L;
			this.currentBufferPointer = 0;
			this.size = 0L;
			this.bufferListIndex = 0;
			this.bufferListMaxIndex = 0;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0001A560 File Offset: 0x00018760
		public RandomAccessBuffer(byte[] input)
		{
			this.chunkSize = RandomAccessBuffer.DefaultChunkSize;
			base..ctor();
			this.bufferList = new List<byte[]>(1);
			this.chunkSize = input.Length;
			this.currentBuffer = input;
			this.bufferList.Add(this.currentBuffer);
			this.pointer = 0L;
			this.currentBufferPointer = 0;
			this.size = (long)this.chunkSize;
			this.bufferListIndex = 0;
			this.bufferListMaxIndex = 0;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0001A5D8 File Offset: 0x000187D8
		public RandomAccessBuffer(BinaryReader input)
		{
			this.chunkSize = RandomAccessBuffer.DefaultChunkSize;
			base..ctor();
			byte[] array = new byte[8192];
			int length;
			while ((length = input.Read(array, 0, 8192)) > -1)
			{
				this.write(array, 0, length);
			}
			this.Seek(0L);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0001A628 File Offset: 0x00018828
		public RandomAccessBuffer Clone()
		{
			RandomAccessBuffer randomAccessBuffer = new RandomAccessBuffer(this.chunkSize)
			{
				bufferList = new List<byte[]>(this.bufferList.Count)
			};
			foreach (byte[] array in this.bufferList)
			{
				byte[] array2 = new byte[array.Length];
				Array.Copy(array, 0, array2, 0, array.Length);
				randomAccessBuffer.bufferList.Add(array2);
			}
			if (this.currentBuffer != null)
			{
				randomAccessBuffer.currentBuffer = randomAccessBuffer.bufferList[randomAccessBuffer.bufferList.Count - 1];
			}
			else
			{
				randomAccessBuffer.currentBuffer = null;
			}
			randomAccessBuffer.pointer = this.pointer;
			randomAccessBuffer.currentBufferPointer = this.currentBufferPointer;
			randomAccessBuffer.size = this.size;
			randomAccessBuffer.bufferListIndex = this.bufferListIndex;
			randomAccessBuffer.bufferListMaxIndex = this.bufferListMaxIndex;
			return randomAccessBuffer;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0001A724 File Offset: 0x00018924
		public void Dispose()
		{
			this.currentBuffer = null;
			this.bufferList.Clear();
			this.pointer = 0L;
			this.currentBufferPointer = 0;
			this.size = 0L;
			this.bufferListIndex = 0;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x0001A758 File Offset: 0x00018958
		public void clear()
		{
			this.bufferList.Clear();
			this.currentBuffer = new byte[this.chunkSize];
			this.bufferList.Add(this.currentBuffer);
			this.pointer = 0L;
			this.currentBufferPointer = 0;
			this.size = 0L;
			this.bufferListIndex = 0;
			this.bufferListMaxIndex = 0;
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0001A7B8 File Offset: 0x000189B8
		public void Seek(long position)
		{
			this.CheckClosed();
			if (position < 0L)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(17, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid position ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(position);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.pointer = position;
			if (this.pointer < this.size)
			{
				this.bufferListIndex = (int)(this.pointer / (long)this.chunkSize);
				this.currentBufferPointer = (int)(this.pointer % (long)this.chunkSize);
				this.currentBuffer = this.bufferList[this.bufferListIndex];
				return;
			}
			this.bufferListIndex = this.bufferListMaxIndex;
			this.currentBuffer = this.bufferList[this.bufferListIndex];
			this.currentBufferPointer = (int)(this.size % (long)this.chunkSize);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0001A88A File Offset: 0x00018A8A
		public long GetPosition()
		{
			this.CheckClosed();
			return this.pointer;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0001A898 File Offset: 0x00018A98
		public int Read()
		{
			this.CheckClosed();
			if (this.pointer >= this.size)
			{
				return -1;
			}
			int num;
			if (this.currentBufferPointer >= this.chunkSize)
			{
				if (this.bufferListIndex >= this.bufferListMaxIndex)
				{
					return -1;
				}
				List<byte[]> list = this.bufferList;
				num = this.bufferListIndex + 1;
				this.bufferListIndex = num;
				this.currentBuffer = list[num];
				this.currentBufferPointer = 0;
			}
			this.pointer += 1L;
			byte[] array = this.currentBuffer;
			num = this.currentBufferPointer;
			this.currentBufferPointer = num + 1;
			return array[num] & 255;
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0001A930 File Offset: 0x00018B30
		public int Read(byte[] b, int offset, int length)
		{
			this.CheckClosed();
			if (this.pointer >= this.size)
			{
				return 0;
			}
			int num = this.ReadRemainingBytes(b, offset, length);
			while (num < length && this.Available() > 0)
			{
				num += this.ReadRemainingBytes(b, offset + num, length - num);
				if (this.currentBufferPointer == this.chunkSize)
				{
					this.NextBuffer();
				}
			}
			return num;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0001A994 File Offset: 0x00018B94
		private int ReadRemainingBytes(byte[] b, int offset, int length)
		{
			if (this.pointer >= this.size)
			{
				return 0;
			}
			int num = (int)Math.Min((long)length, this.size - this.pointer);
			int num2 = this.chunkSize - this.currentBufferPointer;
			if (num2 == 0)
			{
				return 0;
			}
			if (num >= num2)
			{
				Array.Copy(this.currentBuffer, this.currentBufferPointer, b, offset, num2);
				this.currentBufferPointer += num2;
				this.pointer += (long)num2;
				return num2;
			}
			Array.Copy(this.currentBuffer, this.currentBufferPointer, b, offset, num);
			this.currentBufferPointer += num;
			this.pointer += (long)num;
			return num;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0001AA43 File Offset: 0x00018C43
		public long Length()
		{
			this.CheckClosed();
			return this.size;
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0001AA54 File Offset: 0x00018C54
		public void write(int b)
		{
			this.CheckClosed();
			if (this.currentBufferPointer >= this.chunkSize)
			{
				if (this.pointer + (long)this.chunkSize >= 2147483647L)
				{
					throw new OutOfMemoryException("RandomAccessBuffer overflow");
				}
				this.ExpandBuffer();
			}
			byte[] array = this.currentBuffer;
			int num = this.currentBufferPointer;
			this.currentBufferPointer = num + 1;
			array[num] = (byte)b;
			this.pointer += 1L;
			if (this.pointer > this.size)
			{
				this.size = this.pointer;
			}
			if (this.currentBufferPointer >= this.chunkSize)
			{
				if (this.pointer + (long)this.chunkSize >= 2147483647L)
				{
					throw new OutOfMemoryException("RandomAccessBuffer overflow");
				}
				this.ExpandBuffer();
			}
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0001AB14 File Offset: 0x00018D14
		public void write(byte[] b)
		{
			this.write(b, 0, b.Length);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0001AB24 File Offset: 0x00018D24
		public void write(byte[] b, int offset, int length)
		{
			this.CheckClosed();
			long num = this.pointer + (long)length;
			int num2 = this.chunkSize - this.currentBufferPointer;
			if (length >= num2)
			{
				if (num > 2147483647L)
				{
					throw new OutOfMemoryException("RandomAccessBuffer overflow");
				}
				Array.Copy(b, offset, this.currentBuffer, this.currentBufferPointer, num2);
				int num3 = offset + num2;
				long num4 = (long)(length - num2);
				int num5 = (int)num4 / this.chunkSize;
				for (int i = 0; i < num5; i++)
				{
					this.ExpandBuffer();
					Array.Copy(b, num3, this.currentBuffer, this.currentBufferPointer, this.chunkSize);
					num3 += this.chunkSize;
				}
				num4 -= (long)num5 * (long)this.chunkSize;
				if (num4 >= 0L)
				{
					this.ExpandBuffer();
					if (num4 > 0L)
					{
						Array.Copy(b, num3, this.currentBuffer, this.currentBufferPointer, (int)num4);
					}
					this.currentBufferPointer = (int)num4;
				}
			}
			else
			{
				Array.Copy(b, offset, this.currentBuffer, this.currentBufferPointer, length);
				this.currentBufferPointer += length;
			}
			this.pointer += (long)length;
			if (this.pointer > this.size)
			{
				this.size = this.pointer;
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0001AC54 File Offset: 0x00018E54
		private void ExpandBuffer()
		{
			if (this.bufferListMaxIndex > this.bufferListIndex)
			{
				this.NextBuffer();
				return;
			}
			this.currentBuffer = new byte[this.chunkSize];
			this.bufferList.Add(this.currentBuffer);
			this.currentBufferPointer = 0;
			this.bufferListMaxIndex++;
			this.bufferListIndex++;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0001ACBC File Offset: 0x00018EBC
		private void NextBuffer()
		{
			if (this.bufferListIndex == this.bufferListMaxIndex)
			{
				throw new InvalidOperationException("No more chunks available, end of buffer reached");
			}
			this.currentBufferPointer = 0;
			List<byte[]> list = this.bufferList;
			int num = this.bufferListIndex + 1;
			this.bufferListIndex = num;
			this.currentBuffer = list[num];
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0001AD0B File Offset: 0x00018F0B
		private void CheckClosed()
		{
			if (this.currentBuffer == null)
			{
				throw new ObjectDisposedException("RandomAccessBuffer already closed");
			}
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0001AD20 File Offset: 0x00018F20
		public bool IsClosed()
		{
			return this.currentBuffer == null;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0001AD2B File Offset: 0x00018F2B
		public bool IsEof()
		{
			this.CheckClosed();
			return this.pointer >= this.size;
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0001AD44 File Offset: 0x00018F44
		public int Available()
		{
			return (int)Math.Min(this.Length() - this.GetPosition(), 2147483647L);
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0001AD5F File Offset: 0x00018F5F
		public void ReturnToBeginning()
		{
			this.Seek(0L);
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x0001AD69 File Offset: 0x00018F69
		public int Peek()
		{
			int num = this.Read();
			if (num != -1)
			{
				this.Rewind(1);
			}
			return num;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0001AD7C File Offset: 0x00018F7C
		public void Rewind(int bytes)
		{
			this.CheckClosed();
			this.Seek(this.GetPosition() - (long)bytes);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0001AD94 File Offset: 0x00018F94
		public byte[] ReadFully(int length)
		{
			byte[] array = new byte[length];
			for (int i = this.Read(array); i < length; i += this.Read(array, i, length - i))
			{
			}
			return array;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0001ADC5 File Offset: 0x00018FC5
		public int Read(byte[] b)
		{
			return this.Read(b, 0, b.Length);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0001ADD2 File Offset: 0x00018FD2
		public void Unread(int b)
		{
			this.Rewind(1);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001ADDB File Offset: 0x00018FDB
		public void Unread(byte[] bytes)
		{
			this.Rewind(bytes.Length);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0001ADE6 File Offset: 0x00018FE6
		public void Unread(byte[] bytes, int start, int len)
		{
			this.Rewind(len - start);
		}

		// Token: 0x0400022B RID: 555
		private static readonly int DefaultChunkSize = 1024;

		// Token: 0x0400022C RID: 556
		private int chunkSize;

		// Token: 0x0400022D RID: 557
		private List<byte[]> bufferList;

		// Token: 0x0400022E RID: 558
		private byte[] currentBuffer;

		// Token: 0x0400022F RID: 559
		private long pointer;

		// Token: 0x04000230 RID: 560
		private int currentBufferPointer;

		// Token: 0x04000231 RID: 561
		private long size;

		// Token: 0x04000232 RID: 562
		private int bufferListIndex;

		// Token: 0x04000233 RID: 563
		private int bufferListMaxIndex;
	}
}
