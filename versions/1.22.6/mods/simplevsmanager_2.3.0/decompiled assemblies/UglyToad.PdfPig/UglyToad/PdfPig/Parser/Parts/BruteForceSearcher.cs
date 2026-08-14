using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.Parser.Parts
{
	// Token: 0x020000C9 RID: 201
	internal static class BruteForceSearcher
	{
		// Token: 0x060003D2 RID: 978 RVA: 0x000171F8 File Offset: 0x000153F8
		[NotNull]
		public static IReadOnlyDictionary<IndirectReference, long> GetObjectLocations(IInputBytes bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			int i = 0;
			long lastEndOfFileMarker = BruteForceSearcher.GetLastEndOfFileMarker(bytes);
			Dictionary<IndirectReference, long> dictionary = new Dictionary<IndirectReference, long>();
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			long currentOffset = bytes.CurrentOffset;
			long num = 6L;
			bool flag = false;
			byte[] array = new byte[4];
			while (i <= 10000000)
			{
				i++;
				if (flag)
				{
					if (bytes.CurrentByte == 101)
					{
						byte? b = bytes.Peek();
						if (b != null)
						{
							byte? b2 = b;
							int? num2 = (b2 != null) ? new int?((int)b2.GetValueOrDefault()) : default(int?);
							int num3 = 110;
							if (num2.GetValueOrDefault() == num3 & num2 != null)
							{
								if (ReadHelper.IsString(bytes, "endobj"))
								{
									flag = false;
									i = 0;
									for (int j = 0; j < "endobj".Length; j++)
									{
										bytes.MoveNext();
										num += 1L;
									}
									goto IL_2BA;
								}
								bytes.MoveNext();
								num += 1L;
								goto IL_2BA;
							}
						}
						bytes.MoveNext();
						num += 1L;
					}
					else
					{
						bytes.MoveNext();
						num += 1L;
						i = 0;
					}
				}
				else
				{
					bytes.Seek(num);
					bytes.Read(array, default(int?));
					if (!BruteForceSearcher.IsStartObjMarker(array))
					{
						num += 1L;
					}
					else
					{
						long num4 = num + 1L;
						bytes.Seek(num4);
						while (ReadHelper.IsWhitespace(bytes.CurrentByte))
						{
							if (num4 < 6L)
							{
								break;
							}
							bytes.Seek(num4 -= 1L);
						}
						while (ReadHelper.IsDigit((int)bytes.CurrentByte) && num4 >= 6L)
						{
							stringBuilder.Insert(0, (char)bytes.CurrentByte);
							num4 -= 1L;
							bytes.Seek(num4);
						}
						if (!ReadHelper.IsWhitespace(bytes.CurrentByte))
						{
							num += 1L;
						}
						else
						{
							while (ReadHelper.IsWhitespace(bytes.CurrentByte))
							{
								bytes.Seek(num4 -= 1L);
							}
							while (ReadHelper.IsDigit((int)bytes.CurrentByte) && num4 >= 6L)
							{
								stringBuilder2.Insert(0, (char)bytes.CurrentByte);
								num4 -= 1L;
								bytes.Seek(num4);
							}
							if (stringBuilder2.Length == 0 || stringBuilder.Length == 0)
							{
								stringBuilder.Clear();
								stringBuilder2.Clear();
								num += 1L;
							}
							else
							{
								long num5 = long.Parse(stringBuilder2.ToString(), CultureInfo.InvariantCulture);
								int num6 = int.Parse(stringBuilder.ToString(), CultureInfo.InvariantCulture);
								dictionary[new IndirectReference(num5, num6)] = bytes.CurrentOffset;
								stringBuilder.Clear();
								stringBuilder2.Clear();
								flag = true;
								num += 1L;
								bytes.Seek(num);
								i = 0;
							}
						}
					}
				}
				IL_2BA:
				if (num >= lastEndOfFileMarker || bytes.IsAtEnd())
				{
					bytes.Seek(currentOffset);
					return dictionary;
				}
			}
			throw new PdfDocumentFormatException("Failed to brute-force search the file due to an infinite loop.");
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x000174D8 File Offset: 0x000156D8
		private static long GetLastEndOfFileMarker(IInputBytes bytes)
		{
			long currentOffset = bytes.CurrentOffset;
			long num = bytes.Length - (long)"%%EOF".Length + 1L;
			bytes.Seek(num);
			while (bytes.CurrentOffset > 0L)
			{
				if (ReadHelper.IsString(bytes, "%%EOF"))
				{
					long currentOffset2 = bytes.CurrentOffset;
					bytes.Seek(currentOffset);
					return currentOffset2;
				}
				long num2 = num;
				num = num2 - 1L;
				bytes.Seek(num2);
			}
			bytes.Seek(currentOffset);
			return long.MaxValue;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001754C File Offset: 0x0001574C
		private static bool IsStartObjMarker(byte[] data)
		{
			return ReadHelper.IsWhitespace(data[0]) && ((data[1] == 111 || data[1] == 79) && (data[2] == 98 || data[2] == 66)) && (data[3] == 106 || data[3] == 74);
		}

		// Token: 0x040001F5 RID: 501
		private const int MinimumSearchOffset = 6;
	}
}
