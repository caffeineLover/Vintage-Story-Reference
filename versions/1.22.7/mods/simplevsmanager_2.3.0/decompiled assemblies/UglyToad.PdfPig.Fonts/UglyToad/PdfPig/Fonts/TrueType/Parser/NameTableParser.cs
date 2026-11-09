using System;
using System.Text;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.TrueType.Names;
using UglyToad.PdfPig.Fonts.TrueType.Tables;

namespace UglyToad.PdfPig.Fonts.TrueType.Parser
{
	// Token: 0x02000059 RID: 89
	internal class NameTableParser : ITrueTypeTableParser<NameTable>
	{
		// Token: 0x0600025C RID: 604 RVA: 0x0000C244 File Offset: 0x0000A444
		public NameTable Parse(TrueTypeHeaderTable header, TrueTypeDataBytes data, TableRegister.Builder register)
		{
			data.Seek((long)((ulong)header.Offset));
			data.ReadUnsignedShort();
			ushort num = data.ReadUnsignedShort();
			ushort num2 = data.ReadUnsignedShort();
			NameTableParser.NameRecordBuilder[] array = new NameTableParser.NameRecordBuilder[(int)num];
			for (int i = 0; i < (int)num; i++)
			{
				array[i] = NameTableParser.NameRecordBuilder.Read(data);
			}
			TrueTypeNameRecord[] array2 = new TrueTypeNameRecord[(int)num];
			uint offset = header.Offset + (uint)num2;
			for (int j = 0; j < (int)num; j++)
			{
				array2[j] = NameTableParser.GetTrueTypeNameRecord(array[j], data, offset);
			}
			return new NameTable(header, NameTableParser.GetName(4, array2), NameTableParser.GetName(1, array2), NameTableParser.GetName(2, array2), array2);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000C2EC File Offset: 0x0000A4EC
		private static TrueTypeNameRecord GetTrueTypeNameRecord(NameTableParser.NameRecordBuilder nameRecord, TrueTypeDataBytes data, uint offset)
		{
			TrueTypeNameRecord result;
			try
			{
				Encoding encoding = OtherEncodings.Iso88591;
				switch (nameRecord.PlatformId)
				{
				case TrueTypePlatformIdentifier.Unicode:
					encoding = Encoding.BigEndianUnicode;
					break;
				case TrueTypePlatformIdentifier.Iso:
				{
					ushort platformEncodingId = nameRecord.PlatformEncodingId;
					if (platformEncodingId != 0)
					{
						if (platformEncodingId == 1)
						{
							encoding = Encoding.GetEncoding("ISO-10646-UCS-2");
						}
					}
					else
					{
						encoding = Encoding.GetEncoding("US-ASCII");
					}
					break;
				}
				case TrueTypePlatformIdentifier.Windows:
				{
					TrueTypeWindowsEncodingIdentifier platformEncodingId2 = (TrueTypeWindowsEncodingIdentifier)nameRecord.PlatformEncodingId;
					if (platformEncodingId2 == TrueTypeWindowsEncodingIdentifier.Symbol || platformEncodingId2 == TrueTypeWindowsEncodingIdentifier.UnicodeBmp)
					{
						encoding = Encoding.BigEndianUnicode;
					}
					break;
				}
				}
				uint num = offset + (uint)nameRecord.Offset;
				if ((ulong)num >= (ulong)data.Length)
				{
					result = null;
				}
				else
				{
					data.Seek((long)((ulong)num));
					string s;
					if (data.TryReadString((int)nameRecord.Length, encoding, out s))
					{
						result = nameRecord.ToNameRecord(s);
					}
					else
					{
						result = null;
					}
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000C3CC File Offset: 0x0000A5CC
		private static string GetName(int nameId, TrueTypeNameRecord[] names)
		{
			string text = null;
			string text2 = null;
			foreach (TrueTypeNameRecord trueTypeNameRecord in names)
			{
				if (trueTypeNameRecord != null && (int)trueTypeNameRecord.NameId == nameId)
				{
					if (trueTypeNameRecord.PlatformId == TrueTypePlatformIdentifier.Windows && trueTypeNameRecord.LanguageId == 409)
					{
						return trueTypeNameRecord.Value;
					}
					if (trueTypeNameRecord.PlatformId == TrueTypePlatformIdentifier.Windows)
					{
						text = trueTypeNameRecord.Value;
					}
					text2 = trueTypeNameRecord.Value;
				}
			}
			return text ?? text2;
		}

		// Token: 0x020000D1 RID: 209
		private struct NameRecordBuilder
		{
			// Token: 0x1700024D RID: 589
			// (get) Token: 0x0600052A RID: 1322 RVA: 0x0001F9CA File Offset: 0x0001DBCA
			public readonly TrueTypePlatformIdentifier PlatformId { get; }

			// Token: 0x1700024E RID: 590
			// (get) Token: 0x0600052B RID: 1323 RVA: 0x0001F9D2 File Offset: 0x0001DBD2
			public readonly ushort PlatformEncodingId { get; }

			// Token: 0x1700024F RID: 591
			// (get) Token: 0x0600052C RID: 1324 RVA: 0x0001F9DA File Offset: 0x0001DBDA
			private readonly ushort LanguageId { get; }

			// Token: 0x17000250 RID: 592
			// (get) Token: 0x0600052D RID: 1325 RVA: 0x0001F9E2 File Offset: 0x0001DBE2
			private readonly ushort NameId { get; }

			// Token: 0x17000251 RID: 593
			// (get) Token: 0x0600052E RID: 1326 RVA: 0x0001F9EA File Offset: 0x0001DBEA
			public readonly ushort Length { get; }

			// Token: 0x17000252 RID: 594
			// (get) Token: 0x0600052F RID: 1327 RVA: 0x0001F9F2 File Offset: 0x0001DBF2
			public readonly ushort Offset { get; }

			// Token: 0x06000530 RID: 1328 RVA: 0x0001F9FA File Offset: 0x0001DBFA
			private NameRecordBuilder(ushort platformId, ushort platformEncodingId, ushort languageId, ushort nameId, ushort length, ushort offset)
			{
				this.PlatformId = platformId;
				this.PlatformEncodingId = platformEncodingId;
				this.LanguageId = languageId;
				this.NameId = nameId;
				this.Length = length;
				this.Offset = offset;
			}

			// Token: 0x06000531 RID: 1329 RVA: 0x0001FA29 File Offset: 0x0001DC29
			public TrueTypeNameRecord ToNameRecord(string s)
			{
				return new TrueTypeNameRecord(this.PlatformId, this.PlatformEncodingId, this.LanguageId, this.NameId, s);
			}

			// Token: 0x06000532 RID: 1330 RVA: 0x0001FA49 File Offset: 0x0001DC49
			public static NameTableParser.NameRecordBuilder Read(TrueTypeDataBytes data)
			{
				return new NameTableParser.NameRecordBuilder(data.ReadUnsignedShort(), data.ReadUnsignedShort(), data.ReadUnsignedShort(), data.ReadUnsignedShort(), data.ReadUnsignedShort(), data.ReadUnsignedShort());
			}
		}
	}
}
