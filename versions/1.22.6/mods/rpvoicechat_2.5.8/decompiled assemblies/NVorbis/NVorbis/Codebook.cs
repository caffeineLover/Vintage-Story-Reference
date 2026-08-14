using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NVorbis.Contracts;

namespace NVorbis
{
	// Token: 0x02000002 RID: 2
	internal class Codebook : ICodebook
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public void Init(IPacket packet, IHuffman huffman)
		{
			if (packet.ReadBits(24) != 5653314UL)
			{
				throw new InvalidDataException("Book header had invalid signature!");
			}
			this.Dimensions = (int)packet.ReadBits(16);
			this.Entries = (int)packet.ReadBits(24);
			this._lengths = new int[this.Entries];
			this.InitTree(packet, huffman);
			this.InitLookupTable(packet);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020B8 File Offset: 0x000002B8
		private void InitTree(IPacket packet, IHuffman huffman)
		{
			int num = 0;
			bool flag;
			int num4;
			if (packet.ReadBit())
			{
				int num2 = (int)packet.ReadBits(5) + 1;
				int i = 0;
				while (i < this.Entries)
				{
					int num3 = (int)packet.ReadBits(Utils.ilog(this.Entries - i));
					while (--num3 >= 0)
					{
						this._lengths[i++] = num2;
					}
					num2++;
				}
				num = 0;
				flag = false;
				num4 = num2;
			}
			else
			{
				num4 = -1;
				flag = packet.ReadBit();
				for (int j = 0; j < this.Entries; j++)
				{
					if (!flag || packet.ReadBit())
					{
						this._lengths[j] = (int)packet.ReadBits(5) + 1;
						num++;
					}
					else
					{
						this._lengths[j] = -1;
					}
					if (this._lengths[j] > num4)
					{
						num4 = this._lengths[j];
					}
				}
			}
			if ((this._maxBits = num4) > -1)
			{
				int[] array = null;
				if (flag && num >= this.Entries >> 2)
				{
					array = new int[this.Entries];
					Array.Copy(this._lengths, array, this.Entries);
					flag = false;
				}
				int num5;
				if (flag)
				{
					num5 = num;
				}
				else
				{
					num5 = 0;
				}
				int[] array2 = null;
				int[] array3 = null;
				if (!flag)
				{
					array3 = new int[this.Entries];
				}
				else if (num5 != 0)
				{
					array = new int[num5];
					array3 = new int[num5];
					array2 = new int[num5];
				}
				if (!this.ComputeCodewords(flag, array3, array, this._lengths, this.Entries, array2))
				{
					throw new InvalidDataException();
				}
				IReadOnlyList<int> readOnlyList = array2;
				IReadOnlyList<int> value = readOnlyList ?? Codebook.FastRange.Get(0, array3.Length);
				huffman.GenerateTable(value, array ?? this._lengths, array3);
				this._prefixList = huffman.PrefixTree;
				this._prefixBitLength = huffman.TableBits;
				this._overflowList = huffman.OverflowList;
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002284 File Offset: 0x00000484
		private bool ComputeCodewords(bool sparse, int[] codewords, int[] codewordLengths, int[] len, int n, int[] values)
		{
			int num = 0;
			uint[] array = new uint[32];
			int num2 = 0;
			while (num2 < n && len[num2] <= 0)
			{
				num2++;
			}
			if (num2 == n)
			{
				return true;
			}
			this.AddEntry(sparse, codewords, codewordLengths, 0U, num2, num++, len[num2], values);
			for (int i = 1; i <= len[num2]; i++)
			{
				array[i] = 1U << 32 - i;
			}
			for (int i = num2 + 1; i < n; i++)
			{
				int num3 = len[i];
				if (num3 > 0)
				{
					while (num3 > 0 && array[num3] == 0U)
					{
						num3--;
					}
					if (num3 == 0)
					{
						return false;
					}
					uint num4 = array[num3];
					array[num3] = 0U;
					this.AddEntry(sparse, codewords, codewordLengths, Utils.BitReverse(num4), i, num++, len[i], values);
					if (num3 != len[i])
					{
						for (int j = len[i]; j > num3; j--)
						{
							array[j] = num4 + (1U << 32 - j);
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002372 File Offset: 0x00000572
		private void AddEntry(bool sparse, int[] codewords, int[] codewordLengths, uint huffCode, int symbol, int count, int len, int[] values)
		{
			if (sparse)
			{
				codewords[count] = (int)huffCode;
				codewordLengths[count] = len;
				values[count] = symbol;
				return;
			}
			codewords[symbol] = (int)huffCode;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002394 File Offset: 0x00000594
		private void InitLookupTable(IPacket packet)
		{
			this.MapType = (int)packet.ReadBits(4);
			if (this.MapType == 0)
			{
				return;
			}
			float num = Utils.ConvertFromVorbisFloat32((uint)packet.ReadBits(32));
			float num2 = Utils.ConvertFromVorbisFloat32((uint)packet.ReadBits(32));
			int count = (int)packet.ReadBits(4) + 1;
			bool flag = packet.ReadBit();
			int num3 = this.Entries * this.Dimensions;
			float[] array = new float[num3];
			if (this.MapType == 1)
			{
				num3 = this.lookup1_values();
			}
			uint[] array2 = new uint[num3];
			for (int i = 0; i < num3; i++)
			{
				array2[i] = (uint)packet.ReadBits(count);
			}
			if (this.MapType == 1)
			{
				for (int j = 0; j < this.Entries; j++)
				{
					double num4 = 0.0;
					int num5 = 1;
					for (int k = 0; k < this.Dimensions; k++)
					{
						int num6 = j / num5 % num3;
						double num7 = (double)(array2[num6] * num2 + num) + num4;
						array[j * this.Dimensions + k] = (float)num7;
						if (flag)
						{
							num4 = num7;
						}
						num5 *= num3;
					}
				}
			}
			else
			{
				for (int l = 0; l < this.Entries; l++)
				{
					double num8 = 0.0;
					int num9 = l * this.Dimensions;
					for (int m = 0; m < this.Dimensions; m++)
					{
						double num10 = (double)(array2[num9] * num2 + num) + num8;
						array[l * this.Dimensions + m] = (float)num10;
						if (flag)
						{
							num8 = num10;
						}
						num9++;
					}
				}
			}
			this._lookupTable = array;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002530 File Offset: 0x00000730
		private int lookup1_values()
		{
			int num = (int)Math.Floor(Math.Exp(Math.Log((double)this.Entries) / (double)this.Dimensions));
			if (Math.Floor(Math.Pow((double)(num + 1), (double)this.Dimensions)) <= (double)this.Entries)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002580 File Offset: 0x00000780
		public int DecodeScalar(IPacket packet)
		{
			int num2;
			int num = (int)packet.TryPeekBits(this._prefixBitLength, out num2);
			if (num2 == 0)
			{
				return -1;
			}
			HuffmanListNode huffmanListNode = this._prefixList[num];
			if (huffmanListNode != null)
			{
				packet.SkipBits(huffmanListNode.Length);
				return huffmanListNode.Value;
			}
			int num3;
			num = (int)packet.TryPeekBits(this._maxBits, out num3);
			for (int i = 0; i < this._overflowList.Count; i++)
			{
				huffmanListNode = this._overflowList[i];
				if (huffmanListNode.Bits == (num & huffmanListNode.Mask))
				{
					packet.SkipBits(huffmanListNode.Length);
					return huffmanListNode.Value;
				}
			}
			return -1;
		}

		// Token: 0x17000001 RID: 1
		public float this[int entry, int dim]
		{
			get
			{
				return this._lookupTable[entry * this.Dimensions + dim];
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002634 File Offset: 0x00000834
		// (set) Token: 0x0600000A RID: 10 RVA: 0x0000263C File Offset: 0x0000083C
		public int Dimensions { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002645 File Offset: 0x00000845
		// (set) Token: 0x0600000C RID: 12 RVA: 0x0000264D File Offset: 0x0000084D
		public int Entries { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002656 File Offset: 0x00000856
		// (set) Token: 0x0600000E RID: 14 RVA: 0x0000265E File Offset: 0x0000085E
		public int MapType { get; private set; }

		// Token: 0x04000001 RID: 1
		private int[] _lengths;

		// Token: 0x04000002 RID: 2
		private float[] _lookupTable;

		// Token: 0x04000003 RID: 3
		private IReadOnlyList<HuffmanListNode> _overflowList;

		// Token: 0x04000004 RID: 4
		private IReadOnlyList<HuffmanListNode> _prefixList;

		// Token: 0x04000005 RID: 5
		private int _prefixBitLength;

		// Token: 0x04000006 RID: 6
		private int _maxBits;

		// Token: 0x0200003D RID: 61
		private class FastRange : IReadOnlyList<int>, IEnumerable<int>, IEnumerable, IReadOnlyCollection<int>
		{
			// Token: 0x0600024C RID: 588 RVA: 0x000086B3 File Offset: 0x000068B3
			internal static Codebook.FastRange Get(int start, int count)
			{
				Codebook.FastRange fastRange;
				if ((fastRange = Codebook.FastRange._cachedRange) == null)
				{
					fastRange = (Codebook.FastRange._cachedRange = new Codebook.FastRange());
				}
				Codebook.FastRange fastRange2 = fastRange;
				fastRange2._start = start;
				fastRange2._count = count;
				return fastRange2;
			}

			// Token: 0x0600024D RID: 589 RVA: 0x000086D7 File Offset: 0x000068D7
			private FastRange()
			{
			}

			// Token: 0x170000DF RID: 223
			public int this[int index]
			{
				get
				{
					if (index > this._count)
					{
						throw new ArgumentOutOfRangeException();
					}
					return this._start + index;
				}
			}

			// Token: 0x170000E0 RID: 224
			// (get) Token: 0x0600024F RID: 591 RVA: 0x000086F8 File Offset: 0x000068F8
			public int Count
			{
				get
				{
					return this._count;
				}
			}

			// Token: 0x06000250 RID: 592 RVA: 0x00008700 File Offset: 0x00006900
			public IEnumerator<int> GetEnumerator()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06000251 RID: 593 RVA: 0x00008707 File Offset: 0x00006907
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x040000E1 RID: 225
			[ThreadStatic]
			private static Codebook.FastRange _cachedRange;

			// Token: 0x040000E2 RID: 226
			private int _start;

			// Token: 0x040000E3 RID: 227
			private int _count;
		}
	}
}
