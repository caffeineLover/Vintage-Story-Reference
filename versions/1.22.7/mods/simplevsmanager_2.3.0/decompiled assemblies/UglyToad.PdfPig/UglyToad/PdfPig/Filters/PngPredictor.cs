using System;
using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig.IO;

namespace UglyToad.PdfPig.Filters
{
	// Token: 0x020001C9 RID: 457
	internal static class PngPredictor
	{
		// Token: 0x06000ADA RID: 2778 RVA: 0x00036B9C File Offset: 0x00034D9C
		public static byte[] Decode(byte[] inputBytes, int predictor, int colors, int bitsPerComponent, int columns)
		{
			if (inputBytes == null)
			{
				throw new ArgumentNullException("inputBytes");
			}
			if (predictor == 1)
			{
				return inputBytes;
			}
			int num = colors * bitsPerComponent;
			int num2 = (num + 7) / 8;
			int num3 = (columns * num + 7) / 8;
			byte[] array = new byte[num3];
			byte[] array2 = new byte[num3];
			int num4 = predictor;
			List<byte> list = new List<byte>();
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					using (RandomAccessBuffer randomAccessBuffer = new RandomAccessBuffer(inputBytes))
					{
						while (randomAccessBuffer.Available() > 0)
						{
							if (predictor >= 10)
							{
								num4 = randomAccessBuffer.Read();
								if (num4 == -1)
								{
									return list.ToArray();
								}
								num4 += 10;
							}
							int num5 = 0;
							int num6;
							while (num5 < num3 && (num6 = randomAccessBuffer.Read(array, num5, num3 - num5)) != -1 && num6 != 0)
							{
								num5 += num6;
							}
							if (num4 != 2)
							{
								switch (num4)
								{
								case 11:
									for (int i = num2; i < num3; i++)
									{
										int num7 = (int)array[i];
										int num8 = (int)array[i - num2];
										array[i] = (byte)(num7 + num8);
									}
									break;
								case 12:
									for (int j = 0; j < num3; j++)
									{
										int num9 = (int)(array[j] & byte.MaxValue);
										int num10 = (int)(array2[j] & byte.MaxValue);
										array[j] = (byte)(num9 + num10 & 255);
									}
									break;
								case 13:
									for (int k = 0; k < num3; k++)
									{
										int num11 = (int)(array[k] & byte.MaxValue);
										int num12 = (int)((k - num2 >= 0) ? (array[k - num2] & byte.MaxValue) : 0);
										int num13 = (int)(array2[k] & byte.MaxValue);
										array[k] = (byte)(num11 + (num12 + num13) / 2 & 255);
									}
									break;
								case 14:
									for (int l = 0; l < num3; l++)
									{
										int num14 = (int)(array[l] & byte.MaxValue);
										int num15 = (int)((l - num2 >= 0) ? (array[l - num2] & byte.MaxValue) : 0);
										int num16 = (int)(array2[l] & byte.MaxValue);
										int num17 = (int)((l - num2 >= 0) ? (array2[l - num2] & byte.MaxValue) : 0);
										int num18 = num15 + num16 - num17;
										int num19 = Math.Abs(num18 - num15);
										int num20 = Math.Abs(num18 - num16);
										int num21 = Math.Abs(num18 - num17);
										if (num19 <= num20 && num19 <= num21)
										{
											array[l] = (byte)(num14 + num15 & 255);
										}
										else if (num20 <= num21)
										{
											array[l] = (byte)(num14 + num16 & 255);
										}
										else
										{
											array[l] = (byte)(num14 + num17 & 255);
										}
									}
									break;
								}
							}
							else if (bitsPerComponent == 8)
							{
								for (int m = num2; m < num3; m++)
								{
									int num22 = (int)(array[m] & byte.MaxValue);
									int num23 = (int)(array[m - num2] & byte.MaxValue);
									array[m] = (byte)(num22 + num23);
								}
							}
							else if (bitsPerComponent == 16)
							{
								for (int n = num2; n < num3; n += 2)
								{
									int num24 = ((int)(array[n] & byte.MaxValue) << 8) + (int)(array[n + 1] & byte.MaxValue);
									int num25 = ((int)(array[n - num2] & byte.MaxValue) << 8) + (int)(array[n - num2 + 1] & byte.MaxValue);
									array[n] = (byte)(num24 + num25 >> 8 & 255);
									array[n + 1] = (byte)(num24 + num25 & 255);
								}
							}
							else if (bitsPerComponent == 1 && colors == 1)
							{
								for (int num26 = 0; num26 < num3; num26++)
								{
									for (int num27 = 7; num27 >= 0; num27--)
									{
										int num28 = array[num26] >> num27 & 1;
										if (num26 != 0 || num27 != 7)
										{
											int num29;
											if (num27 == 7)
											{
												num29 = (int)(array[num26 - 1] & 1);
											}
											else
											{
												num29 = (array[num26] >> num27 + 1 & 1);
											}
											if ((num28 + num29 & 1) == 0)
											{
												array[num26] = (byte)((int)array[num26] & ~(1 << num27));
											}
											else
											{
												array[num26] = (byte)((int)array[num26] | 1 << num27);
											}
										}
									}
								}
							}
							else
							{
								int num30 = columns * colors;
								for (int num31 = colors; num31 < num30; num31++)
								{
									int num32 = num31 * bitsPerComponent / 8;
									int startBit = 8 - num31 * bitsPerComponent % 8 - bitsPerComponent;
									int num33 = (num31 - colors) * bitsPerComponent / 8;
									int startBit2 = 8 - (num31 - colors) * bitsPerComponent % 8 - bitsPerComponent;
									int bitSeq = PngPredictor.GetBitSeq((int)array[num32], startBit, bitsPerComponent);
									int bitSeq2 = PngPredictor.GetBitSeq((int)array[num33], startBit2, bitsPerComponent);
									array[num32] = (byte)PngPredictor.CalcSetBitSeq((int)array[num32], startBit, bitsPerComponent, bitSeq + bitSeq2);
								}
							}
							Array.Copy(array, 0, array2, 0, num3);
							binaryWriter.Write(array);
						}
						result = memoryStream.ToArray();
					}
				}
			}
			return result;
		}

		// Token: 0x06000ADB RID: 2779 RVA: 0x0003707C File Offset: 0x0003527C
		private static int GetBitSeq(int by, int startBit, int bitSize)
		{
			int num = (1 << bitSize) - 1;
			return by >> startBit & num;
		}

		// Token: 0x06000ADC RID: 2780 RVA: 0x0003709C File Offset: 0x0003529C
		private static int CalcSetBitSeq(int by, int startBit, int bitSize, int val)
		{
			int num = (1 << bitSize) - 1;
			int num2 = val & num;
			num = ~(num << startBit);
			return (by & num) | num2 << startBit;
		}
	}
}
