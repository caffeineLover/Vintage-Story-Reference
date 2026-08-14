using System;
using System.Collections.Generic;
using System.Text;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x02000046 RID: 70
	public class PostScriptTable : ITrueTypeTable
	{
		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000A5B7 File Offset: 0x000087B7
		public string Tag
		{
			get
			{
				return "post";
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0000A5BE File Offset: 0x000087BE
		public TrueTypeHeaderTable DirectoryTable { get; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000A5C6 File Offset: 0x000087C6
		public float Format { get; }

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000A5CE File Offset: 0x000087CE
		public float ItalicAngle { get; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000204 RID: 516 RVA: 0x0000A5D6 File Offset: 0x000087D6
		public short UnderlinePosition { get; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000A5DE File Offset: 0x000087DE
		public short UnderlineThickness { get; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000A5E6 File Offset: 0x000087E6
		public uint IsFixedPitch { get; }

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000A5EE File Offset: 0x000087EE
		public uint MinimumMemoryType42 { get; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000A5F6 File Offset: 0x000087F6
		public uint MaximumMemoryType42 { get; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000A5FE File Offset: 0x000087FE
		public uint MinimumMemoryType1 { get; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600020A RID: 522 RVA: 0x0000A606 File Offset: 0x00008806
		public uint MaximumMemoryType1 { get; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000A60E File Offset: 0x0000880E
		public IReadOnlyList<string> GlyphNames { get; }

		// Token: 0x0600020C RID: 524 RVA: 0x0000A618 File Offset: 0x00008818
		public PostScriptTable(TrueTypeHeaderTable directoryTable, float format, float italicAngle, short underlinePosition, short underlineThickness, uint isFixedPitch, uint minimumMemoryType42, uint maximumMemoryType42, uint minimumMemoryType1, uint maximumMemoryType1, string[] glyphNames)
		{
			this.DirectoryTable = directoryTable;
			this.Format = format;
			this.ItalicAngle = italicAngle;
			this.UnderlinePosition = underlinePosition;
			this.UnderlineThickness = underlineThickness;
			this.IsFixedPitch = isFixedPitch;
			this.MinimumMemoryType42 = minimumMemoryType42;
			this.MaximumMemoryType42 = maximumMemoryType42;
			this.MinimumMemoryType1 = minimumMemoryType1;
			this.MaximumMemoryType1 = maximumMemoryType1;
			if (glyphNames == null)
			{
				throw new ArgumentNullException("glyphNames");
			}
			this.GlyphNames = glyphNames;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000A690 File Offset: 0x00008890
		internal static PostScriptTable Load(TrueTypeDataBytes data, TrueTypeHeaderTable table, BasicMaximumProfileTable maximumProfileTable)
		{
			data.Seek((long)((ulong)table.Offset));
			float num = data.Read32Fixed();
			float italicAngle = data.Read32Fixed();
			short underlinePosition = data.ReadSignedShort();
			short underlineThickness = data.ReadSignedShort();
			uint isFixedPitch = data.ReadUnsignedInt();
			uint minimumMemoryType = data.ReadUnsignedInt();
			uint maximumMemoryType = data.ReadUnsignedInt();
			uint minimumMemoryType2 = data.ReadUnsignedInt();
			uint maximumMemoryType2 = data.ReadUnsignedInt();
			string[] glyphNamesByFormat = PostScriptTable.GetGlyphNamesByFormat(data, maximumProfileTable, num);
			return new PostScriptTable(table, num, italicAngle, underlinePosition, underlineThickness, isFixedPitch, minimumMemoryType, maximumMemoryType, minimumMemoryType2, maximumMemoryType2, glyphNamesByFormat);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000A710 File Offset: 0x00008910
		private static string[] GetGlyphNamesByFormat(TrueTypeDataBytes data, BasicMaximumProfileTable maximumProfileTable, float formatType)
		{
			string[] array;
			if (Math.Abs(formatType - 1f) < 1E-45f)
			{
				array = new string[WindowsGlyphList4.NumberOfMacGlyphs];
				for (int i = 0; i < WindowsGlyphList4.MacGlyphNames.Count; i++)
				{
					array[i] = WindowsGlyphList4.MacGlyphNames[i];
				}
			}
			else if (Math.Abs(formatType - 2f) < 1E-45f)
			{
				array = PostScriptTable.GetFormat2GlyphNames(data);
			}
			else if (Math.Abs((double)formatType - 2.5) < 1.401298464324817E-45)
			{
				int[] array2 = new int[(maximumProfileTable != null) ? maximumProfileTable.NumberOfGlyphs : 0];
				for (int j = 0; j < array2.Length; j++)
				{
					int num = data.ReadSignedByte();
					array2[j] = j + 1 + num;
				}
				array = new string[array2.Length];
				for (int k = 0; k < array.Length; k++)
				{
					string text = WindowsGlyphList4.MacGlyphNames[array2[k]];
					if (text != null)
					{
						array[k] = text;
					}
				}
			}
			else if (Math.Abs(formatType - 3f) < 1E-45f)
			{
				array = EmptyArray<string>.Instance;
			}
			else
			{
				array = EmptyArray<string>.Instance;
			}
			return array;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000A828 File Offset: 0x00008A28
		private static string[] GetFormat2GlyphNames(TrueTypeDataBytes data)
		{
			ushort num = data.ReadUnsignedShort();
			int[] array = new int[(int)num];
			string[] array2 = new string[(int)num];
			int num2 = int.MinValue;
			for (int i = 0; i < (int)num; i++)
			{
				ushort num3 = data.ReadUnsignedShort();
				array[i] = (int)num3;
				if (num3 < 32768)
				{
					num2 = Math.Max(num2, (int)num3);
				}
			}
			string[] array3 = null;
			if (num2 >= WindowsGlyphList4.NumberOfMacGlyphs)
			{
				int num4 = num2 - WindowsGlyphList4.NumberOfMacGlyphs + 1;
				array3 = new string[num4];
				for (int j = 0; j < num4; j++)
				{
					byte bytesToRead = data.ReadByte();
					string text;
					if (data.TryReadString((int)bytesToRead, Encoding.UTF8, out text))
					{
						array3[j] = text;
					}
				}
			}
			for (int k = 0; k < (int)num; k++)
			{
				int num5 = array[k];
				if (num5 < WindowsGlyphList4.NumberOfMacGlyphs)
				{
					array2[k] = WindowsGlyphList4.MacGlyphNames[num5];
				}
				else if (num5 >= WindowsGlyphList4.NumberOfMacGlyphs && num5 < 32768)
				{
					if (array3 == null)
					{
						throw new InvalidOperationException("The name array was null despite the number of glyphs exceeding the maximum Mac Glyphs.");
					}
					array2[k] = array3[num5 - WindowsGlyphList4.NumberOfMacGlyphs];
				}
				else
				{
					array2[k] = ".undefined";
				}
			}
			return array2;
		}
	}
}
