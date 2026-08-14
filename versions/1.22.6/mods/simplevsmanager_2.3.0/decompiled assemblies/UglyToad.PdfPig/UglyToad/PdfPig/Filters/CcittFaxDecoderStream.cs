using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.IO;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001BD RID: 445
	internal class CcittFaxDecoderStream : StreamWrapper
	{
		// Token: 0x06000AA6 RID: 2726 RVA: 0x000354C4 File Offset: 0x000336C4
		public CcittFaxDecoderStream(Stream stream, int columns, CcittFaxCompressionType type, bool byteAligned) : base(stream)
		{
			this.columns = columns;
			this.type = type;
			this.decodedRow = new byte[(columns + 7) / 8];
			this.changesReferenceRow = new int[columns + 2];
			this.changesCurrentRow = new int[columns + 2];
			this.optionByteAligned = byteAligned;
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00035528 File Offset: 0x00033728
		private void Fetch()
		{
			if (this.decodedPos >= this.decodedLength)
			{
				this.decodedLength = 0;
				try
				{
					this.DecodeRow();
				}
				catch (InvalidOperationException)
				{
					if (this.decodedLength != 0)
					{
						throw;
					}
					this.decodedLength = -1;
				}
				this.decodedPos = 0;
			}
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00035580 File Offset: 0x00033780
		private void Decode1D()
		{
			int num = 0;
			bool flag = true;
			this.changesCurrentRowCount = 0;
			do
			{
				int num2 = flag ? this.DecodeRun(CcittFaxDecoderStream.WhiteRunTree) : this.DecodeRun(CcittFaxDecoderStream.BlackRunTree);
				num += num2;
				int[] array = this.changesCurrentRow;
				int num3 = this.changesCurrentRowCount;
				this.changesCurrentRowCount = num3 + 1;
				array[num3] = num;
				flag = !flag;
			}
			while (num < this.columns);
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x000355E0 File Offset: 0x000337E0
		private void Decode2D()
		{
			this.changesReferenceRowCount = this.changesCurrentRowCount;
			int[] array = this.changesCurrentRow;
			this.changesCurrentRow = this.changesReferenceRow;
			this.changesReferenceRow = array;
			bool flag = true;
			int i = 0;
			this.changesCurrentRowCount = 0;
			IL_176:
			while (i < this.columns)
			{
				CcittFaxDecoderStream.Node node = CcittFaxDecoderStream.CodeTree.Root;
				do
				{
					node = node.Walk(this.ReadBit());
					if (node == null)
					{
						goto IL_176;
					}
				}
				while (!node.IsLeaf);
				int value = node.Value;
				if (value != -4000)
				{
					if (value != -3000)
					{
						int nextChangingElement = this.GetNextChangingElement(i, flag);
						if (nextChangingElement >= this.changesReferenceRowCount || nextChangingElement == -1)
						{
							i = this.columns + node.Value;
						}
						else
						{
							i = this.changesReferenceRow[nextChangingElement] + node.Value;
						}
						this.changesCurrentRow[this.changesCurrentRowCount] = i;
						this.changesCurrentRowCount++;
						flag = !flag;
					}
					else
					{
						int num = this.GetNextChangingElement(i, flag) + 1;
						if (num >= this.changesReferenceRowCount)
						{
							i = this.columns;
						}
						else
						{
							i = this.changesReferenceRow[num];
						}
					}
				}
				else
				{
					int num2 = this.DecodeRun(flag ? CcittFaxDecoderStream.WhiteRunTree : CcittFaxDecoderStream.BlackRunTree);
					i += num2;
					int[] array2 = this.changesCurrentRow;
					int num3 = this.changesCurrentRowCount;
					this.changesCurrentRowCount = num3 + 1;
					array2[num3] = i;
					num2 = this.DecodeRun(flag ? CcittFaxDecoderStream.BlackRunTree : CcittFaxDecoderStream.WhiteRunTree);
					i += num2;
					int[] array3 = this.changesCurrentRow;
					num3 = this.changesCurrentRowCount;
					this.changesCurrentRowCount = num3 + 1;
					array3[num3] = i;
				}
			}
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x00035770 File Offset: 0x00033970
		private int GetNextChangingElement(int a0, bool white)
		{
			int num = (int)((long)this.lastChangingElement & (long)((ulong)-2)) + (white ? 0 : 1);
			if (num > 2)
			{
				num -= 2;
			}
			if (a0 == 0)
			{
				return num;
			}
			for (int i = num; i < this.changesReferenceRowCount; i += 2)
			{
				if (a0 < this.changesReferenceRow[i])
				{
					this.lastChangingElement = i;
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x000357C5 File Offset: 0x000339C5
		private void DecodeRowType2()
		{
			if (this.optionByteAligned)
			{
				this.ResetBuffer();
			}
			this.Decode1D();
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x000357DC File Offset: 0x000339DC
		private void DecodeRowType4()
		{
			if (this.optionByteAligned)
			{
				this.ResetBuffer();
			}
			for (;;)
			{
				CcittFaxDecoderStream.Node node = CcittFaxDecoderStream.EolOnlyTree.Root;
				for (;;)
				{
					node = node.Walk(this.ReadBit());
					if (node == null)
					{
						break;
					}
					if (node.IsLeaf)
					{
						goto Block_3;
					}
				}
			}
			Block_3:
			if (this.type == CcittFaxCompressionType.Group3_1D || this.ReadBit())
			{
				this.Decode1D();
				return;
			}
			this.Decode2D();
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x00035838 File Offset: 0x00033A38
		private void DecodeRowType6()
		{
			if (this.optionByteAligned)
			{
				this.ResetBuffer();
			}
			this.Decode2D();
		}

		// Token: 0x06000AAE RID: 2734 RVA: 0x00035850 File Offset: 0x00033A50
		private void DecodeRow()
		{
			switch (this.type)
			{
			case CcittFaxCompressionType.ModifiedHuffman:
				this.DecodeRowType2();
				break;
			case CcittFaxCompressionType.Group3_1D:
			case CcittFaxCompressionType.Group3_2D:
				this.DecodeRowType4();
				break;
			case CcittFaxCompressionType.Group4_2D:
				this.DecodeRowType6();
				break;
			default:
				throw new InvalidOperationException(this.type.ToString() + " is not a supported compression type.");
			}
			int num = 0;
			bool flag = true;
			this.lastChangingElement = 0;
			for (int i = 0; i <= this.changesCurrentRowCount; i++)
			{
				int num2 = this.columns;
				if (i != this.changesCurrentRowCount)
				{
					num2 = this.changesCurrentRow[i];
				}
				if (num2 > this.columns)
				{
					num2 = this.columns;
				}
				int num3 = num / 8;
				while (num % 8 != 0 && num2 - num > 0)
				{
					byte[] array = this.decodedRow;
					int num4 = num3;
					array[num4] |= (byte)(flag ? 0 : (1 << 7 - num % 8));
					num++;
				}
				if (num % 8 == 0)
				{
					num3 = num / 8;
					byte b = flag ? 0 : byte.MaxValue;
					while (num2 - num > 7)
					{
						this.decodedRow[num3] = b;
						num += 8;
						num3++;
					}
				}
				while (num2 - num > 0)
				{
					if (num % 8 == 0)
					{
						this.decodedRow[num3] = 0;
					}
					byte[] array2 = this.decodedRow;
					int num5 = num3;
					array2[num5] |= (byte)(flag ? 0 : (1 << 7 - num % 8));
					num++;
				}
				flag = !flag;
			}
			if (num != this.columns)
			{
				throw new InvalidOperationException("Sum of run-lengths does not equal scan line width: " + num.ToString() + " > " + this.columns.ToString());
			}
			this.decodedLength = (num + 7) / 8;
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x000359F4 File Offset: 0x00033BF4
		private int DecodeRun(CcittFaxDecoderStream.Tree tree)
		{
			int num = 0;
			CcittFaxDecoderStream.Node node = tree.Root;
			for (;;)
			{
				bool next = this.ReadBit();
				node = node.Walk(next);
				if (node == null)
				{
					break;
				}
				if (node.IsLeaf)
				{
					num += node.Value;
					if (node.Value < 64)
					{
						goto IL_4A;
					}
					node = tree.Root;
				}
			}
			throw new InvalidOperationException("Unknown code in Huffman RLE stream");
			IL_4A:
			if (node.Value >= 0)
			{
				return num;
			}
			return this.columns;
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00035A5C File Offset: 0x00033C5C
		private void ResetBuffer()
		{
			this.bufferPos = -1;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00035A68 File Offset: 0x00033C68
		private bool ReadBit()
		{
			if (this.bufferPos < 0 || this.bufferPos > 7)
			{
				this.buffer = this.Stream.ReadByte();
				if (this.buffer == -1)
				{
					throw new InvalidOperationException("Unexpected end of Huffman RLE stream");
				}
				this.bufferPos = 0;
			}
			bool result = (this.buffer >> 7 - this.bufferPos & 1) == 1;
			this.bufferPos++;
			if (this.bufferPos > 7)
			{
				this.bufferPos = -1;
			}
			return result;
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x00035AE8 File Offset: 0x00033CE8
		public override int ReadByte()
		{
			if (this.decodedLength < 0)
			{
				return 0;
			}
			if (this.decodedPos >= this.decodedLength)
			{
				this.Fetch();
				if (this.decodedLength < 0)
				{
					return 0;
				}
			}
			byte[] array = this.decodedRow;
			int num = this.decodedPos;
			this.decodedPos = num + 1;
			return array[num] & 255;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00035B40 File Offset: 0x00033D40
		public override int Read(byte[] b, int off, int len)
		{
			if (this.decodedLength < 0)
			{
				ArrayHelper.Fill<byte>(b, off, off + len, 0);
				return len;
			}
			if (this.decodedPos >= this.decodedLength)
			{
				this.Fetch();
				if (this.decodedLength < 0)
				{
					ArrayHelper.Fill<byte>(b, off, off + len, 0);
					return len;
				}
			}
			int num = Math.Min(this.decodedLength - this.decodedPos, len);
			Array.Copy(this.decodedRow, this.decodedPos, b, off, num);
			this.decodedPos += num;
			return num;
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00035BC4 File Offset: 0x00033DC4
		static CcittFaxDecoderStream()
		{
			CcittFaxDecoderStream.FILL.Left = CcittFaxDecoderStream.FILL;
			CcittFaxDecoderStream.FILL.Right = CcittFaxDecoderStream.EOL;
			CcittFaxDecoderStream.EolOnlyTree = new CcittFaxDecoderStream.Tree();
			CcittFaxDecoderStream.EolOnlyTree.Fill(12, 0, CcittFaxDecoderStream.FILL);
			CcittFaxDecoderStream.EolOnlyTree.Fill(12, 1, CcittFaxDecoderStream.EOL);
			CcittFaxDecoderStream.BlackRunTree = new CcittFaxDecoderStream.Tree();
			for (int i = 0; i < CcittFaxDecoderStream.BLACK_CODES.Length; i++)
			{
				for (int j = 0; j < CcittFaxDecoderStream.BLACK_CODES[i].Length; j++)
				{
					CcittFaxDecoderStream.BlackRunTree.Fill(i + 2, (int)CcittFaxDecoderStream.BLACK_CODES[i][j], (int)CcittFaxDecoderStream.BLACK_RUN_LENGTHS[i][j]);
				}
			}
			CcittFaxDecoderStream.BlackRunTree.Fill(12, 0, CcittFaxDecoderStream.FILL);
			CcittFaxDecoderStream.BlackRunTree.Fill(12, 1, CcittFaxDecoderStream.EOL);
			CcittFaxDecoderStream.WhiteRunTree = new CcittFaxDecoderStream.Tree();
			for (int k = 0; k < CcittFaxDecoderStream.WHITE_CODES.Length; k++)
			{
				for (int l = 0; l < CcittFaxDecoderStream.WHITE_CODES[k].Length; l++)
				{
					CcittFaxDecoderStream.WhiteRunTree.Fill(k + 4, (int)CcittFaxDecoderStream.WHITE_CODES[k][l], (int)CcittFaxDecoderStream.WHITE_RUN_LENGTHS[k][l]);
				}
			}
			CcittFaxDecoderStream.WhiteRunTree.Fill(12, 0, CcittFaxDecoderStream.FILL);
			CcittFaxDecoderStream.WhiteRunTree.Fill(12, 1, CcittFaxDecoderStream.EOL);
			CcittFaxDecoderStream.CodeTree = new CcittFaxDecoderStream.Tree();
			CcittFaxDecoderStream.CodeTree.Fill(4, 1, -3000);
			CcittFaxDecoderStream.CodeTree.Fill(3, 1, -4000);
			CcittFaxDecoderStream.CodeTree.Fill(1, 1, 0);
			CcittFaxDecoderStream.CodeTree.Fill(3, 3, 1);
			CcittFaxDecoderStream.CodeTree.Fill(6, 3, 2);
			CcittFaxDecoderStream.CodeTree.Fill(7, 3, 3);
			CcittFaxDecoderStream.CodeTree.Fill(3, 2, -1);
			CcittFaxDecoderStream.CodeTree.Fill(6, 2, -2);
			CcittFaxDecoderStream.CodeTree.Fill(7, 2, -3);
		}

		// Token: 0x0400055D RID: 1373
		private readonly int columns;

		// Token: 0x0400055E RID: 1374
		private readonly byte[] decodedRow;

		// Token: 0x0400055F RID: 1375
		private readonly bool optionByteAligned;

		// Token: 0x04000560 RID: 1376
		private readonly CcittFaxCompressionType type;

		// Token: 0x04000561 RID: 1377
		private int decodedLength;

		// Token: 0x04000562 RID: 1378
		private int decodedPos;

		// Token: 0x04000563 RID: 1379
		private int[] changesReferenceRow;

		// Token: 0x04000564 RID: 1380
		private int[] changesCurrentRow;

		// Token: 0x04000565 RID: 1381
		private int changesReferenceRowCount;

		// Token: 0x04000566 RID: 1382
		private int changesCurrentRowCount;

		// Token: 0x04000567 RID: 1383
		private int lastChangingElement;

		// Token: 0x04000568 RID: 1384
		private int buffer = -1;

		// Token: 0x04000569 RID: 1385
		private int bufferPos = -1;

		// Token: 0x0400056A RID: 1386
		private static readonly short[][] BLACK_CODES = new short[][]
		{
			new short[]
			{
				2,
				3
			},
			new short[]
			{
				2,
				3
			},
			new short[]
			{
				2,
				3
			},
			new short[]
			{
				3
			},
			new short[]
			{
				4,
				5
			},
			new short[]
			{
				4,
				5,
				7
			},
			new short[]
			{
				4,
				7
			},
			new short[]
			{
				24
			},
			new short[]
			{
				23,
				24,
				55,
				8,
				15
			},
			new short[]
			{
				23,
				24,
				40,
				55,
				103,
				104,
				108,
				8,
				12,
				13
			},
			new short[]
			{
				18,
				19,
				20,
				21,
				22,
				23,
				28,
				29,
				30,
				31,
				36,
				39,
				40,
				43,
				44,
				51,
				52,
				53,
				55,
				56,
				82,
				83,
				84,
				85,
				86,
				87,
				88,
				89,
				90,
				91,
				100,
				101,
				102,
				103,
				104,
				105,
				106,
				107,
				108,
				109,
				200,
				201,
				202,
				203,
				204,
				205,
				210,
				211,
				212,
				213,
				214,
				215,
				218,
				219
			},
			new short[]
			{
				74,
				75,
				76,
				77,
				82,
				83,
				84,
				85,
				90,
				91,
				100,
				101,
				108,
				109,
				114,
				115,
				116,
				117,
				118,
				119
			}
		};

		// Token: 0x0400056B RID: 1387
		private static readonly short[][] BLACK_RUN_LENGTHS = new short[][]
		{
			new short[]
			{
				3,
				2
			},
			new short[]
			{
				1,
				4
			},
			new short[]
			{
				6,
				5
			},
			new short[]
			{
				7
			},
			new short[]
			{
				9,
				8
			},
			new short[]
			{
				10,
				11,
				12
			},
			new short[]
			{
				13,
				14
			},
			new short[]
			{
				15
			},
			new short[]
			{
				16,
				17,
				0,
				18,
				64
			},
			new short[]
			{
				24,
				25,
				23,
				22,
				19,
				20,
				21,
				1792,
				1856,
				1920
			},
			new short[]
			{
				1984,
				2048,
				2112,
				2176,
				2240,
				2304,
				2368,
				2432,
				2496,
				2560,
				52,
				55,
				56,
				59,
				60,
				320,
				384,
				448,
				53,
				54,
				50,
				51,
				44,
				45,
				46,
				47,
				57,
				58,
				61,
				256,
				48,
				49,
				62,
				63,
				30,
				31,
				32,
				33,
				40,
				41,
				128,
				192,
				26,
				27,
				28,
				29,
				34,
				35,
				36,
				37,
				38,
				39,
				42,
				43
			},
			new short[]
			{
				640,
				704,
				768,
				832,
				1280,
				1344,
				1408,
				1472,
				1536,
				1600,
				1664,
				1728,
				512,
				576,
				896,
				960,
				1024,
				1088,
				1152,
				1216
			}
		};

		// Token: 0x0400056C RID: 1388
		private static readonly short[][] WHITE_CODES = new short[][]
		{
			new short[]
			{
				7,
				8,
				11,
				12,
				14,
				15
			},
			new short[]
			{
				18,
				19,
				20,
				27,
				7,
				8
			},
			new short[]
			{
				23,
				24,
				42,
				43,
				3,
				52,
				53,
				7,
				8
			},
			new short[]
			{
				19,
				23,
				24,
				36,
				39,
				40,
				43,
				3,
				55,
				4,
				8,
				12
			},
			new short[]
			{
				18,
				19,
				20,
				21,
				22,
				23,
				26,
				27,
				2,
				36,
				37,
				40,
				41,
				42,
				43,
				44,
				45,
				3,
				50,
				51,
				52,
				53,
				54,
				55,
				4,
				74,
				75,
				5,
				82,
				83,
				84,
				85,
				88,
				89,
				90,
				91,
				100,
				101,
				103,
				104,
				10,
				11
			},
			new short[]
			{
				152,
				153,
				154,
				155,
				204,
				205,
				210,
				211,
				212,
				213,
				214,
				215,
				216,
				217,
				218,
				219
			},
			new short[0],
			new short[]
			{
				8,
				12,
				13
			},
			new short[]
			{
				18,
				19,
				20,
				21,
				22,
				23,
				28,
				29,
				30,
				31
			}
		};

		// Token: 0x0400056D RID: 1389
		private static readonly short[][] WHITE_RUN_LENGTHS = new short[][]
		{
			new short[]
			{
				2,
				3,
				4,
				5,
				6,
				7
			},
			new short[]
			{
				128,
				8,
				9,
				64,
				10,
				11
			},
			new short[]
			{
				192,
				1664,
				16,
				17,
				13,
				14,
				15,
				1,
				12
			},
			new short[]
			{
				26,
				21,
				28,
				27,
				18,
				24,
				25,
				22,
				256,
				23,
				20,
				19
			},
			new short[]
			{
				33,
				34,
				35,
				36,
				37,
				38,
				31,
				32,
				29,
				53,
				54,
				39,
				40,
				41,
				42,
				43,
				44,
				30,
				61,
				62,
				63,
				0,
				320,
				384,
				45,
				59,
				60,
				46,
				49,
				50,
				51,
				52,
				55,
				56,
				57,
				58,
				448,
				512,
				640,
				576,
				47,
				48
			},
			new short[]
			{
				1472,
				1536,
				1600,
				1728,
				704,
				768,
				832,
				896,
				960,
				1024,
				1088,
				1152,
				1216,
				1280,
				1344,
				1408
			},
			new short[0],
			new short[]
			{
				1792,
				1856,
				1920
			},
			new short[]
			{
				1984,
				2048,
				2112,
				2176,
				2240,
				2304,
				2368,
				2432,
				2496,
				2560
			}
		};

		// Token: 0x0400056E RID: 1390
		private static readonly CcittFaxDecoderStream.Node EOL = new CcittFaxDecoderStream.Node
		{
			IsLeaf = true,
			Value = -2000
		};

		// Token: 0x0400056F RID: 1391
		private static readonly CcittFaxDecoderStream.Node FILL = new CcittFaxDecoderStream.Node
		{
			Value = -1000
		};

		// Token: 0x04000570 RID: 1392
		private static readonly CcittFaxDecoderStream.Tree BlackRunTree;

		// Token: 0x04000571 RID: 1393
		private static readonly CcittFaxDecoderStream.Tree WhiteRunTree;

		// Token: 0x04000572 RID: 1394
		private static readonly CcittFaxDecoderStream.Tree EolOnlyTree;

		// Token: 0x04000573 RID: 1395
		private static readonly CcittFaxDecoderStream.Tree CodeTree;

		// Token: 0x04000574 RID: 1396
		private const int VALUE_EOL = -2000;

		// Token: 0x04000575 RID: 1397
		private const int VALUE_FILL = -1000;

		// Token: 0x04000576 RID: 1398
		private const int VALUE_PASSMODE = -3000;

		// Token: 0x04000577 RID: 1399
		private const int VALUE_HMODE = -4000;

		// Token: 0x020002B1 RID: 689
		private class Node
		{
			// Token: 0x17000417 RID: 1047
			// (get) Token: 0x06000EED RID: 3821 RVA: 0x00043A43 File Offset: 0x00041C43
			// (set) Token: 0x06000EEE RID: 3822 RVA: 0x00043A4B File Offset: 0x00041C4B
			public CcittFaxDecoderStream.Node Left { get; set; }

			// Token: 0x17000418 RID: 1048
			// (get) Token: 0x06000EEF RID: 3823 RVA: 0x00043A54 File Offset: 0x00041C54
			// (set) Token: 0x06000EF0 RID: 3824 RVA: 0x00043A5C File Offset: 0x00041C5C
			public CcittFaxDecoderStream.Node Right { get; set; }

			// Token: 0x17000419 RID: 1049
			// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x00043A65 File Offset: 0x00041C65
			// (set) Token: 0x06000EF2 RID: 3826 RVA: 0x00043A6D File Offset: 0x00041C6D
			public int Value { get; set; }

			// Token: 0x1700041A RID: 1050
			// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x00043A76 File Offset: 0x00041C76
			// (set) Token: 0x06000EF4 RID: 3828 RVA: 0x00043A7E File Offset: 0x00041C7E
			public bool CanBeFill { get; set; }

			// Token: 0x1700041B RID: 1051
			// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x00043A87 File Offset: 0x00041C87
			// (set) Token: 0x06000EF6 RID: 3830 RVA: 0x00043A8F File Offset: 0x00041C8F
			public bool IsLeaf { get; set; }

			// Token: 0x06000EF7 RID: 3831 RVA: 0x00043A98 File Offset: 0x00041C98
			public void Set(bool next, CcittFaxDecoderStream.Node node)
			{
				if (!next)
				{
					this.Left = node;
					return;
				}
				this.Right = node;
			}

			// Token: 0x06000EF8 RID: 3832 RVA: 0x00043AAC File Offset: 0x00041CAC
			public CcittFaxDecoderStream.Node Walk(bool next)
			{
				if (!next)
				{
					return this.Left;
				}
				return this.Right;
			}

			// Token: 0x06000EF9 RID: 3833 RVA: 0x00043AC0 File Offset: 0x00041CC0
			public override string ToString()
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(9, 6);
				defaultInterpolatedStringHandler.AppendLiteral("[");
				defaultInterpolatedStringHandler.AppendFormatted("IsLeaf");
				defaultInterpolatedStringHandler.AppendLiteral("=");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(this.IsLeaf);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted("Value");
				defaultInterpolatedStringHandler.AppendLiteral("=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Value);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted("CanBeFill");
				defaultInterpolatedStringHandler.AppendLiteral("=");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(this.CanBeFill);
				defaultInterpolatedStringHandler.AppendLiteral("]");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x020002B2 RID: 690
		private class Tree
		{
			// Token: 0x1700041C RID: 1052
			// (get) Token: 0x06000EFB RID: 3835 RVA: 0x00043B85 File Offset: 0x00041D85
			public CcittFaxDecoderStream.Node Root { get; } = new CcittFaxDecoderStream.Node();

			// Token: 0x06000EFC RID: 3836 RVA: 0x00043B90 File Offset: 0x00041D90
			public void Fill(int depth, int path, int value)
			{
				CcittFaxDecoderStream.Node node = this.Root;
				for (int i = 0; i < depth; i++)
				{
					int num = depth - 1 - i;
					bool next = (path >> num & 1) == 1;
					CcittFaxDecoderStream.Node node2 = node.Walk(next);
					if (node2 == null)
					{
						node2 = new CcittFaxDecoderStream.Node();
						if (i == depth - 1)
						{
							node2.Value = value;
							node2.IsLeaf = true;
						}
						if (path == 0)
						{
							node2.CanBeFill = true;
						}
						node.Set(next, node2);
					}
					else if (node2.IsLeaf)
					{
						throw new InvalidOperationException("node is leaf, no other following");
					}
					node = node2;
				}
			}

			// Token: 0x06000EFD RID: 3837 RVA: 0x00043C1C File Offset: 0x00041E1C
			public void Fill(int depth, int path, CcittFaxDecoderStream.Node node)
			{
				CcittFaxDecoderStream.Node node2 = this.Root;
				for (int i = 0; i < depth; i++)
				{
					int num = depth - 1 - i;
					bool next = (path >> num & 1) == 1;
					CcittFaxDecoderStream.Node node3 = node2.Walk(next);
					if (node3 == null)
					{
						if (i == depth - 1)
						{
							node3 = node;
						}
						else
						{
							node3 = new CcittFaxDecoderStream.Node();
						}
						if (path == 0)
						{
							node3.CanBeFill = true;
						}
						node2.Set(next, node3);
					}
					else if (node3.IsLeaf)
					{
						throw new InvalidOperationException("node is leaf, no other following");
					}
					node2 = node3;
				}
			}
		}
	}
}
