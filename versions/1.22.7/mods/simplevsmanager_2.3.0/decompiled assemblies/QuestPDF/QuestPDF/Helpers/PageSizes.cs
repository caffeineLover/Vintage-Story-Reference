using System;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Helpers
{
	// Token: 0x02000078 RID: 120
	[NullableContext(1)]
	[Nullable(0)]
	public static class PageSizes
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x00007F04 File Offset: 0x00006104
		public static PageSize A0 { get; } = new PageSize(2384f, 3370f, Unit.Point);

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00007F0B File Offset: 0x0000610B
		public static PageSize A1 { get; } = new PageSize(1684f, 2384f, Unit.Point);

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00007F12 File Offset: 0x00006112
		public static PageSize A2 { get; } = new PageSize(1191f, 1684f, Unit.Point);

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00007F19 File Offset: 0x00006119
		public static PageSize A3 { get; } = new PageSize(842f, 1191f, Unit.Point);

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060002FC RID: 764 RVA: 0x00007F20 File Offset: 0x00006120
		public static PageSize A4 { get; } = new PageSize(595f, 842f, Unit.Point);

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00007F27 File Offset: 0x00006127
		public static PageSize A5 { get; } = new PageSize(420f, 595f, Unit.Point);

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060002FE RID: 766 RVA: 0x00007F2E File Offset: 0x0000612E
		public static PageSize A6 { get; } = new PageSize(298f, 420f, Unit.Point);

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002FF RID: 767 RVA: 0x00007F35 File Offset: 0x00006135
		public static PageSize A7 { get; } = new PageSize(210f, 298f, Unit.Point);

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000300 RID: 768 RVA: 0x00007F3C File Offset: 0x0000613C
		public static PageSize A8 { get; } = new PageSize(147f, 210f, Unit.Point);

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000301 RID: 769 RVA: 0x00007F43 File Offset: 0x00006143
		public static PageSize A9 { get; } = new PageSize(105f, 147f, Unit.Point);

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000302 RID: 770 RVA: 0x00007F4A File Offset: 0x0000614A
		public static PageSize A10 { get; } = new PageSize(74f, 105f, Unit.Point);

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000303 RID: 771 RVA: 0x00007F51 File Offset: 0x00006151
		public static PageSize B0 { get; } = new PageSize(2835f, 4008f, Unit.Point);

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000304 RID: 772 RVA: 0x00007F58 File Offset: 0x00006158
		public static PageSize B1 { get; } = new PageSize(2004f, 2835f, Unit.Point);

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00007F5F File Offset: 0x0000615F
		public static PageSize B2 { get; } = new PageSize(1417f, 2004f, Unit.Point);

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000306 RID: 774 RVA: 0x00007F66 File Offset: 0x00006166
		public static PageSize B3 { get; } = new PageSize(1001f, 1417f, Unit.Point);

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000307 RID: 775 RVA: 0x00007F6D File Offset: 0x0000616D
		public static PageSize B4 { get; } = new PageSize(709f, 1001f, Unit.Point);

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000308 RID: 776 RVA: 0x00007F74 File Offset: 0x00006174
		public static PageSize B5 { get; } = new PageSize(499f, 709f, Unit.Point);

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000309 RID: 777 RVA: 0x00007F7B File Offset: 0x0000617B
		public static PageSize B6 { get; } = new PageSize(354f, 499f, Unit.Point);

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600030A RID: 778 RVA: 0x00007F82 File Offset: 0x00006182
		public static PageSize B7 { get; } = new PageSize(249f, 354f, Unit.Point);

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600030B RID: 779 RVA: 0x00007F89 File Offset: 0x00006189
		public static PageSize B8 { get; } = new PageSize(176f, 249f, Unit.Point);

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600030C RID: 780 RVA: 0x00007F90 File Offset: 0x00006190
		public static PageSize B9 { get; } = new PageSize(125f, 176f, Unit.Point);

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00007F97 File Offset: 0x00006197
		public static PageSize B10 { get; } = new PageSize(88f, 125f, Unit.Point);

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600030E RID: 782 RVA: 0x00007F9E File Offset: 0x0000619E
		public static PageSize C0 { get; } = new PageSize(2599f, 3677f, Unit.Point);

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00007FA5 File Offset: 0x000061A5
		public static PageSize C1 { get; } = new PageSize(1837f, 2599f, Unit.Point);

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000310 RID: 784 RVA: 0x00007FAC File Offset: 0x000061AC
		public static PageSize C2 { get; } = new PageSize(1298f, 1837f, Unit.Point);

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x06000311 RID: 785 RVA: 0x00007FB3 File Offset: 0x000061B3
		public static PageSize C3 { get; } = new PageSize(918f, 1298f, Unit.Point);

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000312 RID: 786 RVA: 0x00007FBA File Offset: 0x000061BA
		public static PageSize C4 { get; } = new PageSize(649f, 918f, Unit.Point);

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00007FC1 File Offset: 0x000061C1
		public static PageSize C5 { get; } = new PageSize(459f, 649f, Unit.Point);

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000314 RID: 788 RVA: 0x00007FC8 File Offset: 0x000061C8
		public static PageSize C6 { get; } = new PageSize(323f, 459f, Unit.Point);

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00007FCF File Offset: 0x000061CF
		public static PageSize C7 { get; } = new PageSize(230f, 323f, Unit.Point);

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000316 RID: 790 RVA: 0x00007FD6 File Offset: 0x000061D6
		public static PageSize C8 { get; } = new PageSize(162f, 230f, Unit.Point);

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000317 RID: 791 RVA: 0x00007FDD File Offset: 0x000061DD
		public static PageSize C9 { get; } = new PageSize(113f, 162f, Unit.Point);

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000318 RID: 792 RVA: 0x00007FE4 File Offset: 0x000061E4
		public static PageSize C10 { get; } = new PageSize(79f, 113f, Unit.Point);

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00007FEB File Offset: 0x000061EB
		public static PageSize Env10 { get; } = new PageSize(297f, 684f, Unit.Point);

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600031A RID: 794 RVA: 0x00007FF2 File Offset: 0x000061F2
		public static PageSize EnvC4 { get; } = new PageSize(649f, 918f, Unit.Point);

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600031B RID: 795 RVA: 0x00007FF9 File Offset: 0x000061F9
		public static PageSize EnvDL { get; } = new PageSize(312f, 624f, Unit.Point);

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600031C RID: 796 RVA: 0x00008000 File Offset: 0x00006200
		public static PageSize Postcard { get; } = new PageSize(284f, 419f, Unit.Point);

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00008007 File Offset: 0x00006207
		public static PageSize Executive { get; } = new PageSize(522f, 756f, Unit.Point);

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600031E RID: 798 RVA: 0x0000800E File Offset: 0x0000620E
		public static PageSize Letter { get; } = new PageSize(612f, 792f, Unit.Point);

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600031F RID: 799 RVA: 0x00008015 File Offset: 0x00006215
		public static PageSize Legal { get; } = new PageSize(612f, 1008f, Unit.Point);

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000320 RID: 800 RVA: 0x0000801C File Offset: 0x0000621C
		public static PageSize Ledger { get; } = new PageSize(792f, 1224f, Unit.Point);

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000321 RID: 801 RVA: 0x00008023 File Offset: 0x00006223
		public static PageSize Tabloid { get; } = new PageSize(1224f, 792f, Unit.Point);

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0000802A File Offset: 0x0000622A
		public static PageSize ARCH_A { get; } = new PageSize(648f, 864f, Unit.Point);

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000323 RID: 803 RVA: 0x00008031 File Offset: 0x00006231
		public static PageSize ARCH_B { get; } = new PageSize(864f, 1296f, Unit.Point);

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000324 RID: 804 RVA: 0x00008038 File Offset: 0x00006238
		public static PageSize ARCH_C { get; } = new PageSize(1296f, 1728f, Unit.Point);

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0000803F File Offset: 0x0000623F
		public static PageSize ARCH_D { get; } = new PageSize(1728f, 2592f, Unit.Point);

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000326 RID: 806 RVA: 0x00008046 File Offset: 0x00006246
		public static PageSize ARCH_E { get; } = new PageSize(2592f, 3456f, Unit.Point);

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000327 RID: 807 RVA: 0x0000804D File Offset: 0x0000624D
		public static PageSize ARCH_E1 { get; } = new PageSize(2160f, 3024f, Unit.Point);

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000328 RID: 808 RVA: 0x00008054 File Offset: 0x00006254
		public static PageSize ARCH_E2 { get; } = new PageSize(1872f, 2736f, Unit.Point);

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000329 RID: 809 RVA: 0x0000805B File Offset: 0x0000625B
		public static PageSize ARCH_E3 { get; } = new PageSize(1944f, 2808f, Unit.Point);

		// Token: 0x04000236 RID: 566
		public const int PointsPerInch = 72;
	}
}
