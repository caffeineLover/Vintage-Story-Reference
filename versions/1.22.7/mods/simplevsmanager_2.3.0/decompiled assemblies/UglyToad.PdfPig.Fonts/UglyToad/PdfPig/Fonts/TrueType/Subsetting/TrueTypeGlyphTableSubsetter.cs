using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.TrueType.Glyphs;
using UglyToad.PdfPig.Fonts.TrueType.Tables;

namespace UglyToad.PdfPig.Fonts.TrueType.Subsetting
{
	// Token: 0x02000050 RID: 80
	internal static class TrueTypeGlyphTableSubsetter
	{
		// Token: 0x06000238 RID: 568 RVA: 0x0000AFD8 File Offset: 0x000091D8
		public static TrueTypeSubsetGlyphTable SubsetGlyphTable(TrueTypeFont font, byte[] fontBytes, TrueTypeSubsetter.OldToNewGlyphIndex[] mapping)
		{
			if (font == null)
			{
				throw new ArgumentNullException("font");
			}
			if (fontBytes == null)
			{
				throw new ArgumentNullException("fontBytes");
			}
			if (mapping == null)
			{
				throw new ArgumentNullException("mapping");
			}
			TrueTypeDataBytes trueTypeDataBytes = new TrueTypeDataBytes(fontBytes);
			HorizontalMetricsTable horizontalMetricsTable = font.TableRegister.HorizontalMetricsTable;
			if (horizontalMetricsTable == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(65, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Font: ");
				defaultInterpolatedStringHandler.AppendFormatted<TrueTypeFont>(font);
				defaultInterpolatedStringHandler.AppendLiteral(" did not contain a horizontal metrics table, cannot subset.");
				throw new InvalidFontFormatException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			TrueTypeGlyphTableSubsetter.GlyphRecord[] glyphRecordsInFont = TrueTypeGlyphTableSubsetter.GetGlyphRecordsInFont(font, trueTypeDataBytes);
			List<TrueTypeGlyphTableSubsetter.GlyphRecord> list = new List<TrueTypeGlyphTableSubsetter.GlyphRecord>(mapping.Length);
			List<int> list2 = new List<int>(mapping.Length);
			foreach (TrueTypeSubsetter.OldToNewGlyphIndex oldToNewGlyphIndex in mapping)
			{
				TrueTypeGlyphTableSubsetter.GlyphRecord glyphRecord = glyphRecordsInFont[(int)oldToNewGlyphIndex.OldIndex];
				list.Add(glyphRecord);
				list2.Add((int)oldToNewGlyphIndex.OldIndex);
			}
			List<uint> list3 = new List<uint>();
			List<HorizontalMetric> list4 = new List<HorizontalMetric>();
			List<ValueTuple<uint, ushort>> list5 = new List<ValueTuple<uint, ushort>>();
			TrueTypeSubsetGlyphTable result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				for (int j = 0; j < list.Count; j++)
				{
					list5.Clear();
					TrueTypeGlyphTableSubsetter.GlyphRecord glyphRecord2 = list[j];
					if (glyphRecord2.Type == TrueTypeGlyphTableSubsetter.GlyphType.Composite)
					{
						for (int k = 0; k < glyphRecord2.DependencyIndices.Count; k++)
						{
							TrueTypeGlyphTableSubsetter.CompositeGlyphIndexReference dependency = glyphRecord2.DependencyIndices[k];
							int? alreadyCopiedDependencyIndex = TrueTypeGlyphTableSubsetter.GetAlreadyCopiedDependencyIndex(dependency, list2);
							if (alreadyCopiedDependencyIndex == null)
							{
								TrueTypeGlyphTableSubsetter.GlyphRecord glyphRecord3 = glyphRecordsInFont[(int)dependency.Index];
								alreadyCopiedDependencyIndex = new int?(list.Count);
								list.Add(glyphRecord3);
								list2.Add((int)dependency.Index);
							}
							long num = (long)((ulong)dependency.OffsetOfIndexWithinData - (ulong)((long)glyphRecord2.Offset));
							list5.Add(new ValueTuple<uint, ushort>((uint)num, (ushort)alreadyCopiedDependencyIndex.Value));
						}
					}
					list3.Add((uint)memoryStream.Position);
					HorizontalMetric horizontalMetric = horizontalMetricsTable.HorizontalMetrics[list2[j]];
					list4.Add(horizontalMetric);
					if (glyphRecord2.Type != TrueTypeGlyphTableSubsetter.GlyphType.Empty)
					{
						trueTypeDataBytes.Seek((long)glyphRecord2.Offset);
						byte[] array = trueTypeDataBytes.ReadByteArray(glyphRecord2.DataLength);
						foreach (ValueTuple<uint, ushort> valueTuple in list5)
						{
							uint item = valueTuple.Item1;
							ushort item2 = valueTuple.Item2;
							array[(int)item] = (byte)(item2 >> 8);
							array[(int)(item + 1U)] = (byte)item2;
						}
						memoryStream.Write(array, 0, array.Length);
						int num2 = array.Length % 4;
						int num3 = (num2 == 0) ? 0 : (4 - num2);
						for (int l = 0; l < num3; l++)
						{
							memoryStream.WriteByte(0);
						}
					}
				}
				byte[] array2 = memoryStream.ToArray();
				list3.Add((uint)array2.Length);
				uint[] glyphOffsets = list3.ToArray();
				result = new TrueTypeSubsetGlyphTable(array2, glyphOffsets, list4.ToArray());
			}
			return result;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000B2FC File Offset: 0x000094FC
		private static TrueTypeGlyphTableSubsetter.GlyphRecord[] GetGlyphRecordsInFont(TrueTypeFont font, TrueTypeDataBytes data)
		{
			IndexToLocationTable indexToLocationTable = font.TableRegister.IndexToLocationTable;
			int num = indexToLocationTable.GlyphOffsets.Count - 1;
			TrueTypeHeaderTable directoryTable = font.TableRegister.GlyphTable.DirectoryTable;
			data.Seek((long)((ulong)directoryTable.Offset));
			TrueTypeGlyphTableSubsetter.GlyphRecord[] array = new TrueTypeGlyphTableSubsetter.GlyphRecord[num];
			for (int i = 0; i < num; i++)
			{
				int num2 = (int)(directoryTable.Offset + indexToLocationTable.GlyphOffsets[i]);
				if (indexToLocationTable.GlyphOffsets[i + 1] <= indexToLocationTable.GlyphOffsets[i])
				{
					array[i] = new TrueTypeGlyphTableSubsetter.GlyphRecord(num2);
				}
				else
				{
					data.Seek((long)num2);
					if ((long)num2 >= (long)((ulong)(directoryTable.Offset + directoryTable.Length)))
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						defaultInterpolatedStringHandler..ctor(91, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Failed to read expected number of glyphs ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(num);
						defaultInterpolatedStringHandler.AppendLiteral(", only got to index ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(i);
						defaultInterpolatedStringHandler.AppendLiteral(" before reaching end of input.");
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					short num3 = data.ReadSignedShort();
					TrueTypeGlyphTableSubsetter.GlyphType glyphType = (num3 >= 0) ? TrueTypeGlyphTableSubsetter.GlyphType.Simple : TrueTypeGlyphTableSubsetter.GlyphType.Composite;
					data.ReadSignedShort();
					data.ReadSignedShort();
					data.ReadSignedShort();
					data.ReadSignedShort();
					if (glyphType == TrueTypeGlyphTableSubsetter.GlyphType.Simple)
					{
						TrueTypeGlyphTableSubsetter.ReadSimpleGlyph(data, (int)num3);
						array[i] = new TrueTypeGlyphTableSubsetter.GlyphRecord(num2, glyphType, (int)(data.Position - (long)num2), null);
					}
					else
					{
						IReadOnlyList<TrueTypeGlyphTableSubsetter.CompositeGlyphIndexReference> dependentIndices = TrueTypeGlyphTableSubsetter.ReadCompositeGlyph(data);
						uint num4 = indexToLocationTable.GlyphOffsets[i + 1];
						data.Seek((long)((ulong)(directoryTable.Offset + num4 - 1U)));
						array[i] = new TrueTypeGlyphTableSubsetter.GlyphRecord(num2, glyphType, (int)(data.Position - (long)num2), dependentIndices);
					}
				}
			}
			return array;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000B4AC File Offset: 0x000096AC
		private static int? GetAlreadyCopiedDependencyIndex(TrueTypeGlyphTableSubsetter.CompositeGlyphIndexReference dependency, IReadOnlyList<int> copiedGlyphOriginalIndices)
		{
			for (int i = 0; i < copiedGlyphOriginalIndices.Count; i++)
			{
				if ((long)copiedGlyphOriginalIndices[i] == (long)((ulong)dependency.Index))
				{
					return new int?(i);
				}
			}
			return default(int?);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000B4EC File Offset: 0x000096EC
		private static void ReadSimpleGlyph(TrueTypeDataBytes data, int numberOfContours)
		{
			if (numberOfContours == 0)
			{
				return;
			}
			ushort[] array = new ushort[numberOfContours];
			for (int i = 0; i < numberOfContours; i++)
			{
				array[i] = data.ReadUnsignedShort();
			}
			ushort num = data.ReadUnsignedShort();
			byte[] array2 = new byte[(int)num];
			for (int j = 0; j < (int)num; j++)
			{
				array2[j] = data.ReadByte();
			}
			int num2 = (int)(array[numberOfContours - 1] + 1);
			SimpleGlyphFlags[] array3 = new SimpleGlyphFlags[num2];
			for (int k = 0; k < num2; k++)
			{
				SimpleGlyphFlags simpleGlyphFlags = (SimpleGlyphFlags)data.ReadByte();
				array3[k] = simpleGlyphFlags;
				if (TrueTypeGlyphTableSubsetter.<ReadSimpleGlyph>g__HasFlag|3_0(simpleGlyphFlags, SimpleGlyphFlags.Repeat))
				{
					byte b = data.ReadByte();
					for (int l = 0; l < (int)b; l++)
					{
						k++;
						array3[k] = simpleGlyphFlags;
					}
				}
			}
			TrueTypeGlyphTableSubsetter.ReadCoordinates(array3, data, SimpleGlyphFlags.XSingleByte, SimpleGlyphFlags.ThisXIsTheSame);
			TrueTypeGlyphTableSubsetter.ReadCoordinates(array3, data, SimpleGlyphFlags.YSingleByte, SimpleGlyphFlags.ThisYIsTheSame);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000B5C0 File Offset: 0x000097C0
		private static short[] ReadCoordinates(SimpleGlyphFlags[] flags, TrueTypeDataBytes data, SimpleGlyphFlags isSingleByte, SimpleGlyphFlags isTheSameAsPrevious)
		{
			short[] array = new short[flags.Length];
			int num = 0;
			for (int i = 0; i < flags.Length; i++)
			{
				SimpleGlyphFlags set = flags[i];
				if (TrueTypeGlyphTableSubsetter.<ReadCoordinates>g__HasFlag|4_0(set, isSingleByte))
				{
					byte b = data.ReadByte();
					if (TrueTypeGlyphTableSubsetter.<ReadCoordinates>g__HasFlag|4_0(set, isTheSameAsPrevious))
					{
						num += (int)b;
					}
					else
					{
						num -= (int)b;
					}
				}
				else
				{
					short num2;
					if (TrueTypeGlyphTableSubsetter.<ReadCoordinates>g__HasFlag|4_0(set, isTheSameAsPrevious))
					{
						num2 = 0;
					}
					else
					{
						num2 = data.ReadSignedShort();
					}
					num += (int)num2;
				}
				array[i] = (short)num;
			}
			return array;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000B634 File Offset: 0x00009834
		private static IReadOnlyList<TrueTypeGlyphTableSubsetter.CompositeGlyphIndexReference> ReadCompositeGlyph(TrueTypeDataBytes data)
		{
			List<TrueTypeGlyphTableSubsetter.CompositeGlyphIndexReference> list = new List<TrueTypeGlyphTableSubsetter.CompositeGlyphIndexReference>();
			CompositeGlyphFlags actual;
			do
			{
				actual = (CompositeGlyphFlags)data.ReadUnsignedShort();
				long position = data.Position;
				ushort index = data.ReadUnsignedShort();
				list.Add(new TrueTypeGlyphTableSubsetter.CompositeGlyphIndexReference((uint)index, (uint)position));
				if (TrueTypeGlyphTableSubsetter.<ReadCompositeGlyph>g__HasFlag|5_0(actual, CompositeGlyphFlags.Args1And2AreWords))
				{
					data.ReadSignedShort();
					data.ReadSignedShort();
				}
				else
				{
					data.ReadByte();
					data.ReadByte();
				}
				if (TrueTypeGlyphTableSubsetter.<ReadCompositeGlyph>g__HasFlag|5_0(actual, CompositeGlyphFlags.WeHaveAScale))
				{
					data.ReadSignedShort();
				}
				else if (TrueTypeGlyphTableSubsetter.<ReadCompositeGlyph>g__HasFlag|5_0(actual, CompositeGlyphFlags.WeHaveAnXAndYScale))
				{
					data.ReadSignedShort();
					data.ReadSignedShort();
				}
				else if (TrueTypeGlyphTableSubsetter.<ReadCompositeGlyph>g__HasFlag|5_0(actual, CompositeGlyphFlags.WeHaveATwoByTwo))
				{
					data.ReadSignedShort();
					data.ReadSignedShort();
					data.ReadSignedShort();
					data.ReadSignedShort();
				}
			}
			while (TrueTypeGlyphTableSubsetter.<ReadCompositeGlyph>g__HasFlag|5_0(actual, CompositeGlyphFlags.MoreComponents));
			return list;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000B6F4 File Offset: 0x000098F4
		[CompilerGenerated]
		internal static bool <ReadSimpleGlyph>g__HasFlag|3_0(SimpleGlyphFlags flags, SimpleGlyphFlags value)
		{
			return (flags & value) > (SimpleGlyphFlags)0;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000B6FC File Offset: 0x000098FC
		[CompilerGenerated]
		internal static bool <ReadCoordinates>g__HasFlag|4_0(SimpleGlyphFlags set, SimpleGlyphFlags f)
		{
			return (set & f) > (SimpleGlyphFlags)0;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000B704 File Offset: 0x00009904
		[CompilerGenerated]
		internal static bool <ReadCompositeGlyph>g__HasFlag|5_0(CompositeGlyphFlags actual, CompositeGlyphFlags value)
		{
			return (actual & value) > (CompositeGlyphFlags)0;
		}

		// Token: 0x020000CB RID: 203
		private class GlyphRecord
		{
			// Token: 0x1700023B RID: 571
			// (get) Token: 0x0600050F RID: 1295 RVA: 0x0001F7F9 File Offset: 0x0001D9F9
			public int Offset { get; }

			// Token: 0x1700023C RID: 572
			// (get) Token: 0x06000510 RID: 1296 RVA: 0x0001F801 File Offset: 0x0001DA01
			public TrueTypeGlyphTableSubsetter.GlyphType Type { get; }

			// Token: 0x1700023D RID: 573
			// (get) Token: 0x06000511 RID: 1297 RVA: 0x0001F809 File Offset: 0x0001DA09
			public int DataLength { get; }

			// Token: 0x1700023E RID: 574
			// (get) Token: 0x06000512 RID: 1298 RVA: 0x0001F811 File Offset: 0x0001DA11
			public IReadOnlyList<TrueTypeGlyphTableSubsetter.CompositeGlyphIndexReference> DependencyIndices { get; }

			// Token: 0x06000513 RID: 1299 RVA: 0x0001F819 File Offset: 0x0001DA19
			public GlyphRecord(int offset, TrueTypeGlyphTableSubsetter.GlyphType type, int dataLength, IReadOnlyList<TrueTypeGlyphTableSubsetter.CompositeGlyphIndexReference> dependentIndices = null)
			{
				this.Offset = offset;
				this.Type = type;
				this.DataLength = dataLength;
				this.DependencyIndices = (dependentIndices ?? EmptyArray<TrueTypeGlyphTableSubsetter.CompositeGlyphIndexReference>.Instance);
			}

			// Token: 0x06000514 RID: 1300 RVA: 0x0001F847 File Offset: 0x0001DA47
			public GlyphRecord(int offset)
			{
				this.Offset = offset;
				this.Type = 0;
				this.DataLength = 0;
				this.DependencyIndices = EmptyArray<TrueTypeGlyphTableSubsetter.CompositeGlyphIndexReference>.Instance;
			}
		}

		// Token: 0x020000CC RID: 204
		private enum GlyphType
		{
			// Token: 0x04000480 RID: 1152
			Empty,
			// Token: 0x04000481 RID: 1153
			Simple,
			// Token: 0x04000482 RID: 1154
			Composite
		}

		// Token: 0x020000CD RID: 205
		private struct CompositeGlyphIndexReference
		{
			// Token: 0x1700023F RID: 575
			// (get) Token: 0x06000515 RID: 1301 RVA: 0x0001F86F File Offset: 0x0001DA6F
			public readonly uint Index { get; }

			// Token: 0x17000240 RID: 576
			// (get) Token: 0x06000516 RID: 1302 RVA: 0x0001F877 File Offset: 0x0001DA77
			public readonly uint OffsetOfIndexWithinData { get; }

			// Token: 0x06000517 RID: 1303 RVA: 0x0001F87F File Offset: 0x0001DA7F
			public CompositeGlyphIndexReference(uint index, uint offsetOfIndexWithinData)
			{
				this.Index = index;
				this.OffsetOfIndexWithinData = offsetOfIndexWithinData;
			}
		}
	}
}
