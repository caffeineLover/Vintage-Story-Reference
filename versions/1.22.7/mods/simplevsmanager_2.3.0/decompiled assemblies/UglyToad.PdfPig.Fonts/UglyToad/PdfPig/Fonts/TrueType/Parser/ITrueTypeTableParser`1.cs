using System;
using UglyToad.PdfPig.Fonts.TrueType.Tables;

namespace UglyToad.PdfPig.Fonts.TrueType.Parser
{
	// Token: 0x02000058 RID: 88
	internal interface ITrueTypeTableParser<out T> where T : ITrueTypeTable
	{
		// Token: 0x0600025B RID: 603
		T Parse(TrueTypeHeaderTable header, TrueTypeDataBytes data, TableRegister.Builder register);
	}
}
