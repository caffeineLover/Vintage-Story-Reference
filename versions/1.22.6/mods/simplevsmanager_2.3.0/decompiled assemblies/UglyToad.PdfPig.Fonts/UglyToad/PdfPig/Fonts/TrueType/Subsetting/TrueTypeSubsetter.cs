using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.TrueType.Parser;
using UglyToad.PdfPig.Fonts.TrueType.Tables;
using UglyToad.PdfPig.Fonts.TrueType.Tables.CMapSubTables;

namespace UglyToad.PdfPig.Fonts.TrueType.Subsetting
{
	// Token: 0x02000054 RID: 84
	public static class TrueTypeSubsetter
	{
		// Token: 0x0600024E RID: 590 RVA: 0x0000B8EC File Offset: 0x00009AEC
		public static byte[] Subset(byte[] fontBytes, TrueTypeSubsetEncoding newEncoding)
		{
			if (fontBytes == null)
			{
				throw new ArgumentNullException("fontBytes");
			}
			if (newEncoding == null)
			{
				throw new ArgumentNullException("newEncoding");
			}
			TrueTypeFont trueTypeFont = TrueTypeFontParser.Parse(new TrueTypeDataBytes(fontBytes));
			TrueTypeSubsetter.OldToNewGlyphIndex[] indexMapping = TrueTypeSubsetter.GetIndexMapping(trueTypeFont, newEncoding);
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				SortedSet<string> sortedSet = new SortedSet<string>(StringComparer.Ordinal);
				for (int i = 0; i < TrueTypeSubsetter.RequiredTags.Count; i++)
				{
					string text = TrueTypeSubsetter.RequiredTags[i];
					if (!trueTypeFont.TableHeaders.ContainsKey(text))
					{
						throw new InvalidFontFormatException("Font does not contain table required for subsetting: " + text + ".");
					}
					sortedSet.Add(text);
				}
				for (int j = 0; j < TrueTypeSubsetter.OptionalTags.Count; j++)
				{
					string text2 = TrueTypeSubsetter.OptionalTags[j];
					if (trueTypeFont.TableHeaders.ContainsKey(text2))
					{
						sortedSet.Add(text2);
					}
				}
				new TrueTypeOffsetSubtable((byte)sortedSet.Count).Write(memoryStream);
				TrueTypeSubsetter.DirectoryEntry[] array = new TrueTypeSubsetter.DirectoryEntry[sortedSet.Count];
				int num = 0;
				foreach (string text3 in sortedSet)
				{
					TrueTypeSubsetter.DirectoryEntry directoryEntry = new TrueTypeSubsetter.DirectoryEntry(text3, memoryStream.Position, trueTypeFont.TableHeaders[text3]);
					directoryEntry.DummyHeader.Write(memoryStream);
					array[num++] = directoryEntry;
				}
				TrueTypeSubsetGlyphTable trueTypeSubsetGlyphTable = TrueTypeGlyphTableSubsetter.SubsetGlyphTable(trueTypeFont, fontBytes, indexMapping);
				foreach (TrueTypeSubsetter.DirectoryEntry directoryEntry2 in array)
				{
					directoryEntry2.OutputTableOffset = memoryStream.Position;
					if (directoryEntry2.Tag == "cmap")
					{
						TrueTypeSubsetter.GetCMapTable(trueTypeFont, directoryEntry2, indexMapping).Write(memoryStream);
					}
					else if (directoryEntry2.Tag == "glyf")
					{
						memoryStream.Write(trueTypeSubsetGlyphTable.Bytes, 0, trueTypeSubsetGlyphTable.Bytes.Length);
					}
					else if (directoryEntry2.Tag == "hmtx")
					{
						TrueTypeSubsetter.GetHorizontalMetricsTable(directoryEntry2, trueTypeSubsetGlyphTable).Write(memoryStream);
					}
					else if (directoryEntry2.Tag == "loca")
					{
						new IndexToLocationTable(directoryEntry2.DummyHeader, IndexToLocationTable.EntryFormat.Long, trueTypeSubsetGlyphTable.GlyphOffsets).Write(memoryStream);
					}
					else if (directoryEntry2.Tag == "head")
					{
						byte[] rawInputTableBytes = TrueTypeSubsetter.GetRawInputTableBytes(fontBytes, directoryEntry2);
						TrueTypeSubsetter.WriteUShort(rawInputTableBytes, rawInputTableBytes.Length - 4, 1);
						memoryStream.Write(rawInputTableBytes, 0, rawInputTableBytes.Length);
					}
					else if (directoryEntry2.Tag == "hhea")
					{
						byte[] rawInputTableBytes2 = TrueTypeSubsetter.GetRawInputTableBytes(fontBytes, directoryEntry2);
						TrueTypeSubsetter.WriteUShort(rawInputTableBytes2, rawInputTableBytes2.Length - 2, (ushort)trueTypeSubsetGlyphTable.HorizontalMetrics.Length);
						memoryStream.Write(rawInputTableBytes2, 0, rawInputTableBytes2.Length);
					}
					else if (directoryEntry2.Tag == "maxp")
					{
						byte[] rawInputTableBytes3 = TrueTypeSubsetter.GetRawInputTableBytes(fontBytes, directoryEntry2);
						TrueTypeSubsetter.WriteUShort(rawInputTableBytes3, 4, trueTypeSubsetGlyphTable.GlyphCount);
						memoryStream.Write(rawInputTableBytes3, 0, rawInputTableBytes3.Length);
					}
					else
					{
						byte[] rawInputTableBytes4 = TrueTypeSubsetter.GetRawInputTableBytes(fontBytes, directoryEntry2);
						memoryStream.Write(rawInputTableBytes4, 0, rawInputTableBytes4.Length);
					}
					directoryEntry2.Length = (uint)(memoryStream.Position - directoryEntry2.OutputTableOffset);
					long num2 = memoryStream.Position % 4L;
					if (num2 > 0L)
					{
						long num3 = 4L - num2;
						memoryStream.Write(TrueTypeSubsetter.PaddingBytes, 0, (int)num3);
					}
				}
				using (StreamInputBytes streamInputBytes = new StreamInputBytes(memoryStream, false))
				{
					foreach (TrueTypeSubsetter.DirectoryEntry directoryEntry3 in array)
					{
						TrueTypeHeaderTable table = new TrueTypeHeaderTable(directoryEntry3.Tag, 0U, (uint)directoryEntry3.OutputTableOffset, directoryEntry3.Length);
						uint checkSum = TrueTypeChecksumCalculator.Calculate(streamInputBytes, table);
						TrueTypeHeaderTable trueTypeHeaderTable = new TrueTypeHeaderTable(directoryEntry3.Tag, checkSum, (uint)directoryEntry3.OutputTableOffset, directoryEntry3.Length);
						memoryStream.Seek(directoryEntry3.OutputEntryOffset, 0);
						trueTypeHeaderTable.Write(memoryStream);
					}
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000BD40 File Offset: 0x00009F40
		private static TrueTypeSubsetter.OldToNewGlyphIndex[] GetIndexMapping(TrueTypeFont font, TrueTypeSubsetEncoding newEncoding)
		{
			TrueTypeSubsetter.OldToNewGlyphIndex[] array = new TrueTypeSubsetter.OldToNewGlyphIndex[newEncoding.Characters.Count + 1];
			array[0] = new TrueTypeSubsetter.OldToNewGlyphIndex(0, 0, '\0');
			ICMapSubTable icmapSubTable;
			if ((icmapSubTable = font.WindowsUnicodeCMap) == null)
			{
				icmapSubTable = (font.WindowsSymbolCMap ?? font.MacRomanCMap);
			}
			ICMapSubTable icmapSubTable2 = icmapSubTable;
			if (icmapSubTable2 == null)
			{
				throw new InvalidOperationException("Cannot subset font due to missing cmap subtables.");
			}
			for (int i = 0; i < newEncoding.Characters.Count; i++)
			{
				char c = newEncoding.Characters[i];
				ushort oldIndex = (ushort)icmapSubTable2.CharacterCodeToGlyphIndex((int)c);
				array[i + 1] = new TrueTypeSubsetter.OldToNewGlyphIndex(oldIndex, (ushort)(i + 1), c);
			}
			return array;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000BDDC File Offset: 0x00009FDC
		private static CMapTable GetCMapTable(TrueTypeFont font, TrueTypeSubsetter.DirectoryEntry entry, TrueTypeSubsetter.OldToNewGlyphIndex[] encoding)
		{
			byte[] array = new byte[256];
			int num = 0;
			while (num < array.Length && num < encoding.Length)
			{
				array[num] = encoding[num].NewIndex;
				num++;
			}
			return new CMapTable(font.TableRegister.CMapTable.Version, entry.DummyHeader, new ByteEncodingCMapTable[]
			{
				new ByteEncodingCMapTable(TrueTypeCMapPlatform.Macintosh, 0, 0, array)
			});
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000BE44 File Offset: 0x0000A044
		private static HorizontalMetricsTable GetHorizontalMetricsTable(TrueTypeSubsetter.DirectoryEntry entry, TrueTypeSubsetGlyphTable glyphTable)
		{
			return new HorizontalMetricsTable(entry.DummyHeader, glyphTable.HorizontalMetrics, EmptyArray<short>.Instance);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000BE5C File Offset: 0x0000A05C
		private static byte[] GetRawInputTableBytes(byte[] font, TrueTypeSubsetter.DirectoryEntry entry)
		{
			byte[] array = new byte[entry.InputHeader.Length];
			Array.Copy(font, (long)((ulong)entry.InputHeader.Offset), array, 0L, (long)array.Length);
			return array;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000BE9A File Offset: 0x0000A09A
		private static void WriteUShort(byte[] array, int offset, ushort value)
		{
			array[offset] = (byte)(value >> 8);
			array[offset + 1] = (byte)value;
		}

		// Token: 0x040001D9 RID: 473
		private const ushort IndexToLocLong = 1;

		// Token: 0x040001DA RID: 474
		private static readonly IReadOnlyList<string> RequiredTags = new string[]
		{
			"cmap",
			"glyf",
			"head",
			"hhea",
			"hmtx",
			"loca",
			"maxp"
		};

		// Token: 0x040001DB RID: 475
		private static readonly IReadOnlyList<string> OptionalTags = new string[]
		{
			"cvt ",
			"fpgm",
			"prep",
			"name"
		};

		// Token: 0x040001DC RID: 476
		private static readonly byte[] PaddingBytes = new byte[4];

		// Token: 0x020000CE RID: 206
		private class DirectoryEntry
		{
			// Token: 0x17000241 RID: 577
			// (get) Token: 0x06000518 RID: 1304 RVA: 0x0001F88F File Offset: 0x0001DA8F
			public string Tag { get; }

			// Token: 0x17000242 RID: 578
			// (get) Token: 0x06000519 RID: 1305 RVA: 0x0001F897 File Offset: 0x0001DA97
			public long OutputEntryOffset { get; }

			// Token: 0x17000243 RID: 579
			// (get) Token: 0x0600051A RID: 1306 RVA: 0x0001F89F File Offset: 0x0001DA9F
			public TrueTypeHeaderTable InputHeader { get; }

			// Token: 0x17000244 RID: 580
			// (get) Token: 0x0600051B RID: 1307 RVA: 0x0001F8A7 File Offset: 0x0001DAA7
			public TrueTypeHeaderTable DummyHeader { get; }

			// Token: 0x17000245 RID: 581
			// (get) Token: 0x0600051C RID: 1308 RVA: 0x0001F8AF File Offset: 0x0001DAAF
			// (set) Token: 0x0600051D RID: 1309 RVA: 0x0001F8B7 File Offset: 0x0001DAB7
			public long OutputTableOffset { get; set; }

			// Token: 0x17000246 RID: 582
			// (get) Token: 0x0600051E RID: 1310 RVA: 0x0001F8C0 File Offset: 0x0001DAC0
			// (set) Token: 0x0600051F RID: 1311 RVA: 0x0001F8C8 File Offset: 0x0001DAC8
			public uint Length { get; set; }

			// Token: 0x06000520 RID: 1312 RVA: 0x0001F8D1 File Offset: 0x0001DAD1
			public DirectoryEntry(string tag, long outputEntryOffset, TrueTypeHeaderTable inputHeader)
			{
				this.Tag = tag;
				this.OutputEntryOffset = outputEntryOffset;
				this.InputHeader = inputHeader;
				this.DummyHeader = TrueTypeHeaderTable.GetEmptyHeaderTable(tag);
			}
		}

		// Token: 0x020000CF RID: 207
		public struct OldToNewGlyphIndex
		{
			// Token: 0x17000247 RID: 583
			// (get) Token: 0x06000521 RID: 1313 RVA: 0x0001F8FA File Offset: 0x0001DAFA
			public readonly ushort OldIndex { get; }

			// Token: 0x17000248 RID: 584
			// (get) Token: 0x06000522 RID: 1314 RVA: 0x0001F902 File Offset: 0x0001DB02
			public readonly byte NewIndex { get; }

			// Token: 0x17000249 RID: 585
			// (get) Token: 0x06000523 RID: 1315 RVA: 0x0001F90A File Offset: 0x0001DB0A
			public readonly char Represents { get; }

			// Token: 0x06000524 RID: 1316 RVA: 0x0001F912 File Offset: 0x0001DB12
			public OldToNewGlyphIndex(ushort oldIndex, ushort newIndex, char represents)
			{
				this.OldIndex = oldIndex;
				this.NewIndex = (byte)newIndex;
				this.Represents = represents;
			}

			// Token: 0x06000525 RID: 1317 RVA: 0x0001F92C File Offset: 0x0001DB2C
			public override string ToString()
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(12, 3);
				defaultInterpolatedStringHandler.AppendFormatted<char>(this.Represents);
				defaultInterpolatedStringHandler.AppendLiteral(": From ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(this.OldIndex);
				defaultInterpolatedStringHandler.AppendLiteral(" To ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(this.NewIndex);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}
	}
}
