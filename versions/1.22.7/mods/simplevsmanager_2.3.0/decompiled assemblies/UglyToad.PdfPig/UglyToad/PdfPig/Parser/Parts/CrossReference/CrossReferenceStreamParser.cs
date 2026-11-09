using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.CrossReference;
using UglyToad.PdfPig.Filters;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util;

namespace UglyToad.PdfPig.Parser.Parts.CrossReference
{
	// Token: 0x020000CE RID: 206
	internal class CrossReferenceStreamParser
	{
		// Token: 0x060003E2 RID: 994 RVA: 0x00017B0E File Offset: 0x00015D0E
		public CrossReferenceStreamParser(IFilterProvider filterProvider)
		{
			this.filterProvider = filterProvider;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00017B20 File Offset: 0x00015D20
		public CrossReferenceTablePart Parse(long streamOffset, long? fromTableAtOffset, StreamToken stream)
		{
			IReadOnlyList<byte> readOnlyList = stream.Decode(this.filterProvider);
			CrossReferenceStreamFieldSize crossReferenceStreamFieldSize = new CrossReferenceStreamFieldSize(stream.StreamDictionary);
			int num = readOnlyList.Count / crossReferenceStreamFieldSize.LineLength;
			long previous = -1L;
			IToken token;
			if (stream.StreamDictionary.TryGet(NameToken.Prev, ref token))
			{
				NumericToken numericToken = token as NumericToken;
				if (numericToken != null)
				{
					previous = numericToken.Long;
				}
			}
			CrossReferenceTablePartBuilder crossReferenceTablePartBuilder = new CrossReferenceTablePartBuilder
			{
				Offset = streamOffset,
				Previous = previous,
				Dictionary = stream.StreamDictionary,
				XRefType = CrossReferenceType.Stream,
				TiedToPreviousAtOffset = fromTableAtOffset
			};
			IEnumerable<long> objectNumbers = CrossReferenceStreamParser.GetObjectNumbers(stream.StreamDictionary);
			int num2 = 0;
			byte[] array = new byte[crossReferenceStreamFieldSize.LineLength];
			foreach (long objectNumber in objectNumbers)
			{
				if (num2 >= num)
				{
					break;
				}
				int num3 = num2 * crossReferenceStreamFieldSize.LineLength;
				for (int i = 0; i < crossReferenceStreamFieldSize.LineLength; i++)
				{
					array[i] = readOnlyList[num3 + i];
				}
				int num4;
				if (crossReferenceStreamFieldSize.Field1Size == 0)
				{
					num4 = 1;
				}
				else
				{
					num4 = 0;
					for (int j = 0; j < crossReferenceStreamFieldSize.Field1Size; j++)
					{
						num4 += (int)(array[j] & byte.MaxValue) << (crossReferenceStreamFieldSize.Field1Size - j - 1) * 8;
					}
				}
				CrossReferenceStreamParser.ReadNextStreamObject(num4, objectNumber, crossReferenceStreamFieldSize, crossReferenceTablePartBuilder, array);
				num2++;
			}
			return crossReferenceTablePartBuilder.Build();
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00017CA4 File Offset: 0x00015EA4
		private static void ReadNextStreamObject(int type, long objectNumber, CrossReferenceStreamFieldSize fieldSizes, CrossReferenceTablePartBuilder builder, byte[] lineBuffer)
		{
			switch (type)
			{
			case 0:
				break;
			case 1:
			{
				int num = 0;
				for (int i = 0; i < fieldSizes.Field2Size; i++)
				{
					num += (int)(lineBuffer[i + fieldSizes.Field1Size] & byte.MaxValue) << (fieldSizes.Field2Size - i - 1) * 8;
				}
				int num2 = 0;
				for (int j = 0; j < fieldSizes.Field3Size; j++)
				{
					num2 += (int)(lineBuffer[j + fieldSizes.Field1Size + fieldSizes.Field2Size] & byte.MaxValue) << (fieldSizes.Field3Size - j - 1) * 8;
				}
				builder.Add(objectNumber, num2, (long)num);
				return;
			}
			case 2:
			{
				int num3 = 0;
				for (int k = 0; k < fieldSizes.Field2Size; k++)
				{
					num3 += (int)(lineBuffer[k + fieldSizes.Field1Size] & byte.MaxValue) << (fieldSizes.Field2Size - k - 1) * 8;
				}
				builder.Add(objectNumber, 0, (long)(-(long)num3));
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00017D94 File Offset: 0x00015F94
		private static IEnumerable<long> GetObjectNumbers(DictionaryToken dictionary)
		{
			IToken token;
			if (dictionary.TryGet(NameToken.Size, ref token))
			{
				NumericToken numericToken = token as NumericToken;
				if (numericToken != null)
				{
					List<long> list = new List<long>();
					IToken token2;
					if (dictionary.TryGet(NameToken.Index, ref token2))
					{
						ArrayToken arrayToken = token2 as ArrayToken;
						if (arrayToken != null)
						{
							for (int i = 0; i < arrayToken.Length; i += 2)
							{
								int @int = arrayToken.GetNumeric(i).Int;
								int int2 = arrayToken.GetNumeric(i + 1).Int;
								for (int j = 0; j < int2; j++)
								{
									list.Add((long)(@int + j));
								}
							}
							return list;
						}
					}
					for (int k = 0; k < numericToken.Int; k++)
					{
						list.Add((long)k);
					}
					return list;
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(58, 1);
			defaultInterpolatedStringHandler.AppendLiteral("The stream dictionary must contain a numeric size value: ");
			defaultInterpolatedStringHandler.AppendFormatted<DictionaryToken>(dictionary);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new PdfDocumentFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x040001FC RID: 508
		private readonly IFilterProvider filterProvider;
	}
}
