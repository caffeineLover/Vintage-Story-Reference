using System;
using System.Collections.Generic;
using System.IO;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.TrueType.Tables.CMapSubTables;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x02000036 RID: 54
	public class CMapTable : ITrueTypeTable, IWriteable
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00008B5C File Offset: 0x00006D5C
		public string Tag
		{
			get
			{
				return "cmap";
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00008B63 File Offset: 0x00006D63
		public TrueTypeHeaderTable DirectoryTable { get; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00008B6B File Offset: 0x00006D6B
		public ushort Version { get; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00008B73 File Offset: 0x00006D73
		public IReadOnlyList<ICMapSubTable> SubTables { get; }

		// Token: 0x0600016A RID: 362 RVA: 0x00008B7B File Offset: 0x00006D7B
		public CMapTable(ushort version, TrueTypeHeaderTable directoryTable, IReadOnlyList<ICMapSubTable> subTables)
		{
			this.SubTables = subTables;
			this.Version = version;
			this.DirectoryTable = directoryTable;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00008B98 File Offset: 0x00006D98
		public bool TryGetGlyphIndex(int characterCode, out int glyphIndex)
		{
			glyphIndex = 0;
			if (this.SubTables.Count == 0)
			{
				return false;
			}
			ICMapSubTable icmapSubTable = null;
			foreach (ICMapSubTable icmapSubTable2 in this.SubTables)
			{
				glyphIndex = icmapSubTable2.CharacterCodeToGlyphIndex(characterCode);
				if (glyphIndex != 0)
				{
					return true;
				}
				if (icmapSubTable2.EncodingId == 0 && icmapSubTable2.PlatformId == TrueTypeCMapPlatform.Windows)
				{
					icmapSubTable = icmapSubTable2;
				}
			}
			if (icmapSubTable != null && characterCode >= 0 && characterCode <= 255)
			{
				glyphIndex = icmapSubTable.CharacterCodeToGlyphIndex(characterCode + 61440);
				if (glyphIndex != 0)
				{
					return true;
				}
				glyphIndex = icmapSubTable.CharacterCodeToGlyphIndex(characterCode + 61696);
				if (glyphIndex != 0)
				{
					return true;
				}
				glyphIndex = icmapSubTable.CharacterCodeToGlyphIndex(characterCode + 61952);
				if (glyphIndex != 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00008C68 File Offset: 0x00006E68
		public void Write(Stream stream)
		{
			long position = stream.Position;
			WritingExtensions.WriteUShort(stream, this.Version);
			WritingExtensions.WriteUShort(stream, this.SubTables.Count);
			long[] array = new long[this.SubTables.Count];
			for (int i = 0; i < this.SubTables.Count; i++)
			{
				ICMapSubTable icmapSubTable = this.SubTables[i];
				WritingExtensions.WriteUShort(stream, (ushort)icmapSubTable.PlatformId);
				WritingExtensions.WriteUShort(stream, icmapSubTable.EncodingId);
				array[i] = stream.Position;
				WritingExtensions.WriteUInt(stream, 0U);
			}
			long[] array2 = new long[this.SubTables.Count];
			for (int j = 0; j < this.SubTables.Count; j++)
			{
				ICMapSubTable icmapSubTable2 = this.SubTables[j];
				IWriteable writeable = icmapSubTable2 as IWriteable;
				if (writeable == null)
				{
					throw new InvalidOperationException("Cannot write subtable of type: " + icmapSubTable2.GetType().Name + ".");
				}
				array2[j] = stream.Position - position;
				writeable.Write(stream);
			}
			long position2 = stream.Position;
			for (int k = 0; k < array.Length; k++)
			{
				long num = array2[k];
				stream.Seek(array[k], 0);
				WritingExtensions.WriteUInt(stream, num);
			}
			stream.Seek(position2, 0);
		}
	}
}
