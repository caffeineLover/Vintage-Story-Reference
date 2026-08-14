using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.TrueType.Parser;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x0200003B RID: 59
	public class IndexToLocationTable : ITrueTypeTable, IWriteable
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000099A8 File Offset: 0x00007BA8
		public string Tag
		{
			get
			{
				return "loca";
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x000099AF File Offset: 0x00007BAF
		public TrueTypeHeaderTable DirectoryTable { get; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x000099B7 File Offset: 0x00007BB7
		public IndexToLocationTable.EntryFormat Format { get; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x000099BF File Offset: 0x00007BBF
		public IReadOnlyList<uint> GlyphOffsets { get; }

		// Token: 0x060001A9 RID: 425 RVA: 0x000099C7 File Offset: 0x00007BC7
		public IndexToLocationTable(TrueTypeHeaderTable directoryTable, IndexToLocationTable.EntryFormat format, IReadOnlyList<uint> glyphOffsets)
		{
			this.DirectoryTable = directoryTable;
			this.Format = format;
			if (glyphOffsets == null)
			{
				throw new ArgumentNullException("glyphOffsets");
			}
			this.GlyphOffsets = glyphOffsets;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000099F4 File Offset: 0x00007BF4
		internal static IndexToLocationTable Load(TrueTypeDataBytes data, TrueTypeHeaderTable table, TableRegister.Builder tableRegister)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (tableRegister == null)
			{
				throw new ArgumentNullException("tableRegister");
			}
			data.Seek((long)((ulong)table.Offset));
			HeaderTable headerTable = tableRegister.HeaderTable;
			BasicMaximumProfileTable maximumProfileTable = tableRegister.MaximumProfileTable;
			if (headerTable == null)
			{
				throw new InvalidFontFormatException("No header (head) table was defined in this font.");
			}
			if (maximumProfileTable == null)
			{
				throw new InvalidFontFormatException("No maximum profile (maxp) table was defined in this font.");
			}
			IndexToLocationTable.EntryFormat entryFormat = headerTable.IndexToLocFormat;
			int num = maximumProfileTable.NumberOfGlyphs + 1;
			if (entryFormat != IndexToLocationTable.EntryFormat.Short && entryFormat != IndexToLocationTable.EntryFormat.Long)
			{
				entryFormat = IndexToLocationTable.EntryFormat.Short;
			}
			uint[] array;
			if (entryFormat != IndexToLocationTable.EntryFormat.Short)
			{
				if (entryFormat != IndexToLocationTable.EntryFormat.Long)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(63, 1);
					defaultInterpolatedStringHandler.AppendLiteral("The format ");
					defaultInterpolatedStringHandler.AppendFormatted<IndexToLocationTable.EntryFormat>(entryFormat);
					defaultInterpolatedStringHandler.AppendLiteral(" was invalid for the index to location (loca) table.");
					throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				array = data.ReadUnsignedIntArray(num);
			}
			else
			{
				array = new uint[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = (uint)(data.ReadUnsignedShort() * 2);
				}
			}
			return new IndexToLocationTable(table, entryFormat, array);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00009AE4 File Offset: 0x00007CE4
		public void Write(Stream stream)
		{
			for (int i = 0; i < this.GlyphOffsets.Count; i++)
			{
				uint num = this.GlyphOffsets[i];
				IndexToLocationTable.EntryFormat format = this.Format;
				if (format != IndexToLocationTable.EntryFormat.Short)
				{
					if (format != IndexToLocationTable.EntryFormat.Long)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
						defaultInterpolatedStringHandler..ctor(63, 1);
						defaultInterpolatedStringHandler.AppendLiteral("The format ");
						defaultInterpolatedStringHandler.AppendFormatted<IndexToLocationTable.EntryFormat>(this.Format);
						defaultInterpolatedStringHandler.AppendLiteral(" was invalid for the index to location (loca) table.");
						throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					WritingExtensions.WriteUInt(stream, num);
				}
				else
				{
					WritingExtensions.WriteUShort(stream, (int)((ushort)num / 2));
				}
			}
		}

		// Token: 0x020000C8 RID: 200
		public enum EntryFormat : short
		{
			// Token: 0x04000471 RID: 1137
			Short,
			// Token: 0x04000472 RID: 1138
			Long
		}
	}
}
