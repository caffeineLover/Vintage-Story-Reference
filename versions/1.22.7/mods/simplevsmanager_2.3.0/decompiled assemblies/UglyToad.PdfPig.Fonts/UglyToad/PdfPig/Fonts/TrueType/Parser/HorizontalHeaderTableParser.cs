using System;
using UglyToad.PdfPig.Fonts.TrueType.Tables;

namespace UglyToad.PdfPig.Fonts.TrueType.Parser
{
	// Token: 0x02000056 RID: 86
	internal class HorizontalHeaderTableParser : ITrueTypeTableParser<HorizontalHeaderTable>
	{
		// Token: 0x06000257 RID: 599 RVA: 0x0000C0A4 File Offset: 0x0000A2A4
		public HorizontalHeaderTable Parse(TrueTypeHeaderTable header, TrueTypeDataBytes data, TableRegister.Builder register)
		{
			data.Seek((long)((ulong)header.Offset));
			ushort majorVersion = data.ReadUnsignedShort();
			ushort minorVersion = data.ReadUnsignedShort();
			short ascent = data.ReadSignedShort();
			short descent = data.ReadSignedShort();
			short lineGap = data.ReadSignedShort();
			ushort advanceWidthMaximum = data.ReadUnsignedShort();
			short minimumLeftSideBearing = data.ReadSignedShort();
			short minimumRightSideBearing = data.ReadSignedShort();
			short xMaxExtent = data.ReadSignedShort();
			short caretSlopeRise = data.ReadSignedShort();
			short caretSlopeRun = data.ReadSignedShort();
			short caretOffset = data.ReadSignedShort();
			data.ReadSignedShort();
			data.ReadSignedShort();
			data.ReadSignedShort();
			data.ReadSignedShort();
			short num = data.ReadSignedShort();
			if (num != 0)
			{
				throw new NotSupportedException("The metric data format for a horizontal header table should be 0.");
			}
			ushort numberOfHeaderMetrics = data.ReadUnsignedShort();
			return new HorizontalHeaderTable(header, (int)majorVersion, (int)minorVersion, ascent, descent, lineGap, advanceWidthMaximum, minimumLeftSideBearing, minimumRightSideBearing, xMaxExtent, caretSlopeRise, caretSlopeRun, caretOffset, num, numberOfHeaderMetrics);
		}
	}
}
