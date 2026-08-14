using System;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.Dictionaries
{
	// Token: 0x0200009A RID: 154
	internal class CidFontOperators
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00019EE8 File Offset: 0x000180E8
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x00019EF0 File Offset: 0x000180F0
		public RegistryOrderingSupplement Ros { get; set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x00019EF9 File Offset: 0x000180F9
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x00019F01 File Offset: 0x00018101
		public int Version { get; set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x00019F0A File Offset: 0x0001810A
		// (set) Token: 0x060003A9 RID: 937 RVA: 0x00019F12 File Offset: 0x00018112
		public int Revision { get; set; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060003AA RID: 938 RVA: 0x00019F1B File Offset: 0x0001811B
		// (set) Token: 0x060003AB RID: 939 RVA: 0x00019F23 File Offset: 0x00018123
		public int Type { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060003AC RID: 940 RVA: 0x00019F2C File Offset: 0x0001812C
		// (set) Token: 0x060003AD RID: 941 RVA: 0x00019F34 File Offset: 0x00018134
		public int Count { get; set; } = 8720;

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060003AE RID: 942 RVA: 0x00019F3D File Offset: 0x0001813D
		// (set) Token: 0x060003AF RID: 943 RVA: 0x00019F45 File Offset: 0x00018145
		public decimal UidBase { get; set; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x00019F4E File Offset: 0x0001814E
		// (set) Token: 0x060003B1 RID: 945 RVA: 0x00019F56 File Offset: 0x00018156
		public int FontDictionaryArray { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x00019F5F File Offset: 0x0001815F
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x00019F67 File Offset: 0x00018167
		public int FontDictionarySelect { get; set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x00019F70 File Offset: 0x00018170
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x00019F78 File Offset: 0x00018178
		public string FontName { get; set; }
	}
}
