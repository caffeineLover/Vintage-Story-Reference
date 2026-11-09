using System;
using System.IO;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.TrueType
{
	// Token: 0x02000034 RID: 52
	public struct TrueTypeHeaderTable : IWriteable
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00007F17 File Offset: 0x00006117
		public readonly string Tag { get; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00007F1F File Offset: 0x0000611F
		public readonly uint CheckSum { get; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600015F RID: 351 RVA: 0x00007F27 File Offset: 0x00006127
		public readonly uint Offset { get; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00007F2F File Offset: 0x0000612F
		public readonly uint Length { get; }

		// Token: 0x06000161 RID: 353 RVA: 0x00007F38 File Offset: 0x00006138
		public TrueTypeHeaderTable(string tag, uint checkSum, uint offset, uint length)
		{
			if (tag == null)
			{
				throw new ArgumentNullException("tag");
			}
			if (tag.Length != 4)
			{
				throw new ArgumentException("A TrueType table tag must be a uint32, 4 bytes long, instead got: " + tag + ".", "tag");
			}
			this.Tag = tag;
			this.CheckSum = checkSum;
			this.Offset = offset;
			this.Length = length;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007F94 File Offset: 0x00006194
		public static TrueTypeHeaderTable GetEmptyHeaderTable(string tag)
		{
			return new TrueTypeHeaderTable(tag, 0U, 0U, 0U);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00007FA0 File Offset: 0x000061A0
		public void Write(Stream stream)
		{
			for (int i = 0; i < this.Tag.Length; i++)
			{
				stream.WriteByte((byte)this.Tag.get_Chars(i));
			}
			WritingExtensions.WriteUInt(stream, this.CheckSum);
			WritingExtensions.WriteUInt(stream, this.Offset);
			WritingExtensions.WriteUInt(stream, this.Length);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00007FFC File Offset: 0x000061FC
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(31, 4);
			defaultInterpolatedStringHandler.AppendFormatted(this.Tag);
			defaultInterpolatedStringHandler.AppendLiteral(" - Offset: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.Offset);
			defaultInterpolatedStringHandler.AppendLiteral(" Length: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.Length);
			defaultInterpolatedStringHandler.AppendLiteral(" Checksum: ");
			defaultInterpolatedStringHandler.AppendFormatted<uint>(this.CheckSum);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04000119 RID: 281
		public const string Cmap = "cmap";

		// Token: 0x0400011A RID: 282
		public const string Glyf = "glyf";

		// Token: 0x0400011B RID: 283
		public const string Head = "head";

		// Token: 0x0400011C RID: 284
		public const string Hhea = "hhea";

		// Token: 0x0400011D RID: 285
		public const string Hmtx = "hmtx";

		// Token: 0x0400011E RID: 286
		public const string Loca = "loca";

		// Token: 0x0400011F RID: 287
		public const string Maxp = "maxp";

		// Token: 0x04000120 RID: 288
		public const string Name = "name";

		// Token: 0x04000121 RID: 289
		public const string Post = "post";

		// Token: 0x04000122 RID: 290
		public const string Os2 = "OS/2";

		// Token: 0x04000123 RID: 291
		public const string Cvt = "cvt ";

		// Token: 0x04000124 RID: 292
		public const string Ebdt = "EBDT";

		// Token: 0x04000125 RID: 293
		public const string Eblc = "EBLC";

		// Token: 0x04000126 RID: 294
		public const string Ebsc = "EBSC";

		// Token: 0x04000127 RID: 295
		public const string Fpgm = "fpgm";

		// Token: 0x04000128 RID: 296
		public const string Gasp = "gasp";

		// Token: 0x04000129 RID: 297
		public const string Hdmx = "hdmx";

		// Token: 0x0400012A RID: 298
		public const string Kern = "kern";

		// Token: 0x0400012B RID: 299
		public const string Ltsh = "LTSH";

		// Token: 0x0400012C RID: 300
		public const string Prep = "prep";

		// Token: 0x0400012D RID: 301
		public const string Pclt = "PCLT";

		// Token: 0x0400012E RID: 302
		public const string Vdmx = "VDMX";

		// Token: 0x0400012F RID: 303
		public const string Vhea = "vhea";

		// Token: 0x04000130 RID: 304
		public const string Vmtx = "vmtx";

		// Token: 0x04000131 RID: 305
		public const string Cff = "cff ";
	}
}
