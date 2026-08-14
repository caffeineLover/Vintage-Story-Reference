using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.TrueType.Glyphs;
using UglyToad.PdfPig.Fonts.TrueType.Parser;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x02000037 RID: 55
	internal class GlyphDataTable : ITrueTypeTable
	{
		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600016D RID: 365 RVA: 0x00008DB1 File Offset: 0x00006FB1
		public string Tag
		{
			get
			{
				return "glyf";
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00008DB8 File Offset: 0x00006FB8
		public TrueTypeHeaderTable DirectoryTable { get; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00008DC0 File Offset: 0x00006FC0
		public IReadOnlyList<IGlyphDescription> Glyphs
		{
			get
			{
				return this.glyphs.Value;
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00008DD0 File Offset: 0x00006FD0
		public GlyphDataTable(TrueTypeHeaderTable directoryTable, IReadOnlyList<uint> glyphOffsets, PdfRectangle maxGlyphBounds, TrueTypeDataBytes tableBytes)
		{
			this.glyphOffsets = glyphOffsets;
			this.maxGlyphBounds = maxGlyphBounds;
			this.tableBytes = tableBytes;
			this.DirectoryTable = directoryTable;
			if (tableBytes == null)
			{
				throw new ArgumentNullException("tableBytes");
			}
			if (glyphOffsets == null)
			{
				throw new ArgumentNullException("glyphOffsets");
			}
			if (tableBytes.Length != (long)((ulong)directoryTable.Length))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(77, 2);
				defaultInterpolatedStringHandler.AppendLiteral("glyf table data should match length of directory entry. Expected: ");
				defaultInterpolatedStringHandler.AppendFormatted<uint>(directoryTable.Length);
				defaultInterpolatedStringHandler.AppendLiteral(". Actual: ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(tableBytes.Length);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.glyphs = new Lazy<IReadOnlyList<IGlyphDescription>>(new Func<IReadOnlyList<IGlyphDescription>>(this.ReadGlyphs));
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00008E9C File Offset: 0x0000709C
		public bool TryGetGlyphBounds(int glyphIndex, out PdfRectangle bounds)
		{
			bounds = default(PdfRectangle);
			if (glyphIndex < 0 || glyphIndex >= this.glyphOffsets.Count - 1)
			{
				return false;
			}
			uint num = this.glyphOffsets[glyphIndex];
			if (this.glyphOffsets[glyphIndex + 1] <= num)
			{
				bounds = new PdfRectangle(0, 0, 0, 0);
				return true;
			}
			this.tableBytes.Seek((long)((ulong)num));
			this.tableBytes.ReadSignedShort();
			short num2 = this.tableBytes.ReadSignedShort();
			short num3 = this.tableBytes.ReadSignedShort();
			short num4 = this.tableBytes.ReadSignedShort();
			short num5 = this.tableBytes.ReadSignedShort();
			bounds = new PdfRectangle(num2, num3, num4, num5);
			return true;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00008F50 File Offset: 0x00007150
		public static GlyphDataTable Load(TrueTypeDataBytes data, TrueTypeHeaderTable table, TableRegister.Builder tableRegister)
		{
			data.Seek((long)((ulong)table.Offset));
			byte[] bytes = data.ReadByteArray((int)table.Length);
			return new GlyphDataTable(table, tableRegister.IndexToLocationTable.GlyphOffsets, tableRegister.HeaderTable.Bounds, new TrueTypeDataBytes(bytes));
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00008F9C File Offset: 0x0000719C
		private IReadOnlyList<IGlyphDescription> ReadGlyphs()
		{
			if (this.tableBytes == null)
			{
				throw new InvalidOperationException("Bytes cache was discarded before lazy value evaluated.");
			}
			TrueTypeDataBytes trueTypeDataBytes = this.tableBytes;
			IReadOnlyList<uint> readOnlyList = this.glyphOffsets;
			int num = readOnlyList.Count - 1;
			IGlyphDescription[] array = new IGlyphDescription[num];
			IGlyphDescription glyphDescription = Glyph.Empty(this.maxGlyphBounds);
			Dictionary<int, GlyphDataTable.TemporaryCompositeLocation> dictionary = new Dictionary<int, GlyphDataTable.TemporaryCompositeLocation>();
			for (int i = 0; i < num; i++)
			{
				if (readOnlyList[i + 1] <= readOnlyList[i])
				{
					array[i] = glyphDescription;
				}
				else
				{
					trueTypeDataBytes.Seek((long)((ulong)readOnlyList[i]));
					short num2 = trueTypeDataBytes.ReadSignedShort();
					short num3 = trueTypeDataBytes.ReadSignedShort();
					short num4 = trueTypeDataBytes.ReadSignedShort();
					short num5 = trueTypeDataBytes.ReadSignedShort();
					short num6 = trueTypeDataBytes.ReadSignedShort();
					PdfRectangle bounds;
					bounds..ctor(num3, num4, num5, num6);
					if (num2 >= 0)
					{
						array[i] = GlyphDataTable.ReadSimpleGlyph(trueTypeDataBytes, num2, bounds);
					}
					else
					{
						dictionary.Add(i, new GlyphDataTable.TemporaryCompositeLocation(trueTypeDataBytes.Position, bounds, num2));
					}
				}
			}
			foreach (KeyValuePair<int, GlyphDataTable.TemporaryCompositeLocation> keyValuePair in dictionary)
			{
				array[keyValuePair.Key] = GlyphDataTable.ReadCompositeGlyph(trueTypeDataBytes, keyValuePair.Value, dictionary, array, glyphDescription);
			}
			this.tableBytes = null;
			return array;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000090F4 File Offset: 0x000072F4
		private static Glyph ReadSimpleGlyph(TrueTypeDataBytes data, short contourCount, PdfRectangle bounds)
		{
			if (contourCount == 0)
			{
				return new Glyph(true, EmptyArray<byte>.Instance, EmptyArray<ushort>.Instance, EmptyArray<GlyphPoint>.Instance, new PdfRectangle(0, 0, 0, 0));
			}
			ushort[] array = data.ReadUnsignedShortArray((int)contourCount);
			ushort length = data.ReadUnsignedShort();
			byte[] instructions = data.ReadByteArray((int)length);
			int pointCount = 0;
			if (contourCount > 0)
			{
				pointCount = (int)(array[(int)(contourCount - 1)] + 1);
			}
			SimpleGlyphFlags[] array2 = GlyphDataTable.ReadFlags(data, pointCount);
			short[] array3 = GlyphDataTable.ReadCoordinates(data, pointCount, array2, SimpleGlyphFlags.XSingleByte, SimpleGlyphFlags.ThisXIsTheSame);
			short[] array4 = GlyphDataTable.ReadCoordinates(data, pointCount, array2, SimpleGlyphFlags.YSingleByte, SimpleGlyphFlags.ThisYIsTheSame);
			int num = array.Length - 1;
			int num2 = -1;
			GlyphPoint[] array5 = new GlyphPoint[array3.Length];
			for (int i = array3.Length - 1; i >= 0; i--)
			{
				if (num2 == -1)
				{
					num2 = (int)array[num];
				}
				bool flag = num2 == i;
				if (flag && num > 0)
				{
					num--;
					num2 = -1;
				}
				bool isOnCurve = (array2[i] & SimpleGlyphFlags.OnCurve) == SimpleGlyphFlags.OnCurve;
				array5[i] = new GlyphPoint(array3[i], array4[i], isOnCurve, flag);
			}
			return new Glyph(true, instructions, array, array5, bounds);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000091F0 File Offset: 0x000073F0
		private static IGlyphDescription ReadCompositeGlyph(TrueTypeDataBytes data, GlyphDataTable.TemporaryCompositeLocation compositeLocation, Dictionary<int, GlyphDataTable.TemporaryCompositeLocation> compositeLocations, IGlyphDescription[] glyphs, IGlyphDescription emptyGlyph)
		{
			data.Seek(compositeLocation.Position);
			List<GlyphDataTable.CompositeComponent> list = new List<GlyphDataTable.CompositeComponent>();
			ushort num;
			for (;;)
			{
				CompositeGlyphFlags value = (CompositeGlyphFlags)data.ReadUnsignedShort();
				num = data.ReadUnsignedShort();
				if (glyphs[(int)num] == null)
				{
					GlyphDataTable.TemporaryCompositeLocation compositeLocation2;
					if (!compositeLocations.TryGetValue((int)num, ref compositeLocation2))
					{
						break;
					}
					long position = data.Position;
					IGlyphDescription glyphDescription = GlyphDataTable.ReadCompositeGlyph(data, compositeLocation2, compositeLocations, glyphs, emptyGlyph);
					data.Seek(position);
					glyphs[(int)num] = glyphDescription;
				}
				short num2;
				short num3;
				if (GlyphDataTable.<ReadCompositeGlyph>g__HasFlag|16_0(value, CompositeGlyphFlags.Args1And2AreWords))
				{
					num2 = data.ReadSignedShort();
					num3 = data.ReadSignedShort();
				}
				else
				{
					num2 = (short)data.ReadByte();
					num3 = (short)data.ReadByte();
				}
				double num4 = 1.0;
				double r0C = 0.0;
				double r1C = 0.0;
				double r1C2 = 1.0;
				if (GlyphDataTable.<ReadCompositeGlyph>g__HasFlag|16_0(value, CompositeGlyphFlags.WeHaveAScale))
				{
					num4 = GlyphDataTable.ReadTwoFourteenFormat(data);
					r1C2 = num4;
				}
				else if (GlyphDataTable.<ReadCompositeGlyph>g__HasFlag|16_0(value, CompositeGlyphFlags.WeHaveAnXAndYScale))
				{
					num4 = GlyphDataTable.ReadTwoFourteenFormat(data);
					r1C2 = GlyphDataTable.ReadTwoFourteenFormat(data);
				}
				else if (GlyphDataTable.<ReadCompositeGlyph>g__HasFlag|16_0(value, CompositeGlyphFlags.WeHaveATwoByTwo))
				{
					num4 = GlyphDataTable.ReadTwoFourteenFormat(data);
					r0C = GlyphDataTable.ReadTwoFourteenFormat(data);
					r1C = GlyphDataTable.ReadTwoFourteenFormat(data);
					r1C2 = GlyphDataTable.ReadTwoFourteenFormat(data);
				}
				if (GlyphDataTable.<ReadCompositeGlyph>g__HasFlag|16_0(value, CompositeGlyphFlags.ArgsAreXAndYValues))
				{
					list.Add(new GlyphDataTable.CompositeComponent((int)num, new CompositeTransformMatrix3By2(num4, r0C, r1C, r1C2, (double)num2, (double)num3)));
				}
				if (!GlyphDataTable.<ReadCompositeGlyph>g__HasFlag|16_0(value, CompositeGlyphFlags.MoreComponents))
				{
					goto Block_8;
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(109, 1);
			defaultInterpolatedStringHandler.AppendLiteral("The composite glyph required a contour at index ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(num);
			defaultInterpolatedStringHandler.AppendLiteral(" but there was no simple or composite glyph at this location.");
			throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			Block_8:
			IGlyphDescription glyphDescription2 = null;
			foreach (GlyphDataTable.CompositeComponent compositeComponent in list)
			{
				IGlyphDescription glyphDescription3 = glyphs[compositeComponent.Index].Transform(compositeComponent.Transformation);
				if (glyphDescription2 == null)
				{
					glyphDescription2 = glyphDescription3;
				}
				else
				{
					glyphDescription2 = glyphDescription2.Merge(glyphDescription3);
				}
			}
			glyphDescription2 = (glyphDescription2 ?? emptyGlyph);
			return new Glyph(false, glyphDescription2.Instructions, glyphDescription2.EndPointsOfContours, glyphDescription2.Points, compositeLocation.Bounds);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00009408 File Offset: 0x00007608
		private static SimpleGlyphFlags[] ReadFlags(TrueTypeDataBytes data, int pointCount)
		{
			SimpleGlyphFlags[] array = new SimpleGlyphFlags[pointCount];
			for (int i = 0; i < pointCount; i++)
			{
				array[i] = (SimpleGlyphFlags)data.ReadByte();
				if (array[i].HasFlag(SimpleGlyphFlags.Repeat))
				{
					byte b = data.ReadByte();
					for (int j = 0; j < (int)b; j++)
					{
						array[i + j + 1] = array[i];
					}
					i += (int)b;
				}
			}
			return array;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00009468 File Offset: 0x00007668
		private static short[] ReadCoordinates(TrueTypeDataBytes data, int pointCount, SimpleGlyphFlags[] flags, SimpleGlyphFlags isByte, SimpleGlyphFlags signOrSame)
		{
			short[] array = new short[pointCount];
			int num = 0;
			for (int i = 0; i < pointCount; i++)
			{
				SimpleGlyphFlags value = flags[i];
				int num2;
				if (GlyphDataTable.<ReadCoordinates>g__HasFlag|18_0(value, isByte))
				{
					byte b = data.ReadByte();
					num2 = (int)(GlyphDataTable.<ReadCoordinates>g__HasFlag|18_0(value, signOrSame) ? b : (-(int)b));
				}
				else if (GlyphDataTable.<ReadCoordinates>g__HasFlag|18_0(value, signOrSame))
				{
					num2 = 0;
				}
				else
				{
					num2 = (int)data.ReadSignedShort();
				}
				num += num2;
				array[i] = (short)num;
			}
			return array;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000094D6 File Offset: 0x000076D6
		private static double ReadTwoFourteenFormat(TrueTypeDataBytes data)
		{
			return (double)data.ReadSignedShort() / 16384.0;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000094E9 File Offset: 0x000076E9
		[CompilerGenerated]
		internal static bool <ReadCompositeGlyph>g__HasFlag|16_0(CompositeGlyphFlags value, CompositeGlyphFlags target)
		{
			return (value & target) == target;
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000094F1 File Offset: 0x000076F1
		[CompilerGenerated]
		internal static bool <ReadCoordinates>g__HasFlag|18_0(SimpleGlyphFlags value, SimpleGlyphFlags target)
		{
			return (value & target) == target;
		}

		// Token: 0x0400013C RID: 316
		private readonly IReadOnlyList<uint> glyphOffsets;

		// Token: 0x0400013D RID: 317
		private readonly PdfRectangle maxGlyphBounds;

		// Token: 0x0400013E RID: 318
		private TrueTypeDataBytes tableBytes;

		// Token: 0x04000140 RID: 320
		private readonly Lazy<IReadOnlyList<IGlyphDescription>> glyphs;

		// Token: 0x020000C4 RID: 196
		private struct TemporaryCompositeLocation
		{
			// Token: 0x1700022F RID: 559
			// (get) Token: 0x060004FE RID: 1278 RVA: 0x0001F667 File Offset: 0x0001D867
			public readonly long Position { get; }

			// Token: 0x17000230 RID: 560
			// (get) Token: 0x060004FF RID: 1279 RVA: 0x0001F66F File Offset: 0x0001D86F
			public readonly PdfRectangle Bounds { get; }

			// Token: 0x06000500 RID: 1280 RVA: 0x0001F678 File Offset: 0x0001D878
			public TemporaryCompositeLocation(long position, PdfRectangle bounds, short contourCount)
			{
				if (contourCount >= 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(66, 1);
					defaultInterpolatedStringHandler.AppendLiteral("A composite glyph should not have a positive contour count. Got: ");
					defaultInterpolatedStringHandler.AppendFormatted<short>(contourCount);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), "contourCount");
				}
				this.Position = position;
				this.Bounds = bounds;
			}
		}

		// Token: 0x020000C5 RID: 197
		private class CompositeComponent
		{
			// Token: 0x17000231 RID: 561
			// (get) Token: 0x06000501 RID: 1281 RVA: 0x0001F6D3 File Offset: 0x0001D8D3
			public int Index { get; }

			// Token: 0x17000232 RID: 562
			// (get) Token: 0x06000502 RID: 1282 RVA: 0x0001F6DB File Offset: 0x0001D8DB
			public CompositeTransformMatrix3By2 Transformation { get; }

			// Token: 0x06000503 RID: 1283 RVA: 0x0001F6E3 File Offset: 0x0001D8E3
			public CompositeComponent(int index, CompositeTransformMatrix3By2 transformation)
			{
				this.Index = index;
				this.Transformation = transformation;
			}
		}
	}
}
